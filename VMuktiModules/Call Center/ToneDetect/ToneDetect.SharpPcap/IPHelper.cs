using System;
using System.Runtime.InteropServices;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Summary description for IPHelper.
	/// </summary>
	public class IPHelper
	{
		private const	int		MAX_ADAPTER_NAME				= 128;
		private const	int		MAX_ADAPTER_NAME_LENGTH			= 256;
		private const	int		MAX_ADAPTER_DESCRIPTION_LENGTH	= 128;
		private const	int		MAX_ADAPTER_ADDRESS_LENGTH		= 8;
		private const	int		ERROR_OK						= 0;
		private static	uint	ERROR_BUFFER_OVERFLOW			= (uint)111;

		#region Structs
			
		/// <summary>
		/// Represent a row in an INTF_TABLE (Interfaces Table)
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)]
			internal struct INTF_ROW
		{
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=256)]
			internal String		Name;
			internal uint		Index;
			internal uint		Type;
			internal uint		Mtu;
			internal uint		Speed;
			internal uint		PhysAddrLen;//MAC Address length

			[MarshalAs(UnmanagedType.ByValArray,SizeConst=MAX_ADAPTER_ADDRESS_LENGTH)]
			internal byte[]     PhysAddr;//The MAC address
			internal uint		AdminStatus;
			internal uint		OperStatus;
			internal uint		LastChange;
			internal uint		InOctets;
			internal uint		dwInUcastPkts ;
			internal uint		dwInNUcastPkts ;
			internal uint		dwInDiscards;
			internal uint		dwInErrors ;
			internal uint		dwInUnknownProtos ;
			internal uint		dwOutOctets;
			internal uint		dwOutUcastPkts;
			internal uint		dwOutNUcastPkts ;
			internal uint		dwOutDiscards;
			internal uint		dwOutErrors ;
			internal uint		dwOutQLen;
			internal uint		dwDescrLen ;

			[MarshalAs(UnmanagedType.ByValArray,SizeConst=256)]
			internal byte[]     bDescr;//The description of this interface
		}

		[ComVisible(false),StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)]
			internal struct IP_ADAPTER_INDEX_MAP
		{
			internal uint		Index;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
			internal String		Name;
		}


		[ComVisible(false),StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi)]
			internal struct IP_ADAPTER_INFO 
		{
			internal int /* struct _IP_ADAPTER_INFO* */ Next;
			internal uint           ComboIndex;

			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=MAX_ADAPTER_NAME_LENGTH + 4)]
			internal String         AdapterName;

			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
			internal String         Description;
			internal int           AddressLength;

			[MarshalAs(UnmanagedType.ByValArray,SizeConst=MAX_ADAPTER_ADDRESS_LENGTH)]
			internal byte[]         Address;
			internal int           Index;
			internal int           Type;
			internal int           DhcpEnabled;
			public uint           CurrentIpAddress; /* IP_ADDR_STRING* */
			internal IP_ADDR_STRING IpAddressList;
			internal IP_ADDR_STRING GatewayList;
			internal IP_ADDR_STRING DhcpServer;
			[MarshalAs(UnmanagedType.Bool)]
			internal bool           HaveWins;
			internal IP_ADDR_STRING PrimaryWinsServer;
			internal IP_ADDR_STRING SecondaryWinsServer;
			internal uint/*time_t*/ LeaseObtained;
			internal uint/*time_t*/ LeaseExpires;
		};

		/// <summary>
		/// MAC_ADDR_ARR struct for "iphlpapi.dll" invocation.
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential)]
			internal struct MAC_ADDR_ARRAY 
		{
			[MarshalAs(UnmanagedType.ByValArray,SizeConst=6)]
			public byte[] bytes;
		};

		/// <summary>
		/// Represent a row in an IP address
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential)]
			internal struct IPADDR
		{
			internal byte s_b1;
			internal byte s_b2;
			internal byte s_b3;
			internal byte s_b4;
		};

		/// <summary>
		/// Represent a row in an IP mask
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential)]
			internal struct IPMASK
		{
			internal byte s_b1;
			internal byte s_b2;
			internal byte s_b3;
			internal byte s_b4;
		};

		[ComVisible(false),StructLayout(LayoutKind.Sequential)]
			internal struct  IPFORWARDROW 
		{
			internal int /*DWORD*/ dwForwardDest;  
			internal int /*DWORD*/ dwForwardMask;  
			internal int /*DWORD*/ dwForwardPolicy;  
			internal int /*DWORD*/ dwForwardNextHop;  
			internal int /*DWORD*/ dwForwardIfIndex;  
			internal int /*DWORD*/ dwForwardType;  
			internal int /*DWORD*/ dwForwardProto;  
			internal int /*DWORD*/ dwForwardAge;  
			internal int /*DWORD*/ dwForwardNextHopAS;  
			internal int /*DWORD*/ dwForwardMetric1;  
			internal int /*DWORD*/ dwForwardMetric2;  
			internal int /*DWORD*/ dwForwardMetric3;  
			internal int /*DWORD*/ dwForwardMetric4;  
			internal int /*DWORD*/ dwForwardMetric5;
		};

		[ComVisible(false),StructLayout(LayoutKind.Sequential)]
			internal struct IPFORWARDTABLE 
		{
			uint /*DWORD*/						dwNumEntries;  
			uint /*struct MIB_IPFORWARDROW*/	table;
		};

		/// <summary>
		/// IP_ADDRESS_STRING struct for "iphlpapi.dll" invocation.
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi)]
			internal struct IP_ADDRESS_STRING 
		{
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=16)]
			public string address;
		};

		/// <summary>
		/// IP_MASK_STRING struct for "iphlpapi.dll" invocation.
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi)]
			internal struct IP_MASK_STRING 
		{
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=16)]
			public string address;
		};

		/// <summary>
		/// IP_ADDR_STRING struct for "iphlpapi.dll" invocation.
		/// </summary>
		[ComVisible(false),StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi)]
			internal struct IP_ADDR_STRING 
		{
			public int Next;      /* struct _IP_ADDR_STRING* */
			public IP_ADDRESS_STRING IpAddress;
			public IP_MASK_STRING IpMask;
			public uint Context;
		};


		#endregion Structs

		#region iphlpapi.dll external function

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static uint GetAdaptersInfo(IntPtr pAdapterInfo,ref int pOutBufLen);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int GetInterfaceInfo(IntPtr pIfTable,ref	int pOutBufLen);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static uint IpReleaseAddress(IntPtr AdapterInfo);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static uint IpRenewAddress(IntPtr AdapterInfo);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int GetIfTable(IntPtr pIfTable,ref int pdwSize,bool bOrder);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int SendARP(int DestIP,int SrcIP,IntPtr pMacAddr,ref int PhyAddrLen);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int AddIPAddress(IPADDR Address, IPMASK IpMask,	uint IfIndex, out ulong NTEContext, out ulong NTEInstance);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int DeleteIPAddress (ulong NTEContext);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int SetIfEntry(IntPtr /*PMIB_IFROW*/ pIfRow);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int GetIpForwardTable(IntPtr /*PMIB_IPFORWARDTABLE*/ pIpForwardTable,ref int /*PULONG*/ pdwSize,bool bOrder);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int DeleteIpForwardEntry(IntPtr /*PMIB_IPFORWARDROW*/ pRoute);

		[DllImport("iphlpapi", CharSet=CharSet.Auto)]
		private extern static int CreateIpForwardEntry(IntPtr /*PMIB_IPFORWARDROW*/ pRoute);

		#endregion iphlpapi.dll external function

		/// <summary>
		/// Returns an IP_ADAPTER_INFO struct that represents a network adapter on this machine
		/// </summary>
		internal static IP_ADAPTER_INFO GetAdapterInfo(string adapterName) 
		{
			int size = Marshal.SizeOf(typeof(IP_ADAPTER_INFO));
			IntPtr buffer = Marshal.AllocHGlobal(size);
			uint result = GetAdaptersInfo(buffer,ref size);

			if (result == ERROR_BUFFER_OVERFLOW) 
			{				
				Marshal.FreeHGlobal(buffer);
				buffer = Marshal.AllocHGlobal(size);
				result = GetAdaptersInfo(buffer,ref size);
			}

			if (result == ERROR_OK) 
			{				
				int next=(int)buffer;
				IP_ADAPTER_INFO info;
				while (next != 0) 
				{
					info = (IP_ADAPTER_INFO)Marshal.PtrToStructure((IntPtr)next,typeof(IP_ADAPTER_INFO));
					next=info.Next;
					if(info.AdapterName==adapterName)
					{
						Marshal.FreeHGlobal(buffer);
						return info;
					}
				}
				Marshal.FreeHGlobal(buffer);
				throw new IPHelper_DeviceDoesntExistsException("GetAdaptersInfo failed: adapter doesn't exists: " +
					adapterName);
			} 
			else 
			{
				Marshal.FreeHGlobal(buffer);
				throw new InvalidOperationException("GetAdaptersInfo failed: " +
					result);
			}
		}

		/// <summary>
		/// Return all network devices available on this machine through the IPHelper API
		/// </summary>
		public static NetworkDeviceList GetAllDevices()
		{
			int size = Marshal.SizeOf(typeof(IP_ADAPTER_INFO));
			IntPtr buffer = Marshal.AllocHGlobal(size);
			uint result = GetAdaptersInfo(buffer,ref size);

			NetworkDeviceList deviceList = new NetworkDeviceList();

			if (result == ERROR_BUFFER_OVERFLOW) 
			{				
				Marshal.FreeHGlobal(buffer);
				buffer = Marshal.AllocHGlobal(size);
				result = GetAdaptersInfo(buffer,ref size);
			}

			if (result == ERROR_OK) 
			{				
				int next=(int)buffer;
				IP_ADAPTER_INFO info;
				while (next != 0) 
				{
					info = (IP_ADAPTER_INFO)Marshal.PtrToStructure((IntPtr)next,typeof(IP_ADAPTER_INFO));
					next=info.Next;
					deviceList.Add( new NetworkDevice(info) );
				}
				return deviceList;
			} 
			else 
			{
				Marshal.FreeHGlobal(buffer);
				throw new InvalidOperationException("GetAdaptersInfo failed: " +
					result);
			}
		}
	}

	public class IPHelper_DeviceDoesntExistsException : Exception 
	{
		public IPHelper_DeviceDoesntExistsException(string msg):base(msg)
		{
		}
	}
}
