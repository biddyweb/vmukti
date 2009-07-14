using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace DashBoard.DataAccess.CampaignManagement
{
    public class ClsAssignTreatmentDataService : ClsDataServiceBase
    {
        public ClsAssignTreatmentDataService() : base() { }
        public ClsAssignTreatmentDataService(IDbTransaction txn) : base(txn) { }
        public DataSet GetCampainNames(Int64 UserID)
        {
            return ExecuteDataSet("spGCampaignByUser", CommandType.StoredProcedure ,CreateParameter("@pUserID",SqlDbType.BigInt ,UserID ,ParameterDirection.Input ));
        }
        public DataSet CountGroup(string CampaignName)
        {
            return ExecuteDataSet("select * from CampaignGroup,Campaign where Campaign.Name='"+CampaignName+"' and CampaignGroup.CampaignID=Campaign.ID", CommandType.Text, null);
        }
        public DataSet GetGroup()
        {
            return ExecuteDataSet("select GroupName from [Group] where IsDeleted='False' and IsActive='True'", CommandType.Text, null);
        }
        public DataSet GetCampaignTreatment(string CampaignName,string TreatmentName)
        {
            return ExecuteDataSet("select CampaignTreatment.CampaignID,CampaignTreatment.TreatmentID from CampaignTreatment,Campaign,Treatment where Campaign.Name='" + CampaignName + "'and Campaign.ID=CampaignTreatment.CampaignID and Treatment.ID=CampaignTreatment.TreatmentID and Treatment.TreatmentName='" + TreatmentName + "'", CommandType.Text, null);
        }
        public DataSet GetTreatment()
        {
            return ExecuteDataSet("Select TreatmentName from Treatment where Type='TreatmentOn-Field' and IsDeleted<>1", CommandType.Text, null);
        }
        public DataSet GetUserList()
        {
            return ExecuteDataSet("Select DisplayName from UserInfo where IsDeleted='False' and IsActive='True'",CommandType.Text,null);
        }
        public void InsertCampaignGroup(string CampaignName, string GroupName)
        {
            ExecuteNonQuery("spGCampaignGroup",CreateParameter("@pCampaignName", SqlDbType.VarChar, CampaignName, ParameterDirection.Input), CreateParameter("@pGroupName", SqlDbType.VarChar, GroupName, ParameterDirection.Input));
        }
        public void InsertCampaignUser(string CampaignName, string UserName)
        {
            ExecuteNonQuery("spAECampaignUser", CreateParameter("@pID", SqlDbType.BigInt, -1, ParameterDirection.Input), CreateParameter("@pCampaignName", SqlDbType.VarChar, CampaignName, ParameterDirection.Input), CreateParameter("@pUserName", SqlDbType.VarChar, UserName, ParameterDirection.Input));
        }
        public DataSet GetTreatmentCondition(string TreatmentName)
        {
            return ExecuteDataSet("spGTreatmentCondition", CommandType.StoredProcedure, CreateParameter("@pTreatmentID", SqlDbType.BigInt, -1, ParameterDirection.Input), CreateParameter("@pTreatmentName", SqlDbType.NVarChar, TreatmentName, ParameterDirection.Input));
        }
        public void InsertCampaignTreatment(string CampaignName, string TreatmentName)
        {
            ExecuteNonQuery("spAECampaignTreatment", CreateParameter("@pID", SqlDbType.BigInt , -1 , ParameterDirection.Input  ),CreateParameter("@CampaignName", SqlDbType.NVarChar, CampaignName, ParameterDirection.Input), CreateParameter("@TreatmentName", SqlDbType.NVarChar, TreatmentName, ParameterDirection.Input));
        }
        public void DeleteCampaignTreatment(string CampaignName, string TreatmentName)
        {
            //ExecuteDataSet("Delete from campaignTreatment where CampaignID in ( select [ID] from Campaign where Name= 'CampaignName' ) and TreatmentID in (select ID from Treatment where TreatmentName ='TreatmentName')", CommandType.Text, null);
            //ExecuteDataSet("delete from campaigntreatment where campaignid=CampaignName and treatmentid=13 and =1");
            ExecuteNonQuery("spDCampaignTreatment", CreateParameter("@pCampaignName", SqlDbType.NVarChar, CampaignName, ParameterDirection.Input), CreateParameter("@pTreatmentName", SqlDbType.NVarChar, TreatmentName, ParameterDirection.Input));
        }

        public DataSet GetTreatment1( string CampaignName)
        {
            return ExecuteDataSet("select treatmentname from treatment,campaigntreatment,campaign where treatment.id=campaigntreatment.treatmentid and Campaign.Name = '" + CampaignName + "' and Campaign.ID = campaigntreatment.CampaignID", CommandType.Text, null);
        }
        public DataSet GetUsers(string CampaignName)
        {
            return ExecuteDataSet("select DisplayName from UserInfo,UserGroup,CampaignGroup,Campaign where Campaign.Name='" + CampaignName + "' and Campaign.ID=CampaignGroup.CampaignID and CampaignGroup.GroupID=UserGroup.GroupID and UserGroup.UserID=UserInfo.ID", CommandType.Text, null);
        }
        public DataSet GetGroup(string CampaignName)
        {
            return ExecuteDataSet("select [Group].GroupName from [Group],CampaignGroup,Campaign where [Group].ID=CampaignGroup.GroupID and CampaignGroup.CampaignID=Campaign.ID and Campaign.Name='"+CampaignName+"'", CommandType.Text, null);
        }

        public void DeleteTreatment(string CName, string TName)
        {
            try
            {
                ExecuteNonQuery("spDelTreatment", CreateParameter("@CName", SqlDbType.NVarChar, CName), CreateParameter("@TName", SqlDbType.NVarChar, TName));
            }
            catch (Exception ex)
            {
            }
        }

        public void DeleteUser(string CName, string UName)
        {
            try
            {
                ExecuteNonQuery("spDelUser", CreateParameter("@CName", SqlDbType.NVarChar, CName), CreateParameter("@UName", SqlDbType.NVarChar, UName));
            }
            catch (Exception ex)
            {
            }
        }
        public void DeleteGroup(string CName, string UName)
        {
            try
            {
                ExecuteNonQuery("spDelGroup",CreateParameter("@CName", SqlDbType.NVarChar, CName), CreateParameter("@UName", SqlDbType.NVarChar, UName));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
