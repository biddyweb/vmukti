/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists      
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
 
*/
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
