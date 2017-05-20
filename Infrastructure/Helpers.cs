using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.webjema.PanelsMonster
{
    public static class Helpers
    {

        public static T LoadScriptableObject<T>() where T : ScriptableObject
        {
            System.Type type = typeof(T);
            return Resources.Load<T>(type.ToString().Split('.').ToList().Last());
        }

        [MenuItem("Assets/Create/ScreensSettings Object")]
        public static void CreateMyAsset()
        {
            ScreensSettings asset = ScriptableObject.CreateInstance<ScreensSettings>();

            AssetDatabase.CreateAsset(asset, "Assets/PanelsMonster/Public/Resources/ScreensSettings.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

    } // Helpers
}