public class ShisenShoHelper
{
    
    public static string GetPrefabName(int haiValue )
    {
        string path = "Tiles/";
        int v = haiValue;
        
     //    Debug.LogError($"GetTextureName value={haiValue.ToString()}");
 
        string prefabName = "";
        if (v < 1)
        {
            return "";
        }
        else if (v < 5) // 1, 2, 3, 4
        {
            switch (v)
            {
                case 1:
                    prefabName = "pjihai_ton";
                    break;
                case 2:
                    prefabName = "pjihai_nan";
                    break;
                case 3:
                    prefabName = "pjihai_sha";
                    break;
                case 4:
                    prefabName = "pjihai_pe";
                    break;
            }
            //textureName = "Tiles/Winds/wind_" + (v-1 + 1).ToString();
            // return '東南西北中發　'[v - 1];
        }
        else if (v < 8) // 5, 6 , 7
        {
            switch (v)
            {
                case 5:
                    prefabName = "pjihai_haku";
                    break;
                case 6:
                    prefabName = "pjihai_hatsu";
                    break;
                case 7:
                    prefabName = "pjihai_chun";
                    break;
            }
        }
        else if (v < 17)
        {
            prefabName = "pmanzu_" + (v - 8 + 1).ToString();
        }
        else if (v < 26)
        {
            prefabName = "ppinzu_" + (v - 17 + 1).ToString();
        }
        else if (v < 35)
        {
            prefabName = "psouzu_" + (v - 26 + 1).ToString();
        }

        return path + prefabName;
    }
    
    
    public static string GetTextureName(int haiValue )
    {
        int v = haiValue;
        
     //    Debug.LogError($"GetTextureName value={haiValue.ToString()}");
 
        string textureName = "";
        if (v < 1)
        {
            return "";
        }
        else if (v < 5) // 1, 2, 3, 4
        {
            textureName = "Tiles/Winds/wind_" + (v-1 + 1).ToString();
            // return '東南西北中發　'[v - 1];
        }
        else if (v < 8) // 5, 6 , 7
        {
            textureName = "Tiles/Dragons/dragon_" + (v-5 + 1).ToString();
        }
        else if (v < 17)
        {
            textureName = "Tiles/Bamboo/bamboo_" + (v - 8 + 1).ToString();
        }
        else if (v < 26)
        {
            textureName = "Tiles/Characters/character_" + (v - 17 + 1).ToString();
        }
        else if (v < 35)
        {
            textureName = "Tiles/Circles/circle_" + (v - 26 + 1).ToString();
        }
         
        return textureName.ToString();
    }
     
}
