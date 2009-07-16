/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
//using VMukti.Business.CommonDataContracts;
//using VMukti.Bussiness.InActiveAgent;

namespace AutoProgressivePhone.Business
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapActiveAgentReportService))]
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
                EntsvcSetAgentInfo(uName,campaignId,status,groupName,phoneNo,CallDuration,isPredictive);
            }
        }

        public void svcSetAgentStatus(string uName, string Status, string Phone_No, string CallDuration, bool isPredictive)
        {
            if (EntsvcSetAgentStatus!= null)
            {
                EntsvcSetAgentStatus(uName, Status,Phone_No,CallDuration,isPredictive);
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
    }
}
