using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.webjema.PanelsMonster
{
    public class UIScreenManager : MonoBehaviour
    {
        public Camera uiCamera;
        public CanvasGroup rootCanvasGroup;

        public float fadeInTime = -1;
        public float fadeOutTime = -1;

        public ScreensName backgroundScreen = ScreensName.None;

        private Hashtable _screenArguments;

        void Awake()
        {
            if ((this.fadeInTime > 0 || this.fadeOutTime > 0) && this.rootCanvasGroup == null)
            {
                this.rootCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
                if (this.rootCanvasGroup == null)
                {
                    this.rootCanvasGroup = this.gameObject.AddComponent<CanvasGroup>();
                }
            }
            if (this.uiCamera == null)
            {
                Canvas c = this.gameObject.GetComponent<Canvas>();
                if (c != null && c.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    this.uiCamera = c.worldCamera;
                }
            }
            if (this.uiCamera != null && this.backgroundScreen != ScreensName.None)
            {
                this.uiCamera.clearFlags = CameraClearFlags.Nothing;
            }
            _screenArguments = ScreensManager.Instance.ScreenAwake(this, this.DefaultArgs());
        }

        public virtual Hashtable DefaultArgs()
        {
            return new Hashtable();
        }

        public string GetStringArg(string arg, string def = "")
        {
            object v = this.Args[arg];
            return v == null ? def : v.ToString();
        }

        public int GetIntArg(string arg)
        {
            return this.GetIntArgD(arg, 0);
        }

        public int GetIntArgD(string arg, int def)
        {
            object v = this.Args[arg];
            int result;
            if (v != null && int.TryParse(v.ToString(), out result))
                return result;

            return v == null ? def : (int)v;
        }

        public bool GetBoolArg(string arg)
        {
            object v = this.Args[arg];
            return v == null ? false : (bool)v;
        }

        public Hashtable Args
        {
            get
            {
                return _screenArguments;
            }
        }

        public void ResetArgs(Hashtable args)
        {
            _screenArguments = args;
        }


    } // UIScreenManager
}