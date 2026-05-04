using UnityEngine;
using UnityEngine.UI;

namespace CoinPusher.UI
{
    /// <summary>
    /// コインホッパーの表示を担当するView
    /// </summary>
    public class CoinHopperView : MonoBehaviour
    {
        [Tooltip("コイン数を表示するテキスト")]
        [SerializeField] private Text coinCountText;

        /// <summary>
        /// コイン数の表示を更新します
        /// </summary>
        /// <param name="current">現在のコイン数</param>
        /// <param name="max">最大コイン数</param>
        public void Render(int current, int max)
        {
            if (coinCountText != null)
            {
                // coinCountText.text = $"{current} / {max}";
                coinCountText.text = $"{current}";
            }
        }
    }
}
