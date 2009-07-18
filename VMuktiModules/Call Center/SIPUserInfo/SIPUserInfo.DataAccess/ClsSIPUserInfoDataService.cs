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
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;
namespace SIPUserInfo.DataAccess
{
    public class ClsSIPUserInfoDataService : ClsDataServiceBase
    {

        public ClsSIPUserInfoDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsSIPUserInfoDataService(IDbTransaction txn) : base(txn) { }


        public DataSet SIPUserInfo_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vSIPUsers;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_GetAll()", "ClsSIPUserInfoDataService.cs");
                return null;
            }
        }

        public DataSet SIPUserInfo_GetByID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGSIPUsers", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_GetByID()", "ClsSIPUserInfoDataService.cs");
                return null;
            }
        }

        public Int64 SIPUserInfo_Save(ref int ID, int SIPID, int SIPPass, int ActiveServerID, int CreatedBy)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAESIPUsers",
                    CreateParameter("@pID", SqlDbType.Int, ID),
                    CreateParameter("@pSIPID", SqlDbType.Int, SIPID),
                    CreateParameter("@pSIPPass", SqlDbType.Int, SIPPass),
                    CreateParameter("@pActiveServerID", SqlDbType.BigInt, ActiveServerID),
                    CreateParameter("@pByUserID", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

                Int64 RID = Int64.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());
                cmd.Dispose();
                return RID;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_Save()", "ClsSIPUserInfoDataService.cs");
                return 0;
            }
        }

        public void SIPUserInfo_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDSIPUsers", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_Delete()", "ClsSIPUserInfoDataService.cs");
             
            }
        }

    }
}
