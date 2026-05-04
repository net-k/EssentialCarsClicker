using System;

namespace SushiClicker
{
    /// <summary>
    /// building_item_master.csv の1行に対応するデータクラス。
    /// フィールド名はCSVのヘッダー行と完全一致させること。
    /// </summary>
    [Serializable]
    public class BuildingItemData
    {
        public int id;
        public string item_name;
        public double base_cost;
        public double tick_value;
    }
}
