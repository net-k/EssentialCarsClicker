using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace CoinPusher
{
    public class PrizeSpawner : MonoBehaviour
    {
        // コインのプレイフィールド
        private Transform coinPlayField;

        [Header("Spawn Settings")]
        // クリック/タッチ地点ではなく、スポーンエリア内でランダムにスポーンさせるかどうか。
        public bool spawnRandomLocations = false;

        // 垂直ではなく水平面にスポーンさせるか
        public bool spawnHorizontal = false;

        // スポーン基準位置
        [SerializeField]
        private Transform _spawnBasePosition;
        
        // Addressableのアドレスリスト
        // Assets/Sushi_set_D/Prefabs/Individual/ 以下のPrefabを対象とします
        private List<string> prizeAddresses = new List<string>()
        {
            "Assets/Sushi_set_D/Prefabs/Individual/Eel.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Egg.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Tuna.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Squid.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Salmon.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Shrimp.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Octopus.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Scallop.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Sea_bream.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Inarizushi.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Salmon_roe.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Sea_urchin.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Sushi_roll.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Greater_amberjack.prefab"
        };

        void Awake()
        {
            // プレイフィールドを見つける
            GameObject playFieldObj = GameObject.FindWithTag("CoinsPlayField");
            if (playFieldObj != null)
            {
                coinPlayField = playFieldObj.transform;
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            // デバッグ用: Pキーを押すとプライズをスポーン
            if (Input.GetKeyDown(KeyCode.P))
            {
                SpawnPrize(transform.position);
            }
        }
#endif

        [ContextMenu("Spawn Random Prize")]
        public void DebugSpawnPrize()
        {
            SpawnPrize(transform.position);
        }

        public void SpawnPrize(Vector3 position, int specificIndex = -1)
        {
            if (prizeAddresses.Count == 0) return;

            string address;
            if (specificIndex >= 0 && specificIndex < prizeAddresses.Count)
            {
                address = prizeAddresses[specificIndex];
            }
            else
            {
                // ランダムにアドレスを選択
                address = prizeAddresses[Random.Range(0, prizeAddresses.Count)];
            }

            // スポーン位置の計算
            Vector3 spawnLocation;

            // _spawnBasePosition が設定されている場合はそれを基準にする
            if (_spawnBasePosition != null)
            {
                if (spawnRandomLocations)
                {
                    // _spawnBasePosition のX, Yを使い、Zはランダム（範囲はtransform.localScale.zに基づく）
                    // あるいは _spawnBasePosition の周辺でランダムにするなど、仕様に合わせて調整
                    // ここでは元のロジックを踏襲しつつ、基準点を _spawnBasePosition に変更します
                    
                    // transform.localScale.z を範囲として使用
                    float rangeZ = transform.localScale.z / 2;
                    float randomZ = Random.Range(_spawnBasePosition.position.z - rangeZ, _spawnBasePosition.position.z + rangeZ);
                    
                    spawnLocation = new Vector3(_spawnBasePosition.position.x, _spawnBasePosition.position.y, randomZ);
                }
                else
                {
                    // 指定されたpositionを使うか、_spawnBasePositionを使うか
                    // 引数のpositionが外部から渡されたクリック位置などの場合、それを優先すべきかもしれないが、
                    // 要望は「_spawnBasePosition から落下するように」なので、基本は _spawnBasePosition を使う
                    spawnLocation = _spawnBasePosition.position;
                }
            }
            else
            {
                // 従来の実装（フォールバック）
                if (spawnRandomLocations)
                {
                    spawnLocation = new Vector3(transform.position.x,
                                                transform.position.y,
                                                Random.Range(transform.position.z - (transform.localScale.z / 2), transform.position.z + (transform.localScale.z / 2)));
                }
                else
                {
                    // クリック/タップ位置でスポーン
                    spawnLocation = new Vector3(position.x, transform.position.y, position.z);
                }
            }

            // Addressablesを使ってインスタンス化
            Addressables.InstantiateAsync(address, spawnLocation, Quaternion.identity).Completed += OnPrizeSpawned;
        }

        private void OnPrizeSpawned(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prize = obj.Result;
                
                // 回転の調整
                if (spawnHorizontal)
                {
                    prize.transform.rotation = Quaternion.Euler(180, 0, 0);
                }
            }
            else
            {
                Debug.LogError($"Failed to spawn prize.");
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.5F); // 青色で表示
            Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
            
            if (_spawnBasePosition != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_spawnBasePosition.position, 0.5f);
            }
        }
    }
}
