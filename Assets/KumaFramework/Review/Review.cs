using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace KumaFramework
{
    public class Review : MonoBehaviour
    {
        [SerializeField] private string iosURL = "https://itunes.apple.com/jp/app/id1382248773?mt=8&action=write-review";
        [SerializeField] private string androidURL = "https://play.google.com/store/apps/details?id=appPackageName";
        [SerializeField] private int ShowReviewLaunchCount = 4;
        [SerializeField] private AndroidReviewDialogPresenter _androidReviewDialogPresenter;

        private string key = "LaunchCountForReview";
        private string ReviewCompletedKey = "ReviewCompletedKey";

        private void Awake()
        {
            int launchCount = 1;
            if (PlayerPrefs.HasKey(key))
            {
                launchCount = PlayerPrefs.GetInt(key) + 1;
            }

            PlayerPrefs.SetInt(key, launchCount);
            PlayerPrefs.Save();
        }

        private int GetLaunchCount()
        {
            int launchCount = 0;
            if (PlayerPrefs.HasKey(key))
            {
                launchCount = PlayerPrefs.GetInt(key);
            }

            return launchCount;
        }

        bool IsReviewCompleted()
        {
            if (PlayerPrefs.HasKey(ReviewCompletedKey))
            {
                return PlayerPrefs.GetInt(ReviewCompletedKey) > 0;
            }

            return false;
        }

        public void DoneReview()
        {
            PlayerPrefs.SetInt(ReviewCompletedKey, 1);
            PlayerPrefs.Save();
        }

        public bool ShouldRequest()
        {
            if (GetLaunchCount() % ShowReviewLaunchCount == 0 && !IsReviewCompleted())
            {
                return true;
            }

            return false;
        }

        public void Request()
        {
#if false
        if (GetLaunchCount() != ShowReviewLaunchCount)
        {
            return;
        }
#endif
#if UNITY_IOS
            if (!UnityEngine.iOS.Device.RequestStoreReview())
#endif
            {
#if UNITY_ANDROID
            if (_androidReviewDialogPresenter != null)
            {
                _androidReviewDialogPresenter.gameObject.SetActive(true);
                _androidReviewDialogPresenter.Show(GetURL(), this);
            }
#endif
            }

        }

        private string GetURL()
        {
            string url = "";
#if UNITY_IOS
            url = iosURL;
#else
		url = androidURL;
#endif
            return url;
        }
    }
}
