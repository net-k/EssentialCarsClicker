using UnityEditor;
using UnityEngine;

namespace stickin
{
    [CustomEditor(typeof(LayoutRotate))]
    public class LayoutRotateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var view = target as LayoutRotate;

            if (GUILayout.Button("Reset"))
            {
                view.Reset();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Random rotate"))
            {
                view.RandomRotate();
                EditorUtility.SetDirty(target);
            }
        }
    }
}