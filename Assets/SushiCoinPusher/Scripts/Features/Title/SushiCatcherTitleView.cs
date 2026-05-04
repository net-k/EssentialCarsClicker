using UnityEngine;
using UnityEngine.UI;

namespace ShogiOnline.Presentation
{
    public class SushiCatcherTitleView : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton = null;

        public Button StartButton => _startButton;
        
        [SerializeField]
        public Button _achievenmentButton = null; 
        
        public Button AchievementButton => _achievenmentButton;
        
        [SerializeField]
        private Button _collectionButton = null;
        public Button CollectionButton => _collectionButton;
    }
}
