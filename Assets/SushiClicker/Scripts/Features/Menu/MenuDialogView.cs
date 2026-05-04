using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    public class MenuDialogView : MonoBehaviour
    {
        [SerializeField] private Button _collectionButton = null;
        [SerializeField] private Button _supportButton = null;
        [SerializeField] private Button _closeButton = null;

        public IObservable<Unit> OnCollectionButtonClick
            => _collectionButton.OnClickAsObservable();

        public IObservable<Unit> OnSupportButtonClick
            => _supportButton.OnClickAsObservable();

        public IObservable<Unit> OnCloseButtonClick
            => _closeButton.OnClickAsObservable();
    }
}
