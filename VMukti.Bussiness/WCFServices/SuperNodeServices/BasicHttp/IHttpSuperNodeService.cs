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

using System.Collections.Generic;
using System.ServiceModel;
using VMukti.Business.CommonDataContracts;
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using System;

namespace VMukti.Business.WCFServices.SuperNodeServices.BasicHttp
{
    [ServiceContract]
    public interface IHttpSuperNodeService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName, string bindingType);

        [OperationContract(IsOneWay = false)]
        List<string> GetBuddies(string uName);

        [OperationContract(IsOneWay = true)]
        void AddBuddies(string username, string buddyname, string buddystatus);

        [OperationContract(IsOneWay = true)]
        void AddNodeBuddies(string username, List<string> NodeBuddies);


        [OperationContract(IsOneWay = true)]
		void RemoveBuddies(string username, List<string> buddyname);

        [OperationContract(IsOneWay = true)]
        void svcUnjoin(string uName);

        [OperationContract(IsOneWay = false)]
        List<string> GetBuddyStatus(List<string> BuddyList);

        [OperationContract(IsOneWay = false)]
        bool IsSuperNodeAvailable();

        [OperationContract(IsOneWay = false)]
        List<clsMessage> svcGetSpecialMsgs(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSetSpecialMsgs(string from, string to, string msg, clsModuleInfo objModInfo);

        [OperationContract(IsOneWay = true, Name = "svcSetPageSpecialMsg")]
        void svcSetSpecialMsgs(clsPageInfo objPageInfo);
        //void svcSetSpecialMsgs(string from, string to, string msg, List<clsModuleInfo> lstModInfo);

        [OperationContract(IsOneWay = true, Name = "svcSetSpecialMsg4MultipleBuddies")]
        void svcSetSpecialMsgs(string from, List<string> to, string msg, clsModuleInfo objModInfo);


        [OperationContract(IsOneWay = true, Name = "svcPageBuddyRetSetSpecialMsg")]
        void svcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objPageInfo);

        [OperationContract(IsOneWay = true, Name = "svcSetSpecialMsgBuddiesClick")]
        void svcSetSpecialMsgs(string from, List<string> to, string msg, clsModuleInfo objModInfo,clsPageInfo objPageInfo);


        [OperationContract(IsOneWay = true)]
        void svcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo);

        [OperationContract(IsOneWay = true,Name="svcAddPageDraggedBuddy")]
        void svcAddDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModInfo);


        [OperationContract(IsOneWay = true)]
        void svcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo);


        [OperationContract(IsOneWay = false)]
        string[] svcStartAService(VMuktiAPI.PeerType PeerType, string ModuleName);

        [OperationContract(IsOneWay = false)]
        string[] IsAuthorizedUser(clsBootStrapInfo objBootStrapInfo, string strUserName, string strPassword);

        [OperationContract(IsOneWay = false)]
        string IsAuthorized(string strUserName);

        [OperationContract(IsOneWay = false)]
        string svcAddSIPUser();

        [OperationContract(IsOneWay = true)]
        void svcRemoveSIPUser(string strSIPNumber);

        [OperationContract(IsOneWay = false)]
        string svcGetConferenceNumber();

        [OperationContract(IsOneWay = true)]
        void svcGetNodeNameByIP(string NodeName,string NodeIP);

        [OperationContract(IsOneWay = true, Name = "svcEnterConf")]
        void svcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress);



        [OperationContract(IsOneWay = true, Name = "svcJoinConf")]
        void svcJoinConf(string from, string to, int confid, string ipaddress);

        [OperationContract(IsOneWay = true, Name = "svcSendConfInfo")]
        void svcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress);

        [OperationContract(IsOneWay = true, Name = "svcAddConfBuddy")]
        void svcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress);

        [OperationContract(IsOneWay = true, Name = "svcRemoveConf")]
        void svcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress);

        [OperationContract(IsOneWay = true, Name = "svcUnJoinConf")]
        void svcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress);



        [OperationContract(IsOneWay = true, Name = "svcPodNavigation")]
        void svcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid,string ipaddress);



        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcGetSpecialMsgs(string uName, AsyncCallback callback, object asyncState);
        List<clsMessage> EndsvcGetSpecialMsgs(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetBuddies(string uName, AsyncCallback callback, object asyncState);
        List<string> EndGetBuddies(IAsyncResult result);


    }

    public interface IHTTPSuperNodeServiceChannel : IClientChannel, IHttpSuperNodeService
    {
    }



}
