using Domain.DailyBonus;
using Zenject;

namespace SushiCoinPusher.AppFramework.DI
{
    public class TitleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Bindings for the Title Scene can be added here
            // DailyBonusを シングルトンとして登録 
        }
    }
}
