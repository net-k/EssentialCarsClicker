using System;
using GoogleMobileAds.Api;
using Quiz.Framework.Ad.AdMob;
using UnityEngine;

namespace CatBreeding.Scripts.Infrastructure.Framework.Ad.AdMob
{
    public class AdMobInterstitial : MonoBehaviour
    {
        private InterstitialAd interstitial = null;
    
        // [SerializeField] 
        private readonly bool testMode = AdMobConstants.IsAdMobTestMode;
    
        [SerializeField]
        private string iOSAdUnitId = "ca-app-pub-2837388897714947/4251925801";// 確認済み
        [SerializeField]
        private string AndroidAdUnitId = "ca-app-pub-2837388897714947/1737443105";// 確認済み
   
        static private bool closed;
        static private bool showed;
        public event Action OnAdClose;

        private void Awake()
        {
            closed = false;
            showed = false;
            OnAdClose = null;
        }

        // Use this for initialization
        void Start () {
            RequestInterstitial();
        }
	
        // Update is called once per frame
        void Update () {
            if (closed == true)
            {

                if (OnAdClose != null)
                {
                    OnAdClose();
                }
                closed = false; 
            }
        }

        string GetAdUnitId()
        {
            string adUnitId = "unexpected_platform";
            if (testMode)
            {
#if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/1033173712"; // test interstitial
                // adUnitId = "ca-app-pub-3940256099942544/8691691433"; // test interstitial video
#elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/4411468910"; // test
                // adUnitId = "ca-app-pub-3940256099942544/5135589807"; // test interstitial video
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

        public void RequestInterstitial()
        {
            string adUnitId = GetAdUnitId();

            if (this.interstitial != null)
            {
                this.interstitial.Destroy();
            }
            var adRequest = new AdRequest();

            // Create an interstitial.
            // this.interstitial = new InterstitialAd(adUnitId);
            InterstitialAd.Load(adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    RegisterReloadHandler(ad);
                    this. interstitial = ad;
                });

        }

        private void RegisterReloadHandler(InterstitialAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                RequestInterstitial();
                closed = true;
                showed = false;
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                RequestInterstitial();
            };
        }
        
 

        public bool IsLoaded()
        {
            if (interstitial == null)
            {
                return false;
            }

            return interstitial.CanShowAd();
        }

        bool IsShow()
        {
            if (showed)
            {
                return true;
            }

            return false;
        }
        
        public bool Show()
        {
            if (interstitial.CanShowAd())
            {
                showed = true;
                closed = false;
            //    SoundManager.Instance.Suspend(); // TODO サウンドマネージャーここにいれないほうがよい。Event などのコールバックでやる
                interstitial.Show();
                return true;
            }

            return false;
        }
    }
}
