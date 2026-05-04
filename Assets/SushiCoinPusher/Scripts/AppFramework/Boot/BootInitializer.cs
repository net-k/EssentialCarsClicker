using UnityEngine;

/// <summary>
/// アプリケーション全体の初期化を管理するクラス
/// </summary>
public static class BootInitializer
{
    // RuntimeInitializeLoadType.BeforeSceneLoad を指定することで、
    // 最初のシーンが読み込まれる（Awakeが呼ばれる）よりも前に実行されます。
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeApp()
    {
        Debug.Log("<color=cyan>[AppInitializer] アプリの初期化を開始します...</color>");

        // I2 Localization の言語情報をデバッグ出力
        Debug.Log($"[I2] Unity SystemLanguage: {Application.systemLanguage}");
        Debug.Log($"[I2] Device Language: {I2.Loc.LocalizationManager.GetCurrentDeviceLanguage()}");
        Debug.Log($"[I2] Current Language: {I2.Loc.LocalizationManager.CurrentLanguage}");
        string savedLang = I2.Loc.PersistentStorage.GetSetting_String("I2 Language", "(未保存)");
        Debug.Log($"[I2] Saved Language: {savedLang}");

        // --- SRDebugger の初期化 ---
#if USE_SRDEBUGGER
        InitSRDebugger();
#endif

        // その他、DIコンテナのセットアップやログの設定などもここで行えます
    }

#if USE_SRDEBUGGER
    private static void InitSRDebugger()
    {
        Debug.Log("[AppInitializer] SRDebugger を初期化中...");
        
        // SRDebuggerの設定で "Enable Internal Service Loading" をOFFにしている場合に必要
        SRDebug.Init();

        // 起動直後にオプション画面を出すなどのカスタマイズも可能
        // SRDebug.Instance.ShowDebugPanel();
    }
#endif
}