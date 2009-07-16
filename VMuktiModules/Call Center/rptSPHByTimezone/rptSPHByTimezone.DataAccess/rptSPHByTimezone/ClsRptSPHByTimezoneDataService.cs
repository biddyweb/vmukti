using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptSPHByTimezone.DataAccess
{
    public class ClsRptSPHByTimezone : ClsDataServiceBase
    {
        public ClsRptSPHByTimezone() : base() { }

        public ClsRptSPHByTimezone(IDbTransaction txn) : base(txn) { }

        public DataSet rptSPHByTimezone_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        public DataSet rptSPHByTimezone_GetAllListByCapmID(int CampaignID)
        {
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        public DataSet rptSPHByTimezone_GetCountByDisposition(int ListID,DateTime StartDate,DateTime EndDate)
        {
            return ExecuteDataSet("spReportSHPTimezone", CommandType.StoredProcedure, CreateParameter("@pListID", SqlDbType.BigInt, ListID), CreateParameter("@pdtStart", SqlDbType.DateTime, StartDate), CreateParameter("@pdtEnd", SqlDbType.DateTime, EndDate));
        }
    }
}
