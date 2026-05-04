using System;
using KumaFramework.BuildVersion;
using stickin;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryOnline.Presentation
{
    public class AppVersionView : MonoBehaviour
    {
        [SerializeField]
        private Text _versionText = null;

        /// <summary>
        /// バージョンマーク（ビルド識別用）
        /// 例: "a", "b", "dev" など。GitHubハッシュなども可。
        /// </summary>
        [SerializeField]
        private string versionMark = "a"; 
        
        [SerializeField] private AdmobAdsConfig _config;
 

        // Start is called before the first frame update
        void Start()
        {
            string versionStr = CreateVersionText();
            _versionText.text = versionStr;
            
            // デバッグ: 実際の文字列を確認
            Debug.Log($"[AppVersionView] Version text: '{versionStr}' (length: {versionStr.Length})");
            Debug.Log($"[AppVersionView] versionMark: '{versionMark}'");
            Debug.Log($"[AppVersionView] Application.version: '{Application.version}'");
            Debug.Log($"[AppVersionView] BuildNumber: '{ApplicationBuildVersion.GetBuildNumber()}'");
            
            #if ENABLE_SRDEBUG
            _versionText.text = _versionText.text + " SRDEBUG";
            #endif
            #if DEBUG_MONEY
            _versionText.text = _versionText.text + " MONEYDEBUG";
            #endif
        }

        private string CreateVersionText()
        {
            string baseVersion = $"Ver {Application.version}";
            string buildNum = $".{ApplicationBuildVersion.GetBuildNumber()}";
            string mark = versionMark;
            string ads = CreateAdsCharacter();
            
            return $"{baseVersion}{buildNum}{mark}{ads}";
        }

        private string CreateAdsCharacter()
        {
            if(_config.IsTest )
            {
                return " AdMob テスト配信";
            }

            return "";
        }
    }
}
