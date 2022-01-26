using UnityEditor;

#if UNITY_EDITOR
namespace SiegeUp.ModdingPlugin
{
	class EditorUtils
	{
		public static string ShowOpenFolderDialogue(string message)
		{
			if (!EditorUtility.DisplayDialog("", message, "Ok", "Cancel"))
				return "";
			string path = EditorUtility.SaveFolderPanel("Select a folder", "", "");
			return FileUtils.FixPathSeparator(path);
		}
	}
}
#endif