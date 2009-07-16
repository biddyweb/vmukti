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
using System.Text;
using System.Data;
using VMuktiAPI;
namespace VMukti.Business
{
    public class clsProfileMap : ClsBaseObject
    {        
       

        #region Fields
        
        private int _UserID = VMukti.Common.ClsConstants.NullInt;
        private string _FullName = VMukti.Common.ClsConstants.NullString;
        private string _Email = VMukti.Common.ClsConstants.NullString;
        private string _Country = VMukti.Common.ClsConstants.NullString;

        private string _State = VMukti.Common.ClsConstants.NullString;
        private string _City = VMukti.Common.ClsConstants.NullString;
        private string _Timezone = VMukti.Common.ClsConstants.NullString;
        private string _Language = VMukti.Common.ClsConstants.NullString;
        private string _Gender = VMukti.Common.ClsConstants.NullString;
        private DateTime _BirthDate = VMukti.Common.ClsConstants.NullDateTime;
        private string _HomePage = VMukti.Common.ClsConstants.NullString;
        private string _AboutMe = VMukti.Common.ClsConstants.NullString;
        private string _HomePhone = VMukti.Common.ClsConstants.NullString;
        private string _OfficePhone = VMukti.Common.ClsConstants.NullString;
        private string _MobilePhone = VMukti.Common.ClsConstants.NullString;
        

        #endregion 

        #region Properties
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        public string Timezone
        {
            get { return _Timezone; }
            set { _Timezone = value; }
        }

        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        public string Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        public DateTime BirthDate
        {
            get { return _BirthDate; }
            set { _BirthDate = value; }
        }

        public string HomePage
        {
            get { return _HomePage; }
            set { _HomePage  = value; }
        }

        public string AboutMe
        {
            get { return _AboutMe; }
            set { _AboutMe = value; }
        }

        public string HomePhone
        {
            get { return _HomePhone; }
            set { _HomePhone = value; }
        }

        public string OfficePhone
        {
            get { return _OfficePhone; }
            set { _OfficePhone = value; }
        }

        public string MobilePhone
        {
            get { return _MobilePhone; }
            set { _MobilePhone = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                UserID = GetInt(row, "UserID");
                FullName = GetString(row, "FullName");
                Email = GetString(row, "EMail");
                Country = GetString(row, "Country");
                State = GetString(row, "State");
                City = GetString(row, "City");
                Timezone = GetString(row, "Timezone");
                Language = GetString(row, "LanguageKnown");
                Gender = GetString(row, "Gender");
                BirthDate = GetDateTime(row, "Birthdate");
                HomePage = GetString(row, "HomePage");
                AboutMe = GetString(row, "AboutMe");
                HomePhone = GetString(row, "HomePhone");
                OfficePhone = GetString(row, "OfficePhone");
                MobilePhone = GetString(row, "MobilePhone");


                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "clsProfileMap.cs");               
                return false;
            }
        }
        #endregion

    }
}
