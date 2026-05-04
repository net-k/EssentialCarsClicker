using UnityEngine;
using UnityEngine.UI;

namespace SushiCoinPusher.AppFramework.UI.Count
{
    public class CountView : MonoBehaviour
    {
        [SerializeField]
        private Text _countText;

        public void SetCount(int count)
        {
            _countText.text = count.ToString();
        }
    }
}
