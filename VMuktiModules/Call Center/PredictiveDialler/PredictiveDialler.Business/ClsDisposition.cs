using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using VMuktiAPI;

namespace PredictiveDialler.Business
{
    public class ClsDisposition : ClsBaseObject
    {
        #region Fields

        private int _ID = PredictiveDialler.Common.ClsConstants.NullInt;
        private string _DespositionName = PredictiveDialler.Common.ClsConstants.NullString;
        private string _Description = PredictiveDialler.Common.ClsConstants.NullString;
        private bool _IsActive = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private bool _IsDeleted = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private DateTime _CreatedDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private int _CreatedBy = PredictiveDialler.Common.ClsConstants.NullInt;
        private DateTime _ModifiedDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private int _ModifiedBy = PredictiveDialler.Common.ClsConstants.NullInt;

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string DespositionName
        {
            get { return _DespositionName; }
            set { _DespositionName = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

         public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }
        public int ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        

        #endregion
            
        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            DespositionName = GetString(row, "DespositionName");
            Description = GetString(row, "Description");
            IsActive = GetBool(row, "IsActive");
            IsDeleted = GetBool(row, "IsDeleted");
            CreatedDate = GetDateTime(row, "CreatedDate");
            CreatedBy = GetInt(row, "CreatedBy");            
            ModifiedDate = GetDateTime(row, "ModifiedDate");
            ModifiedBy = GetInt(row, "ModifiedBy");
           
            return base.MapData(row);
        }

        

        public static string GetZoneName(long LeadID)
        {
            try
            {
                return (new PredictiveDialler.DataAccess.ClsUserDataService().GetZoneName(LeadID));
            }
            catch (Exception ex)
            {
                return null;
                VMuktiHelper.ExceptionHandler(ex, "GetByGroupID()", "ClsDisposition.cs");
            }
        }

        //public static string GetDispositionName(long DispositionID)
        //{
        //    try
        //    {
        //        return (new Dialer_AutoProgressive.DataAccess.ClsUserDataService().GetDispositionName(DispositionID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        VMuktiHelper.ExceptionHandler(ex, "GetDispositionName()", "ClsDisposition.cs");
        //    }
        //}

        //public static string GetPhoneNo(long LeadID)
        //{
        //    try
        //    {
        //        return (new Dialer_AutoProgressive.DataAccess.ClsUserDataService().GetPhoneNo(LeadID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        VMuktiHelper.ExceptionHandler(ex, "GetPhoneNo()", "ClsDisposition.cs");
        //    }
        //}
    }
}
