using UnityEngine;

namespace SushiCatcher.Collection
{
    public class NorenPresenter : MonoBehaviour
    {
        [SerializeField] private NorenView norenView;

        private void Start()
        {
            if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                norenView.ShowJapanese();
            }
            else
            {
                norenView.ShowEnglish();
            }
        }
    }
}
