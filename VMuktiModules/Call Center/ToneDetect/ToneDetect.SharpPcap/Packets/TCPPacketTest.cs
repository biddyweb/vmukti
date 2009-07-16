// $Id: TCPPacketTest.java,v 1.1 2002/07/10 23:12:09 pcharles Exp $

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
	
	public class TCPPacketTest:TestCase
	{
		
		// yes, I realize that as TCP packets, these are not SYN-ACK and 
		// PSH-ACKs, but I use the same shell for testing TCPPacket
		private TCPPacket _synAck;
		private TCPPacket _pshAck;
		// and bad is always bad
		private TCPPacket _baddie;
		
		public TCPPacketTest(System.String testName):base(testName)
		{
			SYN_ACK_PACKET = new byte[]{(byte) (0x00), (byte) (0x10), (byte) (0x7b), (byte) (0x38), (byte) (0x46), (byte) (0x33), (byte) (0x08), (byte) (0x00), (byte) (0x20), (byte) (0x89), (byte) (0xa5), (byte) (0x9f), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x2c), (byte) (0x93), (byte) (0x83), (byte) (0x40), (byte) (0x00), (byte) (0xff), (byte) (0x06), (byte) (0x6c), (byte) (0x38), (byte) (0xac), (byte) (0x10), (byte) (0x70), (byte) (0x32), (byte) (0x87), (byte) (0x0d), (byte) (0xd8), (byte) (0xbf), (byte) (0x00), (byte) (0x19), (byte) (0x50), (byte) (0x49), (byte) (0x78), (byte) (0xbe), (byte) (0xe0), (byte) (0xa7), (byte) (0x9f), (byte) (0x3a), (byte) (0xb4), (byte) (0x03), (byte) (0x60), (byte) (0x12), (byte) (0x22), (byte) (0x38), (byte) (0xfc), (byte) (0xc7), (byte) (0x00), (byte) (0x00), (byte) (0x02), (byte) (0x04), (byte) (0x05), (byte) (0xb4), (byte) (0x70), (byte) (0x6c)};
			PSH_ACK_PACKET = new byte[]{(byte) (0x08), (byte) (0x00), (byte) (0x20), (byte) (0x89), (byte) (0xa5), (byte) (0x9f), (byte) (0x00), (byte) (0x10), (byte) (0x7b), (byte) (0x38), (byte) (0x46), (byte) (0x33), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x3e), (byte) (0x87), (byte) (0x08), (byte) (0x40), (byte) (0x00), (byte) (0x3f), (byte) (0x06), (byte) (0x38), (byte) (0xa2), (byte) (0x87), (byte) (0x0d), (byte) (0xd8), (byte) (0xbf), (byte) (0xac), (byte) (0x10), (byte) (0x70), (byte) (0x32), (byte) (0x50), (byte) (0x49), (byte) (0x00), (byte) (0x19), (byte) (0x9f), (byte) (0x3a), (byte) (0xb4), (byte) (0x03), (byte) (0x78), (byte) (0xbe), (byte) (0xe0), (byte) (0xf8), (byte) (0x50), (byte) (0x18), (byte) (0x7d), (byte) (0x78), (byte) (0x86), (byte) (0xf0), (byte) (0x00), (byte) (0x00), (byte) (0x45), (byte) (0x48), (byte) (0x4c), (byte) (0x4f), (byte) (0x20), (byte) (0x61), (byte) (0x6c), (byte) (0x70), (byte) (0x68), (byte) (0x61), (byte) (0x2e), (byte) (0x61), (byte) (0x70), (byte) (0x70), (byte) (0x6c), (byte) (0x65), (byte) (0x2e), (byte) (0x65), (byte) (0x64), (byte) (0x75), (byte) (0x0d), (byte) (0x0a)};
		}
		
		[STAThread]
		public static void  Main1(System.String[] args)
		{
			TCPPacketTest test = new TCPPacketTest("TCPPacket Test");
			test.setUp();

			test.testSynAckPacketHeaderValues();
			test.testSynAckPacketHeaderLengths();
			test.testSynAckPacketDataLengths();
			//test.testSynAckPacketAddresses();
			test.testPshAckPacketHeaderValues();
			test.testPshAckPacketHeaderLengths();
			test.testPshAckPacketDataLengths();
			test.testPshAckPacketAddresses();
			test.testBadPacketHeaderLengths();
			test.testBadPacketDataLengths();
			test.tamirTest();

		}
		
		//UPGRADE_NOTE: The initialization of  'SYN_ACK_PACKET' was moved to static method 'SharpPcap.Packets.TCPPacketTest'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static byte[] SYN_ACK_PACKET;
		//UPGRADE_NOTE: The initialization of  'PSH_ACK_PACKET' was moved to static method 'SharpPcap.Packets.TCPPacketTest'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static byte[] PSH_ACK_PACKET;

		public virtual void tamirTest()
		{
			Console.WriteLine("{0}:{1}>{2}:{3}",_synAck.SourceAddress, _synAck.SourcePort, _synAck.DestinationAddress, _synAck.DestinationPort);
			Console.WriteLine(_synAck.DestinationHwAddress);
			Console.WriteLine(_synAck.Fin);
		}
		
		public virtual void  setUp()
		{
			// get link layer length
			int linkLayerLen = LinkLayer.getLinkLayerLength(Packets.LinkLayers.EN10MB);
			// create syn-ack packet
			_synAck = new TCPPacket(linkLayerLen, SYN_ACK_PACKET);
			// create psh-ack packet
			_pshAck = new TCPPacket(linkLayerLen, PSH_ACK_PACKET);
			// create packet with random garbage
			while(true)
			{
				try
				{
					byte[] badBytes = new byte[SYN_ACK_PACKET.Length];
					(new System.Random()).NextBytes((badBytes));
					_baddie = new TCPPacket(linkLayerLen, badBytes);
					break;
				}
				catch{}				
			}
		}
		
		public virtual void  tearDown()
		{
		}
		
		public virtual void  testSynAckPacketHeaderLengths()
		{
			assertEquals(24, _synAck.TCPHeaderLength);
			assertEquals(24, _synAck.TCPHeader.Length);
			assertEquals(24, _synAck.HeaderLength);
			assertEquals(24, _synAck.Header.Length);
		}
		
		public virtual void  testPshAckPacketHeaderLengths()
		{
			assertEquals(20, _pshAck.TCPHeaderLength);
			assertEquals(20, _pshAck.TCPHeader.Length);
			assertEquals(20, _pshAck.HeaderLength);
			assertEquals(20, _pshAck.Header.Length);
		}
		
		public virtual void  testSynAckPacketDataLengths()
		{
			assertEquals(0, _synAck.TCPData.Length);
			assertEquals(0, _synAck.Data.Length);
		}
		
		public virtual void  testPshAckPacketDataLengths()
		{
			assertEquals(22, _pshAck.TCPData.Length);
			assertEquals(22, _pshAck.Data.Length);
		}
		
		public virtual void  testSynAckPacketPorts()
		{
			assertEquals(25, _synAck.SourcePort);
			assertEquals(20553, _synAck.DestinationPort);
		}
		
		public virtual void  testPshAckPacketAddresses()
		{
			assertEquals(20553, _pshAck.SourcePort);
			assertEquals(25, _pshAck.DestinationPort);
		}
		
		public virtual void  testSynAckPacketHeaderValues()
		{
			assertEquals(2025775271L, _synAck.SequenceNumber);
			assertEquals(2671424515L, _synAck.AcknowledgmentNumber);
			assertEquals(8760, _synAck.WindowSize);
			assertEquals(0xfcc7, _synAck.TCPChecksum);
			assertEquals(0xfcc7, _synAck.Checksum);
			//   	assertTrue   ("Packet should checksum",_synAck.isValidChecksum ());
			assertEquals(0, _synAck.UrgentPointer);
			assertTrue(!_synAck.Urg);
			assertTrue(_synAck.Ack);
			assertTrue(!_synAck.Psh);
			assertTrue(!_synAck.Rst);
			assertTrue(_synAck.Syn);
			assertTrue(!_synAck.Fin);
		}
		
		public virtual void  testPshAckPacketHeaderValues()
		{
			assertEquals(2671424515L, _pshAck.SequenceNumber);
			assertEquals(2025775352L, _pshAck.AcknowledgmentNumber);
			assertEquals(32120, _pshAck.WindowSize);
			assertEquals(0x86f0, _pshAck.TCPChecksum);
			assertEquals(0x86f0, _pshAck.Checksum);
			//   	assertTrue   ("Packet should checksum",_pshAck.isValidChecksum ());
			assertEquals(0, _pshAck.UrgentPointer);
			assertTrue(!_pshAck.Urg);
			assertTrue(_pshAck.Ack);
			assertTrue(_pshAck.Psh);
			assertTrue(!_pshAck.Rst);
			assertTrue(!_pshAck.Syn);
			assertTrue(!_pshAck.Fin);
		}
		
		public virtual void  testBadPacketHeaderLengths()
		{
			// really just make sure this doesn't crash the thing
			assertTrue("Bad read of TCP header for random data", (_baddie.TCPHeader.Length >= 0));
			assertTrue("Bad read of TCP header for random data", (_baddie.Header.Length >= 0));
		}
		
		public virtual void  testBadPacketDataLengths()
		{
			// really just make sure this doesn't crash the thing
			assertTrue("Bad read of TCP data (payload) for random data", (_baddie.TCPData.Length >= 0));
			assertTrue("Bad read of TCP data (payload) for random data", (_baddie.Data.Length >= 0));
		}
	}
}