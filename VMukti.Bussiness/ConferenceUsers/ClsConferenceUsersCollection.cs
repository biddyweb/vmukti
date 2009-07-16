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
using VMuktiAPI;
using VMukti.DataAccess;

namespace VMukti.Business
{
    public class ClsConferenceUsersCollection : ClsBaseCollection<ClsConferenceUsers>
    {
        public static ClsConferenceUsersCollection GetConferenceIDs()
        {
            ClsConferenceUsersCollection obj = new ClsConferenceUsersCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * from ConferenceUsers where UserID= " + VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString()).dsInfo);
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "GetConferenceIDs()", "Vmukti.Business\\Conference\\ClsConferenceUserCollection");
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * from ConferenceUsers where UserID= " + VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString()).dsInfo);
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "GetConferenceIDs()", "Vmukti.Business\\Conference\\ClsConferenceUserCollection");
                }
            }
            else
            {
                obj.MapObjects(new ClsConferenceUsersDataService().GetConferenceIDs());
            }
            }
            return obj;
        }

        public static ClsConferenceUsersCollection GetConfInfo(int ConfID)
        {
            ClsConferenceUsersCollection obj = new ClsConferenceUsersCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * from ConferenceUsers where ConfID= " + ConfID.ToString()).dsInfo);
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "GetConfInfo()", "Vmukti.Business\\Conference\\ClsConferenceUserCollection");
                }
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * from ConferenceUsers where ConfID= " + ConfID.ToString()).dsInfo);
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "GetConfInfo()", "Vmukti.Business\\Conference\\ClsConferenceUserCollection");
                }
            }
            else
            {
                obj.MapObjects(new ClsConferenceUsersDataService().GetConfInfo(ConfID));
            }
            }
            return obj;
        }

        public static ClsConferenceUsersCollection GetConfParticipants(int ConfID)
        {
            ClsConferenceUsersCollection obj = new ClsConferenceUsersCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * from ConferenceUsers where ConfID=" + ConfID.ToString() + " and RoleID=4").dsInfo);
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "GetConfParticipants()", "Vmukti.Business\\Conference\\ClsConferenceUserCollection");
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * from ConferenceUsers where ConfID=" + ConfID.ToString() + " and RoleID=4").dsInfo);
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "GetConfParticipants()", "Vmukti.Business\\Conference\\ClsConferenceUserCollection");
                }
            }
            else
            {
                obj.MapObjects(new ClsConferenceUsersDataService().GetConfParticipants(ConfID));
                }
            }
            return obj;
        }
    }
}
