using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class DifficultConfig
    {
        public int Number;
        public string Title;
    }

    [CreateAssetMenu(fileName = "DifficultsConfig", menuName = "Stickin/Difficults Config")]
    public class DifficultsConfig : ScriptableObject
    {
        public List<DifficultConfig> Difficults;

        public DifficultConfig GetDifficult(int number)
        {
            foreach (var difficult in Difficults)
            {
                if (difficult.Number == number)
                    return difficult;
            }

            return Difficults.GetRandom();
        }
    }
}
