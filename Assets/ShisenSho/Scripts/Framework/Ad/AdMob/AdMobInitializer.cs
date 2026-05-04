using GoogleMobileAds.Api;
using Quiz.Framework.Ad.AdMob;
using UnityEngine;

public class AdMobInitializer : SingletonMonoBehaviour<AdMobInitializer>
{
	// [SerializeField] 
	private static bool testMode = AdMobConstants.IsAdMobTestMode;
	
//	[SerializeField]
	private static string iOSAppId = "ca-app-pub-2837388897714947~4000074438"; 
//	[SerializeField]
	private static string AndroidAppId = "ca-app-pub-2837388897714947~1497006508"; 
	
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize ()
	{
		Debug.Log("AdMobInitializer.Initialize");
		string appId = GetAppId();
		// MobileAds.Initialize(appId);	
		MobileAds.Initialize(initStatus => { });

	}

	static string GetAppId()
	{
		string appId = "unexpected_platform";
		if (testMode)
		{

#if UNITY_ANDROID
            appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
			appId = "ca-app-pub-3940256099942544~1458002511";
#else
            appId = "unexpected_platform";
#endif
		}
		else
		{
#if UNITY_ANDROID
            appId = AndroidAppId;
#elif UNITY_IPHONE
			appId = iOSAppId;
#else
            appId = "unexpected_platform";
#endif		
		}

		return appId;
	}
}
