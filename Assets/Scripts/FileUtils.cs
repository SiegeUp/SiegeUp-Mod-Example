using System;
using System.IO;
using UnityEngine;

public class FileUtils
{
    public static void OpenExplorer(string path)
    {
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            System.Diagnostics.Process.Start("explorer.exe", path);
        else
            Debug.LogWarning($"Unable to open {path}");
    }

	public static string FixPathSeparator(string path)
	{
        if (path.Contains(Path.AltDirectorySeparatorChar.ToString()))
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        return path;
	}

	public static void CheckOrCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static string TryGetModFolder(string modName)
	{
        if (string.IsNullOrEmpty(BuildingConfig.Instance.ModsFolder))
            return null;
        string path = Path.Combine(BuildingConfig.Instance.ModsFolder, modName);
        CheckOrCreateDirectory(path);
        return path;
	}

    public static void CreateModMetaFile(string modDirectory, SiegeUpModMeta modInfo)
	{
        string manifestData = JsonUtility.ToJson(modInfo);
        string filePath = Path.Combine(modDirectory, "meta.json");
        File.WriteAllText(filePath, manifestData);
	}

    public static bool IsValidFolderName(string name)
    {
        return name.IndexOfAny(Path.GetInvalidPathChars()) == -1;
    }
}