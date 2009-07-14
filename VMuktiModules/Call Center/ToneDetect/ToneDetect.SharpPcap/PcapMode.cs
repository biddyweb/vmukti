using System;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// The working mode of a Pcap device
	/// </summary>
	public enum PcapMode
	{
		/// <summary>
		/// Set a Pcap device to Capture mode (MODE_CAPT)
		/// </summary>
		Capture,

		/// <summary>
		/// Set a Pcap device to Statistics mode (MODE_STAT)
		/// </summary>
		Statistics
	};
}
