using UnityEngine;
using UnityEngine.UI;

namespace TohoReversi.Shop
{
    class CommonDialogView : MonoBehaviour
    {
        [SerializeField]
        private Text _captionText;
        [SerializeField]
        private Text _messageText;
        [SerializeField]
        private Button _okButton = null;

        public Button OkButton => _okButton;
        
        public void SetCaptionText(string caption)
        {
            _captionText.text = caption;
        }
            
        public void SetMessageText(string message)
        {
            _messageText.text = message;
        }
        
        public void SetCaptionTextKey(string captionKey)
        {
            _captionText.text = I2.Loc.LocalizationManager.GetTranslation(captionKey);
        }
            
        public void SetMessageTextKey(string messageKey)
        {
            _messageText.text = I2.Loc.LocalizationManager.GetTranslation(messageKey);
        }
    }
}