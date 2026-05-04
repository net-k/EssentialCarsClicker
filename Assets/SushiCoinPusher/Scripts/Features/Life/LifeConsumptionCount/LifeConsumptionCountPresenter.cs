using App;
using KumaFramework;
using UnityEngine;

namespace Quiz.Presentation.LifeUI.LifeConsumptionCount
{
    /// <summary>
    /// ライフ消費数表示のプレゼンター
    /// </summary>
    public class LifeConsumptionCountPresenter : PresenterBase
    {
        [SerializeField]
        private LifeConsumptionCountView _view;

        LifeImage _lifeImage = new LifeImage();
       
        public void Initialize(int lifeConsumptionCount, LifeSaveDataManager.LifeType lifeType)
        {
            SetLifeConsumptionCount(lifeConsumptionCount);
            _lifeImage.LoadLifeImage(lifeType, _view.LifeImage);
        }
        
        private void SetLifeConsumptionCount(int lifeConsumptionCount)
        {
            _view.SetLifeConsumptionCount(lifeConsumptionCount);
        }
    }
}
