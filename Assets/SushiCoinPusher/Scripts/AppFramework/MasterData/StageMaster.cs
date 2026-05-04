using System.Collections.Generic;
using System.Linq;
using TohoReversi.Master;

public class StageData
{
    public int stage_no;
    public int prize_image;
    public int clear_unlock_achievement_id;
}

public class StageMaster : MasterBase<StageData>
{
    StageMaster()
    {
        
    }
    
    public override bool Load()
    {
        return base.Load("Master/stage_master");
    }

    public List<StageData> GetAllData()
    {
        return _data.ToList();
    }
}
