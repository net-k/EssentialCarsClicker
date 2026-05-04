namespace stickin
{
    public abstract class ASavable
    {
        protected int _levelNumber;
        protected LevelsProgressService _levelsProgressService;

        public ASavable(int levelNumber, LevelsProgressService levelsProgressService)
        {
            _levelNumber = levelNumber;
            _levelsProgressService = levelsProgressService;
            
            if (_levelsProgressService != null)
                _levelsProgressService.OnSaveBegin += Save;
        }
        
        public abstract void Save();
        
        public bool IsExistSave()
        {
            return _levelsProgressService != null && _levelsProgressService.IsExistProgress(_levelNumber, LevelProgressType.Started);
        }

        protected void OnDestroy()
        {
            if (_levelsProgressService != null)
                _levelsProgressService.OnSaveBegin -= Save;
        }
    }
}