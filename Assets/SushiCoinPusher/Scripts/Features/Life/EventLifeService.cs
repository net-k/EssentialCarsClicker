using App;
using Quiz.Framework.Life;

namespace TohoReversi.Event.EventNavigator
{
    public class EventLifeService
    {
        private int _eventId;
        private readonly EventParamHelper _eventParamHelper;
        private readonly LifeManager _lifeManager;

        public EventLifeService(EventParamHelper eventParamHelper, LifeManager lifeManager)
        {
            _eventParamHelper = eventParamHelper;
            _lifeManager = lifeManager;
        }
        
        public void Initialize(int eventId)
        {
            _eventId = eventId;
        }

        public bool IsNeedLife()
        {
            if (_eventParamHelper.IsNeedLife(_eventId))
            {
                return true;
            }

            return false;
        }

        public bool IsLifeEmpty()
        {
            bool ret = _eventParamHelper.GetLifeType(_eventId, out LifeSaveDataManager.LifeType lifeType);
            if (ret)
            {
                return _lifeManager.IsEmpty(lifeType); 
            }

            return true;
        }

        public bool CanProceed()
        {
            return (!IsNeedLife() || !IsLifeEmpty() );
        }
        
        public bool GetLifeType(out LifeSaveDataManager.LifeType lifeType)
        {
            return _eventParamHelper.GetLifeType(_eventId, out lifeType);
        }
    }
}