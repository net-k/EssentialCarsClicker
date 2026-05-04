using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [CreateAssetMenu(fileName = "LocalizationsConfig", menuName = "Stickin/Localizations Config")]
    public class LocalizationsConfig : ScriptableObject
    {
        public TextAsset CsvFile;
        public SystemLanguage DefaultLanguageId;
        public List<LocalizationData> Languages;

        public void Clear()
        {
            Languages.Clear();
        }
        
        public void Add(LocalizationData data)
        {
            Languages.Add(data);
        }
        
        public void Print()
        {
            var str = "All languages in LocalizationsData:\n";
            foreach (var l in Languages)
            {
                str += $"{l.Name} = {l.LocalizedTitle}\n";
            }

            Debug.Log(str);
        }
    }
}