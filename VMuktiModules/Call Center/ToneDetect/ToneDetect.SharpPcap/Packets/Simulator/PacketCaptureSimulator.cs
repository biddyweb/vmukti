//// $Id: PacketCaptureSimulator.java,v 1.8 2002/02/18 21:52:31 pcharles Exp $
//
///// <summary>************************************************************************
///// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
///// Distributed under the Mozilla Public License                            *
///// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
///// *************************************************************************
///// </summary>
//namespace SharpPcap.Packets.Simulator
//{
//	using System;
//	using ArrayHelper = SharpPcap.Packets.Util.ArrayHelper;
//	using LinkLayers = SharpPcap.Packets.LinkLayers;
//	using InvalidFilterException = net.sourceforge.jpcap.capture.InvalidFilterException;
//	using PacketCaptureBase = net.sourceforge.jpcap.capture.PacketCaptureBase;
//	using PacketCaptureCapable = net.sourceforge.jpcap.capture.PacketCaptureCapable;
//	using CaptureStatistics = net.sourceforge.jpcap.capture.CaptureStatistics;
//	using CapturePacketException = net.sourceforge.jpcap.capture.CapturePacketException;
//	using CaptureConfigurationException = net.sourceforge.jpcap.capture.CaptureConfigurationException;
//	using CaptureDeviceLookupException = net.sourceforge.jpcap.capture.CaptureDeviceLookupException;
//	using CaptureDeviceOpenException = net.sourceforge.jpcap.capture.CaptureDeviceOpenException;
//	using CaptureDeviceNotFoundException = net.sourceforge.jpcap.capture.CaptureDeviceNotFoundException;
//	/// <summary> This class has the same external interface as PacketCapture.
//	/// <p>
//	/// Instead of capturing and dispatching packets from a physical network
//	/// device, however, this class generates fake packets.
//	/// The type and frequency of the packets is set by policies configured in
//	/// the simulator property file.
//	/// <p>
//	/// Instances of this class are used mostly by developers working on
//	/// applications that utilize jpcap in environments where a real 
//	/// network device isn't available or when the type of packets arriving 
//	/// needs to be carefully controlled.
//	/// <p>
//	/// For more documentation on this class's methods, see PacketCaptureCapable;
//	/// Javadoc is 'inherited' from this interface.
//	/// *
//	/// </summary>
//	/// <author>  Patrick Charles and Jonas Lehmann
//	/// </author>
//	/// <version>  $Revision: 1.8 $
//	/// @lastModifiedBy $Author: pcharles $
//	/// @lastModifiedAt $Date: 2002/02/18 21:52:31 $
//	/// 
//	/// </version>
//	public class PacketCaptureSimulator:PacketCaptureBase, PacketCaptureCapable
//	{
//		virtual public CaptureStatistics Statistics
//		{
//			get
//			{
//				return new CaptureStatistics(receivedCount, droppedCount);
//			}
//			
//		}
//		virtual public int LinkLayerType
//		{
//			get
//			{
//				return linkType;
//			}
//			
//		}
//		virtual public int SnapshotLength
//		{
//			get
//			{
//				return DEFAULT_SNAPLEN;
//			}
//			
//		}
//		public PacketCaptureSimulator()
//		{
//			// the simulator currently only simulates ethernet with link-level headers
//			linkType = LinkLayers.EN10MB;
//		}
//		
//		
//		// methods for controlling a (simulated) packet capture session
//		
//		public virtual void  open(System.String device, bool promiscuous)
//		{
//			open(device, DEFAULT_SNAPLEN, promiscuous, DEFAULT_TIMEOUT);
//		}
//		
//		public virtual void  open(System.String device, int snaplen, bool promiscuous, int timeout)
//		{
//			// noop
//		}
//		
//		public virtual void  openOffline(System.String fileName)
//		{
//			// noop
//		}
//		
//		public virtual void  setFilter(System.String filterExpression, bool optimize)
//		{
//			// noop. the simulator could potentially parse the filter and 
//			// modify the packet generator's engine accordingly. 
//			// instead, though, prefer to control the generator's behavior
//			// via the simulator properties. the complexities of bpf are 
//			// currently beyond the scope of the simulator. :/
//		}
//		
//		/// <summary> The simulator's implementation of capture causes packets to be 
//		/// randomly generated. The packet frequency and type are configurable.
//		/// </summary>
//		public virtual void  capture(int count)
//		{
//			for (int i = 0; i < count; i++)
//			{
//				byte[] bytes = PacketGenerator.generate();
//				
//				long millis = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
//				int seconds = (int) (millis / 1000);
//				int useconds = (int) (millis * 1000 - seconds * 1000 * 1000);
//				
//				handlePacket(bytes.Length, bytes.Length, seconds, useconds, bytes);
//				receivedCount++;
//			}
//		}
//		
//		
//		public virtual void  close()
//		{
//			// noop
//		}
//		
//		
//		// static native methods to fetch capture device and network information
//		
//		public virtual System.String findDevice()
//		{
//			return "jpcapsim0";
//		}
//		
//		public virtual System.String[] lookupDevices()
//		{
//			return new System.String[]{"jpcapsim0"};
//		}
//		
//		public virtual int getNetwork(System.String device)
//		{
//			return 0x00000000;
//		}
//		
//		public virtual int getNetmask(System.String device)
//		{
//			return 0x00000000;
//		}
//		
//		
//		
//		
//		private System.String _rcsid = "$Id: PacketCaptureSimulator.java,v 1.8 2002/02/18 21:52:31 pcharles Exp $";
//	}
//}