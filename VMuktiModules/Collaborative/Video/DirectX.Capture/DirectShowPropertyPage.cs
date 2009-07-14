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
using System.Windows.Forms;
using DShowNET;

namespace DirectX.Capture
{
	/// <summary>
	///  Property pages for a DirectShow filter (e.g. hardware device). These
	///  property pages do not support persisting their settings. 
	/// </summary>
	public class DirectShowPropertyPage : PropertyPage
	{
		// ---------------- Properties --------------------

		/// <summary> COM ISpecifyPropertyPages interface </summary>
		protected ISpecifyPropertyPages specifyPropertyPages;



		// ---------------- Constructors --------------------

		/// <summary> Constructor </summary>
		public DirectShowPropertyPage(string name, ISpecifyPropertyPages specifyPropertyPages)
		{
			Name = name;
			SupportsPersisting = false;
			this.specifyPropertyPages = specifyPropertyPages;
		}



		// ---------------- Public Methods --------------------

		/// <summary> 
		///  Show the property page. Some property pages cannot be displayed 
		///  while previewing and/or capturing. 
		/// </summary>
		public override void Show(Control owner)
		{
			DsCAUUID cauuid = new DsCAUUID();
			try
			{
				int hr = specifyPropertyPages.GetPages( out cauuid );
				if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr );

				object o = specifyPropertyPages;
				hr = OleCreatePropertyFrame( owner.Handle, 30, 30, null, 1,
					ref o, cauuid.cElems, cauuid.pElems, 0, 0, IntPtr.Zero );
			}
			finally
			{
				if( cauuid.pElems != IntPtr.Zero )
					Marshal.FreeCoTaskMem( cauuid.pElems );
			}
		}

		/// <summary> Release unmanaged resources </summary>
		public new void Dispose()
		{
			if ( specifyPropertyPages != null )
				Marshal.ReleaseComObject( specifyPropertyPages ); specifyPropertyPages = null;
		}



		// ---------------- DLL Imports --------------------

		[DllImport("olepro32.dll", CharSet=CharSet.Unicode, ExactSpelling=true) ]
		private static extern int OleCreatePropertyFrame( 
			IntPtr hwndOwner, int x, int y,
			string lpszCaption, int cObjects,
			[In, MarshalAs(UnmanagedType.Interface)] ref object ppUnk,
			int cPages,	IntPtr pPageClsID, int lcid, int dwReserved, IntPtr pvReserved );


	}
}
