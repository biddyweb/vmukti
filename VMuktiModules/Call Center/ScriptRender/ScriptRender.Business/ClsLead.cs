using System;
using ScriptRender.Common;
using System.Data;
using ScriptRender.DataAccess;

namespace ScriptRender.Business
{
    public class ClsLead : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
        private string _PropertyValue = ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }
        
        public string PropertyValue
        {
            get { return _PropertyValue; }
            set { _PropertyValue = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            PropertyValue = GetString(row, "PropertyValue");
            
            return base.MapData(row);
        }

        #endregion 
    }
}
