using TMPro;
using UnityEngine;

namespace TohoReversi.Effect.TextEffect
{
    public class ItemGetTextView : MonoBehaviour
    {
//    [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private TextMeshPro textMeshPro;
    
        // _textMeshPro の getter
        // public TextMeshProUGUI TextMeshPro => _textMeshPro;
        public TextMeshPro TextMeshPro => textMeshPro;
    }
}
