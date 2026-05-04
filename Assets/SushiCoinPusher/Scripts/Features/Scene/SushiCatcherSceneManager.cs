namespace SushiCatcher
{
    
    public enum SushiCaterScene
    {
        Title,
        Game,
        Achievement,
        Collection,
        Support
    }
   
    public static class SushiCatcherSceneManager
    {

        public static void Load(SushiCaterScene game)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(GetSceneName(game));
        }
        
        public static string GetSceneName(SushiCaterScene scene)
        {
            switch(scene)
            {
                case SushiCaterScene.Title:
                    return "TitleScene";
                case SushiCaterScene.Game:
                    return "BClickerScene";
                case SushiCaterScene.Achievement:
                    return "Achievement";
                case SushiCaterScene.Collection:
                    return "Collection";
                case SushiCaterScene.Support:
                    return "SupportScene";
                default:
                    return "";
            }
        }

    }
}
