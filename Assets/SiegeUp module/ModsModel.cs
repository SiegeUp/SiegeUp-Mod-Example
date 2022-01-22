using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

class ModsModel
{
	private const string ModsFolderName = "mods";
	private const string MetaFileName = "meta.json";

	//public List<SiegeUpModBase> GetInstalledMods()
	//{
	//	Dictionary<string, int> modVersions = new Dictionary<string, int>();
	//	List<SiegeUpModBase> mods = new List<SiegeUpModBase>();

	//	string modsPath = Path.Combine(Application.persistentDataPath, ModsFolderName);
	//	string platformName = GetCurrentPlatformName();
	//	if (platformName == null)
	//		return mods;
	//	var modsFolders = Directory.GetDirectories(modsPath);
	//	foreach (var folder in modsFolders)
	//	{
	//		var meta = JsonUtility.FromJson<SiegeUpModMeta>(Path.Combine(folder, MetaFileName));
	//		if (!modVersions.ContainsKey(meta.ModName))
	//			modVersions.Add(meta.ModName, meta.Version);
	//		else if (modVersions[meta.ModName] >= meta.Version)
	//			continue;
			
	//	}
	//	//TODO
	//	foreach (var metaFile in metaFiles)
	//	{
	//		var mod = TryLoadBundle(Path.Combine(Path.GetDirectoryName(metaFile.Key), platformName));
	//		if (mod == null)
	//			continue;
	//		modVersions.Add(mod);
	//	}
	//	throw new NotImplementedException();
	//}

	public SiegeUpModBase TryLoadBundle(string path)
	{
		var loadedAssetBundle = AssetBundle.LoadFromFile(path);
		if (loadedAssetBundle == null)
		{
			Debug.Log("Failed to load AssetBundle from " + path);
			return null;
		}
		return loadedAssetBundle.LoadAllAssets<SiegeUpModBase>()[0];
	}

	private string GetCurrentPlatformName()
	{
		return Application.platform switch
		{
			RuntimePlatform.WindowsPlayer | RuntimePlatform.WindowsEditor => PlatformShortName.Windows.ToString(),
			RuntimePlatform.Android => PlatformShortName.Android.ToString(),
			RuntimePlatform.LinuxPlayer | RuntimePlatform.LinuxEditor => PlatformShortName.Linux.ToString(),
			RuntimePlatform.OSXPlayer | RuntimePlatform.OSXEditor => PlatformShortName.MacOS.ToString(),
			RuntimePlatform.IPhonePlayer => PlatformShortName.IOS.ToString(),
			_ => null
		};
	}
}
