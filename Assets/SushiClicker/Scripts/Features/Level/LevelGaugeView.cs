using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// レベルゲージの表示を担当するView
    /// </summary>
    public class LevelGaugeView : MonoBehaviour
    {
        [SerializeField] private Slider _slider = null;
        [SerializeField] private Text _levelText = null;
        [SerializeField] private Text _progressText = null;

        /// <summary>
        /// ゲージの進捗を設定する（0〜1）
        /// </summary>
        public void SetGaugeValue(float value)
        {
            _slider.value = Mathf.Clamp01(value);
        }

        /// <summary>
        /// レベルテキストを設定する
        /// </summary>
        public void SetLevel(int level)
        {
            string language = LocalizationManager.CurrentLanguage;
            string levelLabel;
            if (language != null && language.Contains("ja"))
                levelLabel = $"レベル {level}";
            else
                levelLabel = $"Level {level}";
            
            _levelText.text = levelLabel;
        }

        /// <summary>
        /// 進捗数テキストを設定する。例: "1,234 / 10,000"
        /// </summary>
        public void SetProgressText(string current, string max)
        {
            string template = LocalizationManager.GetTranslation("key_LevelGauge_ProgressFormat");
            if (string.IsNullOrEmpty(template))
                template = "{current} / {max}";
            _progressText.text = template
                .Replace("{current}", current)
                .Replace("{max}", max);
        }
    }
}
