using App;
using I2.Loc;
using KumaFramework;
using KumaFramework.UI.RewardUI.AdditionalPointDialog;
using MemoryOnline.Presentation;
using Quiz.Framework.Life;
using Quiz.Infrastructure;
using Quiz.Presentation.LifeUI.LifeConsumptionCount;
using Quiz.Presentation.LifeUI.LifeGauge;
using SushiCatcher.Life;
using UnityEngine;
using Zenject;
using RewardAdsConstants = SushiCatcher.RewardAdsConstants;

namespace Quiz.Presentation.LifeUI
{
    public class LifeUIPresenter : PresenterBase
    {
        [SerializeField] private LifeUIView _view = null;
        [SerializeField] private AdditionalPointPresenter _additionalPointPresenter = null;
        [SerializeField] private LifePresenter _lifePresenter = null;
        [SerializeField] private LifeRecoverTimeGuagePresenter _lifeRecoverTimeGuagePresenter = null;
        [SerializeField] private NotEnoughLifePresenter _notEnoughLifePresenter = null;
        
        private LifeManager _lifeManager;

        [SerializeField]
        private Localize _localize = null;

        private DialogPresenter _dialogPresenter = null;
        
        [SerializeField]
        private LifeSaveDataManager.LifeType _lifeType;
        
        [Inject]
        void Construct(LifeManager lifeManager)
        {
            _lifeManager = lifeManager;
        }
        
        void Awake()
        {
            _view.RecoverButton.onClick.AddListener(() =>
            {
                int watchTimes = LifeRewardAdVideoSaveDataManager.Instance.LoadWatchPointAddedVideoTimes(LifeRewardAdVideoSaveDataManager.RecordType.Life);
                _additionalPointPresenter.Show( new AdditionalPointPresenter.DialogOption( AdditionalPointPresenter.DialogType.Life, watchTimes ) );
            });
        }
     
        private void OnAdClose()
        {
            // 再生
            // todo
            // SoundManager.Instance.Resume();
        }

        private void OnAdRewarded()
        {
            Debug.Log("LifeUIPresenter.OnAdRewarded");
            int watchTimes = LifeRewardAdVideoSaveDataManager.Instance.LoadWatchPointAddedVideoTimes(LifeRewardAdVideoSaveDataManager.RecordType.Life);
            if (watchTimes >= RewardAdsConstants.WatchPointAddedVideoTimesPerDay)
            {
                Debug.LogError("LifeUIPresenter.OnAdRewarded 視聴回数制限を超えています");
                return;
            }
            watchTimes++;

            LifeRewardAdVideoSaveDataManager.Instance.SaveWatchPointAddedVideoTimes(LifeRewardAdVideoSaveDataManager.RecordType.Life, watchTimes);

            _lifeManager.RecoverActionPoint(GameConstants.HeartNumRecoverByMovie, _lifeType);
            if (_dialogPresenter)
            {
                _dialogPresenter.Close();
                _dialogPresenter = null;
            }

            ShowRecoverDialog();
        }

        void Start()
        {
            UpdateView();
        }

        public void UpdateView()
        {
            if (!_lifeManager.IsMax(_lifeType))
            {
                // TODO あとで実装するので、いったん強制的に非表示にする。
                // _view.RecoverButton.gameObject.SetActive(true);
                _view.RecoverButton.gameObject.SetActive(false);
                int point = _lifeManager.GetPoint(_lifeType);
                
                _view.LifeText.text = $"x{point}";
                _view._lifeRecoverTime.text = _lifeManager.GetRestRecoveryTimeLabel(_lifeType);

                if (_lifeRecoverTimeGuagePresenter != null)
                {
                    _lifeRecoverTimeGuagePresenter.Show();
                }
            }
            else
            {
                _view.LifeText.text = $"x{_lifeManager.GetPoint(_lifeType).ToString()}";
                _view.RecoverButton.gameObject.SetActive(false);

                if (_lifeRecoverTimeGuagePresenter != null)
                {
                    _lifeRecoverTimeGuagePresenter.Hide();
                }
            }

            _lifePresenter.UpdateView();

            if (_lifeRecoverTimeGuagePresenter != null)
            {
                _lifeRecoverTimeGuagePresenter.Progress(_lifeManager.GetRestRecoveryTime(_lifeType),
                    _lifeManager.GetMaxRecoveryTime(_lifeType));
            }
        }
        
        public void Update()
        {
            if (_lifeManager != null)
            {
                _lifeManager.Update();
            }

            UpdateView();
        }
        
        private void OnApplicationFocus( bool hasFocus )
        {
            Debug.Log("OnApplicationFocus:" + hasFocus);
        }

        private void OnApplicationPause( bool pauseStatus )
        {
            Debug.Log("OnApplicationPause:" + pauseStatus);
        }

        private void OnApplicationQuit() {
            Debug.Log("OnApplicationQuit");
        }

        void ShowRecoverDialog()
        {
            string dialogText = LocalizationManager.GetTranslation("ライフが回復しました"); // _localize.GetText("ライフが回復しました");
            DialogPresenter.DialogType dialogType = DialogPresenter.DialogType.OK; 
            var dialog = DialogPresenter.Instance.ShowDialog(dialogText, dialogType);
            dialog.onYes += () =>
            {
                dialog.gameObject.SetActive(false);
            };
        }

        public void Show(LifeSaveDataManager.LifeType lifeType)
        {
            _lifeType = lifeType;
            _lifePresenter.Show(lifeType);
            gameObject.SetActive(true);

            LifeImage lifeImage = new LifeImage();
            lifeImage.LoadLifeImage(_lifeType, _view.LifeImage);
        }
        public void Hide()
        {
            _lifePresenter.Hide();
            gameObject.SetActive(false);
        }

        public void ShowNotEnoughLifeDialog()
        {
            _notEnoughLifePresenter.Show();
        }
    }
}