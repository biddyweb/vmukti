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

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapService))]
    public interface INetP2PBootStrapService
    {
        [OperationContract(IsOneWay = true)]
        void svcNetP2PServiceJoin(string sSuperNodeIP);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PAddUser(string uName);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PRemoveUser(string uName);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PGetBuddyInfo(string uName);

        [OperationContract(IsOneWay = true)]
        void svcAddBuddies(string username, string BuddyName, string BuddyStatus);

        [OperationContract(IsOneWay = true)]
        void svcRemoveBuddies(string username, List<string> BuddyName);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PReturnBuddyInfo(string uName, List<string> lstMyBuddyList);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PGetSuperNodeBuddyStatus(string uName);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PReturnSuperNodeBuddyStatus(string uName, List<string> lstSNBuddyList);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PUnJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress);


        [OperationContract(IsOneWay = true, Name = "svcSetPageSpecialMsg")]
        void svcSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress);
        //void svcSetSpecialMsg(string from, string to, string msg, List<clsModuleInfo> lstModuleInfo,string IPAddress);

        [OperationContract(IsOneWay = true, Name = "svcPageBuddyRetSetSpecialMsg")]
        void svcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objPageInfo, string IPAddress);

        [OperationContract(IsOneWay = true, Name = "svcSetSpecialMsg4MultipleBuddies")]
        void svcSetSpecialMsg(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress);

        [OperationContract(IsOneWay = true, Name = "svcSetSpecialMsgBuddiesClick")]
        void svcSetSpecialMsg(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo);

        [OperationContract(IsOneWay = true)]
        void svcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress);


        [OperationContract(IsOneWay = true)]
        void svcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress);

        [OperationContract(IsOneWay = true, Name = "svcAddPageDraggedBuddy")]
        void svcAddDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModuleInfo, string IPAddress);


        [OperationContract(IsOneWay = true)]
        void svcBuzzSuperNode(string uname);

        [OperationContract(IsOneWay = true)]
        void svcSetNodeStatus(string buddyname);

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

    }

    public interface INetP2PBootStrapChannel : INetP2PBootStrapService, IClientChannel
    {
    }
}
