using App;
using TohoReversi.Master;

namespace TohoReversi.Event.EventNavigator
{
    public class EventParamHelper
    {

        public EventParamHelper()
        {
        }

        /// <summary>
        /// イベントにライフが必要か？
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public bool IsNeedLife(int eventId)
        {
            return true;
        }

        public bool GetLifeType (int eventId, out LifeSaveDataManager.LifeType lifeType)
        {
            lifeType = LifeSaveDataManager.LifeType.Default;
            return true;
        }

        public bool CanUseMainEquipment(int instanceEventId)
        {
            return true;
        }
    }
}