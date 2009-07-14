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
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace VMukti.DataAccess
{
    public class ClsConferenceUsersDataService : ClsDataServiceBase
    {
        public ClsConferenceUsersDataService() : base() { }

        public ClsConferenceUsersDataService(IDbTransaction txn) : base(txn) { }     

        public DataSet GetConferenceIDs()
        {
            return ExecuteDataSet("Select * from ConferenceUsers where UserID= " + VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString(), CommandType.Text, null);
        }

      

        public DataSet GetConfInfo(int ConfID)
        {
            return ExecuteDataSet("Select * from ConferenceUsers where ConfID= " + ConfID.ToString(), CommandType.Text, null);
        }


        public void UpdateStarted(int ConfID,int UserID, bool Started)
        {
            ExecuteDataSet("update ConferenceUsers set Started='" + Started + "' where ConfID='" + ConfID + "' and UserID='" + UserID + "'", CommandType.Text, null);
        }

        public DataSet GetConfParticipants(int ConfID)
        {
            return ExecuteDataSet("Select * from ConferenceUsers where ConfID=" + ConfID.ToString() + " and RoleID=4", CommandType.Text, null);
        }
    }
}
