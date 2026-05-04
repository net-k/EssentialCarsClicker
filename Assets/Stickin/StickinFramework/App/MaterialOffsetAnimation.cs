using UnityEngine;

namespace stickin
{
    public class MaterialOffsetAnimation : MonoBehaviour
    {
        [SerializeField] private Vector2 _offsetSpeed;
        [SerializeField] private Renderer _renderer;

        private Material _material;
        private Vector2 _offset;
        
        private void Start()
        {
            _material = _renderer.material;
            _offset = _material.mainTextureOffset;
        }

        private void Update()
        {
            _offset += _offsetSpeed * Time.deltaTime;
            _material.mainTextureOffset = _offset;
        }
    }
}
