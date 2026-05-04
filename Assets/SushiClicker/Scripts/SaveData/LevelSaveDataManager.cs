using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// 建物・アップグレードのレベルデータ（所持数・コスト・アンロック状態）を永続化するマネージャー
    /// </summary>
    public class LevelSaveDataManager : SingletonMonoBehaviour<LevelSaveDataManager>
    {
        private readonly string RecordType_Count = "Level_Count_";
        private readonly string RecordType_Cost = "Level_Cost_";
        private readonly string RecordType_IsUnlocked = "Level_IsUnlocked_";

        /// <summary>
        /// 所持数を保存する
        /// </summary>
        public void SaveCount(string itemName, long count)
        {
            ES3.Save<long>(RecordType_Count + itemName, count);
        }

        /// <summary>
        /// 所持数を読み込む
        /// </summary>
        public long LoadCount(string itemName)
        {
            return ES3.Load<long>(RecordType_Count + itemName, 0L);
        }

        /// <summary>
        /// コストを保存する
        /// </summary>
        public void SaveCost(string itemName, double cost)
        {
            ES3.Save<double>(RecordType_Cost + itemName, cost);
        }

        /// <summary>
        /// コストを読み込む。未保存の場合は defaultCost を返す
        /// </summary>
        public double LoadCost(string itemName, double defaultCost)
        {
            return ES3.Load<double>(RecordType_Cost + itemName, defaultCost);
        }

        /// <summary>
        /// アンロック状態を保存する
        /// </summary>
        public void SaveIsUnlocked(string itemName, bool isUnlocked)
        {
            ES3.Save<bool>(RecordType_IsUnlocked + itemName, isUnlocked);
        }

        /// <summary>
        /// アンロック状態を読み込む
        /// </summary>
        public bool LoadIsUnlocked(string itemName)
        {
            return ES3.Load<bool>(RecordType_IsUnlocked + itemName, false);
        }
    }
}
