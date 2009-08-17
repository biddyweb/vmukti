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
//using VMuktiAPI;

namespace Role.Business
{
    public class ClsRole : ClsBaseObject
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
        private string _RoleName = ClsConstants.NullString;
        private string _Description = ClsConstants.NullString;
        private bool _IsAdmin = false;
        private Int64 _CreatedBy = ClsConstants.NullLong;
        private DateTime _CreatedDate = ClsConstants.NullDateTime;
        private Int64 _ModifiedBy = ClsConstants.NullLong;
        private DateTime _ModifiedDate = ClsConstants.NullDateTime;

        #endregion

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }

        public Int64 CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        public Int64 ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set { _IsAdmin = value; }
        }

        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }


        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetLong(row, "ID");
                RoleName = GetString(row, "RoleName");
                Description = GetString(row, "Description");
                IsAdmin = GetBool(row, "IsAdmin");
                CreatedBy = GetLong(row, "CreatedBy");
                CreatedDate = GetDateTime(row, "CreatedDate");
                ModifiedBy = GetLong(row, "ModifiedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsRole.cs"); 
                return false;
            }
        }

        public static ClsRole GetByRoleID(Int64 ID)
        {
            try
            {
                ClsRole obj = new ClsRole();
                DataSet ds = new Role.DataAccess.ClsRoleDataService().Role_GetByID(ID);
                if (!obj.MapData(ds.Tables[0])) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
              VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByRoleID", "ClsRole.cs");
                return null;
            }
        }

        public static void Delete(Int64 ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
              VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsRole.cs");

            }
        }

        public static void Delete(Int64 ID, IDbTransaction txn)
        {
            try
            {
                new Role.DataAccess.ClsRoleDataService(txn).Role_Delete(ID);

            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsRole.cs");
            }
        }

        public void Delete()
        {
            try
            {
                Delete(ID);

            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsRole.cs");
            }
        }

        public void Delete(IDbTransaction txn)
        {
            try
            {
                Delete(ID, txn);

            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsRole.cs");
            }
        }

        public Int64 Save()
        {
            try
            {
                return (Save(null));

            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save", "ClsRole.cs"); 
                return 0;
            }
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                return (new Role.DataAccess.ClsRoleDataService(txn).Role_Save(ref _ID, _RoleName, _Description, _IsAdmin, _CreatedBy));

            }
            catch (Exception ex)
            {
             VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save", "ClsRole.cs"); 
                return 0;
            }
        }

        #endregion
    }
}
