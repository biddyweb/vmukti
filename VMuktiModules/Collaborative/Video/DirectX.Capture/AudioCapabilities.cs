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
using System.Runtime.InteropServices;
using DShowNET;

namespace DirectX.Capture
{
	/// <summary>
	///  Capabilities of the audio device such as 
	///  min/max sampling rate and number of channels available.
	/// </summary>
	public class AudioCapabilities
	{

		// ------------------ Properties --------------------

		/// <summary> Minimum number of audio channels. </summary>
		public int MinimumChannels;

		/// <summary> Maximum number of audio channels. </summary>
		public int MaximumChannels;

		/// <summary> Granularity of the channels. For example, channels 2 through 4, in steps of 2. </summary>
		public int ChannelsGranularity;

		/// <summary> Minimum number of bits per sample. </summary>
		public int MinimumSampleSize;

		/// <summary> Maximum number of bits per sample. </summary>
		public int MaximumSampleSize;

		/// <summary> Granularity of the bits per sample. For example, 8 bits per sample through 32 bits per sample, in steps of 8. </summary>
		public int SampleSizeGranularity;

		/// <summary> Minimum sample frequency. </summary>
		public int MinimumSamplingRate;

		/// <summary> Maximum sample frequency. </summary>
		public int MaximumSamplingRate;

		/// <summary> Granularity of the frequency. For example, 11025 Hz to 44100 Hz, in steps of 11025 Hz. </summary>
		public int SamplingRateGranularity;



		// ----------------- Constructor ---------------------

		/// <summary> Retrieve capabilities of an audio device </summary>
		internal AudioCapabilities(IAMStreamConfig audioStreamConfig)
		{
			if ( audioStreamConfig == null ) 
				throw new ArgumentNullException( "audioStreamConfig" );

			AMMediaType mediaType = null;
			AudioStreamConfigCaps caps = null;
			IntPtr pCaps = IntPtr.Zero;
			IntPtr pMediaType;
			try
			{
				// Ensure this device reports capabilities
				int c, size;
				int hr = audioStreamConfig.GetNumberOfCapabilities( out c, out size );
				if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr );
				if ( c <= 0 ) 
					throw new NotSupportedException( "This audio device does not report capabilities." );
				if ( size > Marshal.SizeOf( typeof( AudioStreamConfigCaps ) ) )
					throw new NotSupportedException( "Unable to retrieve audio device capabilities. This audio device requires a larger AudioStreamConfigCaps structure." );
				if ( c > 1 )
					Debug.WriteLine("WARNING: This audio device supports " + c + " capability structures. Only the first structure will be used." );

				// Alloc memory for structure
				pCaps = Marshal.AllocCoTaskMem( Marshal.SizeOf( typeof( AudioStreamConfigCaps ) ) ); 

				// Retrieve first (and hopefully only) capabilities struct
				hr = audioStreamConfig.GetStreamCaps( 0, out pMediaType, pCaps );
				if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr );

				// Convert pointers to managed structures
				mediaType = (AMMediaType) Marshal.PtrToStructure( pMediaType, typeof( AMMediaType ) );
				caps = (AudioStreamConfigCaps) Marshal.PtrToStructure( pCaps, typeof( AudioStreamConfigCaps ) );

				// Extract info
				MinimumChannels						= caps.MinimumChannels;
				MaximumChannels						= caps.MaximumChannels;
				ChannelsGranularity					= caps.ChannelsGranularity;
				MinimumSampleSize					= caps.MinimumBitsPerSample;
				MaximumSampleSize					= caps.MaximumBitsPerSample;
				SampleSizeGranularity				= caps.BitsPerSampleGranularity;
				MinimumSamplingRate					= caps.MinimumSampleFrequency;
				MaximumSamplingRate					= caps.MaximumSampleFrequency;
				SamplingRateGranularity				= caps.SampleFrequencyGranularity;
				
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
