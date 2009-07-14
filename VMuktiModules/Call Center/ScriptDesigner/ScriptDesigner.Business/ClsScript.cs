using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsScript : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _ScriptName = ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string ScriptName
        {
            get { return _ScriptName; }
            set { _ScriptName = value;}
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            ScriptName = GetString(row, "ScriptName");
            
            return base.MapData(row);
        }

        //public static ClsScript GetByGroupID(int ID)
        //{
        //    ClsScript obj = new ClsScript();
        //    DataSet ds = new User.DataAccess.ClsUserDataService().User_GetByID(ID);
            
        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new User.DataAccess.ClsUserDataService(txn).User_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        //public void Save()
        //{
        //    Save(null);
        //}

        //public void Save(IDbTransaction txn)
        //{
        //   new User.DataAccess.ClsUserDataService(txn).User_Save(ref _ID,_DisplayName,_RoleID,_FName,_LName,_EMail,_PassWord,_IsActive,_ModifiedBy,_RatePerHour,_OverTimeRate,_DoubleRatePerHour,_DoubleOverTimeRate);
        //}

        #endregion 
    }
}
