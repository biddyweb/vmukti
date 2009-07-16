using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace DashBoard.Business.WCF_Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NetP2PBootStrapActiveAgentReportDelegates : INetP2PBootStrapActiveAgentReportService
    {
        public delegate void DelsvcJoin(string uName, string campaignId);        
        public delegate void DelsvcGetAgentInfo(string uNameHost);
        public delegate void DelsvcSetAgentInfo(string uName, string campaignId, string status, string groupName, string phoneNo, string CallDuration, bool isPredictive);
        public delegate void DelsvcSetAgentStatus(string uName, string Status, string Phone_No, string CallDuration, bool isPredictive);
        public delegate void DelsvcBargeRequest(string uName, string phoneNo);
        public delegate void DelsvcBargeReply(string confNo);
        public delegate void DelsvcHangUp(string uName, string phoneNo);
        public delegate void DelsvcUnJoin(string uName);

        public event DelsvcJoin EntsvcJoin;        
        public event DelsvcGetAgentInfo EntsvcGetAgentInfo;
        public event DelsvcSetAgentInfo EntsvcSetAgentInfo;
        public event DelsvcSetAgentStatus EntsvcSetAgentStatus;
        public event DelsvcBargeRequest EntsvcBargeRequest;
        public event DelsvcBargeReply EntsvcBargeReply;
        public event DelsvcHangUp EntsvcHangUp;
        public event DelsvcUnJoin EntsvcUnJoin;

        #region INetP2PBootStrapActiveAgentReportService Members

        public void svcJoin(string uName, string campaignId)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uName, campaignId);
            }
        }

        public void svcGetAgentInfo(string uNameHost)
        {
            if (EntsvcGetAgentInfo != null)
            {
                EntsvcGetAgentInfo(uNameHost);
            }
        }

        public void svcSetAgentInfo(string uName, string campaignId, string status, string groupName, string phoneNo, string CallDuration, bool isPredictive)
        {
            if (EntsvcSetAgentInfo != null)
            {
                EntsvcSetAgentInfo(uName, campaignId, status, groupName, phoneNo, CallDuration,isPredictive);
            }            
        }

        public void svcSetAgentStatus(string uName, string Status, string Phone_No, string CallDuration, bool isPredictive)
        {
            if (EntsvcSetAgentStatus != null)
            {
                EntsvcSetAgentStatus(uName, Status, Phone_No, CallDuration,isPredictive);
            }
        }

        public void svcBargeRequest(string uName, string phoneNo)
        {
            if (EntsvcBargeRequest != null)
            {
                EntsvcBargeRequest(uName, phoneNo);
            }
        }

        public void svcBargeReply(string confNo)
        {
            if (EntsvcBargeReply != null)
            {
                EntsvcBargeReply(confNo);
            }
        }

        public void svcHangUp(string uName, string phoneNo)
        {
            if (EntsvcHangUp != null)
            {
                EntsvcHangUp(uName, phoneNo);
            }
        }

        public void svcUnJoin(string uName)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uName);
            }            
        }



        #endregion
    }
}
