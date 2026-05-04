using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class ButtonContinue : MonoBehaviour
    {
        [InjectField] private ResourcesService _resourcesService;
        [InjectField] private LevelsProgressService _levelsProgressService;

        private int _level;
        
        private void Start()
        {
            InjectService.BindFields(this);

            _level = _resourcesService.GetResourceValueInt(ResourcesService.LevelKey);
            
            var isExistProgress = _levelsProgressService.IsExistProgress(_level, LevelProgressType.Started);
            gameObject.SetActive(isExistProgress);
            
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameLauncher.PlayLevel(_level, OrderAssetType.Levels);
        }
    }
}