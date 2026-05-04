using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// クリックボタン1種類分の画像設定。
    /// 言語やゲーム種別の知識は持たない。
    /// </summary>
    [CreateAssetMenu(fileName = "ClickButtonConfig", menuName = "SushiClicker/ClickButtonConfig")]
    public class ClickButtonConfig : ScriptableObject
    {
        [Header("ボタン画像")]
        [SerializeField] private Sprite _buttonSprite = null;

        [Header("降ってくるアイテム画像")]
        [SerializeField] private Sprite[] _rainSprites = null;

        public Sprite ButtonSprite => _buttonSprite;
        public Sprite[] RainSprites => _rainSprites;
    }
}
