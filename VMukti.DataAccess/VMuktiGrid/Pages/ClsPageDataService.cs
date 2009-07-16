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

namespace VMukti.DataAccess.VMuktiGrid
{
    public class ClsPageDataService : ClsDataServiceBase
    {
        public ClsPageDataService() : base() { }

        public ClsPageDataService(IDbTransaction txn) : base(txn) { }        
       
        public DataSet GetPages()
        {
            try
            {
                return ExecuteDataSet("select * from page where IsDeleted='false'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetPages()", "clsPageDataService.cs");
                return null;
            }
        }

        public int AddPage(int intPageId, string strPageTitle, string strPageDesc, bool blnIsPublic, int intUserId)
        {
            try
            {
                SqlCommand cmd;
                int id;
                ExecuteNonQuery(out cmd, "spAEPage",
                    CreateParameter("@pID", SqlDbType.BigInt, intPageId),
                    CreateParameter("@pPageTitle", SqlDbType.NVarChar, strPageTitle),
                    CreateParameter("@pDescription", SqlDbType.NVarChar, strPageDesc),
                    CreateParameter("@pIsPublic", SqlDbType.Bit, blnIsPublic),
                    CreateParameter("@pUserID", SqlDbType.BigInt, intUserId),
                    CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));

                id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddPage", "clsPageDataService.cs");              
                return 0;
            }
        }

        public void PageAllocated(int intPageAllocationId, int intPageId, int intUserId)
        {
            try
            {
                ExecuteDataSet("spAEPageAllocation", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.BigInt, intPageAllocationId),
                    CreateParameter("@pPageID", SqlDbType.BigInt, intPageId),
                    CreateParameter("@pUserID", SqlDbType.BigInt, intUserId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PageAllocated(int, int..", "clsPageDataService.cs");
            }
        }

        public int GetMaxPageId()
        {
            try
            {
                SqlCommand cmd;
                int id;
                ExecuteNonQuery(out cmd, "spGMaxPageId",
                    CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));
                id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxPageId", "clsPageDataService.cs");
                return 0;
            }
        }

        public DataSet GetPageInfo(int intPageId)
        {
            try
            {
                return ExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetPageInfo", "ClsPageDataService.cs");                
                return null;
            }
        }

        public DataSet GetPageAllocated(int intUserId)
        {
            try
            {
                return ExecuteDataSet("spGetUsersPageAllocated", CommandType.StoredProcedure,
                   CreateParameter("@pUserID", SqlDbType.BigInt, intUserId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetPageAllocated", "ClsPageDataService.cs");               
                return null;
            }

        }

        public void RemovePage(int intPageId)
        {
            try
            {
                ExecuteDataSet("spDPage", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, intPageId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemovePage", "ClsPageDataService.cs");
            }
        }

         public void RemovePageAllocation(int intPageId,int intUserId)
        {
            try
            {
                ExecuteDataSet("spDPageAllocation", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.BigInt, intPageId),
                     CreateParameter("@uID", SqlDbType.BigInt, intUserId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemovePageAllocation", "ClsPageDataService.cs");
            }
        }

    }
}