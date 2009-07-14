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
using DispositionList.Common;
using System.Data;
using DispositionList.DataAccess;
using VMuktiAPI;
using System.Text;
namespace DispositionList.Business
{
    public class ClsDispositionList : ClsBaseObject
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

        private Int64  _ID = Constants.Nullint;
        private string _DispositionListName = Constants.NullString;
	    private bool _IsDeleted = false;
	    private bool _IsActive =false; 
	    private DateTime _CreatedDate = Constants.NullDateTime;
	    private Int64 _CreatedBy = Constants.Nullint;
	    private DateTime _ModifiedDate = Constants.NullDateTime;
	    private Int64 _ModifiedBy = Constants.Nullint;
        private string _Description = Constants.NullString;

        #endregion 

        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }
        public string DispositionListName
        {
            get{return _DispositionListName;}
            set { _DispositionListName = value; }
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
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = Getint(row, "ID");
                DispositionListName = GetString(row, "DespsitionListName");
                IsDeleted = GetBool(row, "IsDeleted");
                IsActive = GetBool(row, "IsActive");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = Getint(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = Getint(row, "ModifiedBy");
                Description = GetString(row, "Description");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsDispositionCollection.cs");
                return false;
            } 
        }

        public static ClsDispositionList GetByDispositionListID(Int64 rID)
        {
            try
            {
                ClsDispositionList obj = new ClsDispositionList();
                DataSet ds = new DispositionList.DataAccess.ClsDispositionListDataService().DispositionList_GetByID(rID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByDispositionListID()", "ClsDispositionCollection.cs");
                return null;
            } 
        }

        public static void Delete(Int64 rID)
        {
            try
            {
                Delete(rID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDispositionCollection.cs");
            } 
        }

        public static void Delete(Int64 rID, IDbTransaction txn)
        {
            try
            {
                new DispositionList.DataAccess.ClsDispositionListDataService(txn).DispositionList_Delete(rID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDispositionCollection.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDispositionCollection.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDispositionCollection.cs");

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsDispositionCollection.cs");
                return 0;
            } 
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                return (new DispositionList.DataAccess.ClsDispositionListDataService(txn).DispositionList_Save(ref _ID, _DispositionListName, _IsActive, _CreatedBy, _Description));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsDispositionCollection.cs");
                return 0;
            } 
        }

        #endregion 
    }
}
