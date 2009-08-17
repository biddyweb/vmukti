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
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;

namespace Role.DataAccess
{
    public class ClsRoleDataService : ClsDataServiceBase
    {

        public ClsRoleDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsRoleDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Role_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vRoles;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Role_GetAll", "ClsRoleDataService.cs"); 
                return null;
            }
        }

        public DataSet Permission_GetAll(Int64 ModuleID)
        {
            try
            {
                return ExecuteDataSet("Select * from vPermissions where ModuleID='" + ModuleID + "';", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Permission_GetAll", "ClsRoleDataService.cs"); 
                return null;
            }
        }

        public DataSet Module_GetAll()
        {
            try
            {
            return ExecuteDataSet("Select * from vModule;", CommandType.Text, null);}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Module_GetAll", "ClsRoleDataService.cs"); 
                return null;
            }
        }

        public DataSet Role_GetByID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spGRole", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Role_GetByID", "ClsRoleDataService.cs"); 
                return null;
            }
        }

        public Int64 Role_Save(ref Int64 ID, string RoleName, string Description, bool IsAdmin, Int64 ByUserId)
        {
            try
            {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAERoles",
                CreateParameter("@pID", SqlDbType.BigInt, ID),
                CreateParameter("@pRoleName", SqlDbType.NVarChar, RoleName, 50),
                CreateParameter("@pDescription", SqlDbType.NVarChar, Description, 100),
                CreateParameter("@pIsAdmin", SqlDbType.Bit, IsAdmin),
                CreateParameter("@pUserID", SqlDbType.BigInt, ByUserId),
                CreateParameter("@pReturnMaxID", SqlDbType.BigInt,ParameterDirection.Output));

            Int64 ReturnID = Int64.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
            cmd.Dispose();

            return ReturnID;
}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Role_Save", "ClsRoleDataService.cs"); 
                return 0;
            }
        }

        public void Role_Delete(Int64 ID)
        {
            try
            {
            ExecuteNonQuery("spDRoles", CreateParameter("@pID", SqlDbType.BigInt, ID));}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Role_Delete", "ClsRoleDataService.cs");
            }
        }

        public void Permission_Save(Int64 ID, Int64 RoleID)
        {
            try
            {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEModulePermission",
                CreateParameter("@pPermissionID", SqlDbType.BigInt, ID),
                CreateParameter("@pRoleID", SqlDbType.BigInt, RoleID));
            cmd.Dispose();}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Permission_Save", "ClsRoleDataService.cs");
            }
        }

        public DataTable Permissions_Get(Int64 RoleID)
        {
            try
            {
            DataSet ds = ExecuteDataSet("Select * from vModulePermission where RoleID ='" + RoleID + "';", CommandType.Text, null);
            return ds.Tables[0];}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Permissions_Get", "ClsRoleDataService.cs"); 
                return null;
            }
        }

        public void Permission_Delete(Int64 RoleID)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spDModulePermission",
                    CreateParameter("@pRoleID", SqlDbType.BigInt, RoleID));
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Permission_Delete", "ClsRoleDataService.cs");
            }
        }

    }
}
