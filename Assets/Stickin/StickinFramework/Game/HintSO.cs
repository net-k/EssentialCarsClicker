using stickin;
using UnityEngine;

namespace stickin
{
    [CreateAssetMenu(fileName = "Hint", menuName = "Stickin/HintConfig")]
    public class HintSO : ScriptableObject
    {
        public string ResourceId;
        public Sprite Icon;
        public int Price;
        public int CountInOneGame = 0;
        public string LogicClass;
        public HintPriceType PriceType = HintPriceType.Ad;
        
        public ResourcePrizeConfig resourcePrizeConfig;
        
        [InjectField] private ResourcesService _resourcesService;

        public bool TryBuy(Transform transform = null)
        {
            InjectService.BindFields(this);
            
            var coins = _resourcesService.GetResourceValueInt(ResourcesService.CoinKey);

            if (coins >= Price && resourcePrizeConfig != null)
            {
                resourcePrizeConfig.Collected(transform);
                _resourcesService.ChangeResource(ResourcesService.CoinKey, -Price);
            
                return true;
            }

            return false;
        }

    }
}