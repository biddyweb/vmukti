using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using VMukti.Bussiness.CommonDataContracts;
//using VMukti.Bussiness.CommonDataContracts;

namespace VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P
{
    public class NetP2PBootStrapActiveCallReportDelegates : INetP2PBootStrapActiveCallReportService
    {
        public delegate void DelsvcJoin(string uname);
        //public delegate void DelsvcPhoneHangUp();
       // public delegate void DelsvcAgentLoggedIn(clsActive_InActiveAgent objAgentInfo);
        public delegate void DelsvcActiveCalls(ActiveCalls objActiveCalls);
        //public delegate void DelsvcCampActiveUsers(int intCampUsers);
        public delegate void DelsvcUnJoin(string uname);

        public event DelsvcJoin EntsvcJoin;
        //public event DelsvcPhoneHangUp EntsvcPhoneHangUp;
       // public event DelsvcAgentLoggedIn EntsvcAgentLoggedIn;
        public event DelsvcActiveCalls EntsvcActiveCalls;
        //public event DelsvcCampActiveUsers EntsvcCampActiveUsers;
        public event DelsvcUnJoin EntsvcUnJoin;

        public void svcJoin(string uname)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uname);
            }
        }

        //public void svcPhoneHangUp()
        //{
        //    if (EntsvcPhoneHangUp != null)
        //    {
        //        EntsvcPhoneHangUp();
                
        //    }
        //}

        //public void svcAgentLoggedIn(clsActive_InActiveAgent objAgentInfo)
        //{
        //    if (EntsvcAgentLoggedIn != null)
        //    {
        //        EntsvcAgentLoggedIn(objAgentInfo);
        //    }
        //}

        public void svcActiveCalls(ActiveCalls objActiveCalls)
        {
            if (EntsvcActiveCalls != null)
            {
                EntsvcActiveCalls(objActiveCalls);
            }
        }

        //public void svcCampActiveUsers(int intCampUsers)
        //{
        //    if (EntsvcCampActiveUsers != null)
        //    {
        //        EntsvcCampActiveUsers(intCampUsers);
        //    }
        //}

        public void svcUnJoin(string uname)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uname);
            }
        }
    }
}
