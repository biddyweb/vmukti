using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DashBoard.Common;
using DashBoard.DataAccess.CampaignManagement;

namespace DashBoard.Business.CampaignManagement
{
    public class ClsCampaignManagement : ClsBaseObjects 
    {
         #region Fields

        private int _ID = ClsConstants.NullInt;
        private string _TreatmentName = ClsConstants.NullString;
        private string _Description = ClsConstants.NullString;
        private string _Type = ClsConstants.NullString;
        private bool _IsInclude = false;
        private int _UserID = ClsConstants.NullInt;

        #endregion       

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string TreatmentName
        {
            get { return _TreatmentName; }
            set { _TreatmentName = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public bool IsInclude
        {
            get { return _IsInclude; }
            set { _IsInclude = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }


        #endregion

        public ClsCampaignManagement()
        {
        }

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            TreatmentName = GetString(row, "TreatmentName");
            Description = GetString(row, "Description");
            Type = GetString(row, "Type");
            IsInclude = GetBool(row, "IsInclude");
            UserID = GetInt(row, "ModifiedBy");
            return base.MapData(row);
        }
        public static DataSet GetCampainNames(Int64  UserID)
        {
           return GetCampainNames(null,UserID);
        }
        public static DataSet GetCampainNames(IDbTransaction txn, Int64 UserID)
        {
            //ClsCampaignManagement obj = new ClsCampaignManagement();
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetCampainNames(UserID));
            //new AssignTreatment.DataAccess.ClsAssignTreatmentDataService(txn).GetCampainNames();
        }
        public static DataSet GetUserList()
        {
            return GetUserList(null);
        }
        public static DataSet GetTreatment()
        {
            return GetTreatment(null);
        }
        public static DataSet GetTreatment(IDbTransaction txn)
        {
            //ClsCampaignManagement obj = new ClsCampaignManagement();
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetTreatment());
            //new AssignTreatment.DataAccess.ClsAssignTreatmentDataService(txn).GetCampainNames();
        }
        public static DataSet GetCampaignTreatment(string CampaignName,string TreatmentName)
        {
            return GetCampaignTreatment(null,CampaignName,TreatmentName);
        }
        public static DataSet GetCampaignTreatment(IDbTransaction txn, string CampaignName, string TreatmentName)
        {
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetCampaignTreatment(CampaignName, TreatmentName));
        }
        public static DataSet GetUsers(string CampaignName)
        {
            return GetUsers(null,CampaignName);

        }
        public static DataSet GetUsers(IDbTransaction txn,string CampaignName)
        {
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetUsers(CampaignName));
        }
        public static DataSet GetUserList(IDbTransaction txn)
        {
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetUserList());
        }

        public static DataSet GetTreatmentCondition(string TreatmentName)
        {
            return GetTreatmentCondition(null, TreatmentName);
        }
        
        public static DataSet GetTreatmentCondition(IDbTransaction txn, string TreatmentName)
        {
            //ClsCampaignManagement obj = new ClsCampaignManagement();
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetTreatmentCondition(TreatmentName));
            //new AssignTreatment.DataAccess.ClsAssignTreatmentDataService(txn).GetCampainNames();
        }
        public static void InsertCampaignUser(string CampaignName, string UserName)
        {
            InsertCampaignUser(CampaignName, UserName);
        }
        //public static void InsertCampaignUser(string CampaignName, string UserName)
        //{
        //    new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).InsertCampaignUser(CampaignName, UserName);
        //}

        public static void InsertCampaignTreatment(string CampaignName, string TreatmentName)
        {
            InsertCampaignTreatment(null, CampaignName, TreatmentName);
        }

        public static void InsertCampaignTreatment(IDbTransaction txn,string CampaignName, string TreatmentName)
        {
            //ClsCampaignManagement obj = new ClsCampaignManagement();
            new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).InsertCampaignTreatment(CampaignName, TreatmentName);
            //new AssignTreatment.DataAccess.ClsAssignTreatmentDataService(txn).GetCampainNames();
        }
        //DeleteCampaignTreatment
        public static void DeleteCampaignTreatment(string CampaignName, string TreatmentName)
        {
            DeleteCampaignTreatment(null, CampaignName, TreatmentName);
            //InsertCampaignTreatment(null, CampaignName, TreatmentName);
        }

        public static void DeleteCampaignTreatment(IDbTransaction txn, string CampaignName, string TreatmentName)
        {
            //ClsCampaignManagement obj = new ClsCampaignManagement();
            new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).DeleteCampaignTreatment(CampaignName, TreatmentName);
            //new AssignTreatment.DataAccess.ClsAssignTreatmentDataService(txn).GetCampainNames();
        }


        public static DataSet GetTreatment1(string CampaignName)
        {
            return GetTreatment1(null, CampaignName);
        }

        public static DataSet GetTreatment1(IDbTransaction txn,string CampaignName)
        {
            //ClsCampaignManagement obj = new ClsCampaignManagement();
            return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService(txn).GetTreatment1(CampaignName));
        }

        public static void DeleteTreatment(string CName,string TName)
        {
            try
            {
                new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService().DeleteTreatment(CName, TName);
            }
            catch (Exception ex)
            { }
        }

        public static void DeleteUser(string CName, string UName)
        {
            try
            {
                new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService().DeleteUser(CName, UName);
            }
            catch (Exception ex)
            { }
            
        }

        public static DataSet GetAllDesposition()
        {
            try
            {
                return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService().GetAllDesposition());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataSet GetCampaignLeads()
        {
            try
            {
                return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService().GetCampaignLeads());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataSet GetAllCampains()
        {
            try
            {
                return (new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService().GetAllCampaigns());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void AddCampaignUser(string strCamapign, string Actual)
        {
            try
            {              
                new DashBoard.DataAccess.CampaignManagement.ClsCampaignManagementDataService().AddCampaignUser(strCamapign, Actual);
            }
            catch (Exception ex)
            {                
            }
        }
    }
}
