using CatBreeding.Scripts.Infrastructure.Framework.Ad.AdMob;
using Result;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShisenSho.Result
{
    public class ResultPresenter : MonoBehaviour
    {
        [SerializeField]
        private ResultView _view = null;
        
        [SerializeField]
        private AdMobInterstitial _adMobInterstitial = null;
        
        private void Awake()
        {
            _view.OkButton.onClick.AddListener(() =>
            {
                if (_adMobInterstitial.IsLoaded())
                {
                    //    SoundManager.Instance.StopBGM();
                    _adMobInterstitial.OnAdClose += OnInterstitialClosed;
                    _adMobInterstitial.Show();
                }
                else
                {
                    SceneManager.LoadScene("ShisenSho/Scenes/GameScene");
                }
            });
        }
        private void OnInterstitialClosed()
        {
            _adMobInterstitial.OnAdClose -= OnInterstitialClosed;
            SceneManager.LoadScene("ShisenSho/Scenes/GameScene");
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
