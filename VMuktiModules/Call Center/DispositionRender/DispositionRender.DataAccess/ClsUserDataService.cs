using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using VMuktiAPI;
namespace DispositionRender.DataAccess
{
    public class ClsDispositionRenderDataService : ClsDataServiceBase
    {

        public ClsDispositionRenderDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDispositionRenderDataService(IDbTransaction txn) : base(txn) { }


        //public DataSet Disposition_GetAll(Int64 CampID)
        //{
        //    return ExecuteDataSet("Select UserInfo.*,Payroll.* from UserInfo left outer join Payroll on UserInfo.Id=Payroll.UserId where UserInfo.IsDeleted=0;", CommandType.Text, null);
        //}

        //Dispositio_GetAll() function to retrive data using spGDispositionRender store procedure
        public DataSet Disposition_GetAll(Int64 CampID)
        {
            try
            {
                return ExecuteDataSet("spGDispositionRender", CommandType.StoredProcedure, CreateParameter("@pCampID", SqlDbType.BigInt, CampID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsUserDataService.cs");
                return null;
            } 
        }

        public int Disposition_GetByName(string DispoName)
        {
            try
            {
                DataSet ds = ExecuteDataSet("select ID from Disposition where DespositionName='" + DispoName + "'", CommandType.Text, null);
                int id = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                return id;
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetByName()", "ClsUserDataService.cs");
                return 100;
            }
        }
        //public void User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
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

    }
}
