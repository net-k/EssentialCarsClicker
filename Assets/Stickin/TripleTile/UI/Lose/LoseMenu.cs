using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    public class LoseMenu : BaseMenu
    {
        [SerializeField] private Button _rewardBtn;

        [InjectField] private AdsService _adsService;

        private Action _rewardCallback;

        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);
            
            InjectService.BindFields(this);

            if (data != null && data.ContainsKey("rewardCallback"))
                _rewardCallback = (Action) data["rewardCallback"];
        }

        private void Start()
        {
            if (_rewardBtn != null)
                _rewardBtn.onClick.AddListener(OnClickReward);
        }

        private void OnClickReward()
        {
            if (_adsService.IsRewardAvailable())
                _adsService.ShowReward(OnRewardComplete);
            else
                TextMessageMenu.ShowWithText(TextMessageMenu.AdsNotReady);
        }

        private void OnRewardComplete()
        {
            _rewardCallback?.Invoke();
        }
    }
}
