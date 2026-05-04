using UnityEngine;
using UniRx;

namespace SushiClicker
{
    /// <summary>
    /// アップグレードアイテムのロジックを担当するPresenter
    /// </summary>
    public class UpgradeItemPresenter : MonoBehaviour
    {
        [SerializeField] private UpgradeItemView _view = null;
        [SerializeField] private BC_upgradeManager _upgradeManager = null;

        /// <summary>
        /// 一度でも購入済みかどうか
        /// </summary>
        public bool IsSoldOut => _upgradeManager != null && _upgradeManager.count > 0;

        /// <summary>
        /// リスト内でソート対象となる transform（BC_upgradeManager が親の場合はそちらを使う）
        /// </summary>
        public Transform ListItemTransform =>
            _upgradeManager != null ? _upgradeManager.transform : transform;

        private void Reset()
        {
            _view = GetComponent<UpgradeItemView>();
            _upgradeManager = GetComponentInParent<BC_upgradeManager>();
        }

        private void Start()
        {
            string costText = BC_currencyConverter.Instance.GetCurrencyIntoString(
                _upgradeManager.cost, false, false);

            _view.SetItemName(_upgradeManager.itemName);
            double initialPower = _upgradeManager.count > 0
                ? _upgradeManager.clickPower * _upgradeManager.count
                : _upgradeManager.clickPower;
            _view.SetPower(initialPower);
            _view.SetCost(costText);

            // cost の変化を監視して表示を更新
            _upgradeManager.ObserveEveryValueChanged(x => x.cost)
                .Subscribe(cost =>
                {
                    string updatedCostText = BC_currencyConverter.Instance.GetCurrencyIntoString(
                        cost, false, false);
                    _view.SetCost(updatedCostText);
                })
                .AddTo(this);

            // count の変化を監視して累計パワー表示を更新（未購入時は1回分のパワーをプレビュー）
            _upgradeManager.ObserveEveryValueChanged(x => x.count)
                .Subscribe(count =>
                {
                    double displayPower = count > 0 ? _upgradeManager.clickPower * count : _upgradeManager.clickPower;
                    _view.SetPower(displayPower);
                })
                .AddTo(this);
        }
    }
}
