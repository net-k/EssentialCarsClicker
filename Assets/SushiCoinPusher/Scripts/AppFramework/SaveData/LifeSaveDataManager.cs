using System;
using Quiz.Infrastructure;
using UnityEngine;

namespace App
{
    public class LifeSaveDataManager :  SingletonMonoBehaviour<LifeSaveDataManager>
    {
        private readonly string RecordType_Life = "RecordType_Life";
        private readonly string RecordType_LifeLastRecoverTime = "LifeLastRecoverTime";

        public enum LifeType : int
        {
            Default = 0,
        }

        string GetLifeKey(LifeType lifeType)
        {
            switch (lifeType)
            {
                default:
                    return $"{RecordType_Life}_{lifeType}";
            }
        }

        string GetLifeLastRecoverTimeKey(LifeType lifeType)
        {
            switch (lifeType)
            {
                default:
                    return $"{RecordType_LifeLastRecoverTime}_{lifeType}";
            }
        }
        
        public int LoadLife(LifeType lifeType)
        {
            if (!ES3.KeyExists(GetLifeKey(lifeType)))
            {
                Debug.LogWarning("life key not found.");
                ES3.Save<int>(GetLifeKey(lifeType), GameConstants.InitialHeartNum );
                return GameConstants.InitialHeartNum;
            }

            return ES3.Load<int>(GetLifeKey(lifeType), 0 );
        }

        public int ConsumeLife(int consumeNum, LifeType lifeType)
        {
            int lifeNum = LoadLife(lifeType);
            if (lifeNum == 0)
            {
                Debug.LogError("life is zero");
                return 0;
            }
            int consumedNum = lifeNum - consumeNum;
            ES3.Save<int>(GetLifeKey(lifeType), consumedNum);
            
            return consumedNum;
        }

        public int RecoverLife(int recoverNum, LifeType lifeType)
        {
            int lifeNum = LoadLife(lifeType);
            int recoveredNum = lifeNum + recoverNum;
            ES3.Save<int>(GetLifeKey(lifeType), recoveredNum);

            return recoveredNum;
        }

        public int SaveLife(int life, LifeType lifeType)
        {
            ES3.Save<int>(GetLifeKey(lifeType), life);

            return life;
        }

        public DateTime LoadLifeLastRecoverTime(LifeType lifeType)
        {
            if (!ES3.KeyExists( GetLifeLastRecoverTimeKey(lifeType) ))
            {
                Debug.LogError("key (life last recover time) not found.");
                SaveLifeLastRecoverTime(DateTime.Now, lifeType);
                return DateTime.Now;
            }

            var last = ES3.Load<string>(GetLifeLastRecoverTimeKey(lifeType));

            return DateTime.Parse(last);
        }

        public void SaveLifeLastRecoverTime(DateTime last, LifeType lifeType)
        {
            var lastString = last.ToString();
            ES3.Save<string>(GetLifeLastRecoverTimeKey(lifeType), lastString);
        }
    }
}