using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [CreateAssetMenu(fileName = "ResourcesPrizesConfig", menuName = "Stickin/ResourcesPrizesConfig")]
    public class ResourcesPrizesConfig : ScriptableObject
    {
        public List<ResourcePrizeConfig> Prizes;
    }
}