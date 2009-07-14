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

namespace VMukti.Business.WCFServices.SuperNodeServices.NetP2P
{
    public class NetP2PSuperNodeDelegates:INetP2PSuperNodeService
    {
        public delegate void DelsvcJoin(string uname);
        public delegate void DelsvcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo,string IPAddress);
        //public delegate void DelsvcPageSetSpecialMsg(string from, string to, string msg, List<clsModuleInfo> lstModInfo, string IPAddress);

        public delegate void DelsvcPageSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress);
        public delegate void DelsvcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress);
        public delegate void DelsvcSetSpecialMsg4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress);
        public delegate void DelsvcSetSpecialMsgBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo);
        public delegate void DelsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress);
        public delegate void DelsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModInfo, string IPAddress);
        public delegate void DelsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress);
        public delegate void DelsvcUnJoin(string uname);

        public delegate void DelsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcJoinConf(string from, string to, int confid, string ipaddress);
        public delegate void DelsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress);
        public delegate void DelsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress);

        public delegate void DelsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid, string ipaddress);




        public event DelsvcJoin EntsvcJoin;
        public event DelsvcSetSpecialMsg EntsvcSetSpecialMsg;

        public event DelsvcPageSetSpecialMsg EntsvcPageSetSpecialMsg;
        public event DelsvcPageBuddyRetSetSpecialMsg EntsvcPageBuddyRetSetSpecialMsg;
        public event DelsvcSetSpecialMsg4MultipleBuddies EntsvcSetSpecialMsg4MultipleBuddies;
        public event DelsvcSetSpecialMsgBuddiesClick EntsvcSetSpecialMsgBuddiesClick;
        public event DelsvcAddDraggedBuddy EntsvcAddDraggedBuddy;
        public event DelsvcAddPageDraggedBuddy EntsvcAddPageDraggedBuddy;
        public event DelsvcSetRemoveDraggedBuddy EntsvcSetRemoveDraggedBuddy;
        public event DelsvcUnJoin EntsvcUnJoin;

        public event DelsvcEnterConf EntsvcEnterConf;
        public event DelsvcJoinConf EntsvcJoinConf;
        public event DelsvcSendConfInfo EntsvcSendConfInfo;
        public event DelsvcAddConfBuddy EntsvcAddConfBuddy;
        public event DelsvcRemoveConf EntsvcRemoveConf;
        public event DelsvcUnJoinConf EntsvcUnJoinConf;

        public event DelsvcPodNavigation EntsvcPodNavigation;


        public void svcJoin(string uname)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uname);
            }
        }

        public void svcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo,string IPAddress)
        {
            if (EntsvcSetSpecialMsg != null)
            {
                EntsvcSetSpecialMsg(from,to, msg, objModInfo,IPAddress);
            }
        }

        public void svcSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress)
        {
            if (EntsvcPageSetSpecialMsg != null)
            {
                EntsvcPageSetSpecialMsg(objPageInfo, IPAddress);
            }
        }

        public void svcSetSpecialMsg(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcSetSpecialMsg4MultipleBuddies != null)
            {
                EntsvcSetSpecialMsg4MultipleBuddies(from, to, msg, objModInfo, IPAddress);
            }
        }

        public void svcSetSpecialMsg(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo)
        {
            if (EntsvcSetSpecialMsgBuddiesClick != null)
            {
                EntsvcSetSpecialMsgBuddiesClick(from, to, msg, objModInfo, IPAddress, objPageInfo);
            }
        }

        public void svcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress)
        {
            if (EntsvcPageBuddyRetSetSpecialMsg != null)
            {
                EntsvcPageBuddyRetSetSpecialMsg(objBuddyRetPageInfo, IPAddress);
            }
        }

        public void svcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcAddDraggedBuddy != null)
            {
                EntsvcAddDraggedBuddy(from, to, msg, objModInfo, IPAddress);
            }
        }

        public void svcAddDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModInfo, string IPAddress)
        {
            if (EntsvcAddPageDraggedBuddy!= null)
            {
                EntsvcAddPageDraggedBuddy(from, to, msg, lstModInfo, IPAddress);
            }
        }

        public void svcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            if (EntsvcSetRemoveDraggedBuddy != null)
            {
                EntsvcSetRemoveDraggedBuddy(from, to, msg, objModInfo, IPAddress);
            }
        }

        public void svcUnJoin(string uname)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uname);
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

        public void svcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            if (EntsvcUnJoinConf != null)
            {
                EntsvcUnJoinConf(from, lstBuddies, confid, ipaddress);
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


        public void svcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            if (EntsvcEnterConf != null)
            {
                EntsvcEnterConf(from, lstBuddies, confid, ipaddress);
            }
        }

        public void svcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid, string ipaddress)
        {
            if (EntsvcPodNavigation != null)
            {
                EntsvcPodNavigation(from, lstBuddies, pageid, tabid, podid,ipaddress);
            }
        }
    }
}
