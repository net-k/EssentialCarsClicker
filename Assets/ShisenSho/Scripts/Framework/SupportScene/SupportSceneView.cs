using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Framework.SupportScene
{
    public class SupportSceneView : MonoBehaviour
    {
        [SerializeField] private Button privacyButton = null;
        [SerializeField] private Button licenseButton = null;
        [SerializeField] private Button supportButton = null;
        [SerializeField] private Button backButton = null;

        public Button PrivacyButton => privacyButton;

        public Button LicenseButton => licenseButton;

        public Button SupportButton => supportButton;
    
        public Button BackButton => backButton;
    }
}
