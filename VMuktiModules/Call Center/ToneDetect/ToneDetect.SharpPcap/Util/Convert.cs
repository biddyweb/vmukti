using System;
using System.Text;

namespace ToneDetect.SharpPcap.Util
{
	/// <summary>
	/// Summary description for Util.
	/// </summary>
	public class Convert
	{
		public static byte[] GetBytes(string str)
		{
			return Encoding.Default.GetBytes(str);
		}

		public static string GetString(byte[] bytes)
		{
			return Encoding.Default.GetString(bytes, 0, bytes.Length);
		}

		//From: http://www.ip2location.com/README-IP-COUNTRY.htm
		public static uint IpStringToInt32(string DottedIP)
		{
			int i;
			string [] arrDec;
			double num = 0;
			if (DottedIP == "")
			{
				return 0;
			}
			else
			{
				arrDec = DottedIP.Split('.');
				for(i = arrDec.Length - 1; i >= 0 ; i --)
				{
					num += ((int.Parse(arrDec[i])%256) * Math.Pow(256 ,(3 - i )));
				}
				return (uint)num;
			}
		}

		public static string IpInt32ToString(uint ip)
		{
			uint w =  ( ip / 16777216 ) % 256;
			uint x =  ( ip / 65536    ) % 256;
			uint y =  ( ip / 256      ) % 256;
			uint z =  ( ip            ) % 256;
			return (w+"."+x+"."+y+"."+z);
		}

		public static string IpInt32ToString(int ip)
		{
			return IpInt32ToString((uint)ip);
		}

		/// <summary>
		/// Converts a network mask string represntation into an integer representing the number of network bits
		/// </summary>
		public static Int32 MaskStringToBits( string mask )
		{
			uint m = IpStringToInt32( mask );
			int zeros = 0;
			uint mod = (m%2);
			while(mod==0)
			{
				m=m/2;
				mod=m%2;
				zeros++;
			}
			return 32-zeros;
		}

		public static string BytesToHex(byte[] bytes, int start, int len)
		{
			string hex = "";
			string byte_hex;

			for(int i=start; i<len; i++)
			{
				byte_hex = bytes[i].ToString("X");
				if (byte_hex.Length==1) 
					byte_hex="0"+byte_hex;
				hex += byte_hex;
			}
			return hex;
		}

		public static string BytesToHex(byte[] bytes)
		{
			if(bytes !=null)
				return BytesToHex(bytes, 0, bytes.Length);
			else
				return "";
		}

		/// <summary>
		/// Converts a time_t to DateTime
		/// </summary>
		public static DateTime Time_T2DateTime(uint time_t) 
		{
			long win32FileTime = 10000000*(long)time_t + 116444736000000000;
			return DateTime.FromFileTimeUtc(win32FileTime).ToLocalTime();
		}

	}
}
