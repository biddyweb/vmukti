using System;
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.RTP
{
	public class Endian
	{
		/// <summary>
		/// Converts the endian of an unsigned int.
		/// <summary>
		/// <param name="source">The unsigned int to convert.</param>
		/// <returns>The converted unsigned int.</returns>
		public static uint ConvertEndian( uint source )
		{
				return (uint)( source >> 24 ) |
								 ( ( source << 8 ) & 0x00FF0000 ) |
								 ( ( source >> 8 ) & 0x0000FF00 ) |
								  ( source << 24 );
		}

		/// <summary>
		/// Converts the endian of an unsigned short.
		/// <summary>
		/// <param name="source">The unsigned short to convert.</param>
		/// <returns>The converted unsigned short.</returns>
		public static ushort ConvertEndian( ushort source )
		{
			return (ushort)( source >> 8 | source << 8 );
		}
	}
}
