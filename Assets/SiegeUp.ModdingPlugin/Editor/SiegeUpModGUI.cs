using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	[CustomEditor(typeof(SiegeUpModBase))]
	public class SiegeUpModGUI : Editor
	{
		private SiegeUpModBase _targetObject;

		private void OnEnable() => _targetObject = (SiegeUpModBase)target;

		public override void OnInspectorGUI()
		{
			GUILayout.Space(5);
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Build for platform:");
			if (GUILayout.Button("Open mod folder") && ValidateModsFolder())
				FileUtils.OpenExplorer(FileUtils.GetExpectedModFolder(_targetObject.ModInfo));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			int buttonIndex = 0;
			foreach (var platform in BundleBuildingTool.SupportedPlatforms)
			{
				if (GUILayout.Button(platform.Value.ToString()) && ValidateModsFolder())
				{
					BundleBuildingTool.BuildAssetBundle(_targetObject, platform.Key);
#if UNITY_2019_4
					GUIUtility.ExitGUI();
#endif
				}
				buttonIndex++;
				if (buttonIndex % 3 == 0)
				{
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
			}
			GUILayout.EndHorizontal();

			if (GUILayout.Button("All", GUILayout.Height(25)) && ValidateModsFolder())
			{
				BundleBuildingTool.BuildAssetBundle(_targetObject, BundleBuildingTool.SupportedPlatforms.Keys.ToArray());
#if UNITY_2019_4
				GUIUtility.ExitGUI();
#endif
			}
			GUILayout.EndVertical();
			GUILayout.Space(5);

			base.OnInspectorGUI();
		}

		private bool ValidateModsFolder()
		{
			if (SiegeUpModdingPluginConfig.Instance.IsValidModsFolder)
				return true;
			if (EditorUtils.ShowOpenFolderDialogue("Game folder not found. Please select output folder for mods", out string newModsFolder))
				SiegeUpModdingPluginConfig.Instance.ModsFolder = newModsFolder;
			return SiegeUpModdingPluginConfig.Instance.IsValidModsFolder;
		}
	}
}