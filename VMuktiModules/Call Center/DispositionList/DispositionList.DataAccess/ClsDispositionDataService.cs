using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;
namespace DispositionList.DataAccess 
{
    public class ClsDispositionDataService : ClsDataServiceBase
    {

        public ClsDispositionDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDispositionDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Disposition_GetAll(Int64 DispositionListId)
        {
            try
            {
                if (DispositionListId == -1)
                    return ExecuteDataSet("Select ID,DespositionName,IsActive from vDisposition;", CommandType.Text, null);
                else
                    return ExecuteDataSet("Select vDisposition.ID, vDisposition.DespositionName as DespositionName,vDisposition.IsActive from vDisposition,DispListDisp where DispListDisp.DispositionId=vDisposition.ID and DispositionListId=" + DispositionListId + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsDispositionDataService.cs");
                return null;
            } 
        }

        public DataSet Disposition_GetByDispositionListID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spGDispListDisp", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetByDispositionListID()", "ClsDispositionDataService.cs");
                return null;
            } 
        }

        public void Disposition_Save(ref Int64 ID, string DispositionName, Int64 DispositionListId)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEDispListDisp",
                    CreateParameter("@pID", SqlDbType.BigInt, -1, ParameterDirection.Input),
                    CreateParameter("@pDispositionListID", SqlDbType.BigInt, DispositionListId),
                    CreateParameter("@pDispositionID", SqlDbType.BigInt, ID));

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_Save()", "ClsDispositionDataService.cs");
            } 
        }


        public void Disposition_Delete(Int64 ID)
        {
            try
            {
                ExecuteNonQuery("spDDispListDisp", CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_Delete()", "ClsDispositionDataService.cs");
            } 
        }

    }
}
