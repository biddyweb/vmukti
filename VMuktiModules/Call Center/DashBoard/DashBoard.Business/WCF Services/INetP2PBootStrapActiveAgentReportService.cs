using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace DashBoard.Business.WCF_Services
{
    [ServiceContract(CallbackContract=typeof(INetP2PBootStrapActiveAgentReportService))]
    public interface INetP2PBootStrapActiveAgentReportService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName, string campaignId);

        [OperationContract(IsOneWay = true)]
        void svcGetAgentInfo(string uNameHost);

        [OperationContract(IsOneWay = true)]
        void svcSetAgentInfo(string uName, string campaignId, string status, string groupName, string phoneNo, string CallDuration, bool isPredictive);

        [OperationContract(IsOneWay = true)]
        void svcSetAgentStatus(string uName, string Status, string Phone_No, string CallDuration, bool isPredictive);

        [OperationContract(IsOneWay = true)]
        void svcBargeRequest(string uName, string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcBargeReply(string confNo);

        [OperationContract(IsOneWay = true)]
        void svcHangUp(string uName, string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);
    }

    public interface INetP2PBootStrapActiveAgentReportChannel : INetP2PBootStrapActiveAgentReportService, IClientChannel
    {
    }
}
