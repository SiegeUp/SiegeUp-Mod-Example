using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	public class ModsLoader
	{
		private const string ModsFolderName = "mods";
		private const string MetaFileName = "meta.json";

		public List<SiegeUpModBase> GetInstalledMods()
		{
			List<SiegeUpModBase> mods = new List<SiegeUpModBase>();

			string modsPath = Path.Combine(Application.persistentDataPath, ModsFolderName);
			string platformName = GetCurrentPlatformName().ToLower();
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
			var bundleAssets = loadedAssetBundle.LoadAllAssets<SiegeUpModBase>();
			loadedAssetBundle.Unload(false);
			if (bundleAssets.Length < 1)
				return null;
			return bundleAssets[0];
		}

		private string GetCurrentPlatformName()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
					return PlatformShortName.Windows.ToString();
				case RuntimePlatform.Android:
					return PlatformShortName.Android.ToString();
				case RuntimePlatform.LinuxPlayer:
				case RuntimePlatform.LinuxEditor:
					return PlatformShortName.Linux.ToString();
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.OSXEditor:
					return PlatformShortName.MacOS.ToString();
				case RuntimePlatform.IPhonePlayer:
					return PlatformShortName.IOS.ToString();
				default:
					return null;
			};
		}
	}
}
