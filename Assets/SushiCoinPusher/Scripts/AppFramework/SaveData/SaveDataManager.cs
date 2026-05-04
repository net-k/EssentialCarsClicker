using System;
using System.Collections.Generic;
using Quiz.Infrastructure;
using UnityEngine;

namespace App
{
    public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
    {
        private readonly int SaveDataVersion = 1;
        private readonly int InitialRating = 1000;
        private readonly string RecordType_SaveDataVersion = "SaveDataVersion";
        private readonly string RecordType_StageScore = "StageScoreKey";
        private readonly string RecordType_StageUnlock = "StageUnlockKey"; // ステージがアンロックされているか？
        private readonly string RecordType_Life = "Life";
        private readonly string RecordType_LifeLastRecoverTime = "LifeLastRecoverTime";
        private readonly string RecordType_Coin = "Coin";

        private readonly string RecordType_Records = "Records";

        private readonly string RecordType_OnlineBattleNum = "key_OnlineBattleNum";
        private readonly string RecordType_OnlineWinCount = "key_OnlineWinCount";
        private readonly string RecordType_OnlineLoseCount = "key_OnlineLoseCount";
        private readonly string RecordType_OnlineDrawCount = "key_OnlineDrawCount";
        private readonly string RecordType_OnlineRating = "key_OnlineRating";

        private readonly string RecordType_OfflineBattleNum = "key_OfflineBattleNum";
        private readonly string RecordType_OfflineWinCount = "key_OfflineWinCount";
        private readonly string RecordType_OfflineLoseCount = "key_OfflineLoseCount";
        private readonly string RecordType_OfflineDrawCount = "key_OfflineDrawCount";

        struct Record
        {
            public float timeUnitSec;
            private int inputCharacterCount;

            Record(float t, int c)
            {
                timeUnitSec = t;
                inputCharacterCount = c;
            }

            //    public GetInputCharacterPerTimeMin;
        }

        private void Awake()
        {
        }

        public void Initialize()
        {
            if (!ES3.KeyExists(RecordType_SaveDataVersion))
            {
                CreateInitialData_SaveDataVersion1();
            }
        }

        private void CreateInitialData_SaveDataVersion1()
        {
            SaveLife(GameConstants.LifeMaxNum);
            SaveLifeLastRecoverTime(DateTime.Now);
            ES3.Save(RecordType_SaveDataVersion, 1);
            SaveOnlineRating(InitialRating);
            SaveCoin(GameConstants.InitialCoinNum);
        }

        public void SaveScore(string category, int stageNo, int score)
        {
            var key = RecordType_StageScore + $"{stageNo}-{category}";

            ES3.Save<int>(key, score);
        }

        public int LoadScore(string category, int stageNo)
        {
            var key = RecordType_StageScore + $"{stageNo}-{category}";
            if (!ES3.KeyExists(key)) return -1;

            return ES3.Load<int>(key);
        }

        public void SaveUnlock(string category, int stageNo)
        {
            var key = RecordType_StageUnlock + $"{stageNo}-{category}";
            ES3.Save<int>(key, 1);
        }


        public bool LoadUnlock(string category, int stageNo)
        {
            var key = RecordType_StageUnlock + $"{stageNo}-{category}";
            if (!ES3.KeyExists(key)) return false;
            var value = ES3.Load<int>(key);
            return Convert.ToBoolean(value);
        }

        public int LoadBestScore(string category, int stage)
        {
            return LoadScore(category, stage);
        }

        public void SaveBestScore(string category, int stage, int playerCorrectNum)
        {
            SaveScore(category, stage, playerCorrectNum);
        }

        public int LoadLife()
        {
            if (!ES3.KeyExists(RecordType_Life))
            {
                Debug.LogError("life key not found.");
                return GameConstants.InitialHeartNum;
            }

            return ES3.Load<int>(RecordType_Life);
        }

        public int ConsumeLife(int consumeNum)
        {
            int lifeNum = LoadLife();
            if (lifeNum == 0)
            {
                Debug.LogError("life is zero");
                return 0;
            }
            int consumedNum = lifeNum - consumeNum;
            ES3.Save<int>(RecordType_Life, consumedNum);
            MonoBehaviour.print($"ConsumeLife Life={consumedNum.ToString()}");
            return consumedNum;
        }

        public int RecoverLife(int recoverNum)
        {
            int lifeNum = LoadLife();
            int recoveredNum = lifeNum + recoverNum;
            ES3.Save<int>(RecordType_Life, recoveredNum);

            return recoveredNum;
        }

        public int SaveLife(int life)
        {
            ES3.Save<int>(RecordType_Life, life);
            MonoBehaviour.print($"ConsumeLife Life={life.ToString()}");

            return life;
        }

        public DateTime LoadLifeLastRecoverTime()
        {
            if (!ES3.KeyExists(RecordType_LifeLastRecoverTime))
            {
                Debug.LogError("key (life last recover time) not found.");
                SaveLifeLastRecoverTime(DateTime.Now);
                return DateTime.Now;
            }

            var last = ES3.Load<string>(RecordType_LifeLastRecoverTime);

            return DateTime.Parse(last);
        }

        public void SaveLifeLastRecoverTime(DateTime last)
        {
            var lastString = last.ToString();
            ES3.Save<string>(RecordType_LifeLastRecoverTime, lastString);
        }

        void SaveRecord(Record record)
        {
            var records = new List<Record>();
            if (!ES3.KeyExists(RecordType_Records))
            {
                LoadRecord(out records);
            }

            ES3.Save<List<Record>>(RecordType_Records, records);
        }

        void LoadRecord(out List<Record> records)
        {
            records = new List<Record>();
            ES3.Load<List<Record>>(RecordType_Records, records);
        }

        public int LoadOnlineBattleNum()
        {
            return ES3.Load<int>(RecordType_OnlineBattleNum, 0);
        }

        public int LoadOnlineBattleWinCount()
        {
            return ES3.Load<int>(RecordType_OnlineWinCount, 0);
        }

        public int LoadOnlineBattleLoseCount()
        {
            return ES3.Load<int>(RecordType_OnlineLoseCount, 0);
        }

        public int LoadOnlineBattleDrawCount()
        {
            return ES3.Load<int>(RecordType_OnlineDrawCount, 0);
        }

        public float LoadOnlineRating()
        {
            return ES3.Load<float>(RecordType_OnlineRating, 0.0f);
        }

        public int LoadOfflineBattleNum()
        {
            return ES3.Load<int>(RecordType_OfflineBattleNum, 0);
        }

        public int LoadOfflineWinCount()
        {
            return ES3.Load<int>(RecordType_OfflineWinCount, 0);
        }

        public int LoadOfflineLoseCount()
        {
            return ES3.Load<int>(RecordType_OfflineLoseCount, 0);

        }

        public int LoadOfflineDrawCount()
        {
            return ES3.Load<int>(RecordType_OfflineDrawCount, 0);
        }
        
        public void SaveOnlineBattleNum(int battleNum)
        {
           ES3.Save<int>(RecordType_OnlineBattleNum, battleNum );
        }
        
        public void SaveOnlineBattleWinCount(int winCount)
        {
            ES3.Save<int>(RecordType_OnlineWinCount, winCount);
        }
        
        public void SaveOnlineBattleLoseCount(int loseCount)
        {
            ES3.Save<int>(RecordType_OnlineLoseCount, loseCount);
        }
        
        public void SaveOnlineBattleDrawCount(int drawCount)
        {
            ES3.Save<int>(RecordType_OnlineDrawCount, drawCount);
        }

        public void SaveOnlineRating(float rating)
        {
            ES3.Save<float>(RecordType_OnlineRating, rating);
        }

        public void SaveOfflineBattleNum(int battleNum)
        {
            ES3.Save<int>(RecordType_OfflineBattleNum, battleNum);
        }

        public void SaveOfflineWinCount(int winCount)
        {
            ES3.Save<int>(RecordType_OfflineWinCount, winCount);
        }

        public void SaveOfflineLoseCount(int loseCount)
        {
            ES3.Save<int>(RecordType_OfflineLoseCount, loseCount);
        }

        public void SaveOfflineDrawCount(int drawCount)
        {
            ES3.Save<int>(RecordType_OfflineDrawCount, drawCount);
        }

        public int GetMoney()
        {
            // kari
            return 999;
        }

        public int GetShopLevel()
        {
            return 11;
        }

        public bool LevelUpShop()
        {
            return true;
        }

        public int SaveAddMoney(int money)
        {
            // kari
            return 999;
        }
        
        public int LoadCoin()
        {
            return ES3.Load<int>(RecordType_Coin, GameConstants.InitialCoinNum);
        }
        
        public void SaveCoin(int coin)
        {
            ES3.Save<int>(RecordType_Coin, coin);
        }
    }
}
