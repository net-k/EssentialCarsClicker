using UnityEngine;
using UnityEngine.UI;

namespace SushiCoinPusher.Features.Collection.SushiSlot
{
    public class SushiSlotView : MonoBehaviour
    {
        [SerializeField]
        private RawImage _renderImage;
        public RawImage RenderImage => _renderImage;

        [SerializeField]
        private Button _button;
        public Button Button => _button;

        private void Awake()
        {
            // Inspectorで設定されていなかった場合に、子オブジェクトから自動で取得する
            if (_renderImage == null)
            {
                _renderImage = GetComponentInChildren<RawImage>();
                if (_renderImage == null)
                {
                    Debug.LogError("RawImage component not found on SushiSlotView or its children.", this);
                }
            }

            if (_button == null)
            {
                _button = GetComponentInChildren<Button>();
                if (_button == null)
                {
                    // ボタンは必須ではないかもしれないのでWarningに留める
                    Debug.LogWarning("Button component not found on SushiSlotView or its children.", this);
                }
            }
        }
    }
}
