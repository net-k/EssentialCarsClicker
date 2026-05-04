using System;
using UnityEngine;
using KumaFramework;

namespace SushiClicker
{
    /// <summary>
    /// プレイヤーレベルを管理するシングルトン。
    /// 累積バナナ数に基づいてレベルアップを判定し、OnLevelUpイベントを発行する。
    /// </summary>
    public class PlayerLevelManager : SingletonMonoBehaviour<PlayerLevelManager>
    {
        private readonly string RecordType_Level = "PlayerLevel_Level";

        /// <summary>動画視聴時の報酬倍率</summary>
        public const double VideoRewardMultiplier = 4.0;

        private int _currentLevel = Quiz.Infrastructure.GameConstants.InitialPlayerLevel;

        /// <summary>現在のレベル</summary>
        public int CurrentLevel => _currentLevel;

        [SerializeField] private BC_bananaPerSec _bcBps = null;
        [SerializeField] private BC_Click _bcClick = null;

        /// <summary>レベルアップ時に発行。引数: (fromLevel, toLevel, reward)</summary>
        public event Action<int, int, double> OnLevelUp;

        private void Awake()
        {
            _currentLevel = ES3.Load<int>(RecordType_Level, Quiz.Infrastructure.GameConstants.InitialPlayerLevel);
            
            // レベル上限チェックを廃止（旧: Lv10000以上で破損判定）
            // 無制限対応のため、超高レベルでも有効性のみチェック
            if (_currentLevel < 0)
            {
                Debug.LogWarning($"PlayerLevelManager: Invalid level data detected (Level={_currentLevel}). Resetting to InitialPlayerLevel.");
                _currentLevel = Quiz.Infrastructure.GameConstants.InitialPlayerLevel;
                ES3.Save<int>(RecordType_Level, _currentLevel);
            }
        }

        /// <summary>
        /// レベルNのしきい値を返す。
        /// 公式: 2000 × Lv^2.5（急速な指数関数的成長）
        /// Lv1=2k, Lv10=632k, Lv100=63M, Lv1000=2B
        /// 超大型レベル対応（上限なし）
        /// </summary>
        public static double GetThreshold(int level)
        {
            return 2000.0 * Math.Pow(level, 2.5);
        }

        /// <summary>
        /// HighPrecisionNumber対応版のしきい値取得
        /// </summary>
        public static HighPrecisionNumber GetThresholdHigh(int level)
        {
            var baseValue = new HighPrecisionNumber(2000.0);
            var levelValue = new HighPrecisionNumber(level);
            // べき乗計算: level^2.5 = level^2 * sqrt(level)
            var squared = levelValue.Power(2);
            var sqrtLevel = new HighPrecisionNumber(Math.Sqrt(level));
            return baseValue * squared * sqrtLevel;
        }

        /// <summary>
        /// 累積バナナ総数でレベルアップを確認する。BC_Click.AddBananas()の末尾から呼ぶ。
        /// </summary>
        public void CheckLevelUp(double totalBananas)
        {
            int newLevel = _currentLevel;
            
            // 無限ループを防ぐため、最大100レベルアップまでに制限
            // GetThreshold(level) = 1000 * level^1.5 は指数関数的に増加するため、
            // 通常は数回のループで終わる
            int maxIterations = 100;
            int iterations = 0;
            
            while (iterations < maxIterations && totalBananas >= GetThreshold(newLevel + 1))
            {
                newLevel++;
                iterations++;
            }

            if (newLevel <= _currentLevel) return;

            int fromLevel = _currentLevel;
            double bps = GetBps();
            double bpc = GetBananasPerClick();
            
            // 報酬の基本値を計算（1時間分の半分 = 1800秒分）
            double baseReward = (bps + bpc) * 1800.0 * (newLevel - fromLevel);
            
            // 連続レベルアップを防ぐため、報酬を上限でキャップ
            // 上限：次のレベルアップに必要な「差分（Range）」の (0.5 / VideoRewardMultiplier) 
            // （VideoRewardMultiplier 倍したときに 50% を超えないように調整）
            double currentLevelThreshold = GetThreshold(newLevel);
            double nextLevelThreshold = GetThreshold(newLevel + 1);
            double range = nextLevelThreshold - currentLevelThreshold;
            double maxRewardBase = range * (0.5 / VideoRewardMultiplier);
            double reward = Math.Min(baseReward, maxRewardBase);

            _currentLevel = newLevel;
            ES3.Save<int>(RecordType_Level, _currentLevel);
            
            if (iterations >= maxIterations)
            {
                Debug.LogWarning($"PlayerLevelManager.CheckLevelUp: Reached max iterations ({maxIterations}). newLevel={newLevel}, totalBananas={totalBananas}. This indicates a data corruption issue.");
            }

            OnLevelUp?.Invoke(fromLevel, newLevel, reward);
        }
        
        /// <summary>
        /// バナナ総数から正しいレベルを計算する（セーブデータ破損時のリカバリ用）
        /// 上限廃止：何度も反復計算して正しいレベルを求める
        /// </summary>
        public int CalculateCorrectLevel(double totalBananas)
        {
            int level = 1;
            const int maxIterations = 1000; // 無限ループ防止
            int iterations = 0;
            
            while (iterations < maxIterations && totalBananas >= GetThreshold(level + 1))
            {
                level++;
                iterations++;
            }
            
            if (iterations >= maxIterations)
            {
                Debug.LogWarning($"PlayerLevelManager.CalculateCorrectLevel: Reached max iterations. level={level}, totalBananas={totalBananas}");
            }
            
            return level;
        }
        
        /// <summary>
        /// セーブデータが破損している場合は、正しいレベルに修正する
        /// </summary>
        public void RecoverCorruptedLevel(double totalBananas)
        {
            int correctLevel = CalculateCorrectLevel(totalBananas);
            if (correctLevel != _currentLevel)
            {
                Debug.LogWarning($"PlayerLevelManager.RecoverCorruptedLevel: Recovering from level {_currentLevel} to {correctLevel}");
                _currentLevel = correctLevel;
                ES3.Save<int>(RecordType_Level, _currentLevel);
            }
        }

        private double GetBps()
        {
            if (_bcBps == null)
                _bcBps = GameObject.Find("BPSmanager")?.GetComponent<BC_bananaPerSec>();
            if (_bcBps == null)
            {
                Debug.LogWarning("PlayerLevelManager: BC_bananaPerSec が見つかりません");
                return 0.0;
            }
            return _bcBps.GetBananasPerSec();
        }

        /// <summary>
        /// 現在のクリックパワー（アップグレード由来）を取得する
        /// </summary>
        private double GetBananasPerClick()
        {
            if (_bcClick == null)
                _bcClick = FindObjectOfType<BC_Click>();
            if (_bcClick == null)
            {
                Debug.LogWarning("PlayerLevelManager: BC_Click が見つかりません");
                return 0.0;
            }
            return _bcClick.bananasPerClick;
        }

#if UNITY_EDITOR
        /// <summary>デバッグ用: レベルをリセットする</summary>
        public void DebugResetLevel()
        {
            _currentLevel = Quiz.Infrastructure.GameConstants.InitialPlayerLevel;
            ES3.Save<int>(RecordType_Level, _currentLevel);
        }
#endif
    }
}
