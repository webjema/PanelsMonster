using System.Collections;
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

        public void Init()
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[PanelsHolder][Init] '{0}'", this.gameObject.name));
#endif
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
                this.background.SetActive(false);
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
            if (this.background == null)
            {
                return;
            }
            this.MoveBackgroundToPanel(panel);
            this.background.SetActive(true);
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
            GameObject[] panels = Resources.LoadAll(this.panelsInResourcesFolder) as GameObject[];
            if (panels != null && panels.Length > 0)
            {
                this.panelsPrefabs.AddRange(panels.ToList());
            }
        } // ScanResources


        private void MoveBackgroundToPanel(Panel panel)
        {
            this.background.transform.SetSiblingIndex(panel.transform.GetSiblingIndex() - 1);
        }

    } // PanelsHolder
}