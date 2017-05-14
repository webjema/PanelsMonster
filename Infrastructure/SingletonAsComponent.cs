using UnityEngine;

namespace com.webjema.Infrastructure
{

    public class SingletonAsComponent<T> : MonoBehaviour where T : SingletonAsComponent<T>
    {
        private static T __Instance;
        private bool _alive = true;

        protected static SingletonAsComponent<T> _Instance
        {
            get
            {
                if (!__Instance)
                {
                    T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
                    if (managers != null)
                    {
                        if (managers.Length == 1)
                        {
                            __Instance = managers[0];
                            return __Instance;
                        }
                        else if (managers.Length > 1)
                        {
                            Debug.LogError(string.Format("You have more than one {0} in the scene. You only need 1, it's a singleton!", typeof(T).Name));
                            for (int i = 0; i < managers.Length; ++i)
                            {
                                T manager = managers[i];
                                Destroy(manager.gameObject);
                            }
                        }
                    }

                    GameObject go = new GameObject(typeof(T).Name, typeof(T));
                    __Instance = go.GetComponent<T>();
                    DontDestroyOnLoad(__Instance.gameObject);
                }
                return __Instance;
            }
            set
            {
                __Instance = value as T;
            }
        } // SingletonAsComponent

        public static bool IsAlive
        {
            get
            {
                if (__Instance == null)
                {
                    return false;
                }
                return __Instance._alive;
            }
        } // IsAlive

        void OnDestroy()
        {
            _alive = false;
        }

        void OnApplicationQuit()
        {
            _alive = false;
        }

    } // SingletonAsComponent
}