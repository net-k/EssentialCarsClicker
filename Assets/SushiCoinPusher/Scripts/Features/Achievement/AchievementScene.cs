using TohoReversi.Master;
using UnityEngine;
using Zenject;

namespace SushiCoinPusher.Features.Achievement
{
    public class AchievementScene : MonoBehaviour
    {
        MasterLoader _masterLoader;
        
        [Inject]
        void Construct(MasterLoader masterLoader)
        {
            _masterLoader = masterLoader;
        }
        
        void Awake()
        {
            _masterLoader.Load();
        }
    }
}
