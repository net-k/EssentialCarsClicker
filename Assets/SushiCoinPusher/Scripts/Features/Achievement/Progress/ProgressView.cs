using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Achievement.Progress
{
    public class ProgressView : MonoBehaviour
    {
        [SerializeField]
        private Text _progressText;

        public void SetProgress(int current, int max)
        {
            _progressText.text = $"{current}/{max}";
        }
    }
}