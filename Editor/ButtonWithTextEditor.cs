using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using com.webjema.PanelsMonster.Elements;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ButtonWithText), true)]
    [CanEditMultipleObjects]
    public class ButtonWithTextEditor : ButtonEditor
    {
        /*
        SerializedProperty m_OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }
        */

        public override void OnInspectorGUI()
        {
            ButtonWithText component = (ButtonWithText)target;
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            component.text = (TextMeshProUGUI)EditorGUILayout.ObjectField("Button text", component.text, typeof(TextMeshProUGUI), true);
            component.textNormalColor = EditorGUILayout.ColorField("Normal text color", component.textNormalColor);
            component.textHighlightedColor = EditorGUILayout.ColorField("Highlighted text color", component.textHighlightedColor);
            component.textPressedColor = EditorGUILayout.ColorField("Pressed text color", component.textPressedColor);
            component.textDisabledColor = EditorGUILayout.ColorField("Disabled text color", component.textDisabledColor);

            SerializedProperty tps = serializedObject.FindProperty("colorTransitionAdditionalGraphics");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            component.useAllGraphicsAutomatically = EditorGUILayout.Toggle("Use All Graphics Automatically", component.useAllGraphicsAutomatically);
        } // OnInspectorGUI

    } // ButtonWithTextEditor
} // namespace