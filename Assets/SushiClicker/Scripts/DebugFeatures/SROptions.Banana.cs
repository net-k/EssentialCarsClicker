using System.ComponentModel;
using SushiClicker.DebugFeatures;
using UnityEngine;

#if UNITY_EDITOR
public partial class SROptions
{
    [Category("Car")]
    [DisplayName("所持くるま数")]
    public double CarCount
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
                var currentCars = bcClick.bananas;
                var diff = value - currentCars;

                if (diff < 0)
                {
                    Debug.LogWarning($"[Debug] 所持くるま数は減算できません。 current={currentCars}, target={value}");
                    return;
                }

                bcClick.AddCarsForDebug(diff);
                Debug.Log($"[Debug] 所持くるま数を設定: {value}");
            }
        }
    }

    [Category("Car")]
    [DisplayName("くるまを100万台追加")]
    public void AddOneMillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.AddCarsForDebug(1_000_000);
            Debug.Log($"[Debug] くるまを100万台追加しました。 合計: {bcClick.bananas}");
        }
    }

    [Category("Car")]
    [DisplayName("くるまを1億台追加")]
    public void AddHundredMillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.AddCarsForDebug(100_000_000);
            Debug.Log($"[Debug] くるまを1億台追加しました。 合計: {bcClick.bananas}");
        }
    }

    [Category("Car")]
    [DisplayName("くるまを10億台追加")]
    public void AddOneBillion()
    {
        var bcClick = DebugServiceLocator.BCClick;
        if (bcClick != null)
        {
            bcClick.AddCarsForDebug(1_000_000_000);
            Debug.Log($"[Debug] くるまを10億台追加しました。 合計: {bcClick.bananas}");
        }
    }

}
#endif
