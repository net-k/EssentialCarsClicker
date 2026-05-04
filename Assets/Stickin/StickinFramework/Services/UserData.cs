using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class UserData
    {
        public const string SESSIONS = "sessions";
        public const string GAME_PLAYS = "gamePlays";
    
        public string Name;
        public List<ResourceData> Resources;

        [System.NonSerialized] private ResourcesConfig _resourcesConfigData;

        public UserData()
        {
            Resources = new List<ResourceData>();
            Name = "";
        }

        public void Init(ResourcesConfig config)
        {
            _resourcesConfigData = config;
        }

        public double ChangeResource(string id, double value)
        {
            var resource = GetResourceData(id);
            resource.Value += value;

            return resource.Value;
        }
    
        public void SetResource(string id, double value)
        {
            var resource = GetResourceData(id);
            resource.Value = value;
        }

        public void DeleteResource(string id)
        {
            foreach (var res in Resources)
            {
                if (res.Id == id)
                {
                    Resources.Remove(res);
                    break;
                }
            }
        }

        public ResourceData GetResourceData(string id, int defaultValue = 0)
        {
            foreach (var resource in Resources)
            {
                if (resource.Id == id)
                    return resource;
            }

            var newResource = new ResourceData();
            newResource.Id = id;
            newResource.Value = GetResourceDefaultValue(id, defaultValue);

            Resources.Add(newResource);
        
            return newResource;
        }

        private double GetResourceDefaultValue(string id, int defaultValue)
        {
            if (_resourcesConfigData != null)
            {
                foreach (var resource in _resourcesConfigData.ResourcesDouble)
                {
                    if (resource.Id == id)
                        return resource.DefaultValue;
                }
            }

            return defaultValue;
        }

        public Sprite GetResourceSprite(string id)
        {
            if (_resourcesConfigData != null)
            {
                foreach (var resource in _resourcesConfigData.ResourcesDouble)
                {
                    if (resource.Id == id)
                        return resource.Sprite;
                }
            }
        
            Debug.LogError($"UserData.GetResourceSprite: not sprite for resource = {id}");
            return null;
        }

        public string GetResourceString(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}