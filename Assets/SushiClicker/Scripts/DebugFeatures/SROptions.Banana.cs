using System.ComponentModel;
using SushiClicker.DebugFeatures;
using UnityEngine;

public partial class SROptions
{
    [Category("Banana")]
    [DisplayName("バナナ総数")]
    public double BananaCount
    {
        get
        {
            var bcClick = DebugServiceLocator.BCClick;
            return bcClick != null ? bcClick.bananas : 0;
        }
        set
        {
            var bcClick = DebugServiceLocator.BCClick;
            if (bcClick != null)
            {
                bcClick.bananas = value;
                Debug.Log($"[Debug] バナナ総数を設定: {value}");
            }
        }
    }

    [Category("Banana")]
    [DisplayName("バナナを100万個追加")]
    public void AddOneMillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.bananas += 1_000_000;
            Debug.Log($"[Debug] バナナを100万個追加しました。 合計: {bcClick.bananas}");
        }
    }

    [Category("Banana")]
    [DisplayName("バナナを1億個追加")]
    public void AddHundredMillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.bananas += 100_000_000;
            Debug.Log($"[Debug] バナナを1億個追加しました。 合計: {bcClick.bananas}");
        }
    }

    [Category("Banana")]
    [DisplayName("バナナを10億個追加")]
    public void AddOneBillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.bananas += 1_000_000_000;
            Debug.Log($"[Debug] バナナを10億個追加しました。 合計: {bcClick.bananas}");
        }
    }

    [Category("Banana")]
    [DisplayName("バナナを0にリセット")]
    public void ResetBananas()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.bananas = 0;
            Debug.Log("[Debug] バナナを0にリセットしました");
        }
    }
}
