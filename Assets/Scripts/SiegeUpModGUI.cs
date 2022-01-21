#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(SiegeUpModBase))]
public class SiegeUpModGUI : Editor
{
	private SiegeUpModBase instance;

	private void OnEnable() => instance = (SiegeUpModBase)target;

	public override void OnInspectorGUI()
	{
		GUILayout.Space(5);
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Build for platform:");
#if UNITY_EDITOR_WIN
		if (GUILayout.Button("Open output folder"))
			System.Diagnostics.Process.Start("explorer.exe", FileUtils.GetModFolder(instance.ModInfo.ModName));
#endif
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		int buttonIndex = 0;
		foreach (var platform in BundleBuildingTool.SupportedPlatforms)
		{
			if (GUILayout.Button(platform.Value))
				BundleBuildingTool.BuildAssetBundle(instance, platform.Key);
			buttonIndex++;
			if (buttonIndex % 3 == 0)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}
		GUILayout.EndHorizontal();

		if (GUILayout.Button("All", GUILayout.Height(25)))
			BundleBuildingTool.BuildAssetBundle(instance, BundleBuildingTool.SupportedPlatforms.Keys.ToArray());

		GUILayout.EndVertical();
		GUILayout.Space(5);

		base.OnInspectorGUI();
	}
}
#endif