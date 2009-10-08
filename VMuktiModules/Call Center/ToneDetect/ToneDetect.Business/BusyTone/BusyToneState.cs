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

namespace ToneDetect.Business.BusyTone
{
	internal enum BusyDetectDef
	{
		BUSY_PERCENT = 10,	 // the percentage difference between the two last silence periods */
		BUSY_PAT_PERCENT = 7,	// the percentage difference between measured and actual pattern */
		BUSY_THRESHOLD = 100,	// Max number of ms difference between max and min times in busy */
		BUSY_MIN = 75,				// Busy must be at least 75 ms in half-cadence */
		BUSY_MAX = 3100			// Busy can't be more than 3100 ms in half-cadence */
	}

	internal class BusyToneState
	{
		internal const int DEFAULT_THRESHOLD = 512;
		internal const int DSP_HISTORY = 15;	// Remember last 50 units

		#region Constructor
		BusyToneState()
		{
			Initialize();
		}

		public void Initialize()
		{
			int i;
			HistoricSilence = new int [DSP_HISTORY];
			HistoricNoise = new int [DSP_HISTORY];

			for( i = 0; i < DSP_HISTORY; ++i )
			{
				HistoricNoise [i] = 0;
				HistoricSilence [i] = 0;
			}
			BusyCount = DSP_HISTORY;
			Threshold = DEFAULT_THRESHOLD;
		}
		#endregion

		#region Properties
		internal int DspHistory
		{
			get
			{
				return DSP_HISTORY;
			}
		}
		internal float Threshold
		{
			get;
			set;
		}
		internal int TotalSilence
		{
			get;
			set;
		}
		internal int TotalNoise
		{
			get;
			set;
		}

		internal int [] HistoricNoise
		{
			get;
			set;
		}

		internal int [] HistoricSilence
		{
			get;
			set;
		}

		internal int BusyToneLength
		{
			get;
			set;
		}
		internal int BusyQuietLength
		{
			get;
			set;
		}
		internal int BusyMaybe
		{
			get;
			set;
		}
		internal int BusyCount
		{
			get;
			set;
		}
		#endregion
	}
}
