using UnityEngine;
using Zenject;

namespace SushiCatcher.Achievement.AchievementNotificationDialog
{
    public class AchievementNotificationDialogPresenter : MonoBehaviour
    {
        [SerializeField]
        AchievementNotificationDialogView _view;
        AchievementManager _achievementManager;
        
        [Inject]
        void Construct(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }

        public void Initialize(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            if (_view != null)
            {
                _view.Hide();
            }
        }

        public void Show(int achievementId, System.Action onClose = null)
        {
            if (_view == null) return;

            string achievementTitle = _achievementManager.GetAchievementTitleByAchievementId(achievementId);
            
            _view.SetTitle("Achievement Unlocked!");
            _view.SetMessage(achievementTitle);
            
            _view.SetOnOkButtonClicked(() => 
            {
                _view.Hide();
                onClose?.Invoke();
            });
            
            _view.Show();
        }
    }
}
