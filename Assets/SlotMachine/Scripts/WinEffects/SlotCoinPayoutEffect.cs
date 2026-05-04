using System.Collections;
using CoinPusher;
using UnityEngine;

namespace SlotMachine.Scripts
{
    public class SlotCoinPayoutEffect : MonoBehaviour, ISlotWinEffect
    {
        [SerializeField] private CoinSpawner coinSpawner;

        private void Awake()
        {
            if (coinSpawner == null)
            {
                coinSpawner = FindObjectOfType<CoinSpawner>();
            }
        }

        public bool IsApplicable(SlotValue symbol, int score)
        {
            // Wall(box)とコインタワー(coin)以外の絵柄で、スコアが0より大きい場合に実行
            return symbol != SlotValue.wall && symbol != SlotValue.coin && score > 0;
        }

        public void Execute(SlotValue symbol, int score)
        {
            Payout(score);
        }

        public void Payout(int score)
        {
            StartCoroutine(SpawnCoinsRoutine(score));
        }

        private IEnumerator SpawnCoinsRoutine(int amount)
        {
            // 最大枚数制限（必要に応じて調整）
            // int spawnAmount = Mathf.Min(amount, 50);

            // ResultCheckerの値をそのまま使うと多すぎる場合があるので、調整します。
            // ここでは仮に 1/20 とします (100点 -> 5枚, 700点 -> 35枚)
            int coinsToSpawn = amount / 20;
            if (coinsToSpawn < 1) coinsToSpawn = 1;

            for (int i = 0; i < coinsToSpawn; i++)
            {
                if (coinSpawner != null)
                {
                    coinSpawner.coinAttackSpawner(1);
                }
                yield return new WaitForSeconds(0.1f); // 0.1秒間隔で落とす
            }
        }
    }
}
