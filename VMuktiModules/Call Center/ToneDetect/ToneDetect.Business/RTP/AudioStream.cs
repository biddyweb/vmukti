using System;
using System.Collections.Generic;
using System.Text;

using ToneDetect.Business.Detect;

namespace ToneDetect.Business.RTP
{
	/// <summary>
	/// The purpose of this structure is to represent a state in the
	/// noise on a time unit on the domain and true/false no the range.
	/// </summary>
	public class NoiseChange
	{
		/// <summary>
		/// mS into the start of stream where the change occured.
		/// </summary>
		public ulong msOfChange;

		/// <summary>
		/// true means noise false means silence.
		/// </summary>
		public int state;
	}

	/// <summary>
	/// The purpose of this class is to provide a repository of all packets
	/// contained in a RTP stream.  The information stored is the decoded audio
	/// stream from the RTP.
	/// </summary>
	public class AudioStream
	{
		#region Fields
		private ToneDetectDriver detector;

		/// <summary>
		/// Collection of decoded packets.
		/// </summary>
		private List<short []> decodedPackets;

		#endregion

		#region Properties
		/// <summary>
		/// The list of decoded packets.  An array of arrays of shorts.
		/// </summary>
		public List<short []> DecodedPackets
		{
			get
			{
				return decodedPackets;
			}
		}
		#endregion

		#region Construction / Initialization
		/// <summary>
		/// constructor --- provide instance initialization.
		/// </summary>
		public AudioStream()
		{
			Initialize();
		}

		/// <summary>
		/// Provide initialization
		/// </summary>
		private void Initialize()
		{
			decodedPackets = new List<short[]>();
			detector = new ToneDetectDriver();
			detector.Initialize();
		}
		#endregion

		#region Public Interface
		/// <summary>
		/// Just a debug method.
		/// </summary>
		public void DumpResults()
		{
			detector.DumpResults();
		}

		public void Close()
		{
			detector.Close();
		}

		/// <summary>
		/// Add more samples to the overall collection.
		/// </summary>
		/// <param name="decoded">array of decoded samples</param>
		/// <returns>ToneDetected enum noting what if any tone has been detected on this audiostream.</returns>
		public ToneDetected Add( short [] decoded )
		{
			ToneDetected retval;

			retval = detector.Detect( decoded );
			decodedPackets.Add( decoded );

			return retval;
		}
		#endregion
	}
}
