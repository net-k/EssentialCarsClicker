using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// クリックボタンの画像切り替えを制御するPresenter。
    /// CakeSprites が設定されている場合はランダムに選択して表示する。
    /// 未設定の場合は Config の ButtonSprite を適用する。
    /// </summary>
    public class ClickButtonPresenter : MonoBehaviour
    {
        [SerializeField] private ClickButtonView _view = null;
        [SerializeField] private ClickButtonConfig _config = null;

        [Header("ランダム表示するケーキ画像（設定時はConfigより優先）")]
        [SerializeField] private Sprite[] _cakeSprites = null;

#if UNITY_EDITOR
        [Header("デバッグ: 固定表示するケーキのインデックス（-1でランダム / Editorのみ有効）")]
        [SerializeField] private int _debugFixedCakeIndex = -1;
#endif

        private void Start()
        {
            if (_cakeSprites != null && _cakeSprites.Length > 0)
            {
#if UNITY_EDITOR
                // 固定表示はデバッグ用途のためEditor実行時のみ有効
                if (_debugFixedCakeIndex >= 0 && _debugFixedCakeIndex < _cakeSprites.Length)
                {
                    ApplyFixedCake(_debugFixedCakeIndex);
                }
                else
                {
                    ApplyRandomCake();
                }
#else
                ApplyRandomCake();
#endif
            }
            else
            {
                ApplyConfig();
            }
        }

        /// <summary>
        /// CakeSprites からランダムに1枚選んでViewに反映する。
        /// </summary>
        public void ApplyRandomCake()
        {
            var sprite = _cakeSprites[Random.Range(0, _cakeSprites.Length)];
            _view.SetButtonSprite(sprite);
        }

        /// <summary>
        /// CakeSprites の指定インデックスのケーキを固定表示する。
        /// </summary>
        /// <param name="index">表示するケーキのインデックス</param>
        public void ApplyFixedCake(int index)
        {
            _view.SetButtonSprite(_cakeSprites[index]);
        }

        /// <summary>
        /// Config の ButtonSprite をViewに反映する。
        /// </summary>
        public void ApplyConfig()
        {
            if (_config == null)
            {
                Debug.LogWarning("ClickButtonPresenter: ClickButtonConfig がアサインされていません。");
                return;
            }

            _view.SetButtonSprite(_config.ButtonSprite);
        }
    }
}
