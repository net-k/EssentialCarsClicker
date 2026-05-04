using App;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Presentation.LifeUI.LifeConsumptionCount
{
    public class LifeConsumptionCountView : MonoBehaviour
    {
        [SerializeField]
        private Text _lifeConsumptionCountText;

        [SerializeField]
        private Image _lifeImage;

        public Image LifeImage => _lifeImage;

        public void SetLifeConsumptionCount(int lifeConsumptionCount)
        {
            _lifeConsumptionCountText.text = lifeConsumptionCount.ToString();
        }

       
    }
}
