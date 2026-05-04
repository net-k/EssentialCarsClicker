using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class LocalizationTextData
    {
        public string Key;
        public string Value;

        public LocalizationTextData(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Stickin/Localization Data")]
    public class LocalizationData : ScriptableObject
    {
        public string Name;
        public SystemLanguage Id;
        public string LocalizedTitle;
        public List<LocalizationTextData> Texts = new();
        public bool IsAvailable;

        private Dictionary<string, string> _map = new();

        public void Init()
        {
            foreach (var textData in Texts)
            {
                _map[textData.Key] = textData.Value;
            }
        }

        public string GetLocalizedText(string id)
        {
            if (_map.ContainsKey(id))
                return _map[id];

            Debug.LogError($"Not find localized for '{Id} = {id}'");
            return id;
        }
    }
}