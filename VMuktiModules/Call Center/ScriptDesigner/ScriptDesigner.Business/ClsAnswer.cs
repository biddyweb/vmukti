using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsAnswer : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _OptionName = ClsConstants.NullString;
        private int _QuestionID = ClsConstants.NullInt;
        private int _ActionQuestionID = ClsConstants.NullInt;
        private string _Description = ClsConstants.NullString;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string OptionName
        {
            get { return _OptionName; }
            set { _OptionName = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public int QuestionID
        {
            get { return _QuestionID; }
            set { _QuestionID = value; }
        }

        public int ActionQuestionID
        {
            get { return _ActionQuestionID; }
            set { _ActionQuestionID = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            OptionName = GetString(row, "Options");
            QuestionID = GetInt(row, "QuestionID");
            Description = GetString(row, "Description");
            ActionQuestionID = GetInt(row, "ActionQueueID");
            return base.MapData(row);
        }

        public static void Delete(int QuesID)
        {
            Delete(QuesID, null);
        }

        public static void Delete(int QuesID, IDbTransaction txn)
        {
            new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Delete(QuesID);
        }

        public void Save()
        {
            Save(null);
        }

        public void Save(IDbTransaction txn)
        {
            new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Save(_ID, _OptionName, _Description, _QuestionID, _ActionQuestionID);
        }

        #endregion 
    }
}
