using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace com.webjema.PanelsMonster
{

    public enum PanelPropertyType
    {
        none,
        text,
        action,
        button
    }

    [System.Serializable]
    public class PanelProperty
    {
        public PanelPropertyName propertyName;
        public PanelPropertyType propertyType;
        public GameObject propertyObject;

        [HideInInspector]
        public System.Object propertyData;

        public void ApplyData(System.Object data)
        {
            this.propertyData = data;
            if (this.propertyType == PanelPropertyType.text)
            {
                this.SetText((string)data);
            } else if (this.propertyType == PanelPropertyType.button)
            {

            } else
            {

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
        } // SetText

    } // PanelProperty
}