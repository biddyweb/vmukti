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
using PredictiveDialler.DataAccess;
using VMuktiAPI;
using PredictiveDialler.Common;

namespace PredictiveDialler.Business
{
    public class ClsLead : ClsBaseObject
    {
        #region Fields

        private int _ID = PredictiveDialler.Common.ClsConstants.NullInt;
        private DateTime _CreatedDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private int _CreatedBy = PredictiveDialler.Common.ClsConstants.NullInt;

        private long _PhoneNo = PredictiveDialler.Common.ClsConstants.NullLong;
        private int _LeadFormatID = PredictiveDialler.Common.ClsConstants.NullInt;
        private DateTime _DeletedDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private int _DeletedBy = PredictiveDialler.Common.ClsConstants.NullInt;
        private bool _IsDeleted = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private DateTime _ModifiedDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private int _ModifiedBy = PredictiveDialler.Common.ClsConstants.NullInt;
        private bool _DNCFlag = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private int _DNCBy = PredictiveDialler.Common.ClsConstants.NullInt;
        private long _ListID = PredictiveDialler.Common.ClsConstants.NullLong;
        private long _LocationID = PredictiveDialler.Common.ClsConstants.NullLong;
        private int _RecycleCount = PredictiveDialler.Common.ClsConstants.NullInt;
        private string _Status = PredictiveDialler.Common.ClsConstants.NullString;
        private bool _IsAssign = false;


        #endregion 

        #region Properties

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
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

        public long PhoneNo
        {
            get{return _PhoneNo;}
            set{_PhoneNo = value;}
        }
        public int LeadFormatID
        {
            get { return _LeadFormatID; }
            set { _LeadFormatID = value; }
        }

        public DateTime DeletedDate
        {
            get { return _DeletedDate; }
            set { _DeletedDate = value; }
        }
        public int DeletedBy
        {
            get { return _DeletedBy; }
            set { _DeletedBy = value; }
        }

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
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
        public bool DNCFlag
        {
            get { return _DNCFlag; }
            set { _DNCFlag = value; }
        }
        public int DNCBy
        {
            get { return _DNCBy; }
            set { _DNCBy = value; }
        }
        public long ListID
        {
            get { return _ListID; }
            set { _ListID = value; }
        }
        public long LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        public int RecycleCount
        {
            get { return _RecycleCount; }
            set { _RecycleCount = value; }
        }

        public bool IsAssign
        {
            get { return _IsAssign; }
            set { _IsAssign = value; }
        }
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            PhoneNo = GetLong(row, "PhoneNo");
            LeadFormatID = GetInt(row, "LeadFormatID");
            CreatedDate = GetDateTime(row, "CreatedDate");
            CreatedBy = GetInt(row, "CreatedBy");
            DeletedDate = GetDateTime(row, "DeletedDate");
            DeletedBy = GetInt(row, "DeletedBy");
            IsDeleted = GetBool(row, "IsDeleted");
            ModifiedDate = GetDateTime(row, "ModifiedDate");
            ModifiedBy = GetInt(row, "ModifiedBy");
            DNCFlag = GetBool(row, "DNCFlag");
            DNCBy = GetInt(row, "DNCBy");
            ListID = GetLong(row, "ListID");
            LocationID = GetLong(row, "LocationID");
            RecycleCount = GetInt(row, "RecycleCount");
            Status = GetString(row, "Status");

            return base.MapData(row);
        }

        public static ClsLead GetByGroupID(int ID)
        {
            try
            {
            ClsLead obj = new ClsLead();
            DataSet ds = new PredictiveDialler.DataAccess.ClsUserDataService().User_GetByID(ID);
            
            if (!obj.MapData(ds.Tables[0])) obj = null;
            return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByGroupID()", "ClsLead.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsLead.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {
                new PredictiveDialler.DataAccess.ClsUserDataService(txn).User_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsLead.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsLead.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction txn)", "ClsLead.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsLead.cs");
            }
        }

        public void Save(IDbTransaction txn)
        {
         //   new PredictiveDialler.DataAccess.ClsUserDataService(txn).User_Save(ref _ID, _DisplayName, _RoleID, _FName, _LName, _EMail, _PassWord, _IsActive, _ModifiedBy, _RatePerHour, _OverTimeRate, _DoubleRatePerHour, _DoubleOverTimeRate);
         //   new PredictiveDialler.DataAccess.ClsUserDataService(txn).User_Save(ref _ID, _PhoneNo, _LeadFormatID, _CreatedDate, _CreatedBy, _DeletedDate, _DeletedBy, _IsDeleted, _ModifiedDate, _ModifiedBy, _DNCFlag, _DNCBy, _ListID,_LocationID,_RecycleCount);
        }
        public void GetList4Leads(long userID,out long CampaingID)
        {
            try
            {
            DataSet ds = new PredictiveDialler.DataAccess.ClsUserDataService().GetLeadsList(userID, out CampaingID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetList4Leads()", "ClsLead.cs");
                CampaingID = 0;
            }

        }
        
        #endregion 
    }
}
