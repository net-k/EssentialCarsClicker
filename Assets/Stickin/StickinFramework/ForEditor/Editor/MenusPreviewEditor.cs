using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace stickin.menus
{
    [CustomEditor(typeof(MenusPreview))]
    public class MenusPreviewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var gg = target as MenusPreview;

            if (GUILayout.Button("Spawn"))
            {
                InstantiateMenus(gg);
                EditorUtility.SetDirty(gg);
            }

            if (GUILayout.Button("Clear"))
            {
                Clear(gg);
                EditorUtility.SetDirty(gg);
            }
            //
            // if (GUILayout.Button("Generate variants"))
            // {
            //     GenerateVariants(gg.MenusData);
            // }
        }

        private void InstantiateMenus(MenusPreview gg)
        {
            gg.BgPrefab.SetActive(false);
            
            Clear(gg);

            foreach (var menu in gg.MenusData.MenusPrefabs)
            {
                var bg = Instantiate(gg.BgPrefab, gg.transform);
                bg.SetActive(true);

                PrefabUtility.InstantiatePrefab(menu, bg.transform);
                // Instantiate(menu, bg.transform);
                var name = menu.name;
                name = name.Replace("Menu", "");
                bg.name = name;

                foreach (var linkMenu in menu.LinkMenusPrefabs)
                    PrefabUtility.InstantiatePrefab(linkMenu, bg.transform);
            }
        }

        private void Clear(MenusPreview gg)
        {
            var destroyList = new List<Transform>();
            foreach (Transform child in gg.transform)
            {
                if (child != gg.BgPrefab.transform)
                    destroyList.Add(child);
            }
            
            foreach (var child in destroyList)
                DestroyImmediate(child.gameObject);
        }
        
        private void GenerateVariants(MenusData ggMenusData)
        {
            // Object source = Resources.Load(prefabPath);
            // GameObject objSource = (GameObject)PrefabUtility.InstantiatePrefab(source);
            // GameObject obj = PrefabUtility.SaveAsPrefabAsset(objSource, variantAssetPath);
        }
    }
}