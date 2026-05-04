using FruitShop.MasterData;
using UnityEngine;

namespace FruitShop
{
    public class ShopUseCase
    {
        ShopLevelMaster _shopLevelMaster;
        
        ShopUseCase(ShopLevelMaster shopLevelMaster)
        {
            _shopLevelMaster = shopLevelMaster;
            shopLevelMaster.Initialize();
        }
        
        public bool GetShopLevelPrice(int nextLevel, out int price)
        {
            return _shopLevelMaster.GetPrice(nextLevel, out price);
        }

        
    }
}
