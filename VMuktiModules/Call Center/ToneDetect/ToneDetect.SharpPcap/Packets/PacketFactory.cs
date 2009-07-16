// $Id: PacketFactory.java,v 1.12 2004/05/05 23:14:45 pcharles Exp $

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
	using Timeval = Packets.Util.Timeval;
	/// <summary> This factory constructs high-level packet objects from
	/// captured data streams.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.12 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	public class PacketFactory
	{
		/// <summary> Convert captured packet data into an object.
		/// </summary>
		public static Packet dataToPacket(int linkType, byte[] bytes)
		{
			int ethProtocol;
			
			// record the length of the headers associated with this link layer type.
			// this length is the offset to the header embedded in the packet.
			lLen = LinkLayer.getLinkLayerLength(linkType);
			
			// extract the protocol code for the type of header embedded in the 
			// link-layer of the packet
			int offset = LinkLayer.getProtoOffset(linkType);
			if (offset == - 1)
			// if there is no embedded protocol, assume IP?
				ethProtocol = Packets.EthernetProtocols_Fields.IP;
			else
				ethProtocol = ArrayHelper.extractInteger(bytes, offset, EthernetFields.ETH_CODE_LEN);
			
			// try to recognize the ethernet type..
			switch (ethProtocol)
			{
				
				// arp
				case Packets.EthernetProtocols_Fields.ARP: 
					return new ARPPacket(lLen, bytes);
				
				case Packets.EthernetProtocols_Fields.IP: 
					int ipProtocol = IPProtocol.extractProtocol(lLen, bytes);
					switch (ipProtocol)
					{
						
						// icmp
						case Packets.IPProtocols_Fields.ICMP:  return new ICMPPacket(lLen, bytes);
							// igmp
						
						case Packets.IPProtocols_Fields.IGMP:  return new IGMPPacket(lLen, bytes);
							// tcp
						
						case Packets.IPProtocols_Fields.TCP:  return new TCPPacket(lLen, bytes);
							// udp
						
						case Packets.IPProtocols_Fields.UDP:  return new UDPPacket(lLen, bytes);
							// unidentified ip..
						
						default:  return new IPPacket(lLen, bytes);
						
					}
					// ethernet level code not recognized, default to anonymous packet..
					//goto default;
				
				default:  return new EthernetPacket(lLen, bytes);
				
			}
		}
		
		/// <summary> Convert captured packet data into an object.
		/// </summary>
		public static Packet dataToPacket(int linkType, byte[] bytes, Timeval tv)
		{
			int ethProtocol;
			
			// record the length of the headers associated with this link layer type.
			// this length is the offset to the header embedded in the packet.
			lLen = LinkLayer.getLinkLayerLength(linkType);
			
			// extract the protocol code for the type of header embedded in the 
			// link-layer of the packet
			int offset = LinkLayer.getProtoOffset(linkType);
			if (offset == - 1)
			// if there is no embedded protocol, assume IP?
				ethProtocol = Packets.EthernetProtocols_Fields.IP;
			else
				ethProtocol = ArrayHelper.extractInteger(bytes, offset, Packets.EthernetFields.ETH_CODE_LEN);
			
			// try to recognize the ethernet type..
			switch (ethProtocol)
			{
				
				// arp
				case Packets.EthernetProtocols_Fields.ARP: 
					return new ARPPacket(lLen, bytes, tv);
				
				case Packets.EthernetProtocols_Fields.IP: 
					int ipProtocol = IPProtocol.extractProtocol(lLen, bytes);
					switch (ipProtocol)
					{
						
						// icmp
						case Packets.IPProtocols_Fields.ICMP:  return new ICMPPacket(lLen, bytes, tv);
							// igmp
						
						case Packets.IPProtocols_Fields.IGMP:  return new IGMPPacket(lLen, bytes, tv);
							// tcp
						
						case Packets.IPProtocols_Fields.TCP:  return new TCPPacket(lLen, bytes, tv);
							// udp
						
						case Packets.IPProtocols_Fields.UDP:  return new UDPPacket(lLen, bytes, tv);
							// unidentified ip..
						
						default:  return new IPPacket(lLen, bytes, tv);
						
					}
					// ethernet level code not recognized, default to anonymous packet..
					//goto default;
				
				default:  return new EthernetPacket(lLen, bytes, tv);
				
			}
		}
		
		
		/// <summary> Length in bytes of the link-level headers that this factory is 
		/// decoding packets for.
		/// </summary>
		private static int lLen;
		
		private System.String _rcsid = "$Id: PacketFactory.java,v 1.12 2004/05/05 23:14:45 pcharles Exp $";
	}
}