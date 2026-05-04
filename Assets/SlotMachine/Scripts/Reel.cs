using System.Collections;
using UnityEngine;

namespace SlotMachine
{
    public class Reel : MonoBehaviour
    {
        [SerializeField] private float ResetPosition; // Inspectorでの設定値は無視し、定数を使用します
        [SerializeField] private SlotValue initialSlotValue = SlotValue.wall; // 初期表示する絵柄
        public SlotValue _slotValue;
        public bool _reelStop;
        private float ReelSpeed;
        private Vector3 StartPosition;
        
        // 1コマ0.9単位、4ステップで1コマ移動すると仮定
        private float reelStep = 0.225f; 

        // リールの設定定数
        private const float UNIT_HEIGHT = 0.9f;
        private const float REEL_LENGTH = UNIT_HEIGHT * 7.0f; // 6.3f (7絵柄分)
        
        // ズレ補正用のオフセット
        // 「1.4くらい下にずれている」とのことなので、全体を上に1.4上げる
        private const float OFFSET_Y = 1.4f;

        // ループの基準点（一番上の絵柄の位置）
        // 本来の2.7fにオフセットを加算
        private const float START_Y = 2.7f + OFFSET_Y; 
        private const float RESET_Y = START_Y - REEL_LENGTH; 

        private void Awake()
        {
            _slotValue = initialSlotValue; // 初期値を設定
            _reelStop = true;
            ReelSpeed = 0.05f;
            
            // ループ計算用の基準位置
            StartPosition = new Vector3(transform.position.x, START_Y, transform.position.z);
            
            // initialSlotValue に基づいて初期Y座標を決定する
            float initialY = GetYPositionFromSlotValue(initialSlotValue);
            transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
            
            ResetPosition = RESET_Y;
        }
        
        public void RellStart(int num)
        {
            _reelStop = false;
            StartCoroutine(RellRotate(num));
        }
        
        private IEnumerator RellRotate(int num)
        {
            for (int i = 0; i < num; i++)
            {
                float nextY = transform.position.y - reelStep;

                if (nextY <= ResetPosition)
                {
                    nextY += REEL_LENGTH;
                }

                transform.position = new Vector3(transform.position.x, nextY, transform.position.z);
                yield return new WaitForSeconds(ReelSpeed);
            }

            // 停止時に位置を吸着させる
            float finalY = transform.position.y;
            
            // ループ範囲内補正
            if (finalY <= ResetPosition + 0.01f)
            {
                finalY += REEL_LENGTH;
            }
            
            // オフセットを引いてから丸め、再度オフセットを足す
            float relativeY = finalY - OFFSET_Y;
            float roundedRelativeY = Mathf.Round(relativeY / UNIT_HEIGHT) * UNIT_HEIGHT;
            float roundedY = roundedRelativeY + OFFSET_Y;
            
            transform.position = new Vector3(transform.position.x, roundedY, transform.position.z);

            Result();
            _reelStop = true;
        }

        private void Result()
        {
            // Y座標からオフセットを引いて、0.9で割ってインデックス化
            float relativeY = transform.position.y - OFFSET_Y;
            int yIndex = Mathf.RoundToInt(relativeY / UNIT_HEIGHT);
            _slotValue = GetSlotValueFromIndex(yIndex);
        }

        private SlotValue GetSlotValueFromIndex(int index)
        {
            switch (index)
            {
                case 0: return SlotValue.wall;
                case -1: return SlotValue.dia;
                case -2: return SlotValue.shield;
                case -3: return SlotValue.key;
                case 3: return SlotValue.seven;
                case 2: return SlotValue.coin;
                case 1: return SlotValue.prize;
                default: return SlotValue.none;
            }
        }

        private float GetYPositionFromSlotValue(SlotValue value)
        {
            int index = GetIndexFromSlotValue(value);
            return (index * UNIT_HEIGHT) + OFFSET_Y;
        }

        public int GetIndexFromSlotValue(SlotValue value)
        {
            switch (value)
            {
                case SlotValue.wall: return 0;
                case SlotValue.dia: return -1;
                case SlotValue.shield: return -2;
                case SlotValue.key: return -3;
                case SlotValue.seven: return 3;
                case SlotValue.coin: return 2;
                case SlotValue.prize: return 1;
                default: return 0;
            }
        }

        // 現在の位置から指定した絵柄までのステップ数を計算する
        public int GetStepsToTarget(SlotValue targetValue)
        {
            // 現在のインデックスを取得
            float relativeY = transform.position.y - OFFSET_Y;
            int currentIndex = Mathf.RoundToInt(relativeY / UNIT_HEIGHT);

            // ターゲットのインデックスを取得
            int targetIndex = GetIndexFromSlotValue(targetValue);

            // インデックスの差分を計算
            // リールはYが減少する方向に回転するので、インデックスも減少する方向に進む
            // 例: 3 -> 2 -> 1 -> 0 -> -1 -> -2 -> -3 -> 3
            
            // 現在のインデックスを0-6の範囲に正規化（3がトップ、-3がボトム）
            // マッピング: 3->0, 2->1, 1->2, 0->3, -1->4, -2->5, -3->6
            int currentNormalized = 3 - currentIndex;
            int targetNormalized = 3 - targetIndex;

            // 必要な移動量（正規化されたインデックスでの差分）
            int diff = targetNormalized - currentNormalized;
            if (diff < 0) diff += 7; // 7絵柄分

            // 1コマあたり4ステップ
            return diff * 4;
        }
    }
}
