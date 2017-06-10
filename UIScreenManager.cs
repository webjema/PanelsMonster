using com.webjema.Functional;
using com.webjema.PanelsMonster;
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
            ScreensManager.Instance.ScreenAwake(this, this.DefaultArgs());
        }

        public virtual void Back()
        {
            ScreensManager.Instance.PopScreen();
        }

        public virtual Option<IScreenArguments> DefaultArgs()
        {
            return Option<IScreenArguments>.None;
        }

    } // UIScreenManager
}