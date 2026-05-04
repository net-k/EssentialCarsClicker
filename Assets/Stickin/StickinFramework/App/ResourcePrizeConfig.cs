using UnityEngine;

namespace stickin
{
    [CreateAssetMenu(fileName = "ResourcePrizeConfig", menuName = "Stickin/ResourcePrizeConfig")] // for purchase, ads, dailyBonus, spin and other
    public class ResourcePrizeConfig : ScriptableObject
    {
        public ResourceData[] Resources;
        
        [InjectField] private ResourcesService _resourcesService;

        public void Collected(Transform transform = null)
        {
            InjectService.BindFields(this);
            
            foreach (var resourceData in Resources)
                _resourcesService.ChangeResource(resourceData.Id, resourceData.Value, transform);
        }

        public ResourceData GetResource(string id)
        {
            foreach (var resourceData in Resources)
            {
                if (resourceData.Id == id)
                    return resourceData;
            }

            return null;
        }
    }
}