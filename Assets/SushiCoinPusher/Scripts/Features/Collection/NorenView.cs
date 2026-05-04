using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Collection
{
    public class NorenView : MonoBehaviour
    {
        [SerializeField] private Image japaneseNoren;
        [SerializeField] private Image englishNoren;

        public void ShowJapanese()
        {
            if (japaneseNoren != null) japaneseNoren.gameObject.SetActive(true);
            if (englishNoren != null) englishNoren.gameObject.SetActive(false);
        }

        public void ShowEnglish()
        {
            if (japaneseNoren != null) japaneseNoren.gameObject.SetActive(false);
            if (englishNoren != null) englishNoren.gameObject.SetActive(true);
        }
    }
}
