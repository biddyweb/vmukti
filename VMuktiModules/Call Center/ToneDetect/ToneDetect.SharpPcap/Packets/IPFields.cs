// $Id: IPFields.java,v 1.4 2001/06/27 01:46:59 pcharles Exp $

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
	/// <version>  $Revision: 1.4 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/27 01:46:59 $
	/// 
	/// </version>
	public struct IPFields_Fields{
		public readonly static int IP_VER_LEN = 1;
		public readonly static int IP_TOS_LEN = 1;
		public readonly static int IP_LEN_LEN = 2;
		public readonly static int IP_ID_LEN = 2;
		public readonly static int IP_FRAG_LEN = 2;
		public readonly static int IP_TTL_LEN = 1;
		public readonly static int IP_CODE_LEN = 1;
		public readonly static int IP_CSUM_LEN = 2;
		public readonly static int IP_VER_POS = 0;
		public readonly static int IP_TOS_POS;
		public readonly static int IP_LEN_POS;
		public readonly static int IP_ID_POS;
		public readonly static int IP_FRAG_POS;
		public readonly static int IP_TTL_POS;
		public readonly static int IP_CODE_POS;
		public readonly static int IP_CSUM_POS;
		public readonly static int IP_SRC_POS;
		public readonly static int IP_DST_POS;
		public readonly static int IP_HEADER_LEN;
		static IPFields_Fields()
		{
			IP_TOS_POS = Packets.IPFields_Fields.IP_VER_POS + Packets.IPFields_Fields.IP_VER_LEN;
			IP_LEN_POS = Packets.IPFields_Fields.IP_TOS_POS + Packets.IPFields_Fields.IP_TOS_LEN;
			IP_ID_POS = Packets.IPFields_Fields.IP_LEN_POS + Packets.IPFields_Fields.IP_LEN_LEN;
			IP_FRAG_POS = Packets.IPFields_Fields.IP_ID_POS + Packets.IPFields_Fields.IP_ID_LEN;
			IP_TTL_POS = Packets.IPFields_Fields.IP_FRAG_POS + Packets.IPFields_Fields.IP_FRAG_LEN;
			IP_CODE_POS = Packets.IPFields_Fields.IP_TTL_POS + Packets.IPFields_Fields.IP_TTL_LEN;
			IP_CSUM_POS = Packets.IPFields_Fields.IP_CODE_POS + Packets.IPFields_Fields.IP_CODE_LEN;
			IP_SRC_POS = Packets.IPFields_Fields.IP_CSUM_POS + Packets.IPFields_Fields.IP_CSUM_LEN;
			IP_DST_POS = Packets.IPFields_Fields.IP_SRC_POS + IPAddress.WIDTH;
			IP_HEADER_LEN = Packets.IPFields_Fields.IP_DST_POS + IPAddress.WIDTH;
		}
	}
	public interface IPFields
		{
			//UPGRADE_NOTE: Members of interface 'IPFields' were extracted into structure 'IPFields_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			// field lengths
			
			/// <summary> Width of the IP version and header length field in bytes.
			/// </summary>
			/// <summary> Width of the TOS field in bytes.
			/// </summary>
			/// <summary> Width of the header length field in bytes.
			/// </summary>
			/// <summary> Width of the ID field in bytes.
			/// </summary>
			/// <summary> Width of the fragmentation bits and offset field in bytes.
			/// </summary>
			/// <summary> Width of the TTL field in bytes.
			/// </summary>
			/// <summary> Width of the IP protocol code in bytes.
			/// </summary>
			/// <summary> Width of the IP checksum in bytes.
			/// </summary>
			// field positions
			
			/// <summary> Position of the version code and header length within the IP header.
			/// </summary>
			/// <summary> Position of the type of service code within the IP header.
			/// </summary>
			/// <summary> Position of the length within the IP header.
			/// </summary>
			/// <summary> Position of the packet ID within the IP header.
			/// </summary>
			/// <summary> Position of the flag bits and fragment offset within the IP header.
			/// </summary>
			/// <summary> Position of the ttl within the IP header.
			/// </summary>
			/// <summary> Position of the IP protocol code within the IP header.
			/// </summary>
			/// <summary> Position of the checksum within the IP header.
			/// </summary>
			/// <summary> Position of the source IP address within the IP header.
			/// </summary>
			/// <summary> Position of the destination IP address within a packet.
			/// </summary>
			// complete header length 
			
			/// <summary> Length in bytes of an IP header, excluding options.
			/// </summary>
			// == 20
		}
}