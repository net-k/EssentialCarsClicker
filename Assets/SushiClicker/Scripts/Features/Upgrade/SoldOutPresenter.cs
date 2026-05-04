using UnityEngine;
using UniRx;

namespace SushiClicker
{
    /// <summary>
    /// SoldOut（売切れ）状態を管理するPresenter
    /// </summary>
    public class SoldOutPresenter : MonoBehaviour
    {
        [SerializeField] private SoldOutView _view = null;
        [SerializeField] private BC_upgradeManager _upgradeManager = null;

        private void Reset()
        {
            _view = GetComponent<SoldOutView>();
            _upgradeManager = GetComponentInParent<BC_upgradeManager>();
        }

        private void Start()
        {
            // _upgradeManager が未設定の場合は親から探す（シーン配置後の自動解決）
            if (_upgradeManager == null)
                _upgradeManager = GetComponentInParent<BC_upgradeManager>();

            if (_upgradeManager == null)
            {
                Debug.LogWarning($"[SoldOutPresenter] BC_upgradeManager が見つかりません: {name}");
                return;
            }

            // 初期状態: count > 0 ならSoldOutを表示（既に購入済み）
            UpdateSoldOutState(_upgradeManager.count);

            // upgradeManager.count の変更を監視して SoldOut 状態を更新
            _upgradeManager.ObserveEveryValueChanged(x => x.count)
                .Subscribe(count => UpdateSoldOutState(count))
                .AddTo(this);
        }

        private void UpdateSoldOutState(long count)
        {
            // TODO: SoldOut 表示は現在無効化中
            // if (count > 0)
            // {
            //     // 購入済み: SoldOut を表示
            //     _view.Show();
            // }
            // else
            // {
            //     // 未購入: SoldOut を非表示
            //     _view.Hide();
            // }
            _view.Hide();
        }
    }
}
