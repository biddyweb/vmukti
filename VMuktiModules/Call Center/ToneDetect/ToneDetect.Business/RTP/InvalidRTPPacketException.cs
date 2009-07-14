using System;
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.RTP
{
	/// <summary>
	/// The purpose of this class it to provide an exception to
	/// throw when the software detectes an invalid RTP packet.
	/// </summary>
	public class InvalidRTPPacketException : System.Exception
	{
	}
}
