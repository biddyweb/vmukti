/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using System.Collections.Generic;
using System.Text;
//using RTCCORELib;
using System.Data.SqlClient;
using System.Data;
using RTCCORELib;
using VMuktiAPI;



namespace AutoProgressivePhone.Business
{
    public static class SIPCredentials
    {
        private static string _SIPNUMBER = "", _SIPPASSWORD = "", _SIPSERVER = "";

        public static string SIPNumber
        {
            get
            { return _SIPNUMBER; }
            set
            { _SIPNUMBER = value; }
        }

        public static string SIPPassword
        {
            get
            { return _SIPPASSWORD; }
            set
            { _SIPPASSWORD = value; }
        }

        public static string SIPServer
        {
            get
            { return _SIPSERVER; }
            set
            { _SIPSERVER = value; }
        }
    }

    public class RTCAudio
    {
        private int _ChannelId = -1;
        private int _IPGCounter = -1;

        public int ChannelId
        {
            get
            {
                return _ChannelId;
            }
            set
            {
                _ChannelId = value;
            }
        }

        public delegate void delStatus(RTCAudio sender, string status);
        public event delStatus entstatus = null;

        public System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();


        #region Constants
        //Flags for CreateSession/AddBuddy
        const int RTCCS_FORCE_PROFILE = 1;
        const int RTCCS_FAIL_ON_REDIRECT = 2;

        //Media Types
        const int RTCMT_AUDIO_SEND = 1;
        const int RTCMT_AUDIO_RECEIVE = 2;
        const int RTCMT_VIDEO_SEND = 4;
        const int RTCMT_VIDEO_RECEIVE = 8;
        const int RTCMT_T120_SENDRECV = 16;
        const int RTCMT_ALL_RTP = 15;
        const int RTCMT_ALL = 31;

        //Session types
        const int RTCSI_PC_TO_PC = 1;
        const int RTCSI_PC_TO_PHONE = 2;
        const int RTCSI_PHONE_TO_PHONE = 4;
        const int RTCSI_IM = 8;

        //Transports
        const int RTCTR_UDP = 1;
        const int RTCTR_TCP = 2;
        const int RTCTR_TLS = 4;

        //Registration Flags
        const int RTCRF_REGISTER_INVITE_SESSIONS = 1;
        const int RTCRF_REGISTER_MESSAGE_SESSIONS = 2;
        const int RTCRF_REGISTER_PRESENCE = 4;
        const int RTCRF_REGISTER_ALL = 7;

        //Event filters
        const int RTCEF_CLIENT = 1;
        const int RTCEF_REGISTRATION_STATE_CHANGE = 2;
        const int RTCEF_SESSION_STATE_CHANGE = 4;
        const int RTCEF_SESSION_OPERATION_COMPLETE = 8;
        const int RTCEF_PARTICIPANT_STATE_CHANGE = 16;
        const int RTCEF_MEDIA = 32;
        const int RTCEF_INTENSITY = 64;
        const int RTCEF_MESSAGING = 128;
        const int RTCEF_BUDDY = 256;
        const int RTCEF_WATCHER = 512;
        const int RTCEF_PROFILE = 1024;
        const int RTCEF_ALL = 33554431;

        //RTC_EVENTFILTERS will be used to register for receiving events.
        const Int16 RTC_EVENTFILTERS = RTCEF_CLIENT | RTCEF_SESSION_STATE_CHANGE | RTCEF_MEDIA;

        //Constants related to the IVideoInterface. These are used
        const int WS_CHILD = 1073741824;
        const int WS_CLIPSIBLINGS = 67108864;
        #endregion

        #region "Objects and Interfaces for this lab"
        private IRTCProfile2 profile;
        private IRTCProfile profile1;
        private IRTCSession osession;
        private IRTCParticipant oparticipant;
        private RTCClientClass oclient = new RTCClientClass();
        private IRTCSessionCallControl oSessionCallControl;
        public long lCookie;
        #endregion



        public RTCAudio()
        {
            try
            {
                oclient = new RTCClientClass();
                oclient.Initialize();               
                oclient.EventFilter = RTCEF_ALL;
                oclient.ListenForIncomingSessions = RTC_LISTEN_MODE.RTCLM_BOTH;
                oclient.SetPreferredMediaTypes(RTCMT_AUDIO_SEND | RTCMT_AUDIO_RECEIVE, true);
                oclient.EventFilter = 0x00000004 | 0x00000020 | 0x00001000;
                oclient.IRTCEventNotification_Event_Event += new IRTCEventNotification_EventEventHandler(oclient_IRTCEventNotification_Event_Event);               
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                //System.Windows.MessageBox.Show("Asterisk Server is not started or not working properly!!", "Video Conference");
            }
        }

        public RTCAudio(string SIPNumber, string SIPPassword, string SIPServer)
        {
            try
            {
                SIPCredentials.SIPNumber = SIPNumber;
                SIPCredentials.SIPPassword = SIPPassword;
                SIPCredentials.SIPServer = SIPServer;

                oclient = new RTCClientClass();
                oclient.Initialize();
                oclient.EventFilter = RTCEF_ALL;
                oclient.ListenForIncomingSessions = RTC_LISTEN_MODE.RTCLM_BOTH;
                oclient.SetPreferredMediaTypes(RTCMT_AUDIO_SEND | RTCMT_AUDIO_RECEIVE, true);
                oclient.EventFilter = 0x00000004 | 0x00000020 | 0x00001000;
                oclient.IRTCEventNotification_Event_Event += new IRTCEventNotification_EventEventHandler(oclient_IRTCEventNotification_Event_Event);                

                dt.Start();
                dt.Tick += new EventHandler(dt_Tick);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                //System.Windows.MessageBox.Show("Asterisk Server is not started or not working properly!!", "Video Conference");
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            try
            {
                string sprofile = "<provision key='5B29C449-29EE-4fd8-9E3F-04AED077690E' name=\"Asterisk\"> \n <user account='" + SIPCredentials.SIPNumber + "' password='" + SIPCredentials.SIPPassword + "' uri='SIP:" + SIPCredentials.SIPNumber + "@" + SIPCredentials.SIPServer + "' /> <sipsrv addr='" + SIPCredentials.SIPServer + "' protocol='udp' auth='digest' role='registrar'> <session party='first' type='pc2ph' /> </sipsrv> </provision>\n\r";
                this.profile = ((IRTCProfile2)(this.oclient.CreateProfile(sprofile)));
                this.oclient.EnableProfile(this.oclient.CreateProfile(sprofile), 15);
                dt.Stop();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public virtual void Connect(string number)
        {
            try
            {
                string strRemoteURI;
                if (number.Contains("@"))
                {
                    strRemoteURI = "SIP:" + number;
                }
                else
                {
                    strRemoteURI = "SIP:" + number + "@" + SIPCredentials.SIPServer;
                }
                osession = oclient.CreateSession(RTC_SESSION_TYPE.RTCST_PC_TO_PHONE, null, null, 0);
                oparticipant = osession.AddParticipant(strRemoteURI, SIPCredentials.SIPNumber);
                oSessionCallControl = (IRTCSessionCallControl)(osession);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                System.Windows.MessageBox.Show("Could not make a call, Please refer Error Log");
            }
        }

        #region Functionallity
        public virtual void DisConnect()
        {
            try
            {
                if (osession.State == RTCCORELib.RTC_SESSION_STATE.RTCSS_INCOMING)
                {
                    osession.Terminate(RTCCORELib.RTC_TERMINATE_REASON.RTCTR_REJECT);
                    oclient.PlayRing(RTC_RING_TYPE.RTCRT_PHONE, false);
                }
                else
                {
                    osession.Terminate(RTCCORELib.RTC_TERMINATE_REASON.RTCTR_NORMAL);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncTransfer(string Number)
        {
            try
            {                
                oSessionCallControl.Refer("sip:" + Number + "@" + SIPCredentials.SIPServer, "");
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncSpk(string status)
        {
            try
            {
                if (status == "Spk. Off")
                {
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_SPEAKER, true);
                }
                else if (status == "Spk. On")
                {
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_SPEAKER, false);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncMic(string status)
        {
            try
            {
                if (status == "Mic Off")
                {
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_MICROPHONE, true);
                }
                else if (status == "Mic On")
                {
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_MICROPHONE, false);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncMute(string status)
        {
            try
            {
                if (status == "Mute")
                {
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_MICROPHONE, true);
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_SPEAKER, true);
                }
                else if (status == "UnMute")
                {
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_MICROPHONE, false);
                    oclient.set_AudioMuted(RTC_AUDIO_DEVICE.RTCAD_SPEAKER, false);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncHold(string status)
        {
            try
            {
                if (status == "Hold")
                {
                    oSessionCallControl.Hold(Convert.ToInt16(lCookie));
                }
                else
                {
                    oSessionCallControl.UnHold(Convert.ToInt16(lCookie));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncSendDTMF(string number)
        {
            try
            {
                switch (number)
                {
                    case "0":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_0);
                        break;

                    case "1":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_1);
                        break;

                    case "2":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_2);
                        break;

                    case "3":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_3);
                        break;

                    case "4":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_4);
                        break;

                    case "5":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_5);
                        break;

                    case "6":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_6);
                        break;

                    case "7":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_7);
                        break;

                    case "8":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_8);
                        break;

                    case "9":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_9);
                        break;

                    case "#":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_POUND);
                        break;

                    case "*":
                        oclient.SendDTMF(RTC_DTMF.RTC_DTMF_STAR);
                        break;
                }
            }

            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        public void fncAnser()
        {
            try
            {
                osession.Answer();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }
        #endregion

        #region RTC Session Events

        void oclient_IRTCEventNotification_Event_Event(RTC_EVENT RTCEvent, object pEvent)
        {
            try
            {
                switch (RTCEvent)
                {
                    case RTC_EVENT.RTCE_SESSION_STATE_CHANGE:
                        {
                            IRTCSessionStateChangeEvent sessionevent = null;
                            sessionevent = (IRTCSessionStateChangeEvent)pEvent;
                            OnIRTCSessionStateChangeEvent(sessionevent);
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }


        private void OnIRTCSessionStateChangeEvent(IRTCSessionStateChangeEvent sessionevent)
        {
            try
            {
                RTC_SESSION_STATE sessionstate;
                sessionstate = sessionevent.State;

                switch (sessionstate)
                {
                    case RTC_SESSION_STATE.RTCSS_INPROGRESS:
                        {
                            if (_IPGCounter == -1)
                            {
                                entstatus(this, "InPorgress");
                                _IPGCounter = 1;
                            }
                            else if (_IPGCounter == 2)
                            { _IPGCounter = -1; }
                            else
                            { _IPGCounter += 1; }
                            break;
                        }

                    case RTC_SESSION_STATE.RTCSS_INCOMING:
                        {
                            oclient.PlayRing(RTC_RING_TYPE.RTCRT_PHONE, true);
                            osession = sessionevent.Session;
                            entstatus(this, "Incoming");
                            break;
                        }

                    case RTC_SESSION_STATE.RTCSS_ANSWERING:
                        {
                            //oclient.SendDTMF
                            entstatus(this, "Answering");
                            break;
                        }

                    case RTC_SESSION_STATE.RTCSS_CONNECTED:
                        {
                            entstatus(this, "Connected");
                            break;
                        }

                    case RTC_SESSION_STATE.RTCSS_DISCONNECTED:
                        {
                            //int i = this.oclient.SessionCapabilities;
                            //string ss = this.oclient.SessionCapabilities.ToString();
                            //int str1 = this.profile.SessionCapabilities.GetHashCode();
                            osession = null;
                            entstatus(this, "Disconnected");
                            break;
                        }

                    case RTC_SESSION_STATE.RTCSS_HOLD:
                        {
                            entstatus(this, "Hold");
                            break;
                        }

                    case RTC_SESSION_STATE.RTCSS_REFER:
                        {
                            break;
                        }
                }
                sessionevent = null;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Business--:--RTCAudio.cs--:--OnIRTCSessionStateChangeEvent()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Functions
        private void OnInfoEvent(IRTCInfoEvent InfoEvent)
        { }

        private void OnGroupEvent(IRTCBuddyGroupEvent GrupEvent)
        { }

        //RTC_DTMF
        private void OnMediaEvent(IRTCMediaEvent MediaEvent)
        {
            //System.Windows.MessageBox.Show(MediaEvent.MediaType.ToString());
        }
        #endregion

      
    }
}

