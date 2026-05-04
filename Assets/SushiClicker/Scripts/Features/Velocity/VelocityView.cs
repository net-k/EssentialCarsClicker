using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    /// <summary>
    /// バナナ毎秒（BPS）の表示を担当するView
    /// </summary>
    public class VelocityView : MonoBehaviour
    {
        [SerializeField] private Text _velocityText = null;

        private string _unit = "";
        
        void Awake()
        {
            _unit = I2.Loc.LocalizationManager.GetTranslation("key_BananasPerSec");
        }
        
        /// <summary>
        /// BPSを設定する。単位部分はI2ローカライズで翻訳する。
        /// 表示例: "1.23 バナナ/秒" / "1.23 bananas/sec"
        /// </summary>
        public void SetVelocity(double bps)
        {
            string number = BC_currencyConverter.Instance.GetCurrencyIntoString(bps, false, false);
            _velocityText.text = $"{number} {_unit}";
        }
    }
}
