using CoinPusher;
using UnityEngine;
using System.Collections;

namespace SlotMachine.Scripts
{
    public class SlotPrizeEffect : MonoBehaviour, ISlotWinEffect
    {
        [SerializeField] private PrizeSpawner prizeSpawner;

        private void Awake()
        {
            if (prizeSpawner == null)
            {
                prizeSpawner = FindObjectOfType<PrizeSpawner>();
            }
        }

        public bool IsApplicable(SlotValue symbol, int score)
        {
            return symbol == SlotValue.prize; 
        }

        public void Execute(SlotValue symbol, int score)
        {
            if (prizeSpawner != null)
            {
                SpawnPrizeBasedOnResult(symbol);
            }
        }

        private void SpawnPrizeBasedOnResult(SlotValue slotValue)
        {
            int prizeIndex = -1;
            switch (slotValue)
            {
                case SlotValue.wall:
                    // このエフェクトはboxでは呼ばれないが、念のため
                    prizeIndex = 0;
                    break;
                case SlotValue.coin:
                    // このエフェクトはcoinでは呼ばれないが、念のため
                    prizeIndex = 1;
                    break;
                case SlotValue.dia:
                    prizeIndex = 2;
                    break;
                case SlotValue.key:
                    prizeIndex = 3;
                    break;
                case SlotValue.prize:
                    prizeIndex = 4;
                    break;
                case SlotValue.shield:
                    prizeIndex = 5;
                    break;
                case SlotValue.seven:
                    prizeIndex = 6;
                    break;
            }

            if (prizeIndex != -1)
            {
                Debug.Log($"賞品をスポーンします。インデックス: {prizeIndex} (絵柄: {slotValue})");
                prizeSpawner.SpawnPrize(prizeSpawner.transform.position, prizeIndex);
            }
            else
            {
                Debug.LogWarning($"対応する賞品が見つかりませんでした。絵柄: {slotValue}");
            }
        }
    }
}
