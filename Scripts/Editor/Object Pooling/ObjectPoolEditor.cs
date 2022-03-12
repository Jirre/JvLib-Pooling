using JvLib.Pooling.Objects;
using UnityEditor;
using UnityEngine;

namespace JvLib.Editor.Pooling.Objects
{
    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : UnityEditor.Editor
    {
        private ObjectPool _typedTarget;

        private void OnEnable()
        {
            _typedTarget = target as ObjectPool;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = false;
            EditorGUILayout.TextField(new GUIContent("ID"),
                _typedTarget.Id);
            EditorGUILayout.Space();
            EditorGUILayout.IntField(new GUIContent("Active"),
                _typedTarget.ActiveCount);
            EditorGUILayout.IntField(new GUIContent("Passive"),
                _typedTarget.PassiveCount);
            EditorGUILayout.Separator();
            EditorGUILayout.IntField(new GUIContent("Total"),
                _typedTarget.ActiveCount + _typedTarget.PassiveCount);

            GUI.enabled = true;
        }
    }
}
