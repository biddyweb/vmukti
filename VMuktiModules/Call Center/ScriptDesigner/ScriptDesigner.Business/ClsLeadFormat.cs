using System;
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsLeadFormat : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _FormatName = ClsConstants.NullString;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string FormatName
        {
            get { return _FormatName; }
            set { _FormatName = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            FormatName = GetString(row, "LeadFormatName");
            return base.MapData(row);
        }

        #endregion 
    }
}
