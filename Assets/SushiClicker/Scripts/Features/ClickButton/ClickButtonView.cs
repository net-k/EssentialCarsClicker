using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// クリックボタンの表示を担当するView。
    /// </summary>
    public class ClickButtonView : MonoBehaviour
    {
        [SerializeField] private Image _buttonImage = null;

        /// <summary>
        /// ボタン画像を設定する
        /// </summary>
        public void SetButtonSprite(Sprite sprite)
        {
            if (_buttonImage == null) return;
            _buttonImage.sprite = sprite;
        }
    }
}
