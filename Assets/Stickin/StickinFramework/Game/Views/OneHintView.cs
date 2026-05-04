using UnityEngine;

namespace stickin
{
    [RequireComponent(typeof(ButtonHint))]
    public class OneHintView : MonoBehaviour
    {
        [SerializeField] private HintSO _hintSo;
        
        public void Init(Game game)
        {
            var btn = GetComponent<ButtonHint>();
            btn.Init(_hintSo, game);
        }
    }
}