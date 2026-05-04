using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// 背景に寿司を一定間隔で生成するマネージャー。
    /// SpriteRenderer + 一括Update で大量表示に対応する。
    /// </summary>
    public class SushiRainManager : MonoBehaviour
    {
        [SerializeField] private ClickButtonConfig _config = null;
        [SerializeField] private GameObject _fallingSushiPrefab = null;

        [Header("Pool Settings")]
        [SerializeField] private int _poolInitialSize = 30;

        [Header("Spawn Settings")]
        [SerializeField] private int _maxActiveCount = 200;
        [SerializeField] private float _spawnIntervalMin = 0.05f;
        [SerializeField] private float _spawnIntervalMax = 0.3f;
        [SerializeField] private float _speedMin = 2f;
        [SerializeField] private float _speedMax = 5f;
        [SerializeField] private float _scaleMin = 0.005f;
        [SerializeField] private float _scaleMax = 0.02f;

        [Header("Sorting")]
        [SerializeField] private string _sortingLayerName = "Default";
        [SerializeField] private int _sortingOrderBase = -100;

        private Camera _mainCamera;
        private Transform _container;
        private readonly Queue<FallingSushiItem> _pool = new Queue<FallingSushiItem>();
        private readonly List<FallingSushiItem> _activeItems = new List<FallingSushiItem>();

        // カメラ境界キャッシュ（ワールド座標）
        private float _worldLeft;
        private float _worldRight;
        private float _worldTop;
        private float _worldBottom;

        private void Start()
        {
            _mainCamera = Camera.main;

            // Canvas外にコンテナを作成（SpriteRendererはCanvas配下にしない）
            var containerGo = new GameObject("SushiRainContainer");
            containerGo.transform.SetParent(null);
            _container = containerGo.transform;

            CacheWorldBounds();
            PrewarmPool();
            StartCoroutine(SpawnLoop());
        }

        private void OnDestroy()
        {
            if (_container != null)
            {
                Destroy(_container.gameObject);
            }
        }

        private void CacheWorldBounds()
        {
            if (_mainCamera == null) return;

            // カメラのビューポート端をワールド座標に変換
            // Canvas との干渉を避けるため、カメラから 10 単位離れた位置を基準にする
            Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 10f));
            Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 10f));

            _worldLeft = bottomLeft.x;
            _worldRight = topRight.x;
            _worldBottom = bottomLeft.y;
            _worldTop = topRight.y;
        }

        private void PrewarmPool()
        {
            for (int i = 0; i < _poolInitialSize; i++)
            {
                var item = CreateNewItem();
                item.gameObject.SetActive(false);
                _pool.Enqueue(item);
            }
        }

        private FallingSushiItem CreateNewItem()
        {
            var go = Instantiate(_fallingSushiPrefab, _container);
            return go.GetComponent<FallingSushiItem>();
        }

        private FallingSushiItem GetFromPool()
        {
            FallingSushiItem item;
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
            }
            else
            {
                item = CreateNewItem();
            }
            item.gameObject.SetActive(true);
            _activeItems.Add(item);
            return item;
        }

        private void ReturnToPool(FallingSushiItem item)
        {
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        /// <summary>
        /// 全アクティブアイテムを一括で移動させる。個別の Update() を不要にする。
        /// </summary>
        private void Update()
        {
            float dt = Time.deltaTime;

            // 逆順ループで安全に削除
            for (int i = _activeItems.Count - 1; i >= 0; i--)
            {
                var item = _activeItems[i];
                var t = item.transform;
                var pos = t.position;
                pos.y -= item.Speed * dt;
                t.position = pos;

                if (pos.y < item.DestroyY)
                {
                    _activeItems.RemoveAt(i);
                    ReturnToPool(item);
                }
            }
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                float interval = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
                yield return new WaitForSeconds(interval);

                if (_activeItems.Count >= _maxActiveCount) continue;

                SpawnSushi();
            }
        }

        private void SpawnSushi()
        {
            var sprites = _config != null ? _config.RainSprites : null;
            if (_fallingSushiPrefab == null || sprites == null || sprites.Length == 0) return;

            float margin = 0.5f;
            float spawnX = Random.Range(_worldLeft - margin, _worldRight + margin);
            float spawnY = _worldTop + 1f;
            float destroyY = _worldBottom - 1f;
            float speed = Random.Range(_speedMin, _speedMax);

            var item = GetFromPool();
            item.transform.position = new Vector3(spawnX, spawnY, 0f);
            float scale = Random.Range(_scaleMin, _scaleMax);
            item.transform.localScale = new Vector3(scale, scale, 1f);

            var sprite = sprites[Random.Range(0, sprites.Length)];
            item.Initialize(sprite, destroyY, speed, _sortingLayerName, _sortingOrderBase);
        }
    }
}
