using System;
using Aquarium.Presentation.GameScene.AdditionalPointDialog;
using I2.Loc;
using Quiz.Framework.Ad.AdMob;
using SushiCatcher;
using TohoReversi.Shop;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace KumaFramework.UI.RewardUI.AdditionalPointDialog
{
    [RequireComponent(typeof(AdditionalPointView))]
    public class AdditionalPointPresenter : MonoBehaviour
    {
        private AsyncOperation async;
        private AdditionalPointView _view = null;

        [SerializeField]
        private AdMobRewardVideo _adMobRewardVideo = null;

        [SerializeField] private CommonDialogPresenter _commonDialogPresenter = null;

        private DialogPresenter _dialogPresenter = null;

        [SerializeField] private GameObject LoadingPanel = null;

        public enum DialogType
        {
            ShopReward,
            Gacha,
            Life
        }

        DialogType dialogType = DialogType.ShopReward;

        public class DialogOption
        {
            public DialogType dialogType;
            public int watchTimes = 0;
        
            public DialogOption(DialogType dialogType, int watchTimes)
            {
                this.dialogType = dialogType;
                this.watchTimes = watchTimes;
            }
        }

        private AdditionalPointPresenter.DialogOption _dialogOption;
     
        private DialogParamsFactory _dialogParamsFactory = null;
        private DialogParams _dialogParams = null;
        
        [Inject]
        public void Construct()
        {
        
        }
    
        public void Awake()
        {
            _view = GetComponent<AdditionalPointView>();
            if (_view == null)
            {
                throw new Exception("view の取得に失敗");
            }

            SetEvents();
        }

        private void SetEvents()
        {
            _view.MovieButton.onClick.AddListener(OnMovieButtonClicked);
            _view.CloseButton.onClick.AddListener(OnCloseButtonClicked);

        }
        
        public void Show(AdditionalPointPresenter.DialogOption dialogOption)
        {
            Initialize(dialogOption);
        }

        private void Initialize(DialogOption dialogOption)
        {
            _dialogOption = dialogOption;
        
            gameObject.SetActive(true);
            
            _dialogParamsFactory = new DialogParamsFactory();
            _dialogParams = _dialogParamsFactory.CreateParams(dialogOption.dialogType);

            _view.CaptionText.text = _dialogParams.captionText;
        }

        void OnMovieButtonClicked()
        {
            OpenMovieDialog(_dialogOption );
        }

        private void OpenMovieDialog(AdditionalPointPresenter.DialogOption dialogOption)
        {
           _view.MovieButton.GetComponentInChildren<Text>().text = LocalizationManager.GetTranslation("AdditionalPointDialogMovieButton" );

            var dialogText = BuildDialogText(dialogOption, _dialogParams, _dialogParamsFactory, out var dialogType, _dialogOption.watchTimes);
            var dialog = DialogPresenter.Instance.ShowDialog(dialogText, dialogType);
            dialog.onYes += () => { OnYesButton(dialog); };
            dialog.onNo += () =>
            {
                // Debug.Log("リワード動画広告 Dialog.OnNo");
            };
        }

        private static string BuildDialogText(AdditionalPointPresenter.DialogOption dialogOption, DialogParams dialogParams,
            DialogParamsFactory dialogParamsFactory, out DialogPresenter.DialogType dialogType, int watchTimes)
        {
            string dialogText = "";
            string dialogContext =	string.Format( LocalizationManager.GetTranslation("PointDialogLimitText"), watchTimes.ToString(), RewardAdsConstants.WatchPointAddedVideoTimesPerDay.ToString() ) ;
            dialogType = DialogPresenter.DialogType.OK;
            if (watchTimes >= RewardAdsConstants.WatchPointAddedVideoTimesPerDay)
            {
                // dialogText = string.Format( LocalizationManager.GetTranslation("PointDialogTomorrowText"));
                dialogText = dialogParams.tomorrowText;
            }
            else
            {
                //	動画を見てポイントを追加しますか
                dialogText = dialogParamsFactory.BuildDialogText( dialogOption.dialogType );

                dialogType = DialogPresenter.DialogType.YesNo;
            }

            dialogContext = dialogContext.Replace( "\\n", Environment.NewLine );
            dialogText = dialogText + dialogContext;
            return dialogText;
        }

        private void OnYesButton(DialogPresenter dialog)
        {
            if (_adMobRewardVideo == null)
            {
                string captionKey = "key_ErrorCaption";
                string messageKey = "key_RewardAdFailedToLoad";
                _commonDialogPresenter.Show(captionKey, messageKey);
                // Debug.LogError("_adMobRewardVideo is null リワード動画は再生できません");
                // DialogPresenter.Instance.ShowDialog("動画の読み込みに失敗しました", DialogPresenter.DialogType.OK);

                return;
            }

            if (!_adMobRewardVideo.Show() )
            {
                string captionKey = "key_ErrorCaption";
                string messageKey = "key_RewardAdFailedToLoad";
                _commonDialogPresenter.Show(captionKey, messageKey);
                // DialogPresenter.Instance.ShowDialog("動画の読み込みに失敗しました", DialogPresenter.DialogType.OK);
            }
            else
            {
                // SoundManager.Instance.Suspend();
                dialog.Close();
                gameObject.SetActive(false);
            }
        }


        void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }


        public void Update()
        {
            if (_adMobRewardVideo == null || !_adMobRewardVideo.IsLoaded())
            {
                if (LoadingPanel != null)
                {
                    LoadingPanel.SetActive(true);
                }

                _view.MovieButton.interactable = false;
            }
            else
            {
                if (LoadingPanel != null)
                {
                    LoadingPanel.SetActive(false);
                }

                _view.MovieButton.interactable = true;
            }
        }
    }
}