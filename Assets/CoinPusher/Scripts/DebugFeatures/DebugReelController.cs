#if UNITY_EDITOR || DEVELOPMENT_BUILD
using CoinPusher.Core;
using UnityEngine;
using SlotMachine.Scripts;
using System.Collections;
using SlotMachine;

namespace CoinPusher.DebugFeatures
{
    public class DebugReelController : MonoBehaviour
    {
        [Tooltip("Key to align all reels")]
        public KeyCode alignKey = KeyCode.R;

        [Tooltip("Key to resume spinning all reels")]
        public KeyCode resumeKey = KeyCode.T;

        [Tooltip("Key to trigger Coin Tower")]
        public KeyCode coinTowerKey = KeyCode.C;
        
        [Tooltip("Key to spawn CollectableDonut")]
        public KeyCode donutKey = KeyCode.D;
        
        [Tooltip("Key to spawn Dice")]
        public KeyCode diceKey = KeyCode.I;

        private Spinner[] spinners;
        private EffectsManager effectsManager;
        private CoinManager coinManager;
        private CoinSpawner coinSpawner;
        
        [SerializeField]
        private FeverEffect feverEffect;

        void Start()
        {
            spinners = FindObjectsOfType<Spinner>();
            effectsManager = FindObjectOfType<EffectsManager>();
            coinManager = FindObjectOfType<CoinManager>();
            coinSpawner = FindObjectOfType<CoinSpawner>();
        }

        void Update()
        {
            if (Input.GetKeyDown(alignKey))
            {
                AlignAllReels();
            }

            if (Input.GetKeyDown(resumeKey))
            {
                ResumeAllReels();
            }

            if (Input.GetKeyDown(coinTowerKey))
            {
                TriggerCoinTower();
            }
            
           
            if (Input.GetKeyDown(diceKey))
            {
                if (coinSpawner != null)
                {
                    coinSpawner.SpawnDice();
                    Debug.Log("Debug: SpawnDice triggered.");
                }
                else
                {
                    Debug.LogWarning("Debug: CoinSpawner not found.");
                }
            }
            
            // 壁出現
            if( Input.GetKeyDown(KeyCode.B))
            {
                if (effectsManager != null)
                {
                    effectsManager.runEffect(CoinEffect.Effect.BumperWallCoin);
                    Debug.Log("Debug: Wall Appear effect triggered.");
                }
                else
                {
                    Debug.LogWarning("Debug: EffectsManager not found.");
                }
            }
            
            // フィーバー状態
            if (Input.GetKeyDown(KeyCode.F))
            {
                // FeverEffect  でのフィーバー処理を呼び出す
                // EffectManager ではなくて、FerverEffect クラスのメソッドを直接呼び出す必要があるかもしれません
                var feverEffect = FindObjectOfType<FeverEffect>();
                if (feverEffect != null)
                {
                    feverEffect.StartFever();
                }
            }
            
            // prize effect
            if (Input.GetKeyDown(KeyCode.P))
            {
                 DebugSpawnPrizes();
            }

        }
        
    

        private void DebugSpawnPrizes()
        {
            var prizeEffect = FindObjectOfType<SlotPrizeEffect>();
            if (prizeEffect != null)
            {
                    // SlotValue.prize として実行
                    prizeEffect.Execute(SlotValue.prize, 0);
            }
            else
            {
                Debug.LogWarning("Debug: SlotPrizeEffect not found.");
            }
        }

        public void AlignAllReels()
        {
            if (spinners == null) return;

            foreach (var spinner in spinners)
            {
                if (spinner != null)
                {
                    spinner.AlignReel();
                }
            }
            Debug.Log("Debug: All reels aligned.");
        }

        public void ResumeAllReels()
        {
            if (spinners == null) return;

            foreach (var spinner in spinners)
            {
                if (spinner != null)
                {
                    spinner.StartSpinning();
                }
            }
            Debug.Log("Debug: All reels resumed spinning.");
        }

        public void TriggerCoinTower()
        {
            var coinTowerEffect = FindObjectOfType<SlotCoinTowerEffect>();
            if (coinTowerEffect != null)
            {
                coinTowerEffect.Execute(SlotValue.coin, 0);
                Debug.Log("Debug: SlotCoinTowerEffect triggered.");
            }
            else
            {
                Debug.LogWarning("Debug: SlotCoinTowerEffect not found.");
            }
        }
    }
}
#endif
