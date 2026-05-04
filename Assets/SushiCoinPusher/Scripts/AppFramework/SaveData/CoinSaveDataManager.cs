using Quiz.Infrastructure;
using UnityEngine;
using CoinPusher.Core;
using KumaFramework;

namespace App
{
    public class CoinSaveDataManager : SingletonMonoBehaviour<CoinSaveDataManager>
    {
        private readonly string RecordType_Coin = "Coin";
        private readonly string RecordType_Cash = "Cash";

        /// <summary>コインの最大値（long型の上限に近い値）</summary>
        private const long MAX_COIN = long.MaxValue - 1;

        public long LoadCoin()
        {
            return ES3.Load<long>(RecordType_Coin, GameConstants.InitialCoinNum);
        }

        public void SaveCoin(long coin)
        {
            // 上限チェック
            if (coin > MAX_COIN)
            {
                Debug.LogWarning($"CoinSaveDataManager.SaveCoin: Coin amount ({coin}) exceeded MAX_COIN ({MAX_COIN}). Capping to MAX.");
                coin = MAX_COIN;
            }
            ES3.Save<long>(RecordType_Coin, coin);
        }

        public long AddCoin(long amount)
        {
            long currentCoin = LoadCoin();
            // オーバーフロー防止
            long newCoin = (currentCoin > MAX_COIN - amount) ? MAX_COIN : currentCoin + amount;
            if (newCoin > MAX_COIN)
            {
                Debug.LogWarning($"CoinSaveDataManager.AddCoin: Result would exceed MAX_COIN. Capping to MAX.");
                newCoin = MAX_COIN;
            }
            SaveCoin(newCoin);
            return newCoin;
        }

        public long ConsumeCoin(long amount)
        {
            long currentCoin = LoadCoin();
            if (currentCoin < amount)
            {
                Debug.LogError("Not enough coins");
                return currentCoin;
            }
            long newCoin = currentCoin - amount;
            SaveCoin(newCoin);
            return newCoin;
        }

        // 以下、playerCash関連のメソッドを追加
        public long LoadPlayerCash()
        {
            // GameConstants.InitialCashNum があればそれを使う。なければ0
            return ES3.Load<long>(RecordType_Cash, 0L); // 仮に0を設定
        }

        public void SavePlayerCash(long cash)
        {
            ES3.Save<long>(RecordType_Cash, cash);
        }

        public long AddPlayerCash(long amount)
        {
            long currentCash = LoadPlayerCash();
            long newCash = currentCash + amount;
            SavePlayerCash(newCash);
            return newCash;
        }

        public long ConsumePlayerCash(long amount)
        {
            long currentCash = LoadPlayerCash();
            if (currentCash < amount)
            {
                Debug.LogError("Not enough cash");
                return currentCash;
            }
            long newCash = currentCash - amount;
            SavePlayerCash(newCash);
            return newCash;
        }
        
        // 全てのコイン関連データを削除するメソッドを追加
        public void DeleteAllCoinData()
        {
            ES3.DeleteKey(RecordType_Coin);
            ES3.DeleteKey(RecordType_Cash);
        }

        public bool ExistsSaveData()
        {
            return ES3.KeyExists(RecordType_Coin) || ES3.KeyExists(RecordType_Cash);
        }
    }
}
