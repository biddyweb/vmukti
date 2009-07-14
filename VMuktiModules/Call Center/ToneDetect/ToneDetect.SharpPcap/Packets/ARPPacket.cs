// $Id: ARPPacket.java,v 1.14 2004/05/05 23:14:45 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	using AnsiEscapeSequences = Packets.Util.AnsiEscapeSequences_Fields;
	using ArrayHelper = Packets.Util.ArrayHelper;
	using Timeval = Packets.Util.Timeval;
	/// <summary> An ARP protocol packet.
	/// <p>
	/// Extends an ethernet packet, adding ARP header information and an ARP 
	/// data payload. 
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.14 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public class ARPPacket:EthernetPacket/* ,System.Runtime.Serialization.ISerializable*/
	{
		/// <summary> Fetch the hardware source address.
		/// </summary>
		override public System.String SourceHwAddress
		{
			get
			{
				return MACAddress.extract(Packets.ARPFields.ARP_S_HW_ADDR_POS, header);
			}
			
		}
		/// <summary> Fetch the hardware destination address.
		/// </summary>
		override public System.String DestinationHwAddress
		{
			get
			{
				return MACAddress.extract(Packets.ARPFields.ARP_T_HW_ADDR_POS, header);
			}
			
		}
		/// <summary> Fetch the proto sender address.
		/// </summary>
		virtual public System.String SourceProtoAddress
		{
			get
			{
				return IPAddress.extract(Packets.ARPFields.ARP_S_PR_ADDR_POS, header);
			}
			
		}
		/// <summary> Fetch the proto sender address.
		/// </summary>
		virtual public System.String DestinationProtoAddress
		{
			get
			{
				return IPAddress.extract(Packets.ARPFields.ARP_T_PR_ADDR_POS, header);
			}
			
		}
		/// <summary> Fetch the operation code.
		/// Usually one of ARPFields.{ARP_OP_REQ_CODE, ARP_OP_REP_CODE}.
		/// </summary>
		virtual public int Operation
		{
			get
			{
				return ArrayHelper.extractInteger(header, Packets.ARPFields.ARP_OP_POS, Packets.ARPFields.ARP_OP_LEN);
			}
			
		}
		/// <summary> Fetch the arp header, excluding arp data payload.
		/// </summary>
		virtual public byte[] ARPHeader
		{
			get
			{
				return header;
			}
			
		}
		/// <summary> Fetch data portion of the arp header.
		/// </summary>
		virtual public byte[] ARPData
		{
			get
			{
				return data;
			}
			
		}
		/// <summary> Fetch the arp header, excluding arp data payload.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return ARPHeader;
			}
			
		}
		/// <summary> Fetch data portion of the arp header.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return ARPData;
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.PURPLE;
			}
			
		}
		/// <summary> Create a new ARP packet.
		/// </summary>
		public ARPPacket(int lLen, byte[] bytes):base(lLen, bytes)
		{
			
			this.header = PacketEncoding.extractHeader(lLen, Packets.ARPFields.ARP_HEADER_LEN, bytes);
			this.data = PacketEncoding.extractData(lLen, Packets.ARPFields.ARP_HEADER_LEN, bytes);
		}
		
		/// <summary> Create a new ARP packet.
		/// </summary>
		public ARPPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		
		
		
		
		
		
		
		
		
		/// <summary> Convert this ARP packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this ARP packet.
		/// </summary>
		/// <param name="colored">whether or not the string should contain ansi
		/// color escape sequences.
		/// 
		/// </param>
		public override System.String toColoredString(bool colored)
		{
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();
			buffer.Append('[');
			if (colored)
				buffer.Append(Color);
			buffer.Append("ARPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append(Operation == Packets.ARPFields.ARP_OP_REQ_CODE?"request":"reply");
			buffer.Append(' ');
			buffer.Append(SourceHwAddress + " -> " + DestinationHwAddress);
			buffer.Append(", ");
			buffer.Append(SourceProtoAddress + " -> " + DestinationProtoAddress);
			buffer.Append(" l=" + header.Length + "," + data.Length);
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		
		/// <summary> ARP header.
		/// </summary>
		internal byte[] header;
		
		/// <summary> ARP data.
		/// </summary>
		internal byte[] data;
		
		
		new private System.String _rcsid = "$Id: ARPPacket.java,v 1.14 2004/05/05 23:14:45 pcharles Exp $";
	}
}