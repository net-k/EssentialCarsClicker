using Domain.DailyBonus;
using UnityEngine;
using Zenject;

namespace SushiCoinPusher.Features.Scene
{
    public class TitleScene : MonoBehaviour
    {
        DailyBonus _dailyBonus;
        
        [Inject]
        void Construct(DailyBonus dailyBonus)
        {
            _dailyBonus = dailyBonus;
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (_dailyBonus.ShouldResetDailyBonus() )
            {
                _dailyBonus.ResetDailyBonus();
            }  
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
