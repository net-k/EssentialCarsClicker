using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BananaClicker
{
    /// <summary>
    /// アイテム（建物）の表示を担当するView
    /// </summary>
    public class ItemView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _itemInfoText;

        [SerializeField]
        private TMP_Text _itemCountText;

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private Image _backgroundImage;

        [SerializeField]
        private Button _purchaseButton;

        public TMP_Text ItemInfoText => _itemInfoText;
        public TMP_Text ItemCountText => _itemCountText;
        public Slider Slider => _slider;
        public Image BackgroundImage => _backgroundImage;
        public Button PurchaseButton => _purchaseButton;

        private void Awake()
        {
            if (_backgroundImage == null)
            {
                _backgroundImage = GetComponent<Image>();
            }

            if (_slider == null)
            {
                _slider = GetComponentInChildren<Slider>();
                if (_slider == null)
                {
                    Debug.LogError("Slider が見つかりません。Inspector で設定してください。", this);
                }
            }
        }
    }
}
