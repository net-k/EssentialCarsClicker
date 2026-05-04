using UnityEngine;
using UnityEngine.UI;

namespace SushiCoinPusher.Features.Achievement
{
    public class AchievementSceneBackButton : MonoBehaviour
    {
        [SerializeField]
        private Button _backButton;
    
        // Start is called before the first frame update
        void Start()
        {
            _backButton.onClick.AddListener(() =>
            {
                SushiCoinPusherSceneManager.Load( SushiCaterScene.Title );
            });
        }

 
    }
}