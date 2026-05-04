using System;
using KumaFramework.BuildVersion;
using UnityEngine;
using UnityEngine.UI;
namespace ShisenSho.Framework
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private Text _versionText = null;
        private string versionMark = "a"; 
        private void Awake()
        {
            _versionText.text = CreateVersionText();
        }

        private string CreateVersionText()
        {
//            return $"Version {GameConstants.ApplicationVersion}.{ApplicationBuildVersion.GetBuildNumber()}.{CreateEnvCharacter()}";
            return $"Ver {Application.version}.{ApplicationBuildVersion.GetBuildNumber()}{versionMark}";
        }
        
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, 5.0f);
        }
    }
}
