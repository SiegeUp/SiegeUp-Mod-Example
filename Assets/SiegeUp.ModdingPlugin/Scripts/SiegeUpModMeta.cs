using System;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
    [Serializable]
    public class SiegeUpModMeta
    {
        public string ModName;
        public string AuthorName;
        [TextArea(1, 10)]
        public string Description;
        [Min(1)]
        public int Version = 1;
    }
}