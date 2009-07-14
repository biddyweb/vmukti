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
using System.Data.SqlClient;
using VMukti.Common;
using System.Text;
using VMuktiAPI;

namespace VMukti.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///   Defines common DataService routines for transaction management, 
    ///   stored procedure execution, parameter creation, and null value 
    ///   checking
    /// </summary>	
    ////////////////////////////////////////////////////////////////////////////
    public class ClsRecordingDataServiceBase 
    {        
       
        ////////////////////////////////////////////////////////////////////////
        // Fields
        ////////////////////////////////////////////////////////////////////////
        private bool _isOwner = false;   //True if service owns the transaction        
        private SqlTransaction _txn;     //Reference to the current transaction


        ////////////////////////////////////////////////////////////////////////
        // Properties 
        ////////////////////////////////////////////////////////////////////////
        public IDbTransaction Txn
        {
            get { return (IDbTransaction)_txn; }
            set { _txn = (SqlTransaction)value; }
        }


        ////////////////////////////////////////////////////////////////////////
        // Constructors
        ////////////////////////////////////////////////////////////////////////

        public ClsRecordingDataServiceBase() : this(null) { }


        public ClsRecordingDataServiceBase(IDbTransaction txn)
        {
            if (txn == null)
            {
                _isOwner = true;
            }
            else
            {
                _txn = (SqlTransaction)txn;
                _isOwner = false;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        // Connection and Transaction Methods
        ////////////////////////////////////////////////////////////////////////
        protected static string GetConnectionString()
        {
            try
            {
                return VMuktiAPI.VMuktiInfo.MainConnectionString;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"GetConnectionString", "ClsDataServiceBase.cs");
                return null;
            }
        }


        public static IDbTransaction BeginTransaction()
        {
            try
            {
                SqlConnection txnConnection = new SqlConnection(GetConnectionString());
                txnConnection.Open();
                return txnConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BeginTransaction", "clsDataServiceBase.cs");
                return null;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        // ExecuteDataSet Methods
        ////////////////////////////////////////////////////////////////////////
        protected DataSet ExecuteDataSet(string procName,
            params IDataParameter[] procParams)
        {
            try
            {
                SqlCommand cmd;
                return ExecuteDataSet(out cmd, procName, procParams);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ExecuteDataSet", "clsDataServiceBase.cs");
                return null;
            }
        }


        protected DataSet ExecuteDataSet(out SqlCommand cmd, string procName,
            params IDataParameter[] procParams)
        {
            SqlConnection cnx = null;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = null;

            try
            {
                //Setup command object
                cmd = new SqlCommand(procName);
                cmd.CommandType = CommandType.StoredProcedure;
                if (procParams != null)
                {
                    for (int index = 0; index < procParams.Length; index++)
                    {
                        cmd.Parameters.Add(procParams[index]);
                    }
                }
                da.SelectCommand = (SqlCommand)cmd;

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new SqlConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Fill the dataset
                da.Fill(ds);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (cmd != null) cmd.Dispose();
                if (_isOwner)
                {
                    cnx.Dispose(); //Implicitly calls cnx.Close()
                }
            }
            return ds;
        }


        protected DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params IDataParameter[] procParams)
        {
            try
            {
                SqlCommand cmd;
                return ExecuteDataSet(out cmd, cmdText, cmdType, procParams);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ExecuteDataSet(string,CommandType...)", "clsdataservicebase.cs");
                return null;
            }
        }


        protected DataSet ExecuteDataSet(out SqlCommand cmd, string cmdText, CommandType cmdType, params IDataParameter[] procParams)
        {
            SqlConnection cnx = null;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = null;

            try
            {
                //Setup command object
                cmd = new SqlCommand(cmdText);
                cmd.CommandType = cmdType;
                if (procParams != null)
                {
                    for (int index = 0; index < procParams.Length; index++)
                    {
                        cmd.Parameters.Add(procParams[index]);
                    }
                }
                da.SelectCommand = (SqlCommand)cmd;

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new SqlConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Fill the dataset
                da.Fill(ds);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (cmd != null) cmd.Dispose();
                if (_isOwner)
                {
                    cnx.Dispose(); //Implicitly calls cnx.Close()
                }
            }
            return ds;
        }

        ////////////////////////////////////////////////////////////////////////
        // ExecuteNonQuery Methods
        ////////////////////////////////////////////////////////////////////////
        protected void ExecuteNonQuery(string procName,
            params IDataParameter[] procParams)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, procName, procParams);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ExecuteNonQuery", "clsdataservicebase.cs");            
            }
        }


        protected void ExecuteNonQuery(out SqlCommand cmd, string procName,
            params IDataParameter[] procParams)
        {
            //Method variables
            SqlConnection cnx = null;
            cmd = null;  //Avoids "Use of unassigned variable" compiler error

            try
            {
                //Setup command object
                cmd = new SqlCommand(procName);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int index = 0; index < procParams.Length; index++)
                {
                    cmd.Parameters.Add(procParams[index]);
                }

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new SqlConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Execute the command
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    cnx.Dispose(); //Implicitly calls cnx.Close()
                }
                if (cmd != null) cmd.Dispose();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        // CreateParameter Methods
        ////////////////////////////////////////////////////////////////////////
        protected static SqlParameter CreateParameter(string paramName,
            SqlDbType paramType, object paramValue)
        {
            try
            {
                SqlParameter param = new SqlParameter(paramName, paramType);

                if (paramValue != DBNull.Value)
                {
                    switch (paramType)
                    {
                        case SqlDbType.VarChar:
                        case SqlDbType.NVarChar:
                        case SqlDbType.Char:
                        case SqlDbType.NChar:
                        case SqlDbType.Text:
                            paramValue = CheckParamValue((string)paramValue);
                            break;
                        case SqlDbType.DateTime:
                            paramValue = CheckParamValue((DateTime)paramValue);
                            break;
                        case SqlDbType.Int:
                            paramValue = CheckParamValue((int)paramValue);
                            break;
                        case SqlDbType.UniqueIdentifier:
                            paramValue = CheckParamValue(GetGuid(paramValue));
                            break;
                        case SqlDbType.Bit:
                            if (paramValue is bool) paramValue = (int)((bool)paramValue ? 1 : 0);
                            if ((int)paramValue < 0 || (int)paramValue > 1) paramValue = Common.ClsConstants.NullInt;
                            paramValue = CheckParamValue((int)paramValue);
                            break;
                        case SqlDbType.Float:
                            paramValue = CheckParamValue(Convert.ToSingle(paramValue));
                            break;
                        case SqlDbType.Decimal:
                            paramValue = CheckParamValue((decimal)paramValue);
                            break;
                    }
                }
                param.Value = paramValue;
                return param;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string,...", "clsRecordingDataServiceBase");
                return null;
            }
        }

        protected static SqlParameter CreateParameter(string paramName, SqlDbType paramType, ParameterDirection direction)
        {
            try
            {
                SqlParameter returnVal = CreateParameter(paramName, paramType, DBNull.Value);
                returnVal.Direction = direction;
                return returnVal;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string,...", "clsRecordingDataServiceBase");              
                return null;
            }
        }

        protected static SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, ParameterDirection direction)
        {
            try
            {
                SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
                returnVal.Direction = direction;
                return returnVal;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string,...", "clsRecordingDataServiceBase");               
                return null;
            }
        }

        protected static SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size)
        {
            try
            {
                SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
                returnVal.Size = size;
                return returnVal;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string,...", "clsRecordingDataServiceBase");               
                return null;
            }
        }

        protected static SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size, ParameterDirection direction)
        {
            try
            {
                SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
                returnVal.Direction = direction;
                returnVal.Size = size;
                return returnVal;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string, ...", "clsDataServiceBase.cs");
                return null;
            }
        }

        protected static SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size, byte precision)
        {
            try
            {
                SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
                returnVal.Size = size;
                ((SqlParameter)returnVal).Precision = precision;
                return returnVal;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string,...", "clsRecordingDataServiceBase");              
                return null;
            }
        }

        protected static SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size, byte precision, ParameterDirection direction)
        {
            try
            {
                SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
                returnVal.Direction = direction;
                returnVal.Size = size;
                returnVal.Precision = precision;
                return returnVal;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateParameter(string,...", "clsRecordingDataServiceBase");              
                return null;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        // CheckParamValue Methods
        ////////////////////////////////////////////////////////////////////////
        protected static Guid GetGuid(object value)
        {
            
                Guid returnVal = Common.ClsConstants.NullGuid;
                if (value is string)
                {
                    returnVal = new Guid(Convert.ToString(value));
                }
                else if (value is Guid)
                {
                    returnVal = (Guid)value;
                }
                return returnVal;
            }
            
        

        protected static object CheckParamValue(string paramValue)
        {
            try
            {
                if (string.IsNullOrEmpty(paramValue))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase");
                return null;
            }
        }

        protected static object CheckParamValue(Guid paramValue)
        {
            try
            {
                if (paramValue.Equals(Common.ClsConstants.NullGuid))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase.cs");
                return null;
            }
        }

        protected static object CheckParamValue(DateTime paramValue)
        {
            try
            {
                if (paramValue.Equals(Common.ClsConstants.NullDateTime))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase.cs");
                return null;
            }
        }

        protected static object CheckParamValue(double paramValue)
        {
            try
            {
                if (paramValue.Equals(Common.ClsConstants.NullDouble))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase.cs");
                return null;
            }
        }

        protected static object CheckParamValue(float paramValue)
        {
            try
            {
                if (paramValue.Equals(Common.ClsConstants.NullFloat))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase.cs");
                return null;
            }
        }

        protected static object CheckParamValue(Decimal paramValue)
        {
            try
            {
                if (paramValue.Equals(Common.ClsConstants.NullDecimal))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase.cs");
                return null;
            }
        }

        protected static object CheckParamValue(int paramValue)
        {
            try
            {
                if (paramValue.Equals(Common.ClsConstants.NullInt))
                {
                    return DBNull.Value;
                }
                else
                {
                    return paramValue;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckParamValue", "clsRecordingDataServiceBase.cs");
                return null;
            }
        }

    } //class 
}