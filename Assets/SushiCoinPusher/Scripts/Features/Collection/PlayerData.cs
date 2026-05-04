using App;
using Quiz.Infrastructure;

namespace FruitShop
{
    public class PlayerData
    {
        public int GetMoney()
        {
            return SaveDataManager.Instance.GetMoney();
            // 
        }

        public int GetShopLevel()
        {
            return SaveDataManager.Instance.GetShopLevel();
        }
        
        public int SaveAddMoney(int money)
        {
            return SaveDataManager.Instance.SaveAddMoney(money);
        }

        public bool LevelUpShop()
        {
            bool ret = SaveDataManager.Instance.LevelUpShop();
            return ret;
        }

        public bool IsLevelMax()
        {
            if( GameConstants.MaxShopLevel <= GetShopLevel() )
            {
                return true;
            }
            return false;
        }
    }
}
