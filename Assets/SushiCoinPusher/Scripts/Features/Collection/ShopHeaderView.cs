using UnityEngine;
using UnityEngine.UI;

namespace FruitShop.Presentation
{
    public class ShopHeaderView : MonoBehaviour
    {
        [SerializeField] private Text _levelText = null;
        [SerializeField] private Text _coinText = null;

        public Text LevelText => _levelText;

        public Text CoinText => _coinText;

        public void SetLevelText(int level)
        {
            _levelText.text = $"LV{level.ToString()}";
        }

        public void SetCoinText(int coin)
        {
            _coinText.text = coin.ToString();
        }

        public void SetLevelMaxText()
        {
            _levelText.text = $"LV MAX";
        }
    }
}
