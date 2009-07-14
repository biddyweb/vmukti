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
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System;
using VMuktiAPI;

namespace VMukti.DataAccess
{
    public class ClsBuddyDataService : ClsDataServiceBase
    {        
       

        public ClsBuddyDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsBuddyDataService(IDbTransaction txn) : base(txn) { }

        public DataSet Buddy_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * vMyBuddy;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Buddy_GetAll()", "clsBuddyDataService.cs");
                return null;
            }
        }

        public DataSet Buddy_GetByID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGMyBuddy", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Buddy_GetByID()", "clsBuddyDataService.cs");                
                return null;
            }
        }

        public DataSet Buddy_GetByUserID(int UserID)
        {
            try
            {
                return ExecuteDataSet("spGMyBuddyByUserID", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.Int, UserID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Buddy_GetByUserID()", "clsBuddyDataService.cs");               
                return null;
            }
        }

        public void Buddy_Save(ref int ID, int MyUserID, int UserID)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEMyBuddy",
                    CreateParameter("@pID", SqlDbType.Int, ID, ParameterDirection.InputOutput),
                    CreateParameter("@pMyUserID", SqlDbType.Int, MyUserID),
                    CreateParameter("@pUserID", SqlDbType.Int, UserID));

                ID = int.Parse(cmd.Parameters["@pID"].Value.ToString());

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Buddy_Save()", "clsBuddyDataService.cs");             
            }
        }

        public void Buddy_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDMyBuddy", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Buddy_Delete()", "clsBuddyDataService.cs");                
            }
        }

    }
}
