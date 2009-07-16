//using VMukti.Bussiness.CommonDataContracts;

namespace rptActiveCall.Business
{
    public class NetP2PBootStrapActiveCallReportDelegates :  INetP2PBootStrapActiveCallReportService
    {
        public delegate void DelsvcJoinCall(string uname);
        public delegate void DelsvcGetCallInfo(string uName);
        public delegate void DelsvcActiveCalls(string uName, string campaignId, string groupName, string Status, string callDuration, string phoneNo);
        public delegate void DelsvcSetDuration(string uName, string Status, string callDuration, string phoneNo);
        public delegate void DelsvcUnJoinCall(string uname);


        public event DelsvcJoinCall EntsvcJoinCall;
        public event DelsvcGetCallInfo EntsvcGetCallInfo;
        public event DelsvcActiveCalls EntsvcActiveCalls;
        public event DelsvcSetDuration EntsvcSetDuration;
        public event DelsvcUnJoinCall EntsvcUnJoinCall;


        public void svcJoinCall(string uname)
        {
            if (EntsvcJoinCall != null)
            {
                EntsvcJoinCall(uname);
            }
        }

        public void svcGetCallInfo(string uName)
        {
            if (EntsvcGetCallInfo != null)
            {
                EntsvcGetCallInfo(uName);
            }
        }

        public void svcActiveCalls(string uName, string campaignId, string groupName, string Status, string callDuration, string phoneNo)
        {
            if (EntsvcActiveCalls != null)
            {
                EntsvcActiveCalls(uName,campaignId,groupName,Status,callDuration,phoneNo);
            }
        }

        public void svcSetDuration(string uName, string Status, string callDuration, string phoneNo)
        {
            if (EntsvcSetDuration != null)
            {
                EntsvcSetDuration(uName, Status, callDuration, phoneNo);
            }
        }

        public void svcUnJoinCall(string uname)
        {
            if (EntsvcUnJoinCall != null)
            {
                EntsvcUnJoinCall(uname);
            }
        }

    }
}
