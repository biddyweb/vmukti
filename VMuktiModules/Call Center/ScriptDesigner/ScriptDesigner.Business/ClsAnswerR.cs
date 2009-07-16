using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsAnswerR : ClsBaseObject
    {
        #region Fields

        private int _ID = ClsConstants.NullInt;
        private int _CallID = ClsConstants.NullInt;
        private int _QusOptionID = ClsConstants.NullInt;
        private bool _Value = false;
	    
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public int CallID
        {
            get { return _CallID; }
            set { _CallID = value; }
        }

        public int QusOptionID
        {
            get { return _QusOptionID; }
            set { _QusOptionID = value; }
        }

        public bool Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            CallID = GetInt(row, "CallId");
            QusOptionID = GetInt(row, "QusOptionID");
            Value = GetBool(row, "Value");
            return base.MapData(row);
        }

        public void Save()
        {
            Save(null);
        }

        public void Save(IDbTransaction txn)
        {
            new ScriptDesigner.DataAccess.ClsDynamicScriptDataService(txn).Answer_Save(_CallID, _QusOptionID);
        }

        #endregion 
    }
}
