using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRMDesigner.DataAccess
{
    public class ClsDynamicScriptDataService : ClsDataServiceBase
    {

        public ClsDynamicScriptDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDynamicScriptDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Options_GetAll(int QueID)
        {
            return ExecuteDataSet("Select ID,Options,ActionQueueID from QuestionOptions where QuestionID="+ QueID+";" , CommandType.Text, null);
        }

        public DataSet Answer_GetAll()
        {
            return null;
            //return ExecuteDataSet("Select ID,Options,ActionQueueID from QuestionOptions where QuestionID=" + QueID + ";", CommandType.Text, null);
        }

        public DataSet Questions_GetAll(int ScriptID)
        {
            return ExecuteDataSet("Select ID,QuestionName,QuestionText,Category from Question where ScriptID=" + ScriptID+";", CommandType.Text, null);
        }

        public void Answer_Save(int CallID,int QusOptionID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAAnswer",
                CreateParameter("@pCallId", SqlDbType.BigInt, CallID),
                CreateParameter("@pQusOptionId", SqlDbType.BigInt, QusOptionID));
            cmd.Dispose();
        }

        //public DataSet User_GetByID(int ID)
        //{
        //    return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        //public void User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
        //{
        //    SqlCommand cmd;
        //    ExecuteNonQuery(out cmd, "spAEUserInfoPayroll",
        //        CreateParameter("@pID", SqlDbType.Int, ID),
        //        CreateParameter("@pDisplayName", SqlDbType.NVarChar, displayame, 100),
        //        CreateParameter("@pRoleID", SqlDbType.BigInt, roleID),
        //        CreateParameter("@pFirstName", SqlDbType.NVarChar, firstName, 50),
        //        CreateParameter("@pLastName", SqlDbType.NVarChar, lastName, 50),
        //        CreateParameter("@pEMail", SqlDbType.NVarChar, eMail, 256),
        //        CreateParameter("@pPassword", SqlDbType.NVarChar, password, 50),
        //        CreateParameter("@pIsActive", SqlDbType.Bit, isActive),
        //        CreateParameter("@pByUserID", SqlDbType.BigInt, byUserId),
        //        CreateParameter("@pRatePerHour", SqlDbType.Float, ratePerHour),
        //        CreateParameter("@pOverTimeRate", SqlDbType.Float, overTimeRate),
        //        CreateParameter("@pDoubleRatePerHour", SqlDbType.Float, doubleRatePerHour),
        //        CreateParameter("@pDoubleOverTimeRate", SqlDbType.Float, doubleOverTimeRate));

        //    cmd.Dispose();
        //}

        //public void User_Delete(int ID)
        //{
        //    ExecuteNonQuery("spDUserInfoPayroll", CreateParameter("@pID", SqlDbType.Int, ID));
        //}

    }
}
