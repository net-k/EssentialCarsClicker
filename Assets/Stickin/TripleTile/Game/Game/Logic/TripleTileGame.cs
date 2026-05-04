using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace stickin.tripletile
{
    public class TripleTileGame : Game
    {
        private readonly LevelModel _levelModel;
        private readonly IGameView _view;
        
        private int _layersCount;
        private TripleTileGameConfig _config;
        private RewardResourceModule _rewardResourceModule;
        
        public List<CellGameModel> Cells => _levelModel.Cells;
        
        public List<CellGameModel> CollectedCells;

        public event Action<CellGameModel, int> OnCollectCell;
        public event Action<CellGameModel> OnRemoveCell;
        public event Action<List<CellGameModel>> OnRefreshCellsOrders;
        public event Action OnPreShuffle;

        public TripleTileGame(LevelModel levelModel, TripleTileGameConfig config, IGameView view)
        {
            _config = config;
            _levelModel = levelModel;
            _view = view;
            
            _levelModel.Init();
            Init();

            _rewardResourceModule = new RewardResourceModule(ResourcesService.CoinKey, 0);
            RegistrGameModule(_rewardResourceModule);
            
            _view.Init(this, _config, _rewardResourceModule);
        }

        public void ClickCell(CellGameModel cell)
        {
            if (_isLocked)
                return;
            
            if (cell.IsAvailable)
            {
                cell.OnClick -= ClickCell;
                
                Cells.Remove(cell);
                AddedCollectedCell(cell);
                
                // cell.Collect();
                
                RefreshCellsAvailable();
            }
            else
            {
                // cell.Wrong();
                // OnWrongCell?.Invoke(cell);
            }
        }

        private async Task AddedCollectedCell(CellGameModel cell)
        {
            if (_isLocked)
                return;

            LockedTouch(true);
            var insertIndex = CollectedCells.Count;
            var countEquals = 0;
            
            for (var i = 0; i < CollectedCells.Count; i++)
            {
                if (CollectedCells[i].Type == cell.Type)
                {
                    insertIndex = i + 1;
                    countEquals++;
                }
            }
            
            countEquals++;
            CollectedCells.Insert(insertIndex, cell);

            OnCollectCell?.Invoke(cell, insertIndex);

            if (countEquals >= 3)
            {
                await Delay(0.2f);

                for (var i = insertIndex - 2; i <= insertIndex; i++)
                {
                    OnRemoveCell?.Invoke(CollectedCells[i]);
                    await Delay(0.1f);
                }
                
                for (var i = insertIndex; i >= insertIndex - 2; i--)
                    CollectedCells.RemoveAt(i);

                await Delay(0.2f);
                OnCollectCell?.Invoke(null, 0);
            }
            
            if (CollectedCells.Count >= _config.MaxCountSelectedCells)
                EndGame(GameStateType.Lose);
            else if (Cells.Count <= 0)
                EndGame(GameStateType.Win);
            
            LockedTouch(false);
        }

        private void Init()
        {
            if (Cells.Count % 3 != 0)
                Debug.LogError("cells count not divide 3");
            
            foreach (var cell in Cells)
                cell.OnClick += ClickCell;
            
            CollectedCells = new List<CellGameModel>();
            
            _layersCount = 0;
            foreach (var cell in Cells)
                _layersCount = Mathf.Max(_layersCount, cell.Layer);

            RefreshCellsOrder();
            RefreshCellsAvailable();
        }

        private void RefreshCellsOrder()
        {
            Cells.Sort();
            OnRefreshCellsOrders?.Invoke(Cells);
        }

        private void RefreshCellsAvailable()
        {
            foreach (var cell in Cells)
                cell.SetAvailable(CanSelectedTile(cell));
        }
        
        private bool CanSelectedTile(CellGameModel checkCell)
        {
            for (var i = checkCell.Layer + 1; i <= _layersCount; i++)
            {
                var tiles = GetTileForDistance(checkCell.Index, i, 1, 1, checkCell);
                if (tiles.Count > 0)
                    return false;
            }

            // for mahjong
            // var leftTiles = GetTileForDistance(checkCell.Index + Vector2Int.left * 2, checkCell.Layer, 0, 1, checkCell);
            // var rightTiles = GetTileForDistance(checkCell.Index + Vector2Int.right * 2, checkCell.Layer, 0, 1, checkCell);
            // if (leftTiles.Count >= 1 && rightTiles.Count >= 1)
            //     return false;

            return true;
        }

        private List<CellGameModel> GetTileForDistance(Vector2Int position, int layer, int distanceX, int distanceY, CellGameModel ignoreTile)
        {
            var result = new List<CellGameModel>();
        
            foreach (var cell in Cells)
            {
                if (cell == ignoreTile)
                    continue;
            
                if (cell.Layer == layer &&
                    Mathf.Abs(cell.Index.x - position.x) <= distanceX &&
                    Mathf.Abs(cell.Index.y - position.y) <= distanceY)
                {
                    result.Add(cell);
                }
            }

            return result;
        }

        protected override void InitHints()
        {
            _hints = new List<Hint>()
            {
                new LampHint(),
                new ShuffleHint()
            };
        }

        public async Task ShuffleBoard()
        {
            if (_isLocked)
                return;
            
            LockedTouch(true);
            OnPreShuffle?.Invoke();
            
            await Delay(0.5f);

            var indexes = new List<Vector3Int>();
            foreach (var cell in Cells)
                indexes.Add(new Vector3Int(cell.Index.x, cell.Index.y, cell.Layer));

            indexes.Shuffle();

            for (var i = 0; i < indexes.Count; i++)
            {
                Cells[i].Layer = indexes[i].z;
                Cells[i].ChangeIndex(new Vector2Int(indexes[i].x, indexes[i].y));
            }

            RefreshCellsAvailable();
            RefreshCellsOrder();
            
            await Delay(0.5f);
            LockedTouch(false);
        }

        public override void SimulateEndGame()
        {
            base.SimulateEndGame();
            
            EndGame(GameStateType.Win);
        }
    }
}