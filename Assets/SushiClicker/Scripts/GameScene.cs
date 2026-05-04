using UnityEngine;

namespace SushiClicker.Scripts
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private SushiClicker.GameInitializer _gameInitializer = null;

        private void Awake()
        {
            _gameInitializer.Initialize();
        }
    }
}
