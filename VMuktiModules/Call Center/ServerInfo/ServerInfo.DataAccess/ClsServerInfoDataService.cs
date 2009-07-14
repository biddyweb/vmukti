using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;

namespace ServerInfo.DataAccess
{
    public class ClsServerInfoDataService : ClsDataServiceBase
    {

        public ClsServerInfoDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsServerInfoDataService(IDbTransaction txn) : base(txn) { }


        public DataSet ServerInfo_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vServerInfo where IsDeleted=0;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex,"ServerInfo_GetAll()", "ClsServerInfoDataService.cs");
                return null;
            }
        }

        public DataSet User_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select ID,DisplayName from vUserInfo where IsDeleted=0;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "User_GetAll()", "ClsServerInfoDataService.cs");
                return null;
            }
        }

        public DataSet ServerInfo_GetByID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spGServerInfo", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ServerInfo_GetByID()", "ClsServerInfoDataService.cs");
                return null;
            }
        }

        public Int64 ServerInfo_Save(Int64 ID, string ServerName, string IPAddress, string Location, string ServerUserName, string ServerPassword, int PortNumber, DateTime AddedDate, int AddedBY, int CreatedBy)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEServerInfo",
                    CreateParameter("@pID", SqlDbType.BigInt, ID),
                    CreateParameter("@pServerName", SqlDbType.NVarChar, ServerName, 100),
                    CreateParameter("@pLocation", SqlDbType.NVarChar, Location, 100),
                    CreateParameter("@pIPAddress", SqlDbType.NVarChar, IPAddress, 100),
                    CreateParameter("@pServerUserName", SqlDbType.NVarChar, ServerUserName, 100),
                    CreateParameter("@pServerPassword", SqlDbType.NVarChar, ServerPassword, 100),
                    CreateParameter("@pPortNumber", SqlDbType.Int, PortNumber),
                    CreateParameter("@pAddedDate", SqlDbType.DateTime, AddedDate),
                    CreateParameter("@pAddedBY", SqlDbType.BigInt, AddedBY),
                    CreateParameter("@pCreatedBy", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

                ID = Int64.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());
                cmd.Dispose();

                return ID;

            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ServerInfo_Save()", "ClsServerInfoDataService.cs");
                
                return 0;
            }
        }
        public void ServerInfo_Delete(Int64 ID)
        {
            try
            {
                ExecuteNonQuery("spDServerInfo", CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ServerInfo_Delete()", "ClsServerInfoDataService.cs");
            }
        }

    }
}
