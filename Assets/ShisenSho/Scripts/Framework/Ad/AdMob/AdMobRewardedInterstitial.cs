using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Quiz.Framework.Ad.AdMob
{
    public class AdMobRewardedInterstitial : MonoBehaviour
    {
        private RewardedInterstitialAd _rewardedInterstitialAd = null;

        private readonly bool testMode = AdMobConstants.IsAdMobTestMode;

        [SerializeField]
        private string iOSAdUnitId = "";
        [SerializeField]
        private string AndroidAdUnitId = "";

        private static bool _closed = false;
        private static bool _rewarded = false;
        private static bool _hasReceiveReward = false;
        private static long _lastRewardAmount = 0; // 最後に受け取った報酬の数（Google の reward.Amount）

        /// <summary>
        /// 報酬を受け取り済み
        /// </summary>
        public bool Rewarded => _hasReceiveReward;

        /// <summary>
        /// 最後に受け取った報酬の数（通常は 1 など）。0 の場合は ad network が量を返していない。
        /// </summary>
        public long LastRewardAmount => _lastRewardAmount;

        public event Action OnAdClose = null;
        public event Action OnAdRewarded = null;

        private void Awake()
        {
            _closed = false;
            _rewarded = false;
            _hasReceiveReward = false;
        }

        private void Start()
        {
            LoadRewardedInterstitialAd();
        }

        private void Update()
        {
            if (_rewarded)
            {
                OnAdRewarded?.Invoke();
                _hasReceiveReward = true;
                _rewarded = false;
            }

            if (_closed)
            {
                OnAdClose?.Invoke();
                _closed = false;
            }
        }

        private string GetAdUnitId()
        {
            string adUnitId = "unexpected_platform";
            if (testMode)
            {
#if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/6978759866";
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

        private void LoadRewardedInterstitialAd()
        {
            if (_rewardedInterstitialAd != null)
            {
                _rewardedInterstitialAd.Destroy();
                _rewardedInterstitialAd = null;
            }

            Debug.Log("Loading the rewarded interstitial ad.");

            var adRequest = new AdRequest();

            RewardedInterstitialAd.Load(GetAdUnitId(), adRequest,
                (RewardedInterstitialAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedInterstitialAd = ad;
                    RegisterReloadHandler(ad);
                });
        }

        private void RegisterReloadHandler(RewardedInterstitialAd ad)
        {
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content closed.");
                _closed = true;
                LoadRewardedInterstitialAd();
            };

            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded interstitial ad failed to open full screen content " +
                               "with error : " + error);
                LoadRewardedInterstitialAd();
            };
        }

        public bool IsLoaded()
        {
            if (_rewardedInterstitialAd == null)
            {
                return false;
            }

            return _rewardedInterstitialAd.CanShowAd();
        }

        public bool Show()
        {
            if (_rewardedInterstitialAd == null || !_rewardedInterstitialAd.CanShowAd())
            {
                return false;
            }

            _hasReceiveReward = false;
            _rewarded = false;
            _closed = false;

            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                Debug.Log($"Rewarded interstitial ad rewarded the user. Type: {reward.Type}, amount: {reward.Amount}.");
                // GoogleMobileAds から来る amount は double-ish; cast to long for counts
                _lastRewardAmount = (long)reward.Amount;
                _rewarded = true;
            });

            return true;
        }

#if UNITY_EDITOR
        public void DebugAdRewarded()
        {
            OnAdRewarded?.Invoke();
        }
#endif
    }
}
