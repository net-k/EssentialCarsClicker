using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    public class RewardResourceMenu : BaseMenu
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Text _countText;

        [InjectField] private ResourcesService _resourcesService;

        private Action _callbackEnd;

        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            if (data != null && data.ContainsKey("resource"))
            {
                var resource = (ResourceData)data["resource"];
                
                if (data.ContainsKey("callbackEnd"))
                    _callbackEnd = (Action)data["callbackEnd"];
                
                ShowResource(resource);
                
                Invoke(nameof(ShowEnd), 1.5f);
            }
            else
            {
                Debug.LogError("Fail show RewardResourceMenu: not find key 'resource'");
                Hide();
            }
        }

        private void ShowResource(ResourceData data)
        {
            InjectService.BindFields(this);

            _iconImage.sprite = _resourcesService.GetResourceSprite(data.Id);
            _countText.text = $"+{data.Value}";
            
            _resourcesService.ChangeResource(data.Id, data.Value);

            Show();
        }

        private void ShowEnd()
        {
            Hide();
            _callbackEnd?.Invoke();
        }
    }
}