using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SlotCoinPusher.OutOfCoinsPopup
{
    public class OutOfCoinsPopupView : MonoBehaviour
    {
        [SerializeField]
        private Button _rewardButton;

        [SerializeField]
        private Button _backToTitleButton;

        public IObservable<Unit> OnRewardButtonClick
        {
            get
            {
                if (_rewardButton == null)
                {
                    Debug.LogError("RewardButton is not assigned in OutOfCoinsPopupView", this);
                    return Observable.Empty<Unit>();
                }
                return _rewardButton.OnClickAsObservable();
            }
        }

        public IObservable<Unit> OnBackToTitleButtonClick
        {
            get
            {
                if (_backToTitleButton == null)
                {
                    Debug.LogError("BackToTitleButton is not assigned in OutOfCoinsPopupView", this);
                    return Observable.Empty<Unit>();
                }
                return _backToTitleButton.OnClickAsObservable();
            }
        }


        [SerializeField]
        Text _detailText = null;

        public Text DetailText => _detailText;
    }
}
