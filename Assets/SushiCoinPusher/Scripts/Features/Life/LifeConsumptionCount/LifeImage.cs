using App;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Presentation.LifeUI.LifeConsumptionCount
{
    public class LifeImage
    {
        public void LoadLifeImage(LifeSaveDataManager.LifeType lifeType, Image lifeImage)
        {
                
            // LifeType によって、Sprite のカラーを変更する
            switch (lifeType)
            {
                case LifeSaveDataManager.LifeType.Default:
                    lifeImage.color = Color.white;
                    break;
                /*
                case LifeSaveDataManager.LifeType.MusicFes:
                    lifeImage.color = Color.red;
                    break;
                case LifeSaveDataManager.LifeType.RaidBoss:
                    lifeImage.color = Color.cyan;
                    break;
                case LifeSaveDataManager.LifeType.RaidBossEvent:
                    lifeImage.color = Color.green;
                    break;
                case LifeSaveDataManager.LifeType.ShortStory:
                    lifeImage.color = Color.yellow;
                    break;
                */
                default:
                    lifeImage.color = Color.white;
                    break;
            }
                
        }
    }
}