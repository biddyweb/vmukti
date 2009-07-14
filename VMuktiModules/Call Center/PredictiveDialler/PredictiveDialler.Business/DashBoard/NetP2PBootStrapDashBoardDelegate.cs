using System;
using System.Collections.Generic;
using System.Text;

namespace PredictiveDialler.Business.DashBoard
{
    public class NetP2PBootStrapDashBoardDelegate : INetP2PBootStrapDashBoardServices
    {
        public delegate void delsvcJoin(string uname);
        public delegate void delsvcUnJoin(string uname);
        public delegate void DelsvcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal);

        public event delsvcJoin EntsvcJoin;
        public event delsvcUnJoin EntsvcUnJoin;
        public event DelsvcGetCallInfo EntsvcGetCallInfo;




        #region INetP2PBootStrapDashBoardServices Members

        public void svcJoin(string uname)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uname);
            }
        }

        public void svcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal)
        {
            if (EntsvcGetCallInfo != null)
            {
                EntsvcGetCallInfo(LeadID, CalledDate, ModifiedDate, ModifiedBy, GeneratedBy, StartDate, StartTime, DurationInSecond, DispositionID, CampaignID, ConfID, IsDeleted, CallNote, isDNC, isGlobal);
            }

        }

        public void svcUnJoin(string uname)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uname);
            }
        }

        #endregion
    }
}
