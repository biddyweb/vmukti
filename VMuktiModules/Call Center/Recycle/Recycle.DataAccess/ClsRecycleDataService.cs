using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Recycle.DataAccess
{
    public class ClsRecycleDataService : ClsDataServiceBase
    {

        public ClsRecycleDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsRecycleDataService(IDbTransaction txn) : base(txn) { }


        //public DataSet User_GetAll()
        //{
        //    return ExecuteDataSet("Select UserInfo.*,Payroll.* from UserInfo left outer join Payroll on UserInfo.Id=Payroll.UserId where UserInfo.IsDeleted=0;", CommandType.Text, null);
        //}

        //public DataSet User_GetByID(int ID)
        //{
        //    return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        //public void User_Save(ref int ID, string displayame, int roleID, string firstName, string lastName, string eMail, string password, bool isActive, int byUserId, float ratePerHour, float overTimeRate, float doubleRatePerHour, float doubleOverTimeRate)
        //{
        //    SqlCommand cmd;
        //    ExecuteNonQuery(out cmd, "spAEUserInfoPayroll",
        //        CreateParameter("@pID", SqlDbType.Int, ID),
        //        CreateParameter("@pDisplayName", SqlDbType.NVarChar, displayame, 100),
        //        CreateParameter("@pRoleID", SqlDbType.BigInt, roleID),
        //        CreateParameter("@pFirstName", SqlDbType.NVarChar, firstName, 50),
        //        CreateParameter("@pLastName", SqlDbType.NVarChar, lastName, 50),
        //        CreateParameter("@pEMail", SqlDbType.NVarChar, eMail, 256),
        //        CreateParameter("@pPassword", SqlDbType.NVarChar, password, 50),
        //        CreateParameter("@pIsActive", SqlDbType.Bit, isActive),
        //        CreateParameter("@pByUserID", SqlDbType.BigInt, byUserId),
        //        CreateParameter("@pRatePerHour", SqlDbType.Float, ratePerHour),
        //        CreateParameter("@pOverTimeRate", SqlDbType.Float, overTimeRate),
        //        CreateParameter("@pDoubleRatePerHour", SqlDbType.Float, doubleRatePerHour),
        //        CreateParameter("@pDoubleOverTimeRate", SqlDbType.Float, doubleOverTimeRate));

        //    cmd.Dispose();
        //}

        //public void User_Delete(int ID)
        //{
        //    ExecuteNonQuery("spDUserInfoPayroll", CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        public DataSet Recycle_Leads(Int64 DispListID, Int64 ListID)
        {
            try
            {
                return ExecuteDataSet("spRecycle", CommandType.StoredProcedure, CreateParameter("@pDispList", SqlDbType.BigInt, DispListID), CreateParameter("@pListID", SqlDbType.BigInt, ListID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Recycle_Leads()", "ClsDataServiceBase.cs");   
                return null;
            }
        }

        public DataSet Disposition_GetAll(Int64 CampID)
        {
            try
            {
                return ExecuteDataSet("Select Disposition.ID,Disposition.DespositionName from Disposition,CampaignDespoList,DispListDisp where CampaignDespoList.DespositionListID=DispListDisp.DispositionListID and DispListDisp.DispositionID=Disposition.ID and CampaignDespolist.CampaignID=" + CampID + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsDataServiceBase.cs");   
                return null;
            }
        }

        public DataSet Campaign_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vCampaign;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Campaign_GetAll()", "ClsDataServiceBase.cs");   
                return null;
            }
        }

        public DataSet List_GetAll(Int64 CampID)
        {
            try
            {
                return ExecuteDataSet("Select CallingList.ID,CallingList.ListName from CallingList,CampaignCallingList where CallingList.ID = CampaignCallingList.ListID and CampaignCallingList.CampaignID=" + CampID + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "List_GetAll()", "ClsDataServiceBase.cs");   
                return null;
            }
        }
    }
}
