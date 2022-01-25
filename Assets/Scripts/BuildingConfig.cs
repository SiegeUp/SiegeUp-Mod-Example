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
        private const string ConfigFolderName = "Config";

        private void OnEnable() => Instance = this;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void FindInstance()
        {
            string[] assets = AssetDatabase.FindAssets($"Default t:{nameof(BuildingConfig)}");
            if (assets.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                Instance = AssetDatabase.LoadAssetAtPath<BuildingConfig>(path);
            }
			else
			{
                BuildingConfig asset = CreateInstance<BuildingConfig>();
                if (!AssetDatabase.IsValidFolder($"Assets/{ConfigFolderName}"))
                    AssetDatabase.CreateFolder("Assets", ConfigFolderName);
                AssetDatabase.CreateAsset(asset, $"Assets/{ConfigFolderName}/Default config.asset");
                AssetDatabase.SaveAssets();
                Instance = asset;
			}
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