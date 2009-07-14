using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

using ToneDetect.Business;
using ToneDetect.Business.Detect;
using ToneDetect.Business.RTP;
using ToneDetect.Business.SIP;
using VMuktiAPI;

namespace PredictivePhone.Business
{
    internal class RTCAudioWithToneDetect : RTCAudio
    {
        #region Fields
        private AudioStreamState audioStreamState;

        /// <summary>
        /// The RTP Port number (UDP) the audio stream will be coming to us on.
        /// </summary>
        private int RTPPort;

        /// <summary>
        /// tracks the state of the connection.
        /// </summary>
        private SipState sipState;

        private string phoneNumber;

		/// <summary>
		/// a flag to indicate that SOMETHING was found.  The code
		/// will stop checking after this is set.
		/// </summary>
		private bool someToneDetected;

        // when set, the call is logged.
        private bool logCall;

        #endregion

        #region Events/Delegates
        public delegate void NonHumanDetectedDelegate(object sender, ToneDetected tone);
        public event NonHumanDetectedDelegate NonHumanDetected;
        #endregion

        #region Properties
        /// <summary>
        /// set to log the call
        /// </summary>
        public bool LogCall
        {
            get
            {
                return logCall;
            }
            set
            {
                logCall = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Construct the communication link handler
        /// </summary>
        /// <param name="SIPNumber">SIP number that will be dialed</param>
        /// <param name="SIPPassword">password</param>
        /// <param name="SIPServer">server</param>
        internal RTCAudioWithToneDetect(
            string SIPNumber,
            string SIPPassword,
            string SIPServer)
            : base(SIPNumber, SIPPassword, SIPServer)
        {
            //hook up to the sip and rtp events.
            SniffSIP.Instance.SipArrived += OnSipArrived;
            SniffSIP.Instance.RTPArrived += OnRTPArrived;
            this.logCall = false;
        }
        /// <summary>
        /// Construct the communication link handler
        /// </summary>
        /// <param name="SIPNumber">SIP number that will be dialed</param>
        /// <param name="SIPPassword">password</param>
        /// <param name="SIPServer">server</param>
        internal RTCAudioWithToneDetect(
            string SIPNumber,
            string SIPPassword,
            string SIPServer,
            bool logCall)
            : base(SIPNumber, SIPPassword, SIPServer)
        {
            //hook up to the sip and rtp events.
            SniffSIP.Instance.SipArrived += OnSipArrived;
            SniffSIP.Instance.RTPArrived += OnRTPArrived;
            this.logCall = logCall;
        }
        #endregion

        #region Connection / Disconnect overrides
        public override void Connect(string phoneNumber)
        {
            string s;
            s = Thread.CurrentThread.Name;
            RTPPort = 0;
			someToneDetected = false;
            this.phoneNumber = phoneNumber;
			try
			{
            sipState = new SipState();
            audioStreamState = new AudioStreamState(this.LogCall, phoneNumber);
            base.Connect(phoneNumber);
        }
			catch( Exception ex )
			{
				VMuktiHelper.ExceptionHandler( ex, "Connect()", "RTCAudioWithToneDetect.cs" );
			}
		}

        public override void DisConnect()
        {
            RTPPort = 0; // make sure we won't process any more packets on this port.
            sipState.Close();
            sipState = null;
            audioStreamState.Close();
            audioStreamState = null;
            phoneNumber = String.Empty;
            base.DisConnect();
        }
        #endregion

        #region Event Handlers
        private void OnSipArrived(object sender, SipParser parser)
        {
            SipConnectionState state;
            string localNumber; // this is the number to use in the 'client' side
            try
            {
                if ((phoneNumber != null) && (phoneNumber != string.Empty) && (sipState != null))
                {
                    // since the 5555 was added, need to skip over it; it may not always be present.
                    // TODO: need to make this a configurable number from the application in some manner.
                    // (both in length and content)
                    if (phoneNumber.Substring(0, 4).Equals("5555"))
                    {
                        localNumber = phoneNumber.Substring(4);
                    }
                    else
                    {
                        localNumber = phoneNumber;
                    }
                    if (parser.ToField.Contains(localNumber))
                    {
                        state = sipState.Process(parser);
                        if (state == SipConnectionState.RTP_DETECTED)
                        {
                            RTPPort = sipState.RTPPort;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudioWithToneDetect.cs--:--OnSIPArrived()--");
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Event handler for the RTP packets.
        /// </summary>
        /// <param name="sender">the packet sniffer in this case</param>
        /// <param name="eventArgs">contains the port number and RTP data stream.</param>
        private void OnRTPArrived(object sender, RTPPacketEventArgs eventArgs)
        {
            ToneDetected toneDetected;

            // if we care about the port...process.
            if (RTPPort == eventArgs.Port)
            {
                try
                {
					if( !someToneDetected )
					{
                    toneDetected = audioStreamState.AddPacket(eventArgs.Packet);
                    if (toneDetected != ToneDetected.NONE)
                    {
							someToneDetected = true;
                        ClsException.WriteToLogFile(
                            string.Format("{0} Tone Detected: {1}",
                            phoneNumber, toneDetected.ToString()));
                        RaiseNonHumanDetected(toneDetected);
						}
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudioWithToneDetect.cs--:--OnRTPArrived()--");
                    ClsException.WriteToErrorLogFile(ex);
                }
            }
        }

        /// <summary>
        /// Raise the event that something non human was found on the line.
        /// </summary>
        /// <param name="toneDetected"></param>
        private void RaiseNonHumanDetected(ToneDetected toneDetected)
        {
            if (this.NonHumanDetected != null)
            {
                NonHumanDetected(this, toneDetected);
            }
        }
        #endregion
    }
}
