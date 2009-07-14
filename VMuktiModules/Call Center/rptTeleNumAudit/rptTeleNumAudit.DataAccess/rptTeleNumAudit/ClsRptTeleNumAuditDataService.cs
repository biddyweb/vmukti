using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptTeleNumAudit.DataAccess
{
    public class ClsRptTeleNumAudit : ClsDataServiceBase
    {
        public ClsRptTeleNumAudit() : base() { }

        public ClsRptTeleNumAudit(IDbTransaction txn) : base(txn) { }

        public DataSet rptTeleNumAudit_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        public DataSet rptTeleNumAudit_GetAllListByCapmID(int CampaignID)
        {
            //Access database table CallingList, CampaignCallingList
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        public DataSet rptTeleNumAudit_GetAllNumberByListID(int ListID)
        {
            //Access database table Call,Leads
            return ExecuteDataSet("Select Distinct(Leads.PhoneNo), Leads.ID from Leads, [Call] where Call.LeadID=Leads.ID and Leads.ListID = " + ListID.ToString(), CommandType.Text, null);
        }

        public DataSet rptTeleNumAudit_GetTeleNumAudit(Int64 TelePhoneNumber)
        {
            //Access Datbase using spReportTelephoneAudit stored procedure
            return ExecuteDataSet("spReportTelephoneAudit", CommandType.StoredProcedure, CreateParameter("@pTelephoneNumber", SqlDbType.BigInt, TelePhoneNumber));
        }
    }
}
