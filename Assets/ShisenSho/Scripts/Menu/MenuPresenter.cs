using CatBreeding.Scripts.Infrastructure.Framework.Ad.AdMob;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShisenSho.Menu
{
    public class MenuPresenter : MonoBehaviour
    {
        [SerializeField]
        private MenuView _menuView = null;
        
        [SerializeField] 
        private AdMobInterstitial _adMobInterstitial = null;

        private void Awake()
        {
            _menuView.NewGameButton.onClick.AddListener(() =>
            {
                if (_adMobInterstitial.IsLoaded())
                {
                //    SoundManager.Instance.StopBGM();
                    _adMobInterstitial.OnAdClose += OnInterstitialClosedToGameScene;
                    _adMobInterstitial.Show();
                }
                else
                {
                    SceneManager.LoadScene("ShisenSho/Scenes/GameScene");
                }
            
            });
        
            _menuView.TitleBackButton.onClick.AddListener(() =>
            {
                
                if (_adMobInterstitial.IsLoaded())
                {
                //    SoundManager.Instance.StopBGM();
                    _adMobInterstitial.OnAdClose += OnInterstitialClosed;
                    _adMobInterstitial.Show();
                }
                else
                {
                    SceneManager.LoadScene("ShisenSho/Scenes/TitleScene");
                }
            });
        
            _menuView.CloseButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }
        private void OnInterstitialClosedToGameScene()
        {
            _adMobInterstitial.OnAdClose -= OnInterstitialClosed;
            SceneManager.LoadScene("ShisenSho/Scenes/GameScene");
        }
 
        private void OnInterstitialClosed()
        {
            _adMobInterstitial.OnAdClose -= OnInterstitialClosed;
            SceneManager.LoadScene("ShisenSho/Scenes/TitleScene");
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
