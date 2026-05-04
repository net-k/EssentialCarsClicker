// デバッグ用に寿司を強制的に表示する場合は、この行のコメントを解除してください
//#define DEBUG_SUSHI

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SushiCatcher;
using SushiCoinPusher.Features.Collection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using SushiCoinPusher.Features.Collection.SushiSlot; // Add this using statement

namespace SushiCoinPusher.Scripts.Features.Collection
{
    [RequireComponent(typeof(SushiCollectionView))]
    public class SushiCollectionPresenter : MonoBehaviour
    {
        [SerializeField]
        private SushiCollectionView _view;

        [Header("3D Preview Settings")]
        [SerializeField]
        private Transform _modelSpawnRoot;

        [SerializeField]
        private Camera _previewCameraPrefab;

        [SerializeField]
        private Vector2Int _renderTextureSize = new Vector2Int(256, 256);
        
        [SerializeField]
        private Vector3 _cameraOffset = new Vector3(0, 1, -2);

        [Tooltip("プレビュー時のモデルの回転オフセット（オイラー角）")]
        [SerializeField]
        private Vector3 _modelRotationOffset = new Vector3(0, 90, 0); // デフォルトでY軸180度回転

        private List<string> _prizeAddresses = new List<string>()
        {
            "Assets/Sushi_set_D/Prefabs/Individual/Eel.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Egg.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Tuna.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Squid.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Salmon.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Shrimp.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Octopus.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Scallop.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Sea_bream.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Inarizushi.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Salmon_roe.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Sea_urchin.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Sushi_roll.prefab",
            "Assets/Sushi_set_D/Prefabs/Individual/Greater_amberjack.prefab"
        };

        private List<RenderTexture> _renderTextures = new List<RenderTexture>();
        private List<GameObject> _spawnedModels = new List<GameObject>();
        
        private Camera _previewCamera;

        private void Awake()
        {
            if (_previewCameraPrefab != null)
            {
                _previewCamera = Instantiate(_previewCameraPrefab, _modelSpawnRoot);
                _previewCamera.enabled = false; 
                Debug.Log($"Preview Camera '{_previewCamera.name}' instantiated. Culling Mask is: {_previewCamera.cullingMask}");
            }
            else
            {
                Debug.LogError("Preview Camera Prefab is not assigned!");
            }
        }

        private void Start()
        {
            if (_view == null) _view = GetComponent<SushiCollectionView>();

            if (_view.BackButton != null)
            {
                _view.BackButton.onClick.AddListener(() => SceneManager.LoadScene("TitleScene"));
            }

            InitializeCollection().Forget();
        }

        private void OnDestroy()
        {
            _view.ClearSlots();
            foreach (var rt in _renderTextures)
            {
                if (rt != null) rt.Release();
            }
            _renderTextures.Clear();
            foreach (var model in _spawnedModels)
            {
                if(model != null) Addressables.ReleaseInstance(model);
            }
            _spawnedModels.Clear();
            
            if(_previewCamera != null)
            {
                Destroy(_previewCamera.gameObject);
            }
        }

        private async UniTaskVoid InitializeCollection()
        {
            Debug.Log("--- Sushi Collection Initialization Started ---");

            if (_view == null || _previewCamera == null)
            {
                Debug.LogError("SushiCollectionView or PreviewCamera is not ready!");
                return;
            }
            
            _view.ClearSlots();
            
            float spacing = 10.0f;

#if DEBUG_SUSHI
            int itemsToProcess = 10;
#else
            int itemsToProcess = _prizeAddresses.Count;
#endif

            for (int i = 0; i < itemsToProcess; i++)
            {
                if (i >= _prizeAddresses.Count) break;

                int count = 0;
                bool isUnlocked;
#if DEBUG_SUSHI
                isUnlocked = true;
                count = 5; // デバッグ用に適当な数を設定
#else
                int targetId = i + 1;
                if (AchievementSaveDataManager.Instance != null)
                {
                    count = AchievementSaveDataManager.Instance.LoadProgress(targetId);
                }
                isUnlocked = count > 0;
#endif

                // 未入手のものは非表示（スロットを作成しない）
                if (!isUnlocked)
                {
                    continue;
                }

                var slot = _view.CreateSlot();
                if (slot == null) continue;

                // スロットが非アクティブなら強制的にアクティブ化し、警告を出す
                if (!slot.gameObject.activeSelf)
                {
                    Debug.LogWarning($"Slot for item {i} was instantiated as inactive. Forcing it to active. Please check your SushiSlot prefab settings in the Project window.");
                    slot.gameObject.SetActive(true);
                }

                // SushiSlotPresenterを取得
                var slotPresenter = slot.GetComponent<SushiSlotPresenter>();
                if (slotPresenter == null)
                {
                    Debug.LogError($"SushiSlotPresenter not found on slot for item {i}.");
                    continue;
                }
                
                // 所持数を設定
                slotPresenter.SetCount(count);

                Vector3 spawnPosition = _modelSpawnRoot.position + new Vector3(i * spacing, 0, 0);
                var renderTexture = await SetupPreview(_prizeAddresses[i], spawnPosition);
                if (renderTexture != null && slot.RenderImage != null)
                {
                    slot.RenderImage.texture = renderTexture;
                    slot.RenderImage.color = Color.white;
                }
                else
                {
                        Debug.LogError($"Failed to set RenderTexture for item {i}. RenderTexture or RawImage was null.");
                }
            }
            Debug.Log("--- Sushi Collection Initialization Finished ---");
        }

        private async UniTask<RenderTexture> SetupPreview(string address, Vector3 position)
        {
            var handle = Addressables.InstantiateAsync(address, position, Quaternion.identity, _modelSpawnRoot);
            var model = await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded || model == null)
            {
                Debug.LogError($"Failed to load model from address: {address}");
                return null;
            }
            
            _spawnedModels.Add(model);
            Debug.Log($"Model '{model.name}' loaded. Layer is: {LayerMask.LayerToName(model.layer)}");

            // モデルの回転を適用
            model.transform.rotation = Quaternion.Euler(_modelRotationOffset);

            var rigidbodies = model.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }

            var rt = new RenderTexture(_renderTextureSize.x, _renderTextureSize.y, 16);
            rt.Create();
            _renderTextures.Add(rt);

            _previewCamera.transform.position = position + _cameraOffset;
            _previewCamera.transform.LookAt(model.transform);
            _previewCamera.targetTexture = rt;
            _previewCamera.Render();
            _previewCamera.targetTexture = null;

            Debug.Log($"Rendered model {model.name} to RenderTexture. If it appears black, check lighting. If it's empty, check Camera Culling Mask vs Model Layer.");

            return rt;
        }
    }
}
