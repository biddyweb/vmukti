<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Detect/ToneState.cs
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
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
=======
﻿using System;
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Detect/ToneState.cs
using System.Collections.Generic;
using System.Text;

using ToneDetect.Business.RTP;

namespace ToneDetect.Business.Detect
{
	internal class ToneState
	{
		#region Fields
		private double frequency;
		private double minFrequency;
		private double maxFrequency;
		private int blockSize;
		private int hitsRequired;
		private int hitCount;
		private double threshold;
		private double energy;
		private int samplesPending;
		private int missesAllowed;
		private int missCount;
		private int freqRange;
		/// <summary>
		/// this indicates which frequency bin to begin scanning in.
		/// </summary>
		#endregion

		#region Properties
		internal int MissCount
		{
			get
			{
				return missCount;
			}
			set
			{
				missCount = value;
				// check to see if we need to reset the hit count.
				if( missCount > missesAllowed )
				{
					hitCount = 0;
					missCount = 0;
				}
			}
		}
		internal double Frequency
		{
			get
			{
				return frequency;
			}
			set
			{
				frequency = value;
			}
		}

		internal int BlockSize
		{
			get
			{
				return blockSize;
			}
			set
			{
				blockSize = value;
			}
		}
		internal int HitsRequired
		{
			get
			{
				return hitsRequired;
			}
			set
			{
				hitsRequired = value;
			}
		}
		internal int HitCount
		{
			get
			{
				return hitCount;
			}
			set
			{
				hitCount = value;
			}
		}
		internal double Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				threshold = value;
			}
		}
		internal double Energy
		{
			get
			{
				return energy;
			}
			set
			{
				energy = value;
			}
		}
		internal int SamplesPending
		{
			get
			{
				return samplesPending;
			}
			set
			{
				samplesPending = value;
			}
		}

		internal int FreqRange
		{
			get
			{
				return freqRange;
			}
			set
			{
				freqRange = value;
			}
		}
		#endregion

		#region Constructor / Init
		internal ToneState()
		{
			frequency = 0;
			blockSize = 0;
			hitsRequired = 0;
			threshold = 0.0;
			samplesPending = 0;
			hitCount = 0;
			energy = 0;
			freqRange = 0;
		}

		/// <summary>
		/// The purpose of this method is to initialize the requirements of the
		/// tone state based on the frequency/duration passed in.
		/// </summary>
		/// <param name="freq">The frequency to target</param>
		/// <param name="duration">duration in ms to examine</param>
		/// <param name="amp">how much energy?</param>
		internal void Initialize( double freq, int duration, int amp, int frequencyRange )
		{
			int durationSamples;
			double x;
			int periodsInBlock;

			freqRange = frequencyRange;
			missesAllowed = 3;
			missCount = 0;

			frequency = freq;
			minFrequency = frequency - frequencyRange;
			maxFrequency = frequency + frequencyRange;
			durationSamples = duration * (int)RTPEnvironment.SAMPLE_RATE / 1000;
			// 10% deviation in tone duration.
			durationSamples = durationSamples * 9 / 10;
			
			// begin with the nominal number of samples in a frame.
			blockSize = (int)RTPEnvironment.SAMPLES_IN_FRAME;

			periodsInBlock = (int)(blockSize * frequency / (double)RTPEnvironment.SAMPLE_RATE);

			if( periodsInBlock < 5 )
			{
				periodsInBlock = 5;
			}

			blockSize = (int)(periodsInBlock * (int)RTPEnvironment.SAMPLE_RATE / freq);
			hitsRequired = ( durationSamples / AudioParameters.NumberSamples );

			samplesPending = blockSize;
			hitCount = 0;
			energy = 0.0;

			x = Math.Pow( 10.0, amp / 10.0 );
			threshold = x / ( x + 1 );
		}

		internal bool IsFound( double signalFreq )
		{
			bool retval;

			retval = false;
			if( signalFreq >= this.minFrequency &&
				 signalFreq <= this.maxFrequency )
			{
				hitCount++;
				missCount = 0;
				if( hitCount == hitsRequired )
				{
					retval = true;
				}
			}
			else
			{
				if( missCount > 0 )
				{
					hitCount = 0;
				}
				missCount++;
			}
			return retval;
		}
		#endregion
	}
}
