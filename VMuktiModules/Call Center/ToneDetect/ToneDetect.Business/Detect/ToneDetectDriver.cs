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

using ToneDetect.Business.Fourier;

namespace ToneDetect.Business.Detect
{
	/// <summary>
	/// The purpose of this class is to provide the crux of the tone detection algorithms.
	/// It essentially provides initialzation routines as well as hooks to run the algorithm
	/// such that tones may be known.
	/// </summary>
	internal class ToneDetectDriver
	{
		#region Fields
		/// <summary>
		/// the number of packets that have been received
		/// </summary>
		private int packetCounter;

		/// <summary>
		/// the number of mS since the beginning of the stream
		/// </summary>
		private int mS;

		/// <summary>
		/// list of the done detectors
		/// </summary>
		private List<IDetectTone> toneDetectors;

		/// <summary>
		/// a buffer representing a frame to throw at the FFT
		/// </summary>
		private FrameBuffer frameBuffer;

		/// <summary>
		/// these are the frequency bins for the overall signal
		/// </summary>
		private FrequencyBin [] averageFrequencyBins;

		/// <summary>
		/// these are the frequency bins for THIS run
		/// </summary>
		private FrequencyBin[] instantaneousFrequencyBins;
		#endregion

		internal ToneDetectDriver()
		{
			packetCounter = 0;
		}

		internal void Initialize()
		{
			mS = 0;

			toneDetectors = new List<IDetectTone>();
			toneDetectors.Add( new FaxTone() );
			toneDetectors.Add( new TriTone() );
			toneDetectors.Add( new VoiceMail() );

			//require 256 (a power of 2 for FFT).  Note, this yields bins of 31.5hz
			frameBuffer = new FrameBuffer( AudioParameters.NumberSamples );

			averageFrequencyBins = new FrequencyBin [AudioParameters.BinsToAnayze];
			instantaneousFrequencyBins = new FrequencyBin[AudioParameters.BinsToAnayze];
			for( int i = 0; i < AudioParameters.BinsToAnayze; ++i )
			{
				averageFrequencyBins[i] = new FrequencyBin( i * AudioParameters.HertzPerBin, 0 );
				instantaneousFrequencyBins[i] = new FrequencyBin( i * AudioParameters.HertzPerBin, 0 );
			}
		}

		/// <summary>
		/// perform debug output as necessary
		/// </summary>
		internal void DumpResults()
		{
			TextWriter textWriter;
			textWriter = new StreamWriter( @"c:\dev\bins.csv" );
			for( int i = 0; i < averageFrequencyBins.Length; ++i )
			{
				textWriter.WriteLine( String.Format( "{0},{1}",
				averageFrequencyBins[i].Frequency * 100,
				averageFrequencyBins[i].Energy / 100 ) );
			}

			foreach( IDetectTone iDetect in toneDetectors )
			{
				iDetect.DumpResults();
			}
		}

		internal void Close()
		{
			foreach( IDetectTone iDetect in toneDetectors )
			{
				iDetect.Close();
			}
		}

		/// <summary>
		/// here is the main entry point into the detection routines for each packet received from
		/// the stream
		/// </summary>
		/// <param name="samples">array of samples decoded from the RTP stream</param>
		/// <returns>ToneDetected enum represent what if any tone has been found</returns>
		internal ToneDetected Detect( short [] samples )
		{
			ToneDetected ret;
			short [] bufferedSamples;
			ret = ToneDetected.NONE;
			Complex [] cdata;
			double magnitude; // magnitude of the complex sample
			double maxFrequency; // the frequency at which the maximum energy was found by FFT

			// buffer the samples... returns true if the buffer is ready to process
			if( frameBuffer.AddBuffer( samples ) )
			{
				packetCounter++;
				bufferedSamples = frameBuffer.Samples;
				cdata = new Complex [bufferedSamples.Length];
				for( int z = 0; z < bufferedSamples.Length; ++z )
				{
					cdata[z] = (Complex)bufferedSamples[z];
				}

				Fourier.Fourier.FFT( cdata, cdata.Length, FourierDirection.Forward );
				for( int freq = 0; freq < averageFrequencyBins.Length; freq++ )
				{
					magnitude = Math.Sqrt( ( cdata[freq].Re * cdata[freq].Re ) +
									( cdata[freq].Im * cdata[freq].Im ) );
					averageFrequencyBins[freq].Energy =
						( averageFrequencyBins[freq].Energy + magnitude );
					instantaneousFrequencyBins[freq].Energy = magnitude;
				}

				maxFrequency = ToneDetect.FindMaxFrequency( instantaneousFrequencyBins );
				mS += bufferedSamples.Length / 8;

				foreach( IDetectTone iDetect in toneDetectors )
				{
					ret = iDetect.Detect(
						bufferedSamples,
						cdata,
						averageFrequencyBins,
						instantaneousFrequencyBins,
						mS,
						maxFrequency );
					if( ret != ToneDetected.NONE )
					{
						break;
					}
				}
			}
			return ret;
		}
	}
}
