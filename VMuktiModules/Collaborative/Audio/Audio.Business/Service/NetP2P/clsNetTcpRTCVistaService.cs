using System;
using System.Collections.Generic;
using System.Text;

namespace Audio.Business.Service.NetP2P
{
    public class clsNetTcpRTCVistaService : INetTcpRTCVistaService
    {
        public delegate void DelsvcJoin();
        public delegate void DelsvcCreateRTCClient();
        public delegate void DelsvcRegisterSIPPhone(string SIPUserName, string SIPPassword, string SIPServer);
        public delegate void DelsvcDial(string PhoneNo, int Channel);
        public delegate void DelsvcHangup(int Channel);
        public delegate void DelsvcSendDTMF(string DTMF, int Channel);
        public delegate void DelsvcAnswer();
        public delegate void DelsvcHold(int Channel, string HoldContent);
        public delegate void DelsvcTransfer(string PhoneNo, int Channel);
        public delegate void DelsvcRTCEvent(int ChannelId, string RTCEventName);
        public delegate void DelsvcUnJoin();

        public event DelsvcJoin entsvcJoin;
        public event DelsvcCreateRTCClient entsvcCreateRTCClient;
        public event DelsvcRegisterSIPPhone entsvcRegisterSIPPhone;
        public event DelsvcDial entsvcDial;
        public event DelsvcHangup entsvcHangup;
        public event DelsvcSendDTMF entsvcSendDTMF;
        public event DelsvcAnswer entsvcAnswer;
        public event DelsvcHold entsvcHold;
        public event DelsvcTransfer entsvcTransfer;
        public event DelsvcRTCEvent entsvcRTCEvent;
        public event DelsvcUnJoin entsvcUnJoin;

        #region INetTcpRTCVistaService Members

        void INetTcpRTCVistaService.svcJoin()
        {
            if (entsvcJoin != null)
            {
                entsvcJoin();
            }
        }

        void INetTcpRTCVistaService.svcCreateRTCClient()
        {
            if (entsvcCreateRTCClient != null)
            {
                entsvcCreateRTCClient();
            }
        }

        void INetTcpRTCVistaService.svcRegisterSIPPhone(string SIPUserName, string SIPPassword, string SIPServer)
        {
            if (entsvcRegisterSIPPhone != null)
            {
                entsvcRegisterSIPPhone(SIPUserName, SIPPassword, SIPServer);
            }
        }

        public void svcDial(string PhoneNo, int Channel)
        {
            if (entsvcDial != null)
            {
                entsvcDial(PhoneNo, Channel);
            }
        }

        public void svcHangup(int Channel)
        {
            if (entsvcHangup != null)
            {
                entsvcHangup(Channel);
            }
        }

        public void svcSendDTMF(string DTMF, int Channel)
        {
            if (entsvcSendDTMF != null)
            {
                entsvcSendDTMF(DTMF, Channel);
            }
        }

        public void svcAnswer()
        {
            if (entsvcAnswer != null)
            {
                entsvcAnswer();
            }
        }

        public void svcHold(int Channel, string HoldContent)
        {
            if (entsvcHold != null)
            {
                entsvcHold(Channel, HoldContent);
            }
        }

        public void svcTransfer(string PhoneNo, int Channel)
        {
            if (entsvcTransfer != null)
            {
                entsvcTransfer(PhoneNo, Channel);
            }
        }

        public void svcRTCEvent(int ChannelId, string RTCEventName)
        {
            if (entsvcRTCEvent != null)
            {
                entsvcRTCEvent(ChannelId, RTCEventName);
            }
        }

        void INetTcpRTCVistaService.svcUnJoin()
        {
            if (entsvcUnJoin != null)
            {
                entsvcUnJoin();
            }
        }
        #endregion
    }
}
