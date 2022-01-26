using UnityEditor;

#if UNITY_EDITOR
namespace SiegeUp.ModdingPlugin
{
	class SiegeUpModUtils
	{
		public static bool ValidateMetaInfo(SiegeUpModBase mod)
		{
			if (string.IsNullOrEmpty(mod.ModInfo.ModName))
				mod.ModInfo.ModName = mod.name;
			if (!FileUtils.IsValidFolderName(mod.ModInfo.ModName))
			{
				EditorUtility.DisplayDialog("Error", "Invalid mod name.\nMod name should not contain the following chars:\n" + string.Join(" ", System.IO.Path.GetInvalidPathChars()), "Ok");
				return false;
			}
			return true;
		}
	}

}
#endif