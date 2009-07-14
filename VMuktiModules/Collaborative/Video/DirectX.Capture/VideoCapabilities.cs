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
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using DShowNET;

namespace DirectX.Capture
{
	/// <summary>
	///  Capabilities of the video device such as 
	///  min/max frame size and frame rate.
	/// </summary>
	public class VideoCapabilities
	{

		// ------------------ Properties --------------------

		/// <summary> Native size of the incoming video signal. This is the largest signal the filter can digitize with every pixel remaining unique. Read-only. </summary>
		public Size InputSize;

		/// <summary> Minimum supported frame size. Read-only. </summary>
		public Size MinFrameSize;

		/// <summary> Maximum supported frame size. Read-only. </summary>
		public Size MaxFrameSize;

		/// <summary> Granularity of the output width. This value specifies the increments that are valid between MinFrameSize and MaxFrameSize. Read-only. </summary>
		public int FrameSizeGranularityX;

		/// <summary> Granularity of the output height. This value specifies the increments that are valid between MinFrameSize and MaxFrameSize. Read-only. </summary>
		public int FrameSizeGranularityY;

		/// <summary> Minimum supported frame rate. Read-only. </summary>
		public double MinFrameRate;

		/// <summary> Maximum supported frame rate. Read-only. </summary>
		public double MaxFrameRate;



		// ----------------- Constructor ---------------------

		/// <summary> Retrieve capabilities of a video device </summary>
		internal VideoCapabilities(IAMStreamConfig videoStreamConfig)
		{
			if ( videoStreamConfig == null ) 
				throw new ArgumentNullException( "videoStreamConfig" );

			AMMediaType mediaType = null;
			VideoStreamConfigCaps caps = null;
			IntPtr pCaps = IntPtr.Zero;
			IntPtr pMediaType;
			try
			{
				// Ensure this device reports capabilities
				int c, size;
				int hr = videoStreamConfig.GetNumberOfCapabilities( out c, out size );
				if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr );
				if ( c <= 0 ) 
					throw new NotSupportedException( "This video device does not report capabilities." );
				if ( size > Marshal.SizeOf( typeof( VideoStreamConfigCaps ) ) )
					throw new NotSupportedException( "Unable to retrieve video device capabilities. This video device requires a larger VideoStreamConfigCaps structure." );
				if ( c > 1 )
					Debug.WriteLine("This video device supports " + c + " capability structures. Only the first structure will be used." );

				// Alloc memory for structure
				pCaps = Marshal.AllocCoTaskMem( Marshal.SizeOf( typeof( VideoStreamConfigCaps ) ) ); 

				// Retrieve first (and hopefully only) capabilities struct
				hr = videoStreamConfig.GetStreamCaps( 0, out pMediaType, pCaps );
				if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr );

				// Convert pointers to managed structures
				mediaType = (AMMediaType) Marshal.PtrToStructure( pMediaType, typeof( AMMediaType ) );
				caps = (VideoStreamConfigCaps) Marshal.PtrToStructure( pCaps, typeof( VideoStreamConfigCaps ) );

				// Extract info
				InputSize = caps.InputSize;
				MinFrameSize = caps.MinOutputSize;
				MaxFrameSize = caps.MaxOutputSize;
				FrameSizeGranularityX = caps.OutputGranularityX;
				FrameSizeGranularityY = caps.OutputGranularityY;
				MinFrameRate = (double)10000000 / caps.MaxFrameInterval;
				MaxFrameRate = (double)10000000 / caps.MinFrameInterval;
			}
			finally
			{
				if ( pCaps != IntPtr.Zero )
					Marshal.FreeCoTaskMem( pCaps ); pCaps = IntPtr.Zero;
				if ( mediaType != null )
					DsUtils.FreeAMMediaType( mediaType ); mediaType = null;
			}
		}
	}
}
