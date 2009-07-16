// $Id: IPVersions.java,v 1.1 2001/05/23 02:42:22 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	
	
	/// <summary> Code constants for internet protocol versions.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/05/23 02:42:22 $
	/// 
	/// </version>
	public struct IPVersions_Fields{
		public readonly static int IPV4 = 4;
		public readonly static int IPV6 = 6;
	}
	public interface IPVersions
		{
			//UPGRADE_NOTE: Members of interface 'IPVersions' were extracted into structure 'IPVersions_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
			/// <summary> Internet protocol version 4.
			/// </summary>
			/// <summary> Internet protocol version 6.
			/// </summary>
		}
}