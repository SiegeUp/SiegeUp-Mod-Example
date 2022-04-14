using UnityEngine;
using System.Linq;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiegeUp.ModdingPlugin.DevUtils
{
	[ExecuteInEditMode]
	public class PluginVersionChecker : ScriptableObject
	{
		private static DateTime _lastUpdateTime = DateTime.MinValue;

		private const string _manifestFileName = @"Assets\SiegeUp.ModdingPlugin\package.json";
		private const int UpdatePeriodSec = 2;

#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static void Init()
		{
			if (!File.Exists(_manifestFileName))
				return;
			EditorApplication.update -= OnEditorUpdate;
			EditorApplication.update += OnEditorUpdate;
		}

		private static void OnEditorUpdate()
		{
			if ((DateTime.UtcNow - _lastUpdateTime).TotalSeconds < UpdatePeriodSec)
				return;
			_lastUpdateTime = DateTime.UtcNow;
			var versionInManifest = GetPluginVersionFromManifest();
			if (ModsLoader.Version != versionInManifest)
				Debug.LogError($"Don't forget to update plugin version!\n" +
					$"Manifest ver: {versionInManifest}. ModsLoader ver: {ModsLoader.Version}");
		}

		private static string GetPluginVersionFromManifest()
		{
			var data = File.ReadAllLines(_manifestFileName);
			var versionInfo = data.FirstOrDefault(x => x.Contains("\"version\":"));
			return GetVersionFromJsonString(versionInfo);
		}

		private static string GetVersionFromJsonString(string infoLine)
		{
			infoLine = infoLine.Replace(",", "");
			int separatorIndex = infoLine.LastIndexOf(':');
			return infoLine.Substring(separatorIndex + 3, infoLine.Length - separatorIndex - 4);
		}
#endif
	}
}