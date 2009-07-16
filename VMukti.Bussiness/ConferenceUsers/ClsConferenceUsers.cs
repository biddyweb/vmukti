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
using System.Data;
using System.Collections.Generic;
using VMukti.Common;
using VMukti.DataAccess;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;


namespace VMukti.Business
{
    public class ClsConferenceUsers : ClsBaseObject
    {
        #region Fields
        private int _ID = VMukti.Common.ClsConstants.NullInt;
        private int _ConfID = VMukti.Common.ClsConstants.NullInt;
        private int _UserID = VMukti.Common.ClsConstants.NullInt;
        private int _RoleID = VMukti.Common.ClsConstants.NullInt;
        private bool _Started = VMukti.Common.ClsConstants.NullBoolean;
       
        #endregion 

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int ConfID
        {
            get { return _ConfID; }
            set { _ConfID = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }

        public bool Started
        {
            get
            {
                return _Started;
            }
            set
            {
                _Started = value;
            }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                ConfID = GetInt(row, "ConfID");
                UserID = GetInt(row, "UserID");
                RoleID = GetInt(row, "RoleID");
                Started = GetBool(row, "Started");
                return base.MapData(row);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsConferenceUsers.cs");
                return false;
            }

        }        
        
        public void UpdateStarted(int ConfID,int UserID,bool Started)
        {
            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update ConferenceUsers set Started='" + Started + "' where ConfID='" + ConfID + "' and UserID='" + UserID + "'");
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update ConferenceUsers set Started='" + Started + "' where ConfID='" + ConfID + "' and UserID='" + UserID + "'");
            }
            else
            {
                new ClsConferenceUsersDataService().UpdateStarted(ConfID,UserID, Started);
                }
            }
        }

    #endregion
    }
}
