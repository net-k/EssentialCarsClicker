using Domain.DailyBonus;
using Quiz.Framework.Life;
using SushiCatcher;
using SushiCatcher.Master;
using TohoReversi.Master;
using Zenject;

namespace SushiCoinPusher.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
           
            // Achievement
            Container.Bind<AchievementManager>().AsSingle();
            
            // Master
            Container.Bind<MasterLoader>().AsSingle();
            Container.Bind<AchievementMaster>().AsSingle();
            Container.Bind<StageMaster>().AsSingle();

            // Life
            Container.Bind<LifeManager>().AsSingle();
            Container.Bind<DailyBonus>().AsSingle();
        }
    }
}
