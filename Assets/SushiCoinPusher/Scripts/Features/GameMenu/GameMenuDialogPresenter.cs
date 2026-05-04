using System;
using CatBreeding.Scripts.Infrastructure.Framework.Ad.AdMob;
using UnityEngine;

namespace SushiCoinPusher.Features.GameMenu
{
    public class GameMenuDialogPresenter : MonoBehaviour
    {
        [SerializeField]
        private GameMenuDialogView _view = null;

        [SerializeField]
        private AdMobInterstitial _adMobInterstitial = null;

        private void Awake()
        {
            _view.CloseButton.onClick.AddListener(Hide);
            _view.TitleBackButton.onClick.AddListener(() =>
            {
                if (_adMobInterstitial != null && _adMobInterstitial.IsLoaded())
                {
                    _adMobInterstitial.OnAdClose += OnInterstitialClosed;
                    _adMobInterstitial.Show();
                }
                else
                {
                    SushiCoinPusherSceneManager.Load(SushiCaterScene.Title);
                }
            });
        }

        private void OnInterstitialClosed()
        {
            if (_adMobInterstitial != null)
            {
                _adMobInterstitial.OnAdClose -= OnInterstitialClosed;
            }
            SushiCoinPusherSceneManager.Load(SushiCaterScene.Title);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
