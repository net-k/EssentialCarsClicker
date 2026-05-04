using UnityEngine;

namespace stickin
{
    public class LayoutRotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _minAngle;
        [SerializeField] private Vector3 _maxAngle;

        public void Reset()
        {
            for (var i = 0; i < transform.childCount; i++)
                transform.GetChild(i).localEulerAngles = Vector3.zero;
        }

        public void RandomRotate()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var angle = new Vector3(
                    Random.Range(_minAngle.x, _maxAngle.x),
                    Random.Range(_minAngle.y, _maxAngle.y),
                    Random.Range(_minAngle.z, _maxAngle.z));

                transform.GetChild(i).localEulerAngles = angle;
            }
        }
    }
}