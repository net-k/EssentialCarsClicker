using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Quiz.Framework.Ad.AdMob
{
    public class AdMobRewardVideo : MonoBehaviour
    {
        private RewardedAd rewardBasedVideo;

        // [SerializeField] 
        private readonly bool testMode = AdMobConstants.IsAdMobTestMode;
    
        [SerializeField]
        private string iOSAdUnitId = "";// 確認済み
        [SerializeField]
        private string AndroidAdUnitId = "ca-app-pub-2837388897714947/4707437285"; 

        private static bool closed = false;
        private static bool rewarded = false;
        private static bool hasReceiveReward = false;
        
        /// <summary>
        /// 報酬を受け取り済み
        /// </summary>
        public bool Rewarded => hasReceiveReward;

        public event Action OnAdClose = null;
        public event Action OnAdRewarded = null;

        public void Awake()
        {
            closed = false;
            rewarded = false;
            hasReceiveReward = false;
        //    OnAdClose = null;
        //    OnAdRewarded = null;
        }
        public void Start()
        {
            this.RequestRewardBasedVideo();
        }

        private void Update()
        {

            if (rewarded == true)
            {
                if (OnAdRewarded != null)
                {
                    print("Update OnAdRewarded");
                    OnAdRewarded();
                    hasReceiveReward = true;
                }
                rewarded = false;
            }

            if (closed == true)
            {
                if (OnAdClose != null)
                {
                    print("Update OnAdClose");
                    OnAdClose();
                }
                closed = false; 
            }
        }

#if UNITY_EDITOR
        public void DebugAdRewarded()
        {
            if( OnAdRewarded != null)
            {
                OnAdRewarded();
            }
        }
#endif

        string GetAdUnitId()
        {
            string adUnitId = "unexpected_platform";
            if (testMode)
            {
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
#endif
            }
            else
            {
#if UNITY_ANDROID
            adUnitId = AndroidAdUnitId;
#elif UNITY_IPHONE
                adUnitId = iOSAdUnitId;
#endif
            }

            return adUnitId;
        }
    
        private void RequestRewardBasedVideo()
        {
            // Get singleton reward based video ad reference.
            string adUnitId = GetAdUnitId();

            if ( rewardBasedVideo != null)
            {
                rewardBasedVideo.Destroy();
                rewardBasedVideo = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();
            // adRequest.Keywords.Add("unity-admob-sample");

            // send the request to load the ad.
            RewardedAd.Load(adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardBasedVideo = ad;
                    RegisterReloadHandler(ad);
                });
        }

        private void RegisterReloadHandler(RewardedAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                RequestRewardBasedVideo();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                RequestRewardBasedVideo();
            };
        }
        public void HandleRewardBasedVideoRewarded()
        {
            Debug.Log("AdMob Rewarded");
            rewarded = true;
        }
        public bool Show()
        {
            if (rewardBasedVideo.CanShowAd())
            {
                hasReceiveReward = false;  
                rewarded = false;
                closed = false;
                rewardBasedVideo.Show( (Reward reward) =>
                {
                    HandleRewardBasedVideoRewarded();
                });
                return true;
            }
            else
            {
                // this.RequestRewardBasedVideo();
            }
            return false;
        }

        public bool IsLoaded()
        {
            return rewardBasedVideo.CanShowAd();
        }
    }
}
