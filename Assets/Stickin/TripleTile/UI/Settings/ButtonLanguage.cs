using System;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class ButtonLanguage : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            MenusService.Show<LanguageMenu>();
        }
    }
}