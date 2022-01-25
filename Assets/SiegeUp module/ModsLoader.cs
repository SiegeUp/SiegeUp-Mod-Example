using System.Collections.Generic;
using System.IO;
using UnityEngine;

class ModsLoader
{
	private const string ModsFolderName = "mods";
	private const string MetaFileName = "meta.json";

	public List<SiegeUpModBase> GetInstalledMods()
	{
		List<SiegeUpModBase> mods = new List<SiegeUpModBase>();

		string modsPath = Path.Combine(Application.persistentDataPath, ModsFolderName);
		string platformName = GetCurrentPlatformName();
		if (platformName == null)
			return mods;
		var modsFolders = Directory.GetDirectories(modsPath);
		foreach (var folder in modsFolders)
		{
			var mod = TryLoadBundle(Path.Combine(folder, platformName));
			if (mod == null)
				continue;
			mods.Add(mod);
		}
		return mods;
	}

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
