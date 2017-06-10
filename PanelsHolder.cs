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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.webjema.PanelsMonster
{

    public class PanelsHolder : MonoBehaviour
    {
        // use this list to link panels in scene (optional)
        public List<Panel> panels;

        // use this list to link panels gameobjects in scene (optional)
        public List<GameObject> panelsGameObjects;

        // use this list to link panels prefabs (optional)
        public List<GameObject> panelsPrefabs;

        // provide resources folder, if panels are in resources (optional)
        public string panelsInResourcesFolder;

        public bool doNotScanPanelsHolderForPanels = false; // set it to true if all panels are hard-linked (via panels, panelsGameObjects, panelsPrefabs or panelsInResourcesFolder)
        public bool disablePanelsOnStart = true;

        public GameObject background;

        private List<Panel> _openPanels;

        public void Init()
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[PanelsHolder][Init] '{0}'", this.gameObject.name));
#endif
            this._openPanels = new List<Panel>();

            if (this.panels == null)
            {
                this.panels = new List<Panel>();
            }
            if (this.panelsGameObjects == null)
            {
                this.panelsGameObjects = new List<GameObject>();
            }
            if (this.panelsPrefabs == null)
            {
                this.panelsPrefabs = new List<GameObject>();
            }
            if (!this.doNotScanPanelsHolderForPanels)
            {
                this.ScanHolder();
            }
            if (!string.IsNullOrEmpty(this.panelsInResourcesFolder))
            {
                this.ScanResources();
            }

            this.panels.ForEach(p => p.OnStart());
            if (this.disablePanelsOnStart)
            {
                if (this.background != null)
                {
                    this.background.SetActive(false);
                }
                this.panelsGameObjects.ForEach(g => g.SetActive(false));
            }
        } // Init

        public Panel FindPanelByName(string name)
        {
            Panel p = this.panels.FirstOrDefault(e => e.gameObject.name == name);
            if (p != null)
            {
                return p;
            }
            GameObject po = this.panelsGameObjects.FirstOrDefault(e => e.name == name);
            if (po != null)
            {
                return this.InitPanelObject(po);
            }
            po = this.panelsPrefabs.FirstOrDefault(e => e.name == name);
            if (po != null)
            {
                return this.CreatePanelObject(po);
            }
            return null;
        } // FindPanelByName

        public void OnPanelShow(Panel panel)
        {
            if (!panel.enableBackground)
            {
                return;
            }
            if (!this._openPanels.Contains(panel))
            {
                this._openPanels.Add(panel);
            }
            if (this.background == null)
            {
                return;
            }
            this.MoveBackgroundToPanel(panel);
            this.background.SetActive(true);
        }

        public void OnPanelClose(Panel panel)
        {
            if (!panel.enableBackground)
            {
                return;
            }
            
            if (this._openPanels.Contains(panel))
            {
                this._openPanels.Remove(panel);
            }
            
            if (this.background == null)
            {
                return;
            }
            Panel anotherOpenPanel = this.GetOpenPanel();
            
            if (anotherOpenPanel != null)
            {
                this.MoveBackgroundToPanel(anotherOpenPanel, toBack: true);
                this.background.SetActive(true);
            } else
            {
                this.background.SetActive(false);
            }
        }

        public void BackgroundClick()
        {
            if (this.background == null)
            {
                return;
            }

            Panel panel = this._openPanels.LastOrDefault();
            if (panel == null)
            {
                return;
            }

            if (!panel.enableBackground || !panel.closeOnBackgroundClick)
            {
                return;
            }

            panel.Close();
        }

        private Panel GetOpenPanel()
        {
#if PANELS_DEBUG_ON
            Debug.Log("[GetOpenPanel] this._openPanels.Count = " + this._openPanels.Count);
#endif
            if (this._openPanels.Count > 0)
            {
                return this._openPanels.Last();
            }
            return null;
        }


        private Panel InitPanelObject(GameObject go)
        {
            Panel p = go.GetComponent<Panel>();
            if (p == null)
            {
                p = go.AddComponent<Panel>();
            }
            p.panelsHolder = this;
            return p;
        } // InitPanelObject

        private Panel CreatePanelObject(GameObject go)
        {
            GameObject panelGo = Instantiate(go);
            panelGo.name = go.name;
            panelGo.transform.SetParent(this.transform, false);
            return this.InitPanelObject(panelGo);
        }

        private void ScanHolder()
        {
            foreach (Transform child in this.transform)
            {
                Panel chPanel = child.GetComponent<Panel>();
                if (chPanel != null)
                {
                    chPanel.panelsHolder = this;
                    this.panels.Add(chPanel);
                }
                else
                {
                    this.panelsGameObjects.Add(child.gameObject);
                }
            }
        } // ScanChilds

        private void ScanResources()
        {
            List<GameObject> panels = Resources.LoadAll(this.panelsInResourcesFolder, typeof(GameObject)).Cast<GameObject>().ToList();
            if (panels != null && panels.Count > 0)
            {
                this.panelsPrefabs.AddRange(panels);
            }
        } // ScanResources


        private void MoveBackgroundToPanel(Panel panel, bool toBack = false)
        {
            int correction = toBack ? 0 : -1;
            this.background.transform.SetSiblingIndex(Mathf.Max(panel.transform.GetSiblingIndex() + correction, 0));
        }

    } // PanelsHolder
}