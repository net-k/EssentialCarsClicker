using System;
using UnityEngine;

namespace stickin
{
    public class AdmobAdWrapper : AdWrapper
    {
        [SerializeField] private AdmobAdsConfig _config;

        private AdmobBannerWrapper _bannerWrapper;
        private AdmobInterstitialWrapper _interstitialWrapper;
        private AdmobRewardWrapper _rewardWrapper;
        
        private bool _isInterstitialAvailable;

        public override void Init()
        {
            AdmobConfiguration.Init(_config.IsTest, _config.TestDevices);

            _bannerWrapper = new AdmobBannerWrapper(
                // _config.Ids.BannerId, 
                GetBannerUnitId(),
                _config.BannerPosition == BannerPosition.Top,
                _config.ShowBannerOnStart, 
                OnInitComplete);

            _interstitialWrapper = new AdmobInterstitialWrapper(/*_config.Ids.InterstitialId*/ GetInterstitialUnitId());
            _rewardWrapper = new AdmobRewardWrapper(/*_config.Ids.RewardId*/ GetRewardUnitId());
            
            InterstitialAvailableDelay(_config.StartDelaySeconds);
        }

        string GetBannerUnitId()
        {
            if (_config.IsTest)
            {
#if UNITY_ANDROID
            return "ca-app-pub-3940256099942544/6300978111"; // test
#elif UNITY_IPHONE
            return "ca-app-pub-3940256099942544/2934735716"; // test
#endif
            }
            
            return _config.Ids.BannerId;
        }

        string GetInterstitialUnitId()
        {
            if (_config.IsTest)
            {
#if UNITY_ANDROID
                return "ca-app-pub-3940256099942544/1033173712"; // test interstitial
#elif UNITY_IPHONE
                return "ca-app-pub-3940256099942544/4411468910"; // test
#endif 
            }

            return _config.Ids.InterstitialId;
        }

        string GetRewardUnitId()
        {
            if (_config.IsTest)
            {
#if UNITY_ANDROID
                return "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
                return "ca-app-pub-3940256099942544/1712485313";
#endif 
            }

            return _config.Ids.RewardId;
        }

        private void OnInitComplete()
        {
            
        }

        public override bool IsInterstitialAvailable()
        {
            return _isInterstitialAvailable && _interstitialWrapper.IsLoad();
        }

        public override void ShowInterstitial()
        {
            _interstitialWrapper.Show(null, null);
            InterstitialAvailableDelay(_config.DelayBetweenAdsSeconds);
        }

        public override bool IsRewardAvailable()
        {
            return _rewardWrapper.IsLoad();
        }

        public override void ShowReward(Action callbackComplete, Action callbackFail)
        {
            _rewardWrapper.Show(callbackComplete, callbackFail);
            
            if (_config.RewardAdsIsRestartDelay)
                InterstitialAvailableDelay(_config.DelayBetweenAdsSeconds);
        }

        public override bool IsBannerAvailable()
        {
            return _bannerWrapper.IsLoad();
        }

        public override bool IsShowBanner()
        {
#if ST_ADS
            return _bannerWrapper.IsShow();
#endif
            return false;
        }
        
        public override void ShowBanner()
        {
            _bannerWrapper.Show(null, null);
        }

        public override void HideBanner()
        {
            _bannerWrapper.Hide();
        }

        public override float GetBannerHeight()
        {
            return _bannerWrapper.GetBannerHeight();
        }

        public override void CheckAvailableAd()
        {
            if (!_bannerWrapper.IsLoad())
                _bannerWrapper.Request();
        
            if (!_interstitialWrapper.IsLoad())
                _interstitialWrapper.Request();
        
            if (!_rewardWrapper.IsLoad())
                _rewardWrapper.Request();
        }
        
        private void InterstitialAvailableDelay(float delay)
        {
            _isInterstitialAvailable = false;
            Debug.Log($"InterstitialAvailableDelay {_isInterstitialAvailable}     with delay = {delay}");

            if (delay > 0)
            {
                Updater.Instance.RemoveDelayedCall(InterstitialAvailable);
                Updater.Instance.AddDelayedCall(delay, InterstitialAvailable);
            }
            else
            {
                InterstitialAvailable();
            }
        }
        
        private void InterstitialAvailable()
        {
            _isInterstitialAvailable = true;
        }
    }
}