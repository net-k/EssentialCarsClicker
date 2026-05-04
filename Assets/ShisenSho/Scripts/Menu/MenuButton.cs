using SushiClicker;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button _menuButton = null;
        [SerializeField] private MenuDialogPresenter _menuDialogPresenter = null;
        
        private void Awake()
        {
            _menuButton.onClick.AddListener(() =>
            {
                _menuDialogPresenter.Show();
            });
        }
    }
}
