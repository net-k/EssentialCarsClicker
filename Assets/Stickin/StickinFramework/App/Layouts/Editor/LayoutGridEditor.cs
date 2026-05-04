using UnityEditor;
using UnityEngine;

namespace stickin
{
    [CustomEditor(typeof(LayoutGrid))]
    public class LayoutGridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            DrawDefaultInspector();

            var view = target as LayoutGrid;

            if (EditorGUI.EndChangeCheck())
                view.Refresh();
        }
    }
}