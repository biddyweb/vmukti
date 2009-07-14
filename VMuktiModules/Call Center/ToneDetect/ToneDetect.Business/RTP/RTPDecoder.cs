using System;
using System.Collections.Generic;
using System.Text;

using ToneDetect.Business.PayloadDecoders;

namespace ToneDetect.Business.RTP
{
	public class RTPDecoder
	{
		#region Fields
		/// <summary>
		/// RTP version number
		/// </summary>
		private byte version;

		/// <summary>
		/// padding at the end of the packet
		/// 
		/// From the RFC 3550:
		/// If the padding bit is set, the packet contains one or more
      /// additional padding octets at the end which are not part of the
      /// payload.  The last octet of the padding contains a count of how
      /// many padding octets should be ignored, including itself.  Padding
      /// may be needed by some encryption algorithms with fixed block sizes
      /// or for carrying several RTP packets in a lower-layer protocol data
		/// unit.
		/// </summary>
		private bool padding;

		/// <summary>
		/// Extension bit --- TODO: need to come to standard with this as well
		/// since it will affect the protocol.
		/// </summary>
		private bool extension;

		/// <summary>
		/// Number of contributing sources in the packet
		/// </summary>
		private byte contributingSourceIdCount;

		/// <summary>
		/// Marker for synch
		/// </summary>
		private byte marker;

		/// <summary>
		/// Specifies the payload type of the packet
		/// </summary>
		private byte payloadType;

		/// <summary>
		/// Packet sequence number
		/// </summary>
		private uint sequenceNumber;

		/// <summary>
		/// The time of the first sample in the packet.
		/// </summary>
		private uint timestamp;

		/// <summary>
		/// Synch source
		/// </summary>
		private uint synchronizationSource;

		/// <summary>
		/// The contributing sources
		/// </summary>
		private uint [] contributingSources;

		/// <summary>
		/// The packet payload
		/// </summary>
		private byte [] payload;

		/// <summary>
		/// The decoded payload.
		/// </summary>
		private short [] decodedPayload;
		#endregion

		#region Properties
		/// <summary>
		/// RTP version number
		/// </summary>
		public byte Version
		{
			get
			{
				return version;
			}
		}

		/// <summary>
		/// padding at the end of the packet
		/// 
		/// From the RFC 3550:
		/// If the padding bit is set, the packet contains one or more
      /// additional padding octets at the end which are not part of the
      /// payload.  The last octet of the padding contains a count of how
      /// many padding octets should be ignored, including itself.  Padding
      /// may be needed by some encryption algorithms with fixed block sizes
      /// or for carrying several RTP packets in a lower-layer protocol data
		/// unit.
		/// </summary>
		public bool Padding
		{
			get
			{
				return padding;
			}
		}

		/// <summary>
		/// Extension bit --- TODO: need to come to standard with this as well
		/// since it will affect the protocol.
		/// </summary>
		public bool Extension
		{
			get
			{
				return extension;
			}
		}

		/// <summary>
		/// Number of contributing sources in the packet
		/// </summary>
		public byte ContributingSourceIdCount
		{
			get
			{
				return contributingSourceIdCount;
			}
		}

		/// <summary>
		/// Marker for synch
		/// </summary>
		public byte Marker
		{
			get
			{
				return marker;
			}
		}

		/// <summary>
		/// Specifies the payload type of the packet
		/// </summary>
		public byte PayloadType
		{
			get
			{
				return payloadType;
			}
		}

		/// <summary>
		/// Packet sequence number
		/// </summary>
		public uint SequenceNumber
		{
			get
			{
				return sequenceNumber;
			}
		}

		/// <summary>
		/// The time of the first sample in the packet.
		/// </summary>
		public uint Timestamp
		{
			get
			{
				return timestamp;
			}
		}

		/// <summary>
		/// Synch source
		/// </summary>
		public uint SynchronizationSource
		{
			get
			{
				return synchronizationSource;
			}
		}

		/// <summary>
		/// The contributing sources
		/// </summary>
		public uint [] ContributingSources
		{
			get
			{
				return contributingSources;
			}
		}
		/// <summary>
		/// The packet payload
		/// </summary>
		public byte [] Payload
		{
			get
			{
				return payload;
			}
		}

		/// <summary>
		/// The decoded payload.
		/// </summary>
		public short [] DecodedPayload
		{
			get
			{
				return decodedPayload;
			}
		}
		#endregion

		#region Constructor
		public RTPDecoder( byte [] packet )
		{
			version = 0;
			padding = false;
			extension = false;
			contributingSourceIdCount = 0;
			marker = 0;
			payloadType = 0;
			sequenceNumber = 0;
			timestamp = 0;
			synchronizationSource = 0;
			contributingSources = null;
			payload = null;

			DecodeRTP( packet );
			DecodePayload();
		}
		#endregion

		#region Decode
		/// <summary>
		/// The purpose of this method is to decode the RTP payload into raw audio.
		/// </summary>
		private void DecodePayload()
		{
			// u-law decoding
			if( this.payloadType == 0 )
			{
				decodedPayload = MuLawDecoder.MuLawDecode( payload );
			}
			// a-law decoding
			else if( this.payloadType == 1 )
			{
				decodedPayload = ALawDecoder.ALawDecode( payload );
			}
			else if( this.payloadType == 101 )
			{
				decodedPayload = null;
			}
			else
			{
				throw new InvalidRTPPacketException();
				// unknown decoding --- more may need to be added in the future.
			}
		}

		/// <summary>
		/// The purpose of this method is to decode an array of bytes that
		/// presumably represents a RTP packet.
		/// 
		/// The packet has the following format from RFC 3550:
		/// 0                   1                   2                   3
		/// 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
		/// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
		/// |V=2|P|X|  CC   |M|     PT      |       sequence number         |
   	/// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   	/// |                           timestamp                           |
   	/// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   	/// |           synchronization source (SSRC) identifier            |
   	/// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
   	/// |            contributing source (CSRC) identifiers             |
   	/// |                             ....                              |
		/// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
		/// </summary>
		/// <param name="packet"></param>
		void DecodeRTP( byte [] packet )
		{
			ushort twoBytes;

			// The header of the packet must be at least 12 bytes.
			if( packet.Length < 12 )
			{
				throw new InvalidRTPPacketException();
			}
			twoBytes = BitConverter.ToUInt16( packet, 0 );
			twoBytes = Endian.ConvertEndian( twoBytes );

			// version if the upper two bits.
			version = (byte)((twoBytes & 0xC000) >> 14);

			padding = ((twoBytes & 0x2000) >> 13) == 1?true:false;

			extension = ((twoBytes & 0x1000) >> 12) == 1?true:false;

			contributingSourceIdCount = (byte)((twoBytes & 0x0f00) >> 8);

			marker = (byte)((twoBytes & 0x0080) >> 7);

			payloadType = (byte)(twoBytes & 0x007f);

			// the sequence number is the next 2 bytes in (2 bytes from the beginning)
			twoBytes = BitConverter.ToUInt16( packet, 2 );
			sequenceNumber = Endian.ConvertEndian( twoBytes );

			// the timestamp is 4 bytes from the beginning
			timestamp = Endian.ConvertEndian( BitConverter.ToUInt32( packet, 4 ) );

			// the synchronization source id is 8 bytes from the beginning.
			synchronizationSource = Endian.ConvertEndian( BitConverter.ToUInt32( packet, 8 ) );

			// if there are contributing sources (more than zero) then they are
			// a set of 4-byte ids count specified by the count from the
			// fields above in the header.
			if( contributingSourceIdCount > 0 )
			{
				contributingSources = new uint [contributingSourceIdCount];
				for( int i = 0; i < contributingSourceIdCount; ++i )
				{
					contributingSources [i] = Endian.ConvertEndian( BitConverter.ToUInt32( packet, 12 + ( i * 4 ) ) );
				}
			}

			//TODO: need to properly handle the padding.
			if( padding )
			{
				System.Console.WriteLine( "Padding Bit Set" );
			}

			// now, need to pull out the packet itself.  The offset is given by
			// 12 + 4 * contributingSourceIdCount - the number of bytes specified by the
			// padding count (if the padding bit is set).  12 is the size of the nominal
			// packet, and each contributing source is specified with 4 bytes.
			int offset = 12 + (4 * contributingSourceIdCount);
			int payloadCount;
			payloadCount = packet.Length - offset;
			this.payload = new byte [payloadCount];
			Array.Copy( packet, offset, payload, 0, payloadCount );
		}
		#endregion
	}
}
