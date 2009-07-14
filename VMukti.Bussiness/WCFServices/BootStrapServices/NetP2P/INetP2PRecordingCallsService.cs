using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;
using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetP2PRecordingCallsService))]
    public interface INetP2PRecordingCallsService
    {
        [OperationContract(IsOneWay = true)]
        void svcNetP2PRecordingServiceJoin();

        [OperationContract(IsOneWay = true)]
        void svcNetP2PSendRecordedFile(string strFileName, byte[] bytearr);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PReceiveRecordedFile(string strFileName, byte[] bytearr,int intSig);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PRecordingUnJoin();
      
    }

    public interface INetP2PRecordingCallsServiceChannel : INetP2PRecordingCallsService, IClientChannel
    {
    }
}
