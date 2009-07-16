using System;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Summary description for PcapException.
	/// </summary>
	public class PcapException : Exception
	{
		public PcapException()
		{
		}

		public PcapException(string msg):base(msg)
		{
		}
	}
}
