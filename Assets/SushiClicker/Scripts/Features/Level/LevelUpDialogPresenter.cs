using KumaFramework;
using Quiz.Framework.Ad.AdMob;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TohoReversi.Shop;

namespace SushiClicker
{
    /// <summary>
    /// レベルアップダイアログのPresenter。
    /// レベルアップイベントを受け取り、ダイアログの表示・広告の再生・報酬付与を制御する。
    /// </summary>
    public class LevelUpDialogPresenter : PresenterBase
    {
        [SerializeField] private LevelUpDialogView _view = null;
        [SerializeField] private AdMobRewardedInterstitial _adMobRewardedInterstitial = null;
        [SerializeField] private BC_Click _bcClick = null;
        [SerializeField] private CommonDialogPresenter _commonDialogPresenter = null;

        // 付与した（もしくは付与予定の）すし個数を保持する
        private double _grantedSushiCount = 0;
        // ボタンに表示する2倍報酬（動画視聴時の獲得予定額）
        private double _doubledRewardDisplay = 0;

        /// <summary>
        /// シーンロードのたびに、非アクティブなダイアログを強制的にアクティブにして
        /// Awake（イベント購読処理）を走らせるための初期化。
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                var presenters = UnityEngine.Object.FindObjectsOfType<LevelUpDialogPresenter>(true);
                foreach (var p in presenters)
                {
                    if (p != null && !p.gameObject.activeInHierarchy)
                    {
                        // 一時的にアクティブにして Awake を実行させる
                        p.gameObject.SetActive(true);
                    }
                }
            };
        }

        private void Awake()
        {
            if (_adMobRewardedInterstitial != null)
                _adMobRewardedInterstitial.OnAdRewarded += HandleAdRewarded;
            
            PlayerLevelManager.Instance.OnLevelUp += Open;
            _view.SetOnRewardButtonClicked(OnRewardButtonClicked);
            _view.SetOnOkButtonClicked(OnOkButtonClicked);
            
            // 初期状態では非表示にする
            Hide();
        }

        private void OnDestroy()
        {
            if (_adMobRewardedInterstitial != null)
                _adMobRewardedInterstitial.OnAdRewarded -= HandleAdRewarded;
            if (PlayerLevelManager.Instance != null)
                PlayerLevelManager.Instance.OnLevelUp -= Open;
        }

        public void Open(int fromLevel, int toLevel, double reward)
        {
            // PlayerLevelManager で既にキャップされた報酬を受け取ったので、
            // 念のため上限を超えないようガード（累積ではなく差分 = Range で判定）
            double currentLevelThreshold = PlayerLevelManager.GetThreshold(toLevel);
            double nextLevelThreshold = PlayerLevelManager.GetThreshold(toLevel + 1);
            double range = nextLevelThreshold - currentLevelThreshold;
            
            // 基本報酬は (0.5 / VideoRewardMultiplier) 倍でキャップ
            double maxRewardBase = range * (0.5 / PlayerLevelManager.VideoRewardMultiplier);
            // 動画報酬は 50% キャップ
            double maxRewardMultiplied = range * 0.5;
            
            // 念のためのガード：報酬が上限を超えていないか確認
            double cappedReward = Math.Min(reward, maxRewardBase);
            
            // 動画視聴報酬は、基本報酬の VideoRewardMultiplier 倍（最大キャップ 50% まで）
            double multipliedRewardValue = Math.Min(cappedReward * PlayerLevelManager.VideoRewardMultiplier, maxRewardMultiplied);
            
            _grantedSushiCount = cappedReward;
            _doubledRewardDisplay = multipliedRewardValue;
            
            Debug.Log($"LevelUpDialogPresenter.Open: Level {fromLevel} -> {toLevel}, baseReward={reward}, cappedReward={cappedReward}, Multiplier={PlayerLevelManager.VideoRewardMultiplier}, multipliedReward={multipliedRewardValue}");

            // キャプションをローカライズしてタイトルを設定
            string caption = I2.Loc.LocalizationManager.GetTranslation("key_LevelUpDialog_Caption");
            
            // レベルを言語に応じて表示（「レベル」または「Level」）
            string levelLabel = GetLevelLabel(toLevel);
            
            // 報酬を単位付きで表示
            string sushiCountWithUnit = BC_currencyConverter.Instance.GetCurrencyIntoString(cappedReward, false, false);
            _view.SetLevelUpTitle($"{caption}\n{levelLabel}", fromLevel, toLevel);

            // 報酬テキスト（基本報酬を表示）
            string rewardTemplate = I2.Loc.LocalizationManager.GetTranslation("key_LevelUpDialog_RewardText");
            string rewardText = rewardTemplate
                .Replace("{sushiCount}", sushiCountWithUnit)
                .Replace("\\n", "\n");
            _view.SetRewardText(rewardText);

            // 報酬ボタンのテキスト（動画視聴で VideoRewardMultiplier 倍になることを案内）
            string rewardButtonTemplate = I2.Loc.LocalizationManager.GetTranslation("key_LevelUpDialog_RewardButton");
            string multipliedSushiCountWithUnit = BC_currencyConverter.Instance.GetCurrencyIntoString(multipliedRewardValue, false, false);
            string rewardButtonText = rewardButtonTemplate
                .Replace("{multiplier}", PlayerLevelManager.VideoRewardMultiplier.ToString("0"))
                .Replace("{sushiCount}", multipliedSushiCountWithUnit)
                .Replace("\\n", "\n");
            _view.SetRewardButtonText(rewardButtonText);

            Show();
        }

        public override void Show()
        {
            base.Show();
            _view.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _view.Hide();
        }

        private void OnRewardButtonClicked()
        {
            if (_adMobRewardedInterstitial == null || !_adMobRewardedInterstitial.Show())
            {
                Debug.LogWarning("LevelUpDialogPresenter: リワードインタースティシャル広告のロードが完了していません");
                Hide();
                return;
            }
            Hide();
        }

        private void OnOkButtonClicked()
        {
            // OKボタン：基本報酬（_grantedSushiCount）を付与
            if (_bcClick != null && _grantedSushiCount > 0)
            {
                _bcClick.AddBananas(_grantedSushiCount);
                Debug.Log($"LevelUpDialogPresenter.OnOkButtonClicked: AddBananas({_grantedSushiCount}) called");
            }
            
            _grantedSushiCount = 0;
            Hide();
        }

        private void HandleAdRewarded()
        {
            // 動画視聴時：2倍報酬を付与（OKボタンのルートとは異なる）
            double doubledReward = _doubledRewardDisplay;
            
            // 広告報酬は追加で付与（別途）
            double adAmount = 0;
            if (_adMobRewardedInterstitial != null)
                adAmount = (double)_adMobRewardedInterstitial.LastRewardAmount;

            if (_bcClick != null && doubledReward > 0)
            {
                _bcClick.AddBananas(doubledReward);
                Debug.Log($"LevelUpDialogPresenter.HandleAdRewarded: AddBananas({doubledReward}) called");
            }

            if (_bcClick != null && adAmount > 0)
            {
                _bcClick.AddBananas(adAmount);
            }

            if (doubledReward > 0)
            {
                // CommonDialog にはトータルで取得した数を表示（2倍報酬 + 広告報酬）
                double totalReward = doubledReward + adAmount;
                string sushiCountWithUnit = BC_currencyConverter.Instance.GetCurrencyIntoString(totalReward, false, false);
                string template = I2.Loc.LocalizationManager.GetTranslation("key_LevelUpReward_Success_Message");
                string message = template
                    .Replace("{sushiCount}", sushiCountWithUnit)
                    .Replace("\\n", "\n");
                if (_commonDialogPresenter != null)
                {
                    Hide();
                    _commonDialogPresenter.ShowWithMessage("key_LevelUpReward_Success_Title", message);
                }

                _grantedSushiCount = 0;
                _doubledRewardDisplay = 0;
            }
        }

        /// <summary>
        /// 現在の言語に応じてレベルラベルを返す（「レベル」または「Level」）
        /// </summary>
        private string GetLevelLabel(int level)
        {
            string language = I2.Loc.LocalizationManager.CurrentLanguage;
            if (language == "ja")
                return $"レベル {level}";
            else
                return $"Level {level}";
        }
    }
}
