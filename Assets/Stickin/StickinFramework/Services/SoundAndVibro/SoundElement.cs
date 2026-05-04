using UnityEngine;

namespace stickin
{
    public class SoundElement : MonoBehaviour
    {
        [SerializeField] private string _soundId;
        [SerializeField] private VibrationType _vibrationType = VibrationType.None;
        
        [InjectField] private SoundsAndVibroService _soundsAndVibroService;
        
        private void Awake()
        {
            InjectService.BindFields(this);
        }

        public void Play()
        {
            if (!string.IsNullOrEmpty(_soundId))
                _soundsAndVibroService.PlaySound(_soundId);
            
            if (_vibrationType != VibrationType.None)
                _soundsAndVibroService.Vibration(_vibrationType);
        }
    }
}