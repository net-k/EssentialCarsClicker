namespace App
{
    public class LifeRewardAdVideoSaveDataManager : SingletonMonoBehaviour<LifeRewardAdVideoSaveDataManager>
    {
        public enum RecordType
        {
            Life,
            Coin
        }
        private readonly string RecordType_WatchPointAddedVideoTimes = "WatchPointAddedVideoTimes"; // ポイント追加の動画を見た回数

        private string GetSaveKey(RecordType recordType)
        {
            return $"{RecordType_WatchPointAddedVideoTimes}_{recordType}";
        }

        /// <summary>
        /// 動画視聴回数を保存する
        /// </summary>
        /// <param name="recordType"></param>
        /// <param name="times"></param>
        public void SaveWatchPointAddedVideoTimes(RecordType recordType, int times)
        {
            ES3.Save<int>(GetSaveKey(recordType), times);
        }

        /// <summary>
        /// 動画視聴回数をロードする
        /// </summary>
        /// <param name="recordType"></param>
        /// <returns></returns>
        public int LoadWatchPointAddedVideoTimes(RecordType recordType)
        {
            var key = GetSaveKey(recordType);
            if (!ES3.KeyExists(key)) return 0;
            return ES3.Load<int>(key);
        }
    }
}