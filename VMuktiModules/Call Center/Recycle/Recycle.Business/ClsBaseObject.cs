using System;
using System.Data;
using Recycle.Common;
//using VMuktiAPI;

namespace Recycle.Business
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetInt()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDateTime()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDecimal()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetBool()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetString()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDouble()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetFloat()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetGuid()", "ClsBaseObjects.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetLong()", "ClsBaseObjects.cs");
                return 0;
            }
        }

        #endregion
    }
}
