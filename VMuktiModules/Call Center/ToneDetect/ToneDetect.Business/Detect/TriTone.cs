<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Detect/TriTone.cs
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
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Detect/TriTone.cs
using System.Collections.Generic;
using System.IO;
using System.Text;

using ToneDetect.Business.Fourier;

namespace ToneDetect.Business.Detect
{
	/// <summary>
	/// The purpose of this class is to provide the detection logic for the TriTone.
	/// Another name for the tritone is Special Information Tone (SIT).
	/// Information for this module was found at:
	/// http://www.ahk.com/Special%20Information%20Tones.pdf
	/// First freq range is 900Hz -> 1000Hz
	/// Second freq range is 1350Hz -> 1450Hz
	/// Third freq range is 1750Hz -> 1850Hz
	/// </summary>
	internal class TriTone : ToneDetect, IDetectTone
	{
		#region Private Declarations
		/// <summary>
		/// There are three states to the tritone.
		/// </summary>
		private enum TriToneState
		{
			WaitingForSignal,
			Tone1,
			Tone2,
			Tone3,
			NoTriTone,
			Done
		}

		/// <summary>
		/// the constants for the tritone
		/// </summary>
		private class TriToneConstants
		{
			//ranges for tone1
			internal const double startTone1 = 900.0;
			internal const double endTone1 = 1000.0;

			//ranges for tone2
			internal const double startTone2 = 1350.0;
			internal const double endTone2 = 1450.0;

			//ranges for tone3
			internal const double startTone3 = 1750.0;
			internal const double endTone3 = 1850.0;
		}
		#endregion

		#region Fields
		/// <summary>
		/// Indicates the state of this detector.
		/// </summary>
		private TriToneState toneState;

		/// <summary>
		/// writer to log data to
		/// </summary>
		private TextWriter textWriter;

		/// <summary>
		/// number of hits in each phase/state
		/// </summary>
		private int hitCount;

		/// <summary>
		/// indicates if the minimum hits for a state have been found.
		/// </summary>
		private bool minHitsFound;

		/// <summary>
		/// number of misses in each phase/state
		/// </summary>
		private int missCount;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor --- perform class level init.
		/// </summary>
		internal TriTone()
		{
			toneState = TriToneState.WaitingForSignal;

			//textWriter = new StreamWriter( @"c:\dev\tritone.log" );
			textWriter = null;
		}
		#endregion

		/// <summary>
		/// one stop shopping for logging and checking validity of the textWriter
		/// </summary>
		/// <param name="stringToLog">The string to output to the stream.</param>
		private void Log( string stringToLog )
		{
			if( textWriter != null )
			{
				textWriter.WriteLine( stringToLog );
				textWriter.Flush();
			}
		}

		#region IDetectTone Implementation
		public void Close()
		{
			if( textWriter != null )
			{
				textWriter.Close();
				textWriter = null;
			}
		}

		public void DumpResults()
		{
		}

		public ToneDetected Detect(
			short [] samples,
			Complex [] cdata,
			FrequencyBin [] averageFrequencyBins,
			FrequencyBin [] instantenousFrequencyBins,
			int mS, // the number of mS since the beginning of the data stream
			double maxFrequency // the frequency where the maximum energy occurs from fft
			)
		{
			ToneDetected retval;
			retval = ToneDetected.NONE;
			double energy;

			// in this method, we are assuming 20 % of the tone is enough to cause
			// a hit.  Then, 8 or 11 hits are required based on the duration of the
			// particular portion of the tone we are detecting based on the specification of
			// the SIT (Special Information Tone).
			// note, 8000hz so we are doing 8 samples per mS
			switch( toneState )
			{
				case TriToneState.WaitingForSignal:
					energy = 0;
					//for(int i = 0; i < 128; ++i)
					//{
					//	energy += cdata[i].Re;
					//}
					for( int i = 0; i < samples.Length; ++i )
					{
						energy += Math.Abs( samples[i] );
					}
					if(energy > 0)
					{
						// need to check the data we currently have for the
						// first part of the tri-tone signal.
						if( (maxFrequency >= TriToneConstants.startTone1) &&
							 (maxFrequency <= TriToneConstants.endTone1) )
						{
							Console.WriteLine( "Moving to state 1: {0}", mS );
						Log( "Moving to state1" );
							missCount = 0;
							toneState = TriToneState.Tone1;
							hitCount = 1;
						}
					}

					break;
				case TriToneState.Tone1:
					streamName = "TriTone1";
					if( ( maxFrequency >= TriToneConstants.startTone1 ) &&
						 ( maxFrequency <= TriToneConstants.endTone1 ) )
					{
						missCount = 0;
						hitCount++;
						Log( string.Format( "Maxfreq={0} hits={1}",
							maxFrequency, hitCount ) );
						if( hitCount == 7 )
						{
							//Console.WriteLine( "Moving to state 2: {0}", mS );
							//toneState = TriToneState.Tone2;
							//Log( "Moving to state2" );
							//hitCount = 0;
							//missCount = 0;
							minHitsFound = true;
							missCount = 0;
						}
					}
					else
					{
						if( ( maxFrequency >= TriToneConstants.startTone2 ) &&
							 ( maxFrequency <= TriToneConstants.endTone2 ) )
						{
							if( minHitsFound )
							{
								Console.WriteLine( "Moving to state 2: {0}", mS );
							Log( "Moving to state2" );
								toneState = TriToneState.Tone2;
								hitCount = 1;
								missCount = 0;
								minHitsFound = false;
							}
							//else
							//{
							//   if( missCount < 1 )
							//   {
							//      missCount++;
							//   }
							//}
						}
						else
						{
							if( missCount < 1 )
							{
								missCount++;
							}
							else
							{
							hitCount = 0;
						}
					}
					}
					break;
				case TriToneState.Tone2:
					streamName = "TriTone2";
					if( ( maxFrequency >= TriToneConstants.startTone2 ) &&
						 ( maxFrequency <= TriToneConstants.endTone2 ) )
					{
						missCount = 0;
						hitCount++;
						Log( string.Format( "Maxfreq={0} hits={1}",
							maxFrequency, hitCount ) );
						if( hitCount == 7 )
						{
							//toneState = TriToneState.Tone3;
							//Console.WriteLine( "Moving to state 3: {0}", mS );
							//Log( "Moving to state3" );
							//hitCount = 0;
							//missCount = 0;

							minHitsFound = true;
							missCount = 0;

						}
					}
					else
					{
						if( ( maxFrequency >= TriToneConstants.startTone3 ) &&
							 ( maxFrequency <= TriToneConstants.endTone3 ) )
						{
							if( minHitsFound )
							{
								Console.WriteLine( "Moving to state 3: {0}", mS );
							Log( "Moving to state3" );
								toneState = TriToneState.Tone3;
								hitCount = 1;
								missCount = 0;
								minHitsFound = false;
							}
							//else
							//{
							//   toneState = TriToneState.NoTriTone;
							//}
						}
						else
						{
							if( missCount < 1 )
							{
								missCount++;
							}
							else
							{
							hitCount = 0;
						}
					}
					}
					break;
				case TriToneState.Tone3:
					streamName = "TriTone3";
					if( ( maxFrequency >= TriToneConstants.startTone3 ) &&
						 ( maxFrequency <= TriToneConstants.endTone3 ) )
					{
						missCount = 0;
						hitCount++;
						Log( string.Format( "Maxfreq={0} hits={1}",
							maxFrequency, hitCount ) );
						if( hitCount == 7 )
						{
							Console.WriteLine( "Found Tri: {0}", mS );
							retval = ToneDetected.TRITONE;
							Log( "tone found" );
							hitCount = 0;
						}
					}
					else
					{
						if( missCount < 1 )
						{
							missCount++;
						}
						else
						{
							toneState = TriToneState.NoTriTone;
						}
					}
					break;
				// this case indicates there definately is not tritone in the signal.
				case TriToneState.NoTriTone:
					break;
				case TriToneState.Done:
					break;
				default:
					break;
			}

			return retval;
		}
		#endregion
	}
}
