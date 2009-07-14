using System;
using System.Data;
using ScriptDesigner.Common;
using ScriptDesigner.DataAccess;
using VMuktiAPI;
namespace ScriptDesigner.Business
{
    public class ClsDisposition : ClsBaseObject
    {
        #region Fields

        private Int64  _ID = ScriptDesigner.Common.ClsConstants.NullInt;
        private string _DespositionName = ScriptDesigner.Common.ClsConstants.NullString;
	    private bool _IsDeleted = false;
	    private bool _IsActive =false;

        #endregion 

        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }
        public string DespositionName
        {
            get { return _DespositionName; }
            set { _DespositionName = value; }
        }
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                DespositionName = GetString(row, "DespositionName");
                //IsDeleted = GetBool(row, "IsDeleted");
                //IsActive = GetBool(row, "IsActive");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsDisposition.cs");
                return false;
            }
            
        }

        #endregion 
    }
}
