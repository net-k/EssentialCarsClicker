using KumaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Presentation.LifeUI.LifeGauge
{
    public class LifeRecoverTimeGuagePresenter : PresenterBase
    {
        [SerializeField]
        LifeRecoverTimeGuageView _view = null;
    
        public void Progress( float currentTime, float maxTime)
        {
            _view.GaugeImage.fillAmount = GetGaugeFillAmount(currentTime, maxTime);
        }

        private float GetGaugeFillAmount(float currentTime, float maxTime)
        {
            if (currentTime >= maxTime)
            {
                currentTime = maxTime;
            }
            
            float fillAmount = 1.0f - currentTime / (float)(maxTime);
            if( fillAmount > 1.0f ){
                fillAmount = 1.0f;
            }
            return fillAmount;
        }

        void UpdateColor( float fillAmount )
        {
            int r = 237;
            int g = 202 - (int)(103.0f * fillAmount);
            int b = 103 - (int)(103.0f * fillAmount);

            var color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 255 / 255.0f);
            _view.GaugeImage.GetComponent<Image>().color = color;
        }
    }
}
