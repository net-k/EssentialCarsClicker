using SlotMachine.Scripts;
using UnityEngine;

namespace SlotMachine
{
    public class SlotPresenter : MonoBehaviour
    {
        [SerializeField]
        private SlotView _slotView;

        void Awake()
        {
            _slotView.SpinButton.onClick.AddListener(() =>
            {
                SlotController.instance.Spin();
            });
        }
      
    }
}
