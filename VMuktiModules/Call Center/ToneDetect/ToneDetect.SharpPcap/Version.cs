using System;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Represent the current version of SharpPcap.
	/// </summary>
	public class Version
	{
		/// <summary>
		/// Returns the current version string of the SharpPcap library
		/// </summary>
		/// <returns>the current version string of the SharpPcap library</returns>
		public static string GetVersionString()
		{
			System.Reflection.Assembly asm
				= System.Reflection.Assembly.GetAssembly(typeof(Version));
			return asm.GetName().Version.ToString();
		}
	}
}
