using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace rptActiveAgent.DataAccess
   
{
    public class ClsRptActiveAgentDataService : ClsDataServiceBase
    {
        //public ClsRptActiveAgentDataService() : base() { }

        //public ClsRptActiveAgentDataService(IDbTransaction txn) : base(txn) { }

        //public DataSet rptActiveAgent_GetHistoryDataOfDates()
        //{
        //    return ExecuteDataSet("spDialTable", CommandType.StoredProcedure);
            
        //}

        public ClsRptActiveAgentDataService() : base() { }

        public ClsRptActiveAgentDataService(IDbTransaction txn) : base(txn) { }

        string strClientConnectionString = string.Empty;

        public DataTable rptActiveAgent_GetHistoryDataOfDates()
        {
          //  strClientConnectionString = AppDomain.CurrentDomain.BaseDirectory + "\\VMukti.sdf";
           // strClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\VMukti.sdf";
           // System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            strClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\VMukti.sdf";
            SqlCeConnection ce = new SqlCeConnection(strClientConnectionString);
            ce.Open();
            DataTable dt4ActiveRecord = new DataTable();
            // string strSelectCommand = "Select UserName  as AgentName , CampaignID as Campaign , GroupID as Group , RoleID as Role , Starttime as SessionStartTime From AgentStatusInfo";

            string strSelectCommand = "Select UserName as AgentName,CampaignName as Campaign,GroupName as [Group],RoleName as Role, starttime as SessionStart From AgentStatusInfo"; 
            
            SqlCeDataAdapter da4ActiveAgent = new SqlCeDataAdapter(strSelectCommand, ce);
            da4ActiveAgent.Fill(dt4ActiveRecord);
            return dt4ActiveRecord;
            
         //   return ExecuteDataSet("spDialTable", CommandType.StoredProcedure, CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));

        }
    }
}
