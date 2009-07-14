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
    public class ClsPodDataService : ClsDataServiceBase
    {
        public ClsPodDataService() : base() { }

        public ClsPodDataService(IDbTransaction txn) : base(txn) { }
        
       

        public int Add_Pod(int PodId, int TabId, string strPodTitle, int ColumnID, string IFile, int intModuleId, int intUserid)
        {
            try
            {
                SqlCommand cmd;
                int id;

                ExecuteNonQuery(out cmd, "spAEPODLayout",
                CreateParameter("@pID", SqlDbType.BigInt, PodId),
                CreateParameter("@pTabID", SqlDbType.BigInt, TabId),
                CreateParameter("@pPodTitle", SqlDbType.NVarChar, strPodTitle),
                CreateParameter("@pColumnID", SqlDbType.BigInt, ColumnID),
                CreateParameter("@pIconFile", SqlDbType.NVarChar, IFile),
                CreateParameter("@pModuleID", SqlDbType.BigInt, intModuleId),
                CreateParameter("@pUserID", SqlDbType.BigInt, intUserid),
                CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));

                id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddPod(int, int, ...)", "clsPodDataService.cs");
                return 0;
            }
        }

        public DataSet GetPodTab(int intTabId)
        {
            try
            {
                return ExecuteDataSet("spGPodTab", CreateParameter("@pID", SqlDbType.Int, intTabId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetPodTab(int intTabId)", "clsPodDataService.cs");
                return null;
            }
        }

        public void RemovePodTab(int intTabId)
        {
            try
            {
                ExecuteDataSet("spDPodTab", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, intTabId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemovePod(int intPodId)", "clsPodDataService.cs");                
            }
        }

        public void RemovePod(int intPodId)
        {
            try
            {
                ExecuteDataSet("spDPODLayout", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, intPodId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemovePod(int intPodId)", "clsPodDataService.cs");
            }
        }
    }
}