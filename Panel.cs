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
        public bool enableBackground = true;
        public bool closeOnBackgroundClick = true;

        public PanelsHolder panelsHolder;
        public List<PanelProperty> panelProperties;

        public Dictionary<PanelActionName, Action> panelActions;

        public virtual void InitPanel()
        {
            if (this.onInit != null)
                this.onInit.Invoke(this);
        } // InitPanel

        public virtual void OnStart()
        {
            if (this.disableOnStart)
            {
                this.gameObject.SetActive(false);
            }
        }

        public virtual void OnClick(string action)
        {
            if (this.panelActions == null || this.panelActions.Count == 0)
            {
#if PANELS_DEBUG_ON
                Debug.LogWarning("[Panel][OnClick] 'panelActions' is not inited!");
#endif
                return;
            }
            action = action.Trim().ToLowerInvariant();
            PanelActionName actionName = panelActions.Keys.FirstOrDefault((a) => a.ToString() == action);
            if (actionName == PanelActionName.none)
            {
#if PANELS_DEBUG_ON
                Debug.LogWarning(string.Format("[Panel][OnClick] Action '{0}' is not found in 'panelActions'", action));
#endif
                return;
            }
            this.panelActions[actionName].Invoke();
        } // OnClick

        public void Show()
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[Panel][Show] '{0}'", this.gameObject.name));
#endif
            PanelsManager.Instance.Show(this);
        }

        public void Close()
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[Panel][Close] '{0}'", this.gameObject.name));
#endif
            PanelsManager.Instance.Close(this);
        }

        public Panel SetProperty(PanelPropertyName propertyName, System.Object data, PanelPropertyType propertyType = PanelPropertyType.none)
        {
            //Debug.Log(string.Format("[Panel][SetProperty] propertyName = '{0}' | this.panelProperties[0].propertyName = '{1}'", propertyName, this.panelProperties[0].propertyName));
            PanelProperty pp = this.panelProperties.FirstOrDefault(p => p.propertyName == propertyName);
            if (pp == null)
            {
                pp = this.AddProperty(propertyName, propertyType, data);
            }
            pp.ApplyData(data, propertyType);
            return this;
        } // SetProperty

        public Panel SetNullProperty(PanelPropertyName propertyName)
        {
            PanelProperty pp = this.panelProperties.FirstOrDefault(p => p.propertyName == propertyName);
            if (pp == null)
            {
                pp = this.AddProperty(propertyName, PanelPropertyType.none, null);
            }
            pp.ApplyData(null, PanelPropertyType.none);
            return this;
        } // SetNullProperty

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

        public virtual Panel Reset(bool resetProperties = false)
        {
            if (resetProperties)
            {
                this.panelProperties = new List<PanelProperty>();
            }
            this.panelActions = new Dictionary<PanelActionName, Action>();
            return this;
        }



        private PanelProperty AddProperty(PanelPropertyName propertyName, PanelPropertyType propertyType, System.Object data)
        {
            PanelProperty property = new PanelProperty()
            {
                propertyName = propertyName,
                propertyType = propertyType,
                propertyData = data
            };
            this.panelProperties.Add(property);
            return property;
        }

    } // Panel
}