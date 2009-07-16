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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VMuktiAPI;

namespace Dialer_AutoProgressive.Business
{
    public class ClsDisposition : ClsBaseObject
    {
        #region Fields

        private int _ID = Dialer_AutoProgressive.Common.ClsConstants.NullInt;
        private string _DespositionName = Dialer_AutoProgressive.Common.ClsConstants.NullString;
        private string _Description = Dialer_AutoProgressive.Common.ClsConstants.NullString;
        private bool _IsActive = Dialer_AutoProgressive.Common.ClsConstants.NullBoolean;
        private bool _IsDeleted = Dialer_AutoProgressive.Common.ClsConstants.NullBoolean;
        private DateTime _CreatedDate = Dialer_AutoProgressive.Common.ClsConstants.NullDateTime;
        private int _CreatedBy = Dialer_AutoProgressive.Common.ClsConstants.NullInt;
        private DateTime _ModifiedDate = Dialer_AutoProgressive.Common.ClsConstants.NullDateTime;
        private int _ModifiedBy = Dialer_AutoProgressive.Common.ClsConstants.NullInt;

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string DespositionName
        {
            get { return _DespositionName; }
            set { _DespositionName = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
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


        #endregion

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            DespositionName = GetString(row, "DespositionName");
            Description = GetString(row, "Description");
            IsActive = GetBool(row, "IsActive");
            IsDeleted = GetBool(row, "IsDeleted");
            CreatedDate = GetDateTime(row, "CreatedDate");
            CreatedBy = GetInt(row, "CreatedBy");
            ModifiedDate = GetDateTime(row, "ModifiedDate");
            ModifiedBy = GetInt(row, "ModifiedBy");

            return base.MapData(row);
        }

        public static string GetZoneName(long LeadID)
        {
            try
            {
                return (new Dialer_AutoProgressive.DataAccess.ClsUserDataService().GetZoneName(LeadID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByGroupID()", "ClsDisposition.cs");
                return null;

            }
        }

        //public static string GetDispositionName(long DispositionID)
        //{
        //    try
        //    {
        //        return (new Dialer_AutoProgressive.DataAccess.ClsUserDataService().GetDispositionName(DispositionID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        VMuktiHelper.ExceptionHandler(ex, "GetDispositionName()", "ClsDisposition.cs");
        //    }
        //}

        //public static string GetPhoneNo(long LeadID)
        //{
        //    try
        //    {
        //        return (new Dialer_AutoProgressive.DataAccess.ClsUserDataService().GetPhoneNo(LeadID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        VMuktiHelper.ExceptionHandler(ex, "GetPhoneNo()", "ClsDisposition.cs");
        //    }
        //}
    }
}
