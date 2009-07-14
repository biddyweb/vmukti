using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

using ToneDetect.SharpPcap;
using ToneDetect.SharpPcap.Packets;
using VMuktiAPI;

namespace ToneDetect.Business.SIP
{
	public class SniffSIP
	{
		#region Fields
		/// <summary>
		/// the instance variable for the singleton pattern.
		/// </summary>
		private static SniffSIP instance = null;

		/// <summary>
		/// This is the IP address of the local machine.  This IP will be used to
		/// examine IP addresses from the device list.  Also, incoming packets are
		/// identified by the destination address of the packet matching this string.
		/// </summary>
		private string ipAddress;

		/// <summary>
		/// The Pcap Device the software is listening on.
		/// </summary>
		private PcapDevice pcapDevice;

		private int packetCount;

		/// <summary>
		/// This delegate is used to indicate the signature of callbacks
		/// hooked in when a SIP packet arrives.
		/// </summary>
		/// <param name="sender">(this)</param>
		/// <param name="parser">the SIP parser</param>
		public delegate void SipArrivedDelegate( object sender, SipParser parser );

		/// <summary>
		/// This delegate is used to indicate the signature of callbacks
		/// hooked in when a RTP packet arrives.
		/// </summary>
		/// <param name="sender">(this)</param>
		/// <param name="eventArgs">Contains the port and data representing the RTP packet</param>
		public delegate void RTPArrivedDelegate( object sender, RTPPacketEventArgs eventArgs );

		/// <summary>
		/// Event raised when a SIP packet has arrived.
		/// </summary>
		public event SipArrivedDelegate SipArrived;

		/// <summary>
		/// Event raised when a RTP packet has arrived on a UDP port specified by the SIP stream
		/// </summary>
		public event RTPArrivedDelegate RTPArrived;

		private bool started;

		#endregion

		#region Properties
		public string IPAddress
		{
			get
			{
				return this.ipAddress;
			}
			set
			{
				this.ipAddress = value;
			}
		}
		#endregion

		#region Constructor/Initilaization
		public static SniffSIP Instance
		{
			get
			{
				if( instance == null )
				{
					instance = new SniffSIP();
				}
				return instance;
			}
		}
		/// <summary>
		/// The sniffer needs to know the IP of the interface to sniff on.
		/// Since machines may have multiples, it is nice to specify:)
		/// </summary>
		/// <param name="ipAddress">just the string of theIP.</param>
		private SniffSIP()
		{
			packetCount = 0;
			//this.ipAddress = ipAddress;
			//Initialize();
		}

		public void Initialize()
		{
			pcapDevice = null;
			started = false;

			/* Retrieve the device list */
			PcapDeviceList devices = SharpPcap.SharpPcap.GetAllDevices();

			if( devices.Count < 1 )
			{
				return;
			}

			/* Scan the list printing every entry */
			foreach( PcapDevice dev in devices )
			{
				if( dev.PcapIpAddress.Equals( ipAddress ) )
				{
					pcapDevice = dev;
					break;
				}
			}
		}
		#endregion

		#region Public Interface
		public void StartSniffing()
		{
			if( started )
			{
				throw new Exception( "Already Sniffing" );
			}

			if( pcapDevice != null )
			{
				Thread t;
				t = new Thread( new ThreadStart( SniffThreadFun ) );
				t.Start();
				started = true;
			}
		}

		/// <summary>
		/// The purpose of this method is to stop the sniffer.
		/// </summary>
		public void StopSniffing()
		{
			if( !started )
			{
				throw new Exception( "Not Sniffing" );
			}

			if( pcapDevice != null )
			{
				pcapDevice.PcapClose();
				pcapDevice = null;
				started = false;
			}
		}
		#endregion

		#region Sniffing Thread
		/// <summary>
		/// The purpose of this method is to provide a thread to perform sniffing in.
		/// It must be threaded because the capture packet calls block, and we need
		/// to perform these actions while allowing the user to work with the system.
		/// </summary>
		private void SniffThreadFun()
		{
			//Register our handler function to the 'packet arrival' event
			pcapDevice.PcapOnPacketArrival +=
				new SharpPcap.SharpPcap.PacketArrivalEvent( device_PcapOnPacketArrival );

			//Open the device for capturing
			//true -- means promiscuous mode
			//1000 -- means a read wait of 1000ms
			pcapDevice.PcapOpen( true, 100 );

			//Associate the filter with this capture
			pcapDevice.PcapSetFilter( "ip and udp" );

			//Start capture 'INFINTE' number of packets
			//TODO: need to figure out if this returns after the close method
			// is called.
			pcapDevice.PcapCapture( SharpPcap.SharpPcap.INFINITE );
		}
		#endregion

		#region Packet Arrival Processing
		/// <summary>
		/// The purpose of this method is to raise the SipArrived event to all
		/// subscribers.
		/// </summary>
		/// <param name="parser">Parser containing the SIP information</param>
		private void RaiseSipArrived( SipParser parser )
		{
			if( this.SipArrived != null )
			{
				// TODO: perform safe invokation of all delegates.
				SipArrived( this, parser );
			}
		}

		private void RaiseRTPArrived( RTPPacketEventArgs eventArgs )
		{
			if( this.RTPArrived != null )
			{
				RTPArrived( this, eventArgs );
			}
		}

		/// <summary>
		/// Event handler called when a Pcap Packet has arrived
		/// </summary>
		/// <param name="sender">who raised it</param>
		/// <param name="packet">the packet</param>
		private void device_PcapOnPacketArrival( object sender, Packet packet )
		{
			string s;
			try
			{
				// not certain if this will be needed, but just for keep sake
				if( packet is TCPPacket )
				{
					/*
					DateTime time = packet.PcapHeader.Date;
					int len = packet.PcapHeader.PacketLength;

					TCPPacket tcp = (TCPPacket)packet;
					string srcIp = tcp.SourceAddress;
					string dstIp = tcp.DestinationAddress;
					int srcPort = tcp.SourcePort;
					int dstPort = tcp.estinationPort;

					Console.WriteLine( "{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
						time.Hour, time.Minute, time.Second, time.Millisecond, len,
						srcIp, srcPort, dstIp, dstPort );
					 */
				}
				else if( packet is UDPPacket )
				{
					IPPacket p = packet as IPPacket;
					//if( p.DestinationAddress.Equals( ipAddress ) )
					{
						DateTime time = packet.PcapHeader.Date;
						int len = packet.PcapHeader.PacketLength;

						UDPPacket udpPacket = (UDPPacket)packet;

						SipParser parser;

						if( packet.Data.Length > 0 )
						{
							// TODO: Is there a better way here?
							s = System.Text.ASCIIEncoding.ASCII.GetString( packet.Data );
							if( s.Length > 0 )
							{
								if( s.Contains( "SIP" ) )
								{
									//DumpPacketToFiles( s );
									//outs = String.Format( "{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
									//	time.Hour, time.Minute, time.Second, time.Millisecond, len,
									//	srcIp, srcPort, dstIp, dstPort );
									//this.sw.WriteLine( outs );
									//this.sw.Write( s );
									//this.sw.Flush();

									parser = new SipParser( s, ipAddress, udpPacket.DestinationAddress );
									//Console.WriteLine(
									//	String.Format( "{0} {1} {2}",
									//	parser.SipStatus, parser.SipStatusString, parser.ToField ) );
									RaiseSipArrived( parser );
									packetCount++;
								}
								else
								{
									RTPPacketEventArgs eventArgs;
									eventArgs = new RTPPacketEventArgs( udpPacket.DestinationPort, udpPacket.Data );
									RaiseRTPArrived( eventArgs );
								}
							}
						}
					}
				}
			}
			catch( Exception ex )
			{
				ClsException.WriteToErrorLogFile( ex );
			}
		}
		#endregion

		/// <summary>
		/// The purpose of this method is to create a file and put the string representing
		/// the packet into the file.  It may need to be enhanced to provide knowledge about
		/// source and destination.
		/// </summary>
		/// <param name="s"></param>
		void DumpPacketToFiles( string s )
		{
			FileStream fs;
			StreamWriter sw;
			string fileName;

			fileName = @"c:\siplogs\" + packetCount.ToString() + ".dat";
			fs = File.Open( fileName, FileMode.Create );
			sw = new StreamWriter( fs );
			sw.Write( s );
			sw.Flush();
			sw.Close();
		}
	}
}
