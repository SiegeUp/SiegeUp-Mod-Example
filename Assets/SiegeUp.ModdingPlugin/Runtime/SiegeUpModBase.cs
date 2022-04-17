using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiegeUp.ModdingPlugin
{
	[CreateAssetMenu(menuName = "New SiegeUp Mod")]
	public class SiegeUpModBase : ScriptableObject
	{
		public SiegeUpModMeta ModInfo = new SiegeUpModMeta();
		public List<ObjectRecord> ObjectRecords = new List<ObjectRecord>();

		public List<GameObject> GetAllObjects()
		{
			return ObjectRecords.ConvertAll(i => i.Prefab);
		}

		public bool Validate()
		{
			RemoveNullRecords();
			var result = ModInfo.Validate(this);
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
			return result;
		}

		private void RemoveNullRecords()
		{
			ObjectRecords = ObjectRecords.Where(x => x != null && x.Prefab).ToList();
		}

		public void UpdateBuildInfo(PlatformShortName platform)
		{
			UpdateBuildInfo(platform, new SiegeUpModBundleInfo(platform, ModsLoader.Version, "1.1.102r19"));
		}

		public void UpdateBuildInfo(PlatformShortName platform, SiegeUpModBundleInfo prevBuildInfo)
		{
			if (prevBuildInfo == null)
				ModInfo.RemoveBuildInfo(platform);
			else
				ModInfo.UpdateBuildInfo(prevBuildInfo);
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}

		[System.Serializable]
		public class ObjectRecord
		{
			public GameObject Prefab;
			public Translation Name;
			public Texture2D Icon;
		}
	}
}