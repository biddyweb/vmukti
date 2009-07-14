// $Id: IGMPMessages.java,v 1.1 2001/07/30 00:00:02 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> Code constants for IGMP message types.
	/// *
	/// From RFC #2236.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/07/30 00:00:02 $
	/// 
	/// </version>
	public struct IGMPMessages_Fields{
		public readonly static int QUERY = 0x11;
		public readonly static int V1_REPORT = 0x12;
		public readonly static int V2_REPORT = 0x16;
		public readonly static int LEAVE = 0x17;
	}
	public interface IGMPMessages
		{
			//UPGRADE_NOTE: Members of interface 'IGMPMessages' were extracted into structure 'IGMPMessages_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			/// <summary> membership query.
			/// </summary>
			/// <summary> v1 membership report.
			/// </summary>
			/// <summary> v2 membership report.
			/// </summary>
			/// <summary> Leave group.
			/// </summary>
		}
}