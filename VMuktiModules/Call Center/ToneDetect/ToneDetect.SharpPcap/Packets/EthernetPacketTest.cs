//// $Id: EthernetPacketTest.java,v 1.1 2002/07/10 23:12:09 pcharles Exp $
//
///// <summary>************************************************************************
///// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
///// Distributed under the Mozilla Public License                            *
///// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
///// *************************************************************************
///// </summary>
//namespace SharpPcap.Packets
//{
//	using System;
//	////using junit.framework;
//	using PacketGenerator = SharpPcap.Packets.Simulator.PacketGenerator;
//	
//	public class EthernetPacketTest:TestCase
//	{
//		
//		private EthernetPacket _goodie;
//		private EthernetPacket _baddie;
//		
//		private const int IP_PROTOCOL = 0x0800;
//		
//		public EthernetPacketTest(System.String testName):base(testName)
//		{
//		}
//		
//		[STAThread]
//		public static void  Main(System.String[] args)
//		{
//			//junit.swingui.TestRunner.main (new String[] {EthernetPacketTest.class.getName ()});
//			junit.textui.TestRunner.run(suite());
//		}
//		
//		public static Test suite()
//		{
//			TestSuite suite = new TestSuite(typeof(EthernetPacketTest));
//			return suite;
//		}
//		
//		//UPGRADE_NOTE: The initialization of  'GOOD_PACKET' was moved to static method 'SharpPcap.Packets.EthernetPacketTest'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
//		private static byte[] GOOD_PACKET;
//		
//		public virtual void  setUp()
//		{
//			int linkLayerLen = LinkLayer.getLinkLayerLength(SharpPcap.Packets.LinkLayers.EN10MB);
//			_goodie = new EthernetPacket(linkLayerLen, GOOD_PACKET);
//			byte[] badBytes = new byte[GOOD_PACKET.Length];
//			(new System.Random()).NextBytes((badBytes));
//			_baddie = new EthernetPacket(linkLayerLen, badBytes);
//		}
//		
//		public virtual void  tearDown()
//		{
//		}
//		
//		public virtual void  testGoodPacketHeaderLengths()
//		{
//			assertEquals(14, _goodie.EthernetHeaderLength);
//			assertEquals(14, _goodie.EthernetHeader.Length);
//			assertEquals(14, _goodie.HeaderLength);
//			assertEquals(14, _goodie.Header.Length);
//		}
//		
//		public virtual void  testGoodPacketDataLengths()
//		{
//			assertEquals(52, _goodie.EthernetData.Length);
//			assertEquals(52, _goodie.Data.Length);
//		}
//		
//		public virtual void  testGoodPacketAddresses()
//		{
//			assertEquals("01:02:03:04:05:06", _goodie.SourceHwAddress);
//			assertEquals("00:04:76:ba:86:19", _goodie.DestinationHwAddress);
//		}
//		
//		public virtual void  testGoodPacketProtocol()
//		{
//			assertEquals(SharpPcap.Packets.EthernetProtocols_Fields.IP, _goodie.EthernetProtocol);
//			assertEquals(SharpPcap.Packets.EthernetProtocols_Fields.IP, _goodie.Protocol);
//		}
//		
//		public virtual void  testBadPacketHeaderLengths()
//		{
//			assertEquals(14, _baddie.EthernetHeaderLength);
//			assertEquals(14, _baddie.EthernetHeader.Length);
//		}
//		
//		public virtual void  testBadPacketDataLengths()
//		{
//			assertEquals(52, _baddie.EthernetData.Length);
//			assertEquals(52, _baddie.Data.Length);
//		}
//		static EthernetPacketTest()
//		{
//			GOOD_PACKET = new byte[]{(byte) (0x00), (byte) (0x04), (byte) (0x76), (byte) (0xba), (byte) (0x86), (byte) (0x19), (byte) (0x01), (byte) (0x02), (byte) (0x03), (byte) (0x04), (byte) (0x05), (byte) (0x06), (byte) (0x08), (byte) (0x00), (byte) (0x45), (byte) (0x00), (byte) (0x00), (byte) (0x2c), (byte) (0x04), (byte) (0x45), (byte) (0x20), (byte) (0x00), (byte) (0x40), (byte) (0x06), (byte) (0xc2), (byte) (0x56), (byte) (0x0a), (byte) (0x32), (byte) (0x01), (byte) (0x52), (byte) (0xc0), (byte) (0xa8), (byte) (0xc8), (byte) (0x04), (byte) (0x04), (byte) (0x4b), (byte) (0x00), (byte) (0x19), (byte) (0x83), (byte) (0xbd), (byte) (0x76), (byte) (0x5c), (byte) (0x7a), (byte) (0xc0), (byte) (0x7f), (byte) (0xbd), (byte) (0x80), (byte) (0x11), (byte) (0x19), (byte) (0x20), (byte) (0xd6), (byte) (0xde), (byte) (0x00), (byte) (0x00), (byte) (0x01), (byte) (0x01), (byte) (0x08), (byte) (0x0a), (byte) (0x01), (byte) (0x17), (byte) (0x75), (byte) (0x84), (byte) (0x01), (byte) (0xb9), (byte) (0x81), (byte) (0x3c)};
//		}
//	}
//}