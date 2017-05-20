using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Linq;
using System.Collections.Generic;

using com.webjema.Functional;
using com.webjema.Infrastructure;
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
        private List<ArgumentsStackItem> _argumentsStack = new List<ArgumentsStackItem>();

        public void PushScreen(ScreensName screenName)
        {
            this.PushScreen(screenName.ToString(), new Hashtable());
        }

        public void PushScreen(ScreensName screenName, Hashtable args)
        {
            this.PushScreen(screenName.ToString(), args);
        }

        public void PushScreen(string screenName)
        {
            this.PushScreen(screenName, new Hashtable());
        }

        public void PushScreen(string screenName, Hashtable args)
        {
            this.PushArgumentsInStack(new ArgumentsStackItem(screenName, args));

            this.LoadScreen(screenName);
        }

        public Hashtable ScreenAwake(UIScreenManager screen, Hashtable defaultArgs = null)
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
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(this._currentBackgroundScene.ToString()));
                    this._currentBackgroundScene = ScreensName.None;
                }
                SceneManager.LoadSceneAsync(screen.backgroundScreen.ToString(), LoadSceneMode.Additive);
                this._currentBackgroundScene = screen.backgroundScreen;
            }

            // fade in
            if (screen.rootCanvasGroup != null && screen.fadeInTime > 0)
            {
                screen.rootCanvasGroup.alpha = 0.0f;
                DOTween.To(() => screen.rootCanvasGroup.alpha, (x) => screen.rootCanvasGroup.alpha = x, 1.0f, screen.fadeInTime);
            }

            if (this._argumentsStack.Count == 0)
            {
                this.PushArgumentsInStack(new ArgumentsStackItem(screen.gameObject.scene.name, new Hashtable()));
            }

            return this._argumentsStack[this._argumentsStack.Count() - 1].args;
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

        private void PushArgumentsInStack(ArgumentsStackItem screen)
        {
            _argumentsStack.Add(screen);
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

    class ArgumentsStackItem
    {
        public string screenName;
        public Hashtable args;

        public ArgumentsStackItem(string screenName, Hashtable arg)
        {
            this.screenName = screenName;
            this.args = arg;
        }
    } // ArgumentsStackItem
}