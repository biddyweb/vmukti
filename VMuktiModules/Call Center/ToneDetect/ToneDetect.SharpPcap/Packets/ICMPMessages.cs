// $Id: ICMPMessages.java,v 1.5 2004/02/24 19:21:30 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> Code constants for ICMP message types.
	/// *
	/// Taken originally from tcpdump/print-icmp.c
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.5 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/02/24 19:21:30 $
	/// 
	/// </version>
	public struct ICMPMessages_Fields{
		public readonly static int ECHO_REPLY = 0x0000;
		public readonly static int UNREACH_NET = 0x0300;
		public readonly static int UNREACH_HOST = 0x0301;
		public readonly static int UNREACH_PROTOCOL = 0x0302;
		public readonly static int UNREACH_PORT = 0x0303;
		public readonly static int UNREACH_NEEDFRAG = 0x0304;
		public readonly static int UNREACH_SRCFAIL = 0x0305;
		public readonly static int UNREACH_NET_UNKNOWN = 0x0306;
		public readonly static int UNREACH_HOST_UNKNOWN = 0x0307;
		public readonly static int UNREACH_ISOLATED = 0x0308;
		public readonly static int UNREACH_NET_PROHIB = 0x0309;
		public readonly static int UNREACH_HOST_PROHIB = 0x030a;
		public readonly static int UNREACH_TOSNET = 0x030b;
		public readonly static int UNREACH_TOSHOST = 0x030c;
		public readonly static int SOURCE_QUENCH = 0x0400;
		public readonly static int REDIRECT_NET = 0x0500;
		public readonly static int REDIRECT_HOST = 0x0501;
		public readonly static int REDIRECT_TOSNET = 0x0502;
		public readonly static int REDIRECT_TOSHOST = 0x0503;
		public readonly static int ECHO = 0x0800;
		public readonly static int ROUTER_ADVERT = 0x0900;
		public readonly static int ROUTER_SOLICIT = 0x0a00;
		public readonly static int TIME_EXCEED_INTRANS = 0x0b00;
		public readonly static int TIME_EXCEED_REASS = 0x0b01;
		public readonly static int PARAM_PROB = 0x0c01;
		public readonly static int TSTAMP = 0x0d00;
		public readonly static int TSTAMP_REPLY = 0x0e00;
		public readonly static int IREQ = 0x0f00;
		public readonly static int IREQ_REPLY = 0x1000;
		public readonly static int MASK_REQ = 0x1100;
		public readonly static int MASK_REPLY = 0x1200;
		public readonly static int LAST_MAJOR_CODE = 0x12;
	}
	public interface ICMPMessages
		{
			//UPGRADE_NOTE: Members of interface 'ICMPMessages' were extracted into structure 'ICMPMessages_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			/// <summary> Echo reply.
			/// </summary>
			/// <summary> Destination network unreachable.
			/// </summary>
			/// <summary> Destination host unreachable.
			/// </summary>
			/// <summary> Bad protocol.
			/// </summary>
			/// <summary> Bad port.
			/// </summary>
			/// <summary> IP_DF caused drop.
			/// </summary>
			/// <summary> Src route failed.
			/// </summary>
			/// <summary> Unknown network.
			/// </summary>
			/// <summary> Unknown host.
			/// </summary>
			/// <summary> Src host isolated.
			/// </summary>
			/// <summary> Network access prohibited.
			/// </summary>
			/// <summary> Host access prohibited.
			/// </summary>
			/// <summary> Bad TOS for net.
			/// </summary>
			/// <summary> Bad TOS for host.
			/// </summary>
			/// <summary> Packet lost, slow down.
			/// </summary>
			/// <summary> Shorter route to network.
			/// </summary>
			/// <summary> Shorter route to host.
			/// </summary>
			/// <summary> Shorter route for TOS and network.
			/// </summary>
			/// <summary> Shorter route for TOS and host.
			/// </summary>
			/// <summary> Echo request.
			/// </summary>
			/// <summary> router advertisement
			/// </summary>
			/// <summary> router solicitation
			/// </summary>
			/// <summary> time exceeded in transit.
			/// </summary>
			/// <summary> time exceeded in reass.
			/// </summary>
			/// <summary> ip header bad; option absent.
			/// </summary>
			/// <summary> timestamp request 
			/// </summary>
			/// <summary> timestamp reply 
			/// </summary>
			/// <summary> information request 
			/// </summary>
			/// <summary> information reply 
			/// </summary>
			/// <summary> address mask request 
			/// </summary>
			/// <summary> address mask reply 
			/// </summary>
			// marker indicating index of largest ICMP message type code
		}
}