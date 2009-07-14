// $Id: HeaderGenerator.java,v 1.2 2001/06/27 02:17:33 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets.Simulator
{
	using System;
	using ArrayHelper = Packets.Util.ArrayHelper;
	using MACAddress = Packets.MACAddress;
	using IPAddress = Packets.IPAddress;
	using EthernetProtocols = Packets.EthernetProtocols_Fields;
	using EthernetFields = Packets.EthernetFields;
	using ARPFields = Packets.ARPFields;
	using IPProtocols = Packets.IPProtocols_Fields;
	using IPFields = Packets.IPFields;
	using IPPorts = Packets.IPPorts_Fields;
	using UDPFields = Packets.UDPFields;
	using TCPFields = Packets.TCPFields;
	using ICMPFields = Packets.ICMPFields;
	using ICMPMessages = Packets.ICMPMessages_Fields;
	/// <summary> This class generates random protocol headers.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.2 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/27 02:17:33 $
	/// 
	/// </version>
	public class HeaderGenerator : EthernetFields, IPFields, TCPFields, UDPFields
	{
		/// <summary> Roll the dice.
		/// </summary>
		/// <returns> whether or not a synthesized probabilistic event occurred.
		/// 
		/// </returns>
		private static bool test(float probability)
		{
			double r = SupportClass.Random.NextDouble();
			return r < probability;
		}
		
		/// <summary> Generate a pseudo-random ethernet protocol code.
		/// </summary>
		public static int randomEthernetProtocol()
		{
			if (test(Settings.PROB_ETH_IP))
				return EthernetProtocols.IP;
			if (test(Settings.PROB_ETH_ARP))
				return EthernetProtocols.ARP;
			if (test(Settings.PROB_ETH_RARP))
				return EthernetProtocols.RARP;
			
			// other..
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (int) (SupportClass.Random.NextDouble() * EthernetProtocols.MASK);
		}
		
		/// <summary> Generate a pseudo-random IP protocol code.
		/// </summary>
		public static int randomIPProtocol()
		{
			if (test(Settings.PROB_IP_TCP))
				return IPProtocols.TCP;
			if (test(Settings.PROB_IP_UDP))
				return IPProtocols.UDP;
			if (test(Settings.PROB_IP_ICMP))
				return IPProtocols.ICMP;
			
			// other..
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (int) (SupportClass.Random.NextDouble() * IPProtocols.MASK);
		}
		
		/// <summary> Generate a pseudo-random IP port.
		/// </summary>
		public static int randomPort()
		{
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (int) (SupportClass.Random.NextDouble() * IPPorts.MASK);
		}
		
		/// <summary> Generate a pseudo-random well-known IP port.
		/// </summary>
		public static int randomPrivilegedPort()
		{
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (int) (SupportClass.Random.NextDouble() * IPPorts.LIMIT_PRIVILEGED);
		}
		
		/// <summary> Generate a pseudo-random ICMP protocol code (message type).
		/// </summary>
		public static int randomICMPType()
		{
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			return (int) (SupportClass.Random.NextDouble() * ICMPMessages.LAST_MAJOR_CODE);
		}
		
		/// <summary> Generate a pseudo-random ethernet header.
		/// </summary>
		public static byte[] generateRandomEthernetHeader()
		{
			byte[] bytes = new byte[Packets.EthernetFields.ETH_HEADER_LEN];
			
			long dst = MACAddress.random();
			ArrayHelper.insertLong(bytes, dst, Packets.EthernetFields.ETH_DST_POS, MACAddress.WIDTH);
			
			long src = MACAddress.random();
			ArrayHelper.insertLong(bytes, src, Packets.EthernetFields.ETH_SRC_POS, MACAddress.WIDTH);
			
			int type = randomEthernetProtocol();
			ArrayHelper.insertLong(bytes, type, Packets.EthernetFields.ETH_CODE_POS, Packets.EthernetFields.ETH_CODE_LEN);
			
			return bytes;
		}
		
		/// <summary> Generate a pseudo-random ARP header.
		/// </summary>
		public static byte[] generateRandomARPHeader()
		{
			byte[] bytes = new byte[Packets.ARPFields.ARP_HEADER_LEN];
			
			ArrayHelper.insertLong(bytes, Packets.ARPFields.ARP_ETH_ADDR_CODE, Packets.ARPFields.ARP_HW_TYPE_POS, Packets.ARPFields.ARP_ADDR_TYPE_LEN);
			ArrayHelper.insertLong(bytes, Packets.ARPFields.ARP_IP_ADDR_CODE, Packets.ARPFields.ARP_PR_TYPE_POS, Packets.ARPFields.ARP_ADDR_TYPE_LEN);
			ArrayHelper.insertLong(bytes, IPAddress.WIDTH, Packets.ARPFields.ARP_HW_LEN_POS, Packets.ARPFields.ARP_ADDR_SIZE_LEN);
			ArrayHelper.insertLong(bytes, MACAddress.WIDTH, Packets.ARPFields.ARP_PR_LEN_POS, Packets.ARPFields.ARP_ADDR_SIZE_LEN);
			ArrayHelper.insertLong(bytes, test(Settings.PROB_ARP_REQUEST)?Packets.ARPFields.ARP_OP_REQ_CODE:Packets.ARPFields.ARP_OP_REP_CODE, Packets.ARPFields.ARP_OP_POS, Packets.ARPFields.ARP_OP_LEN);
			ArrayHelper.insertLong(bytes, MACAddress.random(), Packets.ARPFields.ARP_S_HW_ADDR_POS, MACAddress.WIDTH);
			int srcAddress = IPAddress.random(Settings.SIM_NETWORK, Settings.SIM_NETMASK);
			ArrayHelper.insertLong(bytes, srcAddress, Packets.ARPFields.ARP_S_PR_ADDR_POS, IPAddress.WIDTH);
			ArrayHelper.insertLong(bytes, MACAddress.random(), Packets.ARPFields.ARP_T_HW_ADDR_POS, MACAddress.WIDTH);
			
			int dstAddress = srcAddress;
			int count = 0;
			// cheezy way to make sure that the source and dest address aren't the same
			while (dstAddress == srcAddress && count++ < randomRetryCount)
			{
				dstAddress = IPAddress.random(Settings.SIM_NETWORK, Settings.SIM_NETMASK);
			}
			ArrayHelper.insertLong(bytes, dstAddress, Packets.ARPFields.ARP_T_PR_ADDR_POS, IPAddress.WIDTH);
			
			return bytes;
		}
		
		private static int fakeId = 0;
		/// <summary> Generate a pseudo-random IP header.
		/// </summary>
		public static byte[] generateRandomIPHeader()
		{
			byte[] bytes = new byte[Packets.IPFields_Fields.IP_HEADER_LEN];
			
			// ipv4. WARNING: only IPv4 is currently supported
			ArrayHelper.insertLong(bytes, 0x45, Packets.IPFields_Fields.IP_VER_POS, Packets.IPFields_Fields.IP_VER_LEN);
			
			// specify no special handling flag in type of service code
			ArrayHelper.insertLong(bytes, 0x2, Packets.IPFields_Fields.IP_TOS_POS, Packets.IPFields_Fields.IP_TOS_LEN);
			
			// random packet contains only a header, no payload
			ArrayHelper.insertLong(bytes, Packets.IPFields_Fields.IP_HEADER_LEN, Packets.IPFields_Fields.IP_LEN_POS, Packets.IPFields_Fields.IP_LEN_LEN);
			
			// increment id each time a packet is generated
			ArrayHelper.insertLong(bytes, fakeId++, Packets.IPFields_Fields.IP_ID_POS, Packets.IPFields_Fields.IP_ID_LEN);
			
			// header length and flags (none specified)..
			ArrayHelper.insertLong(bytes, 0x4000, Packets.IPFields_Fields.IP_FRAG_POS, Packets.IPFields_Fields.IP_FRAG_LEN);
			
			// time-to-live
			ArrayHelper.insertLong(bytes, 0xff, Packets.IPFields_Fields.IP_TTL_POS, Packets.IPFields_Fields.IP_TTL_LEN);
			
			// protocol
			ArrayHelper.insertLong(bytes, randomIPProtocol(), Packets.IPFields_Fields.IP_CODE_POS, Packets.IPFields_Fields.IP_CODE_LEN);
			
			// checksum. todo: generate real checksum
			ArrayHelper.insertLong(bytes, 0xcccc, Packets.IPFields_Fields.IP_CSUM_POS, Packets.IPFields_Fields.IP_CSUM_LEN);
			
			int srcAddress = IPAddress.random(Settings.SIM_NETWORK, Settings.SIM_NETMASK);
			// source ip
			ArrayHelper.insertLong(bytes, srcAddress, Packets.IPFields_Fields.IP_SRC_POS, IPAddress.WIDTH);
			
			int dstAddress = srcAddress;
			int count = 0;
			// cheezy way to make sure that the source and dest address aren't the same
			while (dstAddress == srcAddress && count++ < randomRetryCount)
			{
				dstAddress = IPAddress.random(Settings.SIM_NETWORK, Settings.SIM_NETMASK);
			}
			// dest ip
			ArrayHelper.insertLong(bytes, dstAddress, Packets.IPFields_Fields.IP_DST_POS, IPAddress.WIDTH);
			
			return bytes;
		}
		
		/// <summary> Generate a pseudo-random TCP header.
		/// </summary>
		public static byte[] generateRandomUDPHeader()
		{
			byte[] bytes = new byte[Packets.UDPFields_Fields.UDP_HEADER_LEN];
			
			// source port
			ArrayHelper.insertLong(bytes, randomPort(), Packets.UDPFields_Fields.UDP_SP_POS, Packets.UDPFields_Fields.UDP_PORT_LEN);
			
			// destination port
			ArrayHelper.insertLong(bytes, randomPort(), Packets.UDPFields_Fields.UDP_DP_POS, Packets.UDPFields_Fields.UDP_PORT_LEN);
			
			// length
			ArrayHelper.insertLong(bytes, Packets.UDPFields_Fields.UDP_HEADER_LEN, Packets.UDPFields_Fields.UDP_LEN_POS, Packets.UDPFields_Fields.UDP_PORT_LEN);
			
			// checksum. todo: generate real checksum
			ArrayHelper.insertLong(bytes, 0xcccc, Packets.UDPFields_Fields.UDP_CSUM_POS, Packets.UDPFields_Fields.UDP_CSUM_LEN);
			
			return bytes;
		}
		
		/// <summary> Generate a pseudo-random TCP header.
		/// </summary>
		public static byte[] generateRandomTCPHeader()
		{
			byte[] bytes = new byte[Packets.TCPFields_Fields.TCP_HEADER_LEN];
			
			// source port
			ArrayHelper.insertLong(bytes, randomPort(), Packets.TCPFields_Fields.TCP_SP_POS, Packets.TCPFields_Fields.TCP_PORT_LEN);
			
			// destination port
			ArrayHelper.insertLong(bytes, randomPrivilegedPort(), Packets.TCPFields_Fields.TCP_DP_POS, Packets.TCPFields_Fields.TCP_PORT_LEN);
			
			// sequence number
			ArrayHelper.insertLong(bytes, 0x00000000, Packets.TCPFields_Fields.TCP_SEQ_POS, Packets.TCPFields_Fields.TCP_SEQ_LEN);
			
			// acknowledgment number
			ArrayHelper.insertLong(bytes, 0x00000000, Packets.TCPFields_Fields.TCP_ACK_POS, Packets.TCPFields_Fields.TCP_ACK_LEN);
			
			// header length and flags (0x2 is syn).
			ArrayHelper.insertLong(bytes, 0xa002, Packets.TCPFields_Fields.TCP_FLAG_POS, Packets.TCPFields_Fields.TCP_FLAG_LEN);
			
			// window size
			ArrayHelper.insertLong(bytes, 0x0000, Packets.TCPFields_Fields.TCP_WIN_POS, Packets.TCPFields_Fields.TCP_WIN_LEN);
			
			// checksum. todo: generate real checksum
			ArrayHelper.insertLong(bytes, 0xcccc, Packets.TCPFields_Fields.TCP_CSUM_POS, Packets.TCPFields_Fields.TCP_CSUM_LEN);
			
			// urgent pointer
			ArrayHelper.insertLong(bytes, 0x0000, Packets.TCPFields_Fields.TCP_URG_POS, Packets.TCPFields_Fields.TCP_URG_LEN);
			
			return bytes;
		}
		
		/// <summary> Generate a pseudo-random ICMP header.
		/// </summary>
		public static byte[] generateRandomICMPHeader()
		{
			byte[] bytes = new byte[Packets.ICMPFields.ICMP_HEADER_LEN];
			
			// code (message type)
			ArrayHelper.insertLong(bytes, randomICMPType(), Packets.ICMPFields.ICMP_CODE_POS, Packets.ICMPFields.ICMP_CODE_LEN);
			
			// subcode
			ArrayHelper.insertLong(bytes, 0x0, Packets.ICMPFields.ICMP_SUBC_POS, Packets.ICMPFields.ICMP_SUBC_LEN);
			
			// checksum. todo: generate real checksum
			ArrayHelper.insertLong(bytes, 0xcccc, Packets.ICMPFields.ICMP_CSUM_POS, Packets.ICMPFields.ICMP_CSUM_LEN);
			
			return bytes;
		}
		
		
		private static int randomRetryCount = 10;
		
		private System.String _rcsid = "$Id: HeaderGenerator.java,v 1.2 2001/06/27 02:17:33 pcharles Exp $";
	}
}