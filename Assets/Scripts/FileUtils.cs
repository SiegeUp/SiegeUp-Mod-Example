using System;
using System.IO;
using UnityEngine;

public class FileUtils
{
    public static string ModsOutputFolder
    { 
        get
        {
            if (string.IsNullOrEmpty(modsOutputFolder))
            {
                modsOutputFolder = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                        "..",
                        "LocalLow",
                        "Zdorovtsov",
                        "SiegeUp!",
                        "mods");
            }

            if (!Directory.Exists(modsOutputFolder))
            {
#if UNITY_EDITOR
                modsOutputFolder = EditorUtils.GetFolderDialogue("Game folder not found. Please select output folder for mods");
#else
                return null;
#endif
            }
            return modsOutputFolder;
        }
        set => modsOutputFolder = value;
    }

	private static string modsOutputFolder;

    public static void OpenExplorer(string path)
    {
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            System.Diagnostics.Process.Start("explorer.exe", path);
        else
            Debug.LogWarning($"Unable to open {path}");
    }

    public static void CheckOrCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static string TryGetModFolder(string modName)
	{
        if (ModsOutputFolder == null)
            return null;
        string path = Path.Combine(modsOutputFolder, modName);
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