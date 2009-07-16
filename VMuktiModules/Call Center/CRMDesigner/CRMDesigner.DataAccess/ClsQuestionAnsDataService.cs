using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRMDesigner.DataAccess
{
    public class ClsQuestionAnsDataService : ClsDataServiceBase
    {

        public ClsQuestionAnsDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsQuestionAnsDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Script_GetAll()
        {
            return ExecuteDataSet("Select * from vScript where IsDeleted=0;", CommandType.Text, null);
        }

        public DataSet LeadFormat_GetAll()
        {
            return ExecuteDataSet("Select * from LeadFormat;", CommandType.Text, null);
        }

        public DataSet LeadFields_GetAll(Int64 FormtID)
        {
            return ExecuteDataSet("Select CustomFieldID as ID,LeadCustomFields.FieldName as LeadFormatName from LeadFields,LeadCustomFields where LeadFields.CustomFieldID=LeadCustomFields.ID and LeadFields.LeadFormatID=" + FormtID.ToString() + ";", CommandType.Text, null);
        }

        public DataSet Question_GetAll(int ScriptID)
        {
            return ExecuteDataSet("Select * from Question where ScriptID=" + ScriptID.ToString() + ";" , CommandType.Text, null);
        }

        public DataSet Options_GetAll(int QuesID)
        {
            return ExecuteDataSet("Select * from QuestionOptions where QuestionID=" + QuesID.ToString() + ";", CommandType.Text, null);
        }

        public DataSet Question_GetByID(int QueID)
        {
            return ExecuteDataSet("spGQuestion", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, QueID));
        }

        public int Question_Save(int ID, string Header,string Text,string Desc,string Category,int ScriptID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEQuestion",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pQuestionName", SqlDbType.NVarChar, Header, 100),
                CreateParameter("@pQuestionText", SqlDbType.NVarChar, Text,255),
                CreateParameter("@pDescription", SqlDbType.NVarChar, Desc, 100),
                CreateParameter("@pCategory", SqlDbType.NVarChar, Category, 50),
                CreateParameter("@pScriptID", SqlDbType.BigInt, ScriptID),
                CreateParameter("@pReturnMaxId", SqlDbType.BigInt, -1,ParameterDirection.Output));

            cmd.Dispose();

            return (int.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString()));
        }

        public void Options_Delete(int QuestionID)
        {
            SqlCommand cmdDeleteJoin;
            ExecuteNonQuery(out cmdDeleteJoin, "spDQuestionOptions",
                CreateParameter("@pQuestionID", SqlDbType.Int, QuestionID));
            cmdDeleteJoin.Dispose();
        }

        public void Options_Save(int ID, string Option, string Description, int QuestionID, int ActionQuestionID)
        {
            if (ActionQuestionID == null)
            {
                ActionQuestionID = QuestionID;
            }
            
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEQuestionOptions",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pOptions", SqlDbType.NVarChar, Option, 100),
                CreateParameter("@pDescription", SqlDbType.NVarChar, Description, 100),
                CreateParameter("@pQuestionID", SqlDbType.BigInt, QuestionID),
                CreateParameter("@pActionQueueID", SqlDbType.BigInt,ActionQuestionID));
            cmd.Dispose();
        }

        public void Question_Delete(int ID)
        {
            ExecuteNonQuery("spDQuestionOptions", CreateParameter("@pQuestionID", SqlDbType.Int, ID));
            ExecuteNonQuery("spDQuestion", CreateParameter("@pID", SqlDbType.Int, ID));
        }

    }
}
