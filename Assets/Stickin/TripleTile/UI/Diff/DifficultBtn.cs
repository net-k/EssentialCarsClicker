using System;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class DifficultBtn : MonoBehaviour
    {
        [SerializeField] private Image _bgImage;
        [SerializeField] private TextLocalization _titleText;
        
        private Action<DifficultConfig> _clickCallback;
        
        private DifficultConfig _config;

        public void Init(DifficultConfig config, Color bgColor, Action<DifficultConfig> clickCallback)
        {
            _config = config;
            _clickCallback = clickCallback;
            
            _bgImage.color = bgColor;
            _titleText.SetText(config.Title);
        }
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _clickCallback?.Invoke(_config);
        }
    }
}
