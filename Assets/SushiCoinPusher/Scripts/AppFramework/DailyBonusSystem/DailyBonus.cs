using System;
using App;

namespace Domain.DailyBonus
{
    public class DailyBonus 
    {
        
        DailyBonus( )
        {
        }
        /// <summary>
        /// デイリーボーナスをリセットする
        ///
        /// リセットすると、次にデイリーボーナスを取得できるのは 1日後となる
        /// </summary>
        public void ResetDailyBonus()
        {
            var todayInt = GetTodayInt();
            LifeRewardAdVideoSaveDataManager.Instance.SaveWatchPointAddedVideoTimes(LifeRewardAdVideoSaveDataManager.RecordType.Life, 0);
        }

        public bool ShouldResetDailyBonus()
        {
            var todayInt = GetTodayInt();
            var resetDay = DailyBonusSaveDataManager.Instance.LoadResettedDailyBonusDateTime();
            // (前回リセットから）１日以上経過していれば、再度リセットすべき
            // リセットすべきタイミングということは、ログインボーナスを取得できるということでもある
            if (todayInt - resetDay > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 現在の日付を整数表現にして取得する
        /// </summary>
        /// <returns></returns>
        public static int GetTodayInt()
        {
            var now = DateTime.Now;
            int todayInt = 0;
            todayInt = now.Year * 1000 + now.Month * 100 + now.Day;
            return todayInt;
        }
    }
}