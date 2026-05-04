using System;
using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// レベルゲージのロジックを担当するPresenter。
    /// 毎フレーム現在の累積バナナ数から次のレベルへの進捗を計算し、Viewを更新する。
    /// </summary>
    public class LevelGaugePresenter : MonoBehaviour
    {
        [SerializeField] private LevelGaugeView _view = null;
        [SerializeField] private BC_Click _bcClick = null;

        // デバッグログの出力間隔（秒）
        private const float LogInterval = 1f;
        private float _logTimer = 0f;

        private void Start()
        {
            UpdateView(true);
        }

        private void Update()
        {
            _logTimer += Time.deltaTime;
            bool shouldLog = _logTimer >= LogInterval;
            if (shouldLog) _logTimer = 0f;
            UpdateView(shouldLog);
        }

        private void UpdateView(bool shouldLog = false)
        {
            var manager = PlayerLevelManager.Instance;
            if (manager == null || _bcClick == null) return;

            int currentLevel = manager.CurrentLevel;

            // 累積バナナ総数を計算
            double totalBananas = (_bcClick.bananaTrillionCount * 1e12) + _bcClick.BananaCount;

            // レベルとしきい値の不整合（レベルが高すぎる）をチェックして自動リカバリ
            double fromThreshold = (currentLevel <= 1) ? 0.0 : PlayerLevelManager.GetThreshold(currentLevel);
            if (currentLevel > 1 && totalBananas < fromThreshold * 0.8) // 遊びを持たせて判定
            {
                Debug.LogWarning($"LevelGaugePresenter: Level-Threshold mismatch (Level={currentLevel}, Total={totalBananas}, Min={fromThreshold}). Recovering...");
                manager.RecoverCorruptedLevel(totalBananas);
                currentLevel = manager.CurrentLevel;
                fromThreshold = (currentLevel <= 1) ? 0.0 : PlayerLevelManager.GetThreshold(currentLevel);
            }

            double toThreshold = PlayerLevelManager.GetThreshold(currentLevel + 1);

            // 進捗（0.0 〜 1.0）の計算
            float progress;
            double range = toThreshold - fromThreshold;
            if (double.IsInfinity(range) || range <= 0)
            {
                progress = 1f;
            }
            else
            {
                // totalBananas が fromThreshold を下回っている場合は 0 にする
                double currentProgressValue = Math.Max(0.0, totalBananas - fromThreshold);
                progress = (float)Math.Min(1.0, currentProgressValue / range);
            }

            // 表示用テキストの生成
            double progressBananas = Math.Max(0.0, totalBananas - fromThreshold);
            string currentStr = double.IsInfinity(progressBananas) ? "MAX" : FormatProgress(progressBananas);
            string maxStr = double.IsInfinity(range) ? "MAX" : FormatProgress(range);
            
            if (shouldLog)
            {
                Debug.Log($"[LevelGauge] Level={currentLevel}, Total={totalBananas}, from={fromThreshold}, to={toThreshold}, progress={progress}");
            }

            if (_view != null)
            {
                _view.SetGaugeValue(progress);
                _view.SetLevel(currentLevel);
                _view.SetProgressText(currentStr, maxStr);
            }
        }

        /// <summary>
        /// 進捗値をフォーマットする。
        /// BC_currencyConverter に委譲して言語に応じた単位（K/Million… or 万/億/兆…）を使う。
        /// </summary>
        private string FormatProgress(double value)
        {
            if (value <= 0) return "0";
            // BC_currencyConverter.Instance.GetCurrencyIntoString は整数部分のみをフォーマットするため Floor する
            string result = BC_currencyConverter.Instance.GetCurrencyIntoString(Math.Floor(value), false, false);
            if (string.IsNullOrEmpty(result))
            {
                return value.ToString("N0");
            }
            return result;
        }
    }
}
