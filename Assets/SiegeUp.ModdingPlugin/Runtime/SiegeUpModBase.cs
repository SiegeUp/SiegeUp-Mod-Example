using System.Collections.Generic;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	[CreateAssetMenu(menuName = "New SiegeUp Mod")]
	public class SiegeUpModBase : ScriptableObject
	{
		public SiegeUpModMeta ModInfo = new SiegeUpModMeta();
		public List<GameObject> Decorations = new List<GameObject>();

		public List<GameObject> GetAllAssets()
		{
			return new List<GameObject>(Decorations);
		}

		public bool ValidateMetaInfo()
		{
			if (string.IsNullOrEmpty(ModInfo.ModName))
				ModInfo.ModName = name;
			if (!FileUtils.IsValidFolderName(ModInfo.ModName))
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.DisplayDialog("Error", "Invalid mod name.\nMod name should not contain the following chars:\n" + string.Join(" ", System.IO.Path.GetInvalidPathChars()), "Ok");
#endif
				return false;
			}
			return true;
		}
	}
}