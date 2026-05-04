using SushiCoinPusher.AppFramework.UI.Count;
using UnityEngine;

namespace SushiCoinPusher.Features.Collection.SushiSlot
{
    public class SushiSlotPresenter : MonoBehaviour
    {
        [SerializeField]
        private CountPresenter countPresenter;

        public void SetCount(int count)
        {
            if (countPresenter != null)
            {
                countPresenter.SetCount(count);
            }
        }
    }
}
