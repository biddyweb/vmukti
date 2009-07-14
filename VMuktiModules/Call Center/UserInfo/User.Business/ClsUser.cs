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
using User.Common;
using System.Data;
using User.DataAccess;
using System.Text;
using VMuktiAPI;

namespace User.Business
{
    public class ClsUser : ClsBaseObject
    {
        

        #region Fields

        private int  _ID = User.Common.ClsConstants.NullInt;
	    private string _DisplayName = User.Common.ClsConstants.NullString;
        private int _RoleID = User.Common.ClsConstants.NullInt;
        private string _RoleName = User.Common.ClsConstants.NullString;
        private string _FName = User.Common.ClsConstants.NullString;
        private string _LName = User.Common.ClsConstants.NullString;
        private string _EMail = User.Common.ClsConstants.NullString;
        private string _PassWord = User.Common.ClsConstants.NullString;
        private bool _IsActive = false;
	    private DateTime _CreatedDate = User.Common.ClsConstants.NullDateTime;
	    private int _CreatedBy = User.Common.ClsConstants.NullInt;
	    private DateTime _ModifiedDate = User.Common.ClsConstants.NullDateTime;
	    private int _ModifiedBy = User.Common.ClsConstants.NullInt;

        private float _RatePerHour = User.Common.ClsConstants.NullFloat;
        private float _OverTimeRate = User.Common.ClsConstants.NullFloat;
        private float _DoubleRatePerHour = User.Common.ClsConstants.NullFloat;
        private float _DoubleOverTimeRate = User.Common.ClsConstants.NullFloat;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value;}
        }

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }

        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }

        public string FName
        {
            get { return _FName; }
            set { _FName = value; }
        }

        public string LName
        {
            get { return _LName; }
            set { _LName = value; }
        }

        public string EMail
        {
            get { return _EMail; }
            set { _EMail = value; }
        }

        public string PassWord
        {
            get { return _PassWord; }
            set { _PassWord = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public DateTime CreatedDate
        {
            get { return _CreatedDate;}
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

        public float RatePerHour
        {
            get { return _RatePerHour; }
            set { _RatePerHour = value; }
        }

        public float OverTimeRate
        {
            get { return _OverTimeRate; }
            set { _OverTimeRate = value; }
        }

        public float DoubleRatePerHour
        {
            get { return _DoubleRatePerHour; }
            set { _DoubleRatePerHour = value; }
        }

        public float DoubleOverTimeRate
        {
            get { return _DoubleOverTimeRate; }
            set { _DoubleOverTimeRate = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            DisplayName = GetString(row, "DisplayName");
            RoleID = GetInt(row, "RoleID");
            try
            {
                RoleName = GetString(row, "RoleName");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsUser.cs");
            }
            FName = GetString(row, "FirstName");
            LName = GetString(row, "LastName");
            EMail = GetString(row, "EMail");
            PassWord = GetString(row, "Password");
            IsActive = GetBool(row, "IsActive");
            CreatedDate = GetDateTime(row,"CreatedDate");
            CreatedBy = GetInt(row, "CreatedBy");
            ModifiedDate = GetDateTime(row,"ModifiedDate");
            ModifiedBy = GetInt(row, "ModifiedBy");

            RatePerHour = GetFloat(row, "RatePerHour");
            OverTimeRate = GetFloat(row, "OverTimeRate");
            DoubleRatePerHour = GetFloat(row, "DoubleRatePerHour");
            DoubleOverTimeRate = GetFloat(row, "DoubleOverTimeRate");

            return base.MapData(row);
        }

        public static ClsUser GetByGroupID(int ID)
        {
            try
            {
                ClsUser obj = new ClsUser();
                DataSet ds = new User.DataAccess.ClsUserDataService().User_GetByID(ID);

                if (!obj.MapData(ds.Tables[0])) obj = null;
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
                new User.DataAccess.ClsUserDataService(txn).User_Delete(ID);
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

        public int Save()
        {
            try
            {
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsUser.cs");
                return 0;
            }
        }

        public int Save(IDbTransaction txn)
        {
            try
            {

                return (new User.DataAccess.ClsUserDataService(txn).User_Save(ref _ID, _DisplayName, _RoleID, _FName, _LName, _EMail, _PassWord, _IsActive, _ModifiedBy, _RatePerHour, _OverTimeRate, _DoubleRatePerHour, _DoubleOverTimeRate));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsUser.cs");
                return 0;
            }
        }

        #endregion 
    }
}
