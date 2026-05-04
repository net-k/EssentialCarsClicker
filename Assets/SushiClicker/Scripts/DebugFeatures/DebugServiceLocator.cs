using UnityEngine;

namespace SushiClicker.DebugFeatures
{
    /// <summary>
    /// デバッグ用サービスロケータ。
    /// DebugContext から登録された参照を SROptions 等に提供する。
    /// </summary>
    public static class DebugServiceLocator
    {
        private static DebugContext _context;

        /// <summary>
        /// DebugContext を登録する。DebugContext.Awake() から呼ばれる。
        /// </summary>
        public static void Register(DebugContext context)
        {
            _context = context;
            Debug.Log("[DebugServiceLocator] DebugContext registered.");
        }

        /// <summary>
        /// DebugContext の登録を解除する。DebugContext.OnDestroy() から呼ばれる。
        /// </summary>
        public static void Unregister(DebugContext context)
        {
            if (_context == context)
            {
                _context = null;
                Debug.Log("[DebugServiceLocator] DebugContext unregistered.");
            }
        }

        /// <summary>
        /// BC_Click への参照を取得する。
        /// </summary>
        public static BC_Click BCClick => _context != null ? _context.BCClick : null;
    }
}
