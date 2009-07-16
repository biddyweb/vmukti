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
using System.Data.SqlClient;
using VMuktiAPI;

namespace VMukti.DataAccess
{
    public class clsProfile : ClsDataServiceBase
    {
        public clsProfile() : base() { }

        public clsProfile(IDbTransaction txn) : base(txn) { }
       

        public DataSet GetCountryUserProfile()
        {
            try
            {
                return ExecuteDataSet("Select ID,CountryName from vCountryUserProfile order by CountryName;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetCountryUserProfile()", "clsProfile.cs");
            }
            return null;
        }

        public DataSet GetLanguages()
        {
            try
            {
                return ExecuteDataSet("Select ID,Language from vLanguage order by Language;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLanguages()", "clsProfile.cs");              
            }
            return null;
        }

        public DataSet GetTimezones()
        {
            try
            {
                return ExecuteDataSet("Select ID,TimeZoneName from vTimezone order by TimeZoneName;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetTimeZones()", "clsProfile.cs");
            }
            return null;
        }

        public DataSet UpdateUserProfile(int userID,string fullName,string email, string country, string state, string city, string timezone, string language, string gender, DateTime birthDate, string homepage, string aboutMe, string homePhone, string officePhone, string mobilePhone)
        {
            try
            {
                SqlCommand cmd;
                return ExecuteDataSet(out cmd, "spAEUserProfile",
                     CreateParameter("@pUserID", SqlDbType.BigInt, userID),
                     CreateParameter("@pFullName", SqlDbType.NVarChar, fullName),
                     CreateParameter("@pEmail", SqlDbType.NVarChar, email),
                     CreateParameter("@pCountry", SqlDbType.NVarChar, country),
                     CreateParameter("@pState", SqlDbType.NVarChar, state),
                     CreateParameter("@pCity", SqlDbType.NVarChar, city),
                     CreateParameter("@pTimezone", SqlDbType.NVarChar, timezone),
                     CreateParameter("@pLanguage", SqlDbType.NVarChar, language),
                     CreateParameter("@pGender", SqlDbType.NVarChar, gender),
                     CreateParameter("@pBirthDate", SqlDbType.DateTime, birthDate),
                     CreateParameter("@pHomePage", SqlDbType.NText, homepage),
                     CreateParameter("@pAboutMe", SqlDbType.NText, aboutMe),
                     CreateParameter("@pHomePhone", SqlDbType.NVarChar, homePhone),
                     CreateParameter("@pOfficePhone", SqlDbType.NVarChar, officePhone),
                     CreateParameter("@pMobilePhone", SqlDbType.NVarChar, mobilePhone));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UpdateUserProfile()", "clsProfile.cs");               
            }
            return null;
        }

        public DataSet GetUserProfile(int userID)
        {
            try
            {
                SqlCommand cmd;
                return ExecuteDataSet(out cmd, "spGUserProfile",
                    CreateParameter("@pUserID", SqlDbType.BigInt, userID));

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserProfile()", "clsProfile.cs");              
                return null;
            }

        }

        public DataSet GetUserID(string displayName)
        {
            try
            {
                return ExecuteDataSet("select * from vUserInfo where DisplayName='" + displayName + "'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserID()", "clsProfile.cs");
                return null;
            }

        }

        public DataSet GetUserDisplayName(int userID)
        {
            try
            {
                return ExecuteDataSet("select * from vUserInfo where ID='" + userID + "'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserDisplayName()", "clsProfile.cs");
                return null;
            }
        }
    }
}
