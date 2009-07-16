using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;
using VMukti.Bussiness.CommonDataContracts;
//using VMukti.Bussiness.CommonDataContracts;

namespace VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapActiveCallReportService))]
    public interface INetP2PBootStrapActiveCallReportService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uname);

        //[OperationContract(IsOneWay = true)]
        //void svcPhoneHangUp();

        //[OperationContract(IsOneWay = true)]
        //void svcAgentStatus();       

        //[OperationContract(IsOneWay = true)]
        //void svcAgentLoggedIn(clsActive_InActiveAgent objAgentInfo);

        //[OperationContract(IsOneWay = true)]
        //void svcCampActiveUsers(int intCampUsers);

        [OperationContract(IsOneWay = true)]
        void svcActiveCalls(ActiveCalls objActiveCalls);

        //[OperationContract(IsOneWay = true)]
        //void svcAgentLoggedOut(string stragentname);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uname);
    }

    public interface INetP2PBootStrapActiveCallReportChannel : INetP2PBootStrapActiveCallReportService, IClientChannel
    {
    }

}
