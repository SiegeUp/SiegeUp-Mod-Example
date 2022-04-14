using UnityEngine;
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

		[SerializeField]
		private string modsFolder;
		private const string ConfigFolderName = "SiegeUp Modding Plugin Config";

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
#endif
	}
}