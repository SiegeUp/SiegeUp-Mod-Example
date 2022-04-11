using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	[Serializable]
	public class SiegeUpModMeta
	{
		public string ModName;
		public string AuthorName;
		[TextArea(1, 10)]
		public string Description;
		[Min(1)]
		public int Version = 1;
		[HideInInspector]
		public List<SiegeUpModBundleInfo> BuildsInfo = new List<SiegeUpModBundleInfo>();
		[HideInInspector]
		public string ModSourceUrl;
		public string Id;

		public bool Validate(SiegeUpModBase modBase)
		{
			if (string.IsNullOrEmpty(Id))
				Id = Guid.NewGuid().ToString("N");

			if (string.IsNullOrEmpty(ModName))
				ModName = modBase.name;
#if UNITY_EDITOR
			if (ModName.Contains(' '))
			{
				UnityEditor.EditorUtility.DisplayDialog(
					"Warning",
					$"All spaces in the mod name have been replaced with '_' char due to some restrictions",
					"Ok");
			}
#endif
			ModName = ModName.Replace(' ', '_');

			if (!FileUtils.IsValidFolderName(ModName))
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.DisplayDialog(
					"Error",
					$"Invalid mod name.\nMod name should not contain the following chars:\n{string.Join(" ", Path.GetInvalidPathChars())}",
					"Ok");
#endif
				return false;
			}
			BuildsInfo = BuildsInfo
				.Where(x => File.Exists(FileUtils.GetBundlePath(this, x.Platform)))
				.ToList();
			return true;
		}

		public void UpdateBuildInfo(SiegeUpModBundleInfo bundleInfo)
		{
			var oldRecordIndex = BuildsInfo.FindIndex(x => x.Platform == bundleInfo.Platform);
			if (oldRecordIndex != -1)
				BuildsInfo.RemoveAt(oldRecordIndex);
			BuildsInfo.Add(bundleInfo);
		}

		public bool TryGetBuildInfo(PlatformShortName platform, out SiegeUpModBundleInfo buildInfo)
		{
			buildInfo = BuildsInfo.FirstOrDefault(x => x.Platform == platform);
			return buildInfo != null;
		}
	}

	[Serializable]
	public class SiegeUpModBundleInfo
	{
		public PlatformShortName Platform;
		public VersionInfo PluginVersion;
		public VersionInfo GameVersion;

		public SiegeUpModBundleInfo(PlatformShortName platform, string pluginVersion, string gameVersion)
		{
			Platform = platform;
			PluginVersion = new VersionInfo(pluginVersion);
			GameVersion = new VersionInfo(gameVersion);
		}
	}
}