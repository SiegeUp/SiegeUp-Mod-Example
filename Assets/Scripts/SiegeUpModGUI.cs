using UnityEditor;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
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
			if (GUILayout.Button("Open mod folder"))
				FileUtils.OpenExplorer(FileUtils.TryGetModFolder(targetObject.ModInfo.ModName));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			int buttonIndex = 0;
			foreach (var platform in BundleBuildingTool.SupportedPlatforms)
			{
				if (GUILayout.Button(platform.Value.ToString()))
					BundleBuildingTool.BuildAssetBundle(targetObject, platform.Key);
				buttonIndex++;
				if (buttonIndex % 3 == 0)
				{
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
			}
			GUILayout.EndHorizontal();

			if (GUILayout.Button("All", GUILayout.Height(25)))
				BundleBuildingTool.BuildAssetBundle(targetObject, BundleBuildingTool.SupportedPlatforms.Keys.ToArray());

			GUILayout.EndVertical();
			GUILayout.Space(5);

			base.OnInspectorGUI();
		}
	}
}
#endif