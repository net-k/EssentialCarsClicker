using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SushiCatcher.StageButton
{
    public class PrizeImageLoader
    {
        private AsyncOperationHandle<Sprite> _loadHandle;

        public async Task<Sprite> LoadAsync(int prizeId)
        {
            Release();

            string addressableKey = $"Assets/SushiCatcher/Textures/Prizes/{prizeId}.png";
            _loadHandle = Addressables.LoadAssetAsync<Sprite>(addressableKey);
            await _loadHandle.Task;

            if (_loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                return _loadHandle.Result;
            }
            
            Debug.LogError($"Failed to load asset: {addressableKey}");
            return null;
        }

        public void Release()
        {
            if (_loadHandle.IsValid())
            {
                Addressables.Release(_loadHandle);
            }
        }
    }
}
