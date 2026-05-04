using UnityEngine;
using CoinPusher.Core;

namespace CoinPusher.UI
{
    /// <summary>
    /// CoinManagerのデータを監視し、CoinHopperViewを更新するPresenter
    /// </summary>
    public class CoinHopperPresenter : MonoBehaviour
    {
        [SerializeField] private CoinManager coinManager;
        [SerializeField] private CoinHopperView view;

        private void Start()
        {
            if (coinManager == null)
            {
                coinManager = FindObjectOfType<CoinManager>();
            }
            
            // 初期表示更新
            UpdateView();
        }

        private void Update()
        {
            // 毎フレーム更新（必要に応じてイベント駆動に変更可能）
            UpdateView();
        }

        private void UpdateView()
        {
            if (coinManager != null && view != null)
            {
                view.Render(coinManager.currentCoinCount, coinManager.maxCoinDrop);
            }
        }
    }
}
