using System.Collections;
using System.Collections.Generic;
using SlotMachine.Scripts;
using UnityEngine;

namespace SlotCoinPusher
{
    public class SlotSpinQueue : MonoBehaviour
    {
        [SerializeField] private SlotController slotController;
        [SerializeField] private float delayBeforeNextSpin = 2.0f; // 次のスピンまでの待機時間
        private readonly Queue<int> _spinQueue = new Queue<int>();
        private bool _isSlotSpinning;

        private void Start()
        {
            if (slotController != null)
            {
                slotController.OnSpinEnd += HandleSpinEnd;
            }
        }

        private void OnDestroy()
        {
            if (slotController != null)
            {
                slotController.OnSpinEnd -= HandleSpinEnd;
            }
        }

        public void AddSpinRequest()
        {
            _spinQueue.Enqueue(1);
            TryProcessQueue();
        }

        private void TryProcessQueue()
        {
            if (_isSlotSpinning || _spinQueue.Count <= 0) return;
            
            _spinQueue.Dequeue();
            
            _isSlotSpinning = true;
            slotController.Spin();
        }

        private void HandleSpinEnd()
        {
            StartCoroutine(HandleSpinEndRoutine());
        }

        private IEnumerator HandleSpinEndRoutine()
        {
            // スピン終了後、少し待機してから次のスピンを受け付ける
            // 待機中も _isSlotSpinning は true のままにしておくことで、
            // 待機中に AddSpinRequest が来ても即座にスピンが始まらないようにする
            
            yield return new WaitForSeconds(delayBeforeNextSpin);

            _isSlotSpinning = false;
            TryProcessQueue();
        }
    }
}
