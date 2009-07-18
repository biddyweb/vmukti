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
using ToneDetect.Business.FaxInfo;

namespace ToneDetect.Business.Detect
{
	internal class FaxTone : ToneDetect, IDetectTone
	{
		///This member would be used if we were attempting to receive calls which I'm fairly certain we will not be.
		//private ToneState cngState;

		private ToneState cedState;

		private ToneState othState;

		private TextWriter textWriter;

		internal FaxTone()
		{
			//cngState = new ToneState(); // we are not sending signals
			cedState = new ToneState();
			othState = new ToneState();

			//cngState.Initialize( FaxSpecifiecations.CNG_Freq, FaxSpecifiecations.CNG_Dur, FaxSpecifiecations.CNG_DB );
			cedState.Initialize( FaxSpecifiecations.CED_Freq, FaxSpecifiecations.CED_Dur, FaxSpecifiecations.CED_DB, 100 );
			othState.Initialize( FaxSpecifiecations.OTH_Freq, FaxSpecifiecations.OTH_Dur, FaxSpecifiecations.OTH_DB, 200 );

			//textWriter = new StreamWriter( @"c:\dev\fax.log" );
			textWriter = null;
		}

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
#if JUNK
			double max;
			int maxi;
			max = 0.0;
			maxi = 0;
			for( int i = 0; i < cedState.FrequencyBins.Length; ++i )
			{
				if( cedState.FrequencyBins [i].Energy > max && i < 128 )
				{
					max = cedState.FrequencyBins [i].Energy;
					maxi = i;
				}
				if( textWriter != null )
				{
					textWriter.WriteLine( String.Format( "{0},{1}",
					cedState.FrequencyBins [i].Frequency,
					cedState.FrequencyBins [i].Energy ) );
				}
			}

			if( textWriter != null )
			{
				textWriter.WriteLine( String.Format( "ced max = {0} i={1}", max, maxi * 31.25 ) );
				textWriter.WriteLine();
			}

			for( int i = 0; i < othState.FrequencyBins.Length; ++i )
			{
				if( othState.FrequencyBins [i].Energy > max && i < 128 )
				{
					max = othState.FrequencyBins [i].Energy;
					maxi = i;
				}
				if( textWriter != null )
				{
					textWriter.WriteLine( String.Format( "{0},{1}",
					othState.FrequencyBins [i].Frequency,
					othState.FrequencyBins [i].Energy ) );
				}
			}
			if( textWriter != null )
			{
				textWriter.WriteLine( String.Format( "oth max = {0} i={1}", max, maxi * 31.25 ) );
				textWriter.Flush();
			}
#endif
		}

		public ToneDetected Detect( short[] samples,
			Complex[] cdata,
			FrequencyBin[] averageFrequencyBins,
			FrequencyBin[] instantaneousFrequencyBins,
			int mS, // the number of mS since the beginning of the data stream
			double maxFrequency // the frequency where the maximum energy occurs from fft
 )
		{
			ToneDetected retval;
			retval = ToneDetected.NONE;

			if( cedState.IsFound( maxFrequency ) )
				{
					retval = ToneDetected.FAX;
				}
			else if( othState.IsFound( maxFrequency ) )
			{
				retval = ToneDetected.FAX;
			}

			return retval;
		}
		#endregion

	}
}
