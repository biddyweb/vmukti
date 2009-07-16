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
using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    public class NetP2PBootStrapDelegates : INetP2PBootStrapService
    {
        public delegate void DelsvcNetP2PServiceJoin(string sSuperNodeIP);
        public delegate void DelsvcNetP2PAddUser(string uName);
        public delegate void DelsvcNetP2PRemoveUser(string uName);
        public delegate void DelsvcNetP2PGetBuddyInfo(string uName);
        public delegate void DelsvcNetP2PReturnBuddyInfo(string uName, List<string> lstMyBuddyList);
        public delegate void DelsvcNetP2PAddBuddies(string username, string BuddyName, string BuddyStatus);

        public delegate void DelsvcNetP2PRemoveBuddies(string username, List<string> BuddyName);

        public delegate void DelsvcNetP2PGetSuperNodeBuddyStatus(string uName);
        public delegate void DelsvcNetP2PReturnSuperNodeBuddyStatus(string uName, List<string> lstSNBuddyList);
        public delegate void DelsvcNetP2PUnJoin(string uName);
        public delegate void DelsvcNetP2PSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress);
        //public delegate void DelsvcNetP2PPageSetSpecialMsg(string from, string to, string msg, List<clsModuleInfo> lstModuleInfo, string IPAddress);

        public delegate void DelsvcNetP2PPageSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress);
        public delegate void DelsvcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress);
        public delegate void DelsvcNetP2PSetSpecialMsg4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress);
        public delegate void DelsvcNetP2PSetSpecialMsgBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo);
        public delegate void DelsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress);
        public delegate void DelsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress);
        public delegate void DelsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModuleInfo, string IPAddress);

        public delegate void DelsvcBuzzSuperNode(string uname);
        public delegate void DelsvcSetNodeStatus(string buddyname);


        public delegate void DelsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcJoinConf(string from, string to, int confid, string ipaddress);
        public delegate void DelsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress);
        public delegate void DelsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress);

        public delegate void DelsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid,string ipaddress);




        public event DelsvcNetP2PServiceJoin EntsvcNetP2PServiceJoin;
        public event DelsvcNetP2PAddUser EntsvcNetP2PAddUser;
        public event DelsvcNetP2PRemoveUser EntsvcNetP2PRemoveUser;
        public event DelsvcNetP2PGetBuddyInfo EntsvcNetP2PGetBuddyInfo;
        public event DelsvcNetP2PReturnBuddyInfo EntsvcNetP2PReturnBuddyInfo;
        public event DelsvcNetP2PAddBuddies EntsvcNetP2PAddBuddies;

        public event DelsvcNetP2PRemoveBuddies EntsvcNetP2PRemoveBuddies;

        public event DelsvcNetP2PGetSuperNodeBuddyStatus EntsvcNetP2PGetSuperNodeBuddyStatus;
        public event DelsvcNetP2PReturnSuperNodeBuddyStatus EntsvcNetP2PReturnSuperNodeBuddyStatus;
        public event DelsvcNetP2PUnJoin EntsvcNetP2PUnJoin;
        public event DelsvcNetP2PSetSpecialMsg EntsvcNetP2PSetSpecialMsg;
        public event DelsvcNetP2PSetSpecialMsg4MultipleBuddies EntsvcNetP2PSetSpecialMsg4MultipleBuddies;
        public event DelsvcNetP2PSetSpecialMsgBuddiesClick EntsvcNetP2PSetSpecialMsgBuddiesClick;
        public event DelsvcNetP2PPageSetSpecialMsg EntsvcNetP2PPageSetSpecialMsg;
        public event DelsvcPageBuddyRetSetSpecialMsg EntsvcPageBuddyRetSetSpecialMsg;

        public event DelsvcSetRemoveDraggedBuddy EntsvcSetRemoveDraggedBuddy;
        public event DelsvcAddDraggedBuddy EntsvcAddDraggedBuddy;
        public event DelsvcAddPageDraggedBuddy EntsvcAddPageDraggedBuddy;

        public event DelsvcBuzzSuperNode EntsvcBuzzSuperNode;
        public event DelsvcSetNodeStatus EntsvcSetNodeStatus;


        public event DelsvcEnterConf EntsvcEnterConf;
        public event DelsvcJoinConf EntsvcJoinConf;
        public event DelsvcSendConfInfo EntsvcSendConfInfo;
        public event DelsvcAddConfBuddy EntsvcAddConfBuddy;
        public event DelsvcRemoveConf EntsvcRemoveConf;
        public event DelsvcUnJoinConf EntsvcUnJoinConf;

        public event DelsvcPodNavigation EntsvcPodNavigation;



        public void svcNetP2PServiceJoin(string sSuperNodeIP)
        {
            if (EntsvcNetP2PServiceJoin != null)
            {
                EntsvcNetP2PServiceJoin(sSuperNodeIP);
            }
        }

        public void svcNetP2PAddUser(string uName)
        {
            if (EntsvcNetP2PAddUser != null)
            {
                EntsvcNetP2PAddUser(uName);
            }
        }

        public void svcNetP2PRemoveUser(string uName)
        {
            if (EntsvcNetP2PRemoveUser != null)
            {
                EntsvcNetP2PRemoveUser(uName);
            }
        }

        public void svcNetP2PGetBuddyInfo(string uName)
        {
            if (EntsvcNetP2PGetBuddyInfo != null)
            {
                EntsvcNetP2PGetBuddyInfo(uName);
            }
        }

        public void svcNetP2PReturnBuddyInfo(string uName, List<string> lstMyBuddyList)
        {
            if (EntsvcNetP2PReturnBuddyInfo != null)
            {
                EntsvcNetP2PReturnBuddyInfo(uName, lstMyBuddyList);
            }
        }

        public void svcNetP2PGetSuperNodeBuddyStatus(string uName)
        {
            if (EntsvcNetP2PGetSuperNodeBuddyStatus != null)
            {
                EntsvcNetP2PGetSuperNodeBuddyStatus(uName);
            }
        }

        public void svcNetP2PReturnSuperNodeBuddyStatus(string uName, List<string> lstSNBuddyList)
        {
            if (EntsvcNetP2PReturnSuperNodeBuddyStatus != null)
            {
                EntsvcNetP2PReturnSuperNodeBuddyStatus(uName, lstSNBuddyList);
            }
        }

        public void svcNetP2PUnJoin(string uName)
        {
            if (EntsvcNetP2PUnJoin != null)
            {
                EntsvcNetP2PUnJoin(uName);
            }
        }

        public void svcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcNetP2PSetSpecialMsg != null)
            {
                EntsvcNetP2PSetSpecialMsg(from, to, msg, objModInfo, IPAddress);
            }
        }



        public void svcSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress)
        {
            if (EntsvcNetP2PPageSetSpecialMsg != null)
            {
                EntsvcNetP2PPageSetSpecialMsg(objPageInfo, IPAddress);
            }
        }

        public void svcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress)
        {
            if (EntsvcPageBuddyRetSetSpecialMsg != null)
            {
                EntsvcPageBuddyRetSetSpecialMsg(objBuddyRetPageInfo, IPAddress);
            }
        }

        public void svcSetSpecialMsg(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcNetP2PSetSpecialMsg4MultipleBuddies != null)
            {
                EntsvcNetP2PSetSpecialMsg4MultipleBuddies(from, to, msg, objModInfo, IPAddress);
            }
        }

        public void svcSetSpecialMsg(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo)
        {
            if (EntsvcNetP2PSetSpecialMsgBuddiesClick != null)
            {
                EntsvcNetP2PSetSpecialMsgBuddiesClick(from, to, msg, objModInfo, IPAddress, objPageInfo);
            }
        }

        public void svcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcSetRemoveDraggedBuddy != null)
            {
                EntsvcSetRemoveDraggedBuddy(from, to, msg, objModInfo, IPAddress);
            }
        }

        public void svcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcAddDraggedBuddy != null)
            {
                EntsvcAddDraggedBuddy(from, to, msg, objModInfo, IPAddress);
            }
        }

        public void svcAddDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModuleInfo, string IPAddress)
        {
            if (EntsvcAddPageDraggedBuddy != null)
            {
                EntsvcAddPageDraggedBuddy(from, to, msg, lstModuleInfo, IPAddress);
            }
        }

        public void svcAddBuddies(string username, string BuddyName, string BuddyStatus)
        {
            if (EntsvcNetP2PAddBuddies != null)
            {
                EntsvcNetP2PAddBuddies(username, BuddyName, BuddyStatus);
            }
        }


        public void svcRemoveBuddies(string username, List<string> BuddyName)
        {
            if (EntsvcNetP2PRemoveBuddies != null)
            {
                EntsvcNetP2PRemoveBuddies(username, BuddyName);
            }

        }

        public void svcBuzzSuperNode(string uname)
        {
            if (EntsvcBuzzSuperNode != null)
            {
                EntsvcBuzzSuperNode(uname);
            }
        }

        public void svcSetNodeStatus(string buddyname)
        {
            if (EntsvcSetNodeStatus != null)
            {
                EntsvcSetNodeStatus(buddyname);
            }
        }


        public void svcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            if (EntsvcEnterConf != null)
            {
                EntsvcEnterConf(from, lstBuddies, confid, ipaddress);
            }
        }


        public void svcJoinConf(string from, string to, int confid, string ipaddress)
        {
            if (EntsvcJoinConf != null)
            {
                EntsvcJoinConf(from, to, confid, ipaddress);
            }
        }

        public void svcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress)
        {
            if (EntsvcSendConfInfo != null)
            {
                EntsvcSendConfInfo(from, to, confid, pageinfo, ipaddress);
            }
        }

        public void svcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            if (EntsvcAddConfBuddy != null)
            {
                EntsvcAddConfBuddy(from, lstBuddies, confid, ipaddress);
            }
        }


        public void svcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            if (EntsvcRemoveConf != null)
            {
                EntsvcRemoveConf(from, lstBuddies, confid, ipaddress);
            }
        }

        public void svcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            if (EntsvcUnJoinConf != null)
            {
                EntsvcUnJoinConf(from, lstBuddies, confid, ipaddress);
            }
        }

        public void svcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid,string ipaddress)
        {
            if (EntsvcPodNavigation != null)
            {
                EntsvcPodNavigation(from, lstBuddies, pageid, tabid, podid,ipaddress);
            }
        }


    }
}
