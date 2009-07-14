// $Id: IPAddress.java,v 1.5 2002/11/07 23:23:38 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using ArrayHelper = Packets.Util.ArrayHelper;
	/// <summary> IP address.
	/// <p>
	/// This class doesn't store IP addresses. There's a java class for that,
	/// and it is too big and cumbersome for our purposes.
	/// <p>
	/// This class contains a utility method for extracting an IP address 
	/// from a big-endian byte array.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.5 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2002/11/07 23:23:38 $
	/// 
	/// </version>
	public class IPAddress
	{
		/// <summary> Convert an IP address stored in an int to its string representation.
		/// </summary>
		private static System.String toString(int address)
		{
			System.Text.StringBuilder sa = new System.Text.StringBuilder();
			for (int i = 0; i < WIDTH; i++)
			{
				sa.Append(0xff & address >> 24);
				address <<= 8;
				if (i != WIDTH - 1)
					sa.Append('.');
			}
			return sa.ToString();
		}
		
		/// <summary> Extract a string describing an IP address from an array of bytes.
		/// *
		/// </summary>
		/// <param name="offset">the offset of the address data.
		/// </param>
		/// <param name="bytes">an array of bytes containing the IP address.
		/// </param>
		/// <returns> a string of the form "255.255.255.255"
		/// 
		/// </returns>
		public static System.String extract(int offset, byte[] bytes)
		{
			return toString(ArrayHelper.extractInteger(bytes, offset, WIDTH));
			/*
			StringBuffer sa = new StringBuffer();
			for(int i=offset; i<offset + WIDTH; i++) {
			sa.append(0xff & bytes[i]);
			if(i != offset + WIDTH - 1)
			sa.append('.');
			}
			return sa.toString();
			*/
		}
		
		/// <summary> Generate a random IP number between 0.0.0.0 and 255.255.255.255.
		/// </summary>
		public static int random()
		{
			// cast to long before int to preserve all 32-bits of precision
			// (otherwise, highest bit is lost for based on sign)
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (int) (0xffffffffL * SupportClass.Random.NextDouble());
		}
		
		/// <summary> Generate a random IP address.
		/// </summary>
		/// <param name="network">the network number. i.e. 0x0a000000.
		/// </param>
		/// <param name="mask">the network mask. i.e. 0xffffff00.
		/// </param>
		/// <returns> a random IP address on the specified network.
		/// 
		/// </returns>
		public static int random(int network, int mask)
		{
			// the bits that get randomized are the inverse of the mask
			int rbits = ~ mask;
			
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int random = network + (int) (rbits * SupportClass.Random.NextDouble()) + 1;
			
			return random;
		}
		
		
		/// <summary> Unit test.
		/// </summary>
		[STAThread]
		public static void  Main1(System.String[] args)
		{
			for (int i = 0; i < 10; i++)
			{
				// 10.0.0.16/255.255.255.240
				int r = random(0x0a000010, (int) SupportClass.GetConst(0xfffffff0));
				System.Console.Error.WriteLine(System.Convert.ToString(r, 16) + " " + toString(r));
			}
		}
		
		
		/// <summary> The width in bytes of an IP address.
		/// </summary>
		public const int WIDTH = 4;
		
		private System.String _rcsid = "$Id: IPAddress.java,v 1.5 2002/11/07 23:23:38 pcharles Exp $";
	}
}