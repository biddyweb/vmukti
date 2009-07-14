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
using Group.Common;
using System.Data;
using Group.DataAccess;
using VMuktiAPI;
using System.Text;
namespace Group.Business
{
    public class ClsGroup : ClsBaseObject
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

        private int  _ID = Group.Common.ClsConstants.NullInt;
	    private string _GroupName = Group.Common.ClsConstants.NullString;
	    private bool _IsDeleted = false;
	    private bool _IsActive =false;
	    private DateTime _CreatedDate = Group.Common.ClsConstants.NullDateTime;
	    private int _CreatedBy = Group.Common.ClsConstants.NullInt;
	    private DateTime _ModifiedDate = Group.Common.ClsConstants.NullDateTime;
	    private int _ModifiedBy = Group.Common.ClsConstants.NullInt;
        private string _Description = Group.Common.ClsConstants.NullString;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string GroupName
        {
            get{return _GroupName;}
            set { _GroupName = value; }
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

        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }

        public int ModifiedBy
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
                ID = GetInt(row, "ID");
                GroupName = GetString(row, "GroupName");
                IsDeleted = GetBool(row, "IsDeleted");
                IsActive = GetBool(row, "IsActive");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");
                Description = GetString(row, "Description");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsGroup.cs");
                return false;
            }
        }

        public static ClsGroup GetByGroupID(int ID)
        {
            try
            {
                ClsGroup obj = new ClsGroup();
                DataSet ds = new Group.DataAccess.ClsGroupDataService().Group_GetByID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByGroupID", "ClsGroup.cs"); 
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsGroup.cs");
               
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {
                //Function for Delete data 
                new Group.DataAccess.ClsGroupDataService(txn).Group_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsGroup.cs");

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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsGroup.cs");

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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsGroup.cs");

            }
        }

        public int Save()
        {
            try
            {
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsGroup.cs"); 
                return 0;
            }
        }

        public int Save(IDbTransaction txn)
        {
            try
            {
                //Function for saving data in group
                return (new Group.DataAccess.ClsGroupDataService(txn).Group_Save(ref _ID, _GroupName, _IsActive, _CreatedBy, _Description));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsGroup.cs");
                return 0;
            }
        }

        #endregion 
    }
}
