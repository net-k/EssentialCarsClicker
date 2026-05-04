using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.StageButton
{
    public class StageButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public Button Button => _button;

        [SerializeField] private Text _buttonText;
        

        public void SetButtonText(string buttonText)
        {
            _buttonText.text = buttonText;
        }

        public void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }
    }
}
