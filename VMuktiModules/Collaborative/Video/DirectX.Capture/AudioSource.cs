/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using System.Runtime.InteropServices; 
using DShowNET;

namespace DirectX.Capture
{
	/// <summary>
	///  Represents a physical connector or source on an 
	///  audio device. This class is used on filters that
	///  support the IAMAudioInputMixer interface such as 
	///  source cards.
	/// </summary>
	public class AudioSource : Source
	{

		// --------------------- Private/Internal properties -------------------------

		internal IPin		Pin;			// audio mixer interface (COM object)



		// -------------------- Constructors/Destructors ----------------------

		/// <summary> Constructor. This class cannot be created directly. </summary>
		internal AudioSource( IPin pin )
		{
			if ( (pin as IAMAudioInputMixer) == null )
				throw new NotSupportedException( "The input pin does not support the IAMAudioInputMixer interface" );
			this.Pin = pin;
			this.name = getName( pin );
		}



		// ----------------------- Public properties -------------------------

		/// <summary> Enable or disable this source. For audio sources it is 
		/// usually possible to enable several sources. When setting Enabled=true,
		/// set Enabled=false on all other audio sources. </summary>
		public override bool Enabled
		{
			get 
			{
				IAMAudioInputMixer mix = (IAMAudioInputMixer) Pin;
				bool e;
				mix.get_Enable( out e );
				return( e );
			}

			set
			{
				IAMAudioInputMixer mix = (IAMAudioInputMixer) Pin;
				mix.put_Enable( value );
			}

		}


				
		// --------------------------- Private methods ----------------------------

		/// <summary> Retrieve the friendly name of a connectorType. </summary>
		private string getName( IPin pin )
		{
			string s = "Unknown pin";
			PinInfo pinInfo = new PinInfo();

			// Direction matches, so add pin name to listbox
			int hr = pin.QueryPinInfo( out pinInfo);
			if ( hr == 0 )
			{ 
				s = pinInfo.name + "";
			}
			else
				Marshal.ThrowExceptionForHR( hr );

			// The pininfo structure contains a reference to an IBaseFilter,
			// so you must release its reference to prevent resource a leak.
			if ( pinInfo.filter != null )
				Marshal.ReleaseComObject( pinInfo.filter  );  pinInfo.filter  = null;

			return( s );
		}

		// -------------------- IDisposable -----------------------

		/// <summary> Release unmanaged resources. </summary>
		public override void Dispose()
		{
			if ( Pin != null )
				Marshal.ReleaseComObject( Pin );
			Pin = null;
			base.Dispose();
		}	
	}
}
