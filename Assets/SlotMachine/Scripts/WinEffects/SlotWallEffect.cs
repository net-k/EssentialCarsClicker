using System.Collections;
using UnityEngine;
using CoinPusher.Core;
using SlotMachine.Scripts; // EffectsManagerのために必要

namespace SlotMachine
{
    public class SlotWallEffect : MonoBehaviour, ISlotWinEffect
    {
        private EffectsManager effectsManager;

        private void Awake()
        {
            effectsManager = FindObjectOfType<EffectsManager>();
        }

        public bool IsApplicable(SlotValue symbol, int score)
        {
            // box (Wall) の場合に実行
            return symbol == SlotValue.wall;
        }

        public void Execute(SlotValue symbol, int score)
        {
            if (effectsManager != null)
            {
                Debug.Log($"スロット当選！ 絵柄: {symbol} (Wall Effect via EffectsManager)");
                // EffectsManager経由で壁エフェクトを実行
                effectsManager.runEffect(CoinEffect.Effect.BumperWallCoin);
            }
            else
            {
                Debug.LogWarning("SlotWallEffect: EffectsManager not found.");
            }
        }
    }
}
