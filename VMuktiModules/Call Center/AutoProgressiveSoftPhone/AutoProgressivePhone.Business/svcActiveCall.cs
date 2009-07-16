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

namespace AutoProgressivePhone.Business
{

    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapActiveCallReportService))]
    public interface INetP2PBootStrapActiveCallReportService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoinCall(string uname);

        [OperationContract(IsOneWay = true)]
        void svcGetCallInfo(string uName);

        [OperationContract(IsOneWay = true)]
        void svcActiveCalls(string uName, string campaignId, string groupName, string Status, string callDuration, string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcSetDuration(string uName, string Status, string callDuration, string phoneNo);

        [OperationContract(IsOneWay = true)]
        void svcUnJoinCall(string uname);

    }

    public interface INetP2PBootStrapReportChannel : INetP2PBootStrapActiveCallReportService, IClientChannel
    {

    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]

    public class NetP2PBootStrapActiveCallReportDelegates : INetP2PBootStrapActiveCallReportService
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
                EntsvcActiveCalls(uName, campaignId, groupName, Status, callDuration, phoneNo);
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
