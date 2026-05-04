using I2.Loc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Aquarium.Presentation.GameScene.AdditionalPointDialog
{
    public class AdditionalPointView : MonoBehaviour
    {
        [SerializeField]
        private Text captionText = null;
        public Text CaptionText
        {
            get { return captionText; }
        }
    
        [FormerlySerializedAs("MovieButton")]
        [SerializeField]
        private Button movieButton = null;
        public Button MovieButton
        {
            get { return movieButton; }
        }

        [FormerlySerializedAs("CloseButton")]
        [SerializeField]
        private Button closeButton = null;
        public Button CloseButton
        {
            get { return closeButton; }
        }

        [SerializeField]
        private Image _rewardImage;

        public Image RewardImage => _rewardImage;
    }
}
