// $Id: ARPFields.java,v 1.3 2001/06/27 01:46:59 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> IP protocol field encoding information.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.3 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/27 01:46:59 $
	/// 
	/// </version>
	public struct ARPFields{
		
		public readonly static int ARP_ETH_ADDR_CODE = 0x0001;		
		public readonly static int ARP_IP_ADDR_CODE = 0x0800;		
		public readonly static int ARP_OP_REQ_CODE = 0x1;		
		public readonly static int ARP_OP_REP_CODE = 0x2;		
		public readonly static int ARP_OP_LEN = 2;
		
		// field positions 

		/// <summary> Address type length in bytes.	/// </summary>
		public readonly static int ARP_ADDR_TYPE_LEN = 2;
		/// <summary> Address type length in bytes.	/// </summary>
		public readonly static int ARP_ADDR_SIZE_LEN = 1;		
		/// /// <summary> Position of the hardware address type./// </summary>
		public readonly static int ARP_HW_TYPE_POS = 0;		
		/// <summary> Position of the protocol address type./// </summary>
		public readonly static int ARP_PR_TYPE_POS;
		/// <summary> Position of the hardware address length./// </summary>
		public readonly static int ARP_HW_LEN_POS;
		/// <summary> Position of the protocol address length./// </summary>
		public readonly static int ARP_PR_LEN_POS;
		/// <summary> Position of the operation type./// </summary>
		public readonly static int ARP_OP_POS;
		/// <summary> Position of the sender hardware address./// </summary>
		public readonly static int ARP_S_HW_ADDR_POS;
		/// <summary> Position of the sender protocol address./// </summary>
		public readonly static int ARP_S_PR_ADDR_POS;
		/// <summary> Position of the target hardware address./// </summary>
		public readonly static int ARP_T_HW_ADDR_POS;
		/// <summary> Position of the target protocol address./// </summary>
		public readonly static int ARP_T_PR_ADDR_POS;
		// complete header length
			
		/// <summary> Total length in bytes of an ARP header./// </summary>
		// == 28
		public readonly static int ARP_HEADER_LEN;
		static ARPFields()
		{
			ARP_PR_TYPE_POS = Packets.ARPFields.ARP_HW_TYPE_POS + Packets.ARPFields.ARP_ADDR_TYPE_LEN;
			ARP_HW_LEN_POS = Packets.ARPFields.ARP_PR_TYPE_POS + Packets.ARPFields.ARP_ADDR_TYPE_LEN;
			ARP_PR_LEN_POS = Packets.ARPFields.ARP_HW_LEN_POS + Packets.ARPFields.ARP_ADDR_SIZE_LEN;
			ARP_OP_POS = Packets.ARPFields.ARP_PR_LEN_POS + Packets.ARPFields.ARP_ADDR_SIZE_LEN;
			ARP_S_HW_ADDR_POS = Packets.ARPFields.ARP_OP_POS + Packets.ARPFields.ARP_OP_LEN;
			ARP_S_PR_ADDR_POS = Packets.ARPFields.ARP_S_HW_ADDR_POS + MACAddress.WIDTH;
			ARP_T_HW_ADDR_POS = Packets.ARPFields.ARP_S_PR_ADDR_POS + IPAddress.WIDTH;
			ARP_T_PR_ADDR_POS = Packets.ARPFields.ARP_T_HW_ADDR_POS + MACAddress.WIDTH;
			ARP_HEADER_LEN = Packets.ARPFields.ARP_T_PR_ADDR_POS + IPAddress.WIDTH;
		}
	}
}