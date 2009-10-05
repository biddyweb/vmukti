</* VMukti 2.0 -- An Open Source Unified Communications Engine
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.Application/CtlUserInfo.xaml.cs
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;

*/
using System;
using System.Data;
using Treatment.Common;

namespace Treatment.Business
{
    public abstract class ClsBaseObject
    {
        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataSet ds)
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

        public virtual bool MapData(DataTable dt)
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

        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataRow row)
        {
            //You can put common data mapping items here (e.g. created date, modified date, etc)
            return true;
        }

        #region Get Functions

        //////////////////////////////////////////////////////////////////////////////
        protected static int GetInt(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToInt32(row[columnName]) : ClsConstants.NullInt;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static DateTime GetDateTime(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDateTime(row[columnName]) : ClsConstants.NullDateTime;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Decimal GetDecimal(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDecimal(row[columnName]) : ClsConstants.NullDecimal;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static bool GetBool(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToBoolean(row[columnName]) : false;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static string GetString(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToString(row[columnName]) : ClsConstants.NullString;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static double GetDouble(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDouble(row[columnName]) : ClsConstants.NullDouble;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static float GetFloat(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToSingle(row[columnName]) : ClsConstants.NullFloat;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Guid GetGuid(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Guid)(row[columnName]) : ClsConstants.NullGuid;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static long GetLong(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (long)(row[columnName]) : ClsConstants.NullLong;
        }

        #endregion
    }
}
