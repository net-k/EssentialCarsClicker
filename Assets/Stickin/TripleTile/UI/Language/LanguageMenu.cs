using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    public class LanguageMenu : BaseMenu
    {
        [SerializeField] private LanguageCell _cellPrefab;
        [SerializeField] private Button[] _closeBtns;
        [SerializeField] private Button _selectBtn;
        [SerializeField] private Image _loaderIndicator;
        [SerializeField] private bool _needCheckSetUserLanguage;

        [InjectField] private LocalizationService _localizationService;

        private List<LanguageCell> _cells = new List<LanguageCell>();

        protected override void ShowComplete()
        {
            base.ShowComplete();

            _selectBtn.gameObject.SetActive(true);
            _loaderIndicator.gameObject.SetActive(false);

            foreach (var closeBtn in _closeBtns)
                closeBtn.interactable = !_needCheckSetUserLanguage || _localizationService.IsUserSetLanguage;

            SelectedLanguage(_localizationService.CurrentLanguage);
        }
        
        private void Start()
        {
            InjectService.BindFields(this);
            
            foreach (var closeBtn in _closeBtns)
                closeBtn.onClick.AddListener(OnClickClose);

            _selectBtn.onClick.AddListener(OnClickSelect);

            InitCells();
            _cellPrefab.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach (var cell in _cells)
                cell.OnClick -= OnClickCell;

            _cells = null;
        }
        
        private void InitCells()
        {
            var languages = _localizationService.SupportLanguages;

            for (var i = 0; i < languages.Count; i++)
            {
                var cell = Instantiate(_cellPrefab, _cellPrefab.transform.parent);
                cell.Init(languages[i]);
                cell.OnClick += OnClickCell;

                _cells.Add(cell);
            }
        }

        private void OnClickClose()
        {
            MenusService.Show("MainMenu");
        }

        private void OnClickSelect()
        {
            foreach (var cell in _cells)
            {
                if (cell.IsSelected)
                {
                    _localizationService.ChangeLanguage(cell.Data.Id);

                    _selectBtn.gameObject.SetActive(false);
                    _loaderIndicator.gameObject.SetActive(true);

                    break;
                }
            }

            OnClickClose();
        }

        private void SelectedLanguage(SystemLanguage id)
        {
            foreach (var cell in _cells)
                cell.SetSelected(cell.Data.Id == id);
        }

        private void OnClickCell(LanguageCell cell)
        {
            SelectedLanguage(cell.Data.Id);
        }
    }
}