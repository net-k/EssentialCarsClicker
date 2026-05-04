using System;
using System.Collections;
using System.Collections.Generic;
using Quiz.Framework.Ad.AdMob;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AdMob, Applovin, Unity の出し分けをする
/// </summary>
public class RewardAdsManager : SingletonMonoBehaviour<RewardAdsManager>
{
    enum AdType
    {
        AppLovin,
        UnityAds,
        AdMob
    }

    private AdType _adType = AdType.AdMob;
#if false    
    [SerializeField]
    private UnityAdsManager unityAds = null;

    [SerializeField]
    private AppLovinAdsManager appLovinAds = null;
#endif
    [SerializeField] private AdMobRewardVideo _adMobRewardVideo = null;

    /// <summary>
    /// 広告が閉じられた場合のコールバック
    /// </summary>
    public event Action OnAdClose;
    
    /// <summary>
    /// 動画広告の報酬を受け取った場合のコールバック
    /// </summary>
    public event Action OnAdRewarded;

    private void Awake()
    {
        #if false
        if (unityAds)
        {
            unityAds.onHandleShowResultFinished += OnUnityAdsHandleShowResultFinished;
            unityAds.onHandleShowResultSkipped += OnUnityAdsHandleShowResultSkipped;
            unityAds.onHandleShowResultFailed += OnUnityAdsHandleShowResultFailed;
        }

        if (appLovinAds)
        {
            appLovinAds.onHandleShowResultFinished += OnAppLovinHandleShowResultFinished;
        }
        #endif
        if (_adMobRewardVideo)
        {
            _adMobRewardVideo.OnAdClose += OnAdMobClosed;
            _adMobRewardVideo.OnAdRewarded += OnAdMobRewarded;
        }
    }

    private void Update()
    {
        // 必要なかった
        #if false
        switch (_adType)
        {
            case AdType.AppLovin:
                break;
            case AdType.UnityAds:
                break;
            case AdType.AdMob:
                _adMobRewardVideo.Update();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        #endif
        
    }

    private void Start()
    {
        
    }

    public bool IsReady()
    {
        switch (_adType)
        {
       //     case AdType.AppLovin:
       //         return appLovinAds.IsReadyRewardedAd();
       //     case AdType.UnityAds:
       //         return unityAds.IsReadyRewardedAd();
            case AdType.AdMob:
                if (_adMobRewardVideo == null)
                {
                    return false;
                }
                return _adMobRewardVideo.IsLoaded();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool Show()
    {
        bool ret = false;
        switch (_adType)
        {
            #if false
            case AdType.AppLovin:
                ret = appLovinAds.ShowRewardedAd();
                break;
            case AdType.UnityAds:
                ret = unityAds.ShowRewardedAd();
                break;
            #endif
            case AdType.AdMob:
                ret = _adMobRewardVideo.Show();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return ret;
    }

    
    
    private void OnAdMobClosed()
    {
        if (OnAdClose != null) OnAdClose();
    }

    private void OnAdMobRewarded()
    {
        if (OnAdRewarded != null) OnAdRewarded();
    }


 
	public void OnUnityAdsHandleShowResultFinished()
	{
	}

    public void OnUnityAdsHandleShowResultSkipped()
    {
    }
    public void OnUnityAdsHandleShowResultFailed()
    {
    }

    public void OnAppLovinHandleShowResultFinished()
    {
    }


    public void OnAppLovinHandleShowResultSkipped()
    {
    }

    public void OnAppLovinHandleShowResultFailed()
    {
    }

    public void Initialize()
    {
    }
    
    public bool HasReceivedReward()
    {
        bool ret = false;
        switch (_adType)
        {
            case AdType.AppLovin:
                // ret = appLovinAds.ShowRewardedAd();
                break;
            case AdType.UnityAds:
                // ret = unityAds.ShowRewardedAd();
                break;
            case AdType.AdMob:
                ret = _adMobRewardVideo.Rewarded;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return ret;
    }
}
