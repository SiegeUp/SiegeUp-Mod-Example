using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SiegeUp.ModdingPlugin
{
    [CreateAssetMenu]
    public class Translation : ScriptableObject
    {
        [TextArea]
        public string MasterText;

        [System.Serializable]
        public struct Localization
        {
            public SystemLanguage LanguageId;
            [TextArea]
            public string Text;
            [TextArea]
            public string AutoText;
        }

        public List<Localization> Localizations = new List<Localization>();

    }
}
