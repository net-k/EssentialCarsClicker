using System.Collections.Generic;

namespace stickin
{
    public class LevelsModel<T>
    {
        public List<T> Levels = new List<T>();
        
        public T GetLevelModel(int index) => Levels[index % Levels.Count];
        public T GetRandomLevel() => Levels.GetRandom();
    }
    
}