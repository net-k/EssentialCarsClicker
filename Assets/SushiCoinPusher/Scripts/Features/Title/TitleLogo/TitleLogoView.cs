using System;
using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Title
{
    public class TitleLogoView : MonoBehaviour
    {
        [SerializeField]
        private Image _logoJpImage;
        [SerializeField]
        private Image _logoEnImage;

        private void Awake()
        {
            _logoEnImage.gameObject.SetActive(false);
            _logoJpImage.gameObject.SetActive(false);
            
            // 英語ならロゴを切り替える
            if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                _logoJpImage.gameObject.SetActive(true);
            }
            else
            {
                _logoEnImage.gameObject.SetActive(true);
            }
        }
    }
}
