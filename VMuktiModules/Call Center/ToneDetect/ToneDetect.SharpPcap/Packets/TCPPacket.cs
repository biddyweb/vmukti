// $Id: TCPPacket.java,v 1.22 2004/05/05 23:14:45 pcharles Exp $

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
	/// <summary> A TCP packet.
	/// <p>
	/// Extends an IP packet, adding a TCP header and TCP data payload.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.22 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/05/05 23:14:45 $
	/// 
	/// </version>
	[Serializable]
	public class TCPPacket:IPPacket, TCPFields/* ,System.Runtime.Serialization.ISerializable*/
	{
		/// 
		/// <summary> Fetch the port number on the source host.
		/// </summary>
		virtual public int SourcePort
		{
			get
			{
				if (!_sourcePortSet)
				{
					_sourcePort = ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_SP_POS, Packets.TCPFields_Fields.TCP_PORT_LEN);
					_sourcePortSet = true;
				}
				return _sourcePort;
			}
			
		}
		/// 
		/// <summary> Fetches the port number on the destination host.
		/// </summary>
		virtual public int DestinationPort
		{
			get
			{
				if (!_destinationPortSet)
				{
					_destinationPort = ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_DP_POS, Packets.TCPFields_Fields.TCP_PORT_LEN);
					_destinationPortSet = true;
				}
				return _destinationPort;
			}
			
		}
		/// 
		/// <summary> Fetch the packet sequence number.
		/// </summary>
		virtual public long SequenceNumber
		{
			get
			{
				if (!_sequenceNumberSet)
				{
					_sequenceNumber = ArrayHelper.extractLong(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_SEQ_POS, Packets.TCPFields_Fields.TCP_SEQ_LEN);
					_sequenceNumberSet = true;
				}
				return _sequenceNumber;
			}
			
		}
		/// 
		/// <summary>    Fetch the packet acknowledgment number.
		/// </summary>
		virtual public long AcknowledgmentNumber
		{
			get
			{
				if (!_acknowledgmentNumberSet)
				{
					_acknowledgmentNumber = ArrayHelper.extractLong(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_ACK_POS, Packets.TCPFields_Fields.TCP_ACK_LEN);
					_acknowledgmentNumberSet = true;
				}
				return _acknowledgmentNumber;
			}
			
		}
		/// 
		/// <summary> Fetch the packet acknowledgment number. 
		/// </summary>
		virtual public long AcknowledgementNumber
		{
			get
			{
				return AcknowledgmentNumber;
			}
			
		}

		/// 
		/// <summary> Fetch the TCP header length in bytes.
		/// </summary>
		virtual public int TcpHeaderLength
		{
			get
			{
				// this is the old method call, but everything else uses all caps for
				// TCP, so in the interest of consistency...
				return TCPHeaderLength;
			}
			
		}
		/// 
		/// <summary> Fetch the TCP header length in bytes.
		/// </summary>
		virtual public int TCPHeaderLength
		{
			get
			{
				// this is the old method call, but everything else uses all caps for
				// TCP, so in the interest of consistency...
				return _tcpHeaderLength;
			}
			
		}
		/// <summary> Fetches the packet TCP header length.
		/// </summary>
		override public int HeaderLength
		{
			get
			{
				return TCPHeaderLength;
			}
			
		}
		/// <summary> Fetches the length of the payload data.
		/// </summary>
		virtual public int PayloadDataLength
		{
			get
			{
				return _payloadDataLength;
			}
			
		}
		/// <summary> Fetch the window size.
		/// </summary>
		virtual public int WindowSize
		{
			get
			{
				if (!_windowSizeSet)
				{
					_windowSize = ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_WIN_POS, Packets.TCPFields_Fields.TCP_WIN_LEN);
					_windowSizeSet = true;
				}
				return _windowSize;
			}
			
		}
		/// 
		/// <summary> Fetch the header checksum.
		/// </summary>
		virtual public int TCPChecksum
		{
			get
			{
				if (!_tcpChecksumSet)
				{
					_tcpChecksum = ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_CSUM_POS, Packets.TCPFields_Fields.TCP_CSUM_LEN);
					_tcpChecksumSet = true;
				}
				return _tcpChecksum;
			}
			
		}
		/// <summary> Fetch the header checksum.
		/// </summary>
		override public int Checksum
		{
			get
			{
				return TCPChecksum;
			}
			
		}
		/// 
		/// <summary> Fetch the urgent pointer.
		/// </summary>
		virtual public int UrgentPointer
		{
			get
			{
				if (!_urgentPointerSet)
				{
					_urgentPointer = ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_URG_POS, Packets.TCPFields_Fields.TCP_URG_LEN);
					_urgentPointerSet = true;
				}
				return _urgentPointer;
			}
			
		}
		private int AllFlags
		{
			get
			{
				if (!_allFlagsSet)
				{
					_allFlags = ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_FLAG_POS, Packets.TCPFields_Fields.TCP_FLAG_LEN);
				}
				return _allFlags;
			}
			
		}
		/// 
		/// <summary> Check the URG flag, flag indicates if the urgent pointer is valid.
		/// </summary>
		virtual public bool Urg
		{
			get
			{
				if (!_isUrgSet)
				{
					_isUrg = (AllFlags & Packets.TCPFields_Fields.TCP_URG_MASK) != 0;
					_isUrgSet = true;
				}
				return _isUrg;
			}
			
		}
		/// 
		/// <summary> Check the ACK flag, flag indicates if the ack number is valid.
		/// </summary>
		virtual public bool Ack
		{
			get
			{
				if (!_isAckSet)
				{
					_isAck = (AllFlags & Packets.TCPFields_Fields.TCP_ACK_MASK) != 0;
					_isAckSet = true;
				}
				return _isAck;
			}
			
		}
		/// 
		/// <summary> Check the PSH flag, flag indicates the receiver should pass the
		/// data to the application as soon as possible.
		/// </summary>
		virtual public bool Psh
		{
			get
			{
				if (!_isPshSet)
				{
					_isPsh = (AllFlags & Packets.TCPFields_Fields.TCP_PSH_MASK) != 0;
					_isPshSet = true;
				}
				return _isPsh;
			}
			
		}
		/// 
		/// <summary> Check the RST flag, flag indicates the session should be reset between
		/// the sender and the receiver.
		/// </summary>
		virtual public bool Rst
		{
			get
			{
				if (!_isRstSet)
				{
					_isRst = (AllFlags & Packets.TCPFields_Fields.TCP_RST_MASK) != 0;
					_isRstSet = true;
				}
				return _isRst;
			}
			
		}
		/// 
		/// <summary> Check the SYN flag, flag indicates the sequence numbers should
		/// be synchronized between the sender and receiver to initiate
		/// a connection.
		/// </summary>
		virtual public bool Syn
		{
			get
			{
				if (!_isSynSet)
				{
					_isSyn = (AllFlags & Packets.TCPFields_Fields.TCP_SYN_MASK) != 0;
					_isSynSet = true;
				}
				return _isSyn;
			}
			
		}
		/// 
		/// <summary> Check the FIN flag, flag indicates the sender is finished sending.
		/// </summary>
		virtual public bool Fin
		{
			get
			{
				if (!_isFinSet)
				{
					_isFin = (AllFlags & Packets.TCPFields_Fields.TCP_FIN_MASK) != 0;
					_isFinSet = true;
				}
				return _isFin;
			}
			
		}
		/// <summary> Fetch the TCP header a byte array.
		/// </summary>
		virtual public byte[] TCPHeader
		{
			get
			{
				if (_tcpHeaderBytes == null)
				{
					_tcpHeaderBytes = PacketEncoding.extractHeader(_ipOffset, TcpHeaderLength, _bytes);
				}
				return _tcpHeaderBytes;
			}
			
		}
		/// 
		/// <summary> Fetch the TCP header as a byte array.
		/// </summary>
		override public byte[] Header
		{
			get
			{
				return TCPHeader;
			}
			
		}
		/// 
		/// <summary> Fetch the TCP data as a byte array.
		/// </summary>
		virtual public byte[] TCPData
		{
			get
			{
				if (_tcpDataBytes == null)
				{
					// set data length based on info in headers (note: tcpdump
					//  can return extra junk bytes which bubble up to here
					_tcpDataBytes = PacketEncoding.extractData(_ipOffset, TcpHeaderLength, _bytes, PayloadDataLength);
				}
				return _tcpDataBytes;
			}
			
		}
		/// <summary> Fetch the TCP data as a byte array.
		/// </summary>
		override public byte[] Data
		{
			get
			{
				return TCPData;
			}
			
		}
		/// <summary> Fetch ascii escape sequence of the color associated with this packet type.
		/// </summary>
		override public System.String Color
		{
			get
			{
				return AnsiEscapeSequences.YELLOW;
			}
			
		}
		/// 
		/// <summary> Create a new TCP packet.
		/// </summary>
		public TCPPacket(int lLen, byte[] bytes):base(lLen, bytes)
		{
			// set TCP header length
			_tcpHeaderLength = ((ArrayHelper.extractInteger(_bytes, _ipOffset + Packets.TCPFields_Fields.TCP_FLAG_POS, Packets.TCPFields_Fields.TCP_FLAG_LEN) >> 12) & 0xf) * 4;
			// set data (payload) length based on info in headers (note: tcpdump
			//  can return extra junk bytes which bubble up to here
			int tmpLen = Length - Packets.IPFields_Fields.IP_HEADER_LEN - _tcpHeaderLength;
			_payloadDataLength = (tmpLen < 0)?0:tmpLen;
		}
		
		/// <summary> Create a new TCP packet.
		/// </summary>
		public TCPPacket(int lLen, byte[] bytes, Timeval tv):this(lLen, bytes)
		{
			this._timeval = tv;
		}
		
		private int _sourcePort;
		private bool _sourcePortSet = false;
		
		private int _destinationPort;
		private bool _destinationPortSet = false;
		
		private long _sequenceNumber;
		private bool _sequenceNumberSet = false;
		
		private long _acknowledgmentNumber;
		private bool _acknowledgmentNumberSet = false;
		
		
		// this gets set by the constructor
		private int _tcpHeaderLength;
		
		
		
		
		// this gets set by the constructor
		private int _payloadDataLength;
		
		private int _windowSize;
		private bool _windowSizeSet = false;
		
		private int _tcpChecksum;
		private bool _tcpChecksumSet = false;
		
		
		private int _urgentPointer;
		private bool _urgentPointerSet = false;
		
		// next value holds all the flags
		private int _allFlags;
		private bool _allFlagsSet = false;
		
		private bool _isUrg;
		private bool _isUrgSet = false;
		
		private bool _isAck;
		private bool _isAckSet = false;
		
		private bool _isPsh;
		private bool _isPshSet = false;
		
		private bool _isRst;
		private bool _isRstSet = false;
		
		private bool _isSyn;
		private bool _isSynSet = false;
		
		private bool _isFin;
		private bool _isFinSet = false;
		
		private byte[] _tcpHeaderBytes = null;
		
		
		private byte[] _tcpDataBytes = null;
		
		
		
		/// <summary> Convert this TCP packet to a readable string.
		/// </summary>
		public override System.String ToString()
		{
			return toColoredString(false);
		}
		
		/// <summary> Generate string with contents describing this TCP packet.
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
			buffer.Append("TCPPacket");
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
			if (Urg)
				buffer.Append(" urg[0x" + System.Convert.ToString(UrgentPointer, 16) + "]");
			if (Ack)
				buffer.Append(" ack[0x" + System.Convert.ToString(AcknowledgmentNumber, 16) + "]");
			if (Psh)
				buffer.Append(" psh");
			if (Rst)
				buffer.Append(" rst");
			if (Syn)
				buffer.Append(" syn[0x" + System.Convert.ToString(SequenceNumber, 16) + "]");
			if (Fin)
				buffer.Append(" fin");
			buffer.Append(" l=" + TCPHeaderLength + "," + PayloadDataLength);
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		/// <summary> Convert this TCP packet to a verbose.
		/// </summary>
		public override System.String toColoredVerboseString(bool colored)
		{
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();
			buffer.Append('[');
			if (colored)
				buffer.Append(Color);
			buffer.Append("TCPPacket");
			if (colored)
				buffer.Append(AnsiEscapeSequences.RESET);
			buffer.Append(": ");
			buffer.Append("sport=" + SourcePort + ", ");
			buffer.Append("dport=" + DestinationPort + ", ");
			buffer.Append("seqn=0x" + System.Convert.ToString(SequenceNumber, 16) + ", ");
			buffer.Append("ackn=0x" + System.Convert.ToString(AcknowledgmentNumber, 16) + ", ");
			buffer.Append("hlen=" + HeaderLength + ", ");
			buffer.Append("urg=" + Urg + ", ");
			buffer.Append("ack=" + Ack + ", ");
			buffer.Append("psh=" + Psh + ", ");
			buffer.Append("rst=" + Rst + ", ");
			buffer.Append("syn=" + Syn + ", ");
			buffer.Append("fin=" + Fin + ", ");
			buffer.Append("wsize=" + WindowSize + ", ");
			buffer.Append("sum=0x" + System.Convert.ToString(Checksum, 16) + ", ");
			buffer.Append("uptr=0x" + System.Convert.ToString(UrgentPointer, 16));
			buffer.Append(']');
			
			return buffer.ToString();
		}
		
		
		
		private System.String _rcsid = "$Id: TCPPacket.java,v 1.22 2004/05/05 23:14:45 pcharles Exp $";
	}
}