using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.Scripts
{
    public class SlotCoinTowerEffect : MonoBehaviour, ISlotWinEffect
    {
        [SerializeField] private CoinSpawner coinSpawner;
        
        [Header("タワー設定")]
        [SerializeField] private int towerHeight = 20; // タワーの高さ (20枚重ね)
        [SerializeField] private int coinsPerLevel = 6; // 1層あたりのコイン数
        [SerializeField] private float towerRadius = 0.5f; // タワーの半径
        [SerializeField] private float coinHeight = 0.15f; // コイン1枚の高さ（0.1f -> 0.15f に変更して重なりを軽減）
        
        [Header("スペース確保設定")]
        [Tooltip("周囲のコインを削除する範囲の半径")]
        [SerializeField] private float clearRadius = 3.0f; 
        [Tooltip("スペース確保後、タワー生成までの待機時間（秒）")]
        [SerializeField] private float waitTimeAfterClear = 0.01f;

        [Header("物理演算設定")]
        [Tooltip("タワー生成後、物理演算を有効にするまでの待機時間（秒）")]
        [SerializeField] private float physicsEnableDelay = 2.0f;

        [Tooltip("タワーを生成する基準となるオブジェクト（例: フィールド上の空のGameObject）。未設定の場合はCoinSpawnerの位置を使用します。")]
        [SerializeField] private Transform spawnOrigin;

        [Tooltip("基準位置からのオフセット")]
        [SerializeField] private Vector3 spawnOffset = new Vector3(0, -1.0f, 0); 

        private void Awake()
        {
            if (coinSpawner == null)
            {
                coinSpawner = FindObjectOfType<CoinSpawner>();
            }
        }

        public bool IsApplicable(SlotValue symbol, int score)
        {
            return symbol == SlotValue.coin;
        }

        public void Execute(SlotValue symbol, int score)
        {
            if (coinSpawner != null)
            {
                StartCoroutine(BuildCoinTower());
            }
            else
            {
                Debug.LogError("SlotCoinTowerEffect: CoinSpawner is null!");
            }
        }

        private IEnumerator BuildCoinTower()
        {
            Debug.Log("円状のコインタワーを生成します！");

            // 基準位置の決定
            Vector3 basePosition = (spawnOrigin != null) ? spawnOrigin.position : coinSpawner.transform.position;
            Vector3 towerCenter = basePosition + spawnOffset;

            // タワー生成位置の周囲のコインを削除してスペースを確保
            ClearSpaceForTower(towerCenter);

            // コインが消えた後の「間」を作る
            yield return new WaitForSeconds(waitTimeAfterClear);

            List<GameObject> towerCoins = new List<GameObject>();

            // タワーの高さの分だけループ
            for (int i = 0; i < towerHeight; i++)
            {
                // 1層あたりのコインの数だけループ
                for (int j = 0; j < coinsPerLevel; j++)
                {
                    // 円周上の位置を計算
                    float angle = j * (360f / coinsPerLevel);
                    float x = Mathf.Cos(angle * Mathf.Deg2Rad) * towerRadius;
                    float z = Mathf.Sin(angle * Mathf.Deg2Rad) * towerRadius;
                    
                    // Y座標を計算 (タワーとして積み上げる)
                    float y = i * coinHeight;
                    
                    // 最終的なコインの生成位置
                    Vector3 position = towerCenter + new Vector3(x, y, z);
                    
                    // コインを生成
                    GameObject coin = coinSpawner.SpawnCoinAt(position);
                    
                    if (coin != null)
                    {
                        Rigidbody rb = coin.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            // 生成直後は物理演算を無効にして固定する
                            rb.isKinematic = true;
                        }
                        towerCoins.Add(coin);
                    }
                }
            }
            
            Debug.Log("コインタワー生成完了（物理固定中）");

            // 指定時間待機してから物理演算を有効にする
            yield return new WaitForSeconds(physicsEnableDelay);

            foreach (var coin in towerCoins)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        // 物理演算を有効にした直後にスリープさせ、衝突があるまで静止させる
                        rb.Sleep();
                    }
                }
            }

            Debug.Log("コインタワー物理演算有効化");
        }

        /// <summary>
        /// 指定した位置の周囲にあるコインを削除して、スペースを確保します。
        /// </summary>
        /// <param name="center">中心位置</param>
        private void ClearSpaceForTower(Vector3 center)
        {
            // 指定範囲内のコライダーを取得
            Collider[] colliders = Physics.OverlapSphere(center, clearRadius);
            
            foreach (Collider hit in colliders)
            {
                // 床やプッシャーなど、消してはいけないものは除外
                if (hit.CompareTag("Floor") || hit.CompareTag("PushBar") || hit.CompareTag("Wall") || hit.CompareTag("Bumpers")) continue;

                // CoinEffectコンポーネントを持っているオブジェクト（コイン）を削除
                CoinEffect coin = hit.GetComponent<CoinEffect>();
                if (coin != null)
                {
                    coin.removeCoin();
                }
                // CoinEffectはないが、Coinタグがついている場合も削除
                else if (hit.CompareTag("Coin"))
                {
                    Destroy(hit.gameObject);
                }
            }
        }
    }
}
