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
using Role.Common;
using System.Data;
using Role.DataAccess;
using System.Text;

namespace Role.Business
{
    public class ClsPermission : ClsBaseObject
    {
        //public static StringBuilder sb1;
        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}
        #region Fields

        private Int64 _ID = ClsConstants.NullLong;
        private string _PermissionName = ClsConstants.NullString;
        private Int64 _PermissionValue = ClsConstants.NullLong;
        private Int64 _ModuleID = ClsConstants.NullLong;

        #endregion

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string PermissionName
        {
            get { return _PermissionName; }
            set { _PermissionName = value; }
        }

        public Int64 PermissionValue
        {
            get { return _PermissionValue; }
            set { _PermissionValue = value; }
        }

        public Int64 ModuleID
        {
            get { return _ModuleID; }
            set { _ModuleID = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetLong(row, "ID");
                PermissionName = GetString(row, "PermissionName");
                PermissionValue = GetInt(row, "PermissionValue");
                ModuleID = GetLong(row, "ModuleID");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsPermissions.cs");
                return false;
            }



        }

        public static DataTable GetPermByRoleID(Int64 RoleID)
        {
            try
            {
                return GetPermByRoleID(RoleID, null);
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetPermByRoleID", "ClsPermissions.cs");
                return null;
            }

        }


        public static DataTable GetPermByRoleID(Int64 RoleID, IDbTransaction txn)
        {
            try
            {
                return (new Role.DataAccess.ClsRoleDataService(txn).Permissions_Get(RoleID));
            }
            catch (Exception ex)
            {
              VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetPermByRoleID", "ClsPermissions.cs");
                return null;
            }

        }


        public static void Delete(Int64 RoleID)
        {
            try
            {
                Delete(RoleID, null);
            }
            catch (Exception ex)
            {
              VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsPermissions.cs");
            }

        }

        public static void Delete(Int64 RoleID, IDbTransaction txn)
        {
            try
            {
                new Role.DataAccess.ClsRoleDataService(txn).Permission_Delete(RoleID);
            }
            catch (Exception ex)
            {
             VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsPermissions.cs");
            }

        }

        //public void Delete()
        //{
        //    Delete(_PermissionValue);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(_PermissionValue, txn);
        //}

        public void Save(Int64 PID, Int64 RoleID)
        {
            try
            {
                Save(null, PID, RoleID);
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save", "ClsPermissions.cs");
            }

        }

        public void Save(IDbTransaction txn, Int64 PID, Int64 RoleID)
        {
            try
            {
                new Role.DataAccess.ClsRoleDataService(txn).Permission_Save(PID, RoleID);
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save", "ClsPermissions.cs");
            }

        }

        #endregion
    }
}
