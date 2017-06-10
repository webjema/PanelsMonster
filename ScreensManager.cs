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
using UnityEngine.SceneManagement;

using System;
using System.Collections.Generic;

using com.webjema.Functional;
using System.Collections;
using DG.Tweening;

namespace com.webjema.PanelsMonster
{
    public class ScreensManager : SingletonAsComponent<ScreensManager>
    {

        public static ScreensManager Instance
        {
            get { return ((ScreensManager)_Instance); }
            set { _Instance = value; }
        }

        public Option<IScreenArguments> CurrentScreenArguments
        {
            get
            {
                if (this._argumentsStack == null || this._argumentsStack.Count == 0)
                    throw new Exception("[ScreensManager][CurrentScreenArguments Get] argumentsStack is not inited yet");
                return this._argumentsStack[this._argumentsStack.Count - 1].Arguments;
            }
            set
            {
                if (this._argumentsStack == null || this._argumentsStack.Count == 0)
                    throw new Exception("[ScreensManager][CurrentScreenArguments Set] argumentsStack is not inited yet");
                this._argumentsStack[this._argumentsStack.Count - 1].Arguments = value;
            }
        }

        private ScreensSettings _screensSettings;
        private ScreensSettings ScreensSettings
        {
            get
            {
                if (this._screensSettings == null)
                {
                    this._screensSettings = Helpers.LoadScriptableObject<ScreensSettings>();
                }
                return this._screensSettings;
            }
        }

        private ScreensName _currentBackgroundScene;
        private UIScreenManager _currentScreen;
        private List<ArgumentsStackItem<IScreenArguments>> _argumentsStack = new List<ArgumentsStackItem<IScreenArguments>>();

        public void PushScreen(ScreensName screenName)
        {
            this.PushScreen(screenName.ToString(), Option<IScreenArguments>.None);
        }

        public void PushScreen(ScreensName screenName, Option<IScreenArguments> args)
        {
            this.PushScreen(screenName.ToString(), args);
        }

        public void PushScreen(string screenName)
        {
            this.PushScreen(screenName, Option<IScreenArguments>.None);
        }

        public void PushScreen(string screenName, Option<IScreenArguments> args)
        {
            this.PushArgumentsInStack(new ArgumentsStackItem<IScreenArguments>(screenName, args));

            this.LoadScreen(screenName);
        }


        public void SetScreen(ScreensName screenName)
        {
            this.SetScreen(screenName.ToString(), Option<IScreenArguments>.None);
        }

        public void SetScreen(ScreensName screenName, Option<IScreenArguments> args)
        {
            this.SetScreen(screenName.ToString(), args);
        }

        public void SetScreen(string screenName)
        {
            this.SetScreen(screenName, Option<IScreenArguments>.None);
        }

        public void SetScreen(string screenName, Option<IScreenArguments> args)
        {
            this._argumentsStack.RemoveAt(this._argumentsStack.Count - 1);
            this.PushArgumentsInStack(new ArgumentsStackItem<IScreenArguments>(screenName, args));

            this.LoadScreen(screenName);
        }


        public void PopScreen(Option<IScreenArguments> args)
        {
            this.PopScreen(args, true);
        }

        public void PopScreen(bool resetArguments = false)
        {
            this.PopScreen(Option<IScreenArguments>.None, resetArguments);
        }

        private void PopScreen(Option<IScreenArguments> newArguments, bool resetArguments)
        {
            if (this._argumentsStack.Count < 2)
            {
#if PANELS_DEBUG_ON
                Debug.LogError("[PopScreen] nothing to pop from screens stack");
#endif                
                return;
            }

            this._argumentsStack.RemoveAt(this._argumentsStack.Count - 1);
            ArgumentsStackItem<IScreenArguments> args = this._argumentsStack[this._argumentsStack.Count - 1];
            this._argumentsStack.RemoveAt(this._argumentsStack.Count - 1);
            if (resetArguments)
            {
                args.Arguments = newArguments;
            }
            this.PushScreen(args.ScreenName, args.Arguments);
        } // PopScreen

        public void ScreenAwake(UIScreenManager screen, Option<IScreenArguments> screenArgs)
        {
            this._currentScreen = screen;
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[ScreenAwake] scene '{0}'", screen.gameObject.scene.name));
#endif
            if (this._argumentsStack.Count == 0)
            {
                this.Init();
            }

            if (screen.backgroundScreen == ScreensName.None && this._currentBackgroundScene != ScreensName.None)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(this._currentBackgroundScene.ToString()));
                this._currentBackgroundScene = ScreensName.None;
            }
            if (screen.backgroundScreen != ScreensName.None)
            {
                if (this._currentBackgroundScene != ScreensName.None && this._currentBackgroundScene != screen.backgroundScreen)
                {
                    // remove old background
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(this._currentBackgroundScene.ToString()));
                    this._currentBackgroundScene = ScreensName.None;
                }
                if (screen.backgroundScreen != ScreensName.None && screen.backgroundScreen != this._currentBackgroundScene)
                {
                    // create new background
                    SceneManager.LoadSceneAsync(screen.backgroundScreen.ToString(), LoadSceneMode.Additive);
                    this._currentBackgroundScene = screen.backgroundScreen;
                }
            }

            // fade in
            if (screen.rootCanvasGroup != null && screen.fadeInTime > 0)
            {
                screen.rootCanvasGroup.alpha = 0.0f;
                DOTween.To(() => screen.rootCanvasGroup.alpha, (x) => screen.rootCanvasGroup.alpha = x, 1.0f, screen.fadeInTime);
            }

            if (this._argumentsStack.Count == 0)
            {
                this.PushArgumentsInStack(new ArgumentsStackItem<IScreenArguments>(screen.gameObject.scene.name, screenArgs));
            }
            if (this.CurrentScreenArguments.IsNone && screenArgs.IsSome)
            {
                this.CurrentScreenArguments = screenArgs;
            }
        } // ScreenAwake

        private void Init()
        {
            foreach (GameObject o in this.ScreensSettings.globalObjects)
            {
                GameObject.DontDestroyOnLoad(GameObject.Instantiate(o));
            }
            foreach (PlatformSpecific ps in this.ScreensSettings.psGlobalObjects)
            {
                if (ps.Keep)
                {
                    GameObject.DontDestroyOnLoad(GameObject.Instantiate(ps.gObject));
                }
            }
        } // Init

        private void PushArgumentsInStack(ArgumentsStackItem<IScreenArguments> arguments)
        {
            _argumentsStack.Add(arguments);
        }

        private void LoadScreen(string screenName)
        {
#if PANELS_DEBUG_ON
            Debug.Log(string.Format("[LoadScreen] name = '{0}'", screenName));
#endif
            StartCoroutine(this.LoadScreenCoroutine(screenName, this._currentScreen));
        }

        private IEnumerator LoadScreenCoroutine(string newScreenName, UIScreenManager oldScreen)
        {
            this.DisableEvents();

            if (oldScreen != null)
            {
                yield return this.FadeOutScreen(oldScreen);
                if (oldScreen != null)
                {
                    GameObject.Destroy(oldScreen.gameObject);
                }
            }

            Scene oldScene = SceneManager.GetActiveScene();

            AsyncOperation load = SceneManager.LoadSceneAsync(newScreenName, LoadSceneMode.Additive);

            if (!load.isDone)
            {
                yield return load;
            }

            SceneManager.UnloadSceneAsync(oldScene); // it's here to avoid <<Unloading the last loaded scene "...", is not supported>>

            Scene currentScene = SceneManager.GetSceneByName(newScreenName);
            SceneManager.SetActiveScene(currentScene);
        } // LoadScreenCoroutine

        private IEnumerator FadeOutScreen(UIScreenManager screen)
        {
            if (screen is ICustomScreenFade)
            {
                yield return StartCoroutine((screen as ICustomScreenFade).CustomFadeOut());
            }
            else if (screen.rootCanvasGroup != null && screen.fadeOutTime > 0)
            {
                screen.rootCanvasGroup.interactable = false;
                Tweener tween = DOTween.To(() => screen.rootCanvasGroup.alpha,
                                    (x) => screen.rootCanvasGroup.alpha = x,
                                    0.0f,
                                    screen.fadeOutTime);
                yield return tween.WaitForCompletion();
                if (screen.uiCamera != null)
                {
                    GameObject.Destroy(screen.uiCamera.gameObject);
                }
            }
            else
            {
                yield return 0;
            }
        } // FadeOutScreen

        private void DisableEvents()
        {
            UnityEngine.EventSystems.EventSystem es = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
            if (es != null)
            {
                es.enabled = false;
            }
            else
            {
#if PANELS_DEBUG_ON
                Scene activeScene = SceneManager.GetActiveScene();
                Debug.LogWarning(string.Format("[ScreensManager] [DisableEvents] Cannot find EventSystem on the scene '{0}'", activeScene.name));
#endif
            }
        } // DisableEvents

    } // ScreensManager
}