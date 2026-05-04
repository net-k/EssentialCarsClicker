using KumaFramework;
using UniRx;
using UnityEngine.UI; //この行を追加する
using UnityEngine;
using Zenject;

namespace TohoReversi.Presentation.Skill
{
    public class TurnOverCountGaugeController : PresenterBase
    {
        public GameObject GaugeInsideUI; //ゲージ内部UIオブジェクト

        float GaugeMax = 100.0f; //ゲージ最大値
        float GaugeRemain = 100.0f; //ゲージ残量

        [Inject]
        void Construct()
        {
        }
        
        void Start()
        {
        }

        void Update()
        {
#if false
            if (GaugeRemain >= 1.0f)
            {
                GaugeRemain -= 1.0f;//ゲージ残量を1フレームごとに1ずつ減らす
            }
#endif
        }

        private void UpdateGauge()
        {
            float remaining = GaugeRemain / GaugeMax;
            GaugeInsideUI.GetComponent<Image>().fillAmount = remaining;
        }
    }
}
