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