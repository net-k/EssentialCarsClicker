using UnityEngine;

namespace stickin
{
    public class LayoutGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset = Vector3.one;
        [SerializeField] private Vector2Int _size;

        public void Refresh()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var x = _offset.x * (i % _size.x);
                var z = _offset.y * (i / _size.x);

                transform.GetChild(i).localPosition = new Vector3(x, 0, z);
            }
            
        }
    }
}