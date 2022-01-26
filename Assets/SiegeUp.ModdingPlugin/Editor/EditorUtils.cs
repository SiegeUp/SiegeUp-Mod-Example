using UnityEditor;

namespace SiegeUp.ModdingPlugin
{
	class EditorUtils
	{
		public static bool ShowOpenFolderDialogue(string message, out string selectedPath)
		{
			selectedPath = "";
			if (!EditorUtility.DisplayDialog("", message, "Ok", "Cancel"))
				return false;
			selectedPath = EditorUtility.SaveFolderPanel("Select a folder", "", "");
			selectedPath = FileUtils.FixPathSeparator(selectedPath);
			return !string.IsNullOrEmpty(selectedPath);
		}
	}
}