using System.Collections;
using UnityEngine;

namespace stickin.menus
{
    public class MenuHelperEvents : MonoBehaviour
    {
        [SerializeField] private BaseMenu _menu;
        [SerializeField] private string _textKey;
        [SerializeField] private string _textValue;
        [Header("or")]
        [SerializeField] private ScriptableObject _soValue;

        public void ShowMenu()
        {
            if (_menu != null)
            {
                Hashtable data = null;
                if (!string.IsNullOrEmpty(_textKey))
                {
                    if (!string.IsNullOrEmpty(_textValue))
                        data = new Hashtable {[_textKey] = _textValue};
                    else if (_soValue != null)
                        data = new Hashtable {[_textKey] = _soValue};
                }

                MenusService.Show(_menu, data);
            }
        }

        public void HideMenu()
        {
            MenusService.Hide(_menu);
        }
    }
}