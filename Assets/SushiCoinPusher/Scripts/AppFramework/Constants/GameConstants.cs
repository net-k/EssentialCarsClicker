namespace Quiz.Infrastructure
{
    public class GameConstants
    {
        public static string ApplicationVersion = "1.0.0.14";

        // 動画によるポイント取得
        public static int WatchPointAddedVideoTimesPerDay = 10; // 1日に動画を見ることができる回数

        public static float DefaultVolume = -20.0f;

        /// <summary>
        /// 初回ライフ数
        /// </summary>
        public static int InitialHeartNum = 5;

        /// <summary>
        /// ライフ最大数
        /// </summary>
        public static int LifeMaxNum = 5;

        public static int HeartNumRecoverByMovie = 1;

        /// <summary>
        /// デバッグモードにするかどうか
        /// </summary>
        public static bool IsDebugMode = false; // true;

        public static string GameName = "HitAndBlowOnline";

        public static int MaxShopLevel = 11;

        /// <summary>
        /// 初回コイン数
        /// </summary>
        public static int InitialCoinNum = 100;

        /// <summary>
        /// プレイヤーの初期レベル
        /// </summary>
        public static int InitialPlayerLevel = 1;
        public const int MaxCoinDrop = 30;

        
        public static string version = "v1.0.1.1";
        public static float CoinRefillRate = 0.2f;
    }
}
