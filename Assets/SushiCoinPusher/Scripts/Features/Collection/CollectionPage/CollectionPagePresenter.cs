using Cysharp.Threading.Tasks;
using SushiCatcher.StageButton;
using UnityEngine;

namespace SushiCatcher.Collection.CollectionPage
{
    [RequireComponent(typeof(CollectionPageView))]
    public class CollectionPagePresenter : MonoBehaviour
    {
        [SerializeField]
        private CollectionPageView _view;

        private PrizeImageLoader _imageLoader = new PrizeImageLoader();
        private PrizeNameProvider _nameProvider = new PrizeNameProvider();

        void Start()
        {
            _view.OkButton.onClick.AddListener(Close);
        }
        
        private void OnDestroy()
        {
            _view.OkButton.onClick.RemoveListener(Close);
            _imageLoader.Release();
        }

        public async UniTask Show(int prizeId)
        {
            await Setup(prizeId);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public async UniTask Setup(int prizeId)
        {
            var sprite = await _imageLoader.LoadAsync(prizeId);
            if (sprite != null)
            {
                _view.CollectionImage.sprite = sprite;
            }

            _view.CollectionNameText.text = _nameProvider.GetName(prizeId);
            _view.CollectionDescriptionText.text = _nameProvider.GetLongDescription(prizeId);
        }
    }
}
