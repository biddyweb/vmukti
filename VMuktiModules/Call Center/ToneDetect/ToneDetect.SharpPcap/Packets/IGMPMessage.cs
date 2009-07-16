// $Id: IGMPMessage.java,v 1.1 2001/07/30 00:00:02 pcharles Exp $

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
	/// <summary> IGMP message utility class.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/07/30 00:00:02 $
	/// 
	/// </version>
	public class IGMPMessage : IGMPMessages
	{
		/// <summary> Fetch an IGMP message.
		/// </summary>
		/// <param name="code">the code associated with the message.
		/// </param>
		/// <returns> a message describing the significance of the IGMP code.
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
		
		/// <summary> 'Human-readable' IGMP messages.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'messages' was moved to static method 'Packets.IGMPMessage'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static HashMap messages;
		
		
		private System.String _rcsid = "$Id: IGMPMessage.java,v 1.1 2001/07/30 00:00:02 pcharles Exp $";
		static IGMPMessage()
		{
			messages = new HashMap();
			{
				messages[Packets.IGMPMessages_Fields.LEAVE] = "leave group";
				messages[Packets.IGMPMessages_Fields.V1_REPORT] = "v1 membership report";
				messages[Packets.IGMPMessages_Fields.V2_REPORT] = "v2 membership report";
				messages[Packets.IGMPMessages_Fields.QUERY] = "membership query";
			}
		}
	}
}