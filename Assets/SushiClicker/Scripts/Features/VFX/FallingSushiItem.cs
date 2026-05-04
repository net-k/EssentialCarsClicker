using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// 背景に落下する寿司アイテム。SpriteRenderer版。
    /// Update は SushiRainManager が一括で処理するため、このクラスには持たない。
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class FallingSushiItem : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _destroyY;
        private float _speed;

        public float DestroyY => _destroyY;
        public float Speed => _speed;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 初期化。スプライト・削除Y座標・速度・ソート順を設定する。
        /// </summary>
        public void Initialize(Sprite sprite, float destroyY, float speed, string sortingLayerName, int sortingOrder)
        {
            _spriteRenderer.sprite = sprite;
            _destroyY = destroyY;
            _speed = speed;
            _spriteRenderer.sortingLayerName = sortingLayerName;
            _spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
