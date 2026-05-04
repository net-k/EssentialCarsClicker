using System;
using stickin;
using UnityEngine;

namespace TitleMatch.Scripts.Presentation.TitleLogo
{
    public class TitleLogoPresenter : MonoBehaviour
    {
        [SerializeField]
        TitleLogoView _view;

        [InjectField] private LocalizationService _localizationService;
 
        private void Awake()
        {
      
        }

        private void Start()
        {
            InjectService.BindFields(this);

            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            if(_localizationService.CurrentLanguage == SystemLanguage.Japanese)
            {
                _view.Initialize(TitleLogoView.LogoIndex.Japanese);
            }
            else
            {
                _view.Initialize(TitleLogoView.LogoIndex.English);
            }
        }

        private void Update()
        {
            UpdateLanguage();
        }
    }
}
