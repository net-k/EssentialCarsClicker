using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BClickerScene の中身を調査・修復するエディタースクリプト
/// </summary>
public class SceneDebugger : EditorWindow
{
    [MenuItem("Tools/BClicker Scene 調査")]
    public static void InspectScene()
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log($"=== シーン調査: {scene.name} ===");
        Debug.Log($"シーンパス: {scene.path}");
        Debug.Log($"IsLoaded: {scene.isLoaded}");

        // 通常の方法でルートオブジェクトを取得
        var roots = scene.GetRootGameObjects();
        Debug.Log($"ルートGameObject数: {roots.Length}");

        // 全オブジェクト（非ルート含む）を取得
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int sceneObjects = 0;
        foreach (var go in allObjects)
        {
            if (go.scene == scene)
            {
                sceneObjects++;
                Debug.Log($"  [scene obj] {go.name} (active:{go.activeSelf}, parent:{(go.transform.parent != null ? go.transform.parent.name : "none")})");
            }
        }
        Debug.Log($"シーン内の全GameObject数: {sceneObjects}");
    }

    [MenuItem("Tools/BClicker Scene 修復（全オブジェクトをルートに接続）")]
    public static void RepairScene()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.name.Contains("BClicker"))
        {
            Debug.LogError("BClickerScene を開いてから実行してください");
            return;
        }

        var roots = scene.GetRootGameObjects();
        Debug.Log($"修復前のルート数: {roots.Length}");

        // 孤立したGameObjectを探してルートに昇格
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int fixed_count = 0;
        foreach (var go in allObjects)
        {
            if (go.scene == scene && go.transform.parent == null && !go.scene.rootCount.Equals(0))
            {
                // ルートに昇格させる（すでにルートかもしれないが念のため）
                SceneManager.MoveGameObjectToScene(go, scene);
                fixed_count++;
            }
        }

        Debug.Log($"処理したオブジェクト数: {fixed_count}");
        Debug.Log($"修復後のルート数: {scene.GetRootGameObjects().Length}");

        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("シーンを保存するには Ctrl+S を押してください");
    }
}
