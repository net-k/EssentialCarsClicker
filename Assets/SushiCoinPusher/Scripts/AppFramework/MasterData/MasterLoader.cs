using System.Collections.Generic;
using SushiCatcher.Master;
using UnityEngine;

namespace TohoReversi.Master
{
    public class MasterLoader
    {
        private StageMaster _stageMaster;
        private AchievementMaster _achievementMaster;
   
        
        MasterLoader(
            StageMaster stageMaster, 
            AchievementMaster achievementMaster
            )
        {
            _stageMaster = stageMaster;
            _achievementMaster = achievementMaster;
        }
        
        public void Load()
        {
            var masters = new IMasterBase[]
            {
                _stageMaster,
                _achievementMaster
            };

            foreach (var master in masters)
            {
                if (!master.IsLoaded())
                {
                    bool isLoad = master.Load();
                    if (!isLoad)
                    {
                         Debug.LogError("Failed to load master data: " + master.GetType().Name);
                    }
                }
            } 
        }
    }
}
