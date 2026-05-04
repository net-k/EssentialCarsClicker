using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Life
{
    public class LifeUIView : MonoBehaviour
    {
        [SerializeField]
        private Text _lifeText;

        [SerializeField]
        private Image _lifeImage;

        public Image LifeImage => _lifeImage;

        public Text _lifeRecoverTime;

        public Text LifeRecoverTime
        {
            get => _lifeRecoverTime;
            set => _lifeRecoverTime = value;
        }

        [SerializeField]
        private Button _recoverButton;

        public Text LifeText
        {
            get => _lifeText;
            set => _lifeText = value;
        }

        
        public Button RecoverButton
        {
            get => _recoverButton;
            set => _recoverButton = value;
        }

        
    }
}
