using System;
using SushiCoinPusher.Features.Collection.SushiSlot;
using UnityEngine;
using UnityEngine.UI;

namespace SushiCoinPusher.Features.Collection
{
    public class SushiCollectionView : MonoBehaviour
    {
        [SerializeField]
        private Button _backButton;
        public Button BackButton => _backButton;

        [Header("Dynamic Content")]
        [Tooltip("スロットを生成する親となるRectTransform（ScrollViewのContentなど）")]
        [SerializeField]
        private RectTransform _contentRoot;

        [Tooltip("生成するスロットのPrefab")]
        [SerializeField]
        private SushiSlotView _sushiSlotPrefab;


        /// <summary>
        /// 新しい寿司スロットを生成して返します。
        /// </summary>
        /// <returns>生成されたSushiSlotViewのインスタンス</returns>
        public SushiSlotView CreateSlot()
        {
            if (_sushiSlotPrefab == null || _contentRoot == null)
            {
                Debug.LogError("SushiSlotPrefab or ContentRoot is not assigned in SushiCollectionView.");
                return null;
            }
            var slotInstance = Instantiate(_sushiSlotPrefab, _contentRoot);
            return slotInstance;
        }

        /// <summary>
        /// 全てのスロットを削除します。
        /// </summary>
        public void ClearSlots()
        {
            if (_contentRoot == null) return;
            
            foreach (Transform child in _contentRoot)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
