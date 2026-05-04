using UnityEngine;

namespace SushiCoinPusher.AppFramework.UI.Count
{
    public class CountPresenter : MonoBehaviour
    {
        [SerializeField] private CountView _view;

        public void SetCount(int count)
        {
            _view.SetCount(count);
        }
    }
}
