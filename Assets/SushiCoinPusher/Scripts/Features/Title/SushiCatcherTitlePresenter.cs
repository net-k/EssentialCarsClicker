using App;
using MemoryOnline.Presentation;
using Quiz.Framework.Life;
using Quiz.Framework.SupportScene;
using SushiCatcher;
using UnityEngine;
using Zenject;

namespace ShogiOnline.Presentation
{
    public class SushiCatcherTitlePresenter : MonoBehaviour
    {
        [SerializeField]
        private SushiCatcherTitleView _view = null;

        [SerializeField]
        private SupportSceneButton _supportSceneButton = null;

        private LifeManager _lifeManager;
        
        [SerializeField]
        NotEnoughLifePresenter _notEnoughLifePresenter = null;
        
        [Inject]
        private void Construct( LifeManager lifeManager )
        {
            _lifeManager = lifeManager;
        }
        
        private void Awake()
        {
            _view.StartButton.onClick.AddListener(() =>
            {
                if( _lifeManager.IsEmpty(LifeSaveDataManager.LifeType.Default) )
                {
                    _notEnoughLifePresenter.Show();
                    return;
                }

                _lifeManager.UseActionPoint(1, LifeSaveDataManager.LifeType.Default );
                SushiCatcherSceneManager.Load( SushiCaterScene.Game );
            });
            
            _view.AchievementButton.onClick.AddListener(() =>
            {
                SushiCatcherSceneManager.Load( SushiCaterScene.Achievement );
            });
            
            _view.CollectionButton.onClick.AddListener(() =>
            {
                SushiCatcherSceneManager.Load( SushiCaterScene.Collection );
            });
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
