using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonPrivacyPolicy : MonoBehaviour
    {
        [SerializeField] private string _privacyPolicyUrl;

        [InjectField] private ResourcesService _resourcesService;
        
        private void Start()
        {
            InjectService.BindFields(this);
            
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (!string.IsNullOrEmpty(_privacyPolicyUrl))
                Application.OpenURL(_privacyPolicyUrl);
            else
            {
                var resPrivacy = ResourcesService.GetResourceValueString(ResourcesService.PrivacyPolicy);
                
                if (!string.IsNullOrEmpty(resPrivacy))
                    Application.OpenURL(resPrivacy);
                else
                    Debug.LogError("PrivacyPolicy URL is empty");
            }
        }
    }
}