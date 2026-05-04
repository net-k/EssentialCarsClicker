using System;

namespace SushiCatcher.Master
{
    enum AchievementType
    {
        None = 0,
        CollectPrize = 1,
    }
    
    [Serializable]
    public class AchievementData
    {
        public int id;
        public string title;
        public string description;
        public int achievement_type;
        public int target_id;
        public int goal_value;
        public int sort_order;
        public int initial_unlock;
        public int next_unlock_id;
    }
}
