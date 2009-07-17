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
using System;
using System.Runtime.InteropServices;
using ToneDetect.SharpPcap.Packets;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Summary description for PcapSendQueue.
	/// </summary>
	public class PcapSendQueue
	{
		IntPtr m_queue = IntPtr.Zero;

		/// <summary>
		/// Creates and allocates a new PcapSendQueue and 
		/// </summary>
		/// <param name="memSize">
		/// The maximun amount of memory (in bytes) 
		/// to allocate for the queue</param>
		public PcapSendQueue(int memSize)
		{
			m_queue = SharpPcap.pcap_sendqueue_alloc( memSize );
			if(m_queue==IntPtr.Zero)
				throw new PcapException("Error creating PcapSendQueue");
		}

		/// <summary>
		/// Add a packet to this send queue. 
		/// </summary>
		/// <param name="packet">The packet bytes to add</param>
		/// <param name="pcapHdr">The pcap header of the packet</param>
		/// <returns>True if success, else false</returns>
		public bool Add( byte[] packet, SharpPcap.PCAP_PKTHDR pcapHdr )
		{
			if(m_queue==IntPtr.Zero)
			{
				throw new PcapException("Can't add packet, this queue is disposed");
			}

			if(pcapHdr.caplen==0)
				pcapHdr.caplen = packet.Length;//set the length in the header field

			//Marshal packet
			IntPtr pktPtr;
			pktPtr = Marshal.AllocHGlobal(packet.Length);
			Marshal.Copy(packet, 0, pktPtr, packet.Length);

			//Marshal header
			IntPtr hdrPtr;
			hdrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SharpPcap.PCAP_PKTHDR)));
			Marshal.StructureToPtr(pcapHdr, hdrPtr, true);

			int res = SharpPcap.pcap_sendqueue_queue( m_queue, hdrPtr, pktPtr);

			Marshal.FreeHGlobal(pktPtr);
			Marshal.FreeHGlobal(hdrPtr);	
	
			return (res!=-1);
		}
		/// <summary>
		/// Add a packet to this send queue. 
		/// </summary>
		/// <param name="packet">The packet bytes to add</param>
		/// <param name="pcapHdr">The pcap header of the packet</param>
		/// <returns>True if success, else false</returns>
		public bool Add( byte[] packet, PcapHeader pcapHdr )
		{
			return this.Add( packet, pcapHdr.m_pcap_pkthdr);
		}
		/// <summary>
		/// Add a packet to this send queue. 
		/// </summary>
		/// <param name="packet">The packet bytes to add</param>
		/// <returns>True if success, else false</returns>
		public bool Add( byte[] packet )
		{
			SharpPcap.PCAP_PKTHDR hdr = new SharpPcap.PCAP_PKTHDR();
			return this.Add( packet, hdr );
		}
		/// <summary>
		/// Add a packet to this send queue. 
		/// </summary>
		/// <param name="packet">The packet to add</param>
		/// <returns>True if success, else false</returns>
		public bool Add( Packet packet )
		{
			return this.Add( packet.Bytes, packet.PcapHeader.m_pcap_pkthdr );
		}
		/// <summary>
		/// Add a packet to this send queue.
		/// </summary>
		/// <param name="packet">The packet to add</param>
		/// <param name="seconds">The 'seconds' part of the packet's timestamp</param>
		/// <param name="miliseconds">The 'microseconds' part of the packet's timestamp</param>
		/// <returns>True if success, else false</returns>
		public bool Add( byte[] packet, int seconds, int microseconds )
		{
			SharpPcap.PCAP_PKTHDR hdr = new SharpPcap.PCAP_PKTHDR();
			hdr.tv_sec=seconds;
			hdr.tv_usec=microseconds;
			return this.Add( packet, hdr );
		}

		/// <summary>
		/// Send a queue of raw packets to the network. 
		/// </summary>
		/// <param name="device">The PcapDevice on which to send the queue</param>
		/// <param name="synchronize">determines if the send operation must be synchronized: 
		/// if it is non-zero, the packets are sent respecting the timestamps, 
		/// otherwise they are sent as fast as possible
		/// <returns></returns>
		public int Transmit( PcapDevice device, bool synchronize)
		{
			if(!device.PcapOpened)
				throw new PcapException("Can't transmit queue, the pcap device is closed.");

			if(m_queue==IntPtr.Zero)
			{
				throw new PcapException("Can't transmit queue, this queue is disposed");
			}

			int sync = synchronize ? 1 : 0;			
			return SharpPcap.pcap_sendqueue_transmit(device.PcapHandle, m_queue, sync);
		}

		/// <summary>
		/// Destroy the send queue. 
		/// </summary>
		public void Dispose()
		{
			if(m_queue!=IntPtr.Zero)
			{
				SharpPcap.pcap_sendqueue_destroy( m_queue );
			}
		}

		/// <summary>
		/// The current length in bytes of this queue
		/// </summary>
		public int CurrentLength
		{
			get
			{
				if(m_queue==IntPtr.Zero)
				{
					throw new PcapException("Can't perform operation, this queue is disposed");
				}
				SharpPcap.pcap_send_queue q =
					(SharpPcap.pcap_send_queue)Marshal.PtrToStructure
					(m_queue, typeof(SharpPcap.pcap_send_queue));
				return (int)q.len;
			}
		}
	}
}
