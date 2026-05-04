using UnityEngine;
using UnityEngine.UI;


namespace SushiCoinPusher.Features.GameMenu
{
    public class GameMenuDialogView : MonoBehaviour
    {
        [SerializeField]
        Button _closeButton = null;
        [SerializeField]
        Button _titleBackButton = null;
        
        public Button CloseButton => _closeButton;
        public Button TitleBackButton => _titleBackButton;
        
    }
}
