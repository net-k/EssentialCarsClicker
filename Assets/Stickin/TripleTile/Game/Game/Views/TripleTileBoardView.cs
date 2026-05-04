using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace stickin.tripletile
{
    public class TripleTileBoardView : MonoBehaviour
    {
        [SerializeField] private TripleTileBoardCell _cellPrefab;
        
        private RectTransform _rt;
        private TripleTileGame _game;
        private List<TripleTileBoardCell> _cells;

        public void Init(TripleTileGame game, TripleTileGameConfig config)
        {
            _game = game;
            _rt = this.RectTransform();

            game.OnRefreshCellsOrders += OnRefreshCellsOrders;
            game.OnPreShuffle += OnPreShuffle;
            
            var indexes = new List<Vector2Int>();
            foreach (var cellGameModel in game.Cells)
                indexes.Add(cellGameModel.Index);

            _cells = RectBoardExtensions.AddedCells(indexes, _cellPrefab, _rt, xPosMultuplier: 0.5f, yPosMultuplier: 0.5f);

            for (var i = 0; i < _cells.Count; i++)
                _cells[i].Init(game.Cells[i], config.Sprites);

            // _rt.ResizeInParent();
        }

        private void OnPreShuffle()
        {
            foreach (var cellGameModel in _game.Cells)
            {
                var cell = GetCell(cellGameModel);

                if (cell != null)
                {
                    var cellRt = cell.RectTransform();
                    var newPos = (cellRt.anchoredPosition - _rt.sizeDelta / 2) * Random.Range(2f, 4f) + _rt.sizeDelta / 2;

                    cellRt.DOAnchorPos(newPos, 0.5f);
                }
            }
        }

        private void OnRefreshCellsOrders(List<CellGameModel> cellGameModels)
        {
            for(var i = 0; i < cellGameModels.Count; i++)
            {
                var cell = GetCell(cellGameModels[i]);
                if (cell != null)
                    // cell.transform.SetSiblingIndex(i);
                    cell.transform.SetAsLastSibling();
            }
        }

        public TripleTileBoardCell GetCell(CellGameModel cellGameModel)
        {
            foreach (var cell in _cells)
            {
                if (cell.Model == cellGameModel)
                    return cell;
            }

            return null;
        }
    }
}