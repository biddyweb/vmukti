using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsQuestion : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _Header = ClsConstants.NullString;
        private string _Name = ClsConstants.NullString;
        private string _Description = ClsConstants.NullString;
        private string _Category = ClsConstants.NullString;
        private int _ScriptID = ClsConstants.NullInt;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public int ScriptID
        {
            get { return _ScriptID; }
            set { _ScriptID = value; }
        }

        
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            Header = GetString(row, "QuestionName");
            Description = GetString(row, "Description");
            Name = GetString(row, "QuestionText");
            Category = GetString(row, "Category");
            ScriptID = GetInt(row, "ScriptID");
            
            return base.MapData(row);
        }

        public static ClsQuestion GetByQueID(int QueID)
        {
            ClsQuestion obj = new ClsQuestion();
            DataSet ds = new ScriptDesigner.DataAccess.ClsQuestionAnsDataService().Question_GetByID(QueID);

            if (!obj.MapData(ds.Tables[0])) obj = null;
            return obj;
        }

        public static void Delete(int ID)
        {
            Delete(ID, null);
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Question_Delete(ID);
        }

        public void Delete()
        {
            Delete(ID);
        }

        public void Delete(IDbTransaction txn)
        {
            Delete(ID, txn);
        }

        public int Save()
        {
            return(Save(null));
        }

        public int Save(IDbTransaction txn)
        {
            return (new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Question_Save(_ID, _Header, _Name, _Description, _Category, _ScriptID));
        }

        #endregion 
    }
}
