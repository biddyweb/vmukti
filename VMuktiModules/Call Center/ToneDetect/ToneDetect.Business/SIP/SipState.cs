using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using VMuktiAPI;

namespace ToneDetect.Business.SIP
{
	public class SipState
	{
		#region Fields
		/// <summary>
		/// this is the number of the rtp local rtp port.
		/// </summary>
		private int rtpPort;
		#endregion

		#region Properties
		/// <summary>
		/// provide the RTP port number.
		/// </summary>
		public int RTPPort
		{
			get
			{
				return rtpPort;
			}
		}
		#endregion

		#region Constructor
		public SipState()
		{
			rtpPort = 0;
		}
		#endregion

		#region Cleanup
		/// <summary>
		/// Close the log file and do any other cleanup needed.
		/// </summary>
		public void Close()
		{
		}
		#endregion

		#region Functions
		/// <summary>
		/// The purpose of this method is to determine the RTP port from the SDP packet.
		/// The so called Media Description from the stream is what is being parsed here.
		/// The string we are interested in will look like:
		/// m=audio <port> protocol  and some other items.
		/// When the port number here is specified, this is what port the RTP packets wil
		/// be arriving on.
		/// </summary>
		/// <param name="parser">Instance of the SipParser</param>
		/// <returns>a port number if known or 0 if unknown.</returns>
		private int ParsePortFromSDP( SipParser parser )
		{
			int locPort=0;

			if( parser.MediaDescriptions.Count > 0 )
			{
				foreach( string str in parser.MediaDescriptions )
				{
					// this bit is able to properly extract the UDP port number
					// the RTP packets will be coming down on.  So Once we get into
					// this state, everything cares about RTP at higher levels, but
					// when we get into sipstate, the prot will be known.  Need to
					// verify that the channel has its own instance of this class.                             
					if( str.StartsWith( "m=audio" ) )
					{
						string [] strings;
						strings = str.Split( ' ' );
						if( strings.Length > 1 )
						{
							locPort = int.Parse( strings [1] );
						}
					}
				}
			}

			return locPort;
		}

		/// <summary>
		/// the purpose of this method is to process SIP packets and ultimately determine the
		/// rtp port number the local host will be communicating to the server with.
		/// </summary>
		/// <param name="parser">instance of the sip parser</param>
		/// <returns></returns>
		public SipConnectionState Process( SipParser parser )
		{
			SipConnectionState state;

			state = SipConnectionState.GOOD;

			// if the port is 0 we do not yet know what destination port the
			// client will be listening for incoming RTP traffic.
			if( rtpPort == 0 )
			{
				if( parser.LocalSource )
				{
					rtpPort = ParsePortFromSDP( parser );
					if( rtpPort != 0 )
					{
						state = SipConnectionState.RTP_DETECTED;
					}
				}
			}
			else
			{
				// if the port is set, may need to process sip messages and do things
				// when they are of appropriate status based on the source/destination.
			}

			return state;
		}
		#endregion
	}
}
