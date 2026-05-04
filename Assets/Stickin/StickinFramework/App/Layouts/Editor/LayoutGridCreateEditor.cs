using UnityEditor;
using UnityEngine;

namespace stickin
{
    [CustomEditor(typeof(LayoutGridCreate))]
    public class LayoutGridCreateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var view = target as LayoutGridCreate;

            // DrawDefaultInspector();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_prefabs"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_generationType"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minPosition"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxPosition"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_countRandom"), true);
            
            
            if (view.GenerationType == LayoutGenerationType.Line)
            {
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_minDistanceBetween"), true);
            }
            
            // DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();

            
            // serializedObject.FindProperty("m_Content");

            if (GUILayout.Button("Clear"))
            {
                view.Clear();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Generate"))
            {
                view.Clear();
                
                var count = Random.Range(view.CountRandom.x, view.CountRandom.y + 1);
                var prefabs = view.Prefabs;

                for (var i = 0; i < count; i++)
                    PrefabUtility.InstantiatePrefab(prefabs.GetRandom(), view.transform);

                view.GeneratePositions();

                EditorUtility.SetDirty(target);
            }
        }
    }
}