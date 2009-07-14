using System;
using System.Data;
using System.Data.SqlClient;
using DispositionList.Common;
using VMuktiAPI;

/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace DispositionList.DataAccess
{

    public class ClsDispositionListDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService 
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDispositionListDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDispositionListDataService(IDbTransaction txn) : base(txn) { }


        public DataSet DispositionList_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vDispositionList;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--DispositionList--:--DispositionList.DataAccess--:--ClsDispositionListDataService.cs--:--DispositionList_GetAll()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                return null;
            } 
            
        }


        public DataSet DispositionList_GetByID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spGDispositionList", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--DispositionList--:--DispositionList.DataAccess--:--ClsDispositionListDataService.cs--:--DispositionList_GetByID()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                return null;
            } 
        }

        public Int64 DispositionList_Save(ref Int64 ID, string DispositionListName, bool IsActive, Int64 CreatedBy, string Description)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEDispositionList",
                    CreateParameter("@pID", SqlDbType.BigInt, ID, ParameterDirection.Input),
                    CreateParameter("@pDespsitionListName", SqlDbType.NVarChar, DispositionListName),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pDescription", SqlDbType.NVarChar, Description),
                    CreateParameter("@pUserID", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

                ID = Convert.ToInt64(cmd.Parameters["@pReturnMaxId"].Value.ToString());


                cmd.Dispose();
                return ID;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--DispositionList--:--DispositionList.DataAccess--:--ClsDispositionListDataService.cs--:--DispositionList_Save()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                return 0;
            } 
        }


        public void DispositionList_Delete(Int64 ID)
        {
            try
            {
                ExecuteNonQuery("spDDispositionList", CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--DispositionList--:--DispositionList.DataAccess--:--ClsDispositionListDataService.cs--:--DispositionList_Delete()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
               
            } 
        }

    } //class

} //namespace