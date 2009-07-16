// $Id: EthernetFields.java,v 1.3 2001/06/27 02:14:54 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> Ethernet protocol field encoding information.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.3 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/27 02:14:54 $
	/// 
	/// </version>
	public class EthernetFields
	{
		// field lengths
			
		/// <summary> Width of the ethernet type code in bytes./// </summary>
		public readonly static int ETH_CODE_LEN = 2;
		// field positions
			
		/// <summary> Position of the destination MAC address within the ethernet header./// </summary>
		public readonly static int ETH_DST_POS = 0;
		/// <summary> Position of the source MAC address within the ethernet header./// </summary>
		public readonly static int ETH_SRC_POS;
		/// <summary> Position of the ethernet type field within the ethernet header./// </summary>
		public readonly static int ETH_CODE_POS;
		
		// complete header length		
		
		/// <summary> Total length of an ethernet header in bytes./// </summary>
		// == 14
		public readonly static int ETH_HEADER_LEN;

		static EthernetFields()
		{
			ETH_SRC_POS = MACAddress.WIDTH;
			ETH_CODE_POS = MACAddress.WIDTH * 2;
			ETH_HEADER_LEN = Packets.EthernetFields.ETH_CODE_POS + Packets.EthernetFields.ETH_CODE_LEN;
		}
	}
}