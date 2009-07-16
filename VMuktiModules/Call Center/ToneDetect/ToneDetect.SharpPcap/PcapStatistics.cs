using System;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Holds network statistics for a Pcap Devices
	/// </summary>
	public class PcapStatistics
	{
		/// <summary>
		/// This holds time value
		/// </summary>
		private	SharpPcap.PCAP_PKTHDR	m_pktHdr;
		/// <summary>
		/// This holds byte received and packets received
		/// </summary>
		private byte[]	m_pktData;
		/// <summary>
		/// Constructs a new Pcap Statistics strcuture
		/// </summary>
		/// <param name="pktHdr">Time value as PCAP_PKTHDR</param>
		/// <param name="pktData">Statistics values as PCAP_PKTDATA</param>
		internal PcapStatistics(SharpPcap.PCAP_PKTHDR pktHdr, SharpPcap.PCAP_PKTDATA pktData)
		{
			this.m_pktHdr	= pktHdr;
			this.m_pktData	= pktData.bytes;
		}

		internal PcapStatistics(Packets.Packet p)
		{
			this.m_pktHdr	= p.PcapHeader.m_pcap_pkthdr;
			this.m_pktData	= p.Bytes;
		}

		/// <summary>
		/// Number of packets received since last sample
		/// </summary>
		public Int64 RecievedPackets
		{
			get
			{
				return BitConverter.ToInt64(m_pktData, 0);
			}
		}

		/// <summary>
		/// Number of bytes received since last sample
		/// </summary>
		public Int64 RecievedBytes
		{
			get
			{
				return BitConverter.ToInt64(m_pktData, 8);
			}
		}

		/// <summary>
		/// The 'Seconds' part of the timestamp
		/// </summary>
		public int Seconds
		{
			get
			{
				return m_pktHdr.tv_sec;
			}			
		}
		/// <summary>
		/// The 'MicroSeconds' part of the timestamp
		/// </summary>
		public int MicroSeconds
		{
			get
			{
				return m_pktHdr.tv_usec;
			}			
		}
		/// <summary>
		/// The timestamps
		/// </summary>
		public System.DateTime Date
		{
			get
			{
				DateTime timeval = new DateTime(1970,1,1); 
				timeval = timeval.AddSeconds(Seconds); 
				timeval = timeval.AddMilliseconds(MicroSeconds / 1000); 
				return timeval.ToLocalTime();
			}			
		}
	}
}
