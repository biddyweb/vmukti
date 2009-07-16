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

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    public class NetP2PBootStrapDashBoardDelegate : INetP2PBootStrapDashBoardServices
    {
        public delegate void delsvcJoin(string uname);
        public delegate void delsvcUnjoin(string uname);
        public delegate void DelsvcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal);

        public event delsvcJoin EntsvcJoin;
        public event delsvcUnjoin EntsvcUnJoin;
        public event DelsvcGetCallInfo EntsvcGetCallInfo;

        #region INetP2PBootStrapDashBoardServices Members

        void INetP2PBootStrapDashBoardServices.svcJoin(string uname)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uname);
            }
        }

        void INetP2PBootStrapDashBoardServices.svcUnJoin(string uname)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uname);
            }
        }

        void INetP2PBootStrapDashBoardServices.svcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal)
        {
            if (EntsvcGetCallInfo != null)
            {
                EntsvcGetCallInfo(LeadID, CalledDate, ModifiedDate, ModifiedBy, GeneratedBy, StartDate, StartTime, DurationInSecond, DispositionID, CampaignID, ConfID, IsDeleted, CallNote, isDNC, isGlobal);
            }
        }

        #endregion
    }
}
