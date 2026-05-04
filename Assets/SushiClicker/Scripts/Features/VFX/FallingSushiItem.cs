using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// 背景を横走りする車アイテム。SpriteRenderer版。
    /// Update は SushiRainManager が一括で処理するため、このクラスには持たない。
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class FallingSushiItem : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _destroyX;
        private float _velocityX;

        public float DestroyX => _destroyX;
        public float VelocityX => _velocityX;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 初期化。スプライト・削除X座標・X速度・フリップ・ソート順を設定する。
        /// </summary>
        public void Initialize(Sprite sprite, float destroyX, float velocityX, bool flipX, string sortingLayerName, int sortingOrder)
        {
            _spriteRenderer.sprite = sprite;
            _destroyX = destroyX;
            _velocityX = velocityX;
            _spriteRenderer.flipX = flipX;
            _spriteRenderer.sortingLayerName = sortingLayerName;
            _spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
