using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.StageButton
{
    public class PrizeView : MonoBehaviour
    {
        [SerializeField]
        Image _prizeImage;
        
        public void SetPrizeImage(Sprite sprite)
        {
            _prizeImage.sprite = sprite;
        }
    }
}
