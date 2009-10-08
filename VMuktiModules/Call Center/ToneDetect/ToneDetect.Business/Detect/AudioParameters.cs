/* VMukti 2.0 -- An Open Source Unified Communications Engine
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;

*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.Detect
{
	/// <summary>
	/// the purpose of this class is to provide some system wide constants
	/// for the overall tone detection modules.
	/// </summary>
	public class AudioParameters
	{
		/// <summary>
		/// the sampling frequency of the audio stream
		/// </summary>
		public const int SamplingFrequency = 8000;

		/// <summary>
		/// the number of samples required to perform the FFT analysis
		/// </summary>
		public const int NumberSamples = 256;

		/// <summary>
		/// This is the number of hz represented by each frequency bin.
		/// It is gotten by 8000 / 256.
		/// </summary>
		public const double HertzPerBin = 31.25;

		/// <summary>
		/// This is the number of frequency bins that will be meaningful
		/// after the FFT.  It is the number of samples / 2.
		/// </summary>
		public const int BinsToAnayze = 128;
	}
}
