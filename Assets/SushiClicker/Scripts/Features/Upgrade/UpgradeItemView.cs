using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// アップグレードアイテムの表示を担当するView
    /// </summary>
    public class UpgradeItemView : MonoBehaviour
    {
        [SerializeField] private Text _itemNameText = null;
        [SerializeField] private Text _powerText = null;
        [SerializeField] private Text _costText = null;
        [SerializeField] private SoldOutView _soldOutView = null;

        private void Reset()
        {
            foreach (var text in GetComponentsInChildren<Text>(true))
            {
                switch (text.gameObject.name)
                {
                    case "ItemNameText": _itemNameText = text; break;
                    case "PowerText":    _powerText    = text; break;
                    case "CostText":     _costText     = text; break;
                }
            }

            if (_soldOutView == null)
            {
                _soldOutView = GetComponentInChildren<SoldOutView>(true);
            }
        }

        /// <summary>
        /// アイテム名を設定する
        /// </summary>
        public void SetItemName(string itemName)
        {
            // 翻訳する
            string localizedItemName = I2.Loc.LocalizationManager.GetTranslation(itemName);
            _itemNameText.text = localizedItemName;
        }

        /// <summary>
        /// クリック倍率を設定する
        /// </summary>
        public void SetPower(double power)
        {
            string localizedPowerLabel = I2.Loc.LocalizationManager.GetTranslation("key_Power");
            _powerText.text = $"{localizedPowerLabel} +{power}";
        }

        /// <summary>
        /// コストを設定する
        /// </summary>
        public void SetCost(string cost)
        {
            string localizedCostLabel = I2.Loc.LocalizationManager.GetTranslation("key_Cost");
            _costText.text = $"{localizedCostLabel} {cost}";
        }

        /// <summary>
        /// SoldOutView を取得する
        /// </summary>
        public SoldOutView GetSoldOutView()
        {
            return _soldOutView;
        }
    }
}
