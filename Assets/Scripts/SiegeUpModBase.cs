using System;
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
    }
}