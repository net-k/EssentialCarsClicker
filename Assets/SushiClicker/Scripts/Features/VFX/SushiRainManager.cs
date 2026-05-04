using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// 背景に車を左右から一定間隔で生成するマネージャー。
    /// SpriteRenderer + 一括Update で大量表示に対応する。
    /// </summary>
    public class SushiRainManager : MonoBehaviour
    {
        private enum SpawnDirection
        {
            Left,
            Right,
            Both
        }

        [SerializeField] private ClickButtonConfig _config = null;
        [SerializeField] private GameObject _fallingSushiPrefab = null;

        [Header("Pool Settings")]
        [SerializeField] private int _poolInitialSize = 30;

        [Header("Spawn Settings")]
        [SerializeField] private int _maxActiveCount = 200;
        [SerializeField] private float _spawnIntervalMin = 0.05f;
        [SerializeField] private float _spawnIntervalMax = 0.3f;
        [SerializeField] private SpawnDirection _spawnDirection = SpawnDirection.Both;
        [SerializeField] private float _minSpawnYSpacing = 0.6f;
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
        /// 全アクティブアイテムを一括で横移動させる。個別の Update() を不要にする。
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
                pos.x += item.VelocityX * dt;
                t.position = pos;

                // 進行方向の反対側端を超えたら回収
                bool isOutOfBounds = (item.VelocityX > 0f && pos.x > item.DestroyX) ||
                                     (item.VelocityX < 0f && pos.x < item.DestroyX);
                if (isOutOfBounds)
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
            bool fromRight = _spawnDirection switch
            {
                SpawnDirection.Left => false,
                SpawnDirection.Right => true,
                _ => Random.value < 0.5f
            };

            float spawnX;
            float destroyX;
            float velocityX;
            bool flipX;

            if (fromRight)
            {
                // 右から左に向かう：スプライトを反転しない
                spawnX = _worldRight + margin;
                destroyX = _worldLeft - margin;
                velocityX = -Random.Range(_speedMin, _speedMax);
                flipX = false;
            }
            else
            {
                // 左から右に向かう：スプライトを反転する
                spawnX = _worldLeft - margin;
                destroyX = _worldRight + margin;
                velocityX = Random.Range(_speedMin, _speedMax);
                flipX = true;
            }

            if (!TryGetSpawnY(out float spawnY)) return;

            var item = GetFromPool();
            item.transform.position = new Vector3(spawnX, spawnY, 0f);
            float scale = Random.Range(_scaleMin, _scaleMax);
            item.transform.localScale = new Vector3(scale, scale, 1f);

            var sprite = sprites[Random.Range(0, sprites.Length)];
            item.Initialize(sprite, destroyX, velocityX, flipX, _sortingLayerName, _sortingOrderBase);
        }

        private bool TryGetSpawnY(out float spawnY)
        {
            if (_minSpawnYSpacing <= 0f || _activeItems.Count == 0)
            {
                spawnY = Random.Range(_worldBottom, _worldTop);
                return true;
            }

            float availableHeight = _worldTop - _worldBottom;
            if (availableHeight <= 0f)
            {
                spawnY = _worldBottom;
                return false;
            }

            int laneCount = Mathf.Max(1, Mathf.FloorToInt(availableHeight / _minSpawnYSpacing) + 1);
            int startLaneIndex = Random.Range(0, laneCount);

            for (int i = 0; i < laneCount; i++)
            {
                int laneIndex = (startLaneIndex + i) % laneCount;
                float normalized = laneCount == 1 ? 0.5f : (float)laneIndex / (laneCount - 1);
                float candidateY = Mathf.Lerp(_worldBottom, _worldTop, normalized);
                if (!IsSpawnYOccupied(candidateY))
                {
                    spawnY = candidateY;
                    return true;
                }
            }

            spawnY = _worldBottom;
            return false;
        }

        private bool IsSpawnYOccupied(float candidateY)
        {
            for (int i = 0; i < _activeItems.Count; i++)
            {
                float activeY = _activeItems[i].transform.position.y;
                if (Mathf.Abs(activeY - candidateY) < _minSpawnYSpacing)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
