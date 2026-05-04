using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SceneBootstrapper
{
    static SceneBootstrapper()
    {
        // 再生ボタンを押したとき、常に Scene 0 (InitScene) をロードするように設定
        // 引数に false を渡すとこの機能をオフにできます
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
        
        Debug.Log($"<color=cyan>Bootstrapper:</color> 開始シーンを {EditorBuildSettings.scenes[0].path} に固定しました。");
    }
}
