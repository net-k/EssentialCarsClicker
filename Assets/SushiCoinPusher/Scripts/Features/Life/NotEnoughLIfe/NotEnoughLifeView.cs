using UnityEngine;
using UnityEngine.UI;

namespace MemoryOnline.Presentation
{
    public class NotEnoughLifeView : MonoBehaviour
    {
        [SerializeField]
        private Button _okButton;

        public Button OkButton => _okButton;
    }
}
