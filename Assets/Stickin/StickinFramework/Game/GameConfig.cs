using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class GameConfig : ScriptableObject
    {
        [Header("Base params:")]
        public List<HintSO> Hints;
        public List<ScriptableObject> CustomConfigs;

        // public TutorialConfig Tutorial;
        // public DifficultsConfig Difficults;

        public T GetCustomConfig<T>() where  T : ScriptableObject
        {
            if (CustomConfigs != null)
            {
                foreach (var customConfig in CustomConfigs)
                {
                    if (customConfig.GetType() == typeof(T))
                        return customConfig as T;
                }
            }

            return default;
        }
    }
}