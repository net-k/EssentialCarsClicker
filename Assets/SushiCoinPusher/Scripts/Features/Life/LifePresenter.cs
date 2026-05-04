using App;
using KumaFramework;
using SushiCatcher.Life;
using UnityEngine;
using Zenject;

namespace Quiz.Framework.Life
{
    public class LifePresenter : PresenterBase
    {
        [SerializeField]
        private LifeView _lifeView = null;
        private LifeManager _lifeManager;
        private LifeSaveDataManager.LifeType _lifeType;
        
        [Inject]
        void Construct(LifeManager lifeManager)
        {
            _lifeManager = lifeManager;
        }

        public void Show(LifeSaveDataManager.LifeType lifeType)
        {
            base.Show();
            _lifeType = lifeType;
            UpdateView();
        }
        
        public void UpdateView()
        {
            if (_lifeManager == null || _lifeView == null)
            {
                return;
            }

            _lifeView.LifeNumText.text = $"{_lifeManager.GetLifeNum(_lifeType)}/{_lifeManager.GetMaxPoint(_lifeType)}";

            if (_lifeManager.IsMax(_lifeType))
            {
                _lifeView.LifeRecoverTimeText.gameObject.SetActive(false);
                  
            }
            else
            {
                _lifeView.LifeRecoverTimeText.gameObject.SetActive(true);
                _lifeView.SetRecoverTime(_lifeManager.GetLifeRecoverTime(_lifeType));
            }
        }
        
    }
}
