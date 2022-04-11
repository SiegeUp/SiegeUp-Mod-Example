using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiegeUp.ModdingPlugin
{
	[ExecuteInEditMode]
	public class SiegeUpModdingPluginConfig : ScriptableObject
	{
		public static SiegeUpModdingPluginConfig Instance;

		public bool IsValidModsFolder => !string.IsNullOrEmpty(ModsFolder);
		public string ModsFolder
		{
			get
			{
				if (string.IsNullOrEmpty(modsFolder))
					ModsFolder = FileUtils.GetDefaultModsFolder();
				return modsFolder;
			}
			set
			{
				modsFolder = value;
#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
#endif
			}
		}

		public string PluginVersion
		{
			get
			{
				if (string.IsNullOrEmpty(pluginVersion))
					PluginVersion = FindPluginVersion();
				return pluginVersion;
			}
			private set
			{
				pluginVersion = value;
#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
#endif
			}
		}

		[SerializeField]
		private string modsFolder;
		[SerializeField]
		private string pluginVersion;
		private const string ConfigFolderName = "SiegeUp Modding Plugin Config";
		private const string PluginName = "com.siegeup.moddingplugin";

		private void OnEnable() => Instance = this;

#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static void FindInstance()
		{
			string[] assets = AssetDatabase.FindAssets($"Default t:{nameof(SiegeUpModdingPluginConfig)}");
			if (assets.Length > 0)
			{
				string path = AssetDatabase.GUIDToAssetPath(assets[0]);
				Instance = AssetDatabase.LoadAssetAtPath<SiegeUpModdingPluginConfig>(path);
			}
			else
			{
				SiegeUpModdingPluginConfig asset = CreateInstance<SiegeUpModdingPluginConfig>();
				if (!AssetDatabase.IsValidFolder($"Assets/{ConfigFolderName}"))
					AssetDatabase.CreateFolder("Assets", ConfigFolderName);
				AssetDatabase.CreateAsset(asset, $"Assets/{ConfigFolderName}/Default Config.asset");
				AssetDatabase.SaveAssets();
				Instance = asset;
			}
		}

		[InitializeOnLoadMethod]
		private static void UpdatePluginVersion()
		{
			Instance.PluginVersion = FindPluginVersion();
		}
#endif

		public static string FindPluginVersion()
		{
			var pluginInfo = FileUtils.GetInstalledPackageInfo(PluginName);
			if (pluginInfo != null)
			{
				return GetVersionFromJsonString(pluginInfo);
			}
			var packageManifest = FileUtils.GetPackageManifest(PluginName);
			if (packageManifest != null)
			{
				var versionInfo = packageManifest.FirstOrDefault(x => x.Contains("\"version\":"));
				return GetVersionFromJsonString(versionInfo);
			}
			return "Unknown";
		}

		private static string GetVersionFromJsonString(string infoLine)
		{
			infoLine = infoLine.Replace(",", "");
			int separatorIndex = infoLine.LastIndexOf(':');
			return infoLine.Substring(separatorIndex + 3, infoLine.Length - separatorIndex - 4);
		}
	}
}