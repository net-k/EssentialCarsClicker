using System.Collections.Generic;
using UnityEngine;

namespace FruitShop.MasterData
{
    public class ShopLevelMaster
    {
        class ShopLevelData
        {
            public int price;
        }
        
        // Dictionary で level が key で取得できるようにする
        Dictionary<int, ShopLevelData> shopLevelDataList = new Dictionary<int, ShopLevelData>();

        public void Initialize()
        {
            // shopLevelDataList を 1 - 12 までデータを初期化する
            for (int i = 1; i <= 12; i++)
            {
                ShopLevelData shopLevelData = new ShopLevelData();
                // レベルが上がるに連れて、値段が上がるようにする
                shopLevelData.price = 1000 * i + 1000;    
                shopLevelDataList.Add(i, shopLevelData);
            }
        }
        
        /// <summary>
        /// level から price を取得するが、見つからなかったら false を返す
        ///
        /// price は引数で返す
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool GetPrice(int level, out int price)
        {
            if (shopLevelDataList.ContainsKey(level))
            {
                price = shopLevelDataList[level].price;
                return true;
            }

            price = 0;
            return false;     
        }
    }
}
