using System;

namespace SiegeUp.ModdingPlugin
{
	[Serializable]
	public class VersionInfo
	{
		public int Major;
		public int Minor;
		public int Revision;

		public VersionInfo(string version)
		{
			var data = version.Split('.');
			int revisionLength = data[2].IndexOf('r');
			if (revisionLength <= 0)
				revisionLength = data[2].Length;
			Major = int.Parse(data[0]);
			Minor = int.Parse(data[1]);
			Revision = int.Parse(data[2].Substring(0, revisionLength));
		}

		public VersionInfo(int major, int minor, int revision)
		{
			Major = major;
			Minor = minor;
			Revision = revision;
		}

		public bool Supports(VersionInfo other)
		{
			return Major == other.Major && Minor == other.Minor && Revision >= other.Revision;
		}

		public bool IsSupportedBy(VersionInfo other)
		{
			return other.Supports(this);
		}

		public override string ToString()
		{
			return $"{Major}.{Minor}.{Revision}";
		}
	}
}
