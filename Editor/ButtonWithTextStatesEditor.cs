using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using com.webjema.PanelsMonster.Elements;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ButtonWithTextStates), true)]
    [CanEditMultipleObjects]
    public class ButtonWithTextStatesEditor : ButtonEditor
    {

        public override void OnInspectorGUI()
        {
            ButtonWithTextStates component = (ButtonWithTextStates)target;
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            component.textNormalState = (TextMeshProUGUI)EditorGUILayout.ObjectField("Button Normal text", component.textNormalState, typeof(TextMeshProUGUI), true);
            component.textHighlightedState = (TextMeshProUGUI)EditorGUILayout.ObjectField("Button Highlighted text", component.textHighlightedState, typeof(TextMeshProUGUI), true);
            component.textPressedState = (TextMeshProUGUI)EditorGUILayout.ObjectField("Button Pressed text", component.textPressedState, typeof(TextMeshProUGUI), true);
            component.textDisabledState = (TextMeshProUGUI)EditorGUILayout.ObjectField("Button Disabled text", component.textDisabledState, typeof(TextMeshProUGUI), true);

            SerializedProperty tps = serializedObject.FindProperty("colorTransitionAdditionalGraphics");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sss = serializedObject.FindProperty("spriteSwapAdditionalGraphics");
            if (sss != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sss, true);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
            }
            component.useAllGraphicsAutomatically = EditorGUILayout.Toggle("Use All Graphics Automatically", component.useAllGraphicsAutomatically);
        } // OnInspectorGUI

    } // ButtonWithTextStatesEditor
} // namespace