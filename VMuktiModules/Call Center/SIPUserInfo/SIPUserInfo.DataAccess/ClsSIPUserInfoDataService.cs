using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;
namespace SIPUserInfo.DataAccess
{
    public class ClsSIPUserInfoDataService : ClsDataServiceBase
    {

        public ClsSIPUserInfoDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsSIPUserInfoDataService(IDbTransaction txn) : base(txn) { }


        public DataSet SIPUserInfo_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vSIPUsers;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_GetAll()", "ClsSIPUserInfoDataService.cs");
                return null;
            }
        }

        public DataSet SIPUserInfo_GetByID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGSIPUsers", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_GetByID()", "ClsSIPUserInfoDataService.cs");
                return null;
            }
        }

        public Int64 SIPUserInfo_Save(ref int ID, int SIPID, int SIPPass, int ActiveServerID, int CreatedBy)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAESIPUsers",
                    CreateParameter("@pID", SqlDbType.Int, ID),
                    CreateParameter("@pSIPID", SqlDbType.Int, SIPID),
                    CreateParameter("@pSIPPass", SqlDbType.Int, SIPPass),
                    CreateParameter("@pActiveServerID", SqlDbType.BigInt, ActiveServerID),
                    CreateParameter("@pByUserID", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

                Int64 RID = Int64.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());
                cmd.Dispose();
                return RID;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_Save()", "ClsSIPUserInfoDataService.cs");
                return 0;
            }
        }

        public void SIPUserInfo_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDSIPUsers", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPUserInfo_Delete()", "ClsSIPUserInfoDataService.cs");
             
            }
        }

    }
}
