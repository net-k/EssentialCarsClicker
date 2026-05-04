using SushiCatcher.SaveData;
using UnityEngine;

namespace SushiCatcher.StageButton
{
    public class LockPresenter : MonoBehaviour
    {
        [SerializeField]
        private LockView _lockView;

        public void Initialize(int stageId)
        {
            bool isUnlocked = StageSaveDataManager.Instance.LoadStageUnlockStatus(stageId);
            _lockView.SetLockState(!isUnlocked);
        }
    }
}
