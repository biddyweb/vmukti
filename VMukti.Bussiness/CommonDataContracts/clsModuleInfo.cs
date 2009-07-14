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
using System.Runtime.Serialization;

namespace VMukti.Business.CommonDataContracts
{
    [DataContract]
    public class clsModuleInfo
    {
        [DataMember]
        public int intModuleId;

        [DataMember]
        public string strModuleName;

        [DataMember]
        public string[] strUri;

        [DataMember]
        public int[] ModPermissions;

        [DataMember]
        public string strPageid;

        [DataMember]
        public string strTabid;

        [DataMember]
        public string strPodid;

        [DataMember]
        public List<string> lstUsersDropped;

        [DataMember]
        public string strDropType;
    }

    [DataContract]
    public class clsPageInfo
    {
        [DataMember]
        public string strFrom;

        [DataMember]
        public string strTo;

        [DataMember]
        public string strMsg;

        [DataMember]
        public int intPageID;

        [DataMember]
        public string strPageTitle;

        [DataMember]
        public string[] straPageBuddies;
        
        [DataMember]
        public string strDropType;

        [DataMember]
        public int intOwnerID;

        [DataMember]
        public int intOwnerPageIndex;

        [DataMember]
        public clsTabInfo[] objaTabs;

        [DataMember]
        public int ConfID;
    }

    [DataContract]
    public class clsTabInfo
    {
        [DataMember]
        public int intTabID;

        [DataMember]
        public string strTabTitle;

        [DataMember]
        public string[] straTabBuddies;

        [DataMember]
        public int intOwnerTabIndex;
        
        [DataMember]
        public double dblC1Width;

        [DataMember]
        public double dblC2Width;

        [DataMember]
        public double dblC3Width;

        [DataMember]
        public double dblC4Height;

        [DataMember]
        public double dblC5Height;

        [DataMember]
        public clsPodInfo[] objaPods;
    }

    [DataContract]
    public class clsPodInfo
    {
        [DataMember]
        public int intModuleId;

        [DataMember]
        public int intOwnerPodIndex;

        [DataMember]
        public string strPodTitle;

        [DataMember]
        public string[] strUri;

        [DataMember]
        public int intColumnNumber;

        [DataMember]
        public string[] straPodBuddies;

        [DataMember]
        public int intPodID;

   }




    [DataContract]
    public class clsBuddyRetPageInfo
    {
        [DataMember]
        public string strFrom;

        [DataMember]
        public string strTo;

        [DataMember]
        public string strMsg;

        [DataMember]
        public int intPageID;

        [DataMember]
        public string strDropType;

        [DataMember]
        public int intOwnerID;

        [DataMember]
        public int intOwnerPageIndex;

        [DataMember]
        public clsBuddyRetTabInfo[] objaTabs;
    }

    [DataContract]
    public class clsBuddyRetTabInfo
    {
        [DataMember]
        public int intTabID;

        [DataMember]
        public int intOwnerTabIndex;

        [DataMember]
        public clsBuddyRetPodInfo[] objaPods;
    }

    [DataContract]
    public class clsBuddyRetPodInfo
    {
        [DataMember]
        public int intModuleId;

        [DataMember]
        public int intOwnerPodIndex;

        [DataMember]
        public int intColumnNumber;

        [DataMember]
        public string[] straPodBuddies;  
    }
}