using SlotMachine.Scripts;
using UnityEngine;

namespace SlotCoinPusher
{
    /// <summary>
    /// 前後に移動する
    /// </summary>
    public class SlotTriggerBox : MonoBehaviour
    {
        private float _moveSpeed = 0.2f;
        [SerializeField] private float _moveRange = 2f;

        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 _targetPos;

        private void Start()
        {
            // 現在位置を中心とする
            Vector3 centerPosition = transform.position;
            _startPos = centerPosition - new Vector3(0, 0, _moveRange / 2f);
            _endPos = centerPosition + new Vector3(0, 0, _moveRange / 2f);
            
            _targetPos = _endPos;
        }

        private void Update()
        {
            // ターゲットポジションに向かって移動
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);

            // ターゲットポジションに十分に近づいたら、目的地を反転させる
            if (Vector3.Distance(transform.position, _targetPos) < 0.01f)
            {
                // 現在のターゲットが終点なら始点に、始点なら終点に設定する
                _targetPos = (_targetPos == _endPos) ? _startPos : _endPos;
            }
        }
    }
}
