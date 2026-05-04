using System;
using KumaFramework;
using UniRx;
using UnityEngine;

namespace TohoReversi.Shop
{
    class CommonDialogPresenter : PresenterBase
    {
        [SerializeField]
        CommonDialogView _commonDialogView;

        // Subject と observable を作成
        private Subject<Unit> _onOkButtonSubject = new Subject<Unit>();
        public IObservable<Unit> OnOkButtonObservable => _onOkButtonSubject;
        
        private void Awake()
        {
            _commonDialogView.OkButton.onClick.AddListener( 
                () =>
                {
                    _onOkButtonSubject.OnNext(Unit.Default);
                    Hide();
                }
            );
        }
        
        public void Show( string captionTextKey, string messageTextKey )
        {
            base.Show();
            _commonDialogView.SetCaptionTextKey( captionTextKey );
            _commonDialogView.SetMessageTextKey( messageTextKey );
        }

        // Show with raw message string (useful for parameterized messages)
        public void ShowWithMessage(string captionTextKey, string message)
        {
            base.Show();
            _commonDialogView.SetCaptionTextKey(captionTextKey);
            _commonDialogView.SetMessageText(message);
        }


    }
}