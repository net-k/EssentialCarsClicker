using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Achievement.AchievementListItemButton
{
    public class AchievementListItemButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Text _buttonText;
        
        [SerializeField]
        private Image _clearImage;
        
        public void SetText(string text)
        {
            _buttonText.text = text;
        }

        public void SetCleared(bool isCleared)
        {
            _clearImage.gameObject.SetActive(isCleared);
        }
    }
}
