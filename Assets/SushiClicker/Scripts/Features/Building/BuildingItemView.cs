using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// 建物アイテムの表示を担当するView
    /// </summary>
    public class BuildingItemView : MonoBehaviour
    {
        [SerializeField] private Text _itemNameText = null;
        [SerializeField] private Text _tickValueText = null;
        [SerializeField] private Text _costText = null;
        [SerializeField] private Text _itemCountText = null;
        private void Reset()
        {
            foreach (var text in GetComponentsInChildren<Text>(true))
            {
                switch (text.gameObject.name)
                {
                    case "ItemNameText":  _itemNameText  = text; break;
                    case "TickValueText": _tickValueText = text; break;
                    case "CostText":       _costText       = text; break;
                    case "ItemCountText":  _itemCountText  = text; break;
                }
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
        /// 毎秒生産量を設定する
        /// </summary>
        public void SetTickValue(double tickValue)
        {
            string localizedProductionLabel = I2.Loc.LocalizationManager.GetTranslation("key_Production");
            _tickValueText.text = $"{localizedProductionLabel} {tickValue}/s";
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
        /// 所持数を設定する
        /// </summary>
        public void SetItemCount(long count)
        {
            string localizedCostLabel = I2.Loc.LocalizationManager.GetTranslation("key_Count");
            _itemCountText.text = $"{localizedCostLabel} {count}";
        }
    }
}
