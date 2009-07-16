using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptQuestionAnswers.DataAccess
{
    public class ClsRptQuestionAnswersDataService : ClsDataServiceBase
    {
        public ClsRptQuestionAnswersDataService() : base() { }

        public ClsRptQuestionAnswersDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptQuestionAnswers_GetAllScript()
        {
            return ExecuteDataSet("select * from vScript", CommandType.Text, null);
        }

        public DataSet rptQuestionAnswers_GetAllQuestions(int ScriptID)
        {
            return ExecuteDataSet("Select * from vQuestion where ScriptID="+ScriptID, CommandType.Text, null);
        }

        public DataSet rptQuestionAnswers_GetAllQuestionAnswers(int ScriptID)
        {
            try
            {
                return ExecuteDataSet("spGQuestionAnswers", CommandType.StoredProcedure, CreateParameter("@pScriptID", SqlDbType.BigInt, ScriptID));
                //    return ExecuteDataSet("Select * from vQuestionOptions where QuestionID=" + QuestionID, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
