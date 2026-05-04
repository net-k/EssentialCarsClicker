using UnityEngine;

namespace SushiCatcher.Achievement.Progress
{
    public class ProgressPresenter : MonoBehaviour
    {
        [SerializeField]
        private ProgressView _view;

        public void Setup(int currentProgress, int maxProgress)
        {
            // 進捗が最大値を超えないようにする
            if (currentProgress > maxProgress)
            {
                currentProgress = maxProgress;
            }

            _view.SetProgress(currentProgress, maxProgress);
        }
    }
}