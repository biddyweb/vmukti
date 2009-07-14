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

namespace DirectX.Capture
{
	/// <summary>
	///  Represents a physical connector or source on an audio/video device.
	/// </summary>
	public class Source : IDisposable
	{

		// --------------------- Private/Internal properties -------------------------

		protected string				name;				// Name of the source



		// ----------------------- Public properties -------------------------

		/// <summary> The name of the source. Read-only. </summary>
		public string Name { get { return( name ); } }

		/// <summary> Obtains the String representation of this instance. </summary>
		public override string ToString() { return( Name ); }

		/// <summary> Is this source enabled. </summary>
		public virtual bool Enabled 
		{
			get { throw new NotSupportedException( "This method should be overriden in derrived classes." ); } 
			set { throw new NotSupportedException( "This method should be overriden in derrived classes." ); } 
		}

		
		// -------------------- Constructors/Destructors ----------------------

		/// <summary> Release unmanaged resources. </summary>
		~Source()
		{
			Dispose();
		}


		
		// -------------------- IDisposable -----------------------

		/// <summary> Release unmanaged resources. </summary>
		public virtual void Dispose()
		{
			name = null;
		}

	}
}
