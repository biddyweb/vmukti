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
using System.Data.SqlClient;
using CallingList.Common;
using VMuktiAPI;
/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace CallingList.DataAccess
{

    public class ClsCallingListDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCallingListDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCallingListDataService(IDbTransaction txn) : base(txn) { }


        public DataSet CallingList_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vCallingList;", CommandType.Text, null);
            }
            catch (Exception ex)
            {

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CallingList_GetAll", "ClsCallingListDataService.cs");
                return null;
            }
            
        }


        public DataSet CallingList_GetByID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spGCallingList", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "CallingList_GetByID", "ClsCalllingListDataService.cs");
                return null;
            }
        }

        public Int64 CallingList_Save(ref Int64 ID, string ListName, bool IsActive, bool IsDNCList, Int64 CreatedBy)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAECallingList",
                    CreateParameter("@pID", SqlDbType.BigInt, ID, ParameterDirection.Input),
                    CreateParameter("@pListName", SqlDbType.NVarChar, ListName),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pIsDNCList", SqlDbType.Bit, IsDNCList),
                    CreateParameter("@pCreatedBy", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

                ID = Int64.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());


                cmd.Dispose();
                return ID;
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "CallingList_Save", "ClsCallingListDataService.cs");
                return 0;
            } 
        }

        
        public void CallingList_Delete(Int64 ID)
        {
            try
            {
                ExecuteNonQuery("spDCallingList", CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "CallingList_Delete", "ClsCallingListDataService.cs");
            } 
            
        }

    } //class

} //namespace