using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptCountByState.DataAccess
{
    public class ClsRptCountByStateDataService : ClsDataServiceBase
    {
        public ClsRptCountByStateDataService() : base() { }

        public ClsRptCountByStateDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptCountByState_GetAllCampaign()
        {
            //Access Database table vCampaign
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        public DataSet rptCountByState_GetAllListByCapmID(int CampaignID)
        {
            //Access Database table CallingList, CampaignCallingList..
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        public DataSet rptCountByState_GetCountByState(int ListID, DateTime dtStartDate, DateTime dtEndDate)
        {
            //Access Database using spReportLeadsCountByState stored procedure..
            return ExecuteDataSet("spReportLeadsCountByState", CommandType.StoredProcedure, CreateParameter("@pListID", SqlDbType.BigInt, ListID), CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));
        }
    }
}
