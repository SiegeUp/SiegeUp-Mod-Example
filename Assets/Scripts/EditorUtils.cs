using UnityEditor;

#if UNITY_EDITOR
class EditorUtils
{
#nullable enable
	public static string? GetFolderDialogue(string message)
	{
		if (!EditorUtility.DisplayDialog("", message, "Ok", "Cancel"))
			return null;
		return EditorUtility.SaveFolderPanel("Select a folder", "", null);
	}
#nullable disable
}
#endif
