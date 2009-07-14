// $Id: IPProtocol.java,v 1.5 2004/02/24 19:21:31 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using HashMap =  System.Collections.Hashtable;
	/// <summary> IPProtocol utility class.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.5 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/02/24 19:21:31 $
	/// 
	/// </version>
	public class IPProtocol : IPProtocols
	{
		/// <summary> Fetch a protocol description.
		/// </summary>
		/// <param name="code">the code associated with the message.
		/// </param>
		/// <returns> a message describing the significance of the IP protocol.
		/// 
		/// </returns>
		public static System.String getDescription(int code)
		{
			System.Int32 c = code;
			if (messages.ContainsKey(c))
				return (System.String) messages[c];
			else
				return "unknown";
		}
		
		/// <summary> 'Human-readable' IP protocol descriptions.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'messages' was moved to static method 'Packets.IPProtocol'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static HashMap messages;
		
		/// <summary> Extract the protocol code from packet data. The packet data 
		/// must contain an IP datagram.
		/// The protocol code specifies what kind of information is contained in the 
		/// data block of the ip datagram.
		/// *
		/// </summary>
		/// <param name="lLen">the length of the link-level header.
		/// </param>
		/// <param name="packetBytes">packet bytes, including the link-layer header.
		/// </param>
		/// <returns> the IP protocol code. i.e. 0x06 signifies TCP protocol.
		/// 
		/// </returns>
		public static int extractProtocol(int lLen, byte[] packetBytes)
		{
			return packetBytes[lLen + Packets.IPFields_Fields.IP_CODE_POS];
		}
		
		
		private System.String _rcsid = "$Id: IPProtocol.java,v 1.5 2004/02/24 19:21:31 pcharles Exp $";
		static IPProtocol()
		{
			messages = new HashMap();
			{
				messages[Packets.IPProtocols_Fields.IP] = "Dummy protocol for TCP";
				messages[Packets.IPProtocols_Fields.HOPOPTS] = "IPv6 Hop-by-Hop options";
				messages[Packets.IPProtocols_Fields.ICMP] = "Internet Control Message Protocol";
				messages[Packets.IPProtocols_Fields.IGMP] = "Internet Group Management Protocol";
				messages[Packets.IPProtocols_Fields.IPIP] = "IPIP tunnels";
				messages[Packets.IPProtocols_Fields.TCP] = "Transmission Control Protocol";
				messages[Packets.IPProtocols_Fields.EGP] = "Exterior Gateway Protocol";
				messages[Packets.IPProtocols_Fields.PUP] = "PUP protocol";
				messages[Packets.IPProtocols_Fields.UDP] = "User Datagram Protocol";
				messages[Packets.IPProtocols_Fields.IDP] = "XNS IDP protocol";
				messages[Packets.IPProtocols_Fields.TP] = "SO Transport Protocol Class 4";
				messages[Packets.IPProtocols_Fields.IPV6] = "IPv6 header";
				messages[Packets.IPProtocols_Fields.ROUTING] = "IPv6 routing header";
				messages[Packets.IPProtocols_Fields.FRAGMENT] = "IPv6 fragmentation header";
				messages[Packets.IPProtocols_Fields.RSVP] = "Reservation Protocol";
				messages[Packets.IPProtocols_Fields.GRE] = "General Routing Encapsulation";
				messages[Packets.IPProtocols_Fields.ESP] = "encapsulating security payload";
				messages[Packets.IPProtocols_Fields.AH] = "authentication header";
				messages[Packets.IPProtocols_Fields.ICMPV6] = "ICMPv6";
				messages[Packets.IPProtocols_Fields.NONE] = "IPv6 no next header";
				messages[Packets.IPProtocols_Fields.DSTOPTS] = "IPv6 destination options";
				messages[Packets.IPProtocols_Fields.MTP] = "Multicast Transport Protocol";
				messages[Packets.IPProtocols_Fields.ENCAP] = "Encapsulation Header";
				messages[Packets.IPProtocols_Fields.PIM] = "Protocol Independent Multicast";
				messages[Packets.IPProtocols_Fields.COMP] = "Compression Header Protocol";
				messages[Packets.IPProtocols_Fields.RAW] = "Raw IP Packet";
				messages[Packets.IPProtocols_Fields.INVALID] = "INVALID IP";
			}
		}
	}
}