using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;
namespace Group.DataAccess
{
    public class ClsUserDataService : ClsDataServiceBase
    {

        public ClsUserDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsUserDataService(IDbTransaction txn) : base(txn) { }


        public DataSet User_GetAll(int GroupId)
        {
            try
            {
                if (GroupId == -1)
                    return ExecuteDataSet("Select ID, DisplayName from vUserInfo where ID Not in (Select Distinct(UserID) from UserGroup);", CommandType.Text, null);
                else
                    return ExecuteDataSet("Select UserInfo.ID, UserInfo.DisplayName as DisplayName from UserInfo, UserGroup where UserGroup.UserId=UserInfo.ID and GroupId=" + GroupId + "  and UserInfo.IsDeleted=0;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_GetAll", "ClsUserDataService.cs");
                return null;
            }
        }

        public DataSet User_GetByGroupID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGUserGroup", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_GetByGroupID", "ClsUserDataService.cs");
                return null;
            }
        }

        public void User_Save(ref int ID, string AgentName,int GroupId)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEUserGroup",
                    CreateParameter("@pID", SqlDbType.Int, -1, ParameterDirection.Input),
                    CreateParameter("@pGroupID", SqlDbType.BigInt, GroupId),
                    CreateParameter("@pUserID", SqlDbType.BigInt, ID));

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_Save", "ClsUserDataService.cs");
            }
        }


        public void User_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDUserGroup", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_Delete", "ClsUserDataService.cs");
            }
        }

    }
}
