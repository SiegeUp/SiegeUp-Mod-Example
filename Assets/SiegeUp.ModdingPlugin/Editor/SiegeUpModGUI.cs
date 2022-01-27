using UnityEditor;
using UnityEngine;
using System.Linq;

namespace SiegeUp.ModdingPlugin
{
	[CustomEditor(typeof(SiegeUpModBase))]
	public class SiegeUpModGUI : Editor
	{
		private SiegeUpModBase targetObject;

		private void OnEnable() => targetObject = (SiegeUpModBase)target;

		public override void OnInspectorGUI()
		{
			GUILayout.Space(5);
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Build for platform:");
			if (GUILayout.Button("Open mod folder") && ValidateModsFolder())
				FileUtils.OpenExplorer(FileUtils.TryGetModFolder(targetObject.ModInfo.ModName));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			int buttonIndex = 0;
			foreach (var platform in BundleBuildingTool.SupportedPlatforms)
			{
				if (GUILayout.Button(platform.Value.ToString()) && ValidateModsFolder())
					BundleBuildingTool.BuildAssetBundle(targetObject, platform.Key);
				buttonIndex++;
				if (buttonIndex % 3 == 0)
				{
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
			}
			GUILayout.EndHorizontal();

			if (GUILayout.Button("All", GUILayout.Height(25)) && ValidateModsFolder())
				BundleBuildingTool.BuildAssetBundle(targetObject, BundleBuildingTool.SupportedPlatforms.Keys.ToArray());

			GUILayout.EndVertical();
			GUILayout.Space(5);

			base.OnInspectorGUI();
		}

		private bool ValidateModsFolder()
		{
			if (BundlesBuildingConfig.Instance.IsValidModsFolder)
				return true;
			if (EditorUtils.ShowOpenFolderDialogue("Game folder not found. Please select output folder for mods", out string newModsFolder))
				BundlesBuildingConfig.Instance.ModsFolder = newModsFolder;
			return BundlesBuildingConfig.Instance.IsValidModsFolder;
		}
	}
}