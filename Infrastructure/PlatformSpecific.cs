using UnityEngine;

namespace com.webjema.PanelsMonster
{
    [System.Serializable]
    public class PlatformSpecific
    {

        public GameObject gObject;
        public bool IsMobile;
        public bool IsDesktop;
        public bool IsEditor;
        public bool IsDevelopmentBuild;
        public bool isIos;
        public bool isAndroid;
        public bool isWinStore;
        public bool isSteam;
        public bool isEnableConsole;

        public UnityEngine.Events.UnityEvent OnStart;

        public bool Keep
        {
            get
            {
                bool dev = false, mobile = false, desktop = false, editor = false, ios = false, android = false, winstore = false, steam = false;

                dev = IsDevBuild;
                editor = Application.isEditor;

                ios = IsIosPlatform;
                android = IsAndroidPlatform;
                mobile = IsMobilePlatform;
                desktop = IsDesktopPlatform;

                steam = PlatformSpecific.IsSteamBuild;

                bool keep = false;
                keep |= (mobile && IsMobile);
                keep |= (IsDesktop && desktop);
                keep |= (IsEditor && editor);
                keep |= (dev && IsDevelopmentBuild);
                keep |= (ios && isIos);
                keep |= (android && isAndroid);
                keep |= (steam && isSteam);
                keep |= (winstore && isWinStore);

                return keep;
            }
        }

        public static bool IsSteamBuild
        {
            get
            {
#if STEAM_BUILD
            return true;
#else
                return false;
#endif
            }
        }

        public static bool IsCloudBuild
        {
            get
            {
#if UNITY_CLOUD_BUILD
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsDevBuild
        {
            get
            {
#if ENABLE_DEV_BUILD
                return true;
#endif
                if (Debug.isDebugBuild || Application.isEditor)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool IsBuildAndroid
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsBuildIos
        {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsIosPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.IPhonePlayer || IsBuildIos;
            }
        }

        public static bool IsAndroidPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.Android || IsBuildAndroid;
            }
        }

        public static bool IsMobilePlatform
        {
            get
            {
                return IsIosPlatform || IsAndroidPlatform;
            }
        }

        public static bool IsDesktopPlatform
        {
            get
            {
                return !IsMobilePlatform;
            }
        }

    } // PlatformSpecific
}