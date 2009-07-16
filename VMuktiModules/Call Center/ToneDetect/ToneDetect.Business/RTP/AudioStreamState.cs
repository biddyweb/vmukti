using System;
using System.Text;
using System.IO;

using VMuktiAPI;
using ToneDetect.Business.Detect;

namespace ToneDetect.Business.RTP
{
	public class AudioStreamState
	{
		#region Fields
		private AudioStream audioStream;
		private bool doRecordStream;
		private Stream recordStream;
		private string phoneNumber;
		#endregion

		#region Constructor
		public AudioStreamState()
		{
			doRecordStream = false;
			phoneNumber = string.Empty;
			Initialize();
		}

		public AudioStreamState( bool record, string phoneNumber )
		{
			doRecordStream = record;
			this.phoneNumber = phoneNumber;
			Initialize();
		}

		/// <summary>
		/// perform Initialization of the instance to include
		/// creation of the audio stream and opening of the stream to write
		/// packets to if doRecordStream is set.
		/// </summary>
		private void Initialize()
		{
			string fileName;
			//const string sipDir = @"c:\siplogs";
            //string sipDir = Directory.GetCurrentDirectory(); // use the current directory
            string sipDir = AppDomain.CurrentDomain.BaseDirectory + "//ToneDetectionFiles";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "//ToneDetectionFiles"))
            { Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "//ToneDetectionFiles"); }
			audioStream = new AudioStream();

			if( doRecordStream )
			{
				if( !Directory.Exists( sipDir ) )
				{
					Directory.CreateDirectory( sipDir );
				}
				fileName = sipDir + @"\" + phoneNumber + "-" + DateTime.Now.Ticks.ToString() + "rtp.log";
				recordStream = File.Open( fileName, FileMode.Create );
			}
		}
		#endregion

		#region Public Functions
		/// <summary>
		/// Simply close anything that was open if necessary.
		/// </summary>
		public void Close()
		{
			audioStream.Close();
			audioStream = null;
			if( doRecordStream )
			{
				recordStream.Flush();
				recordStream.Close();
			}
		}

		/// <summary>
		/// The purpose of this method is to add a packet to the audio stream.
		/// The packet has not yet been decoded.
		/// </summary>
		/// <param name="rawRtpBytes">raw bytes from the packet</param>
		/// <returns>ToneDetected enum indicating if any tones were detected</returns>
		public ToneDetected AddPacket( byte [] rawRtpBytes )
		{
			ToneDetected retval;
			byte [] bytes;
			RTPDecoder rtpDecoder;

			retval = ToneDetected.NONE;

			if( doRecordStream )
			{
				bytes = BitConverter.GetBytes( rawRtpBytes.Length );
				recordStream.Write( bytes, 0, bytes.Length );
				recordStream.Write( rawRtpBytes, 0, rawRtpBytes.Length );
			}
			
			try
			{
				rtpDecoder = new RTPDecoder( rawRtpBytes );
				if( ( rtpDecoder.DecodedPayload != null ) && ( rtpDecoder.DecodedPayload.Length > 0 ) )
				{
					retval = audioStream.Add( rtpDecoder.DecodedPayload );
				}
			}
			catch( InvalidRTPPacketException invalidRTPPacketException )
			{
				invalidRTPPacketException.Data.Add( "My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--ToneDetect--:--ToneDetect.Business--:--AudioStreamState.cs--:--InvalidRTPPacketException()--" );
				ClsException.LogError( invalidRTPPacketException );
				ClsException.WriteToErrorLogFile( invalidRTPPacketException );
			}

			return retval;
		}
		#endregion
	}
}
