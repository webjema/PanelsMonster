﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.webjema.Functional;
using com.webjema.Infrastructure;
using System.Linq;

namespace com.webjema.PanelsMonster
{
    public class PanelsManager : SingletonAsComponent<PanelsManager>
    {
        public static PanelsManager Instance
        {
            get { return ((PanelsManager)_Instance); }
            set { _Instance = value; }
        }

        public List<PanelsHolder> panelsHolders; // link it in editor if only one scene is used
        public Panel defaultPanel;

        private List<Panel> _panelsInScene;

        public Panel GetPanel(PanelName name)
        {
            return this.GetPanel(name.ToString());
        }

        public Panel GetPanel(string name)
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[PanelsManager][GetPanel] by name '{0}'", name));
#endif
            if (!this.IsManagerInited())
            {
#if PANELS_DEBUG_ON
                Debug.LogError("[PanelsManager][GetPanel] Error! Manager is not inited!");
#endif
                return this.DefaultPanel(name);
            }
            Panel panel = this.FindPanelByName(name);
            if (!this._panelsInScene.Contains(panel))
            {
                this._panelsInScene.Add(panel);
            }
            return panel;
        } // GetPanel

        public void Show(Panel panel)
        {
            panel.gameObject.SetActive(true);
            if (panel.panelsHolder != null)
            {
                panel.panelsHolder.OnPanelShow(panel);
            } else
            {
#if PANELS_DEBUG_ON
                Debug.LogWarning(string.Format("[PanelsManager][GetPanel] No holder for panel '{0}' is found", name));
#endif
            }
        }

        public void ResetManager()
        {
            this.panelsHolders = null;
            this.Init();
        }

        /*
        public Option<T> GetPopup<T>(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = typeof(T).ToString();
            }
            if (!this._activePopupsList.ContainsKey(name))
            {
                // create Popup
                this.AddPopup(name);
            }
            if (!this._activePopupsList.ContainsKey(name))
            {
                Logger.LogError("Error! Popup is not found by name [" + name + "]");
                return Option.None<T>();
            }
            T pb;
            try
            {
                pb = (T)Convert.ChangeType(this._activePopupsList[name], typeof(T));
            }
            catch (InvalidCastException)
            {
                Logger.LogError("Error! Cannot convert popup [" + name + "] to " + typeof(T).ToString());
                return Option.None<T>();
            }
            return Option.Return<T>(pb);
        } // GetPopup<T>
        */

        private void InitPanelsHolders()
        {
            if (this.panelsHolders == null || this.panelsHolders.Count == 0)
            {
#if PANELS_DEBUG_ON
                Debug.LogError("[PanelsManager] [InitPanelsHolders] Error! No PanelsHolders in current scene.");
#endif
                return;
            }
            this.panelsHolders.ForEach(ph => ph.Init());
        }

        private Panel DefaultPanel(string name)
        {
            if (this.defaultPanel != null)
            {
                return this.defaultPanel;
            }
            return this.EmptyPanel(name);
        }

        private Panel EmptyPanel(string name)
        {
#if PANELS_DEBUG_ON
            Debug.LogWarning(string.Format("[PanelsManager][EmptyPanel] Creating empty panel '{0}'", name));
#endif
            Panel ep = new GameObject().AddComponent<Panel>();
            ep.transform.SetParent(this.transform, false);
            ep.gameObject.name = name;
            return ep;
        }

        private Panel FindPanelByName(string name)
        {
            // try already inited panels first
            Panel p = this._panelsInScene.FirstOrDefault(e => e.gameObject.name == name);
            if (p != null)
            {
                return p;
            }

            // try panels holders
            foreach(PanelsHolder ph in  this.panelsHolders)
            {
                p = ph.FindPanelByName(name);
                if (p != null)
                {
                    return p;
                }
            }
            // nothing found - use default
            return this.DefaultPanel(name);
        } // FindPanelByName

        private bool IsManagerInited()
        {
            if (this._panelsInScene != null &&
                this.panelsHolders != null &&
                this.panelsHolders.Count > 0)
            {
                return true;
            }
            return false;
        }

        // autoinit on scene loaded event
        private void Init()
        {
#if PANELS_DEBUG_ON
            Debug.Log("[PanelsManager][Init]");
#endif
            if (this.IsManagerInited())
            {
#if PANELS_DEBUG_ON
                Debug.LogWarning("[PanelsManager] [Init] WARNING! Manager is already inited!");
#endif
                return;
            }
            if (this.panelsHolders == null || this.panelsHolders.Count == 0)
            {
                this.panelsHolders = (GameObject.FindObjectsOfType(typeof(PanelsHolder)) as PanelsHolder[]).ToList();
            }
            this.InitPanelsHolders();

            this._panelsInScene = new List<Panel>();
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("Scene Loaded - '{0}' (mode '{1}')", scene.name, mode));
#endif
            this.ResetManager();
        }

    } // PanelsManager
}