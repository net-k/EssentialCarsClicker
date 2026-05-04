using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// クリックボタンの Config を保持する。
    /// </summary>
    [CreateAssetMenu(fileName = "ClickButtonConfigSelector", menuName = "SushiClicker/ClickButtonConfigSelector")]
    public class ClickButtonConfigSelector : ScriptableObject
    {
        [SerializeField] private ClickButtonConfig _config = null;

        public ClickButtonConfig GetConfig() => _config;
    }
}
