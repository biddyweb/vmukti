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
using System.Data;
using CallingList.Common;
using CallingList.DataAccess;
using VMuktiAPI;
using System.Text;

namespace CallingList.Business
{
    public class ClsCallingList : ClsBaseObject
    {
        #region Fields

        private Int64  _ID = CallingList.Common.ClsConstants.NullInt;
        private string _ListName = CallingList.Common.ClsConstants.NullString;
	    private bool _IsDeleted = false;
	    private bool _IsActive =false;
        private bool _IsDNCList = false;
	    private DateTime _CreatedDate = CallingList.Common.ClsConstants.NullDateTime;
	    private Int64 _CreatedBy = CallingList.Common.ClsConstants.NullInt;
	    private DateTime _ModifiedDate = CallingList.Common.ClsConstants.NullDateTime;
	    private Int64 _ModifiedBy = CallingList.Common.ClsConstants.NullInt;
        

        #endregion 
        public static StringBuilder sb1;

             
        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }
        public string ListName
        {
            get { return _ListName; }
            set { _ListName = value; }
        }
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        public bool IsDNCList
        {
            get { return _IsDNCList; }
            set { _IsDNCList = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public Int64 CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }
        public Int64 ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
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
                ID = GetInt(row, "ID");
                ListName = GetString(row, "ListName");
                IsDeleted = GetBool(row, "IsDeleted");
                IsActive = GetBool(row, "IsActive");
                IsDNCList = GetBool(row, "IsDNCList");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsCallingList.cs"); 
                return false;
            }
        }

        public static ClsCallingList GetByCallingListID(int ID)
        {
            try
            {
                ClsCallingList obj = new ClsCallingList();
                DataSet ds = new CallingList.DataAccess.ClsCallingListDataService().CallingList_GetByID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByCallingListID", "ClsCalllingList.cs"); 
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsCalllingList.cs"); 
            }
        }

        public static void Delete(Int64 ID, IDbTransaction txn)
        {
            try
            {
                new CallingList.DataAccess.ClsCallingListDataService(txn).CallingList_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsCalllingList.cs"); 
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsCalllingList.cs"); 
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsCalllingList.cs"); 
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
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsCalllingList.cs"); 
                return 0;

            }
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                return (new CallingList.DataAccess.ClsCallingListDataService(txn).CallingList_Save(ref _ID, _ListName, _IsActive, _IsDNCList, _CreatedBy));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsCalllingList.cs"); 
                return 0;
            }
        }

        #endregion 
    }
}
