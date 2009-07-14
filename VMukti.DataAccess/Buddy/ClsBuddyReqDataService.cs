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
    public class ClsBuddyReqDataService : ClsDataServiceBase
    {        
       
        public ClsBuddyReqDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsBuddyReqDataService(IDbTransaction txn) : base(txn) { }

        public DataSet BuddyReq_GetAll()
        {
            try
            {

                return ExecuteDataSet("Select * vMyBuddy;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyReq_GetAll()", "ClsBuddyReqDataService.cs");               
                return null;
            }
        }

        public DataSet BuddyReq_GetByID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGBuddyRequest", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.BigInt, ID),
                    CreateParameter("@pUserID", SqlDbType.BigInt, -1));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyReq_GetByID()", "ClsBuddyReqDataService.cs");              
                return null;
            }
        }

        public DataSet BuddyReq_GetByUserID(int UserID)
        {
            try
            {
                return ExecuteDataSet("spGBuddyRequest", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.BigInt, -1),
                    CreateParameter("@pUserID", SqlDbType.BigInt, UserID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyReq_GetByUserID()", "ClsBuddyReqDataService.cs"); 
                return null;
            }
        }

        public void BuddyReq_Save(ref int ID, int UserID, string DisplayName, int ReqUserID, ref int Result)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEBuddyRequest",
                    CreateParameter("@pID", SqlDbType.Int, ID, ParameterDirection.InputOutput),
                    CreateParameter("@pUserID", SqlDbType.Int, UserID),
                    CreateParameter("@pDisplayName", SqlDbType.VarChar, DisplayName, 50),
                    CreateParameter("@pReqUserID", SqlDbType.Int, ReqUserID),
                    CreateParameter("@pResult", SqlDbType.Int, Result, ParameterDirection.InputOutput));

                ID = int.Parse(cmd.Parameters["@pResult"].Value.ToString());
                Result = int.Parse(cmd.Parameters["@pResult"].Value.ToString());

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyReq_Save", "ClsBuddyReqDataService.cs");
            }
        }

        public void BuddyReq_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDBuddyRequest", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyReq_Delete", "ClsBuddyReqDataService.cs");             
            }
        }

    }
}
