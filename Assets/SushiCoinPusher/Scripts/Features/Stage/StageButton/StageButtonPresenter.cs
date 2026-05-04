using System;
using System.Threading.Tasks;
using SushiCatcher.SaveData;
using UnityEngine;
using UniRx;

namespace SushiCatcher.StageButton
{
    public class StageButtonPresenter : MonoBehaviour
    {
        [SerializeField] private StageButtonView _view;
        private int _stageId;

        [SerializeField]
        private LockPresenter _lockPresenter;
        
        [SerializeField]
        private PrizePresenter _prizePresenter;
        
        public async Task Initialize(int stageId)
        {
            _stageId = stageId;

            string buttonText = $"Stage {stageId.ToString()}";
            _view.SetButtonText(buttonText);
            
            _lockPresenter.Initialize(stageId);

            bool isUnlocked = StageSaveDataManager.Instance.LoadStageUnlockStatus(stageId);
            _view.SetInteractable(isUnlocked);

            if (isUnlocked)
            {
                await _prizePresenter.Initialize(stageId);
            }
        }

        public IObservable<int> OnClicked => _view.Button.OnClickAsObservable().Select(_ => _stageId);
    }
}