using stickin.menus;
using UnityEngine;

namespace stickin.menus.type1
{
    public class SpinMenu : BaseMenu
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Spin _spin;

        [InjectField] private AppService _appService;

        private void Start()
        {
            _spin.OnStart += OnSpinStart;
            _spin.OnEnd += OnSpinEnd;
        }

        private void OnDestroy()
        {
            _spin.OnStart -= OnSpinStart;
            _spin.OnEnd -= OnSpinEnd;
        }

        private void OnSpinStart() => _canvasGroup.interactable = false;
        private void OnSpinEnd() => _canvasGroup.interactable = true;

        protected override void ShowStart()
        {
            base.ShowStart();
            
            InjectService.BindFields(this);
            var resourcesPrizesConfig = _appService.GameConfig.GetCustomConfig<ResourcesPrizesConfig>();

            if (resourcesPrizesConfig != null && resourcesPrizesConfig.Prizes != null)
                _spin.Init(resourcesPrizesConfig.Prizes);
            else
                Debug.LogError($"resourcesPrizesConfig is null or prizes is null. Check ResourcesPrizesConfig scriptable object");
        }
    }
}
