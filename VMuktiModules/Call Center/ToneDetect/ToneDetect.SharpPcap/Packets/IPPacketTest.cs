// $Id: IPPacketTest.java,v 1.1 2002/07/10 23:12:09 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets
{
	using System;
	//using junit.framework;
	
	public class IPPacketTest:TestCase
	{
		
		// yes, I realize that as IP packets, these are not SYN-ACK and 
		// PSH-ACKs, but I use the same shell for testing TCPPacket
		private IPPacket _synAck;
		private IPPacket _pshAck;
		// and bad is always bad
		private IPPacket _baddie;
		
		public IPPacketTest(System.String testName):base(testName)
		{
			SYN_ACK_PACKET = new byte[]{(byte) (0x00), (byte) (0x10), (byte) (0x7b), (byte) (0x38), (byte) (0x46), (byte) (0x33), (byte) (0x08), (byte) (0x00), (byte) (0x20), (byte) (0x89), (byte) (0xa5), (byte) (0x9f), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x2c), (byte) (0x93), (byte) (0x83), (byte) (0x40), (byte) (0x00), (byte) (0xff), (byte) (0x06), (byte) (0x6c), (byte) (0x38), (byte) (0xac), (byte) (0x10), (byte) (0x70), (byte) (0x32), (byte) (0x87), (byte) (0x0d), (byte) (0xd8), (byte) (0xbf), (byte) (0x00), (byte) (0x19), (byte) (0x50), (byte) (0x49), (byte) (0x78), (byte) (0xbe), (byte) (0xe0), (byte) (0xa7), (byte) (0x9f), (byte) (0x3a), (byte) (0xb4), (byte) (0x03), (byte) (0x60), (byte) (0x12), (byte) (0x22), (byte) (0x38), (byte) (0xfc), (byte) (0xc7), (byte) (0x00), (byte) (0x00), (byte) (0x02), (byte) (0x04), (byte) (0x05), (byte) (0xb4), (byte) (0x70), (byte) (0x6c)};
			PSH_ACK_PACKET = new byte[]{(byte) (0x08), (byte) (0x00), (byte) (0x20), (byte) (0x89), (byte) (0xa5), (byte) (0x9f), (byte) (0x00), (byte) (0x10), (byte) (0x7b), (byte) (0x38), (byte) (0x46), (byte) (0x33), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x3e), (byte) (0x87), (byte) (0x08), (byte) (0x40), (byte) (0x00), (byte) (0x3f), (byte) (0x06), (byte) (0x38), (byte) (0xa2), (byte) (0x87), (byte) (0x0d), (byte) (0xd8), (byte) (0xbf), (byte) (0xac), (byte) (0x10), (byte) (0x70), (byte) (0x32), (byte) (0x50), (byte) (0x49), (byte) (0x00), (byte) (0x19), (byte) (0x9f), (byte) (0x3a), (byte) (0xb4), (byte) (0x03), (byte) (0x78), (byte) (0xbe), (byte) (0xe0), (byte) (0xf8), (byte) (0x50), (byte) (0x18), (byte) (0x7d), (byte) (0x78), (byte) (0x86), (byte) (0xf0), (byte) (0x00), (byte) (0x00), (byte) (0x45), (byte) (0x48), (byte) (0x4c), (byte) (0x4f), (byte) (0x20), (byte) (0x61), (byte) (0x6c), (byte) (0x70), (byte) (0x68), (byte) (0x61), (byte) (0x2e), (byte) (0x61), (byte) (0x70), (byte) (0x70), (byte) (0x6c), (byte) (0x65), (byte) (0x2e), (byte) (0x65), (byte) (0x64), (byte) (0x75), (byte) (0x0d), (byte) (0x0a)};
		}
		
		[STAThread]
		public static void  Main1(System.String[] args)
		{
			IPPacketTest test = new IPPacketTest("IPPacketTest");
			test.setUp();

			test.testSynAckPacketHeaderValues();
			test.testSynAckPacketHeaderLengths();
			test.testSynAckPacketDataLengths();
			test.testSynAckPacketAddresses();
			test.testPshAckPacketHeaderValues();
			test.testPshAckPacketHeaderLengths();
			test.testPshAckPacketDataLengths();
			test.testPshAckPacketAddresses();
			test.testBadPacketHeaderLengths();
			test.testBadPacketDataLengths();
		}
		
//		public static Test suite()
//		{
//		}
		
		//UPGRADE_NOTE: The initialization of  'SYN_ACK_PACKET' was moved to static method 'SharpPcap.Packets.IPPacketTest'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static byte[] SYN_ACK_PACKET;
		//UPGRADE_NOTE: The initialization of  'PSH_ACK_PACKET' was moved to static method 'SharpPcap.Packets.IPPacketTest'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static byte[] PSH_ACK_PACKET;
		
		public virtual void  setUp()
		{
			// get link layer length
			int linkLayerLen = LinkLayer.getLinkLayerLength(Packets.LinkLayers.EN10MB);
			// create syn-ack packet
			_synAck = new IPPacket(linkLayerLen, SYN_ACK_PACKET);
			// create psh-ack packet
			_pshAck = new IPPacket(linkLayerLen, PSH_ACK_PACKET);
			// create packet with random garbage
			byte[] badBytes = new byte[SYN_ACK_PACKET.Length];
			(new System.Random()).NextBytes((badBytes));
			_baddie = new IPPacket(linkLayerLen, badBytes);
		}
		
		public virtual void  tearDown()
		{
		}
		
		public virtual void  testSynAckPacketHeaderLengths()
		{
			assertEquals(20, _synAck.IPHeaderLength);
			assertEquals(20, _synAck.IPHeader.Length);
			assertEquals(20, _synAck.HeaderLength);
			assertEquals(20, _synAck.Header.Length);
		}
		
		public virtual void  testPshAckPacketHeaderLengths()
		{
			assertEquals(20, _pshAck.IPHeaderLength);
			assertEquals(20, _pshAck.IPHeader.Length);
			assertEquals(20, _pshAck.HeaderLength);
			assertEquals(20, _pshAck.Header.Length);
		}
		
		public virtual void  testSynAckPacketDataLengths()
		{
			assertEquals(24, _synAck.IPData.Length);
			assertEquals(24, _synAck.Data.Length);
		}
		
		public virtual void  testPshAckPacketDataLengths()
		{
			assertEquals(42, _pshAck.IPData.Length);
			assertEquals(42, _pshAck.Data.Length);
		}
		
		public virtual void  testSynAckPacketAddresses()
		{
			assertEquals("172.16.112.50", _synAck.SourceAddress);
			assertEquals("135.13.216.191", _synAck.DestinationAddress);
			assertEquals(2886758450L, _synAck.SourceAddressAsLong);
			assertEquals(2265831615L, _synAck.DestinationAddressAsLong);
			byte[] srcAdd = _synAck.SourceAddressBytes;
			assertTrue("Source address as byte array does not match, bytes are: " + noNeg(srcAdd[0]) + "." + noNeg(srcAdd[1]) + "." + noNeg(srcAdd[2]) + "." + noNeg(srcAdd[3]), ((srcAdd[0] == (byte) (172)) && (srcAdd[1] == (byte) 16) && (srcAdd[2] == (byte) 112) && (srcAdd[3] == (byte) 50)));
			byte[] dstAdd = _synAck.DestinationAddressBytes;
			assertTrue("Dest address as byte array does not match, bytes are: " + noNeg(dstAdd[0]) + "." + noNeg(dstAdd[1]) + "." + noNeg(dstAdd[2]) + "." + noNeg(dstAdd[3]), ((dstAdd[0] == (byte) (135)) && (dstAdd[1] == (byte) 13) && (dstAdd[2] == (byte) (216)) && (dstAdd[3] == (byte) (191))));
		}
		
		public virtual void  testPshAckPacketAddresses()
		{
			assertEquals("135.13.216.191", _pshAck.SourceAddress);
			assertEquals("172.16.112.50", _pshAck.DestinationAddress);
		}
		
		private int noNeg(byte b)
		{
			return 0 | (b & 0xff);
		}
		
		
		public virtual void  testSynAckPacketHeaderValues()
		{
			assertEquals(Packets.IPProtocols_Fields.TCP, _synAck.IPProtocol);
			assertEquals(Packets.IPProtocols_Fields.TCP, _synAck.Protocol);
			assertEquals("IP Checksum mismatch, should be 0x6c38, but is " + System.Convert.ToString(_synAck.IPChecksum, 16), 0x6c38, _synAck.IPChecksum);
			assertEquals("(IP) Checksum mismatch, should be 0x6c38, but is " + System.Convert.ToString(_synAck.Checksum, 16), 0x6c38, _synAck.Checksum);
			IPPacket.TestProbe probe = new Packets.IPPacket.TestProbe(_synAck);
			assertTrue("Computed IP checksum mismatch, should be " + System.Convert.ToString(_synAck.IPChecksum, 16) + ", but is " + System.Convert.ToString(probe.ComputedSenderIPChecksum, 16) + ", (" + System.Convert.ToString(probe.ComputedReceiverIPChecksum, 16) + ")", _synAck.ValidChecksum);
			assertEquals("Version mismatch, should be " + Packets.IPVersions_Fields.IPV4 + ", but is " + _synAck.Version, Packets.IPVersions_Fields.IPV4, _synAck.Version);
			assertEquals("TOS incorrect, should be 0, but is " + _synAck.TypeOfService, 0, _synAck.TypeOfService);
			assertEquals("Length incorrect, should be 44, but is " + _synAck.Length, 44, _synAck.Length);
			assertEquals("ID incorrect, should be 0x9383, but is " + _synAck.Id, 0x9383, _synAck.Id);
			assertEquals("Fragment flags incorrect, should be 0, but are " + _synAck.FragmentFlags, 2, _synAck.FragmentFlags);
			assertEquals("Fragment offset incorrect, should be 0, but is " + _synAck.FragmentOffset, 0, _synAck.FragmentOffset);
			assertEquals("Time-to-live incorrect, should be 255, but is " + _synAck.TimeToLive, 255, _synAck.TimeToLive);
		}
		
		public virtual void  testPshAckPacketHeaderValues()
		{
			assertEquals(Packets.IPProtocols_Fields.TCP, _pshAck.IPProtocol);
			assertEquals(Packets.IPProtocols_Fields.TCP, _pshAck.Protocol);
			assertEquals("IP Checksum mismatch, should be 0x38a2, but is " + System.Convert.ToString(_pshAck.IPChecksum, 16), 0x38a2, _pshAck.IPChecksum);
			assertEquals("(IP) Checksum mismatch, should be 0x38a2, but is " + System.Convert.ToString(_pshAck.Checksum, 16), 0x38a2, _pshAck.Checksum);
			IPPacket.TestProbe probe = new Packets.IPPacket.TestProbe(_pshAck);
			assertTrue("Computed IP checksum mismatch, should be " + System.Convert.ToString(_pshAck.IPChecksum, 16) + ", but is " + System.Convert.ToString(probe.ComputedSenderIPChecksum, 16) + ", (" + System.Convert.ToString(probe.ComputedReceiverIPChecksum, 16) + ")", _pshAck.ValidChecksum);
			assertEquals("Version mismatch, should be " + Packets.IPVersions_Fields.IPV4 + ", but is " + _pshAck.Version, Packets.IPVersions_Fields.IPV4, _pshAck.Version);
			assertEquals("TOS incorrect, should be 0, but is " + _pshAck.TypeOfService, 0, _pshAck.TypeOfService);
			assertEquals("Length incorrect, should be 62, but is " + _pshAck.Length, 62, _pshAck.Length);
			assertEquals("ID incorrect, should be 0x8708, but is " + _pshAck.Id, 0x8708, _pshAck.Id);
			assertEquals("Fragment flags incorrect, should be 0, but are " + _pshAck.FragmentFlags, 2, _pshAck.FragmentFlags);
			assertEquals("Fragment offset incorrect, should be 0, but is " + _pshAck.FragmentOffset, 0, _pshAck.FragmentOffset);
			assertEquals("Time-to-live incorrect, should be 63, but is " + _pshAck.TimeToLive, 63, _pshAck.TimeToLive);
		}
		
		public virtual void  testBadPacketHeaderLengths()
		{
			// really just make sure this doesn't crash the thing
			assertTrue("Bad read of IP header for random data", (_baddie.IPHeader.Length >= 0));
			assertTrue("Bad read of IP header for random data", (_baddie.Header.Length >= 0));
		}
		
		public virtual void  testBadPacketDataLengths()
		{
			// really just make sure this doesn't crash the thing
			assertTrue("Bad read of IP data (payload) for random data", (_baddie.IPData.Length >= 0));
			assertTrue("Bad read of IP data (payload) for random data", (_baddie.Data.Length >= 0));
		}
	}
}