<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.SharpPcap/PcapDevice.cs
/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists      
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
 
*/
=======
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.SharpPcap/PcapDevice.cs
using System;
using System.Text;
using System.Threading;
using ToneDetect.SharpPcap.Util;
using ToneDetect.SharpPcap.Packets;
using System.Runtime.InteropServices;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Capture live packets from a network device
	/// </summary>
	public class PcapDevice
	{
		private SharpPcap.PCAP_IF m_pcapIf;

		private IntPtr		m_pcapAdapterHandle = IntPtr.Zero;
		private IntPtr		m_pcapDumpHandle	= IntPtr.Zero;
		private bool		m_pcapStarted		= false;
		private PcapMode	m_pcapMode			= PcapMode.Capture;
		private int			m_pcapPacketCount	= SharpPcap.INFINITE;//Infinite
		private int			m_ip	= 0;//just for fun
		private int			m_mask	= 0;//for filter expression

		//For thread synchronization
		private ManualResetEvent m_pcapThreadEvent = new ManualResetEvent(true);

		/// <summary>
		/// Constructs a new PcapDevice based on a device name
		/// </summary>
		/// <param name="name">The name of a device.<br>
		/// Can be either in pcap device format or windows network
		/// device format</param>
		internal PcapDevice(string name):this(SharpPcap.GetPcapDeviceStruct(name))
		{
		}

		/// <summary>
		/// Constructs a new PcapDevice based on a 'pcapIf' struct
		/// </summary>
		/// <param name="pcapIf">A 'pcapIf' struct representing
		/// the pcap device
		/// <summary>
		internal PcapDevice( SharpPcap.PCAP_IF pcapIf )
		{
			m_pcapIf = pcapIf;

			if(m_pcapIf.Addresses!=IntPtr.Zero)
			{
				SharpPcap.PCAP_ADDR pcap_addr = 
					SharpPcap.GetPcap_Addr( m_pcapIf.Addresses );
				if(pcap_addr.Addr!=IntPtr.Zero)
					m_ip = System.Net.IPAddress.HostToNetworkOrder( SharpPcap.GetPcapAddress( pcap_addr.Addr ) );
				if(pcap_addr.Netmask!=IntPtr.Zero)
					m_mask = System.Net.IPAddress.HostToNetworkOrder( SharpPcap.GetPcapAddress( pcap_addr.Netmask ));
			}
		}

		/// <summary>
		/// Default contructor for subclasses
		/// </summary>
		protected PcapDevice()
		{
		}

		/// <summary>
		/// Fires whenever a new packet is received on this Pcap Device.<br>
		/// This event is invoked only when working in "PcapMode.Capture" mode.
		/// </summary>
		public event SharpPcap.PacketArrivalEvent PcapOnPacketArrival;

		/// <summary>
		/// Fires whenever a new pcap statistics is available for this Pcap Device.<br>
		/// This event is invoked only when working in "PcapMode.Statistics" mode.
		/// </summary>
		public event SharpPcap.PcapStatisticsEvent PcapOnPcapStatistics;

		/// <summary>
		/// Fired when the capture process of this pcap device is stopped
		/// </summary>
		public event SharpPcap.PcapCaptureStoppedEvent PcapOnCaptureStopped;
		
		/// <summary>
		/// Gets the pcap name of this network device
		/// </summary>
		public virtual string PcapName
		{
			get{return m_pcapIf.Name;}
		}

		/// <summary>
		/// Gets the pcap description of this device
		/// </summary>
		public virtual string PcapDescription
		{
			get{return m_pcapIf.Description;}
		}

		public virtual uint PcapFlags
		{
			get{return m_pcapIf.Flags;}
		}

		public virtual bool PcapLoopback
		{
			get{return (PcapFlags&SharpPcap.PCAP_IF_LOOPBACK)==1;}
		}

		public virtual string PcapIpAddress
		{
			get{return Util.Convert.IpInt32ToString(m_ip);}
		}

		/// <summary>
		/// The underlying pcap device handle
		/// </summary>
		internal virtual IntPtr PcapHandle
		{
			get{return m_pcapAdapterHandle;}
			set{m_pcapAdapterHandle=value;}
		}

		/// <summary>
		/// Return the pcap link layer value of an adapter. 
		/// </summary>
		public virtual int PcapDataLink
		{
			get
			{
				if(!PcapOpened)
					throw new InvalidOperationException("Cannot get datalink, the pcap device is not opened");
				return SharpPcap.pcap_datalink(PcapHandle);
			}
		}

		/// <summary>
		/// Return a value indicating if this adapter is opened
		/// </summary>
		public virtual bool PcapOpened
		{
			get{return (PcapHandle != IntPtr.Zero);}
		}

		/// <summary>
		/// Return a value indicating if the capturing process of this adapter is started
		/// </summary>
		public virtual bool PcapStarted
		{
			get{return m_pcapStarted;}
		}

		public virtual PcapMode PcapMode
		{
			get{return m_pcapMode;}
			set
			{
				if(!PcapOpened)
					throw new PcapException
						("Can't set PcapMode, the device is not opened");

				m_pcapMode = value;
				int mode = ( m_pcapMode==PcapMode.Capture ? 
							 SharpPcap.MODE_CAPT : 
							 SharpPcap.MODE_STAT);
				SharpPcap.pcap_setmode(this.PcapHandle ,mode);
			}
		}

		/// <summary>
		/// Open the device with default values of: promiscuous_mode=false, read_timeout=1000
		/// To start capturing call the 'PcapStartCapture' function
		/// </summary>
		public virtual void PcapOpen()
		{
			this.PcapOpen(false);
		}

		/// <summary>
		/// Open the device. To start capturing call the 'PcapStartCapture' function
		/// </summary>
		/// <param name="promiscuous_mode">A value indicating wether to open the
		///  device in promiscuous mode (true = capture *all* packets on the network,
		///  including packets not for me)</param>
		public virtual void PcapOpen(bool promiscuous_mode)
		{
			this.PcapOpen( promiscuous_mode, 1000 );
		}

		/// <summary>
		/// Open the device. To start capturing call the 'PcapStartCapture' function
		/// </summary>
		/// <param name="promiscuous_mode">A value indicating wether to open the
		///  device in promiscuous mode (true = capture *all* packets on the network,
		///  including packets not for me)</param>
		/// <param name="read_timeout">The timeout in miliseconds to wait for a  packet arrival</param>
		public virtual void PcapOpen(bool promiscuous_mode, int read_timeout)
		{
			short mode = 0;
			if (promiscuous_mode) mode = 1;

			if ( !PcapOpened )
			{
				StringBuilder errbuf = new StringBuilder( SharpPcap.PCAP_ERRBUF_SIZE ); //will hold errors

				PcapHandle = SharpPcap.pcap_open_live
					(	PcapName,			// name of the device
						SharpPcap.MAX_PACKET_SIZE,	// portion of the packet to capture. 
											// MAX_PACKET_SIZE (65536) grants that the whole packet will be captured on all the MACs.
						mode,				// promiscuous mode
						(short)read_timeout,// read timeout												
						errbuf ); 			// error buffer

				if ( PcapHandle == IntPtr.Zero)
				{
					string err = "Unable to open the adapter ("+PcapName+"). "+errbuf.ToString();
					throw new Exception( err );
				}
			}
		}

		/// <summary>
		/// Starts the capturing process
		/// </summary>
		public virtual void PcapStartCapture()
		{
			if(!PcapStarted)
			{
				if ( !PcapOpened )
				{
					throw new Exception("Can't start capture, the pcap device is not opened.");
				}
				Thread thSniff = new Thread(new ThreadStart(this.PcapCaptureLoop));
				thSniff.Start();
				m_pcapThreadEvent.Reset();
				m_pcapThreadEvent.WaitOne();
			}
		}

		/// <summary>
		/// Captures packets on this network device. This method will block
		/// until capturing is finished.
		/// </summary>
		/// <param name="packetCount">The number of packets to be captured. 
		/// Value of '-1' means infinite.</param>
		public virtual void PcapCapture(int packetCount)
		{
			m_pcapPacketCount = packetCount;
			PcapCaptureLoop();
			m_pcapPacketCount = SharpPcap.INFINITE;;
		}

		/// <summary>
		/// Stops the capture process
		/// </summary>
		public virtual void PcapStopCapture()
		{
			if (PcapStarted)
			{
				m_pcapStarted = false;
				WaitForStop();
			}
		}

		/// <summary>
		/// This method blocks until the capturing process is stopped
		/// </summary>
		public virtual void WaitForStop()
		{
			if (PcapStarted)
			{
				m_pcapThreadEvent.Reset();
				m_pcapThreadEvent.WaitOne();
			}
		}

		/// <summary>
		/// Closes this adapter
		/// </summary>
		public virtual void PcapClose()
		{
			if(PcapHandle==IntPtr.Zero)
				return;

			if (PcapStarted)
			{
				PcapStopCapture();
			}
			SharpPcap.pcap_close(PcapHandle);
			PcapHandle = IntPtr.Zero;
			
			//Remove event handlers
			if ( PcapOnPacketArrival != null)
			{
				foreach(SharpPcap.PacketArrivalEvent pa in PcapOnPacketArrival.GetInvocationList())
				{
					PcapOnPacketArrival -= pa;
				}
			}
			if ( PcapOnPcapStatistics != null)
			{
				foreach(SharpPcap.PcapStatisticsEvent pse in PcapOnPcapStatistics.GetInvocationList())
				{
					PcapOnPcapStatistics -= pse;
				}
			}
		}

		/// <summary>
		/// Gets the next packet captured on this device
		/// </summary>
		/// <returns>The next packet captured on this device</returns>
		public virtual Packet PcapGetNextPacket()
		{
			Packet p;
			int res = PcapGetNextPacket( out p );
			if(res==-1)
				throw new PcapException("Error receiving packet.");
			return p;
		}
		
		/// <summary>
		/// Gets the next packet captured on this device
		/// </summary>
		/// <param name="p">A packet reference</param>
		/// <returns>A reference to a packet object</returns
		public virtual int PcapGetNextPacket(out Packet p)
		{
			//Pointer to a packet info struct
			IntPtr header = IntPtr.Zero;
			//Pointer to a packet struct
			IntPtr data = IntPtr.Zero;
			int res = 0;

			//Get a packet from winpcap
			res = SharpPcap.pcap_next_ex( PcapHandle, ref header, ref data);
			p=null;

			if(res>0)
			{
				//Marshal the packet
				if ( (header != IntPtr.Zero) && (data != IntPtr.Zero) )
				{
					SharpPcap.PCAP_PKTHDR pkt_header = (SharpPcap.PCAP_PKTHDR)Marshal.PtrToStructure( header, typeof(SharpPcap.PCAP_PKTHDR) );
					//SharpPcap.PCAP_PKTDATA pkt_data = (SharpPcap.PCAP_PKTDATA)Marshal.PtrToStructure( data, typeof(SharpPcap.PCAP_PKTDATA) );
					byte[] pkt_data = new byte[pkt_header.caplen];
					Marshal.Copy(data, pkt_data, 0, pkt_header.caplen);
					p = Packets.PacketFactory.dataToPacket(PcapDataLink, pkt_data, new Packets.Util.Timeval(pkt_header.tv_sec, pkt_header.tv_usec));
					p.PcapHeader = new PcapHeader( pkt_header );
				}
			}
			return res;
		}

		/// <summary>
		/// The capture procedure
		/// </summary>
		protected virtual void PcapCaptureLoop()
		{
			//Set the 'started' flag
			m_pcapStarted = true;
			//Notify waiting threads that we started
			m_pcapThreadEvent.Set();
			//holds the captured packets
			Packet p = null;
			//counts the captured pcakers
			uint packetCount = 0;
			//read result value
			int res=0;

			while ( (m_pcapStarted) && (res>=0) )
			{
				try
				{
					if(m_pcapPacketCount != SharpPcap.INFINITE)
					{
						//check for packet count limit
						if (packetCount >= m_pcapPacketCount)
						{
							m_pcapStarted=false;
							break;
						}
					}
					//Capture a packet
					res=PcapGetNextPacket( out p );


					if(res==0)/* Timeout elapsed */
						continue;
				
					//If captured ok, send event
					if(p != null)
					{
						//increment packet count
						packetCount++;
						//notify upper application of this packet
						SendPacketArrivalEvent(p);
					}
				}
				catch(Exception e)
				{
					//Notify upper application
					SendCaptureStoppedEvent(true);
					//Notify waiting threads
					m_pcapThreadEvent.Set();
					//re-throw the exception
					throw e;
				}
			}
			//Notify upper application
			SendCaptureStoppedEvent(false);
			//Notify waiting threads
			m_pcapThreadEvent.Set();
		}

		private void SendPacketArrivalEvent(Packet p)
		{
			//If mode is MODE_CAP:
			if(PcapMode==PcapMode.Capture)
			{
				if(PcapOnPacketArrival != null )
				{
					//Invoke the packet arrival event											
					PcapOnPacketArrival(this, p);
				}
			}
			//else mode is MODE_STAT
			else if(PcapMode==PcapMode.Statistics)
			{
				if(PcapOnPcapStatistics != null)
				{
					//Invoke the pcap statistics event
					PcapOnPcapStatistics(this, new PcapStatistics(p));
				}
			}
		}

		private void SendCaptureStoppedEvent(bool error)
		{
			if(PcapOnCaptureStopped!=null)
			{
				//Notify upper applications
				PcapOnCaptureStopped(this, error);
			}
		}

		/// <summary>
		/// Compile a kernel level filtering expression, and associate the filter 
		/// with this device. For more info on filter expression syntax, see:
		/// http://www.winpcap.org/docs/docs31/html/group__language.html
		/// </summary>
		/// <param name="filterExpression">The filter expression to 
		/// compile</param>
		public virtual void PcapSetFilter(string filterExpression)
		{
			int res; IntPtr err_ptr; string err="";

			//pointer to a bpf_program struct 
			IntPtr program = IntPtr.Zero;
			//Alocate an unmanaged buffer
			program = Marshal.AllocHGlobal( Marshal.SizeOf(typeof(SharpPcap.bpf_program)));
			//compile the expreesions
			res = SharpPcap.pcap_compile(PcapHandle, program, filterExpression,1, (uint)m_mask);
			//watch for errors
			if(res<0)
			{
				try
				{
					err_ptr = SharpPcap.pcap_geterr( PcapHandle );
					err = Marshal.PtrToStringAnsi( err_ptr );
				}
				catch{}
				err = "Can't compile filter: "+err;
				throw new PcapException(err);
			}
			//associate the filter with this device
			res = SharpPcap.pcap_setfilter( PcapHandle, program );
			//watch for errors
			if(res<0)
			{
				try
				{
					err_ptr = SharpPcap.pcap_geterr(PcapHandle);
					err = Marshal.PtrToStringAnsi(err_ptr);
				}
				catch{}
				err = "Can't set filter.\n"+err;
				throw new PcapException(err);
			}
			//free allocated buffers
			Marshal.FreeHGlobal(program);
		}

		/// <summary>
		/// Opens a file for packet writings
		/// </summary>
		/// <param name="fileName"></param>
		public void PcapDumpOpen(string fileName)
		{
			m_pcapDumpHandle = SharpPcap.pcap_dump_open(PcapHandle, fileName);
			if(m_pcapDumpHandle==IntPtr.Zero)
				throw new Exception("Error openning dump file.");
		}

		/// <summary>
		/// Writes a packet to the pcap dump file associated with this device.
		/// </summary>
		/// <param name="p">The packet to write</param>
		public void PcapDump(byte[] p, PcapHeader h)
		{
			if(!PcapOpened)
				throw new InvalidOperationException("Cannot dump packet, device is not opened");
			if(!PcapDumpOpened)
				throw new InvalidOperationException("Cannot dump packet, dump file is not opened");

			//Marshal packet
			IntPtr pktPtr;
			pktPtr = Marshal.AllocHGlobal(p.Length);
			Marshal.Copy(p, 0, pktPtr, p.Length);

			//Marshal header
			IntPtr hdrPtr;
			hdrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SharpPcap.PCAP_PKTHDR)));
			Marshal.StructureToPtr(h.m_pcap_pkthdr, hdrPtr, true);

			SharpPcap.pcap_dump(m_pcapDumpHandle, hdrPtr, pktPtr);

			Marshal.FreeHGlobal(pktPtr);
			Marshal.FreeHGlobal(hdrPtr);
		}

		/// <summary>
		/// Writes a packet to the pcap dump file associated with this device.
		/// </summary>
		/// <param name="p">The packet to write</param>
		public void PcapDump(byte[] p)
		{
			PcapDump(p, new PcapHeader(0, 0, p.Length,p.Length));
		}

		/// <summary>
		/// Writes a packet to the pcap dump file associated with this device.
		/// </summary>
		/// <param name="p">The packet to write</param>
		public void PcapDump(Packet p)
		{
			PcapDump(p.Bytes, p.PcapHeader);
		}

		/// <summary>
		/// Gets a value indicating wether pcap dump file is already associated with this device
		/// </summary>
		public bool PcapDumpOpened
		{
			get{return m_pcapDumpHandle!=IntPtr.Zero;}
		}

		/// <summary>
		/// Sends a raw packet throgh this device
		/// </summary>
		/// <param name="p">The packet to send</param>
		public void PcapSendPacket(Packet p)
		{
			PcapSendPacket(p.Bytes);
		}


		/// <summary>
		/// Sends a raw packet throgh this device
		/// </summary>
		/// <param name="p">The packet to send</param>
		/// <param name="size">The number of bytes to send</param>
		public void PcapSendPacket(Packet p, int size)
		{
			PcapSendPacket(p.Bytes, size);
		}

		/// <summary>
		/// Sends a raw packet throgh this device
		/// </summary>
		/// <param name="p">The packet bytes to send</param>
		public void PcapSendPacket(byte[] p)
		{
			PcapSendPacket(p, p.Length);
		}

		/// <summary>
		/// Sends a raw packet throgh this device
		/// </summary>
		/// <param name="p">The packet bytes to send</param>
		/// <param name="size">The number of bytes to send</param>
		public void PcapSendPacket(byte[] p, int size)
		{
			if ( PcapOpened )
			{
				if (size > p.Length)
				{
					throw new ArgumentException("Invalid packetSize value: "+size+
					"\nArgument size is larger than the total size of the packet.");
				}

				if (p.Length > SharpPcap.MAX_PACKET_SIZE) 
				{
					throw new ArgumentException("Packet length can't be larger than "+SharpPcap.MAX_PACKET_SIZE);
				}

				IntPtr p_packet = IntPtr.Zero;			
				p_packet = Marshal.AllocHGlobal( size );
				Marshal.Copy(p, 0, p_packet, size);		

				int res = SharpPcap.pcap_sendpacket(PcapHandle, p_packet, size);
				Marshal.FreeHGlobal(p_packet);
				if(res<0)
					throw new PcapException("Can't send packet: "+PcapLastError);
			}
			else
			{
				throw new PcapException("Can't send packet, the device is closed");
			}
		}

		/// <summary>
		/// Sends all packets in a 'PcapSendQueue' out this pcap device
		/// </summary>
		/// <param name="q">The 'PcapSendQueue' hodling the packets</param>
		public int PcapSendQueue( PcapSendQueue q, bool sync )
		{
			return q.Transmit( this, sync );
		}

		/// <summary>
		/// The last pcap error associated with this pcap device
		/// </summary>
		public string PcapLastError
		{
			get
			{
				IntPtr err_ptr = SharpPcap.pcap_geterr( PcapHandle );
				return Marshal.PtrToStringAnsi( err_ptr );
			}
		}
	}
}
