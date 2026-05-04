using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class TutorialStep
    {
        public Sprite Image;
        public string Text;
    }
    
    [CreateAssetMenu(fileName = "TutorialConfig", menuName = "Stickin/TutorialConfig")]
    public class TutorialConfig : ScriptableObject
    {
        public List<TutorialStep> Steps;
    }
}