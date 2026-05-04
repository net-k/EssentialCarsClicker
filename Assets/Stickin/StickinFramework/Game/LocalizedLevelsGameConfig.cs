using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class LevelsConfig
    {
        public SystemLanguage Language;
        public TextAsset LevelsAsset;
    }

    public class LocalizedLevelsGameConfig : GameConfig
    {
        public List<LevelsConfig> Levels;
        public List<OrderAssetConfig> OrdersAssets;
        
        public T GetLevelModel<T>(int levelNumber, SystemLanguage language, OrderAssetType orderAssetType)
        {
            var levelsConfig = GetLevelsConfig(language);

            if (levelsConfig != null)
            {
                var orderConfig = GetOrderAssetConfig(orderAssetType);
                var levelIndex = levelNumber - 1;

                if (orderConfig != null)
                {
                    orderConfig.Init();
                    levelIndex = orderConfig.GetLevelIndex(levelIndex);
                }

                var levelsModel = JsonUtility.FromJson<LevelsModel<T>>(levelsConfig.LevelsAsset.text);
                return levelsModel.GetLevelModel(levelIndex);
            }

            return default;
        }

        private OrderAssetConfig GetOrderAssetConfig(OrderAssetType orderAssetType)
        {
            foreach (var config in OrdersAssets)
            {
                if (config.Type == orderAssetType)
                    return config;
            }

            return null;
        }

        private LevelsConfig GetLevelsConfig(SystemLanguage language)
        {
            foreach (var levelsConfig in Levels)
            {
                if (levelsConfig.Language == language)
                    return levelsConfig;
            }

            if (Levels.Count > 0)
                return Levels[0];
            
            Debug.LogError("Not Levels in GameConfig");
            return default;
        }
    }
}