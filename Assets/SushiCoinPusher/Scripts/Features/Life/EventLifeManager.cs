using App;
using Quiz.Framework.Life;
using Quiz.Presentation.LifeUI;
using Quiz.Presentation.LifeUI.LifeConsumptionCount;

namespace TohoReversi.Event.EventNavigator
{
    public class EventLifeManager
    {
        private readonly LifeUIPresenter _lifeUIPresenter;
        private readonly LifeManager _lifeManager;

        private bool _isNeedLife = true;
        private LifeSaveDataManager.LifeType _lifeType;
        private EventLifeService _eventLifeService;
        private LifeConsumptionCountPresenter _lifeConsumptionCountPresenter;
        
        public EventLifeManager(
            LifeManager lifeManager,
            LifeUIPresenter lifeUIPresenter,
            EventLifeService eventLifeService)
        {
            _lifeUIPresenter = lifeUIPresenter;
            _lifeManager = lifeManager;
            _eventLifeService = eventLifeService;
        }

        public void Initialize(int eventId)
        {
            _eventLifeService.Initialize(eventId);
            _isNeedLife = true; // 決め打ちで Life は必要とする。
            
            _lifeUIPresenter.Show();
            if (_lifeConsumptionCountPresenter)
            {
                _lifeConsumptionCountPresenter.Hide();
            }
#if false
            if (_eventLifeService.IsNeedLife())
            {
                // bool ret = _eventParamHelper.GetLifeType(eventId, out LifeSaveDataManager.LifeType lifeType);
                bool ret = _eventLifeService.GetLifeType(out LifeSaveDataManager.LifeType lifeType);
                if (ret)
                {
                    _lifeType = lifeType;
                    _isNeedLife = true;
                    _lifePresenter.Show(lifeType);
                    if (_lifeConsumptionCountPresenter)
                    {
                        _lifeConsumptionCountPresenter.Show();
                        _lifeConsumptionCountPresenter.Initialize(GameConstants.EventLifeConsumeCount, _lifeType);
                    }
                }
            }
#endif
        }

        public bool CanProceed()
        {
            return !_isNeedLife || !_eventLifeService.IsLifeEmpty(); // !_lifeManager.IsEmpty(_lifeType);
        }

        public void ShowNotEnoughLifeDialog()
        {
            _lifeUIPresenter.ShowNotEnoughLifeDialog();
        }

        public void Update()
        {
            if (_isNeedLife)
            {
                _lifeUIPresenter.Update();
            }
        }

        public void Consume(int i)
        {
            if (_isNeedLife)
            {
                _lifeManager.Consume(_lifeType, i);
                if (_lifeConsumptionCountPresenter)
                {
                //    _lifeConsumptionCountPresenter.DecreaseCount(i);
                }
            }
        }

        public int GetCurrentLife()
        {
            if (_isNeedLife)
            {
                return _lifeManager.GetPoint(_lifeType);
            }

            return 0;
        }
    }
} 