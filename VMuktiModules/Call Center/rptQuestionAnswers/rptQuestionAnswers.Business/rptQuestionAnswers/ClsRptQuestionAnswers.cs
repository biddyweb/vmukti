    using System;
using System.Data;

using rptQuestionAnswers.DataAccess;
using rptQuestionAnswers.Common;

namespace rptQuestionAnswers.Business
{
    public class ClsRptQuestionAnswers : ClsBaseObject
    {
        #region Fields

        #endregion 

        #region Properties

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            return base.MapData(row);
        }

        public static DataSet GetAllScript()
        {
            return new rptQuestionAnswers.DataAccess.ClsRptQuestionAnswersDataService().rptQuestionAnswers_GetAllScript();
        }

        public static DataSet GetAllListOfQuestion(int ScriptID)
        {
            return new rptQuestionAnswers.DataAccess.ClsRptQuestionAnswersDataService().rptQuestionAnswers_GetAllQuestions(ScriptID);
        }

        public static DataSet GetAllQuestionAnswers(int ScriptID)
        {
            return new rptQuestionAnswers.DataAccess.ClsRptQuestionAnswersDataService().rptQuestionAnswers_GetAllQuestionAnswers(ScriptID);
        }

        #endregion 
    }
}
