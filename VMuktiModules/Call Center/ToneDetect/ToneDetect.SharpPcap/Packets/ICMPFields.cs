// $Id: ICMPFields.java,v 1.5 2001/07/30 00:01:22 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> ICMP protocol field encoding information.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.5 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/07/30 00:01:22 $
	/// 
	/// </version>
	public struct ICMPFields{
		// field lengths
			
		/// <summary> Length of the ICMP message type code in bytes. </summary>
		public readonly static int ICMP_CODE_LEN = 1;
		/// <summary> Length of the ICMP subcode in bytes./// </summary>
		public readonly static int ICMP_SUBC_LEN = 1;
		/// <summary> Length of the ICMP header checksum in bytes./// </summary>
		public readonly static int ICMP_CSUM_LEN = 2;

		// field positions
			
		/// <summary> Position of the ICMP message type.</summary>
		public readonly static int ICMP_CODE_POS = 0;
		/// <summary> Position of the ICMP message subcode.</summary>
		public readonly static int ICMP_SUBC_POS;
		/// <summary> Position of the ICMP header checksum.</summary>
		public readonly static int ICMP_CSUM_POS;	

		// complete header length 
			
		/// <summary> Length in bytes of an ICMP header.</summary>
		// == 4		
		public readonly static int ICMP_HEADER_LEN;

		static ICMPFields()
		{
			ICMP_SUBC_POS = Packets.ICMPFields.ICMP_CODE_POS + Packets.ICMPFields.ICMP_CODE_LEN;
			ICMP_CSUM_POS = Packets.ICMPFields.ICMP_SUBC_POS + Packets.ICMPFields.ICMP_CODE_LEN;
			ICMP_HEADER_LEN = Packets.ICMPFields.ICMP_CSUM_POS + Packets.ICMPFields.ICMP_CSUM_LEN;
		}
	}
}