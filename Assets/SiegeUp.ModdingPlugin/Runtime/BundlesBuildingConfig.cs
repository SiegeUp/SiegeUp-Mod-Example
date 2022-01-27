using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiegeUp.ModdingPlugin
{
    [ExecuteInEditMode]
    public class BundlesBuildingConfig : ScriptableObject
    {
        public static BundlesBuildingConfig Instance;

        public bool IsValidModsFolder
        {
            get => !string.IsNullOrEmpty(ModsFolder);
        }

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
                    modsFolder = "";
                }
                return modsFolder;
            }
            set => modsFolder = value;
        }

		[SerializeField] 
        private string modsFolder;
        private const string ConfigFolderName = "Config";

        private void OnEnable() => Instance = this;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void FindInstance()
        {
            string[] assets = AssetDatabase.FindAssets($"Default t:{nameof(BundlesBuildingConfig)}");
            if (assets.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                Instance = AssetDatabase.LoadAssetAtPath<BundlesBuildingConfig>(path);
            }
			else
			{
                BundlesBuildingConfig asset = CreateInstance<BundlesBuildingConfig>();
                if (!AssetDatabase.IsValidFolder($"Assets/{ConfigFolderName}"))
                    AssetDatabase.CreateFolder("Assets", ConfigFolderName);
                AssetDatabase.CreateAsset(asset, $"Assets/{ConfigFolderName}/Default config.asset");
                AssetDatabase.SaveAssets();
                Instance = asset;
			}
        }
#endif

        public static string GetDefaultGameFolder()
		{
            return Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                   "..",
                   "LocalLow",
                   "Zdorovtsov",
                   "SiegeUp!");
        }

        public static string GetDefaultModsFolder()
        {
            string gameFolder = GetDefaultGameFolder();
            if (!Directory.Exists(gameFolder))
                return "";
            string modsFolder = Path.Combine(gameFolder, "mods");
            FileUtils.CheckOrCreateDirectory(modsFolder);
            return modsFolder;
        }
    }
}