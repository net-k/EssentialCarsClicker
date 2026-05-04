using System;
using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// レベルアップダイアログのView。表示・非表示とデータのセットを担当する。
    /// </summary>
    public class LevelUpDialogView : MonoBehaviour
    {
        [SerializeField] private Text _levelUpTitleText = null;
        [SerializeField] private Text _rewardText = null;
        [SerializeField] private Button _rewardButton = null;
        [SerializeField] private Text _rewardButtonText = null;
        [SerializeField] private Button _okButton = null;

        /// <summary>タイトルを設定する</summary>
        public void SetLevelUpTitle(string titleText, int from, int to)
        {
            _levelUpTitleText.text = titleText;
        }

        /// <summary>報酬テキストを設定する</summary>
        public void SetRewardText(string text)
        {
            _rewardText.text = text;
        }

        /// <summary>報酬ボタンのクリックハンドラーを設定する</summary>
        public void SetOnRewardButtonClicked(Action handler)
        {
            _rewardButton.onClick.RemoveAllListeners();
            _rewardButton.onClick.AddListener(() => handler());
        }

        /// <summary>OKボタンのクリックハンドラーを設定する</summary>
        public void SetOnOkButtonClicked(Action handler)
        {
            _okButton.onClick.RemoveAllListeners();
            _okButton.onClick.AddListener(() => handler());
        }

        /// <summary>ダイアログを表示する</summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>ダイアログを非表示にする</summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetRewardButtonText(string rewardButtonText)
        {
            _rewardButtonText.text = rewardButtonText;
        }
    }
}
