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
using Role.Common;
//using VMuktiAPI;
//using VMuktiAPI;

namespace Role.Business
{
    public abstract class ClsBaseObject
    {
        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return MapData(ds.Tables[0].Rows[0]);

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
             VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsBaseObject.cs");
                return false;
            }
        }

        public virtual bool MapData(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    return MapData(dt.Rows[0]);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsBaseObject.cs");
                return false;
            }
        }
        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataRow row)
        {
            try
            {
                //You can put common data mapping items here (e.g. create date, modified date, etc)
                return true;
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsBaseObject.cs"); 
                return false;
            }
        }

        #region Get Functions

        //////////////////////////////////////////////////////////////////////////////
        protected static int GetInt(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToInt32(row[columnName]) : ClsConstants.NullInt;
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetInt", "ClsBaseObject.cs"); 
                return 0;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static DateTime GetDateTime(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToDateTime(row[columnName]) : ClsConstants.NullDateTime;
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDateTime", "ClsBaseObject.cs"); 
                DateTime a = new DateTime();
                return a;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Decimal GetDecimal(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToDecimal(row[columnName]) : ClsConstants.NullDecimal;
            }
            catch (Exception ex)
            {
              VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDecimal", "ClsBaseObject.cs"); 
                return 0;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static bool GetBool(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToBoolean(row[columnName]) : false;

            }
            catch (Exception ex)
            {
              VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetBool", "ClsBaseObject.cs"); 
                return false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static string GetString(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToString(row[columnName]) : ClsConstants.NullString;
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetString", "ClsBaseObject.cs"); 
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static double GetDouble(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToDouble(row[columnName]) : ClsConstants.NullDouble;

            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDouble", "ClsBaseObject.cs"); 
                return 0;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static float GetFloat(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? Convert.ToSingle(row[columnName]) : ClsConstants.NullFloat;

            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetFloat", "ClsBaseObject.cs"); 
                return 0;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Guid GetGuid(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? (Guid)(row[columnName]) : ClsConstants.NullGuid;
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetGuid", "ClsBaseObject.cs"); 
                Guid a = new Guid();
                return a;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static long GetLong(DataRow row, string columnName)
        {
            try
            {
                return (row[columnName] != DBNull.Value) ? (long)(row[columnName]) : ClsConstants.NullLong;
            }
            catch (Exception ex)
            {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetLong", "ClsBaseObject.cs"); 
                return 0;
            }
        }

        #endregion
    }
}
