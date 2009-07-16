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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SuperNodeDelegates : IHttpSuperNodeService
    {
        public delegate void DelHttpsvcJoin(string uName, string bindingType);
        public delegate List<string> DelHttpGetBuddies(string uName);
        public delegate void DelHttpAddBuddies(string username, string buddyname, string buddystatus);
        public delegate void DelHttpRemoveBuddies(string username, List<string> buddyname);
        public delegate void DelHTTPAddNodeBuddies(string username, List<string> NodeBuddies);

        public delegate void DelHttpsvcUnjoin(string uName);
        public delegate List<string> DelHttpsvcGetBuddyStatus(List<string> BuddyList);
        public delegate bool DelIsSuperNodeAvailable();
        public delegate List<clsMessage> DelsvcGetSpecialMsgs(string uName);
        public delegate void DelsvcSetSpecialMsgs(string from, string to, string msg, clsModuleInfo objModInfo);
        //public delegate void DelsvcPageSetSpecialMsgs(string from, string to, string msg, List<clsModuleInfo> objModInfo);
        public delegate void DelsvcSetSpecialMsgs4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo);
        public delegate void DelsvcSetSpecialMsgsBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo,clsPageInfo objPageInfo);
        public delegate void DelsvcPageSetSpecialMsgs(clsPageInfo objPageInfo);
        public delegate void DelsvcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objPageInfo);

        public delegate string[] DelsvcStartAService(VMuktiAPI.PeerType PeerType, string ModuleName);
        public delegate string[] DelIsAutherizedUser(clsBootStrapInfo objBootStrapInfo, string strUserName, string strPassword);
        public delegate string DelIsAutherized(string strUserName);
        public delegate string DelsvcAddSIPUser();
        public delegate void DelsvcRemoveSIPUser(string strSIPNumber);
        public delegate string DelsvcGetConferenceNumber();
        public delegate void DelsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo);
        public delegate void DelsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo);
        public delegate void DelsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> objModInfo);

        public delegate void DelsvcGetNodeNameByIP(string NodeName, string NodeIP);

        // For Asynchronous Communication of HTTP

        public delegate IAsyncResult DelBeginsvcGetSpecialMsgs(string uName, System.AsyncCallback callback, object asyncState);
        public delegate List<clsMessage> DelEndsvcGetSpecialMsgs(System.IAsyncResult result);
        public delegate IAsyncResult DelBeginGetBuddies(string uName, System.AsyncCallback callback, object asyncState);
        public delegate List<string> DelEndGetBuddies(System.IAsyncResult result);


        public delegate void DelsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcJoinConf(string from, string to, int confid, string ipaddress);
        public delegate void DelsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress);
        public delegate void DelsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress);
        public delegate void DelsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress);

        public delegate void DelsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid,string ipaddress);



        public event DelBeginsvcGetSpecialMsgs EntDelBeginsvcGetSpecialMsgs;
        public event DelEndsvcGetSpecialMsgs EntDelEndsvcGetSpecialMsgs;
        public event DelBeginGetBuddies EntDelBeginGetBuddies;
        public event DelEndGetBuddies EntDelEndGetBuddies;

        // End

        public event DelHttpsvcJoin EntHttpsvcjoin;
        public event DelHttpGetBuddies EntHttpGetBuddies;
        public event DelHttpAddBuddies EntHttpAddBuddies;
        public event DelHttpRemoveBuddies EntHttpRemoveBuddies;
        public event DelHTTPAddNodeBuddies EntHTTPAddNodeBuddies;

        public event DelHttpsvcUnjoin EntHttpsvcUnjoin;
        public event DelHttpsvcGetBuddyStatus EntHttpsvcGetBuddyStatus;
        public event DelIsSuperNodeAvailable EntIsSuperNodeAvailable;
        public event DelsvcGetSpecialMsgs EntsvcGetSpecialMsgs;
        public event DelsvcSetSpecialMsgs EntsvcSetSpecialMsg;

        public event DelsvcPageSetSpecialMsgs EntsvcPageSetSpecialMsg;
        public event DelsvcPageBuddyRetSetSpecialMsg EntsvcPageBuddyRetSetSpecialMsg;
        public event DelsvcSetSpecialMsgs4MultipleBuddies EntsvcSetSpecialMsg4MultipleBuddies;
        public event DelsvcSetSpecialMsgsBuddiesClick EntsvcSetSpecialMsgBuddiesClick;
        public event DelsvcStartAService EntsvcStartAService;
        public event DelIsAutherizedUser EntIsAutherizedUser;
        public event DelIsAutherized EntIsAutherized;
        public event DelsvcAddSIPUser EntsvcAddSIPUser;
        public event DelsvcRemoveSIPUser EntsvcRemoveSIPUser;
        public event DelsvcGetConferenceNumber EntsvcGetConferenceNumber;
        public event DelsvcSetRemoveDraggedBuddy EntsvcSetRemoveDraggedBuddy;
        public event DelsvcAddDraggedBuddy EntsvcAddDraggedBuddy;
        public event DelsvcAddPageDraggedBuddy EntsvcAddPageDraggedBuddy;

        public event DelsvcGetNodeNameByIP EntsvcGetNodeNameByIP;

        public event DelsvcEnterConf EntsvcEnterConf;
        public event DelsvcJoinConf EntsvcJoinConf;
        public event DelsvcSendConfInfo EntsvcSendConfInfo;
        public event DelsvcAddConfBuddy EntsvcAddConfBuddy;
        public event DelsvcRemoveConf EntsvcRemoveConf;
        public event DelsvcUnJoinConf EntsvcUnJoinConf;

        public event DelsvcPodNavigation EntsvcPodNavigation;


        #region IHttpSuperNodeService Members

        public void svcJoin(string uName, string bindingType)
        {
            if (EntHttpsvcjoin != null)
            {
                EntHttpsvcjoin(uName, bindingType);
            }
        }

        public List<string> GetBuddies(string uName)
        {
            if (EntHttpGetBuddies != null)
            {
                return EntHttpGetBuddies(uName);
            }
            else
            {
                return null;
            }
        }

        public void AddBuddies(string username, string buddyname, string buddystatus)
        {
            if (EntHttpAddBuddies != null)
            { EntHttpAddBuddies(username, buddyname, buddystatus); }
        }

        public void svcUnjoin(string uName)
        {
            if (EntHttpsvcUnjoin != null)
            {
                EntHttpsvcUnjoin(uName);
            }
        }

        public List<string> GetBuddyStatus(List<string> Buddyname)
        {
            if (EntHttpsvcGetBuddyStatus != null)
            {
                return EntHttpsvcGetBuddyStatus(Buddyname);
            }
            else
            {
                return null;
            }
        }

        public bool IsSuperNodeAvailable()
        {
            if (EntIsSuperNodeAvailable != null)
            {
                return EntIsSuperNodeAvailable();
            }
            else
            {
                return false;
            }
        }

        public List<clsMessage> svcGetSpecialMsgs(string uName)
        {
            if (EntsvcGetSpecialMsgs != null)
            {
                return EntsvcGetSpecialMsgs(uName);
            }
            else
            {
                return null;
            }
        }

        public void svcSetSpecialMsgs(string from, string to, string msg, clsModuleInfo objModInfo)
        {
            if (EntsvcSetSpecialMsg != null)
            {
                EntsvcSetSpecialMsg(from, to, msg, objModInfo);
            }
        }

        public void svcSetSpecialMsgs(string from, List<string> to, string msg, clsModuleInfo objModInfo)
        {
            if (EntsvcSetSpecialMsg4MultipleBuddies != null)
            {
                EntsvcSetSpecialMsg4MultipleBuddies(from, to, msg, objModInfo);
            }
        }

        public void svcSetSpecialMsgs(string from, List<string> to, string msg, clsModuleInfo objModInfo,clsPageInfo objPageInfo)
        {
            if (EntsvcSetSpecialMsgBuddiesClick != null)
            {
                EntsvcSetSpecialMsgBuddiesClick(from, to, msg, objModInfo, objPageInfo);
            }
        }

        public string[] svcStartAService(VMuktiAPI.PeerType PeerType, string ModuleName)
        {
            if (EntsvcStartAService != null)
            {
                return EntsvcStartAService(PeerType, ModuleName);
            }
            else
            {
                return null;
            }
        }

        public string[] IsAuthorizedUser(clsBootStrapInfo objBootStrapInfo, string strUserName, string strPassword)
        {
            if (EntIsAutherizedUser != null)
            {
                return EntIsAutherizedUser(objBootStrapInfo, strUserName, strPassword);
            }
            else
            {
                return null;
            }
        }

        public string IsAuthorized(string strUserName)
        {
            if (EntIsAutherized != null)
            {
                return EntIsAutherized(strUserName);
            }
            else
            {
                return null;
            }
        }

        public string svcAddSIPUser()
        {
            if (EntsvcAddSIPUser != null)
            {
                return EntsvcAddSIPUser();
            }
            else
            {
                return null;
            }
        }

        public void svcRemoveSIPUser(string strSIPNumber)
        {
            if (EntsvcRemoveSIPUser != null)
            {
                EntsvcRemoveSIPUser(strSIPNumber);
            }
        }

        public string svcGetConferenceNumber()
        {
            if (EntsvcGetConferenceNumber != null)
            {
                return EntsvcGetConferenceNumber();
            }
            else
            {
                return null;
            }
        }

        public void svcSetSpecialMsgs(clsPageInfo objPageInfo)
        {
            if (EntsvcPageSetSpecialMsg != null)
            {
                EntsvcPageSetSpecialMsg(objPageInfo);
            }
        }

        public void svcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objPageInfo)
        {
            if (EntsvcPageBuddyRetSetSpecialMsg != null)
            {
                EntsvcPageBuddyRetSetSpecialMsg(objPageInfo);
            }
        }

        public void svcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo)
        {
            if (EntsvcAddDraggedBuddy != null)
            {
                EntsvcAddDraggedBuddy(from, to, msg, objModInfo);
            }
        }

        public void svcAddDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModInfo)
        {
            if (EntsvcAddPageDraggedBuddy != null)
            {
                EntsvcAddPageDraggedBuddy(from, to, msg, lstModInfo);
            }
        }

        public void svcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo)
        {
            if (EntsvcSetRemoveDraggedBuddy != null)
            {
                EntsvcSetRemoveDraggedBuddy(from, to, msg, objModInfo);
            }
        }

        public void RemoveBuddies(string username, List<string> buddyname)
        {
            if (EntHttpRemoveBuddies != null)
            {
                EntHttpRemoveBuddies(username, buddyname);
            }
        }

        public void AddNodeBuddies(string username, List<string> NodeBuddies)
        {
            if (EntHTTPAddNodeBuddies != null)
            {
                EntHTTPAddNodeBuddies(username, NodeBuddies);
            }
        }

        public void svcGetNodeNameByIP(string NodeName, string NodeIP)
        {
            if (EntsvcGetNodeNameByIP != null)
            {
                EntsvcGetNodeNameByIP(NodeName, NodeIP);
            }

        }

        public IAsyncResult BeginsvcGetSpecialMsgs(string uName, System.AsyncCallback callback, object asyncState)
        {
            if (EntDelBeginsvcGetSpecialMsgs != null)
            {
                return EntDelBeginsvcGetSpecialMsgs(uName, callback, asyncState);
            }
            else
            {
                return null;
            }
        }

        public List<clsMessage> EndsvcGetSpecialMsgs(System.IAsyncResult result)
        {
            if (EntDelEndsvcGetSpecialMsgs != null)
            {
                return EntDelEndsvcGetSpecialMsgs(result);
            }
            else
            {
                return null;
            }
        }

        public IAsyncResult BeginGetBuddies(string uName, System.AsyncCallback callback, object asyncState)
        {
            if (EntDelBeginGetBuddies != null)
            {
                return EntDelBeginGetBuddies(uName, callback, asyncState);
            }
            else
            {
                return null;
            }
        }

        public List<string> EndGetBuddies(System.IAsyncResult result)
        {
            if (EntDelEndGetBuddies != null)
            {
                return EntDelEndGetBuddies(result);
            }
            else
            {
                return null;
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

        public void svcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid, string ipaddress)
        {
            if (EntsvcPodNavigation != null)
            {
                EntsvcPodNavigation(from, lstBuddies,pageid,tabid, podid,ipaddress);
            }
        }

        #endregion
    }
}
