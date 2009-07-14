using System;
using ServerInfo.Common;
using System.Data;
using ServerInfo.DataAccess;
using VMuktiAPI;
namespace ServerInfo.Business
{
    public class ClsUser : ClsBaseObject
    {
        #region Fields

        private int  _ID = ServerInfo.Common.ClsConstants.NullInt;
	    private string _UserName = ServerInfo.Common.ClsConstants.NullString;
       
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value;}
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                UserName = GetString(row, "DisplayName");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsUser.cs");
                return false;
            }
        }

        #endregion 
    }
}
