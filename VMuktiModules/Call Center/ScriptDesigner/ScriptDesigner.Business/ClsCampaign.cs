using System;
using ScriptDesigner.Common;
using System.Data;
using VMuktiAPI;

namespace ScriptDesigner.Business
{
    public class ClsCampaign : ClsBaseObject
    {
        #region Fields

        private Int64 _ID = ScriptDesigner.Common.ClsConstants.NullLong;
        private string _Name = ScriptDesigner.Common.ClsConstants.NullString;
        private bool _IsActive = false;
        private int _DispositionListID = ScriptDesigner.Common.ClsConstants.NullInt;
        
        #endregion 

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public int DispositionListID
        {
            get { return _DispositionListID; }
            set { _DispositionListID = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetLong(row, "ID");
                Name = GetString(row, "Name");
                IsActive = GetBool(row, "IsActive");
                DispositionListID = GetInt(row, "DispositionListID");
                
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapObjects", "ClsCampaign.cs");
                return false;
            }
        }

        #endregion 
    }
}
