using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiegeUp.ModdingPlugin
{
    [ExecuteInEditMode]
    class BuildingConfig : ScriptableObject
    {
        public static BuildingConfig Instance;

        public string ModsFolder
        {
            get
            {
                if (string.IsNullOrEmpty(modsFolder))
                {
                    modsFolder = GetDefaultModsFolder();
                }

                if (!Directory.Exists(modsFolder))
                {
#if UNITY_EDITOR
                    modsFolder = EditorUtils.ShowOpenFolderDialogue("Game folder not found. Please select output folder for mods");
#else
                return "";
#endif
                }
                return modsFolder;
            }
            set => modsFolder = value;
        }

        [SerializeField] private string modsFolder;

        private void OnEnable() => Instance = this;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void FindInstance()
        {
            string path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"Default t:{nameof(BuildingConfig)}")[0]);
            Instance = AssetDatabase.LoadAssetAtPath<BuildingConfig>(path);
            Debug.Assert(Instance != null, "Instance was found, path: " + path);
        }
#endif

        public static string GetDefaultModsFolder()
        {
            return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "..",
                    "LocalLow",
                    "Zdorovtsov",
                    "SiegeUp!",
                    "mods");
        }
    }
}