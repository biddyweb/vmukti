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
