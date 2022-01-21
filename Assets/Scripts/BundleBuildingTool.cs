using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
public class BundleBuildingTool
{
    public static readonly Dictionary<BuildTarget, string> SupportedPlatforms = new Dictionary<BuildTarget, string>
    {
        {BuildTarget.StandaloneWindows64, "Windows" },
        {BuildTarget.Android, "Android" },
        {BuildTarget.StandaloneLinux64, "Linux" },
        {BuildTarget.StandaloneOSX, "MacOS" },
    };

    public static void BuildAssetBundle(SiegeUpModBase modBase, params BuildTarget[] targetPlatforms)
	{
        string modDirectory = FileUtils.GetModFolder(modBase.ModInfo.ModName);
        Debug.Log("Output directory: " + modDirectory);      

        AssetBundleBuild[] map = new AssetBundleBuild[1];
        map[0].assetNames = new[] { AssetDatabase.GetAssetPath(modBase) };

        foreach (var platform in targetPlatforms)
        {
            map[0].assetBundleName = SupportedPlatforms[platform];
            BuildAssetBundle(modBase, map, platform, modDirectory);
            FileUtils.CreateModManifest(modDirectory, modBase.ModInfo);
        }
        FileUtils.ClearUnityFiles(modDirectory);
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
