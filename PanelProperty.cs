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

using UnityEngine;
using TMPro;
using com.webjema.PanelsMonster.Elements;

namespace com.webjema.PanelsMonster
{

    public enum PanelPropertyType
    {
        none,
        text,
        button
    }

    [System.Serializable]
    public class PanelProperty
    {
        public PanelPropertyName propertyName;
        public GameObject propertyObject;

        [HideInInspector]
        public PanelPropertyType propertyType;
        [HideInInspector]
        public System.Object propertyData;

        public void ApplyData(System.Object data, PanelPropertyType propertyType)
        {
            Debug.Log(string.Format("[PanelProperty][ApplyData] Property Name '{0}', Property type is '{1}'", this.propertyName, propertyType));
            this.propertyData = data;
            if (propertyType == PanelPropertyType.none)
            {
                this.DisableElement();
            } else if (propertyType == PanelPropertyType.text)
            {
                this.SetText((string)data);
            } else if (propertyType == PanelPropertyType.button)
            {
                this.SetButton((string)data);
            } else
            {
#if PANELS_DEBUG_ON
                Debug.LogWarning(string.Format("[PanelProperty][ApplyData] Warning! Property '{0}' is not allowed yet", propertyType));
#endif
            }
        } // ApplyData

        private void SetText(string data)
        {
            if (this.propertyObject == null)
            {
#if PANELS_DEBUG_ON
                Debug.LogError(string.Format("[PanelProperty][SetText] Error! PropertyObject is not linked for '{0}' '{1}'", this.propertyName, this.propertyType));
#endif
                return;
            }
            TextMeshProUGUI textComponent = this.propertyObject.GetComponent<TextMeshProUGUI>();
            if (textComponent == null)
            {
#if PANELS_DEBUG_ON
                Debug.LogError(string.Format("[PanelProperty][SetText] Error! PropertyObject '{0}' does not have TextMeshProUGUI component.", this.propertyObject.name));
#endif
                return;
            }
            textComponent.text = data;
            this.propertyObject.SetActive(true);
        } // SetText

        private void SetButton(string data)
        {
            if (this.propertyObject == null)
            {
#if PANELS_DEBUG_ON
                Debug.LogError(string.Format("[PanelProperty][SetButton] Error! PropertyObject is not linked for '{0}' '{1}'", this.propertyName, this.propertyType));
#endif
                return;
            }
            ButtonWithText buttonComponent = this.propertyObject.GetComponent<ButtonWithText>();
            if (buttonComponent == null)
            {
#if PANELS_DEBUG_ON
                Debug.LogError(string.Format("[PanelProperty][SetButton] Error! PropertyObject '{0}' does not have 'ButtonWithText' component.", this.propertyObject.name));
#endif
                return;
            }
            buttonComponent.text.text = data;
            this.propertyObject.SetActive(true);
        } // SetButton




        private void DisableElement()
        {
            if (this.propertyObject == null)
            {
#if PANELS_DEBUG_ON
                Debug.LogError(string.Format("[PanelProperty][DisableElement] Error! PropertyObject is not linked for '{0}' '{1}'", this.propertyName, this.propertyType));
#endif
                return;
            }
            this.propertyObject.SetActive(false);
        }

    } // PanelProperty
}