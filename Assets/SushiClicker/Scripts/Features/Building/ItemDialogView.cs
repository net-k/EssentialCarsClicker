using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    public class ItemDialogView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton = null;

        public IObservable<Unit> OnCloseButtonClick
            => _closeButton.OnClickAsObservable();
    }
}
