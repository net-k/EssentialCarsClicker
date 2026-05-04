using TohoReversi.Master;

namespace SushiClicker
{
    /// <summary>
    /// upgrade_item_master.csv を読み込むマスタークラス。
    /// </summary>
    public class UpgradeItemMaster : MasterBase<UpgradeItemData>
    {
        public override bool Load()
        {
            return base.Load("Master/upgrade_item_master");
        }

        /// <summary>
        /// item_name で検索して該当データを返す。見つからない場合は null。
        /// </summary>
        public UpgradeItemData FindByItemName(string itemName)
        {
            if (_data == null) return null;
            foreach (var data in _data)
            {
                if (data.item_name == itemName) return data;
            }
            return null;
        }
    }
}
