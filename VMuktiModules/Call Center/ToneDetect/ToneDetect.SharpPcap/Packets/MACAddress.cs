// $Id: MACAddress.java,v 1.4 2002/11/07 23:23:46 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using HexHelper = Packets.Util.HexHelper;
	/// <summary> MAC address.
	/// <p>
	/// This class doesn't yet store MAC addresses. Only a utility method
	/// to extract a MAC address from a big-endian byte array is implemented.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.4 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2002/11/07 23:23:46 $
	/// 
	/// </version>
	public class MACAddress
	{
		/// <summary> Extract a MAC address from an array of bytes.
		/// </summary>
		/// <param name="offset">the offset of the address data from the start of the 
		/// packet.
		/// </param>
		/// <param name="bytes">an array of bytes containing at least one MAC address.
		/// 
		/// </param>
		public static System.String extract(int offset, byte[] bytes)
		{
			System.Text.StringBuilder sa = new System.Text.StringBuilder();
			for (int i = offset; i < offset + WIDTH; i++)
			{
				sa.Append(HexHelper.toString(bytes[i]));
				if (i != offset + WIDTH - 1)
					sa.Append(':');
			}
			return sa.ToString();
		}
		
		/// <summary> Generate a random MAC address.
		/// </summary>
		public static long random()
		{
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (long) (0xffffffffffffL * SupportClass.Random.NextDouble());
		}
		
		/// <summary> The width in bytes of a MAC address.
		/// </summary>
		public const int WIDTH = 6;
		
		private System.String _rcsid = "$Id: MACAddress.java,v 1.4 2002/11/07 23:23:46 pcharles Exp $";
	}
}