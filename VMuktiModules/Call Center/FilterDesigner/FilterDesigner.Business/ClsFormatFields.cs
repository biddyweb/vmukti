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
using FilterDesigner.Common;
using System.Data;
using FilterDesigner.DataAccess;
using System.Text;
//using VMuktiAPI;
namespace FilterDesigner.Business
{
    public class ClsFormatField : ClsBaseObject
    {
        //public static StringBuilder sb1;
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _LeadFieldName = ClsConstants.NullString;
        private Int64 _CustomFieldID = ClsConstants.NullLong;
        private int _StartPosition = ClsConstants.NullInt;
        

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string LeadFieldName
        {
            get { return _LeadFieldName; }
            set { _LeadFieldName = value; }
        }

        public Int64 CustomFieldID
        {
            get { return _CustomFieldID; }
            set { _CustomFieldID = value; }
        }

        public int StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }

        #endregion 

        #region Methods

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

        public override bool MapData(DataRow row)
        {
            try
            {
                //ID = GetInt(row, "ID");
                LeadFieldName = GetString(row, "LeadFieldName");
                CustomFieldID = GetLong(row, "CustomFieldID");
                //StartPosition = GetInt(row, "StartPosition");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsFormatField.cs");
                return false;
            }
        }

        //public static ClsUser GetByGroupID(int ID)
        //{
        //    ClsUser obj = new ClsUser();
        //    DataSet ds = new User.DataAccess.ClsUserDataService().User_GetByID(ID);
            
        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new User.DataAccess.ClsUserDataService(txn).User_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        //public void Save()
        //{
        //    Save(null);
        //}

        //public void Save(IDbTransaction txn)
        //{
        //   new User.DataAccess.ClsUserDataService(txn).User_Save(ref _ID,_DisplayName,_RoleID,_FName,_LName,_EMail,_PassWord,_IsActive,_ModifiedBy,_RatePerHour,_OverTimeRate,_DoubleRatePerHour,_DoubleOverTimeRate);
        //}

        #endregion 
    }
}
