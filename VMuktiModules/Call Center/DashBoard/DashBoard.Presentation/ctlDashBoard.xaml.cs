using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DashBoard.Presentation.CampaignManagement;
using System.Data;
using System.Data.SqlServerCe;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServerCe;
using System.IO;
using VMuktiService;
using VMuktiAPI;
using DashBoard.Business.WCF_Services;
using System.Collections.ObjectModel;


namespace DashBoard.Presentation
{
    /// <summary>
    /// Interaction logic for ctlDashBoard.xaml
    /// </summary>
    /// 
    
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlDashBoard : UserControl
    {
        System.Timers.Timer t1;
        double Sale,Sales,Call,Hour;
        double temp = 0.0;
        double Calls;
        int TotalLeads, CallsToday, TotalAgentsInCall, AnsweredCalls, id, intCallsPlaced;
        int LoggedInAgents,intAgentStoppedDialing, intAgentsWaiting4Call, intRingingCalls;
        int TotCount = 0;
        int count = 0;
        int totalcount = 0;
        string CName;
        List<String> objStatuses = new List<string>();
        string SelectQuery1, SelectQuery2;
        long DID;
        
        ObservableCollection<Disposition> DispositionCollection = new ObservableCollection<Disposition>();        

        public delegate void DelShowSaleCall();
        public DelShowSaleCall objDelShowSaleCall;

        public delegate void DelUpdateValues();
        public DelUpdateValues objDelUpdateValues;

        object objDashBoard = new NetP2PBootStrapDashBoardDelegate();
        INetP2PBootStrapdashBoardChannel channelNetTcp = null;

        System.Threading.Thread tHostDashBoard = null;
        string strUri;
        DashBoard.Presentation.Active.ctlrptActiveAgent objActive = null;

        #region SDF Creation - 1

        string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "RealTimeCampaignValues.sdf";
        string strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "RealTimeCampaignValues.sdf";
        SqlCeConnection LocalSQLConn = null;

        #endregion

       
        public ctlDashBoard()
        {
            try
            {
                InitializeComponent();                

                CName = cmbCampaign.Text;

                FncFillCmbCampaign();
                FncFillRealTimeValues();
                FncCampaignManagement();
                
                cmbCampaign.DropDownClosed += new EventHandler(cmbCampaign_DropDownClosed);

                #region SDF Creation-2
                                
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "RealTimeCampaignValues.sdf"))
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory.ToString() + "RealTimeCampaignValues.sdf");
                }

                SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                clientEngine.CreateDatabase();
                LocalSQLConn = new SqlCeConnection();
                LocalSQLConn.ConnectionString = ClientConnectionString;
                LocalSQLConn.Open();

                FncRealTimeValuesSDF();
                FncFillDispositionTable();
                FncFillCampaignLeadsTable();               

                LocalSQLConn.Close();

                #endregion

                objDelUpdateValues = new DelUpdateValues(FncUpDateValues);
                FncLoadActiveAgentCall();                                                

                #region To define thread that hosts the WCF services

                tHostDashBoard = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(HostDashBoardServices));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapDashBoard");
                lstParams.Add("P2PDashBoardMesh");
                tHostDashBoard.Start(lstParams);
                
                #endregion

            }

            catch (Exception ex)
            {                
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDashBoard()", "ctlDashBoard.xaml.cs");
            } 
        }

        #region Host service and WCF Services

        public void HostDashBoardServices(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUri = lstTempObj[0].ToString();

                NetPeerClient npcDashBoard = new NetPeerClient();
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcGetCallInfo += new NetP2PBootStrapDashBoardDelegate.DelsvcGetCallInfo(ctlDashBoard_EntsvcGetCallInfo);
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcJoin += new NetP2PBootStrapDashBoardDelegate.delsvcJoin(ctlDashBoard_EntsvcJoin);
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcUnJoin += new NetP2PBootStrapDashBoardDelegate.delsvcUnjoin(ctlDashBoard_EntsvcUnJoin);
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcSetAgents += new NetP2PBootStrapDashBoardDelegate.DelsvcSetAgents(ctlDashBoard_EntsvcSetAgents);
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcGetAgents += new NetP2PBootStrapDashBoardDelegate.DelsvcGetAgents(ctlDashBoard_EntsvcGetAgents);
                channelNetTcp = (INetP2PBootStrapdashBoardChannel)npcDashBoard.OpenClient<INetP2PBootStrapdashBoardChannel>(strUri, lstTempObj[1].ToString(), ref objDashBoard);
                channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HostDashBoardServices()", "ctlDashBoard.xaml.cs");
            }
        }

        void ctlDashBoard_EntsvcGetAgents(int intCampaignID, string uname)
        {
            //if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uname)
            //{

            //}
        }

        void ctlDashBoard_EntsvcSetAgents(int intAgentID, string strAgentName, int intCampaignID,string to)
        {           
        }

        void ctlDashBoard_EntsvcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal)
        {
            try
            {
                DID = DispositionID;

                string InsertQuery = "Insert into Calls (LeadID,CalledDate,ModifiedDate,ModifiedBy, GeneratedBy,StartDate,StartTime,DurationInSecond, DispositionID,CampaignID, ConfID,IsDeleted,CallNote,IsDNC,IsGlobal) values ('" + LeadID + "','" + CalledDate + "','" + ModifiedDate + "','" + ModifiedBy + "','" + GeneratedBy + "','" + StartDate + "','" + StartTime + "','" + DurationInSecond + "','" + DispositionID + "','" + CampaignID + "','" + ConfID + "','" + IsDeleted + "','" + CallNote + "','" + isDNC + "','" + isGlobal + "')";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(InsertQuery, LocalSQLConn);
                cmd.ExecuteNonQuery();

                FncGetCallsToday(CName);
                FncGetSales(CName);
                FncGetCalls(CName);
                FncCampaignLeadsCount(CName);
                FncAnsweredCalls(CName);
                //FncCampaignAgentsCount(CName);
                FncCampaignStatuses(CName);

                objDelShowSaleCall = new DelShowSaleCall(FncShowLableContents);
                

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelShowSaleCall);

                LocalSQLConn.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDashBoard_EntsvcGetCallInfo()", "ctlDashBoard.xaml.cs");
            }
        }

        void ctlDashBoard_EntsvcUnJoin(string uname)
        {
           
        }

        void ctlDashBoard_EntsvcJoin(string uname)
        {
            try
            {
                //if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uname)
                //{
                //    lblTotLoggedAgentsValues.Content = int.Parse(lblTotLoggedAgentsValues.Content.ToString()) + 1;
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDashBoard_EntsvcJoin()", "ctlDashBoard.xaml.cs");
            }
        }

        #endregion

        #region SDF Creation-3

        void FncRealTimeValuesSDF()
        {
            try
            {
                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }

                if (!IsTableExits("ActiveAgentCalls"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblActiveAgentCall = new SyncTable("ActiveAgentCalls");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("ActiveAgentCalls");


                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("ID");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["ID"].MaxLength = 30;
                    syncSchemaLead.Tables["ActiveAgentCalls"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("uName");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["uName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["uName"].AutoIncrement = false;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["uName"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["uName"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["uName"].MaxLength = 30;
                    //syncSchemaLead.Tables["ActiveAgentCalls"].PrimaryKey = new string[] { "uName" };

                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("Campaign_Id");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Campaign_Id"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Campaign_Id"].MaxLength = 30;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Campaign_Id"].AutoIncrement = false;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Campaign_Id"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Campaign_Id"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("Status");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Status"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Status"].MaxLength = 30;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Status"].AutoIncrement = false;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Status"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Status"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("Group_Name");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Group_Name"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Group_Name"].MaxLength = 30;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Group_Name"].AutoIncrement = false;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Group_Name"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Group_Name"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("Phone_No");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Phone_No"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Phone_No"].MaxLength = 30;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Phone_No"].AutoIncrement = false;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Phone_No"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["Phone_No"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns.Add("callDuration");
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["callDuration"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["callDuration"].MaxLength = 30;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["callDuration"].AutoIncrement = false;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["callDuration"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["ActiveAgentCalls"].Columns["callDuration"].AutoIncrementStep = 1;

                    sync.CreateSchema(tblActiveAgentCall, syncSchemaLead);
                }

                if (!IsTableExits("Disposition"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);
                    SyncTable tblDisposition = new SyncTable("Disposition");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("Disposition");

                    syncSchemaLead.Tables["Disposition"].Columns.Add("ID");
                    syncSchemaLead.Tables["Disposition"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Disposition"].Columns["ID"].AutoIncrement = false;
                    syncSchemaLead.Tables["Disposition"].Columns["ID"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Disposition"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Disposition"].Columns["ID"].MaxLength = 30;
                    syncSchemaLead.Tables["Disposition"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["Disposition"].Columns.Add("DispositionName");
                    syncSchemaLead.Tables["Disposition"].Columns["DispositionName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Disposition"].Columns["DispositionName"].AutoIncrement = false;
                    syncSchemaLead.Tables["Disposition"].Columns["DispositionName"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Disposition"].Columns["DispositionName"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Disposition"].Columns["DispositionName"].MaxLength = 50;

                    sync.CreateSchema(tblDisposition, syncSchemaLead);
                }

                if (!IsTableExits("CampaignLeads"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);
                    SyncTable tblCampaignLeads = new SyncTable("CampaignLeads");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("CampaignLeads");

                    syncSchemaLead.Tables["CampaignLeads"].Columns.Add("ID");
                    syncSchemaLead.Tables["CampaignLeads"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["CampaignLeads"].Columns["ID"].AutoIncrement = false;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["ID"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["ID"].MaxLength = 30;
                    syncSchemaLead.Tables["CampaignLeads"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["CampaignLeads"].Columns.Add("CampaignName");
                    syncSchemaLead.Tables["CampaignLeads"].Columns["CampaignName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["CampaignLeads"].Columns["CampaignName"].AutoIncrement = false;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["CampaignName"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["CampaignName"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["CampaignName"].MaxLength = 30;

                    syncSchemaLead.Tables["CampaignLeads"].Columns.Add("TotalLeads");
                    syncSchemaLead.Tables["CampaignLeads"].Columns["TotalLeads"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["CampaignLeads"].Columns["TotalLeads"].AutoIncrement = false;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["TotalLeads"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["TotalLeads"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["CampaignLeads"].Columns["TotalLeads"].MaxLength = 50;

                    sync.CreateSchema(tblCampaignLeads, syncSchemaLead);
                }

                if (!IsTableExits("Calls"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblCalls = new SyncTable("Calls");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("Calls");

                    syncSchemaLead.Tables["Calls"].Columns.Add("ID");
                    syncSchemaLead.Tables["Calls"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["Calls"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["Calls"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Calls"].Columns["ID"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["Calls"].Columns.Add("LeadID");
                    syncSchemaLead.Tables["Calls"].Columns["LeadID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["LeadID"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["LeadID"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["LeadID"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["LeadID"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("CalledDate");
                    syncSchemaLead.Tables["Calls"].Columns["CalledDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["CalledDate"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["CalledDate"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["CalledDate"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["CalledDate"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("ModifiedDate");
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedDate"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedDate"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedDate"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedDate"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("ModifiedBy");
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedBy"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedBy"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedBy"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["ModifiedBy"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("GeneratedBy");
                    syncSchemaLead.Tables["Calls"].Columns["GeneratedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["GeneratedBy"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["GeneratedBy"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["GeneratedBy"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["GeneratedBy"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("StartDate");
                    syncSchemaLead.Tables["Calls"].Columns["StartDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["StartDate"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["StartDate"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["StartDate"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["StartDate"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("StartTime");
                    syncSchemaLead.Tables["Calls"].Columns["StartTime"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["StartTime"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["StartTime"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["StartTime"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["StartTime"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("DurationInSecond");
                    syncSchemaLead.Tables["Calls"].Columns["DurationInSecond"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["DurationInSecond"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["DurationInSecond"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["DurationInSecond"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["DurationInSecond"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("DispositionID");
                    syncSchemaLead.Tables["Calls"].Columns["DispositionID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["DispositionID"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["DispositionID"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["DispositionID"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["DispositionID"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("CampaignID");
                    syncSchemaLead.Tables["Calls"].Columns["CampaignID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["CampaignID"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["CampaignID"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["CampaignID"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["CampaignID"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("ConfID");
                    syncSchemaLead.Tables["Calls"].Columns["ConfID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["ConfID"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["ConfID"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["ConfID"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["ConfID"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("IsDeleted");
                    syncSchemaLead.Tables["Calls"].Columns["IsDeleted"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["IsDeleted"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["IsDeleted"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["IsDeleted"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["IsDeleted"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("CallNote");
                    syncSchemaLead.Tables["Calls"].Columns["CallNote"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["CallNote"].MaxLength = 250;
                    syncSchemaLead.Tables["Calls"].Columns["CallNote"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["CallNote"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["CallNote"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("IsDNC");
                    syncSchemaLead.Tables["Calls"].Columns["IsDNC"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["IsDNC"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["IsDNC"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["IsDNC"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["IsDNC"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Calls"].Columns.Add("IsGlobal");
                    syncSchemaLead.Tables["Calls"].Columns["IsGlobal"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaLead.Tables["Calls"].Columns["IsGlobal"].MaxLength = 30;
                    syncSchemaLead.Tables["Calls"].Columns["IsGlobal"].AutoIncrement = false;
                    syncSchemaLead.Tables["Calls"].Columns["IsGlobal"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Calls"].Columns["IsGlobal"].AutoIncrementStep = 1;

                    sync.CreateSchema(tblCalls, syncSchemaLead);
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncRealTimeValuesSDF()", "ctlDashBoard.xaml.cs");                
            }
        }

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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsTableExits()", "ctlDashBoard.xaml.cs");                
                return false;
            }

        }

        private void OpenConnection()
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OpenConnection()", "ctlDashBoard.xaml.cs");                
            }
        }

        #endregion 

        #region SDF tables those are filled at load time.

        private void FncFillCampaignLeadsTable()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetCampaignLeads();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string insert_Query = "insert into CampaignLeads (ID, CampaignName,TotalLeads) values ('" + ds.Tables[0].Rows[i][0] + "' , '" + ds.Tables[0].Rows[i][1].ToString() + "' , '" + ds.Tables[0].Rows[i][2].ToString() + "')";

                        SqlCeCommand CeCmd1 = new SqlCeCommand(insert_Query, LocalSQLConn);
                        CeCmd1.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncFillCampaignLeadsTable()", "ctlDashBoard.xaml.cs");                
            }
        }

        private void FncFillDispositionTable()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetAllDesposition();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string insert_Query = "insert into Disposition (ID, DispositionName) values ('" + ds.Tables[0].Rows[i][0] + "' , '" + ds.Tables[0].Rows[i][1].ToString() + "')";

                        SqlCeCommand CeCmd1 = new SqlCeCommand(insert_Query, LocalSQLConn);
                        CeCmd1.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncFillDispositionTable()", "ctlDashBoard.xaml.cs");                
            }
        }

        #endregion

        #region Load ActiveAgentCall Report

        private void FncLoadActiveAgentCall()
        {
            try
            {
                objActive = new DashBoard.Presentation.Active.ctlrptActiveAgent();
                cnvActiveAgent.Children.Add(objActive);
                objActive.entUpdateStatistics += new DashBoard.Presentation.Active.ctlrptActiveAgent.delUpdateStatistics(objActive_entUpdateStatistics);
                objActive.SetCampaign("All");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncLoadActiveAgentCall()", "ctlDashBoard.xaml.cs");
            }
        }

        void objActive_entUpdateStatistics(int LoggedIn, int AgentsInCall, int Waitting, int ringing,int stopped)
        {
            try
            {
                LoggedInAgents = LoggedIn;
                intAgentStoppedDialing = stopped;
                TotalAgentsInCall = AgentsInCall;
                intAgentsWaiting4Call = Waitting;
                intRingingCalls = ringing;

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUpdateValues);
            }
            catch (Exception ex)
            {
            }
        }

        public void FncUpDateValues()
        {
            lblTotLoggedAgentsValues.Content = LoggedInAgents.ToString();
            lblStoppedDialingValues.Content = intAgentStoppedDialing.ToString();
            lblToAgentsInCallValues.Content = TotalAgentsInCall.ToString();
            lblTotRingingCallsValues.Content = intRingingCalls.ToString();
            lblTotWaitingCallAgentsValues.Content = intAgentsWaiting4Call.ToString();
        }

        #endregion

        #region Load Campaign Management report

        private void FncCampaignManagement()
        {
            //ctlCampaignManagement objCampaign = new ctlCampaignManagement();
            //brdCampaign.Child = (UIElement)objCampaign;
            CtlAssignTreatment objCampaign = new CtlAssignTreatment();
            brdCampaign.Child = (UIElement)objCampaign;
        }

        #endregion

        #region Extra Functions

        private void FncFillCmbCampaign()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetAllCampains();

                cmbCampaign.SelectedIndex = -1;
                cmbCampaign.Items.Clear();
                cmbCampaign.Items.Add("All");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbCampaign.Items.Add(ds.Tables[0].Rows[i][1].ToString());
                }
                cmbCampaign.SelectedIndex = 0;

                CName = cmbCampaign.Text;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncFillCmbCampaign()", "ctlDashBoard.xaml.cs");                
            }
        }

        private void FncFillRealTimeValues()
        {
            lblCPHValue.Content = 0.0;
            lblSPHValue.Content = 0.0;
            lblDialableLeadsValue.Content = 0;
            lblCallsTodayValue.Content = 0;
            lblAnsweredValues.Content = 0;
            //lblLocalTimeValues.Content = 0;            
            lblToAgentsInCallValues.Content = 0;
            lblTotLoggedAgentsValues.Content = 0;
            lblTotWaitingCallAgentsValues.Content = 0;
            //lblTotPlacedCallsValues.Content = 0;
            lblStoppedDialingValues.Content = 0;
            lblTotRingingCallsValues.Content = 0;
        }               

        void cmbCampaign_DropDownClosed(object sender, EventArgs e)
        {
            OpenConnection();

            if (cmbCampaign.SelectionBoxItem.ToString() != "")
            {
                CName = cmbCampaign.Text;
                objActive.SetCampaign(CName);
                if (IsTableDataExits("ActiveAgentCalls") && IsTableDataExits("Calls"))
                {
                    FncGetCallsToday(CName);
                    FncGetSales(CName);
                    FncGetCalls(CName);
                    FncAnsweredCalls(CName);
                    FncCampaignAgentsCount(CName);
                    FncCampaignLeadsCount(CName);
                    FncCampaignStatuses(CName);
                }
                else
                    MessageBox.Show("No informations will be shown until Calls are made.");

                objDelShowSaleCall = new DelShowSaleCall(FncShowLableContents);

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelShowSaleCall);

                LocalSQLConn.Close();
            }            
        }       

        private void FncCampaignStatuses(string CName)
        {
            objStatuses.Clear();

            int id;
            //string SelectQuery1 = "select dispositionname from disposition where id in(select dispositionid from calls where campaignid in (select id from campaignLeads where CampaignName='Test Campaign'))";
            if (CName == "All")
                SelectQuery1 = "SELECT Disposition.DispositionName FROM Disposition INNER JOIN Calls ON Disposition.ID = Calls.DispositionID order by calls.id desc";
            else
            {
                string SelectQuery = "select id from campaignleads where campaignname = '" + CName + "' ";
                OpenConnection();

                SqlCeDataAdapter adp = new SqlCeDataAdapter(SelectQuery, LocalSQLConn);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                id = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                
                SelectQuery1 = "SELECT Disposition.DispositionName FROM Calls INNER JOIN Disposition ON Calls.DispositionID = Disposition.ID WHERE Calls.CampaignID = '" + id + "' order by calls.id desc";

                SelectQuery2 = "SELECT distinct Disposition.DispositionName FROM Calls INNER JOIN Disposition ON Calls.DispositionID = Disposition.ID WHERE Calls.CampaignID = '" + id + "' ";

                SqlCeDataAdapter adp2 = new SqlCeDataAdapter(SelectQuery2, LocalSQLConn);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);

                count = ds2.Tables[0].Rows.Count;
            }
            
            SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
            DataSet ds1 = new DataSet();
            adp1.Fill(ds1);

            totalcount = ds1.Tables[0].Rows.Count;
                        
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                objStatuses.Add(ds1.Tables[0].Rows[i][0].ToString());
            }

            LocalSQLConn.Close();
        }
        
        private void FncGetCallsToday(string CName)
        {
            try
            {
                if (CName == "All")
                {
                    SelectQuery1 = "select count(*) from calls where datepart(dd,calleddate) = datepart(dd,getdate()) and datepart(mm,calleddate) = datepart(mm,getdate()) and datepart(yy,calleddate) = datepart(yy,getdate())";

                //"select count(*) from calls where CONVERT(CHAR(10), CalledDate, 23) = CONVERT(CHAR(10), getdate(), 23) ";
                    
                }
                else
                {
                    SelectQuery1 = "select count(*) from calls where datepart(dd,calleddate) = datepart(dd,getdate()) and datepart(mm,calleddate) = datepart(mm,getdate()) and datepart(yy,calleddate) = datepart(yy,getdate()) and CampaignId in (select Id from CampaignLeads where CampaignName = '" + CName + "')";

                    //"select count(*) from calls where CONVERT(CHAR(10), CalledDate, 23) = CONVERT(CHAR(10), getdate(), 23) ";
                }

                OpenConnection();

                SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
                DataSet ds1 = new DataSet();
                adp1.Fill(ds1);
                
                CallsToday = Convert.ToInt16(ds1.Tables[0].Rows[0][0].ToString());
                
                LocalSQLConn.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetCallsToday()", "ctlDashBoard.xaml.cs");                
            }            
        }

        private void FncGetSales(string CName)
        {
            try
            {
                if (CName == "All")
                {
                    SelectQuery1 = "select count(*) from Calls where DispositionID in (select ID from Disposition where DispositionName ='SALE')";
                    SelectQuery2 = "select sum(DurationInSecond) from Calls";                    
                }
                else
                {
                    SelectQuery1 = "select count(*) from Calls where DispositionID in (select ID from Disposition where DispositionName ='SALE') and CampaignId in (select id from CampaignLeads where CampaignName = '"+ CName +"')";
                    SelectQuery2 = "select sum(DurationInSecond) from Calls where CampaignId in (select id from CampaignLeads where CampaignName = '" + CName + "')";                   
                }

                OpenConnection();

                SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
                DataSet ds1 = new DataSet();
                
                adp1.Fill(ds1);
                
                Sales = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter adp2 = new SqlCeDataAdapter(SelectQuery2, LocalSQLConn);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                
                if (ds2.Tables[0].Rows[0][0].ToString() == "")
                    temp = 0.0;
                else
                temp = Convert.ToDouble(ds2.Tables[0].Rows[0][0].ToString());
                    
                Hour = (temp / 3600);
                if (Hour != 0.0)
                Sale = Sales / Hour;
                else
                    Sale = 0.0;
            
                LocalSQLConn.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetSales()", "ctlDashBoard.xaml.cs");                
            }
        }

        private void FncGetCalls(string CName)
        {
            try
            {
                if (CName == "All")
                {
                    SelectQuery1 = "select count(*) from Calls";
                    SelectQuery2 = "select sum(DurationInSecond) from Calls";                                     
                }
                else
                {
                    SelectQuery1 = "select count(*) from Calls where CampaignID in (select id from CampaignLeads where CampaignName = '" + CName + "')";
                    SelectQuery2 = "select sum(DurationInSecond) from Calls where CampaignID in (select id from CampaignLeads where CampaignName = '" + CName + "')";                   
                }

                OpenConnection();

                SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
                DataSet ds1 = new DataSet();

                adp1.Fill(ds1);

                Calls = Convert.ToInt16(ds1.Tables[0].Rows[0][0].ToString());

                SqlCeDataAdapter adp2 = new SqlCeDataAdapter(SelectQuery2, LocalSQLConn);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);

                if (ds2.Tables[0].Rows[0][0].ToString() =="")
                    temp = 0.0;
                else
                temp = Convert.ToDouble(ds2.Tables[0].Rows[0][0].ToString());

                Hour = (temp / 3600);
                if (Hour != 0.0)
                Call = (Calls / Hour);
                else 
                    Call = 0.0;
                
                LocalSQLConn.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetCalls()", "ctlDashBoard.xaml.cs");                
            }
        }

        private void FncCampaignLeadsCount(string CName)
        {
            try
            {
                if (CName == "All")
                    SelectQuery1 = "Select sum(TotalLeads) from CampaignLeads";
                else
                    SelectQuery1 = "Select TotalLeads from CampaignLeads where CampaignName = '" + CName + "' ";

                    OpenConnection();

                    SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
                    DataSet ds1 = new DataSet();
                    adp1.Fill(ds1);

                    TotalLeads = Convert.ToInt16(ds1.Tables[0].Rows[0][0].ToString());

                    lblDialableLeadsValue.Content = TotalLeads.ToString();

                    LocalSQLConn.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCampaignLeadsCount()", "ctlDashBoard.xaml.cs");                
            }
        }

        private void FncAnsweredCalls(string CName)
        {
            try
            {
                if (CName == "All")
                    SelectQuery1 = "select count(*) from calls where dispositionid in(select id from disposition where dispositionname in ('CALLBK','SALE','NI','D2','D3','D3A','D3I','D3P','WN','Q'))";
                else
                    SelectQuery1 = "select count(*) from calls where dispositionid in(select id from disposition where dispositionname in ('CALLBK','SALE','NI','D2','D3','D3A','D3I','D3P','WN','Q')) and CampaignId in (select Id from CampaignLeads where CampaignName = '"+ CName+"')";

                OpenConnection();

                SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
                DataSet ds1 = new DataSet();

                adp1.Fill(ds1);

                AnsweredCalls = Convert.ToInt16(ds1.Tables[0].Rows[0][0].ToString());
                
                LocalSQLConn.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncAnsweredCalls()", "ctlDashBoard.xaml.cs");                
            }
        }

        private void FncCampaignAgentsCount(string CampaignName)
        {
            try
            {
                if (CampaignName == "All")
                    SelectQuery1 = "select count(*) from ActiveAgentCalls where Status = 'Connected'";
                else
                    SelectQuery1 = "Select count(*) from ActiveAgentCalls where Campaign_Id in (select id from CampaignLeads where CampaignName = '" + CampaignName + "' and Status = 'Connected')";
                //where Campaign_ID in (select id from CampaignLeads where CampaignName = '" + CName + "') ";
                OpenConnection();

                SqlCeDataAdapter adp1 = new SqlCeDataAdapter(SelectQuery1, LocalSQLConn);
                DataSet ds1 = new DataSet();

                adp1.Fill(ds1);

                TotalAgentsInCall = Convert.ToInt16(ds1.Tables[0].Rows[0][0].ToString());
                
                LocalSQLConn.Close();
                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCampaignAgentsCount()", "ctlDashBoard.xaml.cs");                
            }
        }

        private bool IsTableDataExits(string strTable)
        {
            try
            {
                string str = "SELECT COUNT(*) FROM " + strTable;
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(str, LocalSQLConn);
                object i = cmd.ExecuteScalar();

                if (int.Parse(i.ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsTableDataExits()", "ctlDashBoard.xaml.cs");                
                return false;
            }
        }

        public void FncShowLableContents()
        {
            lblSPHValue.Content = Sale.ToString("#0");
            lblCPHValue.Content = Call.ToString("#0");
            lblCallsTodayValue.Content = CallsToday.ToString();
            //lblToAgentsInCallValues.Content = TotalAgentsInCall.ToString();
            lblDialableLeadsValue.Content = TotalLeads.ToString();
            lblAnsweredValues.Content = AnsweredCalls.ToString();

            if (objStatuses.Count > 0)
            {
                if (count == DispositionCollection.Count && count > 0)
                {
                    if (totalcount > TotCount)
                    {
                        string SelectQuery = "select dispositionName from disposition where id = '" + DID + "'";
                        OpenConnection();

                        SqlCeDataAdapter adp = new SqlCeDataAdapter(SelectQuery, LocalSQLConn);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);

                        string DName = ds.Tables[0].Rows[0][0].ToString();

                        lstDisposition.ItemsSource = null;
                        lstDisposition.ItemsSource = Disposition.ManageCollection(DName, DispositionCollection);
                    }
                    else
                    {
                        lstDisposition.ItemsSource = null;
                        lstDisposition.ItemsSource = Disposition.ManageCollection("", DispositionCollection);
                    }
                }
                else
                {
                    if (totalcount == TotCount)
                    {
                    lstDisposition.ItemsSource = null;
                        lstDisposition.ItemsSource = Disposition.ManageCollection("", DispositionCollection);
                    }
                    else
                    {
                        lstDisposition.ItemsSource = null;
                        lstDisposition.ItemsSource = Disposition.ManageCollection(objStatuses[0].ToString(), DispositionCollection);
                    }
                    //lstDisposition.ItemsSource = Disposition.ManageCollection(, DispositionCollection);                   
                }

            }
            if (objStatuses.Count == 0)
            {
                lstDisposition.ItemsSource = null;
                lstDisposition.Items.Clear();
            }

            Disposition obj = new Disposition("", 0);
            TotCount = obj.FncSetValue();

        }

        #endregion    

        public void ClosePod()
        {
            try
            {
                if (channelNetTcp != null)
                {
                    channelNetTcp.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    channelNetTcp.Close();
                }
                if (objActive != null)
                {
                    objActive.ClosePod();
                    objActive = null;
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
