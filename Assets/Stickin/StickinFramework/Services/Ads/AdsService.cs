using System;
using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class AdsService : BaseService
    {
        [SerializeField] private List<AdWrapper> _wrappers;

        public event Action OnRefreshBanner;
        public bool BannerIsTop => false; // @TODO Need code

        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            InjectService.Bind<AdsService>(this);

            base.Init(appData, callbackComplete);

            foreach (var wrapper in _wrappers)
                wrapper.Init();

            CheckInternetConnection();

            InitComplete(true);
        }

        public bool IsInterstitialAvailable()
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsInterstitialAvailable())
                    return true;
            }

            return false;
        }

        public void TryShowInterstitial()
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsInterstitialAvailable())
                {
                    wrapper.ShowInterstitial();
                    return;
                }
            }
        }

        public bool IsRewardAvailable()
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsRewardAvailable())
                    return true;
            }

            return false;
        }

        public void ShowReward(Action callbackComplete, Action callbackFail = null)
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsRewardAvailable())
                {
                    wrapper.ShowReward(callbackComplete, callbackFail);
                    return;
                }
            }
        }

        public bool IsShowBanner()
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsShowBanner())
                    return true;
            }

            return false;
        }

        public void ShowBanner()
        {
            HideBanner();
            
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsBannerAvailable())
                {
                    wrapper.ShowBanner();
                    OnRefreshBanner?.Invoke();
                    break;
                }
            }
        }

        public void HideBanner()
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsShowBanner())
                {
                    wrapper.HideBanner();
                    OnRefreshBanner?.Invoke();
                    break;
                }
            }
        }

        public float GetBannerHeight()
        {
            foreach (var wrapper in _wrappers)
            {
                if (wrapper.IsShowBanner())
                    return wrapper.GetBannerHeight();
            }

            return 0;
        }
        
        private void CheckInternetConnection()
        {
            Updater.Instance.AddDelayedCall(5f, CheckInternetConnection); 
			
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Error. Check internet connection!");
            }
            else
            {
                foreach (var wrapper in _wrappers)
                {
                    wrapper.CheckAvailableAd();
                }
            }
        }
    }
}