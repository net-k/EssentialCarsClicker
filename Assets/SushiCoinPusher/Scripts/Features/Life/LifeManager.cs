using System;
using System.Collections.Generic;
using App;
using UnityEngine;

namespace Quiz.Framework.Life
{
    public class LifeManager
    {
       List<Life> _lifeList = new List<Life>();
        
        private Life GetLife(LifeSaveDataManager.LifeType lifeType)
        {
            if (_lifeList.Count == 0)
            {
                Initialize();
            }

            foreach (Life life in _lifeList)
            {
                if (life.GetLifeType() == lifeType)
                {
                    return life;
                }
            }

            return null;
        }
        
        public void Initialize()
        {
            _lifeList.Clear();
            
            // LifeType で LifeList を生成
            foreach (LifeSaveDataManager.LifeType lifeType in Enum.GetValues(typeof(LifeSaveDataManager.LifeType)))
            {
                Life life = new Life(lifeType);
                life.Initialize( GetRecoverUnitSeconds(lifeType) );
                _lifeList.Add(life);
            }
        }
        
        private int GetRecoverUnitSeconds(LifeSaveDataManager.LifeType lifeType)
        {
#if ENABLE_SRDEBUG
    if( DebugConstants.IsEnable( DebugConstants.DebugMode.SpecifyLifeRecoverTime ) )
    {
        return DebugConstants.LifeRecoverTime;
    }
#endif
            // LifeType をkey に、回復時間を value にして Dictionary に保存
            Dictionary<LifeSaveDataManager.LifeType, int> recoveryUnitSeconds = new Dictionary<LifeSaveDataManager.LifeType, int>
            {
                {LifeSaveDataManager.LifeType.Default,  60 * 60}, // 1時間
            };

            if (recoveryUnitSeconds.TryGetValue(lifeType, out int seconds))
            {
                return seconds;
            }

            // デフォルトの回復時間を返す
            return 60 * 60;
        }
        
        /// <summary>
        /// APを使用する
        /// </summary>
        /// <returns>APの使用に成功したかどうか</returns>
        /// <param name="usePoint">使用するポイント</param>
        /// <param name="lifeType"></param>
        public bool UseActionPoint(int usePoint, LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("UseActionPoint() : life is null");
                return false;
            }
            return life.UseActionPoint(usePoint);
        }

        /// <summary>
        /// ハートを回復する
        /// </summary>
        /// <param name="recoverPoint"></param>
        /// <returns></returns>
        public bool RecoverActionPoint(int recoverPoint, LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("RecoverActionPoint() : life is null");
                return false;
            }

            return life.RecoverActionPoint(recoverPoint);
        }

        /// <summary>
        /// APを全回復する
        /// </summary>
        public bool RecoveryAllPoint(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("RecoveryAllPoint() : life is null");
                return false;
            }
            return life.RecoveryAllPoint();
        }
        
        /// <summary>
        ///     次に回復する時間までのカウントダウン用ラベルを返す
        /// </summary>
        /// <returns>カウントダウン(59分59秒まで対応)</returns>
        public string GetRestRecoveryTimeLabel(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("GetRestRecoveryTimeLabel() : life is null");
                return "";
            } 
            return life.GetRestRecoveryTimeLabel();
        }

        /// <summary>
        ///     全体のポイントに対しての現在のポイントの割合を返す
        /// </summary>
        /// <returns>割合(0 ~ 1.0)</returns>
        public float ActionPointRatio(LifeSaveDataManager.LifeType lifeType)
        {
             var life = GetLife(lifeType);
             if (life == null)
             {
                 Debug.LogError("ActionPointRatio() : life is null");
                 return 0.0f;
             }
             return life.ActionPointRatio();
        }

        /// <summary>
        ///     毎フレームAPの更新を行う
        /// </summary>
        // private void Update()
        public void Update()
        {
            foreach( LifeSaveDataManager.LifeType lifeType in Enum.GetValues(typeof(LifeSaveDataManager.LifeType)))
            {
                var life = GetLife(lifeType);
                if (life == null)
                {
                    Debug.LogError("Update() : life is null");
                    return;
                }
                life.Update();
            }
        }


        public int GetLifeNum(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("GetLifeNum() : life is null");
                return 0;
            }

            return life.GetLifeNum();
        }

        public string GetLifeRecoverTime(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("GetLifeRecoverTime() : life is null");
                return "";
            }
            return life.GetLifeRecoverTime();
        }

        public bool IsMax(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError( $"IsMax() : life is null. lifeType={lifeType}");
                return false;
            }

            return life.IsMax();
        }

        public bool IsEmpty(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("IsEmpty() : life is null");
                return false;
            }

            return life.IsEmpty();
        }

        
        public float GetRestRecoveryTime(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("GetRestRecoveryTime() : life is null");
                return 0.0f;
            } 
            return life.GetRestRecoveryTime();
        }
        
        public float GetMaxRecoveryTime(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("GetMaxRecoveryTime() : life is null");
                return 0.0f;
            }
            return life.GetMaxRecoveryTime();
        }

        public int GetMaxPoint(LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("GetMaxPoint() : life is null");
                return 0;
            }
            return life.GetMaxPoint();
        }

        public int GetPoint( LifeSaveDataManager.LifeType lifeType)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError($"GetPoint() : life[{lifeType.ToString()}] is null");
                return 0;
            }
            return life.GetPoint();
        }


        public void Consume(LifeSaveDataManager.LifeType lifeType, int i)
        {
            var life = GetLife(lifeType);
            if (life == null)
            {
                Debug.LogError("Consume() : life is null");
                return;
            }
            life.UseActionPoint(i);
        }
    }
}