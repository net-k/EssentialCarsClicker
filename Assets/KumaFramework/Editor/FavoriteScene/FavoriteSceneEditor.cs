using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FavoriteSceneEditor : EditorWindow
{
    private FavoriteScenes favoriteScenes;
    private Vector2 scrollPosition;

    private const string FavoriteScenesAssetName = "FavoriteScenes.asset";

    [MenuItem("Window/Favorite Scene Selector")]
    public static void ShowWindow()
    {
        GetWindow<FavoriteSceneEditor>("Favorite Scenes");
    }

    private void OnEnable()
    {
        LoadFavoriteScenesAsset();
    }

    private void OnGUI()
    {
        if (favoriteScenes == null)
        {
            EditorGUILayout.HelpBox("FavoriteScenes asset not found. Please create one via 'Assets > Create > ScriptableObjects > FavoriteScenes'.", MessageType.Warning);
            if (GUILayout.Button("Create Asset"))
            {
                CreateFavoriteScenesAsset();
            }
            return;
        }

        EditorGUILayout.LabelField("Favorite Scenes", EditorStyles.boldLabel);

        // ドラッグ＆ドロップエリア
        var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag & Drop Scene(s) Here");
        HandleDragAndDrop(dropArea);

        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (favoriteScenes.scenes.Count == 0)
        {
            EditorGUILayout.LabelField("No favorite scenes yet.");
        }
        else
        {
            for (int i = 0; i < favoriteScenes.scenes.Count; i++)
            {
                var scene = favoriteScenes.scenes[i];
                EditorGUILayout.BeginHorizontal();

                if (scene == null)
                {
                    EditorGUILayout.LabelField("Missing Scene");
                }
                else
                {
                    EditorGUILayout.ObjectField(scene, typeof(SceneAsset), false);
                }

                if (GUILayout.Button("Open", GUILayout.Width(60)))
                {
                    if (scene != null)
                    {
                        string scenePath = AssetDatabase.GetAssetPath(scene);
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scenePath);
                        }
                    }
                }

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    favoriteScenes.scenes.RemoveAt(i);
                    EditorUtility.SetDirty(favoriteScenes);
                    AssetDatabase.SaveAssets();
                    Repaint();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Current Scene"))
        {
            AddCurrentScene();
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        var currentEvent = Event.current;
        if (!dropArea.Contains(currentEvent.mousePosition))
        {
            return;
        }

        if (currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (currentEvent.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                bool changed = false;
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is SceneAsset sceneAsset)
                    {
                        if (!favoriteScenes.scenes.Contains(sceneAsset))
                        {
                            favoriteScenes.scenes.Add(sceneAsset);
                            changed = true;
                        }
                    }
                }
                if (changed)
                {
                    EditorUtility.SetDirty(favoriteScenes);
                    AssetDatabase.SaveAssets();
                }
            }
            currentEvent.Use();
        }
    }

    private void AddCurrentScene()
    {
        var currentScene = EditorSceneManager.GetActiveScene();
        if (!string.IsNullOrEmpty(currentScene.path))
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentScene.path);
            if (sceneAsset != null && !favoriteScenes.scenes.Contains(sceneAsset))
            {
                favoriteScenes.scenes.Add(sceneAsset);
                EditorUtility.SetDirty(favoriteScenes);
                AssetDatabase.SaveAssets();
            }
        }
        else
        {
            Debug.LogWarning("Current scene is not saved. Please save the scene first.");
        }
    }

    private void LoadFavoriteScenesAsset()
    {
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(FavoriteScenes)}");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            favoriteScenes = AssetDatabase.LoadAssetAtPath<FavoriteScenes>(path);
        }
        else
        {
            favoriteScenes = null;
        }
    }

    private void CreateFavoriteScenesAsset()
    {
        var asset = CreateInstance<FavoriteScenes>();
        string path = "Assets"; // 保存先のフォルダ
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, FavoriteScenesAssetName));

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        // 作成後に再読み込み
        LoadFavoriteScenesAsset();
    }
}
