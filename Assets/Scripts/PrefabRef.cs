using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
    public class PrefabRef : MonoBehaviour
    {
        [HideInInspector]
        public string prefabId;
        [HideInInspector]
        public bool ignore;
    }
}