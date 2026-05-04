
using UnityEngine;

namespace stickin.tripletile
{
    public class TripleTileGameView : GameView, IGameView
    {
        [SerializeField] private TripleTileBoardView _board;
        [SerializeField] private CollectedView _collectedView;

        [InjectField] private ResourcesService _resourcesService;
        
        public override void InitWithLevelNumber(GameParams gameParams)
        {
// #if UNITY_EDITOR
//             gameParams.LevelNumber = Random.Range(10, 30);
// #endif
            base.InitWithLevelNumber(gameParams);

            var gameConfig = _gameConfig as TripleTileGameConfig;
            var levelModel = gameConfig.GetLevelModel<LevelModel>(gameParams.LevelNumber, gameParams.OrderType);
            _game = new TripleTileGame(levelModel, _gameConfig as TripleTileGameConfig, this);
        }

        public void Init(TripleTileGame game, TripleTileGameConfig config, RewardResourceModule rewardResourceModule)
        {
            rewardResourceModule.SetResourcesService(_resourcesService);
            
            game.OnCollectCell += OnCollectCell;
            game.OnRemoveCell += OnRemoveCell;
            
            _board.Init(game, config);
            _collectedView.Init(config.MaxCountSelectedCells, rewardResourceModule);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_game != null)
            {
                var game = _game as TripleTileGame;
                
                game.OnCollectCell -= OnCollectCell;
                game.OnRemoveCell -= OnRemoveCell;
            }
        }

        private void OnRemoveCell(CellGameModel cellGameModel)
        {
            _collectedView.RemoveCell(cellGameModel);
        }

        private void OnCollectCell(CellGameModel cellGameModel, int index)
        {
            var cell = _board.GetCell(cellGameModel);

            if (cell != null)
                _collectedView.AddedCell(cell, index);
            else
                _collectedView.RefreshCellsPositions();
        }
    }
}
