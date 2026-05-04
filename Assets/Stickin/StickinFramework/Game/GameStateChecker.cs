using System;
using System.Collections;
using stickin.menus;
using UnityEngine;

namespace stickin
{
    public class GameStateChecker : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private BaseMenu _winMenu;
        [SerializeField] private BaseMenu _loseMenu;
        [SerializeField] private BaseMenu _tutorialMenu;
        
        [Header("Ads")]
        [SerializeField] private bool _tryShowIntersitial = true;

        [InjectField] private AdsService _adsService;
        [InjectField] private AppService _appService;
        [InjectField] private ResourcesService _resourcesService;

        private GameView _gameView;

        private void Start()
        {
            InjectService.BindFields(this);
            
            _gameView = FindObjectOfType<GameView>();
            _gameView.OnChangeGameState += OnChangeGameState;
            
            StartCoroutine(CheckShowTutorial());
        }

        private IEnumerator CheckShowTutorial()
        {
            yield return new WaitForSeconds(0.3f);
            
            var countGamePlays = _resourcesService.GetResourceValueInt(UserData.GAME_PLAYS);
            countGamePlays++;
            _resourcesService.ChangeResource(UserData.GAME_PLAYS, 1);

            if (countGamePlays == 1)
            {
                var tutorial = _appService.GameConfig.GetCustomConfig<TutorialConfig>();
                var data = new Hashtable {["TutorialConfig"] = tutorial};
                MenusService.Show(_tutorialMenu, data);
            }
        }

        private void OnChangeGameState(GameStateType state, GameEndReasonType reason) => StartCoroutine(OnChangeGameStateCoroutine(state, reason));

        private IEnumerator OnChangeGameStateCoroutine(GameStateType state, GameEndReasonType reason)
        {
            yield return null;
            
            if (state == GameStateType.Win)
            {
                var hashtable = new Hashtable
                {
                    ["game"] = _gameView.Game
                };

                MenusService.Show(_winMenu, hashtable);
                TryShowInterstitial();
                
                _resourcesService.ChangeResource(ResourcesService.LevelKey, 1);
            }
            else if (state == GameStateType.Lose)
            {
                var data = new Hashtable
                {
                    ["reason"] = reason,
                    ["rewardCallback"] = (Action)RewardCompleteOnLose
                };
                MenusService.Show(_loseMenu, data);
                TryShowInterstitial();
            }
        }

        private void TryShowInterstitial()
        {
            if (_tryShowIntersitial)
                StartCoroutine(TryShowInterstitialCoroutine());
        }

        private IEnumerator TryShowInterstitialCoroutine()
        {
            yield return new WaitForSeconds(1f);
            
            _adsService.TryShowInterstitial();
        }

        private void RewardCompleteOnLose()
        {
            _gameView.Game.RewardCompleteOnLose();
        }
    }
}