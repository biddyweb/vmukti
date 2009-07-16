// $Id: LinkLayer.java] =v 1.5 2001/07/02 04:02:57 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001] = Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using HashMap =  System.Collections.Hashtable;
	/// <summary> Information about network link layers.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.5 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/07/02 04:02:57 $
	/// 
	/// </version>
	public class LinkLayer
	{
		/// <summary> Fetch the header length associated with various link-layer types.
		/// </summary>
		/// <param name="layerType">the link-layer code
		/// </param>
		/// <returns> the length of the header for the specified link-layer
		/// 
		/// </returns>
		public static int getLinkLayerLength(int layerType)
		{
			switch (layerType)
			{
				
				case Packets.LinkLayers.ARCNET: 
					return 6;
				
				case Packets.LinkLayers.SLIP: 
					return 16;
				
				case Packets.LinkLayers.SLIP_BSDOS: 
					return 24;
				
				case Packets.LinkLayers.NULL: 
				case Packets.LinkLayers.LOOP: 
					return 4;
				
				case Packets.LinkLayers.PPP: 
				case Packets.LinkLayers.CHDLC: 
				case Packets.LinkLayers.PPP_SERIAL: 
					return 4;
				
				case Packets.LinkLayers.PPP_BSDOS: 
					return 24;
				
				case Packets.LinkLayers.FDDI: 
					return 21;
				
				case Packets.LinkLayers.IEEE802_11: 
					return 22;
				
				case Packets.LinkLayers.ATM_RFC1483: 
					return 8;
				
				case Packets.LinkLayers.RAW: 
					return 0;
				
				case Packets.LinkLayers.ATM_CLIP: 
					return 8;
				
				case Packets.LinkLayers.LINUX_SLL: 
					return 16;
				
				case Packets.LinkLayers.EN10MB: 
				default: 
					return 14;
				}
		}
		
		/// <summary> Fetch the offset into the link-layer header where the protocol code
		/// can be found. Returns -1 if there is no embedded protocol code.
		/// </summary>
		/// <param name="layerType">the link-layer code
		/// </param>
		/// <returns> the offset in bytes
		/// 
		/// </returns>
		public static int getProtoOffset(int layerType)
		{
			switch (layerType)
			{
				
				case Packets.LinkLayers.ARCNET: 
					return 2;
				
				case Packets.LinkLayers.SLIP: 
					return - 1;
				
				case Packets.LinkLayers.SLIP_BSDOS: 
					return - 1;
				
				case Packets.LinkLayers.NULL: 
				case Packets.LinkLayers.LOOP: 
					return 0;
				
				case Packets.LinkLayers.PPP: 
				case Packets.LinkLayers.CHDLC: 
				case Packets.LinkLayers.PPP_SERIAL: 
					return 2;
				
				case Packets.LinkLayers.PPP_BSDOS: 
					return 5;
				
				case Packets.LinkLayers.FDDI: 
					return 13;
				
				case Packets.LinkLayers.IEEE802_11: 
					return 14;
				
				case Packets.LinkLayers.ATM_RFC1483: 
					return 6;
				
				case Packets.LinkLayers.RAW: 
					return - 1;
				
				case Packets.LinkLayers.ATM_CLIP: 
					return 6;
				
				case Packets.LinkLayers.LINUX_SLL: 
					return 14;
				
				case Packets.LinkLayers.EN10MB: 
				default: 
					return 12;
				}
		}
		
		/// <summary> Fetch a link-layer type description.
		/// </summary>
		/// <param name="code">the code associated with the description.
		/// </param>
		/// <returns> a description of the link-layer type.
		/// 
		/// </returns>
		public static System.String getDescription(int code)
		{
			System.Int32 c = code;
			if (descriptions.ContainsKey(c))
				return (System.String) descriptions[c];
			else
				return "unknown";
		}
		
		/// <summary> 'Human-readable' link-layer type descriptions.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'descriptions' was moved to static method 'Packets.LinkLayer'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static HashMap descriptions;
		
		
		private System.String _rcsid = "$Id: LinkLayer.java] =v 1.5 2001/07/02 04:02:57 pcharles Exp $";
		static LinkLayer()
		{
			descriptions = new HashMap();
			{
				descriptions[Packets.LinkLayers.NULL] = "no link-layer encapsulation";
				descriptions[Packets.LinkLayers.EN10MB] = "10/100Mb ethernet";
				descriptions[Packets.LinkLayers.EN3MB] = "3Mb experimental ethernet";
				descriptions[Packets.LinkLayers.AX25] = "AX.25 amateur radio";
				descriptions[Packets.LinkLayers.PRONET] = "proteon pronet token ring";
				descriptions[Packets.LinkLayers.CHAOS] = "chaos";
				descriptions[Packets.LinkLayers.IEEE802] = "IEEE802 network";
				descriptions[Packets.LinkLayers.ARCNET] = "ARCNET";
				descriptions[Packets.LinkLayers.SLIP] = "serial line IP";
				descriptions[Packets.LinkLayers.PPP] = "point-to-point protocol";
				descriptions[Packets.LinkLayers.FDDI] = "FDDI";
				descriptions[Packets.LinkLayers.ATM_RFC1483] = "LLC/SNAP encapsulated ATM";
				descriptions[Packets.LinkLayers.RAW] = "raw IP";
				descriptions[Packets.LinkLayers.SLIP_BSDOS] = "BSD SLIP";
				descriptions[Packets.LinkLayers.PPP_BSDOS] = "BSD PPP";
				descriptions[Packets.LinkLayers.ATM_CLIP] = "IP over ATM";
				descriptions[Packets.LinkLayers.PPP_SERIAL] = "PPP over HDLC";
				descriptions[Packets.LinkLayers.CHDLC] = "Cisco HDLC";
				descriptions[Packets.LinkLayers.IEEE802_11] = "802.11 wireless";
				descriptions[Packets.LinkLayers.LOOP] = "OpenBSD loopback";
				descriptions[Packets.LinkLayers.LINUX_SLL] = "Linux cooked sockets";
				descriptions[Packets.LinkLayers.UNKNOWN] = "unknown link-layer type";
			}
		}
	}
}