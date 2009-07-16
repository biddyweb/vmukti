using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DashBoard.DataAccess.CampaignManagement
{
    public class ClsCampaignManagementDataService : ClsDataServiceBase
    {
        public ClsCampaignManagementDataService() : base() { }

        public ClsCampaignManagementDataService(IDbTransaction txn) : base(txn) { }

        public DataSet GetCampainNames(Int64 UserID)
        {
            return ExecuteDataSet("spGCampaignByUser", CommandType.StoredProcedure, CreateParameter("@pUserID", SqlDbType.BigInt, UserID, ParameterDirection.Input));
        }
        public DataSet GetCampaignTreatment(string CampaignName, string TreatmentName)
        {
            return ExecuteDataSet("select CampaignTreatment.CampaignID,CampaignTreatment.TreatmentID from CampaignTreatment,Campaign,Treatment where Campaign.Name='" + CampaignName + "'and Campaign.ID=CampaignTreatment.CampaignID and Treatment.ID=CampaignTreatment.TreatmentID and Treatment.TreatmentName='" + TreatmentName + "'", CommandType.Text, null);
        }
        public DataSet GetTreatment()
        {
            return ExecuteDataSet("Select TreatmentName from Treatment where Type='treatment' and IsDeleted<>1", CommandType.Text, null);
        }
        public DataSet GetUserList()
        {
            return ExecuteDataSet("Select DisplayName from UserInfo where IsDeleted='False' and IsActive='True'", CommandType.Text, null);
        }
        public void InsertCampaignUser(string CampaignName, string UserName)
        {
            ExecuteNonQuery("spAddCampaignUser", CreateParameter("@pCampaignName", SqlDbType.VarChar, CampaignName, ParameterDirection.Input), CreateParameter("@pUserName", SqlDbType.VarChar, UserName, ParameterDirection.Input));
        }
        public DataSet GetTreatmentCondition(string TreatmentName)
        {
            return ExecuteDataSet("spGTreatmentCondition", CommandType.StoredProcedure, CreateParameter("@pTreatmentID", SqlDbType.BigInt, -1, ParameterDirection.Input), CreateParameter("@pTreatmentName", SqlDbType.NVarChar, TreatmentName, ParameterDirection.Input));
        }
        public void InsertCampaignTreatment(string CampaignName, string TreatmentName)
        {
            ExecuteNonQuery("spAECampaignTreatment", CreateParameter("@pID", SqlDbType.BigInt, -1, ParameterDirection.Input), CreateParameter("@CampaignName", SqlDbType.NVarChar, CampaignName, ParameterDirection.Input), CreateParameter("@TreatmentName", SqlDbType.NVarChar, TreatmentName, ParameterDirection.Input));
        }
        public void DeleteCampaignTreatment(string CampaignName, string TreatmentName)
        {
            //ExecuteDataSet("Delete from campaignTreatment where CampaignID in ( select [ID] from Campaign where Name= 'CampaignName' ) and TreatmentID in (select ID from Treatment where TreatmentName ='TreatmentName')", CommandType.Text, null);
            //ExecuteDataSet("delete from campaigntreatment where campaignid=CampaignName and treatmentid=13 and =1");
            ExecuteNonQuery("spDCampaignTreatment", CreateParameter("@pCampaignName", SqlDbType.NVarChar, CampaignName, ParameterDirection.Input), CreateParameter("@pTreatmentName", SqlDbType.NVarChar, TreatmentName, ParameterDirection.Input));
        }

        public DataSet GetTreatment1(string CampaignName)
        {
            return ExecuteDataSet("select treatmentname from treatment,campaigntreatment,campaign where treatment.id=campaigntreatment.treatmentid and Campaign.Name = '" + CampaignName + "' and Campaign.ID = campaigntreatment.CampaignID", CommandType.Text, null);
        }
        public DataSet GetUsers(string CampaignName)
        {
            return ExecuteDataSet("select DisplayName from UserInfo,UserGroup,CampaignGroup,Campaign where Campaign.Name='" + CampaignName + "' and Campaign.ID=CampaignGroup.CampaignID and CampaignGroup.GroupID=UserGroup.GroupID and UserGroup.UserID=UserInfo.ID", CommandType.Text, null);
        }

        public void DeleteTreatment(string CName,string TName)
        {
            try
            {
                ExecuteNonQuery("spDelTreatment", CreateParameter("@CName", SqlDbType.NVarChar, CName), CreateParameter("@TName", SqlDbType.NVarChar, TName));
            }
            catch (Exception ex)
            { }
        }

        public void DeleteUser(string CName, string UName)
        {
            try
            {
                ExecuteNonQuery("spDelUser", CreateParameter("@CName", SqlDbType.NVarChar, CName), CreateParameter("@UName", SqlDbType.NVarChar, UName));
            }
            catch (Exception ex)
            { }
        }

        public DataSet GetAllDesposition()
        {
            try
            {
                return ExecuteDataSet("select ID,DespositionName from Disposition", CommandType.Text, null);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetCampaignLeads()
        {
            try
            {
                return ExecuteDataSet("spLeads", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                return null;
            }   
        }

        public DataSet GetAllCampaigns()
        {
            try
            {
                return ExecuteDataSet("select id,Name from campaign", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                return null;
            }   
        }

        public void AddCampaignUser(string strCamapign, string Actual)
        {
            try
            {
                ExecuteNonQuery("spAddCampaignUser", CreateParameter("@pCampaignName", SqlDbType.NVarChar, strCamapign), CreateParameter("@pUserName", SqlDbType.NVarChar, Actual));
            }
            catch (Exception ex)
            { }
        }
    }
}
