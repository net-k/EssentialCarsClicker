using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    public class ProgressPieceView : MonoBehaviour
    {
        [SerializeField] private RectMask2D _rectMask;
        [SerializeField] private Image _image;
        [SerializeField] private float _changeSpeed = 100f;

        private bool _isInit = false;
        private float _needLeft;
        private float _needRight;
        private float _currenLeft;
        private float _currentRight;
        
        public void Init(Color color, int value, float x, float sum, Vector2 size)
        {
            var left = x / sum * size.x;
            var right = size.x - (x + value) / sum * size.x;
                
            gameObject.SetActive(true);

            _needLeft = left;
            _needRight = right;
            _image.color = color;
            
            if (!_isInit)
            {
                _isInit = true;
                
                _currenLeft = left;
                _currentRight = right;
                RefreshView();
            }
        }

        private void Update()
        {
            if (_currenLeft != _needLeft || _currentRight != _needRight)
            {
                _currenLeft = Mathf.MoveTowards(_currenLeft, _needLeft, _changeSpeed * Time.deltaTime);
                _currentRight = Mathf.MoveTowards(_currentRight, _needRight, _changeSpeed * Time.deltaTime);
                
                RefreshView();
            }
        }

        private void RefreshView()
        {
            _rectMask.padding = new Vector4(_currenLeft, _rectMask.padding.y, _currentRight, _rectMask.padding.w);
        }
    }
}