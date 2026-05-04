using UnityEngine;
using UnityEngine.UI;
using System;

namespace SushiCatcher.Achievement.AchievementNotificationDialog
{
    public class AchievementNotificationDialogView : MonoBehaviour
    {
        [SerializeField]
        private Text _captionText;
        [SerializeField]
        private Text _messageText;
        [SerializeField]
        private Button _okButton;
        
        public void SetTitle(string title)
        {
            if (_captionText != null)
            {
                _captionText.text = title;
            }
        }

        public void SetMessage(string message)
        {
            if (_messageText != null)
            {
                _messageText.text = message;
            }
        }

        public void SetOnOkButtonClicked(Action action)
        {
            if (_okButton != null)
            {
                _okButton.onClick.RemoveAllListeners();
                _okButton.onClick.AddListener(() => action?.Invoke());
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
