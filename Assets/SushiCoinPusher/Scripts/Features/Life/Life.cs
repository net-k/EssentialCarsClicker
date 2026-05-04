using System;
using App;
using Quiz.Infrastructure;
using UnityEngine;

namespace Quiz.Framework.Life
{
    public class Life
    {
        // 1ActionPoint回復をするのに必要な時間
        // private const int RecoveryUnitSeconds = 60 * 5; // 秒
        private int _recoveryUnitSeconds = 60 * 60; // 秒 (60分)
        private DateTime _lastRecoveryTime;
        private double _restRecoveryTime;

        public int Point { get; private set; }
        public int MaxPoint { get; private set; }
        
        private LifeSaveDataManager.LifeType _lifeType;

        public Life(LifeSaveDataManager.LifeType lifeType)
        {
            _lifeType = lifeType;
        }
        
        // 1ActionPoint回復をするのに必要な時間
        // private const int RecoveryUnitSeconds = 60 * 5; // 秒
               
        public void Initialize(int recoverUnitSeconds)
        {
            _recoveryUnitSeconds = recoverUnitSeconds;
            Init(LifeSaveDataManager.Instance.LoadLife(_lifeType), 
                GameConstants.LifeMaxNum,
                LifeSaveDataManager.Instance.LoadLifeLastRecoverTime(_lifeType));
        }
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="point">現在のポイント</param>
        /// <param name="maxPoint">最大のポイント</param>
        /// <param name="lastRecoveryTime">最後に回復した時間</param>
        private void Init(int point, int maxPoint, DateTime lastRecoveryTime)
        {
            this.Point = point;
            this.MaxPoint = maxPoint;
            _lastRecoveryTime = lastRecoveryTime;
        }
        
        /// <summary>
        /// APを使用する
        /// </summary>
        /// <returns>APの使用に成功したかどうか</returns>
        /// <param name="usePoint">使用するポイント</param>
        public bool UseActionPoint(int usePoint)
        {
            if (Point < usePoint) return false;
            if (Point >= MaxPoint)
            {
                _lastRecoveryTime = DateTime.Now;
                LifeSaveDataManager.Instance.SaveLifeLastRecoverTime(_lastRecoveryTime, _lifeType);
            }
        
            Point -= usePoint;
        
            if (Point < 0)
            {
                Debug.LogError("UseActionPoint() : Point is minus");
                Point = 0;
                return false;
            }
                    
            LifeSaveDataManager.Instance.ConsumeLife(usePoint, _lifeType);
            return true;
        }
        
        /// <summary>
        /// ハートを回復する
        /// </summary>
        /// <param name="recoverPoint"></param>
        /// <returns></returns>
        public bool RecoverActionPoint(int recoverPoint)
        {
            // もし（故意・過失の）時間変更でマイナスになった場合のセーフティ
            if (Point < 0)
            {
                Point = 1;
                LifeSaveDataManager.Instance.SaveLife(1, _lifeType);
                return false;
            }
                    
            Point += recoverPoint;
            if (Point >= MaxPoint)
            {
                    
                RecoveryAllPoint();
            }
            else
            {
                LifeSaveDataManager.Instance.RecoverLife(recoverPoint, _lifeType);
                        
            }
                    
            return true;
        }
        
        /// <summary>
        /// APを全回復する
        /// </summary>
        public bool RecoveryAllPoint()
        {
            Point = MaxPoint;
            _lastRecoveryTime = DateTime.Now;
            LifeSaveDataManager.Instance.SaveLife(Point, _lifeType);
            LifeSaveDataManager.Instance.SaveLifeLastRecoverTime(DateTime.Now, _lifeType);
            return true;
        }
                
        /// <summary>
        /// 次に回復する時間までのカウントダウン用ラベルを返す
        /// </summary>
        /// <returns>カウントダウン(1時間以上はhh:mm:ss形式、1時間未満はmm:ss形式)</returns>
        public string GetRestRecoveryTimeLabel()
        {
            if (Point >= MaxPoint) return "00:00";
            var span = TimeSpan.FromSeconds(_restRecoveryTime);

            if (span.TotalHours >= 1)
            {
                return string.Format("{0:00}:{1:00}:{2:00}", (int)span.TotalHours, span.Minutes, span.Seconds);
            }
            else
            {
                return string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);
            }
        }
        
        /// <summary>
        ///     全体のポイントに対しての現在のポイントの割合を返す
        /// </summary>
        /// <returns>割合(0 ~ 1.0)</returns>
        public float ActionPointRatio()
        {
            if (Point >= MaxPoint) return 1f;
        
            if (Point == 0) return 0f;
        
            return (float) Point / MaxPoint;
        }
        
        /// <summary>
        ///     毎フレームAPの更新を行う
        /// </summary>
        // private void Update()
        public void Update()
        {
            UpdateActionPointStatus();
        }
        
        /// <summary>
        /// APの更新処理を行う
        /// </summary>
        private void UpdateActionPointStatus()
        {
            if (Point >= MaxPoint) return;
        
            var time = DateTime.Now;
            var diff = time - _lastRecoveryTime;
        
            var totalSeconds = diff.TotalSeconds;
        
            while (totalSeconds > _recoveryUnitSeconds)
            {
                if (Point >= MaxPoint)
                {
                    LifeSaveDataManager.Instance.SaveLifeLastRecoverTime(DateTime.Now, _lifeType);
                    break;
                }
        
                totalSeconds -= _recoveryUnitSeconds;
                _lastRecoveryTime = _lastRecoveryTime.Add(TimeSpan.FromSeconds(_recoveryUnitSeconds));
                Point++;
        
                LifeSaveDataManager.Instance.SaveLife(Point, _lifeType);
                LifeSaveDataManager.Instance.SaveLifeLastRecoverTime(_lastRecoveryTime, _lifeType);
            }
        
            _restRecoveryTime = _recoveryUnitSeconds - totalSeconds;
        }
        
        
        public int GetLifeNum()
        {
            return this.Point;
        }
        
        public string GetLifeRecoverTime()
        {
            return this.GetRestRecoveryTimeLabel();
        }
        
        public bool IsMax()
        {
            if (this.Point >= this.MaxPoint)
            {
                return true;
            }
        
            return false;
        }
        
        public bool IsEmpty()
        {
            if (this.Point <= 0)
            {
                return true;
            }
        
            return false;
        }
        
                
        public float GetRestRecoveryTime()
        {
            return (float)_restRecoveryTime;
        }
                
        public float GetMaxRecoveryTime()
        {
            return _recoveryUnitSeconds;
        }

        public LifeSaveDataManager.LifeType GetLifeType()
        {
            return _lifeType;
        }

        public int GetMaxPoint()
        {
            return MaxPoint;
        }

        public int GetPoint()
        {
            return Point;
        }
    }
}