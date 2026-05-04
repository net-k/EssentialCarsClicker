using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[InitializeOnLoad]
public static class SerializeFieldChecker
{
    private const int IconSize = 16;

    private const string MenuPath = "GameObject/コンポーネントアイコン表示切替";

    private const string ScriptIconName = "cs Script Icon";

    private const string WarningIconName = "console.warnicon";

    private const string PropertyNameOfFieldId = "m_FileID";

    private static readonly Color colorWhenDisabled = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private static Texture? scriptIcon;

    private static Texture? warningIcon;

    private static bool enabled = true;

    private static bool isEnabled = true;
    
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        UpdateEnabled();

        /*
         * ビルトインアイコンの呼び出し方は以下を参考にした
         * https://qiita.com/Rijicho_nl/items/88e71b5c5930fc7a2af1
         * https://unitylist.com/p/5c3/Unity-editor-icons
         */
#pragma warning disable UNT0023 // Coalescing assignment on Unity objects
        scriptIcon ??= EditorGUIUtility.IconContent(ScriptIconName).image;
        warningIcon ??= EditorGUIUtility.IconContent(WarningIconName).image;
#pragma warning restore UNT0023 // Coalescing assignment on Unity objects
    }

    [MenuItem(MenuPath, false, 20)]
    private static void ToggleEnabled()
    {
        enabled = !enabled;
        UpdateEnabled();
    }

    private static void UpdateEnabled()
    {
        EditorApplication.hierarchyWindowItemOnGUI -= DisplayIcons;
        if (enabled)
            EditorApplication.hierarchyWindowItemOnGUI += DisplayIcons;
    }

    private static void DisplayIcons(int instanceID, Rect selectionRect)
    {
        if (!isEnabled) return; // 追加

        // instanceIDをオブジェクト参照に変換
        if (!(EditorUtility.InstanceIDToObject(instanceID) is GameObject gameObject)) return;

        var pos = selectionRect;
        pos.x = pos.xMax - IconSize;
        pos.width = IconSize;
        pos.height = IconSize;

        // オブジェクトが所持しているコンポーネント一覧を取得
        var components
            = gameObject
                .GetComponents<Component>()
                .Where(x => !(x is Transform || x is ParticleSystemRenderer))
                .Reverse()
                .ToList();

        // Missingなコンポーネントが存在する場合はWarningアイコン表示
        var existsMissing = components.RemoveAll(x => x == null) > 0;
        if (existsMissing)
        {
//UnityEngine.Debug.LogError(gameObject.name + "のコンポーネントにMissingのものが存在します。");
            DrawIcon(ref pos, warningIcon!);
        }

        var existsScriptIcon = false;
        foreach (var component in components)
        {
            // SerializeFieldsにMissingなものが存在する場合はWarningアイコン表示
            var existsMissingField = ExistsMissingField(component);
            if (existsMissingField)
            {
                DrawIcon(ref pos, warningIcon!);
            }
            else
            {
                continue;
            }
#if false
            Texture image = AssetPreview.GetMiniThumbnail(component);
            if (image == null) continue;

            // Scriptのアイコンは1つのみ表示
            if (image == scriptIcon)
            {
                if (existsScriptIcon) continue;
                existsScriptIcon = true;
            }

            // アイコン描画
            DrawIcon(ref pos, image, component.IsEnabled() ? Color.white : colorWhenDisabled);
#endif
        }
    }

    /// <summary>
    /// コンポーネントの設定値にMissingなものが存在するかどうかを確認する
    /// </summary>
    /// <param name="component">確認対象のコンポーネント</param>
    /// <returns>MissingなSerializedFieldが存在するかどうか</returns>
    /// <remarks>
    /// 以下の条件を満たす場合はMissingと見なす。Unityのバージョンが変わると変更になる可能性有。
    /// <list type="bullet">
    /// <item><description><see cref="SerializedProperty.propertyType"/>が<see cref="SerializedPropertyType.ObjectReference"/></description></item>
    /// <item><description><see cref="SerializedProperty.objectReferenceInstanceIDValue"/>がnull</description></item>
    /// <item><description>fileIDが0ではない</description></item>
    /// </list>
    /// </remarks>
    private static bool ExistsMissingField(Component component)
    {
        if( component == null ) return false;
        if (!FilterIgnore(component)) return false;

        bool hasMissingField = false;
        var serializedObject = new SerializedObject(component);
        var serializedProperty = serializedObject.GetIterator();

        // Move to the first serialized property.
        if (serializedProperty.NextVisible(true))
        {
            do
            {
                // Check if the serialized property is a reference type and is null.
                if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference && serializedProperty.objectReferenceValue == null)
                {
                    if (serializedProperty.displayName != "Script") // Ignore the "Script" property.
                    {
                        // Debug.LogError($"{component.name} in {component.GetType().Name} is missing a reference for field {serializedProperty.displayName}.");
                        hasMissingField = true;
                    }
                }
                // Additional checks for other types (e.g., value types) can be added here as needed.
            }
            while (serializedProperty.NextVisible(false)); // Move to the next serialized property.
        }

        return hasMissingField;
    }

    private static bool FilterIgnore(Component component)
    {
        // Check if the component is a Button and its Material is set to None (null)
        if (component is UnityEngine.UI.Button button )
        {
            var buttonImage = button.GetComponent<UnityEngine.UI.Image>();
            if (!buttonImage)
            {
                return false;
            }

            if (button.GetComponent<UnityEngine.UI.Image>().material == null)
            {
                return false;
            }

            return false;
        }

        // Check if the component is an Image and its Material or SourceImage (sprite) is set to None (null)
        if (component is UnityEngine.UI.Image image && (image.GetComponent<Material>() == null || image.sprite == null))
        {
            return false;
        }

        // Check if the component is a Text and its Material is set to None (null)
        if (component is UnityEngine.UI.Text text && text.GetComponent<Material>() == null)
        {
            return false;
        }

        // Check if the component is a Camera and its TargetTexture is set to None (null)
        if (component is UnityEngine.Camera camera && camera.targetTexture == null)
        {
            return false;
        }
        
        // ScrollRect の Scrollbar が null の場合は無視
        if (component is UnityEngine.UI.ScrollRect scrollRect && scrollRect.verticalScrollbar == null)
        {
            return false;
        }
        
        // Light の Flare が null の場合も無視
        if (component is Light light && light.flare == null)
        {
            return false;
        }
        
        // Event System の First Selected が null の場合も無視
        if (component is UnityEngine.EventSystems.EventSystem eventSystem && eventSystem.firstSelectedGameObject == null)
        {
            return false;
        }

        if (component is Canvas)
        {
            Canvas canvas = component as Canvas;
            if (( canvas.renderMode == RenderMode.ScreenSpaceCamera 
                 || canvas.renderMode == RenderMode.WorldSpace) 
                 && canvas.worldCamera == null)
            {
            //    Debug.LogError("Canvas in " + component.gameObject.name + " is missing a reference for field Camera.");
            }
            else
            {
                return false;
            }
        }
        
        // SingletonMonoBehaviour<T> 継承クラスの場合は無視
        var componentType = component.GetType();
        while (componentType != null && componentType != typeof(object))
        {
            if (componentType.IsGenericType && componentType.GetGenericTypeDefinition() == typeof(SingletonMonoBehaviour<>))
            {
                return false; // Ignore SingletonMonoBehaviour<T> instances
            }
            componentType = componentType.BaseType; // 親クラスに更新
        }
        return true;
    }

    private static void DrawIcon(ref Rect pos, Texture image, Color? color = null)
    {
        Color? defaultColor = null;
        if (color.HasValue)
        {
            defaultColor = GUI.color;
            GUI.color = color.Value;
        }

        GUI.DrawTexture(pos, image, ScaleMode.ScaleToFit);
        pos.x -= pos.width;

        if (defaultColor.HasValue)
            GUI.color = defaultColor.Value;
    }

    /// <summary>
    /// コンポーネントが有効かどうかを確認する拡張メソッド
    /// </summary>
    /// <param name="this">拡張対象</param>
    /// <returns>コンポーネントが有効となっているかどうか</returns>
    private static bool IsEnabled(this Component @this)
    {
        var property = @this.GetType().GetProperty( "enabled", typeof(bool));
        return (bool)(property?.GetValue(@this, null) ?? true);
    }
    
}