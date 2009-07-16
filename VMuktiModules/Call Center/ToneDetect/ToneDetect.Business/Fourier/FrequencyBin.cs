using System;
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
