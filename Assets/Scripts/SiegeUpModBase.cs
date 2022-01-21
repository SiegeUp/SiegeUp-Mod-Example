using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New SiegeUp Mod")]
public class SiegeUpModBase : ScriptableObject
{
    public SiegeUpModMeta ModInfo;
    public List<GameObject> Decorations = new List<GameObject>();

    public List<GameObject> GetAllAssets()
	{
        return new List<GameObject>(Decorations);
	}
}