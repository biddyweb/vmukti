using System;
using System.Data;
using ScriptDesigner.Common;
using VMuktiAPI;

namespace ScriptDesigner.Business
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
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsBaseObject.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsBaseObject.cs");
                return false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataRow row)
        {
            //You can put common data mapping items here (e.g. create date, modified date, etc)
            return true;
        }

        #region Get Functions

        //////////////////////////////////////////////////////////////////////////////
        protected static int GetInt(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToInt32(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullInt;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static DateTime GetDateTime(DataRow row, string columnName)
        {            
            return (row[columnName] != DBNull.Value) ? Convert.ToDateTime(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullDateTime;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Decimal GetDecimal(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDecimal(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullDecimal;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static bool GetBool(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToBoolean(row[columnName]) : false;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static string GetString(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToString(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullString;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static double GetDouble(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDouble(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullDouble;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static float GetFloat(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToSingle(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullFloat;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Guid GetGuid(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Guid)(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullGuid;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static long GetLong(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (long)(row[columnName]) : ScriptDesigner.Common.ClsConstants.NullLong;
        }

        #endregion
    }
}
