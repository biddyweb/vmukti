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

namespace VMukti.DataAccess
{
    public class ClsPermissionDataService : ClsDataServiceBase
    {
        public ClsPermissionDataService() : base() { }

        public ClsPermissionDataService(IDbTransaction txn) : base(txn) { }

        public int AddPermission(int permissionId, int moduleId, string permissionName, int permissionValue)
        {
            SqlCommand cmd;
            int id;

            ExecuteNonQuery(out cmd, "spAEPermissions",
            CreateParameter("@pID", SqlDbType.BigInt, permissionId),
            CreateParameter("@pModuleId", SqlDbType.BigInt, moduleId),
            CreateParameter("@pPermissionName", SqlDbType.NVarChar, permissionName),
            CreateParameter("@pPermissionValue", SqlDbType.BigInt, permissionValue),
            CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));

            id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
            cmd.Dispose();
            return id;
        }

        public int AddModulePermission(int permissionId, int intRoleId)
        {
            SqlCommand cmd;
            int id;

            ExecuteNonQuery(out cmd, "spAEModulePermission",
            CreateParameter("@pPermissionID", SqlDbType.BigInt, permissionId),
            CreateParameter("@pRoleID", SqlDbType.BigInt, intRoleId),
            CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));

            id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
            cmd.Dispose();
            return id;
        }

        public DataSet GetPermissions()
        {
            return ExecuteDataSet("select * from permissions", CommandType.Text, null);
        }

        public DataSet GetPermissionRefModule(int intModuleId, int intRoleId)
        {
            return (ExecuteDataSet("spGPermissionRefModule", CommandType.StoredProcedure,
           CreateParameter("@pModuleId", SqlDbType.BigInt, intModuleId),
           CreateParameter("@pRoleId", SqlDbType.BigInt, intRoleId)));
        }
        public DataSet GetAllModPermissions(int intModuleId)
        {
            return ExecuteDataSet("select * from permissions where ModuleID=" + intModuleId, CommandType.Text, null);
        }
    }
}
