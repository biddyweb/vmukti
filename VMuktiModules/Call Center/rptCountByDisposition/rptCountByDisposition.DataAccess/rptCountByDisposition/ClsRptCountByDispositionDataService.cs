using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptCountByDisposition.DataAccess
{
    public class ClsRptCountByDispositionDataService : ClsDataServiceBase
    {
        public ClsRptCountByDispositionDataService() : base() { }

        public ClsRptCountByDispositionDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptCountByDisposition_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        public DataSet rptCountByDisposition_GetAllListByCapmID(int CampaignID)
        {
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        public DataSet rptCountByDisposition_GetCountByDisposition(int ListID, DateTime dtStartDate, DateTime dtEndDate)
        {
            //Access database using spReportLeadsCountByDisposition stored procedure
            return ExecuteDataSet("spReportLeadsCountByDisposition", CommandType.StoredProcedure, CreateParameter("@pListID", SqlDbType.BigInt, ListID), CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));
        }
    }
}
