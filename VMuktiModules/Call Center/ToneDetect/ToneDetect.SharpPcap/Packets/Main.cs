using System;

namespace ToneDetect.SharpPcap.Packets
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class MainClass
	{
		public static void Main(String[] args)
		{
			TCPPacketTest.Main1(null);
		}

		public static void MyTest1()
		{

			byte[] SYN_ACK_PACKET = new byte[]{(byte) (0x00), (byte) (0x10), (byte) (0x7b), (byte) (0x38), (byte) (0x46), (byte) (0x33), (byte) (0x08), (byte) (0x00), (byte) (0x20), (byte) (0x89), (byte) (0xa5), (byte) (0x9f), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x2c), (byte) (0x93), (byte) (0x83), (byte) (0x40), (byte) (0x00), (byte) (0xff), (byte) (0x06), (byte) (0x6c), (byte) (0x38), (byte) (0xac), (byte) (0x10), (byte) (0x70), (byte) (0x32), (byte) (0x87), (byte) (0x0d), (byte) (0xd8), (byte) (0xbf), (byte) (0x00), (byte) (0x19), (byte) (0x50), (byte) (0x49), (byte) (0x78), (byte) (0xbe), (byte) (0xe0), (byte) (0xa7), (byte) (0x9f), (byte) (0x3a), (byte) (0xb4), (byte) (0x03), (byte) (0x60), (byte) (0x12), (byte) (0x22), (byte) (0x38), (byte) (0xfc), (byte) (0xc7), (byte) (0x00), (byte) (0x00), (byte) (0x02), (byte) (0x04), (byte) (0x05), (byte) (0xb4), (byte) (0x70), (byte) (0x6c)};
			byte[] PSH_ACK_PACKET = new byte[]{(byte) (0x08), (byte) (0x00), (byte) (0x20), (byte) (0x89), (byte) (0xa5), (byte) (0x9f), (byte) (0x00), (byte) (0x10), (byte) (0x7b), (byte) (0x38), (byte) (0x46), (byte) (0x33), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x3e), (byte) (0x87), (byte) (0x08), (byte) (0x40), (byte) (0x00), (byte) (0x3f), (byte) (0x06), (byte) (0x38), (byte) (0xa2), (byte) (0x87), (byte) (0x0d), (byte) (0xd8), (byte) (0xbf), (byte) (0xac), (byte) (0x10), (byte) (0x70), (byte) (0x32), (byte) (0x50), (byte) (0x49), (byte) (0x00), (byte) (0x19), (byte) (0x9f), (byte) (0x3a), (byte) (0xb4), (byte) (0x03), (byte) (0x78), (byte) (0xbe), (byte) (0xe0), (byte) (0xf8), (byte) (0x50), (byte) (0x18), (byte) (0x7d), (byte) (0x78), (byte) (0x86), (byte) (0xf0), (byte) (0x00), (byte) (0x00), (byte) (0x45), (byte) (0x48), (byte) (0x4c), (byte) (0x4f), (byte) (0x20), (byte) (0x61), (byte) (0x6c), (byte) (0x70), (byte) (0x68), (byte) (0x61), (byte) (0x2e), (byte) (0x61), (byte) (0x70), (byte) (0x70), (byte) (0x6c), (byte) (0x65), (byte) (0x2e), (byte) (0x65), (byte) (0x64), (byte) (0x75), (byte) (0x0d), (byte) (0x0a)};

			// get link layer length
			int linkLayerLen = LinkLayer.getLinkLayerLength(Packets.LinkLayers.EN10MB);
			// create syn-ack packet
			IPPacket _synAck = new IPPacket(linkLayerLen, SYN_ACK_PACKET);
			// create psh-ack packet
			IPPacket _pshAck = new IPPacket(linkLayerLen, PSH_ACK_PACKET);
			// create packet with random garbage
			byte[] badBytes = new byte[SYN_ACK_PACKET.Length];
			(new System.Random()).NextBytes((badBytes));
			IPPacket _baddie = new IPPacket(linkLayerLen, badBytes);

			IPPacket.TestProbe probe = new Packets.IPPacket.TestProbe(_synAck);
			Console.WriteLine("Computed IP checksum mismatch, should be " + System.Convert.ToString(_synAck.IPChecksum, 16) + ", but is " + System.Convert.ToString(probe.ComputedSenderIPChecksum, 16) + ", (" + System.Convert.ToString(probe.ComputedReceiverIPChecksum, 16) + ") ---- {0}", _synAck.ValidChecksum);

			Console.WriteLine("From: {0}, To:{1}", _synAck.SourceAddress, _synAck.DestinationAddress);
		}
	}
}
