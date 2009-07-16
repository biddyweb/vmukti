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
using System.Data;
using VMuktiService;
using rptActiveCall.Business;
using Microsoft.Reporting.WinForms;
using System.Reflection;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServerCe;
using Microsoft.Synchronization.Data.Server;
using System.Data.SqlServerCe;
using System.IO;

namespace rptActiveCall.Presentation
{
    /// <summary>
    /// Interaction logic for ctlrptActiveCall.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    } 
    public partial class ctlrptActiveCall : UserControl
    {
        string strUri;
        object objActiveCall = new NetP2PBootStrapActiveCallReportDelegates();
        INetP2PBootStrapReportChannel channelNetTcpActiveCall;
        
        bool GoStatus = false; 
        //DataSet dsReport = new dsrptActiveCall();
        ReportDataSource rds = new ReportDataSource();
        string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "rptActiveCall.sdf";
        string strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "rptActiveCall.sdf";
        SqlCeConnection LocalSQLConn = null;
        string ConnectionString;
    
        public delegate void delRefreshReport();
        public delRefreshReport objRefreshReport; 
       

        public ctlrptActiveCall()
        {
            try
            {
                InitializeComponent();

                ConnectionString = VMuktiAPI.VMuktiInfo.MainConnectionString;
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "rptActiveCall.sdf"))
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory.ToString() + "rptActiveCall.sdf");
                }
                SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                clientEngine.CreateDatabase();
                LocalSQLConn = new SqlCeConnection();
                LocalSQLConn.ConnectionString = ClientConnectionString;
                LocalSQLConn.Open();
                fncActiveCallTable();
                LocalSQLConn.Close();

                objRefreshReport = new delRefreshReport(fncRefreshReport);
                NetPeerClient npcActiveCall = new NetPeerClient();
                ((NetP2PBootStrapActiveCallReportDelegates)objActiveCall).EntsvcJoinCall += new NetP2PBootStrapActiveCallReportDelegates.DelsvcJoinCall(ctlrptActiveCall_EntsvcJoinCall);
                ((NetP2PBootStrapActiveCallReportDelegates)objActiveCall).EntsvcGetCallInfo += new NetP2PBootStrapActiveCallReportDelegates.DelsvcGetCallInfo(ctlrptActiveCall_EntsvcGetCallInfo);
                ((NetP2PBootStrapActiveCallReportDelegates)objActiveCall).EntsvcActiveCalls += new NetP2PBootStrapActiveCallReportDelegates.DelsvcActiveCalls(ctlrptActiveCall_EntsvcActiveCalls);
                ((NetP2PBootStrapActiveCallReportDelegates)objActiveCall).EntsvcSetDuration += new NetP2PBootStrapActiveCallReportDelegates.DelsvcSetDuration(ctlrptActiveCall_EntsvcSetDuration);
                ((NetP2PBootStrapActiveCallReportDelegates)objActiveCall).EntsvcUnJoinCall += new NetP2PBootStrapActiveCallReportDelegates.DelsvcUnJoinCall(ctlrptActiveCall_EntsvcUnJoinCall);
                channelNetTcpActiveCall = (INetP2PBootStrapReportChannel)npcActiveCall.OpenClient<INetP2PBootStrapReportChannel>("net.tcp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveCallReport", "ActiveCallMesh", ref objActiveCall);
                channelNetTcpActiveCall.svcJoinCall(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveCall", "ctlrptActiveCall.xaml.cs");
            }
        }

        #region WCF Events

        void ctlrptActiveCall_EntsvcJoinCall(string uname)
        {
            try
            {
                channelNetTcpActiveCall.svcGetCallInfo(uname);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveCall_EntsvcJoinCall()", "ctlrptActiveCall.xaml.cs");
            }
        }
        void ctlrptActiveCall_EntsvcGetCallInfo(string uName)
        {

        }
        void ctlrptActiveCall_EntsvcActiveCalls(string uName,string campaignId,string groupName,string Status,string callDuration,string phoneNo)
        {
            try
            {
                string SelQuery = "select count(*) from Active_call where uName='" + uName + "'";
                OpenConnection();
                SqlCeDataAdapter adp = new SqlCeDataAdapter(SelQuery, LocalSQLConn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    string InsQuery = "insert into Active_call (uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration) values ('" + uName + "','" + campaignId + "','" + Status + "','" + groupName + "','" + phoneNo + "','" + callDuration + "')";
                    SqlCeCommand cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    cmd.ExecuteNonQuery();
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                }
                else
                {
                    string SqlUpdate = "update Active_call set Campaign_Id='" + campaignId + "',Status='" + Status + "',Group_Name='" + groupName + "',Phone_No='" + phoneNo + "',callDuration='" + callDuration + "' where uName='" + uName + "'";
                    SqlCeCommand cmd = new SqlCeCommand(SqlUpdate, LocalSQLConn);
                    cmd.ExecuteNonQuery();
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                }
                closeConnection();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveCall_EntsvcActiveCalls()", "ctlrptActiveCall.xaml.cs");
            }
           
        }
        void ctlrptActiveCall_EntsvcSetDuration(string uName, string Status, string callDuration, string phoneNo)
        {
            try
            {
                string InsQuery = "update Active_call set Status='" + Status + "',Phone_No='" + phoneNo + "',callDuration='" + callDuration + "' where uName='" + uName + "'";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                cmd.ExecuteNonQuery();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                closeConnection();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveCall_EntsvcSetDuration()", "ctlrptActiveCall.xaml.cs");
            }
        }
        void ctlrptActiveCall_EntsvcUnJoinCall(string uname)
        {
            try
            {
                string DelQuery = "delete from Active_call where uName='" + uname + "'";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(DelQuery, LocalSQLConn);
                cmd.ExecuteNonQuery();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                closeConnection();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveCall_EntsvcUnJoinCall()", "ctlrptActiveCall.xaml.cs");
            }
        }

        #endregion

        public void fncActiveCallTable()
        {
            try
            {
                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("Active_call"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblActive_call = new SyncTable("Active_call");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("Active_call");
                    syncSchemaLead.Tables["Active_call"].Columns.Add("uName");
                    syncSchemaLead.Tables["Active_call"].Columns["uName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_call"].Columns["uName"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_call"].Columns["uName"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_call"].Columns["uName"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Active_call"].Columns["uName"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_call"].PrimaryKey = new string[] { "uName" };

                    syncSchemaLead.Tables["Active_call"].Columns.Add("Campaign_Id");
                    syncSchemaLead.Tables["Active_call"].Columns["Campaign_Id"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_call"].Columns["Campaign_Id"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_call"].Columns["Campaign_Id"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_call"].Columns["Campaign_Id"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_call"].Columns["Campaign_Id"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_call"].Columns.Add("Status");
                    syncSchemaLead.Tables["Active_call"].Columns["Status"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_call"].Columns["Status"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_call"].Columns["Status"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_call"].Columns["Status"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_call"].Columns["Status"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_call"].Columns.Add("Group_Name");
                    syncSchemaLead.Tables["Active_call"].Columns["Group_Name"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_call"].Columns["Group_Name"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_call"].Columns["Group_Name"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_call"].Columns["Group_Name"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_call"].Columns["Group_Name"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_call"].Columns.Add("Phone_No");
                    syncSchemaLead.Tables["Active_call"].Columns["Phone_No"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_call"].Columns["Phone_No"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_call"].Columns["Phone_No"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_call"].Columns["Phone_No"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_call"].Columns["Phone_No"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_call"].Columns.Add("callDuration");
                    syncSchemaLead.Tables["Active_call"].Columns["callDuration"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_call"].Columns["callDuration"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_call"].Columns["callDuration"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_call"].Columns["callDuration"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_call"].Columns["callDuration"].AutoIncrementStep = 1;
                  
                    sync.CreateSchema(tblActive_call, syncSchemaLead);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncActivecallTable()", "ctlrptActiveCall.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsTableExits()", "ctlrptActiveCall.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OpenConnection()", "ctlrptActiveCall.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveCall", "ctlrptActiveCall.xaml.cs");
            }
        }

        void fncRefreshReport()
        {
            try
            {
                string selQuery = "select uName,Campaign_Id,Status,Group_Name,Phone_No,callDuration from Active_call";
                OpenConnection();
                SqlCeDataAdapter adp = new SqlCeDataAdapter(selQuery, LocalSQLConn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<ActiveCall> objActiveCall = new List<ActiveCall>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataSet dsCampName = new DataSet();
                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select Name from Campaign where ID='" + ds.Tables[0].Rows[i]["Campaign_Id"].ToString() + "'", conn);
                    System.Data.SqlClient.SqlDataAdapter daCampName = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    daCampName.Fill(dsCampName);
                    objActiveCall.Add(ActiveCall.Create(ds.Tables[0].Rows[i]["uName"].ToString(), dsCampName.Tables[0].Rows[0][0].ToString(), ds.Tables[0].Rows[i]["Status"].ToString(), ds.Tables[0].Rows[i]["Group_Name"].ToString(), ds.Tables[0].Rows[i]["Phone_No"].ToString(), ds.Tables[0].Rows[i]["callDuration"].ToString()));
                }

                AgentItems.ItemsSource = objActiveCall;
                closeConnection();
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncRefreshReport()", "ctlrptActiveCall.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                channelNetTcpActiveCall.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlrptActiveCall.xaml.cs");
            }
        }
    }
}
