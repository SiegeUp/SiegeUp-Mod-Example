using System;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
class BuildingConfig : ScriptableObject
{
    public static BuildingConfig Instance;

    public string ModsFolder
    {
        get
        {
            if (string.IsNullOrEmpty(modsFolder))
            {
                modsFolder = GetDefaultModsFolder();
            }

            if (!Directory.Exists(modsFolder))
            {
#if UNITY_EDITOR
                modsFolder = EditorUtils.ShowOpenFolderDialogue("Game folder not found. Please select output folder for mods");
#else
                return "";
#endif
            }
            return modsFolder;
        }
        set => modsFolder = value;
    }

    [SerializeField] private string modsFolder;

    private void OnEnable() => Instance = this;

    public static string GetDefaultModsFolder()
    {
        return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "..",
                "LocalLow",
                "Zdorovtsov",
                "SiegeUp!",
                "mods");
    }
}
