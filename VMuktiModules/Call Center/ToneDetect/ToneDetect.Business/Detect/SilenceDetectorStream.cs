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
using System.Text;
using System.IO;

namespace ToneDetect.Business.Detect
{
	/// <summary>
	/// The purpose of this class is to be able to detect the number of transitions
	/// from noise to quiet or quiet to noise per second.  It does so by keeping track
	/// of where the noise signal has been found in the overall data stream and can
	/// be called with portions of the stream.
	/// </summary> 
	public class SilenceDetectorStream
	{
		#region Fields
		/// <summary>
		/// This is the minimum amplitude that will be considered
		/// to be noise.
		/// </summary>
		private static int CutoffAmplitude = 75;

		/// <summary>
		/// the number of ms of "silence" to ignore before
		/// actually calling the singal quiet
		/// </summary>
		private static int MaxRange = 50;

		/// <summary>
		/// the offset from the beginning of the call where the end of a noise was
		/// last found.  Units are samples.
		/// </summary>
		private int endNoise;

		/// <summary>
		/// the offset from the beginning of the call where the start of a noise block was
		/// last found.  Units are samples.
		/// </summary>
		private int startNoise;

		/// <summary>
		/// The offset from the beginning of the call where the very first noise was heard.
		/// Units are samples.
		/// </summary>
		private int firstNoise;

		/// <summary>
		/// the call duration in number of samples
		/// </summary>
		private int totalDuration;

		/// <summary>
		/// where the call starts in number of samples
		/// </summary>
		private int startOfCall;

		/// <summary>
		/// if the call is currently "in the noise"
		/// </summary>
		private bool inNoise;

		/// <summary>
		/// the last time quiet was found
		/// </summary>
		private int lastQuiet;

		/// <summary>
		/// the number of noise/quiet transitions per second.
		/// </summary>
		private double transitionsPerSecond;

		/// <summary>
		/// mili seconds per sample based on the sampling frequency.
		/// </summary>
		private double msPerSample;

		/// <summary>
		/// the total number of noise/quiet transitions.
		/// </summary>
		private int totalTransitions;

		/// <summary>
		/// true for the very first packet.
		/// </summary>
		private bool firstPacket;

		/// <summary>
		/// The number of samples found after a transition.  So, the first sample
		/// found from low to high will count for 1.  If the next sample is still high
		/// then the count goes to 2.  This variable is used to count the number of samples
		/// past the very first transition.  When only 1 is found, this sample is thrown out
		/// as just noise or not enough of a trend towards the noise or quiet state.
		/// </summary>
		private int transitionSamples;

		/// <summary>
		/// true once the start of the call has been found.
		/// </summary>
		private bool foundStart;

		/// <summary>
		/// a flag to indicate two samples in a row that are outside the range we are looking for.
		/// </summary>
		private bool strike;

		/// <summary>
		/// Get the number of transitions per second.
		/// </summary>
		public double TransitionsPerSecond
		{
			get
			{
				return transitionsPerSecond;
			}
		}

		/// <summary>
		/// Total number of transitions found by the software.
		/// </summary>
		public int TotalTransitions
		{
			get
			{
				return totalTransitions;
			}
		}

		/// <summary>
		/// The total number of seconds of the recorded data.
		/// </summary>
		public double TotalDurationInSeconds
		{
			get
			{
				return ( ( totalDuration * msPerSample ) / 1000.0 );
			}
		}

		/// <summary>
		/// The detected call duration in seconds.
		/// </summary>
		public double CallDurationInSeconds
		{
			get
			{
				return ( ( ( totalDuration - startOfCall ) * msPerSample ) / 1000.0 );
			}
		}

		/// <summary>
		/// The detected start of the call (after pickup) in seconds.
		/// </summary>
		public double StartOfCallInSeconds
		{
			get
			{
				return ( startOfCall * msPerSample ) / 1000.0;
			}
		}

		/// <summary>
		/// return the boolean fact that a call has been started
		/// or not.
		/// </summary>
		public bool FoundStart
		{
			get
			{
				bool retval;
				retval = false;
				if( startOfCall > 0 )
				{
					retval = true;
				}
				return retval;
			}
		}
		#endregion

		private FileStream stream;
		private StreamWriter streamWriter;
		private FileStream stream1;
		private StreamWriter streamWriter1;

		/// <summary>
		/// This constructor is to be used in debug situations.
		/// </summary>
		/// <param name="fileName">the name of the file to create for the noise/quiet wave</param>
		/// <param name="absFile">the name of the file to create for the absolute value of the data stream</param>
		public SilenceDetectorStream( string fileName, string absFile )
		{
			Initialize();
			InitializeDebug( fileName, absFile );
		}

		/// <summary>
		/// This construcor is to be used in operational situations.  Note, the
		/// debuging streams are set to null.
		/// </summary>
		public SilenceDetectorStream()
		{
			stream = null;
			stream1 = null;
			streamWriter = null;
			streamWriter1 = null;

			Initialize();
		}

		/// <summary>
		/// Initialize the debug streams.
		/// </summary>
		/// <param name="fileName">the name of the file to create for the noise/quiet wave</param>
		/// <param name="absFile">the name of the file to create for the absolute value of the data stream</param>
		private void InitializeDebug( string fileName, string absFile )
		{
			//for debug for now
			stream = File.Create( fileName );
			streamWriter = new StreamWriter( stream );
			stream1 = File.Create( absFile );
			streamWriter1 = new StreamWriter( stream1 );
		}

		/// <summary>
		/// The purpose of this method is to provide some class level initialization
		/// and some defaults.
		/// </summary>
		private void Initialize()
		{
			firstNoise = -1;
			endNoise = -1;
			startNoise = -1;
			startOfCall = -1;
			totalDuration = 0;
			inNoise = false;
			foundStart = false;
			totalTransitions = 0;
			lastQuiet = 0;
			firstPacket = true;
			transitionSamples = 0;
			strike = false;
			msPerSample = 1000.0 / AudioParameters.SamplingFrequency;
		}

		/// <summary>
		/// insert a set of samples into the picture.
		/// </summary>
		/// <param name="samples">array of decoded sound samples</param>
		public void AddSamples( short[] samples )
		{
			int diff;
			double diffMs;
			int i;
			short[] absSamples;

			// first, calculate the absolute value of all samples.
			absSamples = new short[samples.Length];
			for( i = 0; i < samples.Length; ++i )
			{
				absSamples[i] = Math.Abs( samples[i] );
				if( streamWriter1 != null )
				{
					streamWriter1.WriteLine( "{0},{1}", i + totalDuration, absSamples[i] );
					streamWriter1.Flush();
				}
			}

			// on the very first packet, check for noise.
			if( firstPacket )
			{
				firstPacket = false;
				if( absSamples[0] > SilenceDetectorStream.CutoffAmplitude )
				{
					firstNoise = 0;
					inNoise = true;
					if( streamWriter != null )
					{
						streamWriter.WriteLine( "{0},0", firstNoise );
						streamWriter.WriteLine( "{0},{1}", firstNoise, CutoffAmplitude );
					}
				}
			}

			for( i = 0; i < absSamples.Length; ++i )
			{
				if( !foundStart )
				{
					if( inNoise )
					{
						if( ( absSamples[i] < SilenceDetectorStream.CutoffAmplitude ) )
						{
							strike = false;
							transitionSamples++;
							if( transitionSamples > SilenceDetectorStream.MaxRange )
							{
								transitionSamples = 0;
								endNoise = i + totalDuration - SilenceDetectorStream.MaxRange;
								inNoise = false;

								if( streamWriter != null )
								{
									streamWriter.WriteLine( "{0},{1}", endNoise, CutoffAmplitude );
									streamWriter.WriteLine( "{0},0", endNoise );
								}

								if( startNoise > 0 )
								{
									diff = endNoise - startNoise;
									diffMs = diff * this.msPerSample;
									if( ( diffMs > 2200.0 ) || ( diffMs < 1800.0 ) )
									{
										foundStart = true;
										startOfCall = endNoise;
									}
								}
							}
						}
						else
						{
							if( strike )
							{
								transitionSamples = 0;
							}
							strike = true;
						}
					}
					else // in quiet
					{
						if( ( absSamples[i] >= SilenceDetectorStream.CutoffAmplitude ) )
						{
							strike = false;
							transitionSamples++;
							if( transitionSamples > SilenceDetectorStream.MaxRange )
							{
								transitionSamples = 0;
								startNoise = i + totalDuration - SilenceDetectorStream.MaxRange;
								inNoise = true;

								if( streamWriter != null )
								{
									streamWriter.WriteLine( "{0},0", startNoise );
									streamWriter.WriteLine( "{0},{1}", startNoise, CutoffAmplitude );
								}

								if( endNoise > 0 )
								{
									diff = startNoise - endNoise;
									diffMs = diff * this.msPerSample;
									if( ( diffMs > 4200.0 ) || ( diffMs < 3800.0 ) )
									{
										foundStart = true;
										startOfCall = startNoise;
									}
								}
							}
						}
						else
						{
							if( strike )
							{
								transitionSamples = 0;
							}
							strike = true;
						}
					}
				}
				else
				{
					// in a noise block
					if( inNoise )
					{
						// yes, has it transitioned to quiet?
						if( ( absSamples[i] < SilenceDetectorStream.CutoffAmplitude ) )
						{
							strike = false;
							transitionSamples++;
							if( transitionSamples > SilenceDetectorStream.MaxRange )
							{
								transitionSamples = 0;
								endNoise = i + totalDuration - SilenceDetectorStream.MaxRange;
								inNoise = false;
								totalTransitions++;
								if( streamWriter != null )
								{
									streamWriter.WriteLine( "{0},{1}", endNoise, CutoffAmplitude );
									streamWriter.WriteLine( "{0},0", endNoise );
								}
							}
						}
						else
						{
							if( strike )
							{
								transitionSamples = 0;
							}
							strike = true;
						}
					}
					else
					{
						// quiet block...has transitioned to noise?
						//if( (absSamples[i] > SilenceDetectorStream.CutoffAmplitude) &&
						//						( (i+totalDuration) > (lastQuiet + SilenceDetectorStream.MaxRange) ) )
						if( ( absSamples[i] > SilenceDetectorStream.CutoffAmplitude ) )
						{
							strike = false;
							transitionSamples++;
							if( transitionSamples > SilenceDetectorStream.MaxRange )
							{
								transitionSamples = 0;
								startNoise = i + totalDuration - SilenceDetectorStream.MaxRange;
								inNoise = true;
								totalTransitions++;
								if( streamWriter != null )
								{
									streamWriter.WriteLine( "{0},0", startNoise );
									streamWriter.WriteLine( "{0},{1}", startNoise, CutoffAmplitude );
								}
							}
						}
						else
						{
							if( strike )
							{
								transitionSamples = 0;
							}
							strike = true;
						}
					}
				}
			} // for loop

			totalDuration += i;

			transitionsPerSecond = totalTransitions / ( ( ( totalDuration - startOfCall ) * msPerSample ) / 1000.0 );

			if( streamWriter != null )
			{
				streamWriter.Flush();
			}
		}
	}
}
