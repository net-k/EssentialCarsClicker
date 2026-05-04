using Cysharp.Threading.Tasks;
using FruitShop.Presentation;
using FruitShop.Scripts;
using Quiz.Infrastructure;
using SushiCatcher;
using SushiCatcher.Collection.CollectionPage;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace FruitShop
{
    [RequireComponent(typeof(CollectionView))]
    public class CollectionPresenter : MonoBehaviour
    {
        [SerializeField]
        private CollectionView _view = null;

        [SerializeField]
        private CollectionPagePresenter _collectionPagePresenter = null;
        
        private PlayerData _playerData;

        [SerializeField]
        private ShopHeaderPresenter _shopHeaderPresenter = null;
        
        
        [Inject]
        void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }
        
        void Awake()
        {
            _view.BackButton.onClick.AddListener(() =>
            {
                Close();
                SceneManager.LoadScene("Title");
            });

            for (int i = 0; i < _view.GetFruitsCount(); i++)
            {
                int targetId = i + 1;
                var button = _view.GetFruitButton(i);
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        _collectionPagePresenter.Show(targetId).Forget();
                    });
                }
            }
        }

        private void Start()
        {
            ShowFruits();
            
        }


        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void ShowFruits()
        {
            for (int i = 0; i < GameConstants.MaxShopLevel; i++)
            {
                int targetId = i + 1;
                _view.VisibleFruits(targetId, false);
            };
            
            // レベルに応じて、果物の表示を変える
            for (int i = 0; i < _playerData.GetShopLevel(); i++)
            {
                int targetId = i + 1;
                // AchievementSaveDataManager をみて、所持していたら、対応しているフルーツを表示するようにしてほしい
                if (AchievementSaveDataManager.Instance.LoadProgress(targetId) > 0)
                {
                    _view.VisibleFruits(targetId, true);
                }
            }
        }
    }
}
