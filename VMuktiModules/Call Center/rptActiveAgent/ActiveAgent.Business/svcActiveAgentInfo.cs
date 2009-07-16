using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
//using VMukti.Business.CommonDataContracts;
using VMukti.Bussiness.InActiveAgent;

//namespace VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P
namespace VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapActiveAgentReportService))]
    public interface INetP2PBootStrapActiveAgentReportService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName, string campaignId);

        //[OperationContract(IsOneWay = true)]
        //void svcJoined(string uName, string campaignId);

        [OperationContract(IsOneWay = true)]
        void svcGetAgentInfo(string uNameHost);

        [OperationContract(IsOneWay = true)]
        //void svcSetAgentInfo(clsActive_InActiveAgent objAgentInfo, string uName);
        void svcSetAgentInfo(string uName, string campaignId, string status,string groupName,string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcSetAgentStatus(string uName, string Status,string Phone_No);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);
    }

    public interface INetP2PBootStrapActiveAgentReportChannel : INetP2PBootStrapActiveAgentReportService,IClientChannel
    {
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NetP2PBootStrapActiveAgentReportDelegates : INetP2PBootStrapActiveAgentReportService
    {
        public delegate void DelsvcJoin(string uName, string campaignId);
        //public delegate void DelsvcJoined(string uName, string campaignId);
        public delegate void DelsvcGetAgentInfo(string uNameHost);
        //public delegate void DelsvcSetAgentInfo(clsActive_InActiveAgent objAgentInfo, string uName);
        public delegate void DelsvcSetAgentInfo(string uName,string campaignId, string status,string groupName,string phoneNo);
        public delegate void DelsvcSetAgentStatus(string uName, string Status,string Phone_No);
        public delegate void DelsvcUnJoin(string uName);

        public event DelsvcJoin EntsvcJoin;
        //public event DelsvcJoined EntsvcJoined;
        public event DelsvcGetAgentInfo EntsvcGetAgentInfo;
        public event DelsvcSetAgentInfo EntsvcSetAgentInfo;
        public event DelsvcSetAgentStatus EntsvcSetAgentStatus;
        public event DelsvcUnJoin EntsvcUnJoin;

        public void svcJoin(string uName, string campaignId)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uName,campaignId);
            }
        }
        //public void svcJoined(string uName, string campaignId)
        //{
        //    if (EntsvcJoined != null)
        //    {
        //        EntsvcJoined(uName, campaignId);
        //    }
        //}

        public void svcGetAgentInfo(string uNameHost)
        {
            if (EntsvcGetAgentInfo != null)
            {
                EntsvcGetAgentInfo(uNameHost);
            }
        }
        //public void svcSetAgentInfo(clsActive_InActiveAgent objAgentInfo, string uName)
        public void svcSetAgentInfo(string uName,string campaignId,string status,string groupName,string phoneNo)
        {
            if (EntsvcSetAgentInfo != null)
            {
                EntsvcSetAgentInfo(uName,campaignId,status,groupName,phoneNo);
            }
        }

        public void svcSetAgentStatus(string uName, string Status,string Phone_No)
        
        {
            if (EntsvcSetAgentStatus != null)
            {
                EntsvcSetAgentStatus(uName,Status,Phone_No);
            }
        }

        public void svcUnJoin(string uName)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uName);
            }
        }
    }
}
