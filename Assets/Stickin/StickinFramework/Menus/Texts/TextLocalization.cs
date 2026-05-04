using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(Text))]
    public class TextLocalization : MonoBehaviour
    {
        [SerializeField] private string _localKey;
        [SerializeField] private bool _toUpperAll = false;
        [SerializeField] private bool _toUpperFirst = true;
        
        [InjectField] private LocalizationService _localizationService;
        
        private Text _txt;
        private string _key;
        private bool _needLocalized = true;
        private string _param;
        
        #region Public Methods
        public void SetText(string key, string param = null, bool needLocalized = true, string prefix = "")
        {
            InjectService.BindFields(this);
            _param = param;

            if (_txt == null)
            {
                _txt = GetComponent<Text>();
            }

            _needLocalized = needLocalized;
            _key = key;

            if (!string.IsNullOrEmpty(key) && _needLocalized)
            {
                if (_txt != null)
                {
                    var str = _localizationService.GetStrById(key);

                    if (!string.IsNullOrEmpty(str))
                        _txt.text = _param != null ? string.Format(str, _param) : str;
                    else
                        Debug.LogError($"LocalizedTextView.SetText: localized string is null or empty for key '{key}' in '{name}, parent = {transform.parent.name}");
                }
                else
                {
                    Debug.LogError($"LocalizedTextView.SetText: not text component in '{name}, parent = {transform.parent.name}' for key '{key}'");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(key))
                    _txt.text = _param != null ? string.Format(key, _param) : key;
                else
                    _txt.text = _param != null ? _param : string.Empty;
            }

            if (_toUpperAll)
                _txt.text = _txt.text.ToUpper();

            if (_toUpperFirst)
                _txt.text = _txt.text.ToUpperFirst();

            _txt.text = prefix + _txt.text;
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            InjectService.BindFields(this);
            _txt = GetComponent<Text>();
            
            _localizationService.OnChangeLanguage += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            SetText(!string.IsNullOrEmpty(_localKey) ? _localKey : _key, _param, _needLocalized);
        }

        private void OnDestroy()
        {
            if (_localizationService != null)
                _localizationService.OnChangeLanguage -= Refresh;
        }

        private void OnEnable()
        {
            Refresh();
        }
        #endregion
    }
}