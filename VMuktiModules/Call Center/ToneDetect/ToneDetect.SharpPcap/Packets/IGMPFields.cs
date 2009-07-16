// $Id: IGMPFields.java,v 1.1 2001/07/30 00:00:02 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> IGMP protocol field encoding information.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/07/30 00:00:02 $
	/// 
	/// </version>
	public struct IGMPFields{
		// field lengths
			
		/// <summary> Length of the IGMP message type code in bytes.</summary>
		public readonly static int IGMP_CODE_LEN = 1;
		/// <summary> Length of the IGMP max response code in bytes.</summary>
		public readonly static int IGMP_MRSP_LEN = 1;
		/// <summary> Length of the IGMP header checksum in bytes.</summary>
		public readonly static int IGMP_CSUM_LEN = 2;
		/// <summary> Length of group address in bytes.</summary>
		public readonly static int IGMP_GADDR_LEN;

		// field positions
			
		/// <summary> Position of the IGMP message type.</summary>
		public readonly static int IGMP_CODE_POS = 0;
		/// <summary> Position of the IGMP max response code.</summary>
		public readonly static int IGMP_MRSP_POS;
		/// <summary> Position of the IGMP header checksum.</summary>
		public readonly static int IGMP_CSUM_POS;
		/// <summary> Position of the IGMP group address.</summary>
		public readonly static int IGMP_GADDR_POS;
		// complete header length 
			
		/// <summary> Length in bytes of an IGMP header.</summary>
		// 8
		public readonly static int IGMP_HEADER_LEN;	
		
		static IGMPFields()
		{
			IGMP_GADDR_LEN = IPAddress.WIDTH;
			IGMP_MRSP_POS = Packets.IGMPFields.IGMP_CODE_POS + Packets.IGMPFields.IGMP_CODE_LEN;
			IGMP_CSUM_POS = Packets.IGMPFields.IGMP_MRSP_POS + Packets.IGMPFields.IGMP_MRSP_LEN;
			IGMP_GADDR_POS = Packets.IGMPFields.IGMP_CSUM_POS + Packets.IGMPFields.IGMP_CSUM_LEN;
			IGMP_HEADER_LEN = Packets.IGMPFields.IGMP_GADDR_POS + Packets.IGMPFields.IGMP_GADDR_LEN;
		}
	}
}