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
using VMuktiAPI;

namespace VMukti.DataAccess
{
    public class clsBuddyStatus : ClsDataServiceBase
    {
        public clsBuddyStatus() : base() { }

        public clsBuddyStatus(IDbTransaction txn) : base(txn) { }        
       

        public void AddBuddy(string uName, string buddyname, string status)
        {
            //  ExecuteDataSet("", CommandType.Text, null);    

            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEAddBuddy",
               CreateParameter("@pUName", SqlDbType.NVarChar, uName),
               CreateParameter("@pBuddyName", SqlDbType.NVarChar, buddyname),
               CreateParameter("@pStatus", SqlDbType.NVarChar, status));

        }

        public void UpdateBuddy(string uName, string status)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEUpdateBuddyStatus",
                   CreateParameter("@pUName", SqlDbType.NVarChar, uName),
                  CreateParameter("@pStatus", SqlDbType.NVarChar, status));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllBuddies()", "clsBuddyStatus.cs");               
            }
        }

        public void DeleteBuddy(string uName, string buddyname)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEDeleteBuddy",
                   CreateParameter("@pUName", SqlDbType.NVarChar, uName),
                  CreateParameter("@pBuddyName", SqlDbType.NVarChar, buddyname));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DeleteBuddy()", "clsBuddyStatus.cs");               
            }
        }

        public DataSet GetAllBuddies()
        {
            try
            {
                SqlCommand cmd;
                return ExecuteDataSet(out cmd, "spAEGetAllBuddyInfo");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllBuddies()", "clsBuddyStatus.cs");                             
                return null;
            }

        }

    }
}
