using System;
using System.Data;
using System.Collections.Generic;
using DashBoard.Common;
using VMuktiAPI;



namespace DashBoard.Business
{
    public class ClsWidgets : ClsBaseObjects
    {
        #region Fields

        private int intModuleId = DashBoard.Common.ClsConstants.NullInt;
        private string strModuleTitle = DashBoard.Common.ClsConstants.NullString;
        private string strIsCollaborative = DashBoard.Common.ClsConstants.NullString;
        private string strZipName = DashBoard.Common.ClsConstants.NullString;
        private string strAssemblyFile = DashBoard.Common.ClsConstants.NullString;
        private string strClassName = DashBoard.Common.ClsConstants.NullString;
        private byte[] byImageFile;
        #endregion

        #region Properties

        public byte[] ImageFile
        {
            get { return byImageFile; }
            set { byImageFile = value; }
        }

        public int ModuleId
        {
            get { return intModuleId; }
            set { intModuleId = value; }
        }

        public string ModuleTitle
        {
            get { return strModuleTitle; }
            set { strModuleTitle = value; }
        }

        public string IsCollaborative
        {
            get { return strIsCollaborative; }
            set { strIsCollaborative = value; }
        }

        public string ZipFile
        {
            get { return strZipName; }
            set { strZipName = value; }
        }

        public string AssemblyFile
        {
            get { return strAssemblyFile; }
            set { strAssemblyFile = value; }
        }

        public string ClassName
        {
            get { return strClassName; }
            set { strClassName = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                intModuleId = GetInt(row, "ID");
                strModuleTitle = GetString(row, "ModuleName");
                strIsCollaborative = GetString(row, "IsCollaborative");
                strZipName = GetString(row, "ZipFile");
                strAssemblyFile = GetString(row, "AssemblyFile");
                strClassName = GetString(row, "ClassName");
                byImageFile = GetImage(row, "ImageFile");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsWidgets.cs");
                return false;
            }
        }

    #endregion
    }
}
