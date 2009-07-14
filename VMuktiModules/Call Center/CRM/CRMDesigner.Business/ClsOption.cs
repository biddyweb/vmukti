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
using CRMDesigner.Common;
using System.Data;
using CRMDesigner.DataAccess;
using System.Text;
using VMuktiAPI;

namespace CRMDesigner.Business
{
    public class ClsOption : ClsBaseObject
    {
        #region Fields
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
        private int  _ID = CRMDesigner.Common.ClsConstants.NullInt;
	    private string _Options = CRMDesigner.Common.ClsConstants.NullString;
        private int _ActionQueueID = CRMDesigner.Common.ClsConstants.NullInt;
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string Options
        {
            get { return _Options; }
            set { _Options = value;}
        }

        public int ActionQueueID
        {
            get { return _ActionQueueID; }
            set { _ActionQueueID = value; }
        }
        
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                Options = GetString(row, "Options");
                ActionQueueID = GetInt(row, "ActionQueueID");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsOption.cs");
                return false;

            }
        }

        //public static ClsOption GetByRoleID(int ID)
        //{
        //    ClsOption obj = new ClsOption();
        //    DataSet ds = new Role.DataAccess.ClsRoleDataService().Role_GetByID(ID);
        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new Role.DataAccess.ClsRoleDataService(txn).Role_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        //public int Save()
        //{
        //    return(Save(null));
        //}

        //public int Save(IDbTransaction txn)
        //{
        //   return(new Role.DataAccess.ClsRoleDataService(txn).Role_Save(ref _ID,_RoleName,_Description,_IsAdmin,_CreatedBy));
        //}

        #endregion 
    }
}
