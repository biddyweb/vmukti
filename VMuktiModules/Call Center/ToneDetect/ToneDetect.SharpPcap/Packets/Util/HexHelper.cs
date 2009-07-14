// $Id: HexHelper.java,v 1.3 2001/06/04 05:07:06 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets.Util
{
	using System;
	/// <summary> Functions for formatting and printing binary data in hexadecimal.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.3 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/04 05:07:06 $
	/// 
	/// </version>
	public class HexHelper
	{
		/// <summary> Convert an int (32 bits in Java) to a decimal quad of the form
		/// aaa.bbb.ccc.ddd.
		/// </summary>
		public static System.String toQuadString(int i)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int p = 0; p < 4; p++)
			{
				int q = (int) (i & 0xff);
				sb.Append(q);
				if (p < 3)
					sb.Append('.');
				i >>= 8;
			}
			
			return sb.ToString();
		}
		
		/// <summary> Convert an int to a hexadecimal string.
		/// </summary>
		public static System.String toString(int i)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int p = 0; p < 8; p++)
			{
				byte b = (byte) (i & 0xf);
				sb.Append(nibbleToDigit(b));
				i >>= 4;
			}
			
			return sb.ToString();
		}
		
		/// 
		/// <summary> Converts the lower four bits of a byte into the ascii digit 
		/// which represents its hex value. For example:
		/// nibbleToDigit(10) produces 'a'.
		/// </summary>
		public static char nibbleToDigit(byte x)
		{
			char c = (char) (x & 0xf); // mask low nibble
			return (c > 9?(char) (c - 10 + 'a'):(char) (c + '0')); // int to hex char
		}
		
		/// <summary> Convert a single byte into a string representing its hex value.
		/// i.e. -1 -> "ff"
		/// </summary>
		/// <param name="b">the byte to convert.
		/// </param>
		/// <returns> a string containing the hex equivalent.
		/// 
		/// </returns>
		public static System.String toString(byte b)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(nibbleToDigit((byte) (b >> 4)));
			sb.Append(nibbleToDigit(b));
			return sb.ToString();
		}
		
		/// <summary> Returns a text representation of a byte array.
		/// *
		/// </summary>
		/// <param name="bytes">a byte array
		/// </param>
		/// <returns> a string containing the hex equivalent of the bytes.
		/// 
		/// </returns>
		public static System.String toString(byte[] bytes)
		{
			System.IO.StringWriter sw = new System.IO.StringWriter();
			
			int length = bytes.Length;
			if (length > 0)
			{
				for (int i = 0; i < length; i++)
				{
					sw.Write(toString(bytes[i]));
					if (i != length - 1)
						sw.Write(" ");
				}
			}
			return (sw.ToString());
		}
		
		
		internal const System.String _rcsid = "$Id: HexHelper.java,v 1.3 2001/06/04 05:07:06 pcharles Exp $";
	}
}