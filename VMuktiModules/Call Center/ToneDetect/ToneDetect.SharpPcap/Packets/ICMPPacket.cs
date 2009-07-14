// $Id: ICMPPacket.java,v 1.19 2004/05/05 23:14:45 pcharles Exp $

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
	/// <summary> An ICMP packet.
	/// <p>
	/// Extends an IP packet, adding an ICMP header and ICMP data payload.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.19 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public class ICMPPacket:IPPacket/*,System.Runtime.Serialization.ISerializable*/
	{
		/// <summary> Fetch the ICMP header a byte array.
		/// </summary>
		virtual public byte[] ICMPHeader
		{
			get
			{
				if (_icmpHeaderBytes == null)
				{
					_icmpHeaderBytes = PacketEncoding.extractHeader(_ipOffset, Packets.ICMPFields.ICMP_HEADER_LEN, _bytes);
				}
				return _icmpHeaderBytes;
			}
			
		}
		/// <summary> Fetch the ICMP header as a byte array.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return ICMPHeader;
			}
			
		}
		/// 
		/// <summary> Fetch the ICMP data as a byte array.
		/// </summary>
		virtual public byte[] ICMPData
		{
			get
			{
				if (_icmpDataBytes == null)
				{
					// set data length based on info in headers (note: tcpdump
					//  can return extra junk bytes which bubble up to here
					int dataLen = _bytes.Length - _ipOffset - Packets.ICMPFields.ICMP_HEADER_LEN;
					
					_icmpDataBytes = PacketEncoding.extractData(_ipOffset, Packets.ICMPFields.ICMP_HEADER_LEN, _bytes, dataLen);
				}
				return _icmpDataBytes;
			}
			
		}
		/// <summary> Fetch the ICMP data as a byte array.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return ICMPData;
			}
			
		}
		/// <summary> Fetch the ICMP message type, including subcode. Return value can be 
		/// used with ICMPMessage.getDescription().
		/// </summary>
		/// <returns> a 2-byte value containing the message type in the high byte
		/// and the message type subcode in the low byte.
		/// 
		/// </returns>
		virtual public int MessageCode
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.ICMPFields.ICMP_CODE_POS, Packets.ICMPFields.ICMP_CODE_LEN * 2);
			}
			
		}
		/// <summary> Fetch the ICMP message type code. Formerly .getMessageType().
		/// </summary>
		virtual public int MessageMajorCode
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.ICMPFields.ICMP_CODE_POS, Packets.ICMPFields.ICMP_CODE_LEN);
			}
			
		}
		/// <deprecated> use getMessageMajorCode().
		/// 
		/// </deprecated>
		virtual public int MessageType
		{
			get
			{
				return MessageMajorCode;
			}
			
		}
		/// <summary> Fetch the ICMP message subcode.
		/// </summary>
		virtual public int MessageMinorCode
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.ICMPFields.ICMP_CODE_POS + 1, Packets.ICMPFields.ICMP_CODE_LEN);
			}
			
		}
		/// <summary> Fetch the ICMP header checksum.
		/// </summary>
		override public int Checksum
		{
			get
			{
				return ICMPChecksum;
			}
			
		}
		/// <summary> Fetch the ICMP header checksum.
		/// </summary>
		virtual public int ICMPChecksum
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.ICMPFields.ICMP_CSUM_POS, Packets.ICMPFields.ICMP_CSUM_LEN);
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.LIGHT_BLUE;
			}
			
		}
		public ICMPPacket(int lLen, byte[] bytes):base(lLen, bytes)
		{
		}
		
		public ICMPPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		private byte[] _icmpHeaderBytes = null;
		
		
		private byte[] _icmpDataBytes = null;
		
		
		
		
		
		
		
		
		/// <summary> Convert this ICMP packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this ICMP packet.
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
			buffer.Append("ICMPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append(ICMPMessage.getDescription(MessageCode));
			buffer.Append(", ");
			buffer.Append(SourceAddress + " -> " + DestinationAddress);
			buffer.Append(" l=" + Packets.ICMPFields.ICMP_HEADER_LEN + "," + (_bytes.Length - _ipOffset - Packets.ICMPFields.ICMP_HEADER_LEN));
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		
		private System.String _rcsid = "$Id: ICMPPacket.java,v 1.19 2004/05/05 23:14:45 pcharles Exp $";
	}
}