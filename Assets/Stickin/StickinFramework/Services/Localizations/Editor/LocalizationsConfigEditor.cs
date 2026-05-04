using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
#if ST_CSV
using CsvHelper; // added package from url:    https://github.com/yoshida190/CsvHelper.git
#endif
using UnityEngine;

namespace stickin
{
    [CustomEditor(typeof(LocalizationsConfig))]
    public class LocalizationsConfigEditor : Editor
    {
        private class RecordData
        {
            public string Key;
            public string Value;

        }

        private class LangData
        {
            public SystemLanguage Lang;
            public List<RecordData> Records = new();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var config = target as LocalizationsConfig;

            if (GUILayout.Button("Parse"))
            {
                var csvPath = AssetDatabase.GetAssetPath(config.CsvFile);
                var savePath = csvPath.Substring(0, csvPath.LastIndexOf("/"));
                
                Parse(config, csvPath, savePath);
            }
        }

        private void Parse(LocalizationsConfig config, string csvPath, string savePath)
        {
            AssetDatabase.StartAssetEditing();

            config.Clear();
            
            try
            {
                var langKeys = ParseHeader(csvPath, 1); // keys
                var localizedNames = ParseHeader(csvPath, 3); // localizedNames

                var langsMap = new Dictionary<string, LangData>();
                var localizedNamesMap = new Dictionary<string, string>();

                for (var i = 0; i < langKeys.Count; i++)
                {
                    var langKey = langKeys[i];
                    var localizedName = localizedNames[i];

                    if (string.IsNullOrEmpty(langKey))
                        continue;

                    var langData = ParseLang(csvPath, langKey);
                    if (langData != null)
                    {
                        Debug.Log($"{langData.Lang} = {langData.Records.Count} words");

                        langsMap[langKey] = langData;
                        localizedNamesMap[langKey] = localizedName;
                    }
                }

                foreach (var pair in langsMap)
                {
                    var langKey = pair.Key;
                    var langData = pair.Value;

                    var filename = Path.Combine(savePath, $"{langKey}.asset");
                    var localizationData = ScriptableObject.CreateInstance<LocalizationData>();
                    
                    bool exists = File.Exists(filename);
                    if (exists)
                        localizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(filename);
                    else
                        AssetDatabase.CreateAsset(localizationData, filename);

                    localizationData.Name = langKey;
                    localizationData.Id = langData.Lang;
                    localizationData.LocalizedTitle = localizedNamesMap[langKey];
                    localizationData.IsAvailable = true;

                    localizationData.Texts = new List<LocalizationTextData>();
                    foreach (var recordData in langData.Records)
                        localizationData.Texts.Add(new LocalizationTextData(recordData.Key, recordData.Value));

                    EditorUtility.SetDirty(localizationData);
                    
                    config.Add(localizationData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
            
            EditorUtility.SetDirty(config);

            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private List<string> ParseHeader(string csvPath, int startIndex)
        {
            var result = new List<string>();
            
#if ST_CSV
            using var streamReader = new StreamReader(csvPath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            for(var i = 0; i < startIndex; i++)
                csvReader.Read();
        
            for (var i = 1; i < csvReader.Context.Record.Length; i++)
            {
                string field;
                var isExist = csvReader.TryGetField(i, out field);
                if (isExist)
                    result.Add(field);
            }
#endif
            return result;
        }
        
        private LangData ParseLang(string csvPath, string langKey)
        {
#if ST_CSV
            var result = new LangData();
            result.Lang = LocalizationService.GetSystemLanguage(langKey);

            using var streamReader = new StreamReader(csvPath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();
            csvReader.Read();
            csvReader.Read();

            while (csvReader.Read())
            {
                var record = new RecordData
                {
                    Key = csvReader.GetField("key"),
                    Value = csvReader.GetField(langKey)
                };

                if (string.IsNullOrEmpty(record.Key) == false &&
                    string.IsNullOrEmpty(record.Value) == false)
                {
                    result.Records.Add(record);
                }
            }

            return result;
#else
            Debug.LogError("Added define ST_CSV and added package from: https://github.com/yoshida190/CsvHelper.git");
            return null;
#endif
        }
    }
}