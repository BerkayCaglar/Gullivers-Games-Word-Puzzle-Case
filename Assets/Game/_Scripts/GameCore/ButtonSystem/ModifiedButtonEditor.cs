#if UNITY_EDITOR
using UnityEditor;

namespace GameCore.ButtonSystem
{
    [CustomEditor(typeof(ModifiedButton))]
    public class ModifiedButtonEditor : UnityEditor.UI.ButtonEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space(20);
            base.OnInspectorGUI();
        }
    }
}

#endif
