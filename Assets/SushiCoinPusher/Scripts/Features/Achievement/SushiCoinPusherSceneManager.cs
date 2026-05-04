namespace SushiCoinPusher
{
    
    public enum SushiCaterScene
    {
        Title,
        Game,
        Achievement,
        Collection
    }
   
    public static class SushiCoinPusherSceneManager
    {

        public static void Load(SushiCaterScene game)
        {
            string sceneName = GetSceneName(game);
            if (!string.IsNullOrEmpty(sceneName))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
        }
        
        public static string GetSceneName(SushiCaterScene scene)
        {
            switch(scene)
            {
                case SushiCaterScene.Title:
                    return "TitleScene";
                case SushiCaterScene.Game:
                    return "CoinPusherScene";
                case SushiCaterScene.Achievement:
                    return "Achievement";
                case SushiCaterScene.Collection:
                    return "Collection";
                default:
                    return "";
            }
        }

    }
}