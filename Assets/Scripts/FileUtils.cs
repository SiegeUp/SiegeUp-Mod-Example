using System;
using System.IO;
using UnityEngine;

public class FileUtils
{
    public static void CheckOrCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static string GetModFolder(string modName)
	{
        string path = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
                    "..",
                    "LocalLow",
                    "Zdorovtsov",
                    "SiegeUp!",
                    "mods",
                    modName);
        CheckOrCreateDirectory(path);
        return path;
	}

    public static void CreateModManifest(string modDirectory, SiegeUpModMeta modInfo)
	{
        string manifestData = JsonUtility.ToJson(modInfo);
        string filePath = Path.Combine(modDirectory, "meta.json");
        File.WriteAllText(filePath, manifestData);
	}

    public static void ClearUnityFiles(string modDirectory)
	{
        var filesToDelete = Directory.GetFiles(modDirectory, "*.manifest");
        foreach (var file in filesToDelete)
            File.Delete(file);
        string folderName = modDirectory.Substring(modDirectory.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        File.Delete(Path.Combine(modDirectory, folderName));
	}
}