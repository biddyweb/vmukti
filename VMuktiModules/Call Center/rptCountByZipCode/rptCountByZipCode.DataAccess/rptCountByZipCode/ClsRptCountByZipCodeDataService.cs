using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptCountByZipCode.DataAccess
{
    public class ClsRptCountByZipCodeDataService : ClsDataServiceBase
    {
        public ClsRptCountByZipCodeDataService() : base() { }

        public ClsRptCountByZipCodeDataService(IDbTransaction txn) : base(txn) { }

        //retrive data from the database related to campaign

        public DataSet rptCountByZipCode_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }
        //retrive data from the database related to campaign

        public DataSet rptCountByZipCode_GetAllListByCapmID(int CampaignID)
        {
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }
        //retrive data from the store procedure

        public DataSet rptCountByZipCode_GetCountByZipCode(int ListID,DateTime startDate, DateTime endDate)
        {
            return ExecuteDataSet("spReportLeadsCountByZipCode", CommandType.StoredProcedure, CreateParameter("@pListID", SqlDbType.BigInt, ListID), CreateParameter("@pdtStart", SqlDbType.DateTime, startDate), CreateParameter("@pdtEnd", SqlDbType.DateTime, endDate));
        }
    }
}
