
using UnityEngine;

namespace CoinPusher.DebugFeatures
{
    /// <summary>
    /// デバッグ機能を提供します。
    /// このコンポーネントをシーン内のGameObjectに追加してください。
    /// </summary>
    public class DebugManager : MonoBehaviour
    {
        private CoinSpawner _coinSpawner;
        private bool _showDebugMenu = false;

        private void Start()
        {
            // シーン内のCoinSpawnerを検索してキャッシュする
            _coinSpawner = FindObjectOfType<CoinSpawner>();
            if (_coinSpawner == null)
            {
                UnityEngine.Debug.LogError("CoinSpawnerが見つかりません。");
            }
        }

        private void Update()
        {
            // Dキーでコインを50枚投下
            if (Input.GetKeyDown(KeyCode.D))
            {
                TriggerCoinAttack();
            }

            // F1キーでデバッグメニューの表示/非表示を切り替え
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _showDebugMenu = !_showDebugMenu;
            }
        }

        private void OnGUI()
        {
            if (!_showDebugMenu)
            {
                return;
            }

            // デバッグメニューの描画
            GUILayout.BeginArea(new Rect(10, 10, 300, 300), "デバッグメニュー", GUI.skin.window);

            if (GUILayout.Button("コインを50枚投下 (D)"))
            {
                TriggerCoinAttack();
            }

            GUILayout.EndArea();
        }

        /// <summary>
        /// CoinSpawnerのcoinAttackSpawnerを呼び出します。
        /// </summary>
        private void TriggerCoinAttack()
        {
            if (_coinSpawner != null)
            {
                _coinSpawner.coinAttackSpawner(50);
                UnityEngine.Debug.Log("デバッグ機能: コインを50枚投下しました。");
            }
            else
            {
                UnityEngine.Debug.LogError("CoinSpawnerがセットされていません。");
            }
        }
    }
}
