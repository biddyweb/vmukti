using System;
using ServerInfo.Common;
using System.Data;
using ServerInfo.DataAccess;
using VMuktiAPI;
namespace ServerInfo.Business
{
    public class ClsServerInfo : ClsBaseObject
    {
        #region Fields

        private Int64 _ID = ServerInfo.Common.ClsConstants.NullLong;
        private string _ServerName = ServerInfo.Common.ClsConstants.NullString;
        private string _IPAddress = ServerInfo.Common.ClsConstants.NullString;
        private string _Location = ServerInfo.Common.ClsConstants.NullString;
        private string _ServerUserName = ServerInfo.Common.ClsConstants.NullString;
        private string _ServerPassWord = ServerInfo.Common.ClsConstants.NullString;
        private int _PortNumber = ServerInfo.Common.ClsConstants.NullInt;
        private DateTime _AddedDate = ServerInfo.Common.ClsConstants.NullDateTime;
        private int _AddedBy = ServerInfo.Common.ClsConstants.NullInt;
        private int _CreatedBy = ServerInfo.Common.ClsConstants.NullInt;

        #endregion 

        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value;}
        }

        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }

        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        public string ServerUserName
        {
            get { return _ServerUserName; }
            set { _ServerUserName = value; }
        }

        public string ServerPassWord
        {
            get { return _ServerPassWord; }
            set { _ServerPassWord = value; }
        }

        public int PortNumber
        {
            get { return _PortNumber; }
            set { _PortNumber = value; }
        }

        public int AddedBy
        {
            get { return _AddedBy; }
            set { _AddedBy = value; }
        }

        public DateTime AddedDate
        {
            get { return _AddedDate; }
            set { _AddedDate = value; }
        }

        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
      
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetLong(row, "ID");
                ServerName = GetString(row, "ServerName");
                IPAddress = GetString(row, "IPAddress");
                Location = GetString(row, "Location");
                ServerUserName = GetString(row, "ServerUserName");
                ServerPassWord = GetString(row, "ServerPassword");
                PortNumber = GetInt(row, "PortNumber");
                AddedDate = GetDateTime(row, "AddedDate");
                AddedBy = GetInt(row, "AddedBY");
                CreatedBy = GetInt(row, "CreatedBy");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsServerInfo.cs");
                return false;
            }
        }

        public static ClsServerInfo GetByServerInfoID(Int64 ID)
        {
            try
            {
                ClsServerInfo obj = new ClsServerInfo();
                DataSet ds = new ServerInfo.DataAccess.ClsServerInfoDataService().ServerInfo_GetByID(ID);

                if (!obj.MapData(ds.Tables[0])) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByServerInfoID()", "ClsServerInfo.cs");
                return null;
            }
        }

        public static void Delete(Int64 ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsServerInfo.cs");
             
            }
        }

        public static void Delete(Int64 ID, IDbTransaction txn)
        {
            try
            {
                new ServerInfo.DataAccess.ClsServerInfoDataService(txn).ServerInfo_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsServerInfo.cs");

            }
        }

        public void Delete()
        {
            try
            {
                Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsServerInfo.cs");

            }
        }

        public void Delete(IDbTransaction txn)
        {
            try
            {
                Delete(ID, txn);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsServerInfo.cs");

            }
        }

        public Int64 Save()
        {
            try
            {
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsServerInfo.cs");
                return 0;
            }
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                return (new ServerInfo.DataAccess.ClsServerInfoDataService(txn).ServerInfo_Save(_ID, _ServerName, _IPAddress, _Location, _ServerUserName, _ServerPassWord, _PortNumber, _AddedDate, _AddedBy, _CreatedBy));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsServerInfo.cs");
                return 0;
            }
        }

        #endregion 
    }
}
