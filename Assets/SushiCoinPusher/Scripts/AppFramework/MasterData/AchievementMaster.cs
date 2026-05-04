using System.Collections;
using System.Collections.Generic;
using TohoReversi.Master;

namespace SushiCatcher.Master
{
    public class AchievementMaster : MasterBase<AchievementData>
    {
        public override bool Load()
        {
            return base.Load("Master/achievement_master");
        }

        public AchievementData FindById(int id)
        {
            if (_data == null) return null;
            
            foreach (var data in _data)
            {
                if (data.id == id)
                {
                    return data;
                }
            }

            return null;
        }

        public IEnumerable<AchievementData> GetUnlockedAchievements()
        {
            if (_data == null) yield break;
            
            foreach (var data in _data)
            {
                if (data.initial_unlock != 0)
                {
                    yield return data;
                }
            }
        }

        public IEnumerable<AchievementData> GetDataByTargetId(int prizeID)
        {
            if (_data == null) yield break;
            
            foreach (var data in _data)
            {
                if (data.target_id == prizeID)
                {
                    yield return data;
                }
            }
        }
    }
}
