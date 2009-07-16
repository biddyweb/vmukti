using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Dialer_AutoProgressive.Business.Services
{
    public class NetP2PBootStrapDashBoardDelegate : INetP2PBootStrapDashBoardServices
    {
        public delegate void delsvcJoin(string uname);
        public delegate void delsvcUnJoin(string uname);
        public delegate void DelsvcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal);
        public delegate void DelsvcGetAgents(int intCampaignID, string uname);
        public delegate void DelsvcSetAgents(int intAgentID, string strAgentName, int intCampaignID, string to);

        public event delsvcJoin EntsvcJoin;
        public event delsvcUnJoin EntsvcUnJoin;
        public event DelsvcGetCallInfo EntsvcGetCallInfo;
        public event DelsvcGetAgents EntsvcGetAgents;
        public event DelsvcSetAgents EntsvcSetAgents;


        #region INetP2PBootStrapDashBoardServices Members

        void INetP2PBootStrapDashBoardServices.svcJoin(string uname)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uname);
            }
        }

        void INetP2PBootStrapDashBoardServices.svcUnJoin(string uname)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uname);
            }
        }
        void INetP2PBootStrapDashBoardServices.svcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal)
        {
            if (EntsvcGetCallInfo != null)
            {
                EntsvcGetCallInfo(LeadID, CalledDate, ModifiedDate, ModifiedBy, GeneratedBy, StartDate, StartTime, DurationInSecond, DispositionID, CampaignID, ConfID, IsDeleted, CallNote, isDNC, isGlobal);
            }
        }
        void INetP2PBootStrapDashBoardServices.svcGetAgents(int intCampaignID, string uname)
        {
            if (EntsvcGetAgents != null)
            {
                EntsvcGetAgents(intCampaignID, uname);
            }
        }

        void INetP2PBootStrapDashBoardServices.svcSetAgents(int intAgentID, string strAgentName, int intCampaignID, string to)
        {
            if (EntsvcSetAgents != null)
            {
                EntsvcSetAgents(intAgentID, strAgentName, intCampaignID, to);
            }
        }
        #endregion
    }
}
