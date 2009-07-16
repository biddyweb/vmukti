// $Id: Packet.java,v 1.8 2004/05/05 23:14:45 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using Timeval = Packets.Util.Timeval;
	/// <summary> A network packet.
	/// <p>
	/// This class currently contains no implementation because only ethernet 
	/// is supported. In other words, all instances of packets returned by 
	/// packet factory will always be at least as specific as EthernetPacket.
	/// <p>
	/// On large ethernet networks, I sometimes see packets which don't have 
	/// link-level ethernet headers. If and when I figure out what these are, 
	/// maybe this class will be the root node of a packet hierarchy derived 
	/// from something other than ethernet.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.8 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public abstract class Packet/* : System.Runtime.Serialization.ISerializable*/
	{
		/// <summary> Fetch data portion of the packet.
		/// </summary>
		virtual public byte[] Header
		{
			get
			{
				return null;
			}
			
		}
		/// <summary> Fetch data portion of the packet.
		/// </summary>
		virtual public byte[] Data
		{
			get
			{
				return null;
			}
			
		}
		virtual public System.String Color
		{
			get
			{
				return "";
			}
			
		}
		virtual public Timeval Timeval
		{
			get
			{
				return null;
			}
			
		}
		public virtual System.String toColoredString(bool colored)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			return ToString();
		}
		
		public abstract byte[] Bytes
		{
			get;
		}

		public PcapHeader PcapHeader
		{
			get
			{
				if(m_pcapHdr==null)
					m_pcapHdr=new PcapHeader();
				return m_pcapHdr;
			}
			set{m_pcapHdr=value;}
		}
		private PcapHeader m_pcapHdr;
		private System.String _rcsid = "$Id: Packet.java,v 1.8 2004/05/05 23:14:45 pcharles Exp $";
	}
}