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
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using VMukti.Business.WCFServices.SuperNodeServices.DataContract;

namespace VMukti.Business.WCFServices.BootStrapServices.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BootStrapDelegates : IHTTPBootStrapService
    {
        public delegate clsBootStrapInfo DelHttpBSJoin(string uName, clsPeerInfo objPeerInformation);
        public delegate clsSuperNodeDataContract DelsvcHttpBsGetSuperNodeIP(string uName, string IP, bool blSuperNode);
        public delegate List<string> DelsvcHttpGetSuperNodeBuddyList(string uName);
        public delegate string DelsvcHttpAddBuddy(string uName, string BuddyName);
        public delegate void DelsvcRemoveAddBuddy(string uName, List<string> BuddyName);
		public delegate void DelsvcHttpBSUnJoin(string uName, string IP, bool IsSuperNode);
		public delegate void DelsvcHttpBSAuthorizedUser(string uName, string IP, bool blSuperNode);

        public delegate string DelsvcGetNodeNameByIP(string NodeIP);
        public delegate string DelsvcGetOfflineNodeName(string uName, string IP);
        
        public delegate List<string> DelsvcGetAllBuddies();
        public delegate void DelsvcUpdateVMuktiVersion(bool blIsMeetingPlace, bool blIsCallCenter);

        public event DelHttpBSJoin EntHttpBSJoin;
        public event DelsvcHttpBsGetSuperNodeIP EntHttpBsGetSuperNodeIP;
        public event DelsvcHttpGetSuperNodeBuddyList EntHttpGetSuperNodeBuddyList;
        public event DelsvcHttpAddBuddy EntHttpAddBuddy;
		public event DelsvcRemoveAddBuddy EntHttpRemoveBuddy;
        public event DelsvcHttpBSUnJoin EntHttpBSUnJoin;
		public event DelsvcHttpBSAuthorizedUser EntHttpBSAuthorizedUser;

        public event DelsvcGetNodeNameByIP EntsvcGetNodeNameByIP;
        public event DelsvcGetOfflineNodeName EntHTTPGetOfflineNodeName;
        public event DelsvcGetAllBuddies EntsvcGetAllBuddies;
        public event DelsvcUpdateVMuktiVersion EntsvcUpdateVMuktiVersion;

        #region IHTTPBootStrapService Members       

        public clsBootStrapInfo svcHttpBSJoin(string uName, clsPeerInfo objPeerInformation)
        {
            if (EntHttpBSJoin != null)
            {
                return EntHttpBSJoin(uName, objPeerInformation); 
            }
            else
            { 
                return null; 
            }
        }

        public clsSuperNodeDataContract svcHttpBsGetSuperNodeIP(string uName, string IP, bool blSuperNode)
        {
            if (EntHttpBsGetSuperNodeIP != null)
            {
                return EntHttpBsGetSuperNodeIP(uName, IP, blSuperNode);
            }
            else
            {
                return null;
            }
        }

        public List<string> svcHttpGetSuperNodeBuddyList(string uName)
        {
            if (EntHttpGetSuperNodeBuddyList != null)
            {
                return EntHttpGetSuperNodeBuddyList(uName);
            }
            else
            {
                return null;
            }
        }

        public string svcHttpAddBuddy(string uName, string BuddyName)
        {
            if (EntHttpAddBuddy != null)
            {
                return EntHttpAddBuddy(uName, BuddyName);
            }
            else
            {
                return null;
            }
        }

		public void svcHttpBSUnJoin(string uName, string IP, bool IsSuperNode)
        {
            if (EntHttpBSUnJoin != null)
            {
                EntHttpBSUnJoin(uName,IP,IsSuperNode);
            }
        }

		public void svcHttpBSAuthorizedUser(string uName, string IP, bool blSuperNode)
		{
			if (EntHttpBSAuthorizedUser != null)
			{
				EntHttpBSAuthorizedUser(uName,IP,blSuperNode);
			}
		}

        public void svcHttpRemoveBuddy(string uName, List<string> BuddyName)
        {
            if (EntHttpRemoveBuddy != null)
            {
                EntHttpRemoveBuddy(uName, BuddyName);
            }
        }

        public string svcGetNodeNameByIP(string NodeIP)
        {
            if (EntsvcGetNodeNameByIP != null)
            {
                return EntsvcGetNodeNameByIP(NodeIP);
            }
            else
            {
                return null;
            }
        }

        public string svcGetOfflineNodeName(string uName, string IP)
        {
            if (EntHTTPGetOfflineNodeName != null)
            {
                return EntHTTPGetOfflineNodeName(uName, IP);
            }
            else
            {
                return string.Empty;
            }
        }

        public List<string> svcGetAllBuddies()
        {
            if (EntsvcGetAllBuddies != null)
            {
                return EntsvcGetAllBuddies();
            }
            else
            {
                return null;
            }
        }

        public void svcUpdateVMuktiVersion(bool blIsMeetingPlace, bool blIsCallCenter)
        {
            if (EntsvcUpdateVMuktiVersion != null)
            {
                EntsvcUpdateVMuktiVersion(blIsMeetingPlace, blIsCallCenter);
            }
        }
        
        #endregion

       
    }
}
