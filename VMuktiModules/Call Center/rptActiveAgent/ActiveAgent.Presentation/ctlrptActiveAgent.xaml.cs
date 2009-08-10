/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists      
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
 
*/
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
using VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P;
using System.IO;
using System.Reflection;
using System.Timers; 
namespace rptActiveAgent.Presentation
{
    /// <summary>
    /// Interaction logic for ctlChat.xaml
    /// </summary>
    /// 

    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlrptActiveAgent : UserControl
    {
        object objNetTcpActiveAgent = new NetP2PBootStrapActiveAgentReportDelegates();
        INetP2PBootStrapActiveAgentReportChannel channelNetTcp;
        string strUri;

        //DataSet dsReport = new dsrptActiveAgent();
        ReportDataSource rds = new ReportDataSource();
        System.Threading.Thread tHostActiveAgent = null;
        string ConnectionString;
      
        public delegate void delRefreshReport();
        public delRefreshReport objRefreshReport;

        string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "rptActiveAgent.sdf";
        string strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "rptActiveAgent.sdf";
        SqlCeConnection LocalSQLConn = null;

        public ctlrptActiveAgent(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();

                tHostActiveAgent = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostActiveAgentService));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveAgentReport");
                lstParams.Add("P2PActiveAgentMesh");
                tHostActiveAgent.Start(lstParams);
                objRefreshReport = new delRefreshReport(objdelRefreshReport);
                ConnectionString = VMuktiAPI.VMuktiInfo.MainConnectionString;

                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "rptActiveAgent.sdf"))
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory.ToString() + "rptActiveAgent.sdf");
                }
                SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                clientEngine.CreateDatabase();
                LocalSQLConn = new SqlCeConnection();
                LocalSQLConn.ConnectionString = ClientConnectionString;
                LocalSQLConn.Open();
                fncActiveAgentTable();

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
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcUnJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcUnJoin(ctlrptActiveAgent_EntsvcUnJoin);

                channelNetTcp = (INetP2PBootStrapActiveAgentReportChannel)npcActiveAgent.OpenClient<INetP2PBootStrapActiveAgentReportChannel>(strUri, lstTempObj[1].ToString(), ref objNetTcpActiveAgent);
                channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString());
                channelNetTcp.svcGetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostActiveAgentService()", "ctlrptActiveAgent.xaml.cs");
                if (ex.InnerException.Message != "")
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex.InnerException, "hostActiveAgentService()--Inner", "ctlrptActiveAgent.xaml.cs");
                }
            }
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
                    channelNetTcp.svcSetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString(), "", VMuktiAPI.VMuktiInfo.CurrentPeer.GroupName, "90897956");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcGetAgentInfo()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcSetAgentInfo(string uName, string campaignId, string status,string groupName,string phoneNo)
        {
            try
            {
                string SelQuery="select count(*) from Active_agent where uName='"+uName+"'";
                OpenConnection();
                SqlCeDataAdapter adp = new SqlCeDataAdapter(SelQuery, LocalSQLConn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    string InsQuery = "insert into Active_agent (uName,Campaign_Id,Status,Group_Name,Phone_No) values ('" + uName + "','" + campaignId + "','" + status + "','" + groupName + "','" + phoneNo + "')";
                    SqlCeCommand cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                    cmd.ExecuteNonQuery();
                    System.Threading.Thread.Sleep(1000);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                }
                else
                {
                    string SqlUpdate="update Active_agent set Campaign_Id='"+campaignId+"',Status='"+status+"',Group_Name='"+groupName+"',Phone_No='"+phoneNo+"' where uName='"+uName+"'";
                    SqlCeCommand cmd = new SqlCeCommand(SqlUpdate, LocalSQLConn);
                    cmd.ExecuteNonQuery();                   
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                 }
                closeConnection();
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcSetAgentInfo()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcSetAgentStatus(string uName, string Status,string Phone_No)
        {
            try
            {
                string InsQuery = "update Active_agent set Status='" + Status + "',Phone_No='"+Phone_No+"' where uName='" + uName + "'";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(InsQuery, LocalSQLConn);
                cmd.ExecuteNonQuery();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                closeConnection();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcSetAgentStatus()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcUnJoin(string uName)
        {
            try
            {
                //Remvoe uName from the report.
                string DelQuery = "delete from Active_agent where uName='" + uName + "'";
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand(DelQuery, LocalSQLConn);
                cmd.ExecuteNonQuery();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objRefreshReport);
                closeConnection();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcUnJoin()", "ctlrptActiveAgent.xaml.cs");
            }

        }

        #endregion

        void fncActiveAgentTable()
        {
            try
            {
                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("Active_agent"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblActive_agent = new SyncTable("Active_agent");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("Active_agent");
                    syncSchemaLead.Tables["Active_agent"].Columns.Add("uName");
                    syncSchemaLead.Tables["Active_agent"].Columns["uName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_agent"].Columns["uName"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_agent"].Columns["uName"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_agent"].Columns["uName"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Active_agent"].Columns["uName"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_agent"].PrimaryKey = new string[] { "uName" };

                    syncSchemaLead.Tables["Active_agent"].Columns.Add("Campaign_Id");
                    syncSchemaLead.Tables["Active_agent"].Columns["Campaign_Id"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_agent"].Columns["Campaign_Id"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_agent"].Columns["Campaign_Id"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_agent"].Columns["Campaign_Id"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_agent"].Columns["Campaign_Id"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_agent"].Columns.Add("Status");
                    syncSchemaLead.Tables["Active_agent"].Columns["Status"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_agent"].Columns["Status"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_agent"].Columns["Status"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_agent"].Columns["Status"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_agent"].Columns["Status"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_agent"].Columns.Add("Group_Name");
                    syncSchemaLead.Tables["Active_agent"].Columns["Group_Name"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_agent"].Columns["Group_Name"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_agent"].Columns["Group_Name"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_agent"].Columns["Group_Name"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_agent"].Columns["Group_Name"].AutoIncrementStep = 1;

                    syncSchemaLead.Tables["Active_agent"].Columns.Add("Phone_No");
                    syncSchemaLead.Tables["Active_agent"].Columns["Phone_No"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Active_agent"].Columns["Phone_No"].MaxLength = 30;
                    syncSchemaLead.Tables["Active_agent"].Columns["Phone_No"].AutoIncrement = false;
                    syncSchemaLead.Tables["Active_agent"].Columns["Phone_No"].AutoIncrementSeed = 0;
                    syncSchemaLead.Tables["Active_agent"].Columns["Phone_No"].AutoIncrementStep = 1;

                    sync.CreateSchema(tblActive_agent, syncSchemaLead);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncActiveAgentTable()", "ctlrptActiveAgent.xaml.cs");
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

        public void objdelRefreshReport()
        {
            try
            {
                string SelQuery = "select uName,Campaign_Id,Group_Name,Phone_No,Status from Active_agent";
                OpenConnection();
                List < ActiveAgent > objActiveAgent= new List<ActiveAgent>();
                DataSet ds = new DataSet();
                System.Threading.Thread.Sleep(1000);
                SqlCeDataAdapter adp = new SqlCeDataAdapter(SelQuery, LocalSQLConn);
                adp.Fill(ds);
                //dsReport = ds;
                closeConnection();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataSet dsCampName = new DataSet();
                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select Name from Campaign where ID='" + ds.Tables[0].Rows[i]["Campaign_Id"].ToString() + "'", conn);
                    System.Data.SqlClient.SqlDataAdapter daCampName = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    daCampName.Fill(dsCampName);
                    string status=ds.Tables[0].Rows[i]["Status"].ToString();
                    string Color = string.Empty;
                    switch (status)
                    {
                        case "Ready":
                            Color = "White";
                            break;

                        case "InProgress":
                            Color = "Blue";
                            break;

                        case "Connected":
                            Color = "Green";
                            break;

                        case "DisConnected":
                            Color = "Red";
                            break;

                        case "Incoming":
                            Color = "Yellow";
                            break;

                        case "Hold":
                            Color = "Brown";
                            break;

                    }
                    objActiveAgent.Add(ActiveAgent.Create(ds.Tables[0].Rows[i]["uName"].ToString(), dsCampName.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i]["Group_Name"].ToString(), ds.Tables[0].Rows[i]["Phone_No"].ToString(), ds.Tables[0].Rows[i]["Status"].ToString(), Color));
                }

                AgentItems.ItemsSource = objActiveAgent;
                

            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objdelRefreshReport()", "ctlrptActiveAgent.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                channelNetTcp.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlrptActiveAgent.xaml.cs");
            }
        }

    }
}








