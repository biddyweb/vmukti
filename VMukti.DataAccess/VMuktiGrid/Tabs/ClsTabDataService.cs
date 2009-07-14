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
    public class ClsTabDataService : ClsDataServiceBase
    {
        public ClsTabDataService() : base() { }

        public ClsTabDataService(IDbTransaction txn) : base(txn) { }
        

        public DataSet GetTabs(int intPageId)
        {
            try
            {
                return ExecuteDataSet("select * from tabs where pageid=" + intPageId + "and IsDeleted='False'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetTabs", "clsTabDataService.cs");               
                return null;
            }
        }

        public int AddTab(int intTabId, int intPageId, int intTabPosition, string strTabTitle, string strTabDesc, int intUserId, double C1Width, double C2Width, double C3Width, double C4Heigth, double C5Height)
        {
            try
            {
                SqlCommand cmd;
                int id;
                ExecuteNonQuery(out cmd, "spAETabs",
                    CreateParameter("@pID", SqlDbType.BigInt, intTabId),
                    CreateParameter("@pPageID", SqlDbType.BigInt, intPageId),
                    CreateParameter("@pTabPosition", SqlDbType.Int, intTabPosition),
                    CreateParameter("@pTabTitle", SqlDbType.NVarChar, strTabTitle),
                    CreateParameter("@pDescription", SqlDbType.NVarChar, strTabDesc),
                    CreateParameter("@pGivenID", SqlDbType.BigInt, intUserId),
                    CreateParameter("@pC1Width", SqlDbType.Float, C1Width),
                    CreateParameter("@pC2Width", SqlDbType.Float, C2Width),
                    CreateParameter("@pC3Width", SqlDbType.Float, C3Width),
                    CreateParameter("@pC4Height", SqlDbType.Float, C4Heigth),
                    CreateParameter("@pC5Height", SqlDbType.Float, C5Height),
                    CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));

                id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddTab(int, int ...)", "clsTabDataService.cs");
                return 0;
            }

        }

        public int GetMaxTabId()
        {
            try
            {
                SqlCommand cmd;
                int id;
                ExecuteNonQuery(out cmd, "spGMaxTabId",
                    CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));
                id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxTabId", "clsTabDataService.cs");
                return 0;
            }
        }

        public void RemoveTab(int intTabId)
        {
            try
            {
                ExecuteDataSet("spDTabs", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, intTabId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemoteTab(int intTabId)", "clsTabDataService.cs");
            }
        }

        public DataSet GetTabPage(int intPageId)
        {
            try
            {

                return ExecuteDataSet("spGTabPage", CreateParameter("@pID", SqlDbType.Int, intPageId));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetTabPage", "ClsTabDataService.cs");              
                return null;
            }
        }

        public DataSet GetTab(int intTabid)
        {
            try
            {
                return ExecuteDataSet("select * from Tabs where id=" + intTabid + "and isdeleted='False'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetTab", "ClsTabDataService.cs");               
                return null;
            }
        }

    }
}