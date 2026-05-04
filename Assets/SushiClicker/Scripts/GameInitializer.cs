using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// ゲームシーン全体の初期化を担当するクラス。
    /// GameScene.Awake() から呼ばれる。
    ///
    /// ※ BC_ItemManager の Awake より先に実行されるよう、
    ///    GameScene の Script Execution Order を -100 に設定すること。
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Transform _buildingListContent = null;
        [SerializeField] private Transform _upgradeListContent = null;

        private readonly BuildingItemMaster _buildingItemMaster = new BuildingItemMaster();
        private readonly UpgradeItemMaster _upgradeItemMaster = new UpgradeItemMaster();

        /// <summary>
        /// GameScene.Awake() から呼ぶ。
        /// </summary>
        public void Initialize()
        {
            InitializeBuildingList();
            InitializeUpgradeList();
        }

        /// <summary>
        /// building_item_master.csv を読み込み、
        /// BuildingListContent 配下の BC_ItemManager に初期値を適用する。
        /// </summary>
        private void InitializeBuildingList()
        {
            if (_buildingListContent == null)
            {
                Debug.LogWarning("GameInitializer: BuildingListContent がアサインされていません。");
                return;
            }

            if (!_buildingItemMaster.Load())
            {
                Debug.LogError("GameInitializer: building_item_master の読み込みに失敗しました。");
                return;
            }

            var itemManagers = _buildingListContent.GetComponentsInChildren<BC_ItemManager>(includeInactive: true);

            foreach (var manager in itemManagers)
            {
                var data = _buildingItemMaster.FindByItemName(manager.itemName);
                if (data == null)
                {
                    Debug.LogWarning($"GameInitializer: item_name='{manager.itemName}' がマスターに見つかりません。");
                    continue;
                }

                manager.baseCost = data.base_cost;
                manager.cost = data.base_cost;
                manager.tickValue = data.tick_value;

                Debug.Log($"GameInitializer: '{manager.itemName}' baseCost={data.base_cost}, tickValue={data.tick_value}");
            }
        }

        /// <summary>
        /// upgrade_item_master.csv を読み込み、
        /// UpgradeListContent 配下の BC_upgradeManager に初期値を適用する。
        /// </summary>
        private void InitializeUpgradeList()
        {
            if (_upgradeListContent == null)
            {
                Debug.LogWarning("GameInitializer: UpgradeListContent がアサインされていません。");
                return;
            }

            if (!_upgradeItemMaster.Load())
            {
                Debug.LogError("GameInitializer: upgrade_item_master の読み込みに失敗しました。");
                return;
            }

            var upgradeManagers = _upgradeListContent.GetComponentsInChildren<BC_upgradeManager>(includeInactive: true);

            foreach (var manager in upgradeManagers)
            {
                var data = _upgradeItemMaster.FindByItemName(manager.itemName);
                if (data == null)
                {
                    Debug.LogWarning($"GameInitializer: item_name='{manager.itemName}' がUpgradeマスターに見つかりません。");
                    continue;
                }

                manager.baseCost = data.base_cost;
                manager.cost = data.base_cost;
                manager.clickPower = data.click_power;

                Debug.Log($"GameInitializer: '{manager.itemName}' baseCost={data.base_cost}, clickPower={data.click_power}");
            }
        }
    }
}
