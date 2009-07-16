// $Id: ICMPMessage.java,v 1.4 2001/06/20 06:24:37 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using HashMap = System.Collections.Hashtable;
	/// <summary> ICMP message utility class.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.4 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/20 06:24:37 $
	/// 
	/// </version>
	public class ICMPMessage : ICMPMessages
	{
		/// <summary> Fetch an ICMP message.
		/// </summary>
		/// <param name="code">the code associated with the message.
		/// </param>
		/// <returns> a message describing the significance of the ICMP code.
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
		
		/// <summary> 'Human-readable' ICMP messages.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'messages' was moved to static method 'Packets.ICMPMessage'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static HashMap messages;
		
		
		private System.String _rcsid = "$Id: ICMPMessage.java,v 1.4 2001/06/20 06:24:37 pcharles Exp $";
		static ICMPMessage()
		{
			messages = new HashMap();
			{
				messages[Packets.ICMPMessages_Fields.ECHO_REPLY]= "echo reply";
				messages[Packets.ICMPMessages_Fields.ECHO]= "echo request";
				messages[Packets.ICMPMessages_Fields.UNREACH_NET]= "net unreachable";
				messages[Packets.ICMPMessages_Fields.UNREACH_HOST]= "host unreachable";
				messages[Packets.ICMPMessages_Fields.UNREACH_PROTOCOL]= "bad protocol";
				messages[Packets.ICMPMessages_Fields.UNREACH_PORT]= "port unreachable";
				messages[Packets.ICMPMessages_Fields.UNREACH_NEEDFRAG]= "ip_df drop";
				messages[Packets.ICMPMessages_Fields.UNREACH_SRCFAIL] = "source route failed";
				messages[Packets.ICMPMessages_Fields.UNREACH_NET_UNKNOWN] = "unknown network";
				messages[Packets.ICMPMessages_Fields.UNREACH_HOST_UNKNOWN] = "unknown host";
				messages[Packets.ICMPMessages_Fields.UNREACH_ISOLATED] = "source host isolated";
				messages[Packets.ICMPMessages_Fields.UNREACH_NET_PROHIB] = "net access prohibited";
				messages[Packets.ICMPMessages_Fields.UNREACH_HOST_PROHIB] = "host access prohibited";
				messages[Packets.ICMPMessages_Fields.UNREACH_TOSNET] = "tos for net invalid";
				messages[Packets.ICMPMessages_Fields.UNREACH_TOSHOST] = "tos for host invalid";
				messages[Packets.ICMPMessages_Fields.SOURCE_QUENCH] = "packet lost";
				messages[Packets.ICMPMessages_Fields.REDIRECT_NET] = "redirect to network";
				messages[Packets.ICMPMessages_Fields.REDIRECT_HOST] = "redirect to host";
				messages[Packets.ICMPMessages_Fields.REDIRECT_TOSNET] = "tos redirect to network";
				messages[Packets.ICMPMessages_Fields.REDIRECT_TOSHOST] = "tos redirect to host";
				messages[Packets.ICMPMessages_Fields.ROUTER_ADVERT] = "router advert";
				messages[Packets.ICMPMessages_Fields.ROUTER_SOLICIT] = "router solicit";
				messages[Packets.ICMPMessages_Fields.TIME_EXCEED_INTRANS] = "transit time exceeded";
				messages[Packets.ICMPMessages_Fields.TIME_EXCEED_REASS] = "reass time exceeded";
				messages[Packets.ICMPMessages_Fields.PARAM_PROB] = "bad ip header";
				messages[Packets.ICMPMessages_Fields.TSTAMP] = "timestamp request";
				messages[Packets.ICMPMessages_Fields.TSTAMP_REPLY] = "timestamp reply";
				messages[Packets.ICMPMessages_Fields.IREQ] = "information request";
				messages[Packets.ICMPMessages_Fields.IREQ_REPLY] = "information reply";
				messages[Packets.ICMPMessages_Fields.MASK_REQ] = "address mask request";
				messages[Packets.ICMPMessages_Fields.MASK_REPLY] = "address mask reply";
			}
		}
	}
}