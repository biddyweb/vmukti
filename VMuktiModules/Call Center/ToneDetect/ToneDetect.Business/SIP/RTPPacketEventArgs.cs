using System;
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.SIP
{
	public class RTPPacketEventArgs : EventArgs
	{
		#region Fields
		private int port;
		private byte [] packet;
		#endregion

		#region Constructor
		internal RTPPacketEventArgs( int inPort, byte [] data )
		{
			port = inPort;
			packet = data;
		}
		#endregion

		#region Properties
		public int Port
		{
			get
			{
				return port;
			}
		}

		public byte [] Packet
		{
			get
			{
				return packet;
			}
		}
		#endregion
	}
}
