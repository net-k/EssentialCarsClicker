using System;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class LanguageCell : MonoBehaviour
    {
        [SerializeField] private Image _bgImage;
        [SerializeField] private Text _titleTxt;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _passiveColor;
        [SerializeField] private Image _selectedImage;

        public LocalizationData Data { get; private set; }
        public bool IsSelected { get; private set; }

        public Action<LanguageCell> OnClick;

        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClickBtn);
        }

        public void Init(LocalizationData data)
        {
            Data = data;
            _titleTxt.text = data.LocalizedTitle;

            gameObject.SetActive(true);
            SetSelected(false);
        }

        public void SetSelected(bool value)
        {
            IsSelected = value;

            _bgImage.color = value ? _activeColor : _passiveColor;
            _selectedImage.gameObject.SetActive(value);
        }

        private void OnClickBtn()
        {
            OnClick?.Invoke(this);
        }
    }
}