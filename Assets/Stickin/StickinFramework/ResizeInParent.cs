using UnityEngine;

namespace stickin
{
    public class ResizeInParent : MonoBehaviour
    {
        [SerializeField] private float _minScale = 0f;
        [SerializeField] private float _maxScale = 1f;
        
        private RectTransform _thisRT;
        private RectTransform _parentRT;
        
        private Vector2 _previousThisSize;
        private Vector2 _previousParentSize;
        
        private void Start()
        {
            _thisRT = this.RectTransform();
            _parentRT = _thisRT.parent as RectTransform;
        }

        private void Update()
        {
            if (_parentRT != null && 
                _thisRT != null && 
                (_previousParentSize != _parentRT.rect.size ||
                 _previousThisSize != _thisRT.rect.size))
            {
                _previousParentSize = _parentRT.rect.size;
                _previousThisSize = _thisRT.rect.size;
                
                _thisRT.ResizeInParent(_minScale, _maxScale);
            }
        }
    }
}