using System;
using I2.Loc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    public class StatsPanelView : MonoBehaviour
    {
        [SerializeField] private Text _totalSushiCaptionText = null;
        [SerializeField] private Text _totalSushiValueText = null;
        [SerializeField] private Text _prestigeLevelCaptionText = null;
        [SerializeField] private Text _prestigeLevelValueText = null;
        [SerializeField] private Text _prestigeDetailText = null;
        [SerializeField] private Text _prestigeButtonText = null;
        [SerializeField] private Text _closeButtonText = null;
        [SerializeField] private Button _prestigeButton = null;
        [SerializeField] private Button _closeButton = null;

        public IObservable<Unit> OnPrestigeButtonClick => _prestigeButton.OnClickAsObservable();
        public IObservable<Unit> OnCloseButtonClick => _closeButton.OnClickAsObservable();

        private void Start()
        {
            // 変化しない静的テキストを I2 Localization で設定する
            _totalSushiCaptionText.text = LocalizationManager.GetTranslation("key_StatsDialog_TotalSushiText");
            _prestigeLevelCaptionText.text = LocalizationManager.GetTranslation("key_StatsDialog_PrestigeLevelText");
            _prestigeDetailText.text = LocalizationManager.GetTranslation("key_StatsDialog_PrestigeDetailText");
            _prestigeButtonText.text = LocalizationManager.GetTranslation("key_StatsDialog_PrestigeButtonText");
            _closeButtonText.text = LocalizationManager.GetTranslation("key_StatsDialog_CloseButtonText");
        }

        /// <summary>
        /// 累計おすし数（バナナ数）の値のみを表示する
        /// </summary>
        public void SetTotalSushi(double count)
        {
            _totalSushiValueText.text = BC_currencyConverter.Instance.GetCurrencyIntoString(count, false, false);
        }

        /// <summary>
        /// 現在のプレステージレベルの値のみを表示する
        /// </summary>
        public void SetPrestigeLevel(double level)
        {
            _prestigeLevelValueText.text = level.ToString("0");
        }

        /// <summary>
        /// プレステージボタンの押下可否を切り替える（1兆バナナ未満なら非活性）
        /// </summary>
        public void SetPrestigeButtonInteractable(bool interactable)
        {
            _prestigeButton.interactable = interactable;
        }
    }
}
