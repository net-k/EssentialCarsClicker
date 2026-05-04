using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quiz.Infrastructure;
using UnityEngine;

namespace SushiCatcher
{
    public class AchievementSaveDataManager : SingletonMonoBehaviour<AchievementSaveDataManager>
    {
        private readonly string RecordType_AchievementProgressList = "AchievementProgressList";
        private readonly string RecordType_AchievementUnlocked = "AchievementUnlocked_";
        private readonly string RecordType_AchievementCleared = "AchievementCleared_";

        [System.Serializable]
        public class AchievementProgressData
        {
            public int TargetId1;
            public int Progress1;
        }
        
        /// <summary>
        /// 実績の進捗リストを保存する
        /// </summary>
        public void SaveAchievementProgressList(List<AchievementProgressData> list)
        {
            ES3.Save(RecordType_AchievementProgressList, list);
        }

        /// <summary>
        /// 実績の進捗リストを読み込む
        /// </summary>
        public List<AchievementProgressData> LoadAchievementProgressList()
        {
            if (ES3.KeyExists(RecordType_AchievementProgressList))
            {
                return ES3.Load<List<AchievementProgressData>>(RecordType_AchievementProgressList);
            }
            return new List<AchievementProgressData>();
        }
        
        /// <summary>
        /// 実績の進捗データを保存する（リストに追加または更新）
        /// </summary>
        public void SaveProgress(AchievementProgressData data)
        {
            var list = LoadAchievementProgressList();
            var index = list.FindIndex(x => x.TargetId1 == data.TargetId1);
            
            if (index >= 0)
            {
                list[index] = data;
            }
            else
            {
                list.Add(data);
            }
            
            SaveAchievementProgressList(list);
        }

        /// <summary>
        /// 実績の進捗データを読み込む
        /// </summary>
        public AchievementProgressData LoadProgressData(int targetId)
        {
            var list = LoadAchievementProgressList();
            return list.FirstOrDefault(x => x.TargetId1 == targetId);
        }

        /// <summary>
        /// 実績の進捗を加算する（簡易版：Progress1を加算）
        /// </summary>
        public void AddProgress(int targetId, int amount)
        {
            var list = LoadAchievementProgressList();
            var data = list.FirstOrDefault(x => x.TargetId1 == targetId);

            if (data == null)
            {
                // ログ
                Debug.Log($"[AchievementSaveDataManager] AddProgress: No existing data for TargetID={targetId}. Creating new entry.");
                data = new AchievementProgressData
                {
                    TargetId1 = targetId,
                    Progress1 = 0
                };
                list.Add(data);
            }
            
            data.Progress1 += amount;
            Debug.Log($"[AchievementSaveDataManager] AddProgress: TargetID={targetId}, Amount={amount}, NewTotal={data.Progress1}");
            SaveAchievementProgressList(list);
        }

        /// <summary>
        /// 実績の進捗を読み込む（簡易版：Progress1を返す）
        /// </summary>
        public int LoadProgress(int targetId)
        {
            var data = LoadProgressData(targetId);
            return data != null ? data.Progress1 : 0;
        }

        /// <summary>
        /// 実績がアンロック（チャレンジ可能）されたかを保存する
        /// </summary>
        public void SaveUnlocked(int achievementId, bool unlocked)
        {
            var key = RecordType_AchievementUnlocked + achievementId;
            ES3.Save<bool>(key, unlocked);
        }

        /// <summary>
        /// 実績がアンロック（チャレンジ可能）されているかを読み込む
        /// </summary>
        public bool LoadUnlocked(int achievementId)
        {
            var key = RecordType_AchievementUnlocked + achievementId;
            if (!ES3.KeyExists(key)) return false;
            return ES3.Load<bool>(key);
        }

        /// <summary>
        /// 実績がクリアされたかを保存する
        /// </summary>
        public void SaveCleared(int achievementId, bool cleared)
        {
            var key = RecordType_AchievementCleared + achievementId;
            ES3.Save<bool>(key, cleared);
        }

        /// <summary>
        /// 実績がクリアされているかを読み込む
        /// </summary>
        public bool LoadCleared(int achievementId)
        {
            var key = RecordType_AchievementCleared + achievementId;
            if (!ES3.KeyExists(key)) return false;
            return ES3.Load<bool>(key);
        }

        public IEnumerable<int> GetAllUnlockedAchievementIds()
        {
            string[] keys;
            try
            {
                keys = ES3.GetKeys();
            }
            catch (System.IO.FileNotFoundException)
            {
                yield break;
            }

            foreach (var key in keys)
            {
                if (key.StartsWith(RecordType_AchievementUnlocked))
                {
                    bool unlocked = ES3.Load<bool>(key);
                    if (unlocked)
                    {
                        string idStr = key.Substring(RecordType_AchievementUnlocked.Length);
                        if (int.TryParse(idStr, out int achievementId))
                        {
                            yield return achievementId;
                        }
                    }
                }
            }
        }

        public IEnumerable<int> GetAllClearedAchievementIds()
        {
            string[] keys;
            try
            {
                keys = ES3.GetKeys();
            }
            catch (System.IO.FileNotFoundException)
            {
                yield break;
            }

            foreach (var key in keys)
            {
                if (key.StartsWith(RecordType_AchievementCleared))
                {
                    bool cleared = ES3.Load<bool>(key);
                    if (cleared)
                    {
                        string idStr = key.Substring(RecordType_AchievementCleared.Length);
                        if (int.TryParse(idStr, out int achievementId))
                        {
                            yield return achievementId;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存されているAchievementProgressDataを読みやすい文字列で返す（デバッグ用）
        /// </summary>
        public string GetAchievementProgressDebugString()
        {
            var list = LoadAchievementProgressList();
            if (list == null || list.Count == 0) return "No Achievement Progress Data";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--- Achievement Progress Data ---");
            foreach (var data in list)
            {
                sb.AppendLine($"TargetID: {data.TargetId1}, Progress: {data.Progress1}");
            }
            return sb.ToString();
        }
    }
}
