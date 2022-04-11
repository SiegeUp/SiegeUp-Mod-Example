using System.Collections.Generic;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	[CreateAssetMenu(menuName = "Translation (Mod)")]
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
