
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
using Location.DataAccess;
using System.Data;

namespace Location.Business
{
    public class ClsLocationBusiness
    {
        ClsImportLocationDataService objLocationDataservice = new ClsImportLocationDataService();

        public DataSet fncGetTimeZone()
        {
            try
            {
                return objLocationDataservice.GetTimeZone();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetTimeZone()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public DataSet fncGetCountry(bool blIsRelated, string TimeZoneName)
        {
            try
            {
                if (!blIsRelated)
                {
                    return objLocationDataservice.GetCountry();
                }
                else
                {
                    string TimeZoneId = objLocationDataservice.GetTimeZoneId(TimeZoneName);
                    return objLocationDataservice.GetCountry(TimeZoneId);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetCountry()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public DataSet fncGetState(bool blIsRelated, string CountryName)
        {
            try
            {
                if (!blIsRelated)
                {
                    return objLocationDataservice.GetState();
                }
                else
                {
                    string CountryId = objLocationDataservice.GetCountryId(CountryName);
                    return objLocationDataservice.GetState(CountryId);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetState()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public DataSet fncGetAreaCode(string Type,string strvalue)
        {
            try
            {
                if (Type == "TimeZone")
                {
                    return objLocationDataservice.GetAreaCode(Type, strvalue);
                }
                else
                {
                    string StateId = objLocationDataservice.GetStateId(strvalue);
                    return objLocationDataservice.GetAreaCode(Type, StateId);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetAreaCode()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public DataSet fncGetZipCode(string StateName)
        {
            try
            {
                string StateId = objLocationDataservice.GetStateId(StateName);
                return objLocationDataservice.GetZipCode(StateId);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetZipCode()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public string fncGetTimeZoneId(string TimeZoneName)
        {
            try
            {
                return objLocationDataservice.GetTimeZoneId(TimeZoneName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetTimeZoneId()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public string fncGetCountryId(string CountryName)
        {
            try
            {
                return objLocationDataservice.GetCountryId(CountryName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetCountryId()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public string fncGetStateId(string StateName)
        {
            try
            {
                return objLocationDataservice.GetStateId(StateName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetStateId()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public string fncGetAreaCodeId(string Areacode)
        {
            try
            {
                return objLocationDataservice.GetAreacodeId(Areacode);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetAreaCodeId()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public string fncGetZipId(string ZipCode)
        {
            try
            {
                return objLocationDataservice.GetZipId(ZipCode);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetZipId()", "ClsLocationBusiness.cs");
                return null;
            }
        }

        public void fncInsertTimeZone(string TimeZoneName)
        {
            try
            {
                objLocationDataservice.InsertInToTimeZone(TimeZoneName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertTimeZone()", "ClsLocationBusiness.cs");
            }
        }

        public void fncInsertCountry(string CountryName, string countryCode, string Description)
        {
            try
            {
                objLocationDataservice.InsertIntoCountry(CountryName, countryCode, Description);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertCountry()", "ClsLocationBusiness.cs");
            }
        }

        public void fncInsertState(string StateName, string StateAbb)
        {
            try
            {
                objLocationDataservice.InsertInToState(StateName, StateAbb);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertState()", "ClsLocationBusiness.cs");
            }
        }

        public void fncInsertAreaCode(string AreaCode, string TimeZone)
        {
            try
            {
                objLocationDataservice.InsertIntoAreaCode(AreaCode, TimeZone);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertAreaCode()", "ClsLocationBusiness.cs");
            }
        }

        public int fncInsertZipCode(string ZipCode, string StateName)
        {
            try
            {
                string StateId = objLocationDataservice.GetStateId(StateName);
                return objLocationDataservice.InsertZipCode(ZipCode, StateId);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertAreaCode()", "ClsLocationBusiness.cs");
                return 0;
            }
        }

        public void fncInsertLocation(Int64 TimeZoneId, Int64 CountryId, Int64 StateID, Int64 AreacodeId, Int64 ZipCodeID)
        {
            try
            {
                objLocationDataservice.InsertIntoLocation(TimeZoneId, CountryId, StateID, AreacodeId, ZipCodeID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertAreaCode()", "ClsLocationBusiness.cs");
            }
        }

        public bool fncCheckZipcode(string ZipCode)
        {
            try
            {
                int Count = objLocationDataservice.GetZipCodeCount(ZipCode);
                if (Count >= 1)
                {
                    return true;
                }
                else if (Count == -1)
                {
                    System.Windows.MessageBox.Show("Error");
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertAreaCode()", "ClsLocationBusiness.cs");
                return false;
            }
        }

    }
}
