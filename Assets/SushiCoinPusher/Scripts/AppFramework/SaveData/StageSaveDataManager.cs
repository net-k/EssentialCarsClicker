using UnityEngine;

namespace SushiCatcher.SaveData
{
    public class StageSaveDataManager : SingletonMonoBehaviour<StageSaveDataManager>
    {
        // Stage Unlock Logic
        private const string StageUnlockKeyBase = "StageUnlock";
        private const string StageClearKeyBase = "StageClear";

        private string GetStageUnlockKey(int stageId)
        {
            return $"{StageUnlockKeyBase}_{stageId}";
        }

        public bool LoadStageUnlockStatus(int stageId)
        {
            // Stage 1 is default unlocked
            if (stageId == 1) return true;
            return ES3.Load<bool>(GetStageUnlockKey(stageId), false);
        }

        public void SaveStageUnlockStatus(int stageId, bool isUnlocked)
        {
            ES3.Save<bool>(GetStageUnlockKey(stageId), isUnlocked);
        }

        private string GetStageClearKey(int stageId)
        {
            return $"{StageClearKeyBase}_{stageId}";
        }

        public void SaveStageClearStatus(int stageId, bool isCleared)
        {
            ES3.Save<bool>(GetStageClearKey(stageId), isCleared);
        }

        public bool LoadStageClearStatus(int stageId)
        {
            return ES3.Load<bool>(GetStageClearKey(stageId), false);
        }
    }
}
