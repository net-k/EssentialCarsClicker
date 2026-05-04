using UnityEngine;

namespace stickin
{
    public class DebugElement : MonoBehaviour
    {
        [InjectField] private AppService _appService;

        private void Start()
        {
            InjectService.BindFields(this);
            
            gameObject.SetActive(_appService.IsDebug);
        }
    }
}