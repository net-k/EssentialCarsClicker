using System;
using Balaso;
using ShisenSho.Framework;
using SushiCatcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quiz.Application.Boot
{
        
    public class BootScene : MonoBehaviour
    {

        [SerializeField]
        private AdMobBanner _adMobBanner = null;

        
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_IOS
            int iOSVersion = GetiOSVersion().Major;
//        Debug.Log($"iOS Version = {iOSVersion}");
            if ( iOSVersion > 13 )
            {
                AppTrackingTransparency.OnAuthorizationRequestDone += OnAuthorizationRequestDone;

                AppTrackingTransparency.AuthorizationStatus currentStatus =
                    AppTrackingTransparency.TrackingAuthorizationStatus;
                Debug.Log(string.Format("Current authorization status: {0}", currentStatus.ToString()));
                if (currentStatus != AppTrackingTransparency.AuthorizationStatus.AUTHORIZED)
                {
                    Debug.Log("Requesting authorization...");
                    AppTrackingTransparency.RequestTrackingAuthorization();
                }
                else
                {
                    InitializeAds();
                }
            }
            else
            {
                InitializeAds();
            }
#else
        InitializeAds();
#endif
        }
    
#if UNITY_IOS
        /// <summary>
        /// Callback invoked with the user's decision
        /// </summary>
        /// <param name="status"></param>
        private void OnAuthorizationRequestDone(AppTrackingTransparency.AuthorizationStatus status)
        {
            switch (status)
            {
                case AppTrackingTransparency.AuthorizationStatus.NOT_DETERMINED:
                    Debug.Log("AuthorizationStatus: NOT_DETERMINED");
                    break;
                case AppTrackingTransparency.AuthorizationStatus.RESTRICTED:
                    Debug.Log("AuthorizationStatus: RESTRICTED");
                    break;
                case AppTrackingTransparency.AuthorizationStatus.DENIED:
                    Debug.Log("AuthorizationStatus: DENIED");
                    break;
                case AppTrackingTransparency.AuthorizationStatus.AUTHORIZED:
                    Debug.Log("AuthorizationStatus: AUTHORIZED");
                    break;
            }

            // Obtain IDFA
            Debug.Log(string.Format("IDFA: {0}", AppTrackingTransparency.IdentifierForAdvertising()));
            InitializeAds();
        }
#endif
    
        /// <summary>
        /// iPadOS 15.1 の iPad端末は、SystemInfo.operatingSystemが
        ///"iPadOS 15.1"のような「iPadOS」から始まる文字列を返す
        /// </summary>
        /// <returns></returns>
        static Version GetiOSVersion()
        {
            var operatingSystemString = SystemInfo.operatingSystem;

            // 「数字」から始まり、「.(ピリオド)」と「数字」が連続する文字列を探し出す。
            var regex = new System.Text.RegularExpressions.Regex("([0-9]+)(\\.[0-9]+)*");
            var match = regex.Match(operatingSystemString);
            if (!match.Success)
            {
                return null;
            }

            Version version;
            if (Version.TryParse(match.Value, out version))
            {
                return version;
            }

            return null;
        }

        private void InitializeAds()
        {
            _adMobBanner.RequestBanner();

            AdMobObjectHolder.Instance.Setup(_adMobBanner);
            SushiCatcherSceneManager.Load( SushiCaterScene.Game );
        }
        
        public static bool IsIPad {
            get {
#if UNITY_IPHONE
                return SystemInfo.deviceModel.Contains ("iPad");
#else
        return false;
#endif
            }
        } 
           
    }
}
