using System;
using System.Data;
using System.Data.SqlClient;
using Disposition.Common;
//using VMuktiAPI;

/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace Disposition.DataAccess
{

    public class ClsDispositionDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDispositionDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDispositionDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Disposition_GetAll()
        {
            try
            {
                //Accessing data from Database
                return ExecuteDataSet("Select * from vDisposition;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsDispositionDataService.cs");
                return null;
            }
        }


        public DataSet Disposition_GetByID(Int64 ID)
        {
            try
            {
                //Get data from database using spGDisposition stored procedure
                return ExecuteDataSet("spGDisposition", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetByID()", "ClsDispositionDataService.cs");
                return null;
            }
        }


        public Int64 Disposition_Save(ref Int64 ID, string DespositionName, bool IsActive, string Description, Int64 CreatedBy)
        {
            try
            {
                //Save record to Database using spAEDisposition stroed procedure
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEDisposition",
                    CreateParameter("@pID", SqlDbType.BigInt, ID, ParameterDirection.Input),
                    CreateParameter("@pDespositionName", SqlDbType.NVarChar, DespositionName, 50),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pDescription", SqlDbType.NVarChar, Description, 100),
                    CreateParameter("@pCreatedBy", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

                ID = Int64.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());


                cmd.Dispose();
                return ID;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_Save()", "ClsDispositionDataService.cs");
                return 0;
            }
        }

        
        public void Disposition_Delete(Int64 ID)
        {
            //Delete data from database using spDDisposition stored Procedure
            try
            {
                ExecuteNonQuery("spDDisposition", CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_Delete()", "ClsDispositionDataService.cs");
                
            }
        }

    } //class

} //namespace