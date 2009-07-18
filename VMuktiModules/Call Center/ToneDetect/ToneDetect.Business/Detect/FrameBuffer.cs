/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.Detect
{
	/// <summary>
	/// The purpose of this class is to provide buffering capability of the data stream
	/// to include the number of bytes required to perform a FFT on a "Frame".
	/// The calling semantics here are a little bit tricky.
	/// 
	/// First, call the constructor, then do AddBuffer.  As soon as AddBuffer returns
	/// true, call the property Samples to get the samples.  At this time, go back to calling
	/// AddBuffer.  The call to Samples will be valid only one time.
	/// </summary>
	internal class FrameBuffer
	{
		#region Fields
		/// <summary>
		/// the actual buffer.
		/// </summary>
		private List<short> frame;

		/// <summary>
		/// temporary storage until the frame is copied out to the client.
		/// </summary>
		private List<short> tempFrame;

		/// <summary>
		/// how large should the frame be?
		/// </summary>
		private int frameSize;

		#endregion

		#region Properties
		internal short [] Samples
		{
			get
			{
				short [] samples;
				samples = new short[frame.Count];
				frame.CopyTo( samples );
				frame = tempFrame;
				return samples;
			}
		}
		#endregion

		#region Constructor
		internal FrameBuffer( int size )
		{
			frameSize = size;
			frame = new List<short>( frameSize );
		}
		#endregion

		#region Functions
		/// <summary>
		/// The purpose of this method is to add a set of samples to the frame.
		/// If the frame is filled, the method returns true.
		/// </summary>
		/// <param name="samples">set of samples to add to the frame.</param>
		/// <returns>true if the frame is full...call Samples property immediately afterward.</returns>
		internal bool AddBuffer( short [] samples )
		{
			bool retval;
			int lastIndex;
			retval = false;
			int i;

			if( samples.Length + frame.Count > frameSize )
			{
				//lastIndex = samples.Length + frame.Count - frameSize + 1;
				lastIndex = frameSize - frame.Count;
				retval = true; // we know we're going to fill up the buffer.
			}
			else
			{
				lastIndex = samples.Length;
			}

			for( i = 0; i < lastIndex; ++i )
			{
				frame.Add( samples [i] );
			}

			if( retval )
			{
				tempFrame = new List<short>();
				for( ; i < samples.Length; ++i )
				{
					tempFrame.Add( samples [i] );
				}
			}

			return retval;
		}
		#endregion

	}
}
