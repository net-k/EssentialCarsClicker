using UnityEngine;
using UniRx;

namespace SushiClicker
{
    /// <summary>
    /// 建物アイテムのロジックを担当するPresenter
    /// </summary>
    public class BuildingItemPresenter : MonoBehaviour
    {
        [SerializeField] private BuildingItemView _view = null;
        [SerializeField] private BC_ItemManager _itemManager = null;

        private void Reset()
        {
            _view = GetComponent<BuildingItemView>();
            _itemManager = GetComponentInParent<BC_ItemManager>();
        }

        private void Start()
        {
            string costText = BC_currencyConverter.Instance.GetCurrencyIntoString(
                _itemManager.cost, false, false);

            _view.SetItemName(_itemManager.itemName);
            _view.SetTickValue(_itemManager.tickValue);
            _view.SetCost(costText);
            _view.SetItemCount(_itemManager.count);

            // itemManager.count の変更を監視して View を更新する
            _itemManager.ObserveEveryValueChanged(x => x.count)
                .Subscribe(count => _view.SetItemCount(count))
                .AddTo(this);

            // itemManager.cost の変更を監視して View を更新する
            _itemManager.ObserveEveryValueChanged(x => x.cost)
                .Subscribe(cost => _view.SetCost(
                    BC_currencyConverter.Instance.GetCurrencyIntoString(cost, false, false)))
                .AddTo(this);
        }
    }
}
