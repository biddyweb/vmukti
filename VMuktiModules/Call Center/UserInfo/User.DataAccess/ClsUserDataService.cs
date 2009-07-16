using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace User.DataAccess
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


        public DataSet User_GetAll()
        {
            return ExecuteDataSet("Select * from vUserInfo;", CommandType.Text, null);
        }

        public DataSet Role_GetAll()
        {
            return ExecuteDataSet("Select * from vRoles;", CommandType.Text, null);
        }

        public DataSet User_GetByID(int ID)
        {
            return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, 
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pUName", SqlDbType.VarChar, ""),
                CreateParameter("@pPass", SqlDbType.VarChar, ""));
        }

        public int User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEUserInfoPayroll",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pDisplayName", SqlDbType.NVarChar, displayame, 100),
                CreateParameter("@pRoleID", SqlDbType.BigInt, roleID),
                CreateParameter("@pFirstName", SqlDbType.NVarChar, firstName, 50),
                CreateParameter("@pLastName", SqlDbType.NVarChar, lastName, 50),
                CreateParameter("@pEMail", SqlDbType.NVarChar, eMail, 256),
                CreateParameter("@pPassword", SqlDbType.NVarChar, password, 50),
                CreateParameter("@pIsActive", SqlDbType.Bit, isActive),
                CreateParameter("@pByUserID", SqlDbType.BigInt, byUserId),
                CreateParameter("@pRatePerHour", SqlDbType.Float, ratePerHour),
                CreateParameter("@pOverTimeRate", SqlDbType.Float, overTimeRate),
                CreateParameter("@pDoubleRatePerHour", SqlDbType.Float, doubleRatePerHour),
                CreateParameter("@pDoubleOverTimeRate", SqlDbType.Float, doubleOverTimeRate),
                CreateParameter("@pReturnId", SqlDbType.BigInt, ParameterDirection.Output));

            int retID = int.Parse(cmd.Parameters["@pReturnId"].Value.ToString());

            cmd.Dispose();

            return retID;
        }

        public void User_Delete(int ID)
        {
            ExecuteNonQuery("spDUser", CreateParameter("@pID", SqlDbType.Int, ID));
        }

    }
}
