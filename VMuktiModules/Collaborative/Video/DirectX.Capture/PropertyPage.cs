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
using System.Windows.Forms;

namespace DirectX.Capture
{
	/// <summary>
	///  A base class for representing property pages exposed by filters. 
	/// </summary>
	public class PropertyPage : IDisposable
	{

		// ---------------- Properties --------------------

		/// <summary> Name of property page. This name may not be unique </summary>
		public string Name;

		/// <summary> Does this property page support saving and loading the user's choices. </summary>
		public bool SupportsPersisting = false;

		/// <summary> 
		///  Get or set the state of the property page. This is used to save
		///  and restore the user's choices without redisplaying the property page. 
		/// </summary>
		/// <remarks>
		///  After showing this property page, read and store the value of 
		///  this property. At a later time, the user's choices can be 
		///  reloaded by setting this property with the value stored earlier. 
		///  Note that some property pages, after setting this property, 
		///  will not reflect the new state. However, the filter will use the
		///  new settings. 
		///  
		/// <para>
		///  When reading this property, copy the entire array at once then manipulate
		///  your local copy (e..g byte[] myState = propertyPage.State). When
		///  setting this property set the entire array at once (e.g. propertyPage = myState).
		/// </para>
		///  
		/// <para>
		///  Not all property pages support saving/loading state. Check the 
		///  <see cref="SupportsPersisting"/> property to determine if this 
		///  property page supports it.
		/// </para>
		/// </remarks>
		public virtual byte[] State
		{
			get { throw new NotSupportedException( "This property page does not support persisting state." ); }
			set { throw new NotSupportedException( "This property page does not support persisting state." ); }
		}



		// ---------------- Constructors --------------------

		/// <summary> Constructor </summary>
		public PropertyPage()
		{
		}



		// ---------------- Public Methods --------------------

		/// <summary> 
		///  Show the property page. Some property pages cannot be displayed 
		///  while previewing and/or capturing. This method will block until
		///  the property page is closed by the user.
		/// </summary>
		public virtual void Show(Control owner)
		{
			throw new NotSupportedException( "Not implemented. Use a derived class. " );
		}

		/// <summary> Release unmanaged resources </summary>
		public void Dispose()
		{
		}

	}
}
