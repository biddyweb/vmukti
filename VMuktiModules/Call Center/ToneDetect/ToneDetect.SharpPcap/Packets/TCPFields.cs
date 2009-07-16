// $Id: TCPFields.java,v 1.5 2003/10/29 02:38:27 pcharles Exp $

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
	/// <version>  $Revision: 1.5 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2003/10/29 02:38:27 $
	/// 
	/// </version>
	public struct TCPFields_Fields{
		public readonly static int TCP_URG_MASK = 0x0020;
		public readonly static int TCP_ACK_MASK = 0x0010;
		public readonly static int TCP_PSH_MASK = 0x0008;
		public readonly static int TCP_RST_MASK = 0x0004;
		public readonly static int TCP_SYN_MASK = 0x0002;
		public readonly static int TCP_FIN_MASK = 0x0001;
		public readonly static int TCP_PORT_LEN = 2;
		public readonly static int TCP_SEQ_LEN = 4;
		public readonly static int TCP_ACK_LEN = 4;
		public readonly static int TCP_FLAG_LEN = 2;
		public readonly static int TCP_WIN_LEN = 2;
		public readonly static int TCP_CSUM_LEN = 2;
		public readonly static int TCP_URG_LEN = 2;
		public readonly static int TCP_SP_POS = 0;
		public readonly static int TCP_DP_POS;
		public readonly static int TCP_SEQ_POS;
		public readonly static int TCP_ACK_POS;
		public readonly static int TCP_FLAG_POS;
		public readonly static int TCP_WIN_POS;
		public readonly static int TCP_CSUM_POS;
		public readonly static int TCP_URG_POS;
		public readonly static int TCP_HEADER_LEN;
		static TCPFields_Fields()
		{
			TCP_DP_POS = Packets.TCPFields_Fields.TCP_PORT_LEN;
			TCP_SEQ_POS = Packets.TCPFields_Fields.TCP_DP_POS + Packets.TCPFields_Fields.TCP_PORT_LEN;
			TCP_ACK_POS = Packets.TCPFields_Fields.TCP_SEQ_POS + Packets.TCPFields_Fields.TCP_SEQ_LEN;
			TCP_FLAG_POS = Packets.TCPFields_Fields.TCP_ACK_POS + Packets.TCPFields_Fields.TCP_ACK_LEN;
			TCP_WIN_POS = Packets.TCPFields_Fields.TCP_FLAG_POS + Packets.TCPFields_Fields.TCP_FLAG_LEN;
			TCP_CSUM_POS = Packets.TCPFields_Fields.TCP_WIN_POS + Packets.TCPFields_Fields.TCP_WIN_LEN;
			TCP_URG_POS = Packets.TCPFields_Fields.TCP_CSUM_POS + Packets.TCPFields_Fields.TCP_CSUM_LEN;
			TCP_HEADER_LEN = Packets.TCPFields_Fields.TCP_URG_POS + Packets.TCPFields_Fields.TCP_URG_LEN;
		}
	}
	public interface TCPFields
		{
			//UPGRADE_NOTE: Members of interface 'TCPFields' were extracted into structure 'TCPFields_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			// flag bitmasks
			// field lengths
			
			/// <summary> Length of a TCP port in bytes.
			/// </summary>
			/// <summary> Length of the sequence number in bytes.
			/// </summary>
			/// <summary> Length of the acknowledgment number in bytes.
			/// </summary>
			/// <summary> Length of the header length and flags field in bytes.
			/// </summary>
			/// <summary> Length of the window size field in bytes.
			/// </summary>
			/// <summary> Length of the checksum field in bytes.
			/// </summary>
			/// <summary> Length of the urgent field in bytes.
			/// </summary>
			// field positions
			
			/// <summary> Position of the source port field.
			/// </summary>
			/// <summary> Position of the destination port field.
			/// </summary>
			/// <summary> Position of the sequence number field.
			/// </summary>
			/// <summary> Position of the acknowledgment number field.
			/// </summary>
			/// <summary> Position of the header length and flags field.
			/// </summary>
			/// <summary> Position of the window size field.
			/// </summary>
			/// <summary> Position of the checksum field.
			/// </summary>
			/// <summary> Position of the urgent pointer field.
			/// </summary>
			// complete header length 
			
			/// <summary> Length in bytes of a TCP header.
			/// </summary>
			// == 20
		}
}