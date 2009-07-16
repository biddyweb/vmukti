using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Audio.DataAccess
{
    public class ClsUserDataService 
    { 
        SqlConnection serverConnection = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
        public void fncInsertIntoCallTable(long leadID, DateTime calledDate, DateTime startDate, DateTime startTime, long durationInSecond, long despositionID, long campaingnID, long confID, string callNote, bool isDNC, bool isGlobal, long userID, string RecordedFileName)
        {
            try
            {
                serverConnection.Open();
                SqlCommand dCmd = new SqlCommand("spAECall", serverConnection);
                dCmd.CommandType = CommandType.StoredProcedure;
                dCmd.Parameters.AddWithValue("@pID", -1);
                dCmd.Parameters.AddWithValue("@pLeadID", leadID);
                dCmd.Parameters.AddWithValue("@pCalledDate", calledDate);
                dCmd.Parameters.AddWithValue("@pStartDate", startDate);
                dCmd.Parameters.AddWithValue("@pStartTime", startTime);
                dCmd.Parameters.AddWithValue("@pDurationInSecond", durationInSecond);
                dCmd.Parameters.AddWithValue("@pDespositionID", despositionID);
                dCmd.Parameters.AddWithValue("@pCampaignID", campaingnID);
                dCmd.Parameters.AddWithValue("@pConfID", confID);
                dCmd.Parameters.AddWithValue("@pCallNote", callNote);
                dCmd.Parameters.AddWithValue("@pIsDNC", isDNC);
                dCmd.Parameters.AddWithValue("@pIsGlobal", isGlobal);
                dCmd.Parameters.AddWithValue("@pUserID", userID);
                dCmd.Parameters.AddWithValue("@pRecordedFileName", RecordedFileName);
                dCmd.Parameters.AddWithValue("@pCallID", -1);
                dCmd.Parameters.AddWithValue("@pCallBackTime", Convert.ToDateTime("05/05/2005"));
                dCmd.Parameters.AddWithValue("@pSetLeadStatusTo", "Called");

                dCmd.ExecuteNonQuery();
                dCmd.Dispose();
                serverConnection.Close();

                //long parCallID = -1;
                //SqlCommand cmd;
                //ExecuteDataSet(out cmd, "spAECall",
                //    CreateParameter("@pID", SqlDbType.Int, -1),
                //    CreateParameter("@pLeadID", SqlDbType.BigInt, leadID),
                //    CreateParameter("@pCalledDate", SqlDbType.DateTime, calledDate),
                //    CreateParameter("@pStartDate", SqlDbType.DateTime, startDate),
                //    CreateParameter("@pStartTime", SqlDbType.DateTime, startTime),
                //    CreateParameter("@pDurationInSecond", SqlDbType.BigInt, durationInSecond),
                //    CreateParameter("@pDespositionID", SqlDbType.BigInt, despositionID),
                //    CreateParameter("@pCampaignID", SqlDbType.BigInt, campaingnID),
                //    CreateParameter("@pConfID", SqlDbType.BigInt, confID),
                //    CreateParameter("@pCallNote", SqlDbType.NVarChar, callNote),
                //    CreateParameter("@pIsDNC", SqlDbType.Bit, isDNC),
                //    CreateParameter("@pIsGlobal", SqlDbType.Bit, isGlobal),
                //    CreateParameter("@pUserID", SqlDbType.BigInt, userID),
                //    CreateParameter("@pRecordedFileName", SqlDbType.NVarChar, RecordedFileName),
                //    CreateParameter("@pCallID", SqlDbType.BigInt, -1, ParameterDirection.Output),
                //    CreateParameter("@pCallBackTime", SqlDbType.DateTime, Convert.ToDateTime("05/05/2005"), ParameterDirection.Output),
                //    CreateParameter("@pSetLeadStatusTo", SqlDbType.NVarChar, "Called", 30, ParameterDirection.Output));

                //parCallID = long.Parse(cmd.Parameters["@pCallID"].Value.ToString());
                //cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertIntoCallTable()", "Audio.DataAccess--ClsUserDataService.cs");
            }
        }

        public int FncGetTalkTime(int UserId)
        {
            try
            {
                string str = "SELECT SUM(DurationInSecond) FROM Call GROUP BY GeneratedBy HAVING GeneratedBy='" + UserId + "'";
                System.Data.SqlClient.SqlCommand cmd = new SqlCommand(str, serverConnection);
                serverConnection.Open();
                object i = cmd.ExecuteScalar();
                serverConnection.Close();
                if (i != null)
                {
                    return int.Parse(i.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetTalkTime()", "Audio.DataAccess--ClsUserDataService.cs");
                return -1;
            }
        }
    }
}
