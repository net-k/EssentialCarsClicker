using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace stickin.tripletile
{
    public class CollectedView : MonoBehaviour
    {
        [SerializeField] private TripleTileBoardCell _cellPrefab;
        [SerializeField] private RectTransform _contentRt;
        
        private List<TripleTileBoardCell> _cells = new ();
        private RectTransform _rt;
        private RewardResourceModule _rewardResourceModule;
        
        public void Init(int maxCount, RewardResourceModule rewardResourceModule)
        {
            _rewardResourceModule = rewardResourceModule;
            _rt = this.RectTransform();
            var cellSize = _cellPrefab.RectTransform().rect.size;
            
            _contentRt.sizeDelta = new Vector2(cellSize.x * maxCount, cellSize.y);
            _contentRt.ResizeRTinRT(_rt);

            // _rt
        }

        public void AddedCell(TripleTileBoardCell cell, int index)
        {
            _cells.Insert(index, cell);
            cell.transform.SetParent(_contentRt);

            RefreshCellsPositions();
        }

        public void RemoveCell(CellGameModel cellGameModel)
        {
            foreach (var cell in _cells)
            {
                if (cell.Model == cellGameModel)
                {
                    cell.Remove();
                    _cells.Remove(cell);
                    _rewardResourceModule.IncResource(1, cell.transform);
                    break;
                }
            }
        }

        public void RefreshCellsPositions()
        {
            for(var i = 0; i < _cells.Count; i++)
            {
                var cellRt = _cells[i].RectTransform();
                var pos = new Vector2((i + 0.5f) * cellRt.rect.width, cellRt.rect.height / 2f);

                cellRt.DOKill();
                cellRt.DOAnchorPos(pos, 0.2f);
                cellRt.DOScale(Vector3.one, 0.2f);
            }
        }
    }
}