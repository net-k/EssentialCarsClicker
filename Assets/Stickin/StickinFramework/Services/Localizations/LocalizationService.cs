using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace stickin
{
    public class LocalizationService : BaseService
    {
        [SerializeField] private LocalizationsConfig _config;

        private bool _isInit;
        private LocalizationsConfig _localizationsConfig;
        private LocalizationData _currentLanguageData;
        
        private static readonly Dictionary<string, SystemLanguage> pairs = new Dictionary<string, SystemLanguage>
        {
            {"af", SystemLanguage.Afrikaans},
            {"ar", SystemLanguage.Arabic},
            {"be", SystemLanguage.Belarusian},
            {"bg", SystemLanguage.Bulgarian},
            {"ca", SystemLanguage.Catalan},
            {"zh", SystemLanguage.Chinese},
            {"cs", SystemLanguage.Czech},
            {"da", SystemLanguage.Danish},
            {"nl", SystemLanguage.Dutch},
            {"en", SystemLanguage.English},
            {"et", SystemLanguage.Estonian},
            {"fo", SystemLanguage.Faroese},
            {"fi", SystemLanguage.Finnish},
            {"fr", SystemLanguage.French},
            {"de", SystemLanguage.German},
            {"el", SystemLanguage.Greek},
            {"hu", SystemLanguage.Hungarian},
            {"is", SystemLanguage.Icelandic},
            {"id", SystemLanguage.Indonesian},
            {"it", SystemLanguage.Italian},
            {"jp", SystemLanguage.Japanese},
            {"ko", SystemLanguage.Korean},
            {"lv", SystemLanguage.Latvian},
            {"lt", SystemLanguage.Lithuanian},
            {"nb", SystemLanguage.Norwegian},
            {"pl", SystemLanguage.Polish},
            {"pt", SystemLanguage.Portuguese},
            {"ro", SystemLanguage.Romanian},
            {"ru", SystemLanguage.Russian},
            {"sk", SystemLanguage.Slovak},
            {"sl", SystemLanguage.Slovenian},
            {"es", SystemLanguage.Spanish},
            {"sv", SystemLanguage.Swedish},
            {"th", SystemLanguage.Thai},
            {"tr", SystemLanguage.Turkish},
            {"vi", SystemLanguage.Vietnamese}
        };
        
        public bool IsUserSetLanguage { get; private set; }
        
        public SystemLanguage CurrentLanguage => _currentLanguageData?.Id ?? SystemLanguage.English;
        public string CurrentLanguageId => GetId(CurrentLanguage);
        public List<LocalizationData> SupportLanguages => GetSupportedLanguages();
        public event Action OnChangeLanguage;
        
        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            base.Init(appData, callbackComplete);
            
            InjectService.Bind<LocalizationService>(this);
            Init(_config);
            
            InitComplete(true);
        }

        #region Public Methods
        private bool IsJapanese()
        {
            return Application.systemLanguage == SystemLanguage.Japanese;
        }
        public void Init(LocalizationsConfig config)
        {
            if (config != null)
            {
                _isInit = true;
                _localizationsConfig = config;

                if (IsJapanese())
                {
                    _localizationsConfig.DefaultLanguageId = SystemLanguage.Japanese;
                }

                var supportLanguages = "Support language: \n";
                foreach (var localizationData in _localizationsConfig.Languages)
                    supportLanguages += $"{localizationData.Id}    {localizationData.Name}\n";
                Debug.Log(supportLanguages);

                var currentLanguage = GetLanguage(PlayerPrefs.GetString("CurrentLanguage"), Application.systemLanguage.ToString());

                Debug.Log($"ServiceLocalization.Init: Application.systemLanguage = {Application.systemLanguage}");
                var languageData = GetLanguageData(currentLanguage);

                if (languageData == null)
                {
                    languageData = GetLanguageData(config.DefaultLanguageId);
                    Debug.Log($"ServiceLocalization.Init: CurrentLanguage default = {config.DefaultLanguageId}");

                }
                else
                {
                    Debug.Log($"ServiceLocalization.Init: CurrentLanguage = {currentLanguage}");
                }

                if (languageData != null)
                {
                    languageData.Init();
                    _currentLanguageData = languageData;
                }
                else
                {
                    Debug.LogError($"ServiceLocalization.Init: not find languageData for id = " + Application.systemLanguage);
                }
            }
            else
            {
                Debug.LogError("ServiceLocalization.Init: data is null");
            }
        }

        public List<LocalizationData> GetSupportedLanguages()
        {
            var result = new List<LocalizationData>();

            foreach (var localizationData in _localizationsConfig.Languages)
            {
                if(localizationData.IsAvailable)
                    result.Add(localizationData);
            }

            return result;
        }

        public string GetStrById(string id)
        {
            if (_isInit)
            {
                if (!string.IsNullOrEmpty(id))
                    return _currentLanguageData.GetLocalizedText(id);

                return id;
            }

            Debug.LogError("ServiceLocalization not init");
            return id;
        }

        public void ChangeLanguage(string id)
        {
            ChangeLanguage(GetSystemLanguage(id));
        }

        public void ChangeLanguage(SystemLanguage id)
        {
            var data = GetLanguageData(id);
            if (data != null)
            {
                data.Init();
                _currentLanguageData = data;
                IsUserSetLanguage = true;
                PlayerPrefs.SetString("CurrentLanguage", id.ToString());
                OnChangeLanguage?.Invoke();
            }
            else
            {
                Debug.LogError($"ServiceLocalization.ChangeLanguage: no language data for id '{id}'");
                ChangeLanguage(_localizationsConfig.DefaultLanguageId);
            }
        }

        // @TODO test code
        public void ChangeNextLanguage()
        {
            for (var i = 0; i < _localizationsConfig.Languages.Count; i++)
            {
                var data = _localizationsConfig.Languages[i];

                if (CurrentLanguage == data.Id)
                {
                    var index = i + 1 >= _localizationsConfig.Languages.Count ? 0 : i + 1;
                    ChangeLanguage(_localizationsConfig.Languages[index].Id);
                    break;
                }
            }
        }

        public static SystemLanguage GetSystemLanguage(string id)
        {
            foreach (var pair in pairs)
            {
                if (pair.Key == id)
                    return pair.Value;
            }

            Debug.LogError($"ServiceLocalization: Not find system language for id '{id}'");
            return SystemLanguage.English;
        }

        public static string GetId(SystemLanguage lng)
        {
            foreach (var pair in pairs)
            {
                if (pair.Value == lng)
                    return pair.Key;
            }

            Debug.LogError($"ServiceLocalization: Not find id for system language '{lng}'");
            return "en";
        }

        #endregion

        #region Private Methods

        private LocalizationData GetLanguageData(SystemLanguage id)
        {
            foreach (var languageData in _localizationsConfig.Languages)
            {
                if (languageData.IsAvailable && languageData.Id == id)
                    return languageData;
            }

            return null;
        }

//         private void InitMap(TextAsset textAsset)
//         {
//             if (textAsset != null)
//             {
//                 _textsMap.Clear();
//
//                 var fs = textAsset.text;
//                 var lines = Regex.Split(fs, "\n|\r|\r\n");
//
//                 foreach (var line in lines)
//                 {
//                     var index = line.IndexOf(SEPARATOR);
//                     if (index >= 0 && index < line.Length)
//                     {
//                         var key = line.Substring(0, index);
//                         var value = line.Substring(index + SEPARATOR.Length, line.Length - index - SEPARATOR.Length);
//
//                         if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(key))
//                         {
//                             if (!_textsMap.ContainsKey(key))
//                             {
//                                 _textsMap[key] = value;
//                             }
//                             else
//                             {
//                                 Log.E("ServiceLocalization.InitMap", $"Duplicate key '{key}'");
//                             }
//                         }
//                         else
//                         {
//                             Log.E("ServiceLocalization.InitMap", $"Wrong pair key = '{key}'   value = '{value}'");
//                         }
//
// //                        Log.S($"Key =\"{key}\"       value = \"{value}\"");
//                     }
//                 }
//             }
//             else
//             {
//                 Log.E("ServiceLocalization.InitMap", "textAsset is null");
//             }
//         }

        private void CheckWordsInAllLanguages()
        {
            // @TODO need code
        }

        private SystemLanguage GetLanguage(string id, string defaultId)
        {
            if (string.IsNullOrEmpty(id) == false)
            {
                var values = Enum.GetValues(typeof(SystemLanguage)).Cast<SystemLanguage>();

                foreach (var lang in values)
                {
                    if (lang.ToString() == id)
                    {
                        IsUserSetLanguage = true;
                        return lang;
                    }
                }
            }
            else if (string.IsNullOrEmpty(defaultId) == false)
            {
                var values = Enum.GetValues(typeof(SystemLanguage)).Cast<SystemLanguage>();

                foreach (var lang in values)
                {
                    if (lang.ToString() == defaultId)
                    {
                        IsUserSetLanguage = false;
                        return lang;
                    }
                }
            }

            IsUserSetLanguage = false;
            return _localizationsConfig.DefaultLanguageId;
        }

        #endregion
    }
}