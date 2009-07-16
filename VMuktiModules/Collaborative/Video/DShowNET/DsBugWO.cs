/*
*VMukti -- An open source video conferencing platform.
*
* Copyright (C) 2007 - 2008, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.
*
* This program is free software, distributed under the terms of
* the GNU General Public License Version 2. See the LICENSE file
* at the top of the source tree.
*/

using System;
using System.Runtime.InteropServices;

namespace DShowNET
{

public class DsBugWO
{
	/*
	works:
		CoCreateInstance( CLSID_CaptureGraphBuilder2, ..., IID_ICaptureGraphBuilder2, ...);
	doesn't (E_NOTIMPL):
		CoCreateInstance( CLSID_CaptureGraphBuilder2, ..., IID_IUnknown, ...);
	thus .NET 'Activator.CreateInstance' fails
	*/

	public static object CreateDsInstance( ref Guid clsid, ref Guid riid )
	{
		IntPtr ptrIf;
		int hr = CoCreateInstance( ref clsid, IntPtr.Zero, CLSCTX.Inproc, ref riid, out ptrIf );
		if( (hr != 0) || (ptrIf == IntPtr.Zero) )
			Marshal.ThrowExceptionForHR( hr );

		Guid iu = new Guid( "00000000-0000-0000-C000-000000000046" );
		IntPtr ptrXX;
		hr = Marshal.QueryInterface( ptrIf, ref iu, out ptrXX );

		object ooo = System.Runtime.Remoting.Services.EnterpriseServicesHelper.WrapIUnknownWithComObject( ptrIf );
		int ct = Marshal.Release( ptrIf );
		return ooo;
	}

	[DllImport("ole32.dll") ]
	private static extern int CoCreateInstance(	ref Guid clsid, IntPtr pUnkOuter, CLSCTX dwClsContext, ref Guid iid, out IntPtr ptrIf );
}

[Flags]
internal enum CLSCTX
	{
	Inproc					= 0x03,
	Server					= 0x15,
	All						= 0x17,
	}


} // namespace DShowNET
