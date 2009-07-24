<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.SharpPcap/NetworkDevice.cs
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
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.SharpPcap/NetworkDevice.cs
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ToneDetect.SharpPcap.Util;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Represents a physical Network Device on a Windows machine.
	/// </summary>
	public class NetworkDevice : PcapDevice
	{
		private IPAddressList m_ipAddressList;
		private StringList m_gatewaysList;
		private IPHelper.IP_ADAPTER_INFO m_adapterInfo;


		/// <summary>
		/// Constructs a new Network Device
		/// </summary>
		/// <param name="name">The name of the Network Device. The name can be
		/// either in 'pcap' format or 'iphelper' format.</param>
		public NetworkDevice(string name) : base(name)
		{
			Setup();
		}

		/// <summary>
		/// Constructs a new Network Device based on a PCAP_IF struct.
		/// </summary>
		internal NetworkDevice(SharpPcap.PCAP_IF pcapIf):base(pcapIf)
		{
			Setup();
		}

		/// <summary>
		/// Constructs a new Network Device based on a IP_ADAPTER_INFO struct.
		/// </summary>
		internal NetworkDevice(IPHelper.IP_ADAPTER_INFO adapterInfo):base(adapterInfo.AdapterName)
		{
			SetAfapterInfo(adapterInfo);
		}

		/// <summary>
		/// The name of this Network Device
		/// </summary>
		public string Name
		{
			get{return m_adapterInfo.AdapterName;}
		}

		/// <summary>
		/// The Description of this Network Device
		/// </summary>
		public string Description
		{
			get{return m_adapterInfo.Description;}
		}

		/// <summary>
		/// Gets the device index of this network device
		/// </summary>
		public int Index
		{
			get{return m_adapterInfo.Index;}
		}

		/// <summary>
		/// Gets a hex string representing the MAC (physical) Address of this 
		/// Network Device.
		/// </summary>
		public string MacAddress
		{
			get{return Util.Convert.BytesToHex( m_adapterInfo.Address, 0, m_adapterInfo.AddressLength );}
		}

		/// <summary>
		/// Gets the MAC Address of this network device as a byte array.
		/// </summary>
		public byte[] MacAddressBytes
		{
			get
			{
				byte[] mac = new byte[m_adapterInfo.AddressLength];
				Array.Copy(m_adapterInfo.Address, 0, mac, 0, mac.Length);
				return mac;
			}
		}

		/// <summary>
		/// Gets the main IP address of this network device
		/// </summary>
		public string IpAddress
		{
			get
			{
				if(IpAddressList==null||IpAddressList.Count==0)
					return null;
                return IpAddressList[0].Address;
			}
		}

		/// <summary>
		/// Gets the subnet mask of the main IP address of this network device
		/// </summary>
		public string SubnetMask
		{
			get
			{
				if(IpAddressList==null||IpAddressList.Count==0)
					return null;
				return IpAddressList[0].Mask;
			}
		}

		/// <summary>
		/// Gets a list of all IP addresses and subnet masks of this network device
		/// </summary>
		public IPAddressList IpAddressList
		{
			get{return m_ipAddressList;}
		}



		/// <summary>
		/// Gets the primary default gateway of this network device
		/// </summary>
		public string DefaultGateway
		{
			get
			{
				if(DefaultGatewayList==null||DefaultGatewayList.Count==0)
					return null;
				return m_gatewaysList[0];
			}
		}

		/// <summary>
		/// Gets a list of all default gateways on this network device
		/// </summary>
		public StringList DefaultGatewayList
		{
			get{return m_gatewaysList;}
		}

		/// <summary>
		/// Gets a status indicating whether DHCP is enabled on this network device
		/// </summary>
		public bool DhcpEnabled
		{
			get{return m_adapterInfo.DhcpEnabled==1;}
		}

		/// <summary>
		/// Gets the IP Address of the DHCP Server of this network device
		/// </summary>
		public string DhcpServer
		{
			get{return m_adapterInfo.DhcpServer.IpAddress.address;}
		}

		/// <summary>
		/// Gets the date/time of the DHCP lease
		/// </summary>
		public DateTime DhcpLeaseObtained
		{
			get{return Util.Convert.Time_T2DateTime(m_adapterInfo.LeaseObtained);}
		}

		/// <summary>
		/// Gets the date/time of the DHCP expiration
		/// </summary>
		public DateTime DhcpLeaseExpires
		{
			get{return Util.Convert.Time_T2DateTime(m_adapterInfo.LeaseExpires);}
		}

		/// <summary>
		/// Gets the primary WINS Server configured for this network device
		/// </summary>
		public string WinsServerPrimary
		{
			get{return m_adapterInfo.PrimaryWinsServer.IpAddress.address;}
		}

		/// <summary>
		/// Gets the secondary WINS Server configured for this network device
		/// </summary>
		public string WinsServerSecondary
		{
			get{return m_adapterInfo.SecondaryWinsServer.IpAddress.address;}
		}

		/// <summary>
		/// Returns a string representaion of this Network Device
		/// </summary>
		public override string ToString()
		{
			return (this.Name+" ["+this.Description+"]");
		}

		/// <summary>
		/// Returns a detailed string representaion of this Network Device
		/// </summary>
		public string ToStringDetailed()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(this.ToString());sb.Append("\n");
			sb.Append("-------------------------------");sb.Append("\n");
			sb.Append("Pcap Name: "+this.PcapName);sb.Append("\n");
			sb.Append("Pcap Description: "+this.PcapDescription);sb.Append("\n");
			sb.Append("IP Address: "+this.IpAddress);sb.Append("\n");
			sb.Append("Subnet Mask: "+this.SubnetMask);sb.Append("\n");
			sb.Append("Default Gateway: "+this.DefaultGateway);sb.Append("\n");
			sb.Append("MAC Address: "+this.MacAddress);sb.Append("\n");
			sb.Append("DHCP Enabled: "+this.DhcpEnabled);sb.Append("\n");
			sb.Append("DHCP Server: "+this.DhcpServer);sb.Append("\n");
			sb.Append("Primary WINS Server: "+this.WinsServerPrimary);sb.Append("\n");
			sb.Append("Secondary WINS Server: "+this.WinsServerSecondary);sb.Append("\n");
			sb.Append("Lease Obtained: "+this.DhcpLeaseObtained);sb.Append("\n");
			sb.Append("Lease Expires: "+this.DhcpLeaseExpires);
			return sb.ToString();
		}

		/***********************************/
		/***       Private Methods	       */
		/***********************************/


		private void Setup()
		{
			try
			{
				SetAfapterInfo( IPHelper.GetAdapterInfo( FromPcapName(PcapName) ) );
			}
			catch(IPHelper_DeviceDoesntExistsException ddee)
			{
				if(PcapName!=null)
				{
					m_adapterInfo.AdapterName=PcapName;
					m_adapterInfo.Description ="This is a pcap emulated device. Only pcap operations allowed. Inappropriate properties will hold a 'null' value.";
				}
				else
					throw ddee;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		private void SetAfapterInfo(IPHelper.IP_ADAPTER_INFO adapterInfo)
		{
			m_adapterInfo=adapterInfo;
			m_ipAddressList = GetIpAddressList( adapterInfo.IpAddressList );
			m_gatewaysList = GetIpGateways( adapterInfo.GatewayList );
		}

		private IPAddressList GetIpAddressList(IPHelper.IP_ADDR_STRING addr)
		{
			IPAddressList ipList = new IPAddressList();
			ipList.Add( new IPAddress(addr.IpAddress.address,addr.IpMask.address) );
			while(addr.Next != 0)
			{
				addr = (IPHelper.IP_ADDR_STRING)Marshal.PtrToStructure((IntPtr)addr.Next,typeof(IPHelper.IP_ADDR_STRING));
				ipList.Add(new IPAddress(addr.IpAddress.address,addr.IpMask.address));
			}
			return ipList;
		}

		private StringList GetIpGateways(IPHelper.IP_ADDR_STRING addr)
		{
			
			StringList result = new StringList();
			result.Add(addr.IpAddress.address);
			while(addr.Next != 0)
			{
				addr = (IPHelper.IP_ADDR_STRING)Marshal.PtrToStructure((IntPtr)addr.Next,typeof(IPHelper.IP_ADDR_STRING));
				result.Add( addr.IpAddress.address );
			}
			return result;
		}

		private string FromPcapName( string pcapName )
		{
			System.Text.RegularExpressions.Regex regex =
				new System.Text.RegularExpressions.Regex("{.*}");
			System.Text.RegularExpressions.Match match = 
				regex.Match( pcapName );

			if(match.Success)
			{
				return match.Value;
			}
			return pcapName;
		}
		
		internal static bool IsNetworkDevice( string deviceName )
		{
			System.Text.RegularExpressions.Regex regex =
				new System.Text.RegularExpressions.Regex("{.*}");
			System.Text.RegularExpressions.Match match = 
				regex.Match( deviceName );
			return match.Success;
		}
	}
}
