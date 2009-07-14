// $Id: UDPFields.java,v 1.3 2001/06/27 01:47:00 pcharles Exp $

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
	/// @lastModifiedAt $Date: 2001/06/27 01:47:00 $
	/// 
	/// </version>
	public struct UDPFields_Fields{
		public readonly static int UDP_PORT_LEN = 2;
		public readonly static int UDP_LEN_LEN = 2;
		public readonly static int UDP_CSUM_LEN = 2;
		public readonly static int UDP_SP_POS = 0;
		public readonly static int UDP_DP_POS;
		public readonly static int UDP_LEN_POS;
		public readonly static int UDP_CSUM_POS;
		public readonly static int UDP_HEADER_LEN;
		static UDPFields_Fields()
		{
			UDP_DP_POS = Packets.UDPFields_Fields.UDP_PORT_LEN;
			UDP_LEN_POS = Packets.UDPFields_Fields.UDP_DP_POS + Packets.UDPFields_Fields.UDP_PORT_LEN;
			UDP_CSUM_POS = Packets.UDPFields_Fields.UDP_LEN_POS + Packets.UDPFields_Fields.UDP_LEN_LEN;
			UDP_HEADER_LEN = Packets.UDPFields_Fields.UDP_CSUM_POS + Packets.UDPFields_Fields.UDP_CSUM_LEN;
		}
	}
	public interface UDPFields
		{
			//UPGRADE_NOTE: Members of interface 'UDPFields' were extracted into structure 'UDPFields_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			// field lengths
			
			/// <summary> Length of a UDP port in bytes.
			/// </summary>
			/// <summary> Length of the header length field in bytes.
			/// </summary>
			/// <summary> Length of the checksum field in bytes.
			/// </summary>
			// field positions
			
			/// <summary> Position of the source port.
			/// </summary>
			/// <summary> Position of the destination port.
			/// </summary>
			/// <summary> Position of the header length.
			/// </summary>
			/// <summary> Position of the header checksum length.
			/// </summary>
			// complete header length 
			
			/// <summary> Length of a UDP header in bytes.
			/// </summary>
			// == 8
		}
}