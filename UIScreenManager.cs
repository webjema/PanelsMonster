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


using com.webjema.Functional;
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