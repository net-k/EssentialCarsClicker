using TMPro;
using UnityEngine;

namespace TohoReversi.Effect.TextEffect
{
    public class ItemGetTextPresenter : MonoBehaviour
    {
        [SerializeField]
        private ItemGetTextView _view;
        
        public TextMeshPro GetTextMeshPro()
        {
            return _view.TextMeshPro;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
