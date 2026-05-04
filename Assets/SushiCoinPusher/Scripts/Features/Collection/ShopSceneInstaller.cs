using FruitShop.MasterData;
using Zenject;

namespace FruitShop.App
{
    public class ShopSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ShopLevelMaster>().AsSingle();
            Container.Bind<ShopUseCase>().AsSingle();
            Container.Bind<PlayerData>().AsSingle();
        }
    }
}