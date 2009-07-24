<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Fourier/FrequencyBin.cs
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
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/Fourier/FrequencyBin.cs
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.Fourier
{
	/// <summary>
	/// The purpose of this class is essentially to provide a sortable container
	/// for the results of a frequency bin in the goertzel algorithm results during
	/// the processing of packets.
	/// </summary>
	internal class FrequencyBin : IComparable
	{
		#region Fields
		/// <summary>
		/// frequency specified by this 'bin'
		/// </summary>
		private double frequency;

		/// <summary>
		/// energy found in this bin.
		/// </summary>
		private double energy;

		#endregion

		#region Properties
		/// <summary>
		/// Getter/setter for the frequency
		/// </summary>
		internal double Frequency
		{
			get
			{
				return frequency;
			}
			set
			{
				frequency = value;
			}
		}

		/// <summary>
		/// getter/setter for the energy
		/// </summary>
		internal double Energy
		{
			get
			{
				return energy;
			}
			set
			{
				energy = value;
			}
		}
		#endregion

		#region Constructor
		internal FrequencyBin( double freq, double ene )
		{
			frequency = freq;
			energy = ene;
		}
		#endregion

		#region ICompariable Implementation
		public int CompareTo( object obj )
		{
			if( obj is FrequencyBin )
			{
				FrequencyBin bin;
				bin = (FrequencyBin)obj;
				return this.energy.CompareTo( bin.energy );
			}
			else
			{
				throw new ArgumentException( "Object is not a FrequencyBin." );
			}
		}
		#endregion
	}
}
