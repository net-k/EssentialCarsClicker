using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class ButtonPlay : MonoBehaviour
    {
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
            var level = _resourcesService.GetResourceValueInt(ResourcesService.LevelKey);
            _levelsProgressService.ResetProgress(level);

            GameLauncher.PlayLevel(level, OrderAssetType.Levels);
        }
    }
}