using UniRx;
using UnityEngine;

namespace SushiClicker
{
    public class FooterPresenter : MonoBehaviour
    {
        [SerializeField] private FooterView _view = null;
        [SerializeField] private ItemDialogPresenter _itemDialogPresenter = null;
        [SerializeField] private UpgradeDialogPresenter _upgradeDialogPresenter = null;

        private void Awake()
        {
            _view.OnBuildingButtonClick
                .Subscribe(_ => _itemDialogPresenter.Show())
                .AddTo(this);

            _view.OnUpgradeButtonClick
                .Subscribe(_ => _upgradeDialogPresenter.Show())
                .AddTo(this);
        }
    }
}
