using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// SoldOut（売切れ）状態の表示を担当するView
    /// </summary>
    public class SoldOutView : MonoBehaviour
    {
        [SerializeField] private Text _soldOutText = null;

        private void Reset()
        {
            if (_soldOutText == null)
            {
                _soldOutText = GetComponent<Text>();
            }
        }

        /// <summary>
        /// SoldOut 状態を表示する
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// SoldOut 状態を非表示にする
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
