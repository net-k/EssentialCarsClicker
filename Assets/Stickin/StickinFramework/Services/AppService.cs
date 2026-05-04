using System;
using UnityEngine;

namespace stickin
{
    public class AppService : BaseService
    {
        [SerializeField] private GameConfig _gameConfig;

        public GameConfig GameConfig => _gameConfig;
        public bool IsDebug => _appData.IsDebug;
        
        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            base.Init(appData, callbackComplete);
            
#if UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID
            Application.targetFrameRate = 60;
#else
            Application.targetFrameRate = -1;
#endif

            gameObject.AddComponent<Updater>();

            InjectService.Bind<AppService>(this);
            InitComplete(true);
        }
    }
}