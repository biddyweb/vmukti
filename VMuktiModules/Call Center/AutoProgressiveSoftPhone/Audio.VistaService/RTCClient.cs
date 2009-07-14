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
using VMuktiAPI;

namespace VMuktiAudio.VistaService
{
    public class RTCClient
    {
        public bool Isvista = true;

        private string _SIPNUMBER = "", _SIPPASSWORD = "", _SIPSERVER = "";
        private List<RTCAudio> _Channels = new List<RTCAudio>();

        public delegate void delStatus(int ChannelId, string status);
        public event delStatus entstatus = null;

    

        public string SIPNumber
        {
            get
            { return _SIPNUMBER; }
            set
            { _SIPNUMBER = value; }
        }

        public string SIPPassword
        {
            get
            { return _SIPPASSWORD; }
            set
            { _SIPPASSWORD = value; }
        }

        public string SIPServer
        {
            get
            { return _SIPSERVER; }
            set
            { _SIPSERVER = value; }
        }        


        public RTCClient(string SIPNumber, string SIPPassword, string SIPServer)
        {
            try
            {
                _SIPNUMBER = SIPNumber;
                _SIPPASSWORD = SIPPassword;
                _SIPSERVER = SIPServer;

                _Channels.Add(new RTCAudio(_SIPNUMBER, _SIPPASSWORD, _SIPSERVER));
                _Channels[_Channels.Count - 1].ChannelId = _Channels.Count;
                _Channels[_Channels.Count - 1].entstatus += new RTCAudio.delStatus(RTCClient_entstatus);

                for (int i = 0; i < 5; i++)
                {
                    _Channels.Add(new RTCAudio());
                    _Channels[_Channels.Count - 1].ChannelId = _Channels.Count;
                    _Channels[_Channels.Count - 1].entstatus += new RTCAudio.delStatus(RTCClient_entstatus);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RTCCLient()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        void RTCClient_entstatus(RTCAudio sender, string status)
        {
            try
            {
                switch (status)
                {
                    case "InPorgress":
                        entstatus(sender.ChannelId, status);
                        break;

                    case "Connected":
                        entstatus(sender.ChannelId, status);
                        break;

                    case "Disconnected":
                        entstatus(sender.ChannelId, status);
                        break;

                    case "Incoming":
                        entstatus(sender.ChannelId, status);
                        break;

                    case "Answering":
                        entstatus(sender.ChannelId, status);
                        break;

                    case "Hold":
                        entstatus(sender.ChannelId, status);
                        break;

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RTCCLient_entstatus()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        public void Dial(string PhoneNumber, int Channel)
        {
            try
            {
                _Channels[Channel - 1].Connect(PhoneNumber);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "(Dial)", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        public void HangUp(int Channel)
        {
            try
            {
                _Channels[Channel - 1].DisConnect();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Hangup()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        public void Hold(int Channel, string status)
        {
            try { _Channels[Channel - 1].fncHold(status); }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Hold()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        public void Transfer(string number, int Channel)
        {
            try { _Channels[Channel - 1].fncTransfer(number); }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Transfer()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        public void Anser()
        {
            try { _Channels[0].fncAnser(); }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Anser()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }

        public void SendDTMF(string number, int Channel)
        {
            try
            {
                _Channels[Channel - 1].fncSendDTMF(number);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SendDTMF()", "VMuktiAudio.VistaService\\RTCClient.cs");
            }
        }
    }
}

