using System.ServiceModel;
//using VMukti.Bussiness.CommonDataContracts;

namespace rptActiveCall.Business
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapActiveCallReportService))]
    public interface INetP2PBootStrapActiveCallReportService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoinCall(string uname);

        [OperationContract(IsOneWay = true)]
        void svcGetCallInfo(string uName);

        [OperationContract(IsOneWay = true)]
        void svcActiveCalls(string uName,string campaignId,string groupName,string Status,string callDuration,string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcSetDuration(string uName, string Status, string callDuration, string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcUnJoinCall(string uname);

    }

    public interface INetP2PBootStrapReportChannel : INetP2PBootStrapActiveCallReportService, IClientChannel
    {

    }

}
