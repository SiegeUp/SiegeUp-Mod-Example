using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
[CustomEditor(typeof(SiegeUpModBase))]
public class SiegeUpModGUI : Editor
{
	public override void OnInspectorGUI()
	{
		SiegeUpModBase instance = (SiegeUpModBase)target;

		GUILayout.BeginHorizontal(); 
		GUILayout.Label("Mod name", GUILayout.MaxWidth(80)); 
		instance.ModInfo.ModName = GUILayout.TextField(instance.ModInfo.ModName); 
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal(); 
		GUILayout.Label("Author name", GUILayout.MaxWidth(80)); 
		instance.ModInfo.AuthorName = GUILayout.TextField(instance.ModInfo.AuthorName); 
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal(); 
		GUILayout.Label("Description", GUILayout.MaxWidth(80)); 
		instance.ModInfo.Description = GUILayout.TextArea(instance.ModInfo.Description); 
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal(); 
		GUILayout.Label("Version", GUILayout.MaxWidth(80)); 
		instance.ModInfo.Version = GUILayout.TextField(instance.ModInfo.Version); 
		GUILayout.EndHorizontal();

		GUILayout.Space(5);
		GUILayout.Label("Build for platform:");

		GUILayout.BeginHorizontal();
		int buttonIndex = 0;
		foreach (var platform in BundleBuildingTool.SupportedPlatforms)
		{
			if (GUILayout.Button(platform.Value.ShortName))
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

		GUILayout.Space(10);

		base.OnInspectorGUI();
	}
}
#endif