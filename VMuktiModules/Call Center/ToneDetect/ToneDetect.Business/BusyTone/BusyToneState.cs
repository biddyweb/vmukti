<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/BusyTone/BusyToneState.cs
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
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/BusyTone/BusyToneState.cs
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
