using System;
using UnityEngine;

namespace stickin
{
    public class ResourcesService : BaseService
    {
        public const string CoinKey = "coin";
        public const string LevelKey = "level";
        public const string PrivacyPolicy = "PrivacyPolicy";
        
        private const string FILENAME = "ResourcesSave.dat";
        
        [SerializeField] private ResourcesConfig _resourcesConfig;
        
        private UserData _userData;

        public event Action<string, double, Transform> OnChangeResource;
        public event Action<string, double, Transform> OnUserUpdate;
        
        #region Static methods
        public static void SetResource(string id, string value) => PlayerPrefs.SetString(id, value);
        public static string GetResourceValueString(string id) => PlayerPrefs.GetString(id);
        #endregion
        
        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            base.Init(appData, callbackComplete);
            
            InjectService.Bind<ResourcesService>(this);
            
            _userData = SaveHelper.Load<UserData>(FILENAME) ?? new UserData();
            
            // #if UNITY_EDITOR
            // _userData.SetResource(LevelKey, 15); // test
            // #endif
            
            if (_resourcesConfig != null)
                _userData.Init(_resourcesConfig);
            else
                Debug.LogError("ResourcesService.Init: _resourcesConfig is null");

            // increase sessions
            var sessions = GetResourceValue(UserData.SESSIONS);
            SetResource(UserData.SESSIONS, sessions + 1);
            
            InitComplete(true);
            
// #if UNITY_EDITOR
//             SetResource("word", 30);
// #endif

        }

        public void SetResource(string id, bool value)
        {
            SetResource(id, value ? 1 : 0);
        }
        
        public void SetResource(string id, double value, bool needSave = true)
        {
            _userData.SetResource(id, value);
            if (needSave)
                Save();

            OnUserUpdate?.Invoke(id, value, null);
        }

        public void DeleteResource(string id)
        {
            _userData.DeleteResource(id);
            Save();

            OnUserUpdate?.Invoke(id, _userData.GetResourceData(id).Value, null);
        }

        public void ChangeResource(string id, double value, Transform fromTransform = null)
        {
            var newValue = _userData.ChangeResource(id, value);
            Save();

            OnChangeResource?.Invoke(id, value, fromTransform);
            OnUserUpdate?.Invoke(id, newValue, fromTransform);
        }

        public int GetResourceValueInt(string id, int defaultValue = 0) => (int) GetResourceValue(id, defaultValue);

        public double GetResourceValue(string id, int defaultValue = 0) => _userData != null ? _userData.GetResourceData(id, defaultValue).Value : defaultValue;
        
        public Sprite GetResourceSprite(string id) => _userData.GetResourceSprite(id);
        
        public int GetSessions() => GetResourceValueInt(UserData.SESSIONS);
        
        private void Save() => SaveHelper.Save(_userData, FILENAME);
    }
}