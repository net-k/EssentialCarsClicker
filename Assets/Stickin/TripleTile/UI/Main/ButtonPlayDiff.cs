using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class ButtonPlayDiff : MonoBehaviour
    {
        [SerializeField] private Transform _selectDifficultMenuPosition;

        [InjectField] private ResourcesService _resourcesService;
        [InjectField] private LevelsProgressService _levelsProgressService;
        [InjectField] private AppService _appService;

        private void Start()
        {
            InjectService.BindFields(this);
            
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            MenusService.Show<SelectDifficultMenu>(new Hashtable
            {
                ["difficults"] = _appService.GameConfig.GetCustomConfig<DifficultsConfig>(),
                ["position"] = _selectDifficultMenuPosition.position,
                ["callback"] = (Action<int>)PlayNew
            });
        }
        
        public void PlayNew(int difficultNumber)
        {
            var level = _resourcesService.GetResourceValueInt(ResourcesService.LevelKey);
            _levelsProgressService.ResetProgress(level);
            
            var data = new Hashtable {["difficult"] = difficultNumber};
            GameLauncher.PlayLevel(level, OrderAssetType.Levels, data);
        }
    }
}