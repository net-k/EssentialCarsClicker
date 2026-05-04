using System;
using I2.Loc;
using UnityEngine;

namespace KumaFramework.UI.RewardUI.AdditionalPointDialog
{
    public class DialogParams
    {
        public string captionText;
        public string tomorrowText;
    }

    public class DialogParamsFactory
    {

        public DialogParamsFactory()
        {
        }
        
        public DialogParams CreateParams(AdditionalPointPresenter.DialogType dialogType)
        {
            switch (dialogType)
            {
                case AdditionalPointPresenter.DialogType.ShopReward:
                    
                    break;
                case AdditionalPointPresenter.DialogType.Gacha:
                    break;
                case AdditionalPointPresenter.DialogType.Life:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
            }

            return CreateDefaultParams();
        }

        private DialogParams CreateDefaultParams()
        {
            var param = new DialogParams();
            param.captionText = LocalizationManager.GetTranslation("key_AdditionalPointDialogCaption");
            param.tomorrowText = LocalizationManager.GetTranslation("PointDialogTomorrowText");
            return param;
        }

        private DialogParams CreateEventParams()
        {
            var param = new DialogParams();
            param.captionText = LocalizationManager.GetTranslation("key_AdditionalPointDialogCaption_ShopReward");
            // 動画で{itemName}をゲット
            // このような形式で、itemName を設定したい
            param.tomorrowText = LocalizationManager.GetTranslation("PointDialogTomorrowText_ShopReward");
            return param;
        }

        public string BuildDialogText(AdditionalPointPresenter.DialogType dialogType)
        {
            string dialogText = "";
            switch (dialogType)
            {
                case AdditionalPointPresenter.DialogType.ShopReward:
                    RewardInfo rewardInfo = new RewardInfo();
                    if (rewardInfo == null)
                    {
                        Debug.LogError("");
                        break;
                    }
                    // 広告動画を視聴して{itemName}を{itemCount}個獲得しますか？
                    // これをReplace で置き換える
                    string localizedText = LocalizationManager.GetTranslation("PointDialogText_ShopReward");
                    
                    dialogText = localizedText. Replace("{itemName}", rewardInfo.itemName);
                    dialogText = dialogText.Replace("{itemCount}", rewardInfo.quantity.ToString() );
                    
                    break;
                case AdditionalPointPresenter.DialogType.Gacha:
                    break;
                case AdditionalPointPresenter.DialogType.Life:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
            }
            
            return dialogText;
        }
    }

    public class RewardInfo
    {
        public string itemName = "スペシャルアイテム";
        public int quantity = 1;
    }
}
        
