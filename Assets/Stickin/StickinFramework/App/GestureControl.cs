using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace stickin
{
    public class GestureControl : TouchMonoBehaviour
    {
        [SerializeField] private float _distanceSensetive = 5f;
        
        public event Action<Vector2Int> OnSwipe;

        private Vector2 _startPosition;
        private Vector2 _force;
        
        protected override void OnTouchedBegan(PointerEventData eventData)
        {
            base.OnTouchedBegan(eventData);

            _startPosition = ConvertPosToTransform(eventData.position, transform);
        }

        protected override void OnTouchedMoved(PointerEventData eventData)
        {
            base.OnTouchedMoved(eventData);
            
            var position = ConvertPosToTransform(eventData.position, transform);
            
            // logic
            _force = position - _startPosition;

            _startPosition = position;
        }

        protected override void OnTouchedEnded(PointerEventData eventData)
        {
            base.OnTouchedEnded(eventData);

            if (Vector2.Distance(Vector2.zero, _force) >= _distanceSensetive)
            {
                var direction = Vector2Int.zero;
                
                if (Mathf.Abs(_force.x) > Mathf.Abs(_force.y))
                {
                    if (_force.x > 0)
                        direction = Vector2Int.right;
                    else
                        direction = Vector2Int.left;
                }
                else
                {
                    if (_force.y > 0)
                        direction = Vector2Int.up;
                    else
                        direction = Vector2Int.down;
                }
                
                OnSwipe?.Invoke(direction);
            }
        }

        public Vector2 ConvertPosInRt(Vector2 screenPosition, Transform tr)
        {
            var result = ConvertPosToTransform(screenPosition, tr);
            return result;
        }
    }
}