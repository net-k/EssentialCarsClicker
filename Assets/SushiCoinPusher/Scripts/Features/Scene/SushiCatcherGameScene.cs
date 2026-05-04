using UnityEngine;
using Zenject;

namespace SushiCatcher
{
    /// <summary>
    /// Manages the main game scene for SushiCatcher.
    /// </summary>
    public class SushiCatcherGameScene : MonoBehaviour
    {
        private AchievementManager _achievementManager;
        
        [Inject]
        private void Construct(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }
        
        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// Called before the first frame update.
        /// </summary>
        private void Start()
        {
            _achievementManager.Initialize();
        }
    }
}
