namespace SushiCatcher.Collection.CollectionPage
{
    public class PrizeNameProvider
    {
        public string GetName(int prizeId)
        {
            string targetNameKey = $"prize_id_{prizeId}_name";
            return I2.Loc.LocalizationManager.GetTranslation(targetNameKey);
        }
        
        // 以下も実装して
        // prize_11_description
        public string GetDescription(int prizeId)
        {
            string targetDescriptionKey = $"prize_id_{prizeId}_description";
            return I2.Loc.LocalizationManager.GetTranslation(targetDescriptionKey);
        }

        public string GetTrivia(int prizeId)
        {
            string targetTriviaKey = $"prize_id_{prizeId}_trivia";
            return I2.Loc.LocalizationManager.GetTranslation(targetTriviaKey);
        }

        public string GetLongDescription(int prizeId)
        {
             return GetDescription(prizeId) + "\n\n" + GetTrivia(prizeId);
        }
    }
}
