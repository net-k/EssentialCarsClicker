using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace SlotMachine.Scripts
{
    [System.Serializable]
    public class SlotProbability
    {
        public SlotValue slotValue;
        public int weight;
    }

    /// <summary>
    /// スロットマシンのメインコントローラー。
    /// リールの回転制御、抽選ロジック、結果の判定、および当選時のエフェクト実行を管理します。
    /// </summary>
    public class SlotController : MonoBehaviour
    {
        public static SlotController instance { get; private set; }
        
        [SerializeField] private Reel[] reels;
        [SerializeField] private Text winText;
        
        [Header("Probability Settings")]
        [Tooltip("当選確率 (0-100%)")]
        [Range(0, 100)]
        public int winProbability = 30;
        
        [Tooltip("絵柄ごとの当選重み (合計値に対する割合で確率が決まります)")]
        public List<SlotProbability> probabilities;

        [Header("Win Effects")]
        [Tooltip("当選エフェクトを持つコンポーネント（ISlotWinEffect実装）をここに登録してください。空の場合はこのGameObjectから自動取得します。")]
        [SerializeField] private List<MonoBehaviour> registeredEffects;

        [Header("Debug Settings")]
        [Tooltip("有効にすると、指定した絵柄が必ず揃います")]
        public bool debugForceWin = false;
        public SlotValue debugTargetValue = SlotValue.coin;

        private int winPrize;
        private int totalWin; // 合計勝利数
        private bool isSpinning;

        // 当選時に実行されるエフェクトのリスト
        private List<ISlotWinEffect> winEffects;

        public event Action OnSpinEnd;

        private void Awake()
        {
            instance = this;
            
            // デフォルトの確率設定（もし空なら）
            if (probabilities == null || probabilities.Count == 0)
            {
                probabilities = new List<SlotProbability>
                {
                    new SlotProbability { slotValue = SlotValue.wall, weight = 10 },
                    new SlotProbability { slotValue = SlotValue.coin, weight = 20 },
                    new SlotProbability { slotValue = SlotValue.dia, weight = 5 },
                    new SlotProbability { slotValue = SlotValue.key, weight = 5 },
                    new SlotProbability { slotValue = SlotValue.prize, weight = 10 },
                    new SlotProbability { slotValue = SlotValue.shield, weight = 10 },
                    new SlotProbability { slotValue = SlotValue.seven, weight = 5 }
                };
            }

            InitializeWinEffects();
        }

        private void InitializeWinEffects()
        {
            winEffects = new List<ISlotWinEffect>();

            // Inspectorで登録されたものがあればそれを使う
            if (registeredEffects != null && registeredEffects.Count > 0)
            {
                foreach (var mono in registeredEffects)
                {
                    if (mono is ISlotWinEffect effect)
                    {
                        winEffects.Add(effect);
                    }
                }
            }

            // 登録がなければ、このGameObjectにアタッチされているものを自動取得
            if (winEffects.Count == 0)
            {
                winEffects.AddRange(GetComponents<ISlotWinEffect>());
            }
            
            Debug.Log($"SlotController: {winEffects.Count} 個のWinEffectが登録されました。");
        }

        private void Update()
        {
            // スピン中で、かつ全てのリールが停止したかを確認
            if (isSpinning && IsAllReelStop())
            {
                OnStopSpin();
            }
        }

        /// <summary>
        /// リール停止時の処理
        /// </summary>
        private void OnStopSpin()
        {
            isSpinning = false;
                
            // 各リールの停止位置にある絵柄を取得
            SlotValue[] reelValues = new SlotValue[reels.Length];
            for (int i = 0; i < reels.Length; i++)
            {
                reelValues[i] = reels[i]._slotValue;
            }
                
            // 結果を判定してスコアを計算
            winPrize = ResultChecker.Check(reelValues);
            totalWin += winPrize; // 合計勝利数に加算
                
            if (winText != null)
            {
                winText.text = "Win: " + totalWin; // 合計を表示
            }

            // 絵柄が揃っているか確認（最初の絵柄と全て一致するか）
            var isAligned = IsAligned(reelValues);

            // 絵柄が揃っていた場合、登録されているエフェクトを実行
            if (isAligned)
            {
                OnAligned(reelValues);
            }

            OnSpinEnd?.Invoke();
        }

        /// <summary>
        /// リールが揃った時
        /// </summary>
        /// <param name="reelValues"></param>
        private void OnAligned(SlotValue[] reelValues)
        {
            SlotValue winSymbol = reelValues[0];
            Debug.Log($"スロット当選！ 絵柄: {winSymbol}, 獲得スコア: {winPrize}");

            // 登録されているエフェクトを実行
            // 各エフェクト自身が実行条件(IsApplicable)を判定します
            if (winEffects != null)
            {
                foreach (var effect in winEffects)
                {
                    if (effect.IsApplicable(winSymbol, winPrize))
                    {
                        effect.Execute(winSymbol, winPrize);
                    }
                }
            }
        }

        private static bool IsAligned(SlotValue[] reelValues)
        {
            bool isAligned = (reelValues.Length > 0 && reelValues[0] != SlotValue.none);
            if (isAligned)
            {
                for (int i = 1; i < reelValues.Length; i++)
                {
                    if (reelValues[i] != reelValues[0])
                    {
                        isAligned = false;
                        break;
                    }
                }
            }

            return isAligned;
        }

        /// <summary>
        /// スロットを回転させます。
        /// </summary>
        public void Spin()
        {
            if (IsAllReelStop())
            {
                isSpinning = true;
                winPrize = 0;
                StartReelsWithProbability();
            }
        }

        /// <summary>
        /// 外部から強制的に特定の絵柄を揃えてスピンさせるデバッグ用メソッド
        /// </summary>
        /// <param name="target">揃えたい絵柄</param>
        public void DebugForceSpin(SlotValue target)
        {
            if (IsAllReelStop())
            {
                isSpinning = true;
                winPrize = 0;
                
                // 一時的にデバッグ設定を上書きして実行
                bool prevDebug = debugForceWin;
                SlotValue prevTarget = debugTargetValue;
                
                debugForceWin = true;
                debugTargetValue = target;
                
                StartReelsWithProbability();
                
                // 設定を戻す（必要であれば）
                debugForceWin = prevDebug;
                debugTargetValue = prevTarget;
            }
        }

        /// <summary>
        /// 確率に基づいて抽選を行い、リールの回転を開始します。
        /// </summary>
        private void StartReelsWithProbability()
        {
            SlotValue[] targetValues = new SlotValue[reels.Length];
            
            // デバッグ強制モードのチェック
            if (debugForceWin)
            {
                for (int i = 0; i < reels.Length; i++)
                {
                    targetValues[i] = debugTargetValue;
                }
                Debug.Log($"[Debug] 強制当選モード有効: {debugTargetValue} を揃えます");
            }
            else
            {
                // 通常の確率ロジック
                bool isWin = UnityEngine.Random.Range(0, 100) < winProbability;

                if (isWin)
                {
                    // 当選：重みに基づいて絵柄を決定
                    SlotValue winSymbol = GetRandomWinSymbol();
                    for (int i = 0; i < reels.Length; i++)
                    {
                        targetValues[i] = winSymbol;
                    }
                    Debug.Log($"抽選結果: 当選 ({winSymbol})");
                }
                else
                {
                    // ハズレ：揃わないようにランダムに決定
                    targetValues = GetLosingCombination();
                    Debug.Log($"抽選結果: ハズレ ({targetValues[0]}, {targetValues[1]}, {targetValues[2]})");
                }
            }

            // 各リールの回転数を計算
            // 最低回転数（ステップ数）
            // 1回転 = 7絵柄 * 4ステップ = 28ステップ
            int baseSteps = 28 * 5; // 最低5回転
            int currentBaseSteps = baseSteps;

            for (int i = 0; i < reels.Length; i++)
            {
                // ターゲットまでの最短ステップ数を取得
                int stepsToTarget = reels[i].GetStepsToTarget(targetValues[i]);
                
                // 前のリールより長く回るように調整
                // 最低でも前のリール + 2回転分くらい遅らせる
                if (i > 0)
                {
                    currentBaseSteps += 28 * 2; 
                }

                // 現在のベースステップ数 + ターゲットまでの端数
                int remainder = currentBaseSteps % 28;
                int diff = stepsToTarget - remainder;
                if (diff < 0) diff += 28;
                
                int totalSteps = currentBaseSteps + diff;
                
                reels[i].RellStart(totalSteps);
                
                // 次のリールの基準ステップ数を更新
                currentBaseSteps = totalSteps;
            }
        }

        /// <summary>
        /// 設定された重みに基づいて、当選時の絵柄をランダムに決定します。
        /// </summary>
        private SlotValue GetRandomWinSymbol()
        {
            int totalWeight = 0;
            foreach (var p in probabilities)
            {
                totalWeight += p.weight;
            }

            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int currentWeight = 0;

            foreach (var p in probabilities)
            {
                currentWeight += p.weight;
                if (randomValue < currentWeight)
                {
                    return p.slotValue;
                }
            }
            return SlotValue.coin; // フォールバック
        }

        /// <summary>
        /// ハズレとなる絵柄の組み合わせを生成します。
        /// </summary>
        private SlotValue[] GetLosingCombination()
        {
            SlotValue[] result = new SlotValue[3];
            
            // 全ての絵柄のリスト
            List<SlotValue> allValues = new List<SlotValue>();
            foreach(SlotValue val in Enum.GetValues(typeof(SlotValue)))
            {
                if (val != SlotValue.none) allValues.Add(val);
            }

            // ランダムに3つ選ぶ
            result[0] = allValues[UnityEngine.Random.Range(0, allValues.Count)];
            result[1] = allValues[UnityEngine.Random.Range(0, allValues.Count)];
            result[2] = allValues[UnityEngine.Random.Range(0, allValues.Count)];

            // 万が一揃ってしまった場合は、3つ目をずらす
            if (result[0] == result[1] && result[1] == result[2])
            {
                int index = allValues.IndexOf(result[2]);
                index = (index + 1) % allValues.Count;
                result[2] = allValues[index];
            }

            return result;
        }

        /// <summary>
        /// 全てのリールが停止しているかどうかを確認します。
        /// </summary>
        public bool IsAllReelStop()
        {
            foreach (var reel in reels)
            {
                if (!reel._reelStop)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
