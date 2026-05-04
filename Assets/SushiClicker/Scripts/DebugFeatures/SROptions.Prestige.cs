using System.ComponentModel;
using SushiClicker.DebugFeatures;
using UnityEngine;

public partial class SROptions
{
    [Category("Prestige")]
    [DisplayName("Prestige Level")]
    public double PrestigeLevelDebug
    {
        get
        {
            var bcClick = DebugServiceLocator.BCClick;
            return bcClick != null ? bcClick.PrestigeLevel : 0;
        }
        set
        {
            var bcClick = DebugServiceLocator.BCClick;
            if (bcClick != null)
            {
                bcClick.PrestigeLevel = value;
                Debug.Log($"[Debug] Prestige Level を設定: {value}");
            }
        }
    }

    [Category("Prestige")]
    [DisplayName("Golden Bananas")]
    public double GoldenBananasDebug
    {
        get
        {
            var bcClick = DebugServiceLocator.BCClick;
            return bcClick != null ? bcClick.GoldenBananas : 0;
        }
        set
        {
            var bcClick = DebugServiceLocator.BCClick;
            if (bcClick != null)
            {
                bcClick.GoldenBananas = value;
                Debug.Log($"[Debug] Golden Bananas を設定: {value}");
            }
        }
    }

    [Category("Prestige")]
    [DisplayName("バナナを1兆個追加 (Prestige 準備)")]
    public void AddOneTrillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.AddBananas(1_000_000_000_000);
            Debug.Log($"[Debug] バナナを1兆個追加しました。 bananaTrillionCount: {bcClick.bananaTrillionCount}");
        }
    }

    [Category("Prestige")]
    [DisplayName("Prestige を実行")]
    public void ExecutePrestige()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.BuyPrestige();
            Debug.Log($"[Debug] Prestige を実行しました。 PrestigeLevel: {bcClick.PrestigeLevel}, GoldenBananas: {bcClick.GoldenBananas}");
        }
    }

    [Category("Prestige")]
    [DisplayName("Prestige をリセット (Level & Golden Bananas を 0 に)")]
    public void ResetPrestige()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.PrestigeLevel = 0;
            bcClick.GoldenBananas = 0;
            Debug.Log("[Debug] Prestige Level と Golden Bananas を 0 にリセットしました");
        }
    }
}
