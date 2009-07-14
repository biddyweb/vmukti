using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ToneDetect.Business.Fourier;

namespace ToneDetect.Business.Detect
{
	/// <summary>
	/// The purpose of this class is to provide the detection logic for the voice mail.
	/// </summary>
	internal class VoiceMail : ToneDetect, IDetectTone
	{
		/// <summary>
		/// the constants for the beep tone
		/// </summary>
		private class BeepConstants
		{
			//ranges for the frequency
			internal const double startTone = 900.0;
			internal const double endTone = 1100.0;
		}

		#region Fields
		/// <summary>
		/// the instance of the silence detector...the thing with the brains
		/// </summary>
		SilenceDetectorStream sds;

		/// <summary>
		/// number of hits found for the "beep" frequency which we have found is 1000Hz
		/// </summary>
		private int hitCount;

		/// <summary>
		/// number of misses found once the hits start
		/// </summary>
		private int missCount;
		#endregion

		#region Constructor
		internal VoiceMail()
		{
			sds = new SilenceDetectorStream();
			hitCount = 0;
			missCount = 0;

		}
		#endregion

		#region IDetectTone Members

		ToneDetected IDetectTone.Detect(
			short[] samples,
			Complex [] cdata,
			FrequencyBin [] averageFrequencyBins,
			FrequencyBin [] instantanousFrequencyBins,
			int mS, // the number of mS since the beginning of the data stream
			double maxFrequency // the frequency where the maximum energy occurs from fft
			)
		{
			ToneDetected tone;
			tone = ToneDetected.NONE;
			sds.AddSamples( samples );
			if( sds.FoundStart )
			{
				// first, check to see if the frequency is in the range of
				// the beep freq
				if( ( maxFrequency >= BeepConstants.startTone ) &&
					 ( maxFrequency <= BeepConstants.endTone ) )
				{
					missCount = 0;
					hitCount++;
					if( hitCount == 14 )
					{
						tone = ToneDetected.RECORDING;
					}
				}
				else
				{
					// missed the signal -- forgive 1 miss
					if( missCount < 1 )
					{
						missCount++;
					}
					else
					{
						hitCount = 0;
					}

					// do the checking based on the transitions per second algorithm.
					if( sds.TotalDurationInSeconds > 3.0 && sds.CallDurationInSeconds > 2.0 )
					{
						if( sds.CallDurationInSeconds < 5.0 )
						{
							if( sds.TransitionsPerSecond > 4.0 )
							{
								tone = ToneDetected.RECORDING;
							}
						}
						else if( sds.CallDurationInSeconds > 5.0 )
						{
							if( sds.TransitionsPerSecond > 3.0 )
							{
								tone = ToneDetected.RECORDING;
							}
						}
					}
				}
			}
			return tone;
		}

		void IDetectTone.DumpResults()
		{
		}

		void IDetectTone.Close()
		{
		}
		#endregion
	}
}
