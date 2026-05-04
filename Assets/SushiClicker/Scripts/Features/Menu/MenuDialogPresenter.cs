using KumaFramework;
using SushiCatcher;
using UniRx;
using UnityEngine;

namespace SushiClicker
{
    public class MenuDialogPresenter : PresenterBase
    {
        [SerializeField] private MenuDialogView _view = null;

        private void Awake()
        {
            _view.OnCollectionButtonClick
                .Subscribe(_ => SushiCatcherSceneManager.Load(SushiCaterScene.Collection))
                .AddTo(this);

            _view.OnSupportButtonClick
                .Subscribe(_ => SushiCatcherSceneManager.Load(SushiCaterScene.Support))
                .AddTo(this);

            _view.OnCloseButtonClick
                .Subscribe(_ => Hide())
                .AddTo(this);

        }
    }
}
