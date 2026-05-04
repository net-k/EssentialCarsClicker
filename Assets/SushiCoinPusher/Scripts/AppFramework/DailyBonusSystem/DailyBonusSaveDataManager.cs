namespace App
{
    public class DailyBonusSaveDataManager : SingletonMonoBehaviour<DailyBonusSaveDataManager>
    {
        private string RecordType_DailyBonusResettedDateTime ="DailyBonusResettedDateTime";


        public void SaveResettedDailyBonusDateTime(int todayInt)
        {
            ES3.Save<int>(RecordType_DailyBonusResettedDateTime, todayInt);
        }

        public int LoadResettedDailyBonusDateTime()
        {
            if (!ES3.KeyExists(RecordType_DailyBonusResettedDateTime))
            {
                ES3.Save<int>(RecordType_DailyBonusResettedDateTime, 0);
                return 0;
            }
            return ES3.Load<int>(RecordType_DailyBonusResettedDateTime, 0 );
        }
    }
}