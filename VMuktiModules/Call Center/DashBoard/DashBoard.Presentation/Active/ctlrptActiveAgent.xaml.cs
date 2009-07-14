using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using VMuktiService;
using System.Data;
using Microsoft.Reporting.WinForms;
using System.Data.SqlServerCe;
using VMuktiAPI;
using Microsoft.Synchronization.Data.SqlServerCe;
using Microsoft.Synchronization.Data;

using System.IO;
using System.Reflection;
using System.Timers;
using DashBoard.Presentation.Active;
using DashBoard.Business.WCF_Services;
using System.Windows.Input; 
using System.Windows.Threading;
using System.Collections;


namespace DashBoard.Presentation.Active
{
    /// <summary>
    /// Interaction logic for ctlChat.xaml
    /// </summary>
    /// 

    ////public enum ModulePermissions
    ////{
    ////    Add = 0,
    ////    Edit = 1,
    ////    Delete = 2,
    ////    View = 3
    ////}

    public partial class ctlrptActiveAgent : UserControl
    {
        object objNetTcpActiveAgent = new NetP2PBootStrapActiveAgentReportDelegates();
        INetP2PBootStrapActiveAgentReportChannel channelNetTcp;
        string strUri;
        
        ReportDataSource rds = new ReportDataSource();
        System.Threading.Thread tHostActiveAgent = null;
        string ConnectionString;
      
        public delegate void delRefreshReport(DataSet ds);
        public delRefreshReport objRefreshReport;

        public delegate void delBargConf(string strConfNo);
        public delBargConf objBargeConf;

        public delegate void delBargeHangup();
        public delBargeHangup objBargeHangup;

        string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "RealTimeCampaignValues.sdf";
        SqlCeConnection LocalSQLConn = null;

        DashBoard.Business.Audio.RTCAudio rAudio = null;
        bool isBarging = false;

        public delegate void delUpdateStatistics(int LoggedIn, int AgentsInCall, int Waitting, int ringing, int StoppedDialing);
        public event delUpdateStatistics entUpdateStatistics;
        string CampName = "";
        
        Hashtable hashConfNumber = new Hashtable();

        
        public ctlrptActiveAgent()
        {
            try
            {
                InitializeComponent();

                rAudio = new DashBoard.Business.Audio.RTCAudio("1110", "1110", VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP.ToString());
                tHostActiveAgent = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostActiveAgentService));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" +VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveAgentReport");
                lstParams.Add("P2PActiveAgentMesh");
                tHostActiveAgent.Start(lstParams);
                objRefreshReport = new delRefreshReport(objdelRefreshReport);
                objBargeConf = new delBargConf(BargeConf);
                objBargeHangup = new delBargeHangup(BargeHangup);
                ConnectionString = VMuktiAPI.VMuktiInfo.MainConnectionString;

                SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                LocalSQLConn = new SqlCeConnection();
                LocalSQLConn.ConnectionString = ClientConnectionString;
                LocalSQLConn.Open();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent", "ctlrptActiveAgent.xaml.cs");
            }
        }

        public void hostActiveAgentService(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUri = lstTempObj[0].ToString();

                NetPeerClient npcActiveAgent = new NetPeerClient();
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcJoin(ctlrptActiveAgent_EntsvcJoin);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcGetAgentInfo += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcGetAgentInfo(ctlrptActiveAgent_EntsvcGetAgentInfo);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcSetAgentInfo += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcSetAgentInfo(ctlrptActiveAgent_EntsvcSetAgentInfo);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcSetAgentStatus += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcSetAgentStatus(ctlrptActiveAgent_EntsvcSetAgentStatus);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcBargeRequest += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcBargeRequest(ctlrptActiveAgent_EntsvcBargeRequest);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcBargeReply += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcBargeReply(ctlrptActiveAgent_EntsvcBargeReply);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcHangUp += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcHangUp(ctlrptActiveAgent_EntsvcHangUp);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcUnJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcUnJoin(ctlrptActiveAgent_EntsvcUnJoin);

                channelNetTcp = (INetP2PBootStrapActiveAgentReportChannel)npcActiveAgent.OpenClient<INetP2PBootStrapActiveAgentReportChannel>(strUri, lstTempObj[1].ToString(), ref objNetTcpActiveAgent);
                channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString());
                
                channelNetTcp.svcGetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostActiveAgentService()", "ctlrptActiveAgent.xaml.cs");
                if (ex.InnerException != null && ex.InnerException.Message!="")
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex.InnerException, "hostActiveAgentService()--Inner", "ctlrptActiveAgent.xaml.cs");
                }
            }
        }

        public void SetCampaign(string Name)
        {
            CampName = Name;
        }

        #region WCF Events for ActiveAgent

        void ctlrptActiveAgent_EntsvcJoin(string uName, string campaignId)
        {
            try
            {                
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uName)
                {
                    channelNetTcp.svcGetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcJoin()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcGetAgentInfo(string uNameHost)
        {
            try
            {
                if (uNameHost != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    //channelNetTcp.svcSetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString(), "", VMuktiAPI.VMuktiInfo.CurrentPeer.GroupName, "90897956","00:00:00");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcGetAgentInfo()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcSetAgentInfo(string uName, string campaignId, string status, string groupName, string phoneNo, string CallDuration, bool isPredictive)
        {
            try
            {
                string SelQuery = "select count(*) from ActiveAgentCalls where uName='" + uName + "'";
                OpenConnection();
                SqlCeDataAdapter adp = new SqlCeDataAdapter(SelQuery, LocalSQLConn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    string InsQuery = string.Empty;
                    SqlCeCommand cmd = null;

                    if (isPredictive)
                    {
                        if (string.Compare(phoneNo, "NotRegisterd") == 0)
                        {

                            InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd1','" + CallDuration + "')";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            cmd.ExecuteNonQuery();

                            InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd2','" + CallDuration + "')";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','" + phoneNo + "','" + CallDuration + "')";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            cmd.ExecuteNonQuery();

                            InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','ready','" + groupName + "','NotRegisterd2','" + CallDuration + "')";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','" + phoneNo + "','" + CallDuration + "')";
                        cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                        cmd.ExecuteNonQuery();
                    }

                    #region for testing purpose

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd3','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd4','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd5','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd6','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd7','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd8','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd9','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd10','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd11','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd12','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd13','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd14','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd15','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd16','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd17','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd18','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd19','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd20','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd21','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd22','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd23','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd24','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd25','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd26','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd27','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    //InsQuery = "insert into ActiveAgentCalls (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','NotRegisterd28','" + CallDuration + "')";
                    //cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    //cmd.ExecuteNonQuery();

                    #endregion

                    //System.Threading.Thread.Sleep(1000);
                    string SelQueryActiveAgent = "select uName,Campaign_Id,Group_Name,Phone_No,Status,callDuration from ActiveAgentCalls";
                    DataSet dsActiveAgent = new DataSet();
                    SqlCeDataAdapter adpActiveAgent = new SqlCeDataAdapter(SelQueryActiveAgent, LocalSQLConn);
                    adpActiveAgent.Fill(dsActiveAgent);

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport, dsActiveAgent);
                }
                else
                {
                    string SqlUpdate = "update ActiveAgentCalls set Campaign_Id='" + campaignId + "',Status='" + status + "',Group_Name='" + groupName + "',Phone_No='" + phoneNo + "',callDuration='" + CallDuration + "' where uName='" + uName + "'";
                    SqlCeCommand cmd = new SqlCeCommand(SqlUpdate, LocalSQLConn);
                    cmd.ExecuteNonQuery();

                    string SelQueryActiveAgent = "select uName,Campaign_Id,Group_Name,Phone_No,Status,callDuration from ActiveAgentCalls";
                    DataSet dsActiveAgent = new DataSet();
                    SqlCeDataAdapter adpActiveAgent = new SqlCeDataAdapter(SelQueryActiveAgent, LocalSQLConn);
                    adpActiveAgent.Fill(dsActiveAgent);

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport, dsActiveAgent);
                }

                string SelectQueryForLoggedInAgents = "";
                string SelectQueryForAgentsInCall = "";
                string SelectQueryForAgentsWaittingForCall = "";
                string SelectQueryForCallsRinging = "";
                string SelectQueryForStoppedDialing = "";

                if (CampName == "All")
                {
                    SelectQueryForLoggedInAgents = "select count(*) from (select distinct(uname) from activeagentcalls) AS temp";
                    SelectQueryForAgentsInCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Connected') AS temp";
                    SelectQueryForAgentsWaittingForCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Disconnected') AS temp";
                    SelectQueryForCallsRinging = "select count(*) from ActiveAgentCalls where Status='InProgress'";
                    SelectQueryForStoppedDialing = "Select count(*) from ActiveAgentCalls where Status='Stopped'";
                }
                else
                {
                    SelectQueryForLoggedInAgents = "select count(*) from (select distinct(uname) from activeagentcalls where Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForAgentsInCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Connected' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForAgentsWaittingForCall = "select count(*) from (select Distinct(uName) from ActiveAgentCalls where Status= 'Disconnected' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForCallsRinging = "select count(*) from ActiveAgentCalls where Status='InProgress' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')";
                    SelectQueryForStoppedDialing = "Select count(*) from ActiveAgentCalls where Status='Stopped'and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')";

                }

                SqlCeDataAdapter LoggedInAgentsDA = new SqlCeDataAdapter(SelectQueryForLoggedInAgents, LocalSQLConn);
                DataSet LoggedInAgentDS = new DataSet();
                LoggedInAgentsDA.Fill(LoggedInAgentDS);
                int LoggedInAgents = Convert.ToInt16(LoggedInAgentDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter AgentsInCallDA = new SqlCeDataAdapter(SelectQueryForAgentsInCall, LocalSQLConn);
                DataSet AgentsInCallDS = new DataSet();
                AgentsInCallDA.Fill(AgentsInCallDS);
                int AgentsInCall = Convert.ToInt16(AgentsInCallDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter AgentsWaittingForCallDA = new SqlCeDataAdapter(SelectQueryForAgentsWaittingForCall, LocalSQLConn);
                DataSet AgentsWaittingForCallDS = new DataSet();
                AgentsWaittingForCallDA.Fill(AgentsWaittingForCallDS);
                int intAgentsWaiting4Call = Convert.ToInt16(AgentsWaittingForCallDS.Tables[0].Rows[0][0].ToString());


                SqlCeDataAdapter CallsRingingDA = new SqlCeDataAdapter(SelectQueryForCallsRinging, LocalSQLConn);
                DataSet CallsRingingDS = new DataSet();
                CallsRingingDA.Fill(CallsRingingDS);
                int intRingingCalls = Convert.ToInt16(CallsRingingDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter StppedDialingDA = new SqlCeDataAdapter(SelectQueryForStoppedDialing, LocalSQLConn);
                DataSet StoppedDialingDS = new DataSet();
                StppedDialingDA.Fill(StoppedDialingDS);
                int intStoppedDialing = Convert.ToInt16(StoppedDialingDS.Tables[0].Rows[0][0].ToString());

                if (entUpdateStatistics != null)
                {
                    entUpdateStatistics(LoggedInAgents, AgentsInCall, intAgentsWaiting4Call, intRingingCalls, intStoppedDialing);
                }

                closeConnection();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcSetAgentInfo()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcSetAgentStatus(string uName, string Status, string Phone_No, string CallDuration, bool isPredictive)
        {
            try
            {
                string InsQuery = string.Empty;
                SqlCeCommand cmd = null;
                int result;
                OpenConnection();

                if (isPredictive)
                {
                    InsQuery = "update ActiveAgentCalls set Status='" + Status + "',Phone_No='" + Phone_No + "',callDuration='" + CallDuration + "' where uName='" + uName + "' and Phone_No='" + Phone_No + "'";
                    cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    result = cmd.ExecuteNonQuery();

                    if (result == 0)
                    {
                        string SelQuery = "select ID from ActiveAgentCalls where uName='" + uName + "' and Status='Disconnected' or Status='Stopped'";
                        DataSet ds = new DataSet();
                        SqlCeDataAdapter adp = new SqlCeDataAdapter(SelQuery, LocalSQLConn);
                        adp.Fill(ds);

                        if (ds.Tables[0].Rows.Count == 2)
                        {
                            InsQuery = "update ActiveAgentCalls set Status='" + Status + "',Phone_No='" + Phone_No + "',callDuration='" + CallDuration + "' where (ID=" + ds.Tables[0].Rows[0][0] + ")and (uName='" + uName + "') and (Status='Disconnected' or Status='Stopped')";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            result = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            InsQuery = "update ActiveAgentCalls set Status='" + Status + "',Phone_No='" + Phone_No + "',callDuration='" + CallDuration + "' where (uName='" + uName + "') and (Status='Disconnected' or Status='Stopped')";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            result = cmd.ExecuteNonQuery();
                        }

                        if (result == 0)
                        {
                            InsQuery = "update ActiveAgentCalls set Status='" + Status + "',Phone_No='" + Phone_No + "',callDuration='" + CallDuration + "' where uName='" + uName + "' and Phone_No='NotRegisterd1'";
                            cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                            result = cmd.ExecuteNonQuery();

                            if (result == 0)
                            {
                                InsQuery = "update ActiveAgentCalls set Status='" + Status + "',Phone_No='" + Phone_No + "',callDuration='" + CallDuration + "' where uName='" + uName + "' and Phone_No='NotRegisterd2'";
                                cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    InsQuery = "update ActiveAgentCalls set Status='" + Status + "',Phone_No='" + Phone_No + "',callDuration='" + CallDuration + "' where uName='" + uName + "'";
                    cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    cmd.ExecuteNonQuery();
                }
                closeConnection();

                string SelectQueryForLoggedInAgents = "";
                string SelectQueryForAgentsInCall = "";
                string SelectQueryForAgentsWaittingForCall = "";
                string SelectQueryForCallsRinging = "";
                string SelectQueryForStoppedDialing = "";

                if (CampName == "All")
                {
                    SelectQueryForLoggedInAgents = "select count(*) from (select distinct(uname) from activeagentcalls) AS temp";
                    SelectQueryForAgentsInCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Connected') AS temp";
                    if (isPredictive)
                    {
                        SelectQueryForAgentsWaittingForCall = "select count(*) from( select uname FROM ActiveAgentCalls WHERE (Status = 'Disconnected')GROUP BY uName HAVING (COUNT(*) = 2)) as temp";
                    }
                    else
                    {
                        SelectQueryForAgentsWaittingForCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Disconnected') AS temp";
                    }
                    SelectQueryForCallsRinging = "select count(*) from ActiveAgentCalls where Status='InProgress'";
                    SelectQueryForStoppedDialing = "Select count(*) from ActiveAgentCalls where Status='Stopped'";
                }
                else
                {
                    SelectQueryForLoggedInAgents = "select count(*) from (select distinct(uname) from activeagentcalls where Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForAgentsInCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Connected' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    if (isPredictive)
                    {
                        SelectQueryForAgentsWaittingForCall = "select count(*) from( select uname FROM ActiveAgentCalls WHERE (Status = 'Disconnected') and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "') GROUP BY uName HAVING (COUNT(*) = 2)) as temp";
                    }
                    else
                    {
                        SelectQueryForAgentsWaittingForCall = "select count(*) from (select Distinct(uName) from ActiveAgentCalls where Status= 'Disconnected' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    }
                    SelectQueryForCallsRinging = "select count(*) from ActiveAgentCalls where Status='InProgress' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')";
                    SelectQueryForStoppedDialing = "Select count(*) from ActiveAgentCalls where Status='Stopped'and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')";
                }

                OpenConnection();
                SqlCeDataAdapter LoggedInAgentsDA = new SqlCeDataAdapter(SelectQueryForLoggedInAgents, LocalSQLConn);
                DataSet LoggedInAgentDS = new DataSet();
                LoggedInAgentsDA.Fill(LoggedInAgentDS);
                int LoggedInAgents = Convert.ToInt16(LoggedInAgentDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter AgentsInCallDA = new SqlCeDataAdapter(SelectQueryForAgentsInCall, LocalSQLConn); 
                DataSet AgentsInCallDS = new DataSet();
                AgentsInCallDA.Fill(AgentsInCallDS);
                int AgentsInCall = Convert.ToInt16(AgentsInCallDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter AgentsWaittingForCallDA = new SqlCeDataAdapter(SelectQueryForAgentsWaittingForCall, LocalSQLConn);
                DataSet AgentsWaittingForCallDS = new DataSet();
                AgentsWaittingForCallDA.Fill(AgentsWaittingForCallDS);
                int intAgentsWaiting4Call = Convert.ToInt16(AgentsWaittingForCallDS.Tables[0].Rows[0][0].ToString());


                SqlCeDataAdapter CallsRingingDA = new SqlCeDataAdapter(SelectQueryForCallsRinging, LocalSQLConn);
                DataSet CallsRingingDS = new DataSet();
                CallsRingingDA.Fill(CallsRingingDS);
                int intRingingCalls = Convert.ToInt16(CallsRingingDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter StppedDialingDA = new SqlCeDataAdapter(SelectQueryForStoppedDialing, LocalSQLConn);
                DataSet StoppedDialingDS = new DataSet();
                StppedDialingDA.Fill(StoppedDialingDS);
                int intStoppedDialing = Convert.ToInt16(StoppedDialingDS.Tables[0].Rows[0][0].ToString());

                if (entUpdateStatistics != null)
                {
                    entUpdateStatistics(LoggedInAgents, AgentsInCall, intAgentsWaiting4Call, intRingingCalls, intStoppedDialing);
                }


                string SelQueryActiveAgent = "select uName,Campaign_Id,Group_Name,Phone_No,Status,callDuration from ActiveAgentCalls";                
                DataSet dsActiveAgent = new DataSet();
                SqlCeDataAdapter adpActiveAgent = new SqlCeDataAdapter(SelQueryActiveAgent, LocalSQLConn);
                adpActiveAgent.Fill(dsActiveAgent);

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport, dsActiveAgent);
                closeConnection();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcSetAgentStatus()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcBargeRequest(string uName, string phoneNo)
        {
        }

        void ctlrptActiveAgent_EntsvcBargeReply(string confNo)
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objBargeConf, confNo);
            }
            catch (Exception ex)
            {
            }
        }
        
        public void BargeReply(string ConfNum)
        {
            try
            {
                rAudio.Connect(ConfNum);
                rAudio.fncMic("Mic Off");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objBargeReply()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcHangUp(string uName, string phoneNo)
        {
        }    

        void ctlrptActiveAgent_EntsvcUnJoin(string uName)
        {
            try
            {
                //Remvoe uName from the report.
                string DelQuery = "delete from ActiveAgentCalls where uName='" + uName + "'";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(DelQuery, LocalSQLConn);
                cmd.ExecuteNonQuery();

                string SelectQueryForLoggedInAgents = "";
                string SelectQueryForAgentsInCall = "";
                string SelectQueryForAgentsWaittingForCall = "";
                string SelectQueryForCallsRinging = "";
                string SelectQueryForStoppedDialing = "";

                if (CampName == "All")
                {
                    SelectQueryForLoggedInAgents = "select count(*) from (select distinct(uname) from activeagentcalls) AS temp";
                    SelectQueryForAgentsInCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Connected') AS temp";
                    SelectQueryForAgentsWaittingForCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Disconnected') AS temp";
                    SelectQueryForCallsRinging = "select count(*) from ActiveAgentCalls where Status='InProgress'";
                    SelectQueryForStoppedDialing = "Select count(*) from ActiveAgentCalls where Status='Stopped'";
                }
                else
                {
                    SelectQueryForLoggedInAgents = "select count(*) from (select distinct(uname) from activeagentcalls where Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForAgentsInCall = "select count(*) from (select distinct(uname) from activeagentcalls where Status='Connected' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForAgentsWaittingForCall = "select count(*) from (select Distinct(uName) from ActiveAgentCalls where Status= 'Disconnected' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')) AS temp";
                    SelectQueryForCallsRinging = "select count(*) from ActiveAgentCalls where Status='InProgress' and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')";
                    SelectQueryForStoppedDialing = "Select count(*) from ActiveAgentCalls where Status='Stopped'and Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampName + "')";
                }

                SqlCeDataAdapter LoggedInAgentsDA = new SqlCeDataAdapter(SelectQueryForLoggedInAgents, LocalSQLConn);
                DataSet LoggedInAgentDS = new DataSet();
                LoggedInAgentsDA.Fill(LoggedInAgentDS);
                int LoggedInAgents = Convert.ToInt16(LoggedInAgentDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter AgentsInCallDA = new SqlCeDataAdapter(SelectQueryForAgentsInCall, LocalSQLConn);
                DataSet AgentsInCallDS = new DataSet();
                AgentsInCallDA.Fill(AgentsInCallDS);
                int AgentsInCall = Convert.ToInt16(AgentsInCallDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter AgentsWaittingForCallDA = new SqlCeDataAdapter(SelectQueryForAgentsWaittingForCall, LocalSQLConn);
                DataSet AgentsWaittingForCallDS = new DataSet();
                AgentsWaittingForCallDA.Fill(AgentsWaittingForCallDS);
                int intAgentsWaiting4Call = Convert.ToInt16(AgentsWaittingForCallDS.Tables[0].Rows[0][0].ToString());


                SqlCeDataAdapter CallsRingingDA = new SqlCeDataAdapter(SelectQueryForCallsRinging, LocalSQLConn);
                DataSet CallsRingingDS = new DataSet();
                CallsRingingDA.Fill(CallsRingingDS);
                int intRingingCalls = Convert.ToInt16(CallsRingingDS.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter StppedDialingDA = new SqlCeDataAdapter(SelectQueryForStoppedDialing, LocalSQLConn);
                DataSet StoppedDialingDS = new DataSet();
                StppedDialingDA.Fill(StoppedDialingDS);
                int intStoppedDialing = Convert.ToInt16(StoppedDialingDS.Tables[0].Rows[0][0].ToString());

                if (entUpdateStatistics != null)
                {
                    entUpdateStatistics(LoggedInAgents, AgentsInCall, intAgentsWaiting4Call, intRingingCalls, intStoppedDialing);
                }

                string SelQueryActiveAgent = "select uName,Campaign_Id,Group_Name,Phone_No,Status,callDuration from ActiveAgentCalls";
                DataSet dsActiveAgent = new DataSet();
                SqlCeDataAdapter adpActiveAgent = new SqlCeDataAdapter(SelQueryActiveAgent, LocalSQLConn);
                adpActiveAgent.Fill(dsActiveAgent);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport, dsActiveAgent);

                closeConnection();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcUnJoin()", "ctlrptActiveAgent.xaml.cs");
            }

        }
        
        #endregion      

        private bool IsTableExits(string strTableName)
        {
            try
            {
                string str = "SELECT COUNT(*) FROM Information_Schema.Tables WHERE (TABLE_NAME ='" + strTableName + "')";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(str, LocalSQLConn);
                object i = cmd.ExecuteScalar();

                if (int.Parse(i.ToString()) > 0)
                {
                    closeConnection();
                    return true;
                }
                else
                {
                    closeConnection();
                    return false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsTableExits()", "ctlrptActiveAgent.xaml.cs");
                   return false;
            }

        }

        void OpenConnection()
        {
            try
            {
                if (LocalSQLConn.State != ConnectionState.Open)
                {
                    LocalSQLConn.Open();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OpenConnection()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void closeConnection()
        {
            try
            {
                if (LocalSQLConn.State != ConnectionState.Closed)
                {
                    LocalSQLConn.Close();
                }
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "closeConnection()", "ctlrptActiveAgent.xaml.cs");
            }
        }
        
        public void btnGo_Click(object sender, RoutedEventArgs e)
        { }

        public void objdelRefreshReport(DataSet ds)
        {
            try
            {
                List<ActiveAgent> objActiveAgent1 = new List<ActiveAgent>(); //first list
                List<ActiveAgent> objActiveAgent2 = new List<ActiveAgent>(); //second list


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //DataSet dsCampName = new DataSet();
                    //System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
                    //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select Name from Campaign where ID='" + ds.Tables[0].Rows[i]["Campaign_Id"].ToString() + "'", conn);
                    //System.Data.SqlClient.SqlDataAdapter daCampName = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    //daCampName.Fill(dsCampName);
                    string status = ds.Tables[0].Rows[i]["Status"].ToString();
                    string Color = string.Empty;
                    switch (status)
                    {
                        case "ready":
                            Color = "White";
                            break;

                        case "InProgress":
                            Color = "Blue";
                            break;

                        case "Connected":
                            Color = "Green";
                            break;

                        case "Disconnected":
                            Color = "Red";
                            break;

                        case "Incoming":
                            Color = "Yellow";
                            break;

                        case "Hold":
                            Color = "Brown";
                            break;

                        case "Stopped":
                            Color = "Orange";
                            break;

                    }


                    if (objActiveAgent1.Count < 26)
                    {
                        objActiveAgent1.Add(ActiveAgent.Create(ds.Tables[0].Rows[i]["uName"].ToString(), ds.Tables[0].Rows[i]["Campaign_Id"].ToString(), ds.Tables[0].Rows[i]["Group_Name"].ToString(), ds.Tables[0].Rows[i]["Phone_No"].ToString(), ds.Tables[0].Rows[i]["Status"].ToString(), Color, ds.Tables[0].Rows[i]["callDuration"].ToString()));
                    }
                    else
                    {
                        objActiveAgent2.Add(ActiveAgent.Create(ds.Tables[0].Rows[i]["uName"].ToString(), ds.Tables[0].Rows[i]["Campaign_Id"].ToString(), ds.Tables[0].Rows[i]["Group_Name"].ToString(), ds.Tables[0].Rows[i]["Phone_No"].ToString(), ds.Tables[0].Rows[i]["Status"].ToString(), Color, ds.Tables[0].Rows[i]["callDuration"].ToString()));
                    }
                }

                AgentItems.ItemsSource = objActiveAgent1;
                AgentItems1.ItemsSource = objActiveAgent2;

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objdelRefreshReport()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        public void BargeConf(string strConfNo)
        {
            try
            {
                rAudio.Connect(strConfNo);
                rAudio.fncMic("Mic Off");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BargeConf()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        public void BargeHangup()
        {
            try
            {
                rAudio.DisConnect();
            }
            catch (ExecutionEngineException x)
            {

            }
        }

        #region CommandBinding Events

        void OnBargeClick(object sender, ExecutedRoutedEventArgs args)
        {
            //Code to Barge perticular call
            try
            {
                string[] strValue = args.Parameter.ToString().Split(',');
                string strName = strValue[0];
                string PhNo = strValue[1];

                foreach (ActiveAgent aa in ((DashBoard.Presentation.Active.ctlrptActiveAgent)sender).AgentItems.Items)
                {
                    if (aa.uName == strName)
                    {
                        if (aa.BtnBargeContent == "Barge") 
                        {
                            if (!isBarging)
                            {
                                channelNetTcp.svcBargeRequest(strName, PhNo);
                                aa.BtnBargeContent = "UnBarge";
                                isBarging = true;

                                if (!hashConfNumber.Contains(strName))
                                {
                                    hashConfNumber.Add(strName, PhNo);
                                }


                                break;
                            }
                            else
                            {
                                MessageBox.Show("You are all ready Barging one call please Disconnect it and Barge it again");
                                break;
                            }
                        }
                        else
                        {
                            aa.BtnBargeContent = "Barge";
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objBargeHangup);
                            isBarging = false;

                            if (hashConfNumber.Contains(strName))
                            {
                                hashConfNumber.Remove(strName);
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnBargeClick()", "ctlrptActiveAgent.xaml.cs");
            }
        }


        void OnHangUpClick(object sender, ExecutedRoutedEventArgs args)
        {
            //Code to Hang up perticular call
            try
            {
                string strValue = args.Parameter.ToString();
                string strName = strValue.Split(',')[0];
                string PhNo = strValue.Split(',')[1];

                channelNetTcp.svcHangUp(strName, PhNo);

                if (hashConfNumber.Contains(strName))
                {
                    hashConfNumber.Remove(strName);

                    isBarging = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        public void ClosePod()
        {
            try
            {
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }

                LocalSQLConn.Close();
                LocalSQLConn.Dispose();
                LocalSQLConn = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlrptActiveAgent.xaml.cs");
            }
        }
    }
}








