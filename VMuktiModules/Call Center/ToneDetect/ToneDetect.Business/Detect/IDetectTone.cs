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

using ToneDetect.Business.Fourier;

namespace ToneDetect.Business.Detect
{
	internal interface IDetectTone
	{
		ToneDetected Detect(
			short [] samples,
			Complex [] cdata,
			FrequencyBin [] averageFrequencyBins,
			FrequencyBin [] instantaneousFrequencyBins,
			int mS, // the number of mS since the beginning of the data stream
			double maxFrequency // the frequency where the maximum energy occurs from fft
			);
		void DumpResults();
		void Close();
	}
}
