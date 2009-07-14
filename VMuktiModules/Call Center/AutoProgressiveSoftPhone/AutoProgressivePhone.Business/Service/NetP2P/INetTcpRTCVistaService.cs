using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace AutoProgressivePhone.Business.Service.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetTcpRTCVistaService))]
    public interface INetTcpRTCVistaService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin();

        [OperationContract(IsOneWay = true)]
        void svcCreateRTCClient();

        [OperationContract(IsOneWay = true)]
        void svcRegisterSIPPhone(string SIPUserName, string SIPPassword, string SIPServer);

        [OperationContract(IsOneWay = true)]
        void svcDial(string PhoneNo, int Channel);

        [OperationContract(IsOneWay = true)]
        void svcHangup(int Channel);

        [OperationContract(IsOneWay = true)]
        void svcSendDTMF(string DTMF, int Channel);

        [OperationContract(IsOneWay = true)]
        void svcAnswer();

        [OperationContract(IsOneWay = true)]
        void svcHold(int Channel, string HoldContent);

        [OperationContract(IsOneWay = true)]
        void svcTransfer(string PhoneNo, int Channel);

        [OperationContract(IsOneWay = true)]
        void svcRTCEvent(int ChannelID, string Event);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin();
    }

    public interface INetTcpRTCVistaServiceChannel : IClientChannel, INetTcpRTCVistaService
    { }
}
