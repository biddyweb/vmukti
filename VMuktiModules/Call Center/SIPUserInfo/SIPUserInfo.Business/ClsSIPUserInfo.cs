using System;
using SIPUserInfo.Common;
using System.Data;
using SIPUserInfo.DataAccess;
using VMuktiAPI;
namespace SIPUserInfo.Business
{
    public class ClsSIPUserInfo : ClsBaseObject
    {
        #region Fields

        private int  _ID = SIPUserInfo.Common.ClsConstants.NullInt;
        private int _SIPID = SIPUserInfo.Common.ClsConstants.NullInt;
        private int _SIPPass = SIPUserInfo.Common.ClsConstants.NullInt;
        private int _ActiveServerID = SIPUserInfo.Common.ClsConstants.NullInt;
        private int _CreatedBy = SIPUserInfo.Common.ClsConstants.NullInt;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public int SIPID
        {
            get { return _SIPID; }
            set { _SIPID = value; }
        }

        public int SIPPass
        {
            get { return _SIPPass; }
            set { _SIPPass = value; }
        }

        public int ActiveServerID
        {
            get { return _ActiveServerID; }
            set { _ActiveServerID = value; }
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
                ID = GetInt(row, "ID");
                SIPID = GetInt(row, "SIPID");
                SIPPass = GetInt(row, "SIPPass");
                ActiveServerID = GetInt(row, "ActiveServerID");
                CreatedBy = GetInt(row, "CreatedBy");

                return base.MapData(row);
            }
            catch (Exception ex)
            {

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsSIPUserInfo.cs");
                return false;
            }
        }

        public static ClsSIPUserInfo GetByID(int ID)
        {
            try
            {
                ClsSIPUserInfo obj = new ClsSIPUserInfo();
                DataSet ds = new SIPUserInfo.DataAccess.ClsSIPUserInfoDataService().SIPUserInfo_GetByID(ID);

                if (!obj.MapData(ds.Tables[0])) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByID()", "ClsSIPUserInfo.cs");
                return null;
            }
        }

        public static void Delete(int ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete(int)", "ClsSIPUserInfo.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {
                new SIPUserInfo.DataAccess.ClsSIPUserInfoDataService(txn).SIPUserInfo_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete(int,IDbTransaction)", "ClsSIPUserInfo.cs");

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsSIPUserInfo.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction)", "ClsSIPUserInfo.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsSIPUserInfo.cs");
                return 0;
            }
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                return (new SIPUserInfo.DataAccess.ClsSIPUserInfoDataService(txn).SIPUserInfo_Save(ref _ID, _SIPID, _SIPPass, _ActiveServerID, _CreatedBy));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save(IDbTransaction)", "ClsSIPUserInfo.cs");
                return 0;
            }
        }

        #endregion 
    }
}
