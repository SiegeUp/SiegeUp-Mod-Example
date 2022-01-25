using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace SiegeUp.ModdingPlugin
{
#if UNITY_EDITOR
    public class BundleBuildingTool
    {
        public static readonly Dictionary<BuildTarget, PlatformShortName> SupportedPlatforms = new Dictionary<BuildTarget, PlatformShortName>
        {
            {BuildTarget.StandaloneWindows64, PlatformShortName.Windows },
            {BuildTarget.Android, PlatformShortName.Android },
            {BuildTarget.StandaloneLinux64, PlatformShortName.Linux },
            {BuildTarget.StandaloneOSX, PlatformShortName.MacOS },
        };

        public static void BuildAssetBundle(SiegeUpModBase modBase, params BuildTarget[] targetPlatforms)
        {
            PrefabManager.updatePrefabManager();
            if (!SiegeUpModUtils.ValidateMetaInfo(modBase))
                return;
            string modDirectory = FileUtils.TryGetModFolder(modBase.ModInfo.ModName);
            if (modDirectory == null)
                return;
            Debug.Log("Output directory: " + modDirectory);

            AssetBundleBuild[] map = new AssetBundleBuild[1];
            map[0].assetNames = new[] { AssetDatabase.GetAssetPath(modBase) };

            foreach (var platform in targetPlatforms)
            {
                map[0].assetBundleName = SupportedPlatforms[platform].ToString();
                BuildAssetBundle(modBase, map, platform, modDirectory);
                FileUtils.CreateModMetaFile(modDirectory, modBase.ModInfo);
            }
            AssetDatabase.Refresh();
        }

        private static void BuildAssetBundle(SiegeUpModBase modBase, AssetBundleBuild[] map, BuildTarget targetPlatform, string outputDir)
        {
            var manifest = BuildPipeline.BuildAssetBundles(outputDir, map, BuildAssetBundleOptions.StrictMode, targetPlatform);
            if (manifest != null)
                Debug.Log($"Mod {modBase.ModInfo.ModName} for {targetPlatform} was builded successfully!");
        }
    }
#endif
}