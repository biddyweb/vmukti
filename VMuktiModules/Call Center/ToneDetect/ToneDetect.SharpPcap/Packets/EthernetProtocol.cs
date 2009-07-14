// $Id: EthernetProtocol.java,v 1.3 2001/06/27 01:50:42 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	//using HashMap =  System.Collections.Hashtable;
	/// <summary> Ethernet protocol utility class.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.3 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/27 01:50:42 $
	/// 
	/// </version>
	public class EthernetProtocol : EthernetProtocols
	{
		/// <summary> Extract the protocol type field from packet data.
		/// <p>
		/// The type field indicates what type of data is contained in the 
		/// packet's data block.
		/// </summary>
		/// <param name="packetBytes">packet bytes.
		/// </param>
		/// <returns> the ethernet type code. i.e. 0x800 signifies IP datagram.
		/// 
		/// </returns>
		public static int extractProtocol(byte[] packetBytes)
		{
			// convert the bytes that contain the type code into a value..
			return packetBytes[Packets.EthernetFields.ETH_CODE_POS] << 8 | packetBytes[Packets.EthernetFields.ETH_CODE_POS + 1];
		}
		
		private System.String _rcsid = "$Id: EthernetProtocol.java,v 1.3 2001/06/27 01:50:42 pcharles Exp $";
	}
}