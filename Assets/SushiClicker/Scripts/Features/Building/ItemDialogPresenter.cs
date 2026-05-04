using KumaFramework;
using UniRx;
using UnityEngine;

namespace SushiClicker
{
    public class ItemDialogPresenter : PresenterBase
    {
        [SerializeField] private ItemDialogView _view = null;

        private void Awake()
        {
            _view.OnCloseButtonClick
                .Subscribe(_ => Hide())
                .AddTo(this);
        }
    }
}
