using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	public class BundleBuildingTool
	{
		public static readonly Dictionary<BuildTarget, PlatformShortName> SupportedPlatforms = new Dictionary<BuildTarget, PlatformShortName>
		{
			{BuildTarget.StandaloneWindows, PlatformShortName.Windows },
			{BuildTarget.Android, PlatformShortName.Android },
			{BuildTarget.StandaloneLinux64, PlatformShortName.Linux },
			{BuildTarget.StandaloneOSX, PlatformShortName.MacOS },
			{BuildTarget.iOS, PlatformShortName.IOS },
		};

		public static void BuildAssetBundle(SiegeUpModBase modBase, params BuildTarget[] targetPlatforms)
		{
			if (!modBase.Validate())
				return;
			RegeneratePrefabIds(modBase);
			string modDirectory = FileUtils.GetExpectedModFolder(modBase.ModInfo);
			if (modDirectory == null)
				return;
			Debug.Log("Output directory: " + modDirectory);

			AssetBundleBuild[] map = new AssetBundleBuild[1];
			map[0].assetNames = new[] { AssetDatabase.GetAssetPath(modBase) };

			foreach (var platform in targetPlatforms)
			{
				map[0].assetBundleName = FileUtils.GetBundleFileName(modBase.ModInfo, SupportedPlatforms[platform]);
				BuildAssetBundle(modBase, map, platform, modDirectory);
				FileUtils.CreateModMetaFile(modDirectory, modBase.ModInfo);
			}
			AssetDatabase.Refresh();
		}

		private static void BuildAssetBundle(SiegeUpModBase modBase, AssetBundleBuild[] map, BuildTarget targetPlatform, string outputDir)
		{
			modBase.ModInfo.TryGetBuildInfo(SupportedPlatforms[targetPlatform], out var prevBuildInfo);
			modBase.UpdateBuildInfo(SupportedPlatforms[targetPlatform]);
			var manifest = BuildPipeline.BuildAssetBundles(outputDir, map, BuildAssetBundleOptions.StrictMode, targetPlatform);
			if (manifest != null)
			{
				Debug.Log($"Mod \"{modBase.ModInfo.ModName}\" for \"{SupportedPlatforms[targetPlatform]}\" platform was builded successfully!");
				return;
			}
			modBase.UpdateBuildInfo(SupportedPlatforms[targetPlatform], prevBuildInfo);
		}

		static void RegeneratePrefabIds(SiegeUpModBase modBase)
		{
			const string prefabRefName = "PrefabRef";
			var allObjects = modBase.GetAllObjects();
			foreach (var obj in allObjects)
			{
				var prefabRef = obj.GetComponent(prefabRefName);
				if (!prefabRef)
					throw new UnityException($"Please add {prefabRefName} component in object {obj.name}");
				prefabRef.GetType().GetMethod("Regenerate").Invoke(prefabRef, default);
			}
		}
	}
}