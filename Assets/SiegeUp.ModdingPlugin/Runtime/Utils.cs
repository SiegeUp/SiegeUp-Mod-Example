using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	public class Utils
	{
		public static PlatformShortName GetCurrentPlatform()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
					return PlatformShortName.Windows;
				case RuntimePlatform.Android:
					return PlatformShortName.Android;
				case RuntimePlatform.LinuxPlayer:
				case RuntimePlatform.LinuxEditor:
					return PlatformShortName.Linux;
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.OSXEditor:
					return PlatformShortName.MacOS;
				case RuntimePlatform.IPhonePlayer:
					return PlatformShortName.IOS;
				default:
					return PlatformShortName.Unsupported;
			};
		}
	}
}
