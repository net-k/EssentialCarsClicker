using CoinPusher.Core;
using Quiz.Framework.Ad.AdMob;
using SushiCatcher;
using TohoReversi.Shop;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlotCoinPusher.OutOfCoinsPopup
{
    public class OutOfCoinsPopupPresenter : MonoBehaviour
    {
        [SerializeField]
        private OutOfCoinsPopupView _view = null;
        
        [SerializeField]
        private AdMobRewardVideo _adMobRewardVideo;
        
        [SerializeField]
        private CoinManager _coinManager;
        
        [SerializeField]
        private int _rewardAmount = 50;
        [SerializeField]
        private CommonDialogPresenter _commonDialogPresenter = null;

        public System.Action OnRewarded;
 
        void Awake ()
        {
            _adMobRewardVideo.OnAdRewarded += HandleAdRewarded;

            _view.OnRewardButtonClick
                .Subscribe(_ => OnReward())
                .AddTo(this);
            
            _view.OnBackToTitleButtonClick
                .Subscribe(_ => OnBackToTitle())
                .AddTo(this);

            string key = "key_OutOfCoinsDialog_Info";
            string text = I2.Loc.LocalizationManager.GetTranslation( key ); 
            if ( !string.IsNullOrEmpty( text ) )
            {
                _view.DetailText.text = text.Replace( "{0}", _rewardAmount.ToString() );
                return;
            }
            _view.DetailText.text = $"Watch an ad to earn {_rewardAmount} coins!";
        }

        private void OnReward()
        {
            if (!_adMobRewardVideo.Show() )
            {
                string captionKey = "key_ErrorCaption";
                string messageKey = "key_RewardAdFailedToLoad";
                _commonDialogPresenter.Show(captionKey, messageKey);
                // DialogPresenter.Instance.ShowDialog("動画の読み込みに失敗しました", DialogPresenter.DialogType.OK);
            }
            else
            {
            // todo    SoundManager.Instance.Suspend();
            // todo    dialog.Close();
                gameObject.SetActive(false);
            }             
        }
        
        private void OnBackToTitle()
        {
            SceneManager.LoadScene("TitleScene");
        }
        
        private void OnAdFailedToLoad()
        {
            string captionKey = "key_ErrorCaption";
            string messageKey = "key_RewardAdFailedToLoad";
            _commonDialogPresenter.Show(captionKey, messageKey);
			
        }

        private void OnAdRewarded()
        {
            // kari
            int gachaShopRewardItemId = 0;
            
            
            // コインを増やす
            _coinManager.addCoin(RewardAdsConstants.RewardVideoAdditionalCoin, true);
        }
        
        private void OnAdClose()
        {
           //  SoundManager.Instance.Resume();
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        private void HandleAdRewarded()
        {
           _coinManager.addCoin(_rewardAmount, false);
           OnRewarded?.Invoke();
        }

      
    }
}
