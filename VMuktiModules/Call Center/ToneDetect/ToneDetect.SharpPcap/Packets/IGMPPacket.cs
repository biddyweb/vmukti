// $Id: IGMPPacket.java,v 1.7 2004/05/05 23:14:45 pcharles Exp $

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
	/// <summary> An IGMP packet.
	/// <p>
	/// Extends an IP packet, adding an IGMP header and IGMP data payload.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.7 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public class IGMPPacket:IPPacket/*,System.Runtime.Serialization.ISerializable*/
	{
		/// <summary> Fetch the IGMP header a byte array.
		/// </summary>
		virtual public byte[] IGMPHeader
		{
			get
			{
				if (_igmpHeaderBytes == null)
				{
					_igmpHeaderBytes = PacketEncoding.extractHeader(_ethOffset, Packets.IGMPFields.IGMP_HEADER_LEN, _bytes);
				}
				return _igmpHeaderBytes;
			}
			
		}
		/// <summary> Fetch the IGMP header as a byte array.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return IGMPHeader;
			}
			
		}
		/// <summary> Fetch the IGMP data as a byte array.
		/// </summary>
		virtual public byte[] IGMPData
		{
			get
			{
				if (_igmpDataBytes == null)
				{
					// set data length based on info in headers (note: tcpdump
					//  can return extra junk bytes which bubble up to here
					int dataLen = _bytes.Length - _ethOffset - Packets.IGMPFields.IGMP_HEADER_LEN;
					
					_igmpDataBytes = PacketEncoding.extractData(_ethOffset, Packets.IGMPFields.IGMP_HEADER_LEN, _bytes, dataLen);
				}
				return _igmpDataBytes;
			}
			
		}
		/// <summary> Fetch the IGMP data as a byte array.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return IGMPData;
			}
			
		}
		/// <summary> Fetch the IGMP message type, including subcode. Return value can be 
		/// used with IGMPMessage.getDescription().
		/// </summary>
		/// <returns> a 2-byte value containing the message type in the high byte
		/// and the message type subcode in the low byte.
		/// 
		/// </returns>
		virtual public int MessageType
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.IGMPFields.IGMP_CODE_POS, Packets.IGMPFields.IGMP_CODE_LEN);
			}
			
		}
		/// <summary> Fetch the IGMP max response time.
		/// </summary>
		virtual public int MaxResponseTime
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.IGMPFields.IGMP_MRSP_POS, Packets.IGMPFields.IGMP_MRSP_LEN);
			}
			
		}
		/// <summary> Fetch the IGMP header checksum.
		/// </summary>
		virtual public int IGMPChecksum
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.IGMPFields.IGMP_CSUM_POS, Packets.IGMPFields.IGMP_CSUM_LEN);
			}
			
		}
		/// <summary> Fetch the IGMP header checksum.
		/// </summary>
		override public int Checksum
		{
			get
			{
				return IGMPChecksum;
			}
			
		}
		/// <summary> Fetch the IGMP group address.
		/// </summary>
		virtual public System.String GroupAddress
		{
			get
			{
				return IPAddress.extract(_ipOffset + Packets.IGMPFields.IGMP_GADDR_POS, _bytes);
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.BROWN;
			}
			
		}
		public IGMPPacket(int lLen, byte[] bytes):base(lLen, bytes)
		{
		}
		
		public IGMPPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		private byte[] _igmpHeaderBytes = null;
		
		
		private byte[] _igmpDataBytes = null;
		
		
		
		
		
		
		
		
		/// <summary> Convert this IGMP packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this IGMP packet.
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
			buffer.Append("IGMPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append(IGMPMessage.getDescription(MessageType));
			buffer.Append(", ");
			buffer.Append(GroupAddress + ": ");
			buffer.Append(SourceAddress + " -> " + DestinationAddress);
			buffer.Append(" l=" + Packets.IGMPFields.IGMP_HEADER_LEN + "," + (_bytes.Length - _ipOffset - Packets.IGMPFields.IGMP_HEADER_LEN));
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		
		
		private System.String _rcsid = "$Id: IGMPPacket.java,v 1.7 2004/05/05 23:14:45 pcharles Exp $";
	}
}