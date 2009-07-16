/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using Group.Common;
using VMuktiAPI;
namespace Group.Business
{
    public class ClsUser : ClsBaseObject 
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

        private int _ID = VMuktiAPI.ClsConstants.NullInt;
        private string _AgentName = VMuktiAPI.ClsConstants.NullString;
        public int GroupId = VMuktiAPI.ClsConstants.NullInt;

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string AgentName
        {
            get { return _AgentName; }
            set { _AgentName = value; }
        }
        
        //public int GroupId
        //{
        //    get { return _GroupId; }
        //    set { _GroupId = value; }
        //}
        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                AgentName = GetString(row, "DisplayName");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsUser.cs");
                return false;
            }
        }

        public static ClsUser GetByGroupID(int ID)
        {
            try
            {
                ClsUser obj = new ClsUser();
                DataSet ds = new Group.DataAccess.ClsUserDataService().User_GetByGroupID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByGroupID", "ClsUser.cs"); 
                return null;
            }
        }

        public static void Delete(int ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsUser.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {
                new Group.DataAccess.ClsUserDataService(txn).User_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsUser.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsUser.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsUser.cs");
            }
        }

        public void Save()
        {
            try
            {
                Save(null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsUser.cs");
            }
        }

        public void Save(IDbTransaction txn)
        {
            try
            {
                //Function for saving data in usergroup
                new Group.DataAccess.ClsUserDataService(txn).User_Save(ref _ID, _AgentName, GroupId);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsUser.cs");
            }
        }

        #endregion 
    }
}
