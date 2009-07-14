// $Id: IPPacket.java,v 1.24 2004/05/05 23:14:45 pcharles Exp $

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
	/// <summary> An IP protocol packet.
	/// <p>
	/// Extends an ethernet packet, adding IP header information and an IP 
	/// data payload. 
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.24 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public class IPPacket:EthernetPacket, IPFields/* ,System.Runtime.Serialization.ISerializable*/
	{
		/// 
		/// <summary> Get the IP version code.
		/// </summary>
		virtual public int Version
		{
			// have to use a boolean, int!=Object
			
			get
			{
				if (!_versionSet)
				{
					_version = (ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_VER_POS, Packets.IPFields_Fields.IP_VER_LEN) >> 4) & 0xf;
					_versionSet = true;
				}
				return _version;
			}
			
		}
		/// 
		/// <summary> Fetch the IP header length in bytes.
		/// </summary>
		virtual public int IPHeaderLength
		{
			get
			{
				return _ipHeaderLength;
			}
			
		}
		/// 
		/// <summary> Fetch the IP header length in bytes.
		/// </summary>
		public int IpHeaderLength
		{
			get
			{
				// this is the old method call, but everything else uses all caps for
				// TCP, so in the interest of consistency...
				return IPHeaderLength;
			}
		}

		/// 
		/// <summary> Fetch the packet IP header length.
		/// </summary>
		override public int HeaderLength
		{
			get
			{
				return IPHeaderLength;
			}
			
		}
		/// 
		/// <summary> Fetch the type of service. 
		/// For more information refer to the TypesOfService interface.
		/// </summary>
		virtual public int TypeOfService
		{
			get
			{
				if (!_typeOfServiceSet)
				{
					_typeOfService = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_TOS_POS, Packets.IPFields_Fields.IP_TOS_LEN);
					_typeOfServiceSet = true;
				}
				return _typeOfService;
			}
			
		}
		/// 
		/// <summary> Fetch the IP length in bytes.
		/// </summary>
		virtual public int Length
		{
			get
			{
				if (!_lengthSet)
				{
					_length = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_LEN_POS, Packets.IPFields_Fields.IP_LEN_LEN);
					_lengthSet = true;
				}
				return _length;
			}
			
		}
		/// <summary> Fetch the unique ID of this IP datagram. The ID normally 
		/// increments by one each time a datagram is sent by a host.
		/// </summary>
		virtual public int Id
		{
			get
			{
				if (!_idSet)
				{
					_id = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_ID_POS, Packets.IPFields_Fields.IP_ID_LEN);
					_idSet = true;
				}
				return _id;
			}
			
		}
		/// 
		/// <summary> Fetch fragmentation flags.
		/// </summary>
		virtual public int FragmentFlags
		{
			get
			{
				if (!_fragmentFlagsSet)
				{
					// fragment flags are the high 3 bits
					int huh = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_FRAG_POS, Packets.IPFields_Fields.IP_FRAG_LEN);
					_fragmentFlags = (ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_FRAG_POS, Packets.IPFields_Fields.IP_FRAG_LEN) >> 13) & 0x7;
					_fragmentFlagsSet = true;
				}
				return _fragmentFlags;
			}
			
		}
		/// 
		/// <summary> Fetch fragmentation offset.
		/// </summary>
		virtual public int FragmentOffset
		{
			get
			{
				if (!_fragmentOffsetSet)
				{
					// offset is the low 13 bits
					_fragmentOffset = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_FRAG_POS, Packets.IPFields_Fields.IP_FRAG_LEN) & 0x1fff;
					_fragmentOffsetSet = true;
				}
				return _fragmentOffset;
			}
			
		}
		/// <summary> Fetch the time to live. TTL sets the upper limit on the number of 
		/// routers through which this IP datagram is allowed to pass.
		/// </summary>
		virtual public int TimeToLive
		{
			get
			{
				if (!_timeToLiveSet)
				{
					_timeToLive = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_TTL_POS, Packets.IPFields_Fields.IP_TTL_LEN);
					_timeToLiveSet = true;
				}
				return _timeToLive;
			}
			
		}
		/// <summary> Fetch the code indicating the type of protocol embedded in the IP
		/// </summary>
		/// <seealso cref="">IPProtocols.
		/// 
		/// </seealso>
		virtual public int IPProtocol
		{
			get
			{
				if (!_ipProtocolSet)
				{
					_ipProtocol = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_CODE_POS, Packets.IPFields_Fields.IP_CODE_LEN);
					_ipProtocolSet = true;
				}
				return _ipProtocol;
			}
			
		}
		/// <summary> Fetch the code indicating the type of protocol embedded in the IP
		/// </summary>
		/// <seealso cref="">IPProtocols.
		/// 
		/// </seealso>
		override public int Protocol
		{
			get
			{
				return IPProtocol;
			}
			
		}
		/// 
		/// <summary> Fetch the header checksum.
		/// </summary>
		virtual public int IPChecksum
		{
			get
			{
				if (!_ipChecksumSet)
				{
					_ipChecksum = ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_CSUM_POS, Packets.IPFields_Fields.IP_CSUM_LEN);
					_ipChecksumSet = true;
				}
				return _ipChecksum;
			}
			
		}
		/// 
		/// <summary> Fetch the header checksum.
		/// </summary>
		virtual public int Checksum
		{
			get
			{
				return IPChecksum;
			}
			
		}
		/// 
		/// <summary> Fetch the IP address of the host where the packet originated from.
		/// </summary>
		virtual public System.String SourceAddress
		{
			get
			{
				if (_sourceAddress == null)
				{
					_sourceAddress = IPAddress.extract(_ethOffset + Packets.IPFields_Fields.IP_SRC_POS, _bytes);
				}
				return _sourceAddress;
			}
			
		}
		/// 
		/// <summary> Fetch the source address as a byte array.
		/// </summary>
		virtual public byte[] SourceAddressBytes
		{
			get
			{
				if (_sourceAddressBytes == null)
				{
					_sourceAddressBytes = new byte[4];
					Array.Copy(_bytes, _ethOffset + Packets.IPFields_Fields.IP_SRC_POS, _sourceAddressBytes, 0, 4);
				}
				return _sourceAddressBytes;
			}
			
		}
		/// 
		/// <summary> Fetch the source address as a long.
		/// </summary>
		virtual public long SourceAddressAsLong
		{
			get
			{
				if (!_sourceAddressAsLongSet)
				{
					_sourceAddressAsLong = ArrayHelper.extractLong(_bytes, _ethOffset + Packets.IPFields_Fields.IP_SRC_POS, 4);
					_sourceAddressAsLongSet = true;
				}
				return _sourceAddressAsLong;
			}
			
		}
		/// 
		/// <summary> Fetch the IP address of the host where the packet is destined.
		/// </summary>
		virtual public System.String DestinationAddress
		{
			get
			{
				if (_destinationAddress == null)
				{
					_destinationAddress = IPAddress.extract(_ethOffset + Packets.IPFields_Fields.IP_DST_POS, _bytes);
				}
				return _destinationAddress;
			}
			
		}
		/// 
		/// <summary> Fetch the destination address as a byte array.
		/// </summary>
		virtual public byte[] DestinationAddressBytes
		{
			get
			{
				if (_destinationAddressBytes == null)
				{
					_destinationAddressBytes = new byte[4];
					Array.Copy(_bytes, _ethOffset + Packets.IPFields_Fields.IP_DST_POS, _destinationAddressBytes, 0, 4);
				}
				return _destinationAddressBytes;
			}
			
		}
		/// 
		/// <summary> Fetch the destination address as a long.
		/// </summary>
		virtual public long DestinationAddressAsLong
		{
			get
			{
				if (!_destinationAddressAsLongSet)
				{
					_destinationAddressAsLong = ArrayHelper.extractLong(_bytes, _ethOffset + Packets.IPFields_Fields.IP_DST_POS, 4);
					_destinationAddressAsLongSet = true;
				}
				return _destinationAddressAsLong;
			}
			
		}
		/// 
		/// <summary> Fetch the IP header a byte array.
		/// </summary>
		virtual public byte[] IPHeader
		{
			get
			{
				if (_ipHeaderBytes == null)
				{
					_ipHeaderBytes = PacketEncoding.extractHeader(_ethOffset, IPHeaderLength, _bytes);
				}
				return _ipHeaderBytes;
			}
			
		}
		/// 
		/// <summary> Fetch the IP header as a byte array.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return IPHeader;
			}
			
		}
		/// 
		/// <summary> Fetch the IP data as a byte array.
		/// </summary>
		virtual public byte[] IPData
		{
			get
			{
				if (_ipDataBytes == null)
				{
					// set data length based on info in headers (note: tcpdump
					//  can return extra junk bytes which bubble up to here
					int tmpLen = Length - IPHeaderLength;
					_ipDataBytes = PacketEncoding.extractData(_ethOffset, IPHeaderLength, _bytes, tmpLen);
				}
				return _ipDataBytes;
			}
			
		}
		/// 
		/// <summary> Fetch the IP data as a byte array.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return IPData;
			}
			
		}
		/// 
		/// <summary> Check if the IP packet is valid, checksum-wise.
		/// </summary>
		virtual public bool ValidChecksum
		{
			get
			{
				if (!_isValidChecksumSet)
				{
					// first validate other information about the packet. if this stuff
					// is not true, the packet (and therefore the checksum) is invalid
					// - ip_hl >= 5 (ip_hl is the length in 4-byte words)
					if (IPHeaderLength < Packets.IPFields_Fields.IP_HEADER_LEN)
					{
						_isValidChecksum = false;
					}
					else
					{
						_isValidChecksum = (computeReceiverIPChecksum() == 0xffff);
					}
					_isValidChecksumSet = true;
				}
				return _isValidChecksum;
			}
			
		}
		/// <summary> Check if the IP packet is valid, checksum-wise.
		/// </summary>
		virtual public bool ValidIPChecksum
		{
			get
			{
				return ValidChecksum;
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.WHITE;
			}
			
		}
		// offset from beginning of byte array where IP header ends (i.e.,
		//  size of ethernet frame header and IP header
		protected internal int _ipOffset;
		
		/// 
		/// <summary> Create a new IP packet. 
		/// </summary>
		public IPPacket(int lLen, byte[] bytes):base(lLen, bytes)
		{
			// fetch the actual header length from the incoming bytes
			_ipHeaderLength = (ArrayHelper.extractInteger(_bytes, _ethOffset + Packets.IPFields_Fields.IP_VER_POS, Packets.IPFields_Fields.IP_VER_LEN) & 0xf) * 4;
			// set offset into _bytes of previous layers
			_ipOffset = _ethOffset + _ipHeaderLength;
		}
		
		/// <summary> Create a new IP packet.
		/// </summary>
		public IPPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		private int _version;
		private bool _versionSet = false;
		
		// set in constructor
		private int _ipHeaderLength;
		
		
		
		private int _typeOfService;
		private bool _typeOfServiceSet = false;
		
		private int _length;
		private bool _lengthSet = false;
		
		private int _id;
		private bool _idSet = false;
		
		private int _fragmentFlags;
		private bool _fragmentFlagsSet = false;
		
		private int _fragmentOffset;
		private bool _fragmentOffsetSet = false;
		
		private int _timeToLive;
		private bool _timeToLiveSet = false;
		
		private int _ipProtocol;
		private bool _ipProtocolSet = false;
		
		private int _ipChecksum;
		private bool _ipChecksumSet = false;
		
		
		private System.String _sourceAddress = null;
		
		private byte[] _sourceAddressBytes = null;
		
		private long _sourceAddressAsLong;
		private bool _sourceAddressAsLongSet = false;
		
		private System.String _destinationAddress = null;
		
		private byte[] _destinationAddressBytes = null;
		
		private long _destinationAddressAsLong;
		private bool _destinationAddressAsLongSet = false;
		
		private byte[] _ipHeaderBytes = null;
		
		
		private byte[] _ipDataBytes = null;
		
		
		private bool _isValidChecksum;
		private bool _isValidChecksumSet = false;
		
		protected internal virtual int computeReceiverIPChecksum()
		{
			return computeReceiverChecksum(_ethOffset, IPHeaderLength);
		}
		protected internal virtual int computeReceiverChecksum(int start, int len)
		{
			// checksum should come out to -1 if checksum is correct
			return onesCompSum(_bytes, start, len);
		}
		protected internal virtual int computeSenderIPChecksum()
		{
			return computeSenderChecksum(_ethOffset, IPHeaderLength, 10);
		}
		protected internal virtual int computeSenderChecksum(int start, int len, int csumPos)
		{
			// quick bad-data check
			if (csumPos >= len)
				return 0;
			// bad data, header too short
			// copy bytes, zero out checksum
			byte[] bytes = new byte[len];
			Array.Copy(_bytes, start, bytes, 0, len);
			// zero out any current checksum
			bytes[csumPos] = (byte) (bytes[csumPos + 1] = 0);
			// checksum should come out to -1 if checksum is correct
			return onesCompSum(bytes, 0, len);
		}
		protected internal virtual int onesCompSum(byte[] bytes, int start, int len)
		{
			int sum = 0;
			// basically, IP checksums are done by taking the 16 bit ones-
			// complement sum of the IP header. This means summing two bytes
			// at a time. no error checking is done (e.g. bounds checking)
			int i;
			for (i = 0; i < len; i += 2)
			{
				// put bytes in ints so we can forget about sign-extension
				int i1 = bytes[start + i] & 0xff;
				// zero-pad, maybe
				int i2 = (start + i + 1 < len?bytes[start + i + 1] & 0xff:0);
				sum += ((i1 << 8) + i2);
				while ((sum & 0xffff) != sum)
				{
					sum &= 0xffff;
					sum += 1;
				}
			}
			return sum;
		}
		
		/// <summary> Convert this IP packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this IP packet.
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
			buffer.Append("IPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append(SourceAddress + " -> " + DestinationAddress);
			buffer.Append(" proto=" + Protocol);
			buffer.Append(" l=" + IPHeaderLength + "," + Length);
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		/// <summary> Convert this IP packet to a more verbose string.
		/// </summary>
		public virtual System.String toColoredVerboseString(bool colored)
		{
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();
			buffer.Append('[');
			if (colored)
				buffer.Append(Color);
			buffer.Append("IPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append("version=" + Version + ", ");
			buffer.Append("hlen=" + HeaderLength + ", ");
			buffer.Append("tos=" + TypeOfService + ", ");
			buffer.Append("length=" + Length + ", ");
			buffer.Append("id=" + Id + ", ");
			buffer.Append("flags=0x" + System.Convert.ToString(FragmentFlags, 16) + ", ");
			buffer.Append("offset=" + FragmentOffset + ", ");
			buffer.Append("ttl=" + TimeToLive + ", ");
			buffer.Append("proto=" + Protocol + ", ");
			buffer.Append("sum=0x" + System.Convert.ToString(Checksum, 16) + ", ");
			buffer.Append("src=" + SourceAddress + ", ");
			buffer.Append("dest=" + DestinationAddress);
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		
		/// 
		/// <summary> This inner class provides access to private methods for unit testing.
		/// </summary>
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'TestProbe' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		public class TestProbe
		{
			public TestProbe(IPPacket enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(IPPacket enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private IPPacket enclosingInstance;
			virtual public int ComputedReceiverIPChecksum
			{
				get
				{
					return Enclosing_Instance.computeReceiverIPChecksum();
				}
				
			}
			virtual public int ComputedSenderIPChecksum
			{
				get
				{
					return Enclosing_Instance.computeSenderIPChecksum();
				}
				
			}
			public IPPacket Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
		}
		
		
		private System.String _rcsid = "$Id: IPPacket.java,v 1.24 2004/05/05 23:14:45 pcharles Exp $";
	}
}