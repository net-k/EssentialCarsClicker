using UnityEngine;

namespace SlotCoinPusher
{
    [RequireComponent(typeof(BoxCollider))]
    public class SlotTriggerBoxCollider : MonoBehaviour
    {
        [SerializeField] private SlotSpinQueue slotSpinQueue;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Coin")) return;
            
            slotSpinQueue.AddSpinRequest();
            Destroy(other.gameObject);
        }
    }
}
