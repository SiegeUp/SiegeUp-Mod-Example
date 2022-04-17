using System.Collections.Generic;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	public class ModsLoader
	{
		public static ModsLoader Instance { get; private set; }
		public readonly VersionInfo CurrentPluginVersion;
		public readonly VersionInfo CurrentGameVersion;
		public List<AssetBundle> LoadedBundles => _loadedBundles;
		public const string Version = "1.3.3";

		private readonly List<AssetBundle> _loadedBundles = new List<AssetBundle>();

		public ModsLoader(string gameVersion)
		{
			Instance = this;
			CurrentGameVersion = new VersionInfo(gameVersion);
			CurrentPluginVersion = new VersionInfo(Version);
		}

		public List<SiegeUpModBase> LoadInstalledMods()
		{
			List<SiegeUpModBase> mods = new List<SiegeUpModBase>();

			var platform = Utils.GetCurrentPlatform();
			if (platform == PlatformShortName.Unsupported)
			{
				Debug.LogError("Unable to load mods for current platform");
				return mods;
			}
			foreach (var meta in FileUtils.GetInstalledModsMeta())
			{
				if (!meta.TryGetBuildInfo(platform, out SiegeUpModBundleInfo buildInfo) || !CanLoad(buildInfo))
					continue;
				var mod = LoadBundle(FileUtils.GetBundlePath(meta, platform));
				if (mod == null)
					continue;
				mods.Add(mod);
			}
			return mods;
		}

		public SiegeUpModBase LoadBundle(string path)
		{
			var loadedAssetBundle = AssetBundle.LoadFromFile(path);
			if (loadedAssetBundle == null)
			{
				Debug.LogWarning($"Failed to load AssetBundle from {path}");
				return null;
			}
			var bundleAssets = loadedAssetBundle.LoadAllAssets<SiegeUpModBase>();

			if (bundleAssets.Length < 1)
			{
				loadedAssetBundle.Unload(true);
				Debug.LogWarning($"Failed to load AssetBundle from {path} because it has no {nameof(SiegeUpModBase)} asset");
				return null;
			}
			_loadedBundles.Add(loadedAssetBundle);
			return bundleAssets[0];
		}

		public void UnloadMods()
		{
			foreach (var bundle in _loadedBundles)
				bundle.Unload(true);
			_loadedBundles.Clear();
		}

		public bool CanLoad(SiegeUpModBundleInfo buildInfo)
		{
			return CurrentPluginVersion.Supports(new VersionInfo(buildInfo.PluginVersion))
				&& CurrentGameVersion.Supports(new VersionInfo(buildInfo.GameVersion));
		}
	}
}
