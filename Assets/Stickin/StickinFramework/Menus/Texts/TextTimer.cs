using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Text))]
    public class TextTimer : MonoBehaviour
    {
        [SerializeField] private bool _withDoubleZero = true;
        [SerializeField] private bool _autoInit = false;
        
        private Text _txt;
        private GameTimer _timer;

        private void Start()
        {
            if (_autoInit)
            {
                var game = FindAnyObjectByType<GameView>().Game;
                var timer = game.GetGameModule<GameTimer>();
                Init(timer);
            }
        }

        public void Init(GameTimer timer)
        {
            _timer = timer;
            _txt = GetComponent<Text>();

            if (_timer != null)
                _timer.AddedCallback(OnChangeTimer);
        }

        private void OnDestroy()
        {
            if (_timer != null)
                _timer.RemoveCallback(OnChangeTimer);
        }

        private void OnChangeTimer(float seconds)
        {
            var str = StringExtensions.SecondsToText((int)seconds, _withDoubleZero);
            _txt.text = str;
        }
    }
}