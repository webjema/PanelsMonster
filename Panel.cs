using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace com.webjema.PanelsMonster
{
    public class Panel : MonoBehaviour
    {
        public UnityEvent<Panel> onInit;
        public bool disableOnStart = true;

        public PanelsHolder panelsHolder;
        public List<PanelProperty> panelProperties;

        public Dictionary<PanelActionName, Action> panelActions;

        public virtual void InitPanel()
        {
            this.onInit.Invoke(this);
        } // InitPanel

        public void OnStart()
        {
            if (this.disableOnStart)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void OnClick(string action)
        {

        }

        public void Show()
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[Panel][Show] '{0}'", this.gameObject.name));
#endif
            PanelsManager.Instance.Show(this);
        }

        public Panel SetProperty(PanelPropertyName propertyName, System.Object data, PanelPropertyType propertyType = PanelPropertyType.none)
        {
            PanelProperty pp = this.panelProperties.FirstOrDefault(p => p.propertyName == propertyName);
            if (pp == null)
            {
                this.AddProperty(propertyName, propertyType, data);
            } else
            {
                pp.ApplyData(data);
            }
            return this;
        } // SetProperty

        public Panel SetAction(PanelActionName actionName, Action act)
        {
            if (this.panelActions.ContainsKey(actionName))
            {
#if PANELS_DEBUG_ON
                Debug.LogWarning(string.Format("[Panel][SetAction] Panel '{0}' already has action '{1}'. Forgot to reset?", this.gameObject.name, actionName));
#endif
                this.panelActions[actionName] = act;
                return this;
            }
            this.panelActions.Add(actionName, act);
            return this;
        } // SetAction

        public Panel Reset()
        {
            this.panelProperties = new List<PanelProperty>();
            this.panelActions = new Dictionary<PanelActionName, Action>();
            return this;
        }



        private void AddProperty(PanelPropertyName propertyName, PanelPropertyType propertyType, System.Object data)
        {
            this.panelProperties.Add(
                new PanelProperty() {
                    propertyName = propertyName,
                    propertyType = propertyType,
                    propertyData = data
                }
            );
        }

    } // Panel
}