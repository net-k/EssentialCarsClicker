using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SushiCoinPusher.Features.GameMenu
{
    public class GameMenuButton : MonoBehaviour
    {
        [SerializeField]
        private Button _menuButton = null;

        [FormerlySerializedAs("_gameMenuPresenter")] [SerializeField]
        GameMenuDialogPresenter gameMenuDialogPresenter = null;
        
        void Awake()
        {
            _menuButton.onClick.AddListener( () =>
            {
                gameMenuDialogPresenter.Show();
            });
        }
   
    }
}
