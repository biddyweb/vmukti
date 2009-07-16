using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsQuestionR : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _QuestionName = ClsConstants.NullString;
        private string _Category = ClsConstants.NullString;
        private string _QuestionText = ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string QuestionName
        {
            get { return _QuestionName; }
            set { _QuestionName = value;}
        }

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public string QuestionText
        {
            get { return _QuestionText; }
            set { _QuestionText = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            QuestionName = GetString(row, "QuestionName");
            Category = GetString(row, "Category");
            QuestionText = GetString(row, "QuestionText");
            
            return base.MapData(row);
        }

        //public static ClsQuestion GetByRoleID(int ID)
        //{
        //    ClsQuestion obj = new ClsQuestion();
        //    DataSet ds = new Role.DataAccess.ClsRoleDataService().Role_GetByID(ID);
        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new Role.DataAccess.ClsRoleDataService(txn).Role_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        //public int Save()
        //{
        //    return(Save(null));
        //}

        //public int Save(IDbTransaction txn)
        //{
        //   return(new Role.DataAccess.ClsRoleDataService(txn).Role_Save(ref _ID,_QuestionName,_Category,_IsAdmin,_CreatedBy));
        //}

        #endregion 
    }
}
