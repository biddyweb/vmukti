using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace PredictiveDialler.Business.DashBoard
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapDashBoardServices))]
    public interface INetP2PBootStrapDashBoardServices
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uname);

        [OperationContract(IsOneWay = true)]
        void svcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uname);

    }

    public interface INetP2PBootStrapdashBoardChannel : IClientChannel, INetP2PBootStrapDashBoardServices
    {

    }
}
