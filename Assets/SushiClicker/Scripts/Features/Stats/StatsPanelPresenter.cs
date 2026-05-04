using KumaFramework;
using UniRx;
using UnityEngine;

namespace SushiClicker
{
    public class StatsPanelPresenter : PresenterBase
    {
        [SerializeField] private StatsPanelView _view = null;

        // BC_Click はレガシーシングルトンではないため FindObjectOfType で遅延取得する
        private BC_Click _click;

        private void Awake()
        {
            _view.OnCloseButtonClick
                .Subscribe(_ => Hide())
                .AddTo(this);

            _view.OnPrestigeButtonClick
                .Subscribe(_ => OnPrestigeButtonClicked())
                .AddTo(this);
        }

        private void OnEnable()
        {
            // 表示のたびに最新データで更新する
            UpdateView();
        }

        private void UpdateView()
        {
            if (_click == null)
            {
                _click = FindObjectOfType<BC_Click>();
                if (_click == null)
                {
                    Debug.LogWarning("StatsPanelPresenter: BC_Click が見つかりません");
                    return;
                }
            }

            var totalCount = (_click.bananaTrillionCount * 1e12) + _click.BananaCount;
            _view.SetTotalSushi(totalCount);
            _view.SetPrestigeLevel(_click.PrestigeLevel);
            _view.SetPrestigeButtonInteractable(_click.bananaTrillionCount >= 1);
        }

        private void OnPrestigeButtonClicked()
        {
            _click?.BuyPrestige();
            UpdateView();
        }
    }
}
