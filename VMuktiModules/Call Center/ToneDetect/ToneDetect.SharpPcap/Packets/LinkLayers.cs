// $Id: LinkLayers.java,v 1.3 2001/07/02 02:45:46 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	

	public struct LinkLayers
		{
		public const int NULL = 0;
		public const int EN10MB = 1;
		public const int EN3MB = 2;
		public const int AX25 = 3;
		public const int PRONET = 4;
		public const int CHAOS = 5;
		public const int IEEE802 = 6;
		public const int ARCNET = 7;
		public const int SLIP = 8;
		public const int PPP = 9;
		public const int FDDI = 10;
		public const int ATM_RFC1483 = 11;
		public const int RAW = 12;
		public const int SLIP_BSDOS = 15;
		public const int PPP_BSDOS = 16;
		public const int ATM_CLIP = 19;
		public const int PPP_SERIAL = 50;
		public const int CHDLC = 104;
		public const int IEEE802_11 = 105;
		public const int LOOP = 108;
		public const int LINUX_SLL = 113;
		public const int UNKNOWN = - 1;
			//UPGRADE_NOTE: Members of interface 'LinkLayers' were extracted into structure 'LinkLayers'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			/// <summary> no link-layer encapsulation 
			/// </summary>
			/// <summary> Ethernet (10Mb) 
			/// </summary>
			/// <summary> Experimental Ethernet (3Mb) 
			/// </summary>
			/// <summary> Amateur Radio AX.25 
			/// </summary>
			/// <summary> Proteon ProNET Token Ring 
			/// </summary>
			/// <summary> Chaos 
			/// </summary>
			/// <summary> IEEE 802 Networks 
			/// </summary>
			/// <summary> ARCNET 
			/// </summary>
			/// <summary> Serial Line IP 
			/// </summary>
			/// <summary> Point-to-point Protocol 
			/// </summary>
			/// <summary> FDDI 
			/// </summary>
			/// <summary> LLC/SNAP encapsulated atm 
			/// </summary>
			/// <summary> raw IP 
			/// </summary>
			/// <summary> BSD Slip.
			/// </summary>
			/// <summary> BSD PPP.
			/// </summary>
			/// <summary> IP over ATM.
			/// </summary>
			/// <summary> PPP over HDLC.
			/// </summary>
			/// <summary> Cisco HDLC.
			/// </summary>
			/// <summary> IEEE 802.11 wireless.
			/// </summary>
			/// <summary> OpenBSD loopback.
			/// </summary>
			/// <summary> Linux cooked sockets.
			/// </summary>
			/// <summary> unknown link-layer type
			/// </summary>
		}
}