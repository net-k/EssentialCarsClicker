using UnityEngine;

namespace stickin
{
    public class DestroyOnStart : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject);
        }
    }
}