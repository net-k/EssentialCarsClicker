using System.ComponentModel;
using App;

public partial class SROptions
{
    [Category("Life")]
    [DisplayName("Add Default Life")]
    public void AddShortStoryLife()
    {
        LifeSaveDataManager.Instance.RecoverLife(1, LifeSaveDataManager.LifeType.Default);
    }
}