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
using System.Data;
using System.Collections.Generic;
using VMukti.Common;
using VMuktiAPI;



namespace VMukti.Business
{
    public class ClsConference : ClsBaseObject
    {
        #region Fields
        private Int64 _ID = VMukti.Common.ClsConstants.NullLong;
        private string _ConfTitle = VMukti.Common.ClsConstants.NullString;
        private DateTime _StartDateTime = VMukti.Common.ClsConstants.NullDateTime;
        private DateTime _EndDateTime = VMukti.Common.ClsConstants.NullDateTime;
        private int _TimezoneId = VMukti.Common.ClsConstants.NullInt;
        private int _PageID = VMukti.Common.ClsConstants.NullInt;
        private string _ConferenceDetail = VMukti.Common.ClsConstants.NullString;
        private bool _IsDeleted;
        private DateTime _CreatedDate = VMukti.Common.ClsConstants.NullDateTime;
        private Int64 _CreatedBy = VMukti.Common.ClsConstants.NullLong;
        private DateTime _ModifiedDate = VMukti.Common.ClsConstants.NullDateTime;
        private Int64 _ModifiedBy = VMukti.Common.ClsConstants.NullLong;
        private bool _Started = VMukti.Common.ClsConstants.NullBoolean;

      
       

        #endregion 

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string ConfTitle
        {
            get { return _ConfTitle; }
            set { _ConfTitle = value; }
        }

        public DateTime StartDateTime
        {
            get { return _StartDateTime; }
            set { _StartDateTime = value; }
        }

        public DateTime EndDateTime
        {
            get { return _EndDateTime; }
            set { _EndDateTime = value; }
        }

       public int TimezoneID
        {
            get { return _TimezoneId; }
            set { _TimezoneId = value; }
        }

       public int PageID
       {
           get
           {
               return _PageID;
           }
           set
           {
               _PageID = value;
           }
       }
      
        public string ConferenceDetail
        {
            get { return _ConferenceDetail; }
            set { _ConferenceDetail = value; }
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

        public bool Started
        {
            get
            {
                return _Started;
            }
            set
            {
                _Started = value;
            }
        }
    
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                ConfTitle = GetString(row, "ConfTitle");
                StartDateTime = GetDateTime(row, "ConfStartDateTime");
                EndDateTime = GetDateTime(row, "ConfEndDateTime");
                TimezoneID = GetInt(row, "TimezoneId");
                PageID = GetInt(row, "PageID");

                ConferenceDetail = GetString(row, "Description");
                
                IsDeleted = GetBool(row, "IsDeleted");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");
                return base.MapData(row);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsConference.cs");
                return false;
            }

        }        

    #endregion
    }
}
