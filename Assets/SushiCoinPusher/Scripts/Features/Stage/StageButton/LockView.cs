using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.StageButton
{
    public class LockView : MonoBehaviour
    {
        [SerializeField]
        Image _lockImage;
        
        public void SetLockState(bool isLocked)
        {
            gameObject.SetActive(isLocked);
        }
    }
}
