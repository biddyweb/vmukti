using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptRemainingLeads.DataAccess
{
    public class ClsRptRemainingLeadsDataService : ClsDataServiceBase
    {
        public ClsRptRemainingLeadsDataService() : base() { }

        public ClsRptRemainingLeadsDataService(IDbTransaction txn) : base(txn) { }

       // function to get all campaign from RptRemainingLeads.DataAccess
        public DataSet rptRemainingLeads_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        // function to get all List of campaign related to campaignID from RptRemainingLeads.DataAccess
        public DataSet rptRemainingLeads_GetAllListByCapmID(int CampaignID)
        {
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        // function to get all data from spReportRemainingLeads store procedure of RptRemainingLeads.DataAccess
        public DataSet rptRemainingLeads_GetCountByDisposition(int ListID,DateTime strtDate, DateTime endDate)
        {
            return ExecuteDataSet("spReportRemainingLeads", CommandType.StoredProcedure, CreateParameter("@pListID", SqlDbType.BigInt, ListID),CreateParameter("@pdtStart",SqlDbType.DateTime,strtDate),CreateParameter("@pdtEnd",SqlDbType.DateTime,endDate));
        }
    }
}
