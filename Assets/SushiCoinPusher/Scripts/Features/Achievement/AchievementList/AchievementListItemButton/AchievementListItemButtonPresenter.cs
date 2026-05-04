using SushiCatcher.Achievement.AchievementListItemButton;
using SushiCatcher.Achievement.Progress;
using UnityEngine;

namespace SushiCatcher.Achievement.AchievementList.AchievementListItemButton
{
    public class AchievementListItemButtonPresenter : MonoBehaviour
    {
        [SerializeField]
        AchievementListItemButtonView _view;
    
        [SerializeField]
        ProgressPresenter _progressPresenter;

        public void Initialize(string achievementTitle, bool isCleared, int achievementId, int currentValue, int goalValue)
        {
            _view.SetText( achievementTitle);
            _view.SetCleared(isCleared);
            if (_progressPresenter != null)
            {
                _progressPresenter.Setup(currentValue, goalValue);
            }
        }
    }
}
