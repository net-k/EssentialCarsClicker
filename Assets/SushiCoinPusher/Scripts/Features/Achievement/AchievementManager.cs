using System;
using System.Collections.Generic;
using OTooleSoftware;
using SushiCatcher.Master;
using SushiCatcher.SaveData;
using UnityEngine;

namespace SushiCatcher
{
    public class AchievementManager
    {
        private AchievementMaster _achievementMaster;
        private StageMaster _stageMaster;
        public event Action<int> OnAchievementCleared;

        public AchievementManager(AchievementMaster achievementMaster, StageMaster stageMaster)
        {
            _achievementMaster = achievementMaster;
            _stageMaster = stageMaster;
        }

        public void Initialize()
        {
            if (PrizeCollectionManager.Instance != null)
            {
                // 多重登録を防ぐ
                PrizeCollectionManager.Instance.OnPrizeCountChanged += OnPrizeCountChanged;
            }
            else
            {
                Debug.LogWarning("[AchievementManager] PrizeCollectionManager.Instance is null. Event subscription failed. Check Initialization Order.");
            }
        }

        public void Uninitialize()
        {
            if (PrizeCollectionManager.Instance != null)
            {
                PrizeCollectionManager.Instance.OnPrizeCountChanged -= OnPrizeCountChanged;
            }
        }

        private void OnPrizeCountChanged(int prizeId, int count)
        {
            SaveProgress(prizeId);
        }

        public void SaveProgress(int prizeId)
        {
            // Debug.Log($"OnPrizeCountChanged: PrizeID={prize.id}, Count={count}");

            // プライズ獲得時に実績の進捗を保存する
            AchievementSaveDataManager.Instance.AddProgress(prizeId, 1);
            
            // achivement_master にて、target_id が prize.id の実績をすべてチェックする
            var achievements = _achievementMaster.GetDataByTargetId(prizeId);
            foreach (var achievement in achievements)
            {
                CheckAchievement(achievement.id);
            }
        }

        /// <summary>
        /// 実績の達成状況をチェックし、達成していればクリア状態にする
        /// </summary>
        /// <param name="achievementId"></param>
        private void CheckAchievement(int achievementId)
        {
            if (_achievementMaster == null) return;

            var data = _achievementMaster.FindById(achievementId);
            if (data == null) return;

            int currentProgress = AchievementSaveDataManager.Instance.LoadProgress(data.target_id);
            if (currentProgress >= data.goal_value)
            {
                if (!AchievementSaveDataManager.Instance.LoadCleared(achievementId))
                {
                    AchievementSaveDataManager.Instance.SaveCleared(achievementId, true);
                    Debug.Log($"Achievement Cleared: {data.title}");
                    // data.next_unlock_id を Unlock する
                    if (data.next_unlock_id != 0)
                    {
                        AchievementSaveDataManager.Instance.SaveUnlocked(data.next_unlock_id, true);
                        Debug.Log($"Achievement Unlocked: {data.next_unlock_id}");
                    }
                    OnAchievementCleared?.Invoke(achievementId);
                }
            }
        }

        public string GetAchievementTitleByAchievementId(int achievementId)
        {
            var data = _achievementMaster.FindById(achievementId);
            if (data == null) return "";

            return GetAchievementTitle(achievementId, data.target_id, data.goal_value);
        }

        public string GetAchievementTitle(int achievementId, int target_id,int goal_value)
        {
            string target_name_key = $"prize_id_{target_id}_name";
            string target_name = I2.Loc.LocalizationManager.GetTranslation(target_name_key);
           
            
            // Collect {goal_value} {target_name}.
            // こういうのが
            string title_key = "key_Achievement_Title_Type_1";
            string title_template = I2.Loc.LocalizationManager.GetTranslation(title_key);
            string title = title_template.FormatBy(
                new Dictionary<string, object>
                {
                    { "target_name", target_name },
                    { "goal_value", goal_value }
                }
            );

            return title;
        }

        public List<AchievementData> GetAchievementListUnlocked()
        {
            List<AchievementData> unlockedAchievements = new List<AchievementData>();

            // Unlockedなもの
            var unlockedIds = AchievementSaveDataManager.Instance.GetAllUnlockedAchievementIds();
            foreach (var id in unlockedIds)
            {
                var data = _achievementMaster.FindById(id);
                if (data != null && !unlockedAchievements.Contains(data))
                {
                    unlockedAchievements.Add(data);
                }
            }

            // Clearedなもの
            var clearedIds = AchievementSaveDataManager.Instance.GetAllClearedAchievementIds();
            foreach (var id in clearedIds)
            {
                var data = _achievementMaster.FindById(id);
                if (data != null && !unlockedAchievements.Contains(data))
                {
                    unlockedAchievements.Add(data);
                }
            }

            // 初期Unlockedなもの
            foreach (var data in _achievementMaster.GetUnlockedAchievements())
            {
                if (!unlockedAchievements.Contains(data))
                {
                    unlockedAchievements.Add(data);
                }
            }
            // stage_master の clear_unlock_achievement_id でアンロックされるもの
            var stages = _stageMaster.GetAllData();
            
            
            foreach (var stage in stages)
            {
                // ステージがクリアされているかどうかをチェック
                bool isCleared = StageSaveDataManager.Instance.LoadStageClearStatus(stage.stage_no);
                if (isCleared)
                {
                    var achievementId = stage.clear_unlock_achievement_id;
                    if (achievementId != 0)
                    {
                        var data = _achievementMaster.FindById(achievementId);
                        if (data != null && !unlockedAchievements.Contains(data))
                        {
                            unlockedAchievements.Add(data);
                        }
                    }
                }
            }
            
            unlockedAchievements.Sort((a, b) => a.sort_order.CompareTo(b.sort_order));
            
            return unlockedAchievements;
        }

        public bool IsAchievementCleared(int achievementId)
        {
            return AchievementSaveDataManager.Instance.LoadCleared(achievementId);
        }
    }
}
