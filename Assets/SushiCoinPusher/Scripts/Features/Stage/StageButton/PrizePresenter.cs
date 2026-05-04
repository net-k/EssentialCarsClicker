using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SushiCatcher.StageButton
{
    public class PrizePresenter : MonoBehaviour
    {
        [SerializeField]
        PrizeView _prizeView = null;

        private PrizeImageLoader _imageLoader = new PrizeImageLoader();

        private void OnDestroy()
        {
            _imageLoader.Release();
        }

        public async Task Initialize(int prizeId)
        {
            var sprite = await _imageLoader.LoadAsync(prizeId);
            if (sprite != null)
            {
                _prizeView.SetPrizeImage(sprite);
            }
        }
    }
}
