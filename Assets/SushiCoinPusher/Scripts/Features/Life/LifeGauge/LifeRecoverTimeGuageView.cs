using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Presentation.LifeUI.LifeGauge
{
    public class LifeRecoverTimeGuageView : MonoBehaviour
    {
        [SerializeField]
        Image _gaugeImage = null;

        public Image GaugeImage => _gaugeImage;
    }
}
