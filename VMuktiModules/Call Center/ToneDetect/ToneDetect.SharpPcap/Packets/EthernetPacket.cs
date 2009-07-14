// $Id: EthernetPacket.java,v 1.19 2004/05/05 23:14:45 pcharles Exp $

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
	/// <summary> An ethernet packet.
	/// <p>
	/// Contains link-level header and data payload encapsulated by an ethernet
	/// packet.
	/// <p>
	/// There are currently two subclasses. IP and ARP protocols are supported.
	/// IPPacket extends with ip header and data information.
	/// ARPPacket extends with hardware and protocol addresses.
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
	public class EthernetPacket:Packet /*,System.Runtime.Serialization.ISerializable*/
	{
		/// 
		/// <summary> Fetch the ethernet header length in bytes.
		/// </summary>
		virtual public int EthernetHeaderLength
		{
			get
			{
				return _ethernetHeaderLength;
			}
			
		}
		/// 
		/// <summary> Fetch the packet ethernet header length.
		/// </summary>
		virtual public int HeaderLength
		{
			get
			{
				return EthernetHeaderLength;
			}
			
		}
		/// <summary> Fetch the ethernet header as a byte array.
		/// </summary>
		virtual public byte[] EthernetHeader
		{
			get
			{
				if (_ethernetHeaderBytes == null)
				{
					_ethernetHeaderBytes = PacketEncoding.extractHeader(0, EthernetHeaderLength, _bytes);
				}
				return _ethernetHeaderBytes;
			}
			
		}
		/// <summary> Fetch the ethernet header as a byte array.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return EthernetHeader;
			}
			
		}
		/// <summary> Fetch the ethernet data as a byte array.
		/// </summary>
		virtual public byte[] EthernetData
		{
			get
			{
				if (_ethernetDataBytes == null)
				{
					_ethernetDataBytes = PacketEncoding.extractData(0, EthernetHeaderLength, _bytes);
				}
				return _ethernetDataBytes;
			}
			
		}
		/// <summary> Fetch the ethernet data as a byte array.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return EthernetData;
			}
			
		}
		/// <summary> Fetch the IP address of the host where the packet originated from.
		/// </summary>
		virtual public System.String SourceHwAddress
		{
			get
			{
				if (_sourceHwAddress == null)
				{
					_sourceHwAddress = MACAddress.extract(Packets.EthernetFields.ETH_SRC_POS, _bytes);
				}
				return _sourceHwAddress;
			}
			
		}
		/// <summary> Fetch the IP address of the host where the packet originated from.
		/// </summary>
		virtual public System.String DestinationHwAddress
		{
			get
			{
				if (_destinationHwAddress == null)
				{
					_destinationHwAddress = MACAddress.extract(Packets.EthernetFields.ETH_DST_POS, _bytes);
				}
				return _destinationHwAddress;
			}
			
		}
		/// <summary> Fetch the ethernet protocol.
		/// </summary>
		virtual public int EthernetProtocol
		{
			get
			{
				if (!_etherProtocolSet)
				{
					_etherProtocol = ArrayHelper.extractInteger(_bytes, Packets.EthernetFields.ETH_CODE_POS, Packets.EthernetFields.ETH_CODE_LEN);
					_etherProtocolSet = true;
				}
				return _etherProtocol;
			}
			
		}
		/// <summary> Fetch the ethernet protocol.
		/// </summary>
		virtual public int Protocol
		{
			get
			{
				return EthernetProtocol;
			}
			
		}
		/// <summary> Fetch the timeval containing the time the packet arrived on the 
		/// device where it was captured.
		/// </summary>
		override public Timeval Timeval
		{
			get
			{
				return _timeval;
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.DARK_GRAY;
			}
			
		}
		// store the data here, all subclasses can offset into this
		protected internal byte[] _bytes;
		
		// offset from beginning of byte array where the data payload 
		// (i.e. IP packet) starts. The size of the ethernet frame header.
		protected internal int _ethOffset;
		
		// time that the packet was captured off the wire
		protected internal Timeval _timeval;
		
		
		/// <summary> Construct a new ethernet packet.
		/// <p>
		/// For the purpose of jpcap, when the type of ethernet packet is 
		/// recognized as a protocol for which a class exists network library, 
		/// then a more specific class like IPPacket or ARPPacket is instantiated.
		/// The subclass can always be cast into a more generic form.
		/// </summary>
		public EthernetPacket(int lLen, byte[] bytes)
		{
			_bytes = bytes;
			_ethernetHeaderLength = lLen;
			_ethOffset = lLen;
		}
		
		/// <summary> Construct a new ethernet packet, including the capture time.
		/// </summary>
		public EthernetPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		// set in constructor
		private int _ethernetHeaderLength;
		
		
		private byte[] _ethernetHeaderBytes = null;
		
		
		private byte[] _ethernetDataBytes = null;
		
		
		private System.String _sourceHwAddress = null;
		
		private System.String _destinationHwAddress = null;
		
		private int _etherProtocol;
		private bool _etherProtocolSet = false;
		
		
		
		/// <summary> Convert this ethernet packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this ethernet packet.
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
			buffer.Append("EthernetPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append(SourceHwAddress + " -> " + DestinationHwAddress);
			buffer.Append(" proto=0x" + System.Convert.ToString(Protocol, 16));
			buffer.Append(" l=" + EthernetHeaderLength); // + "," + data.length);
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		public override byte[] Bytes
		{
			get
			{
				return _bytes;
			}
		}

		
		new private System.String _rcsid = "$Id: EthernetPacket.java,v 1.19 2004/05/05 23:14:45 pcharles Exp $";
	}
}