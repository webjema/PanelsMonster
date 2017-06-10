// Copyright © 2017 Nick Kavunenko. All rights reserved.
// Contact me: nick@kavunenko.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using TMPro;
using com.webjema.PanelsMonster.Elements;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ButtonWithText), true)]
    [CanEditMultipleObjects]
    public class ButtonWithTextEditor : ButtonEditor
    {
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