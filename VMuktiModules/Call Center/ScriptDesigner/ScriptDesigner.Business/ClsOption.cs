using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsOption : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _Options = ClsConstants.NullString;
        private int _ActionQueueID = ClsConstants.NullInt;
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string Options
        {
            get { return _Options; }
            set { _Options = value;}
        }

        public int ActionQueueID
        {
            get { return _ActionQueueID; }
            set { _ActionQueueID = value; }
        }
        
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            Options = GetString(row, "Options");
            ActionQueueID = GetInt(row, "ActionQueueID");
            return base.MapData(row);
        }

        //public static ClsOption GetByRoleID(int ID)
        //{
        //    ClsOption obj = new ClsOption();
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
        //   return(new Role.DataAccess.ClsRoleDataService(txn).Role_Save(ref _ID,_RoleName,_Description,_IsAdmin,_CreatedBy));
        //}

        #endregion 
    }
}
