using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptUserAudit.DataAccess
{
    public class ClsRptUserAudit : ClsDataServiceBase
    {
        public ClsRptUserAudit() : base() { }

        public ClsRptUserAudit(IDbTransaction txn) : base(txn) { }

        public DataSet rptUserAudit_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        public DataSet rptUserAudit_GetAllListByCapmID(int CampaignID)
        {
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        public DataSet rptUserAudit_GetAllUserByListID()
        {
            //get user name and id from database table vUserInfo... 
            return ExecuteDataSet("select DisplayName, ID From vUserInfo", CommandType.Text, null);
        }

        public DataSet rptUserAudit_GetUserAudit(int UserID)
        {
            //get user information from database stored procedure spReportUserAudit...
            return ExecuteDataSet("spReportUserAudit", CommandType.StoredProcedure, CreateParameter("@pUserID", SqlDbType.BigInt, UserID));
        }
    }
}
