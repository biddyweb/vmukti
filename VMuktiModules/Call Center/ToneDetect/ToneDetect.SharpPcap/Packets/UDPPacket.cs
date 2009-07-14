// $Id: UDPPacket.java,v 1.18 2004/05/05 23:14:45 pcharles Exp $

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
	/// <summary> A UDP packet.
	/// <p>
	/// Extends an IP packet, adding a UDP header and UDP data payload.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.18 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public class UDPPacket:IPPacket, UDPFields/* ,System.Runtime.Serialization.ISerializable*/
	{
		/// <summary> Fetch the port number on the source host.
		/// </summary>
		virtual public int SourcePort
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.UDPFields_Fields.UDP_SP_POS, Packets.UDPFields_Fields.UDP_PORT_LEN);
			}
			
		}
		/// <summary> Fetch the port number on the target host.
		/// </summary>
		virtual public int DestinationPort
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.UDPFields_Fields.UDP_DP_POS, Packets.UDPFields_Fields.UDP_PORT_LEN);
			}
			
		}
		/// <summary> Fetch the total length of the UDP packet, including header and
		/// data payload, in bytes.
		/// </summary>
		override public int Length
		{
			get
			{
				// should produce the same value as header.length + data.length
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.UDPFields_Fields.UDP_LEN_POS, Packets.UDPFields_Fields.UDP_LEN_LEN);
			}
			
		}
		/// <summary> Fetch the header checksum.
		/// </summary>
		virtual public int UDPChecksum
		{
			get
			{
				return ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.UDPFields_Fields.UDP_CSUM_POS, Packets.UDPFields_Fields.UDP_CSUM_LEN);
			}
			
		}
		/// <summary> Fetch the header checksum.
		/// </summary>
		override public int Checksum
		{
			get
			{
				return UDPChecksum;
			}
			
		}
		/// 
		/// <summary> Fetch the UDP header a byte array.
		/// </summary>
		virtual public byte[] UDPHeader
		{
			get
			{
				if (_udpHeaderBytes == null)
				{
					_udpHeaderBytes = PacketEncoding.extractHeader(_ipOffset, Packets.UDPFields_Fields.UDP_HEADER_LEN, _bytes);
				}
				return _udpHeaderBytes;
			}
			
		}
		/// <summary> Fetch the UDP header as a byte array.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return UDPHeader;
			}
			
		}
		/// <summary> Fetch the UDP data as a byte array.
		/// </summary>
		virtual public byte[] UDPData
		{
			get
			{
				if (_udpDataBytes == null)
				{
					// set data length based on info in headers (note: tcpdump
					//  can return extra junk bytes which bubble up to here
					int tmpLen = _bytes.Length - _ipOffset - Packets.UDPFields_Fields.UDP_HEADER_LEN;
					_udpDataBytes = PacketEncoding.extractData(_ipOffset, Packets.UDPFields_Fields.UDP_HEADER_LEN, _bytes, tmpLen);
				}
				return _udpDataBytes;
			}
			
		}
		/// <summary> Fetch the UDP data as a byte array.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return UDPData;
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.LIGHT_GREEN;
			}
			
		}
		/// <summary> Create a new UDP packet.
		/// </summary>
		public UDPPacket(int lLen, byte[] bytes):base(lLen, bytes)
		{
			int i;
			i = 9;
		}
		
		/// <summary> Create a new UDP packet.
		/// </summary>
		public UDPPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		
		
		
		
		
		private byte[] _udpHeaderBytes = null;
		
		
		private byte[] _udpDataBytes = null;
		
		
		/// <summary> Convert this UDP packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this UDP packet.
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
			buffer.Append("UDPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append(SourceAddress);
			buffer.Append('.');
			buffer.Append(IPPort.getName(SourcePort));
			buffer.Append(" -> ");
			buffer.Append(DestinationAddress);
			buffer.Append('.');
			buffer.Append(IPPort.getName(DestinationPort));
			buffer.Append(" l=" + Packets.UDPFields_Fields.UDP_HEADER_LEN + "," + (Length - Packets.UDPFields_Fields.UDP_HEADER_LEN));
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		
		
		private System.String _rcsid = "$Id: UDPPacket.java,v 1.18 2004/05/05 23:14:45 pcharles Exp $";
	}
}