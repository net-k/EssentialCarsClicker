using System;
using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// 寿司クリッカーの主要なセーブデータ（バナナ数、プレステージ等）を管理するマネージャー
    /// </summary>
    public class SushiSaveDataManager : SingletonMonoBehaviour<SushiSaveDataManager>
    {
        private const string Key_BananaCount = "BananaCount";
        private const string Key_BananaTrillions = "BananaTrillions";
        private const string Key_BananaTotal = "BananaTotal";
        private const string Key_GoldBananas = "GoldBananas";
        private const string Key_PrestigeLevel = "PrestigeLevel";
        private const string Key_TickPerSec = "tickPerSec";
        private const string Key_BPerClick = "bPerClick";
        private const string Key_CloseTime = "closeTime";

        /// <summary>
        /// バナナ数（現在値）を保存
        /// </summary>
        public void SaveBananaCount(double count)
        {
            ES3.Save<double>(Key_BananaCount, count);
        }

        /// <summary>
        /// バナナ数（現在値）を読み込み
        /// </summary>
        public double LoadBananaCount(double defaultValue = 0)
        {
            return ES3.Load<double>(Key_BananaCount, defaultValue);
        }

        /// <summary>
        /// バナナ数（兆単位）を保存
        /// </summary>
        public void SaveBananaTrillions(double count)
        {
            ES3.Save<double>(Key_BananaTrillions, count);
        }

        /// <summary>
        /// バナナ数（兆単位）を読み込み
        /// </summary>
        public double LoadBananaTrillions(double defaultValue = 0)
        {
            return ES3.Load<double>(Key_BananaTrillions, defaultValue);
        }

        /// <summary>
        /// バナナ総獲得数を保存
        /// </summary>
        public void SaveBananaTotal(double count)
        {
            ES3.Save<double>(Key_BananaTotal, count);
        }

        /// <summary>
        /// バナナ総獲得数を読み込み
        /// </summary>
        public double LoadBananaTotal(double defaultValue = 0)
        {
            return ES3.Load<double>(Key_BananaTotal, defaultValue);
        }

        /// <summary>
        /// ゴールデンバナナ数を保存
        /// </summary>
        public void SaveGoldBananas(double count)
        {
            ES3.Save<double>(Key_GoldBananas, count);
        }

        /// <summary>
        /// ゴールデンバナナ数を読み込み
        /// </summary>
        public double LoadGoldBananas(double defaultValue = 0)
        {
            return ES3.Load<double>(Key_GoldBananas, defaultValue);
        }

        /// <summary>
        /// プレステージレベルを保存
        /// </summary>
        public void SavePrestigeLevel(double level)
        {
            ES3.Save<double>(Key_PrestigeLevel, level);
        }

        /// <summary>
        /// プレステージレベルを読み込み
        /// </summary>
        public double LoadPrestigeLevel(double defaultValue = 0)
        {
            return ES3.Load<double>(Key_PrestigeLevel, defaultValue);
        }

        /// <summary>
        /// 秒間バナナ生産量を保存
        /// </summary>
        public void SaveTickPerSec(double tick)
        {
            ES3.Save<double>(Key_TickPerSec, tick);
        }

        /// <summary>
        /// 秒間バナナ生産量を読み込み
        /// </summary>
        public double LoadTickPerSec(double defaultValue = 0)
        {
            return ES3.Load<double>(Key_TickPerSec, defaultValue);
        }

        /// <summary>
        /// クリック毎のバナナ生産量を保存
        /// </summary>
        public void SaveBananasPerClick(double amount)
        {
            ES3.Save<double>(Key_BPerClick, amount);
        }

        /// <summary>
        /// クリック毎のバナナ生産量を読み込み
        /// </summary>
        public double LoadBananasPerClick(double defaultValue = 1)
        {
            return ES3.Load<double>(Key_BPerClick, defaultValue);
        }

        /// <summary>
        /// アプリ終了時刻を保存
        /// </summary>
        public void SaveCloseTime(DateTime time)
        {
            ES3.Save<string>(Key_CloseTime, time.ToBinary().ToString());
        }

        /// <summary>
        /// アプリ終了時刻を読み込み
        /// </summary>
        public DateTime LoadCloseTime(DateTime defaultTime)
        {
            // ファイルが存在しない場合はデフォルト値を返す
            if (!ES3.FileExists())
            {
                return defaultTime;
            }
            
            string closeTimeStr = ES3.Load<string>(Key_CloseTime, defaultValue: "");
            if (string.IsNullOrEmpty(closeTimeStr))
            {
                return defaultTime;
            }
            try
            {
                long temp = Convert.ToInt64(closeTimeStr);
                return DateTime.FromBinary(temp);
            }
            catch
            {
                return defaultTime;
            }
        }

        /// <summary>
        /// セーブデータが存在するかどうか
        /// </summary>
        public bool HasSaveData()
        {
            return ES3.KeyExists(Key_CloseTime);
        }

        /// <summary>
        /// 全データの削除
        /// </summary>
        public void DeleteAllData()
        {
            ES3.DeleteKey(Key_BananaCount);
            ES3.DeleteKey(Key_BananaTrillions);
            ES3.DeleteKey(Key_BananaTotal);
            ES3.DeleteKey(Key_GoldBananas);
            ES3.DeleteKey(Key_PrestigeLevel);
            ES3.DeleteKey(Key_TickPerSec);
            ES3.DeleteKey(Key_BPerClick);
            ES3.DeleteKey(Key_CloseTime);
        }

        /// <summary>
        /// プレステージ時のデータ削除（一部データを保持）
        /// </summary>
        public void DeleteDataForPrestige()
        {
            ES3.DeleteKey(Key_BananaCount);
            ES3.DeleteKey(Key_BananaTrillions);
            ES3.DeleteKey(Key_BananaTotal);
            // PrestigeLevel, GoldBananas は削除しない
            ES3.DeleteKey(Key_TickPerSec);
            ES3.DeleteKey(Key_BPerClick);
            ES3.DeleteKey(Key_CloseTime);
        }
    }
}
