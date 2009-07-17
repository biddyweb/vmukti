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

namespace ToneDetect.SharpPcap
{
	/// <summary>
	///  A wrapper class for libpcap's PCAP_PKTHDR structure
	/// </summary>
	public class PcapHeader
	{
		/// <summary>
		/// The underlying PCAP_PKTHDR structure
		/// </summary>
		internal SharpPcap.PCAP_PKTHDR m_pcap_pkthdr;
		/// <summary>
		/// Constructs a new PcapHeader
		/// </summary>
		public PcapHeader()
		{
			m_pcap_pkthdr=new SharpPcap.PCAP_PKTHDR();
		}
		/// <summary>
		/// Constructs a new PcapHeader
		/// </summary>
		/// <param name="m_pcap_pkthdr">The underlying PCAP_PKTHDR structure</param>
		public PcapHeader( SharpPcap.PCAP_PKTHDR m_pcap_pkthdr )
		{
			this.m_pcap_pkthdr=m_pcap_pkthdr;
		}
		/// <summary>
		/// Constructs a new PcapHeader
		/// </summary>
		/// <param name="seconds">The seconds value of the packet's timestamp</param>
		/// <param name="microseconds">The microseconds value of the packet's timestamp</param>
		/// <param name="packetLength">The actual length of the packet</param>
		/// <param name="captureLength">The length of the capture</param>
		public PcapHeader( int seconds, int microseconds, int packetLength, int captureLength )
		{
			this.m_pcap_pkthdr.tv_sec		=	seconds;
			this.m_pcap_pkthdr.tv_usec	=	microseconds;
			this.m_pcap_pkthdr.len		=	packetLength;
			this.m_pcap_pkthdr.caplen		=	captureLength;
		}
		/// <summary>
		/// The seconds value of the packet's timestamp
		/// </summary>
		public int Seconds
		{
			get{return m_pcap_pkthdr.tv_sec;}
			set{m_pcap_pkthdr.tv_sec=value;}
		}
		/// <summary>
		/// The microseconds value of the packet's timestamp
		/// </summary>
		public int MicroSeconds
		{
			get{return m_pcap_pkthdr.tv_usec;}
			set{m_pcap_pkthdr.tv_usec=value;}
		}

		/// <summary>
		/// The actual length of the packet
		/// </summary>
		public int PacketLength
		{
			get{return m_pcap_pkthdr.len;}
			set{m_pcap_pkthdr.len=value;}
		}
		/// <summary>
		/// The length of the capture
		/// </summary>
		public int CaptureLength
		{
			get{return m_pcap_pkthdr.caplen;}
			set{m_pcap_pkthdr.caplen=value;}
		}
		
		/// <summary>
		/// Return the DateTime value of this pcap header
		/// </summary>
		virtual public System.DateTime Date
		{
			get
			{
				DateTime time = new DateTime(1970,1,1); 
				time = time.AddSeconds(Seconds); 
				time = time.AddMilliseconds(MicroSeconds / 1000); 
				return time.ToLocalTime();
			}			
		}
	}
}
