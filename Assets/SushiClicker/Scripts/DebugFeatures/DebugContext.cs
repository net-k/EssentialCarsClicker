using UnityEngine;

namespace SushiClicker.DebugFeatures
{
    /// <summary>
    /// デバッグ用コンテキスト。
    /// シーン内のデバッグ対象コンポーネントへの参照を Inspector で保持し、
    /// Awake で DebugServiceLocator に登録する。
    /// </summary>
    public class DebugContext : MonoBehaviour
    {
        [Header("デバッグ対象の参照")]
        [SerializeField] private BC_Click _bcClick;

        /// <summary>BC_Click への参照</summary>
        public BC_Click BCClick => _bcClick;

        private void Awake()
        {
            DebugServiceLocator.Register(this);
        }

        private void OnDestroy()
        {
            DebugServiceLocator.Unregister(this);
        }
    }
}
