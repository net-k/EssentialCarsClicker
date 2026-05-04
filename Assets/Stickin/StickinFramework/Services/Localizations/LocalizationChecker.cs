using System.Collections;
using stickin.menus;
using stickin.menus.type1;
using UnityEngine;

namespace stickin
{
    public class LocalizationChecker : MonoBehaviour
    {
        [InjectField] private LocalizationService _localizationService;
        
        private void Start()
        {
            InjectService.BindFields(this);
            
            if (!_localizationService.IsUserSetLanguage)
                StartCoroutine(Check());
        }

        private IEnumerator Check()
        {
            yield return new WaitForSeconds(1f);

            CheckCoroutine();
        }

        private void CheckCoroutine()
        {
            if (!_localizationService.IsUserSetLanguage)
                MenusService.Show<LanguageMenu>();
        }
    }
}