using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace DashBoard.DataAccess.CPH
{
    public class ClsCallsPerHourDataService : ClsDataServiceBase 
    {

        //public DataSet GetTotalCalls()
        //{
        //    try
        //    {
        //        string strClientConnectionString = string.Empty;
        //        strClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "RealTimeCampaignValues.sdf";

        //        SqlCeConnection ceCon = new SqlCeConnection(strClientConnectionString);
        //        ceCon.Open();

                
        //        SqlCeDataAdapter sqlceDa = new SqlCeDataAdapter();


        //        return ExecuteDataSet("select CampaignName,totalleads from campaignleads where id in (select campaignid from activeagentcalls where status='called')", CommandType.Text, null);

        //        //SqlCommand cmd;
        //        //int ReturnCalls = 0;               

        //        //ExecuteNonQuery(out cmd, "spGetCallsPerHour", CreateParameter("@calls", SqlDbType.Int, -1, ParameterDirection.InputOutput));
        //        //cmd.Dispose();
        //        //ReturnCalls = Convert.ToInt32(cmd.Parameters["@calls"].Value.ToString());
        //        //return ReturnCalls;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public DataSet GetDuration()
        //{
        //    try
        //    { 
        //        return ExecuteDataSet("select count(*) ,DurationInSecond from Call", CommandType.Text, null);
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
