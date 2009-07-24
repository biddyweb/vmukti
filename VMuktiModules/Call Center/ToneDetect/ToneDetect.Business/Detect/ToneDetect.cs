<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Detect/ToneDetect.cs
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
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Detect/ToneDetect.cs
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ToneDetect.Business.Fourier;

namespace ToneDetect.Business.Detect
{
	internal class ToneDetect
	{
		private int counter;

		protected string streamName;

		internal ToneDetect()
		{
			counter = 0;
		}

		/// <summary>
		/// Determine the maximum frequency given the set of frequency bins
		/// </summary>
		/// <param name="frequencyBins">set of bins to find max freq in</param>
		/// <returns>the frequency of the bin with the highest energy</returns>
		static internal double FindMaxFrequency( FrequencyBin[] frequencyBins )
		{
			double retval;
			double max;

			retval = 0.0;
			max = 0.0;
			for( int i = 0; i < frequencyBins.Length; ++i )
			{
				if( frequencyBins[i].Energy > max )
				{
					max = frequencyBins[i].Energy;
					retval = frequencyBins[i].Frequency;
				}
			}

			return retval;
		}
	}
}
