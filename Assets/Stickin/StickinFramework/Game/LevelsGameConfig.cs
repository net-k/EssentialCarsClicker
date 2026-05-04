using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class LevelsGameConfig : GameConfig
    {
        [Header("Levels")] 
        public List<OrderAssetConfig> OrdersAssets;
        public TextAsset LevelsTextAsset;
        
        public virtual LevelsModel<T> GetLevelsModels<T>()
        {
            return JsonUtility.FromJson<LevelsModel<T>>(LevelsTextAsset.text);
        }
        
        public T GetLevelModel<T>(LevelsModel<T> levelsModel, int index, OrderAssetType orderType)
        {
            var orderAsset = GetOrderAsset(orderType);
            index = orderAsset.GetLevelIndex(index);

            Debug.Log($"Max levels count = {levelsModel.Levels.Count}");
            
            return levelsModel.GetLevelModel(index);
        }
        
        public T GetLevelModel<T>(int number, OrderAssetType orderType)
        {
            var index = number - 1;
            var levelsModel = GetLevelsModels<T>();
            return GetLevelModel(levelsModel, index, orderType);
        }

        private OrderAssetConfig GetOrderAsset(OrderAssetType type)
        {
            foreach (var orderAsset in OrdersAssets)
            {
                if (orderAsset.Type == type)
                    return orderAsset;
            }

            return null;
        }
    }
}