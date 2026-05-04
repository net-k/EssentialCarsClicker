using UnityEngine;

namespace KumaFramework
{
    public class PresenterBase : MonoBehaviour
    {
        public virtual void Show()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Hide()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}