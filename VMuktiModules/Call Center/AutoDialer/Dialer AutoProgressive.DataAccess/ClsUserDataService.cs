
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
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Synchronization.Data;
//using Microsoft.Synchronization.Data.Client;
using Microsoft.Synchronization.Data.Server;
using System.Data.SqlTypes;
using System.Windows.Forms;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data.SqlServerCe;
using System.Data.SqlServerCe;
using VMuktiAPI;

namespace Dialer_AutoProgressive.DataAccess
{
    public class ClsUserDataService : ClsDataServiceBase
    {

        public ClsUserDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsUserDataService(IDbTransaction txn) : base(txn) { }


        public DataSet User_GetAll()
        {
      //      return ExecuteDataSet("Select UserInfo.*,Payroll.* from UserInfo left outer join Payroll on UserInfo.Id=Payroll.UserId where UserInfo.IsDeleted=0;", CommandType.Text, null);
            return null;
        }

        public DataSet User_GetByID(int ID)
        {
            return null;
        //    return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        }

        // Function will fetch 10 new leads from database based on campaing assign to him/her/?.
        public DataSet GetLeadsList(long userID, out long CampaingID)//, int totalRecordToFetch)
        {
            DataSet myDS = null;
            CampaingID = Common.ClsConstants.NullLong;
            try
            {
                SqlCommand cmd;
                myDS = ExecuteDataSet(out cmd, "spGLeadsByUserID",
                        CreateParameter("@pUserID", SqlDbType.BigInt, userID, ParameterDirection.Input),
                        CreateParameter("@pCampaignID", SqlDbType.BigInt, long.Parse("1"), ParameterDirection.Output));

                //DataTable tempDT = new DataTable();
                //tempDT = myDS.Tables[1].Merge(myDS.Tables[0]);
                 myDS.Tables[0].Merge(myDS.Tables[1]);
                CampaingID = long.Parse(cmd.Parameters["@pCampaignID"].Value.ToString());
                return myDS;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLeadsList()", "AutoProgressiveDialer--:--ClsUserDataService.cs");
                CampaingID = 0;
                return null;
            }
        }
      
        // This function will save current call detail into SDF file.
        public void User_Save(out long parCallID, long leadID, DateTime calledDate, DateTime startDate, DateTime startTime, long durationInSecond, long despositionID, long campaingnID, long confID,string callNote, bool isDNC, bool isGlobal,long userID,string RecordedFileName)    
        {
            try
            {
                //SqlCommand cmd;
                //ExecuteNonQuery(out cmd, "spAECall",
                //    CreateParameter("@pID", SqlDbType.Int, ID),
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
                //    CreateParameter("@pUserID", SqlDbType.BigInt, userID));


                //cmd.Dispose();
                parCallID = 0;
                fncInsertIntoCallTable(out parCallID, leadID, calledDate, startDate, startTime, durationInSecond, despositionID, campaingnID, confID, callNote, isDNC, isGlobal, userID, RecordedFileName);
            }
            catch (Exception ex)
            {
                parCallID = -1;
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "User_Save()", "ClsUserDataService.cs");
                //throw ex;
            }
        }
        public void UpdateLeadStatus(long leadID,string Status)
        {
            try
            {
                //ExecuteDataSet("UPDATE Leads SET Status='Fresh' WHERE ID=" + leadID + "", CommandType.Text, null);
                fncUpdateLeadStatus(leadID, Status);                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UpdateLeadStatus()", "ClsUserDataService.cs");
            }
        }

        //This function will Update  DNC status if any in SDF File.
        public void UpdateDNCStatus(long leadID, long userID, bool isDNC)
        {
            try
            {
               // ExecuteDataSet("UPDATE Leads SET DNCBy=" + userID + ",DNCFlag=1 WHERE ID=" + leadID + "", CommandType.Text, null);
                fncUpdateDNCStatus(leadID, userID, isDNC);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UpdateDNCStatus()", "ClsUserDataService.cs");
            }
            //if (isDNC)            
            {
                //ExecuteDataSet("UPDATE Leads SET DNCBy=" + userID + ",DNCFlag=1 WHERE ID=" + leadID + "", CommandType.Text, null);
            }
            //else
            //{
            //    ExecuteDataSet("UPDATE Leads SET DNCBy=" + userID + ",DNCFlag=1 WHERE ID=" + leadID + "", CommandType.Text, null);

            //}   
        }
        public void SetCallBackNo(long CallID, long leadID,string callBackNote, DateTime callBackDate,bool isPublic,bool isDeleted)
        {
            try
            {
                //long callID;
                //DataSet ds = ExecuteDataSet("select ID from Call where LeadID=" + leadID + "", CommandType.Text, null);
                //callID = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                //callID = GetCallID(leadID);
                fncInsertIntoCallBackTable(CallID, callBackNote, callBackDate, isPublic, isDeleted);
                
                //SqlCommand cmd;
                //ExecuteNonQuery(out cmd, "spAECallBack",
                //    CreateParameter("@pID", SqlDbType.Int, ID),
                //    CreateParameter("@pCallID", SqlDbType.BigInt, callID),
                //    CreateParameter("@pCallBackDate", SqlDbType.DateTime, callBackDate),
                //    CreateParameter("@pComment", SqlDbType.NVarChar, callBackNote),
                //    CreateParameter("@pIsPublic", SqlDbType.Bit, isPublic),
                //    CreateParameter("@pIsDeleted", SqlDbType.Bit, isDeleted));

                //cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetCallBackNo()", "ClsUserDataService.cs");
            }
        }
        
        public void User_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDUserInfoPayroll", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "User_Delete()", "ClsUserDataService.cs");
            }
        }

        public void SIPServerIP(out string SIPIP)
        {
            try
            {
                DataSet ds = ExecuteDataSet("select IPAddress from ServerInfo where ID=(select ServerID from ActiveServerInfo)", CommandType.Text, null);
                SIPIP = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SIPServerIP()", "ClsUserDataService.cs");
                SIPIP = null;
            }
        }

        public void AgentSIPInformation(out string AgentNumber, out string AgentPassword)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAllocSIPUsers",
                    CreateParameter("@pReturnID", SqlDbType.BigInt, ParameterDirection.Output),
                    CreateParameter("@pReturnPass", SqlDbType.BigInt, ParameterDirection.Output));

                AgentNumber = cmd.Parameters["@pReturnID"].Value.ToString();
                AgentPassword = cmd.Parameters["@pReturnPass"].Value.ToString();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AgentSIPInformation()", "ClsUserDataService.cs");
                AgentNumber = null;
                AgentPassword = null;
            }
        }

        public void DeallocateSIPInformation(long SIPNumber)
        {
            try
            {
                ExecuteNonQuery("spDeAllocSIPUsers", CreateParameter("@pID", SqlDbType.BigInt, SIPNumber));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DeallocateSIPInformation()", "ClsUserDataService.cs");
            }
        }

        public long FncGetCountryCode(long CampID)
        {
            try
            {
                DataSet ds = ExecuteDataSet("select CountryCode from Country where Id= (select CountryId from Location where Id =(select min(LocationId) from Leads where ListId=(select min(ListId) from CampaignCallingList where CampaignId='" + CampID + "')))", CommandType.Text, null);
                long Code = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                return Code;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetCountryCode()", "ClsUserDataService.cs");
                return 0;
            }
        }

        public string FncGetCampaginPrefix(long CampID)
        {
            try
            {
                DataSet ds = ExecuteDataSet("select CampaignPrefix from Campaign where Id='" + CampID + "'", CommandType.Text, null);
                string Prefix = ds.Tables[0].Rows[0][0].ToString();
                return Prefix;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetCampaginPrefix", "ClsUserDataService.cs");
                return null;
            }
        }
         

        #region Sync Functions

        string strClientConnectionString = string.Empty;
        DbServerSyncProvider serverSyncProvider = null;
        SyncAgent syncAgent = null;
        SqlConnection serverConnection = null;
        SqlCeClientSyncProvider clientSyncProvider = null;

        SyncTable tblCallTable = null;
        SyncTable tblLeadsTable = null;
        SyncTable tblCallBackTable = null;
        SyncTable tblDispositionTable = null;   
       
       // SqlCeClientSyncProvider sync = null;
        SyncGroup myGroup = null;
       
        SqlSyncAdapterBuilder CallAdapter = null;
        SqlSyncAdapterBuilder CallBackAdapter = null;
        SqlSyncAdapterBuilder LeadAdapter = null;
        SqlSyncAdapterBuilder DispositionAdapter = null;    
        
        SyncAdapter CallAdapterSyncAdapter = null;
        SyncAdapter CallBackAdapterSyncAdapter = null;
        SyncAdapter LeadAdapterSyncAdapter = null;
        SyncAdapter DispositionAdapterSyncAdapter = null;   

        SqlCeConnection ce = null;

        public void CreateInitialLocalDB(string strConnectionString,bool isCreated)
        {
            try
            {
                strClientConnectionString = strConnectionString;
                
               // sync = new SqlCeClientSyncProvider(strClientConnectionString);
                clientSyncProvider = new SqlCeClientSyncProvider(strClientConnectionString);
                if (!isCreated)
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(strClientConnectionString);
                    clientEngine.CreateDatabase();
                    clientEngine.Dispose();                    
                    tblCallTable = CreateCallTable();
                    tblLeadsTable = CreateLeadsTable();
                    tblCallBackTable = CreateCallBackTable();
                    tblDispositionTable = CreateDispositionTable();
                }
                else
                {
                    tblCallTable = new SyncTable("Call");
                    tblCallTable.SyncDirection = SyncDirection.UploadOnly;

                    tblLeadsTable = new SyncTable("Leads");
                    tblLeadsTable.SyncDirection = SyncDirection.UploadOnly;

                    tblCallBackTable = new SyncTable("CallBack");
                    tblCallBackTable.SyncDirection = SyncDirection.UploadOnly;

                     //Creating Disposition Table (Added by Alpa)
                    tblDispositionTable = new SyncTable("Disposition");
                    tblDispositionTable.SyncDirection = SyncDirection.UploadOnly;
                }
                strClientConnectionString = strConnectionString;

               // sync = new SqlCeClientSyncProvider(strClientConnectionString);

                serverSyncProvider = new DbServerSyncProvider();

                syncAgent = new SyncAgent();
              //  syncAgent.ServerSyncProvider = serverSyncProvider;
                syncAgent.RemoteProvider = serverSyncProvider;
                
                serverConnection = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                serverSyncProvider.Connection = serverConnection;
                serverSyncProvider.ApplyChangeFailed += new EventHandler<ApplyChangeFailedEventArgs>(serverSyncProvider_ApplyChangeFailed);
                
         
               
                //syncAgent.ClientSyncProvider = clientSyncProvider;
                syncAgent.LocalProvider = clientSyncProvider;
                myGroup = new SyncGroup("DialerGroup");
                tblCallTable.SyncGroup = myGroup;
                tblLeadsTable.SyncGroup = myGroup;
                tblCallBackTable.SyncGroup = myGroup;
                 tblDispositionTable.SyncGroup = myGroup;



                syncAgent.Configuration.SyncTables.Add(tblCallTable);
                syncAgent.Configuration.SyncTables.Add(tblLeadsTable);
                syncAgent.Configuration.SyncTables.Add(tblCallBackTable);
                syncAgent.Configuration.SyncTables.Add(tblDispositionTable);

                
                CallAdapter = new SqlSyncAdapterBuilder();
                CallAdapter.Connection = serverConnection;
                CallAdapter.SyncDirection = SyncDirection.UploadOnly;
                CallAdapter.TableName = "Call";
               // CallAdapter.DataColumns.Add("ID");
                CallAdapter.DataColumns.Add("LeadID");
                CallAdapter.DataColumns.Add("CalledDate");
                CallAdapter.DataColumns.Add("ModifiedDate");
                CallAdapter.DataColumns.Add("ModifiedBy");
                CallAdapter.DataColumns.Add("GeneratedBy");
                CallAdapter.DataColumns.Add("StartDate");
                CallAdapter.DataColumns.Add("StartTime");
                CallAdapter.DataColumns.Add("DurationInSecond");
                CallAdapter.DataColumns.Add("DespositionID");
                CallAdapter.DataColumns.Add("CampaignID");
                CallAdapter.DataColumns.Add("ConfID");
                CallAdapter.DataColumns.Add("IsDeleted");
                CallAdapter.DataColumns.Add("CallNote");
                CallAdapter.DataColumns.Add("IsDNC");
                CallAdapter.DataColumns.Add("IsGlobal");
				CallAdapter.DataColumns.Add("RecordedFileName");    //For Recording File Name
                CallAdapterSyncAdapter = CallAdapter.ToSyncAdapter();
                CallAdapterSyncAdapter.DeleteCommand = null;
                serverSyncProvider.SyncAdapters.Add(CallAdapterSyncAdapter);


                LeadAdapter = new SqlSyncAdapterBuilder();
                LeadAdapter.Connection = serverConnection;
                LeadAdapter.SyncDirection = SyncDirection.UploadOnly;
                LeadAdapter.TableName = "Leads";
                LeadAdapter.DataColumns.Add("ID");
                LeadAdapter.DataColumns.Add("PhoneNo");
                LeadAdapter.DataColumns.Add("LeadFormatID");
                LeadAdapter.DataColumns.Add("CreatedDate");
                LeadAdapter.DataColumns.Add("CreatedBy");
                LeadAdapter.DataColumns.Add("DeletedDate");
                LeadAdapter.DataColumns.Add("DeletedBy");
                LeadAdapter.DataColumns.Add("IsDeleted");
                LeadAdapter.DataColumns.Add("ModifiedDate");
                LeadAdapter.DataColumns.Add("ModifiedBy");
                LeadAdapter.DataColumns.Add("DNCFlag");
                LeadAdapter.DataColumns.Add("DNCBy");
                LeadAdapter.DataColumns.Add("ListID");
                LeadAdapter.DataColumns.Add("LocationID");
                LeadAdapter.DataColumns.Add("RecycleCount");
                LeadAdapter.DataColumns.Add("Status");
                LeadAdapter.DataColumns.Add("IsGlobalDNC");
                //LeadAdapter.DataColumns.Add("LastEditDate");
                //LeadAdapter.DataColumns.Add("CreationDate");
                LeadAdapterSyncAdapter = LeadAdapter.ToSyncAdapter();

                LeadAdapterSyncAdapter.DeleteCommand = null;
                LeadAdapterSyncAdapter.InsertCommand = null;
                serverSyncProvider.SyncAdapters.Add(LeadAdapterSyncAdapter);
                


                CallBackAdapter = new SqlSyncAdapterBuilder();
                CallBackAdapter.Connection = serverConnection;
                CallBackAdapter.SyncDirection = SyncDirection.UploadOnly;
                CallBackAdapter.TableName = "CallBack";
                CallBackAdapter.DataColumns.Add("ID");
                CallBackAdapter.DataColumns.Add("CallID");
                CallBackAdapter.DataColumns.Add("CallBackDate");
                CallBackAdapter.DataColumns.Add("Comment");
                CallBackAdapter.DataColumns.Add("IsPublic");
                CallBackAdapter.DataColumns.Add("IsDeleted");
                CallBackAdapterSyncAdapter = CallBackAdapter.ToSyncAdapter();
                CallBackAdapterSyncAdapter.DeleteCommand = null;
                serverSyncProvider.SyncAdapters.Add(CallBackAdapterSyncAdapter);

                //Creating Disposition Table in sdf (Added by Alpa)

                DispositionAdapter = new SqlSyncAdapterBuilder();
                DispositionAdapter.Connection = serverConnection;
                DispositionAdapter.SyncDirection = SyncDirection.UploadOnly;
                DispositionAdapter.TableName = "Disposition";
                DispositionAdapter.DataColumns.Add("ID");
                DispositionAdapter.DataColumns.Add("DespositionName");
                DispositionAdapter.DataColumns.Add("Description");
                DispositionAdapter.DataColumns.Add("IsActive");
                DispositionAdapter.DataColumns.Add("IsDeleted");
                DispositionAdapter.DataColumns.Add("CreatedDate");
                DispositionAdapter.DataColumns.Add("CreatedBy");
                DispositionAdapter.DataColumns.Add("ModifiedDate");
                DispositionAdapter.DataColumns.Add("ModifiedBy");
                DispositionAdapterSyncAdapter = DispositionAdapter.ToSyncAdapter();
                DispositionAdapterSyncAdapter.DeleteCommand = null;
                DispositionAdapterSyncAdapter.InsertCommand = null; 
                serverSyncProvider.SyncAdapters.Add(DispositionAdapterSyncAdapter);

             
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
                CheckPreviousSyncWithServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
      
        void serverSyncProvider_ApplyChangeFailed(object sender, ApplyChangeFailedEventArgs e)
        {
            //MessageBox.Show(e.Conflict.ErrorMessage);
        }

        public void CheckPreviousSyncWithServer()
        {
            try
            {
                if (IsTableExits("Leads"))
                {
                    SqlCeCommand cmd = new SqlCeCommand("Select Count(*) From Leads", ce);
                    object i = cmd.ExecuteScalar();
                    if (int.Parse(i.ToString()) > 0)
                    {
                        cmd = new SqlCeCommand("Update Leads Set Status='Fresh' Where Status<>'Called' and Status<>'CallBack'", ce);
                        cmd.ExecuteNonQuery();
                        SynchronizeWithServer();

                        // Remove All Entries from lead table 

                        cmd = new SqlCeCommand("Delete From Leads", ce);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public SyncTable CreateCallTable()
        {
            try
            {
                //  if (!IsTableExits("Call"))
                {
                    tblCallTable = new SyncTable("Call");
                    tblCallTable.SyncDirection = SyncDirection.UploadOnly;
                    

                    SyncSchema syncSchemaCall= new SyncSchema();
                    syncSchemaCall.Tables.Add("Call");

                    syncSchemaCall.Tables["Call"].Columns.Add("ID");
                    syncSchemaCall.Tables["Call"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].PrimaryKey = new string[] { "ID" };
                    syncSchemaCall.Tables["Call"].Columns["ID"].AutoIncrement = true;
                    syncSchemaCall.Tables["Call"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaCall.Tables["Call"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaCall.Tables["Call"].Columns["ID"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("LeadID");
                    syncSchemaCall.Tables["Call"].Columns["LeadID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["LeadID"].AllowNull = false;


                    syncSchemaCall.Tables["Call"].Columns.Add("CalledDate");
                    syncSchemaCall.Tables["Call"].Columns["CalledDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaCall.Tables["Call"].Columns["CalledDate"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("ModifiedDate");
                    syncSchemaCall.Tables["Call"].Columns["ModifiedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaCall.Tables["Call"].Columns["ModifiedDate"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("ModifiedBy");
                    syncSchemaCall.Tables["Call"].Columns["ModifiedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["ModifiedBy"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("GeneratedBy");
                    syncSchemaCall.Tables["Call"].Columns["GeneratedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["GeneratedBy"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("StartDate");
                    syncSchemaCall.Tables["Call"].Columns["StartDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaCall.Tables["Call"].Columns["StartDate"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("StartTime");
                    syncSchemaCall.Tables["Call"].Columns["StartTime"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaCall.Tables["Call"].Columns["StartTime"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("DurationInSecond");
                    syncSchemaCall.Tables["Call"].Columns["DurationInSecond"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["DurationInSecond"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("DespositionID");
                    syncSchemaCall.Tables["Call"].Columns["DespositionID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["DespositionID"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("CampaignID");
                    syncSchemaCall.Tables["Call"].Columns["CampaignID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["CampaignID"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("ConfID");
                    syncSchemaCall.Tables["Call"].Columns["ConfID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCall.Tables["Call"].Columns["ConfID"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("IsDeleted");
                    syncSchemaCall.Tables["Call"].Columns["IsDeleted"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaCall.Tables["Call"].Columns["IsDeleted"].AllowNull = false;

                    syncSchemaCall.Tables["Call"].Columns.Add("CallNote");
                    syncSchemaCall.Tables["Call"].Columns["CallNote"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaCall.Tables["Call"].Columns["CallNote"].MaxLength = 50;
                    syncSchemaCall.Tables["Call"].Columns["CallNote"].AllowNull = true;

                    syncSchemaCall.Tables["Call"].Columns.Add("IsDNC");
                    syncSchemaCall.Tables["Call"].Columns["IsDNC"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaCall.Tables["Call"].Columns["IsDNC"].AllowNull = true;

                    syncSchemaCall.Tables["Call"].Columns.Add("IsGlobal");
                    syncSchemaCall.Tables["Call"].Columns["IsGlobal"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaCall.Tables["Call"].Columns["IsGlobal"].AllowNull = true;

                    syncSchemaCall.Tables["Call"].Columns.Add("RecordedFileName");
                    syncSchemaCall.Tables["Call"].Columns["RecordedFileName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaCall.Tables["Call"].Columns["RecordedFileName"].MaxLength = 100;
                    syncSchemaCall.Tables["Call"].Columns["RecordedFileName"].AllowNull = true;

                    //sync.CreateSchema(tblCallTable, syncSchemaCall);
                    clientSyncProvider.CreateSchema(tblCallTable, syncSchemaCall);
                    return tblCallTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }

        }

        private SyncTable CreateLeadsTable()
        {
            try
            {

              //  if (!IsTableExits("Leads"))
                {
              //      SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    tblLeadsTable = new SyncTable("Leads");
                    tblLeadsTable.SyncDirection = SyncDirection.UploadOnly;

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("Leads");

                    syncSchemaLead.Tables["Leads"].Columns.Add("ID");
                    syncSchemaLead.Tables["Leads"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Leads"].PrimaryKey = new string[] { "ID" };
                    //syncSchemaLead.Tables["Leads"].Columns["ID"].AutoIncrement = true;
                    //syncSchemaLead.Tables["Leads"].Columns["ID"].AutoIncrementSeed = 1;
                    //syncSchemaLead.Tables["Leads"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Leads"].Columns["ID"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("PhoneNo");
                    syncSchemaLead.Tables["Leads"].Columns["PhoneNo"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["PhoneNo"].AllowNull = false;


                    syncSchemaLead.Tables["Leads"].Columns.Add("LeadFormatID");
                    syncSchemaLead.Tables["Leads"].Columns["LeadFormatID"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["LeadFormatID"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("CreatedDate");
                    syncSchemaLead.Tables["Leads"].Columns["CreatedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["CreatedDate"].AllowNull = true;

                    syncSchemaLead.Tables["Leads"].Columns.Add("CreatedBy");
                    syncSchemaLead.Tables["Leads"].Columns["CreatedBy"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["CreatedBy"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("DeletedDate");
                    syncSchemaLead.Tables["Leads"].Columns["DeletedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["DeletedDate"].AllowNull = true;

                    syncSchemaLead.Tables["Leads"].Columns.Add("DeletedBy");
                    syncSchemaLead.Tables["Leads"].Columns["DeletedBy"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["DeletedBy"].AllowNull = true;

                    syncSchemaLead.Tables["Leads"].Columns.Add("IsDeleted");
                    syncSchemaLead.Tables["Leads"].Columns["IsDeleted"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["IsDeleted"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("ModifiedDate");
                    syncSchemaLead.Tables["Leads"].Columns["ModifiedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["ModifiedDate"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("ModifiedBy");
                    syncSchemaLead.Tables["Leads"].Columns["ModifiedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["ModifiedBy"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("DNCFlag");
                    syncSchemaLead.Tables["Leads"].Columns["DNCFlag"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["DNCFlag"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("DNCBy");
                    syncSchemaLead.Tables["Leads"].Columns["DNCBy"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["DNCBy"].AllowNull = true;

                    syncSchemaLead.Tables["Leads"].Columns.Add("ListID");
                    syncSchemaLead.Tables["Leads"].Columns["ListID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["ListID"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("LocationID");
                    syncSchemaLead.Tables["Leads"].Columns["LocationID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["LocationID"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("RecycleCount");
                    syncSchemaLead.Tables["Leads"].Columns["RecycleCount"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["RecycleCount"].AllowNull = false;

                    syncSchemaLead.Tables["Leads"].Columns.Add("Status");
                    syncSchemaLead.Tables["Leads"].Columns["Status"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["Status"].MaxLength = 50;
                    syncSchemaLead.Tables["Leads"].Columns["Status"].AllowNull = true;

                    syncSchemaLead.Tables["Leads"].Columns.Add("IsGlobalDNC");
                    syncSchemaLead.Tables["Leads"].Columns["IsGlobalDNC"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaLead.Tables["Leads"].Columns["IsGlobalDNC"].AllowNull = true;

                    //syncSchemaLead.Tables["Leads"].Columns.Add("LastEditDate");
                    //syncSchemaLead.Tables["Leads"].Columns["LastEditDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    //syncSchemaLead.Tables["Leads"].Columns["LastEditDate"].AllowNull = false;

                    //syncSchemaLead.Tables["Leads"].Columns.Add("CreationDate");
                    //syncSchemaLead.Tables["Leads"].Columns["CreationDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    //syncSchemaLead.Tables["Leads"].Columns["CreationDate"].AllowNull = false;


                  //  sync.CreateSchema(tblLeadsTable, syncSchemaLead);
                    clientSyncProvider.CreateSchema(tblLeadsTable, syncSchemaLead);
                    return tblLeadsTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Creating Leads Table");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateLeadsTable()", "AutoDialer_ClsUserDataService.xaml.cs");
                return null;
            }
        }

        private SyncTable CreateDispositionTable()
        {
            try
            {
                tblDispositionTable = new SyncTable("Disposition");
                tblDispositionTable.SyncDirection = SyncDirection.UploadOnly;

                SyncSchema syncSchemaDisposition = new SyncSchema();
                syncSchemaDisposition.Tables.Add("Disposition");

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("ID");
                syncSchemaDisposition.Tables["Disposition"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                syncSchemaDisposition.Tables["Disposition"].PrimaryKey = new string[] { "ID" };
                //syncSchemaDisposition.Tables["Disposition"].Columns["ID"].AutoIncrement = true;
                //syncSchemaDisposition.Tables["Disposition"].Columns["ID"].AutoIncrementSeed = 1;
                //syncSchemaDisposition.Tables["Disposition"].Columns["ID"].AutoIncrementStep = 1;
                syncSchemaDisposition.Tables["Disposition"].Columns["ID"].AllowNull = false;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("DespositionName");
                syncSchemaDisposition.Tables["Disposition"].Columns["DespositionName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["DespositionName"].MaxLength = 50;
                syncSchemaDisposition.Tables["Disposition"].Columns["DespositionName"].AllowNull = false;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("Description");
                syncSchemaDisposition.Tables["Disposition"].Columns["Description"].ProviderDataType = SqlDbType.NVarChar.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["Description"].MaxLength = 250;
                syncSchemaDisposition.Tables["Disposition"].Columns["Description"].AllowNull = true;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("IsActive");
                syncSchemaDisposition.Tables["Disposition"].Columns["IsActive"].ProviderDataType = SqlDbType.Bit.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["IsActive"].AllowNull = true;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("IsDeleted");
                syncSchemaDisposition.Tables["Disposition"].Columns["IsDeleted"].ProviderDataType = SqlDbType.Bit.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["IsDeleted"].AllowNull = true;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("CreatedDate");
                syncSchemaDisposition.Tables["Disposition"].Columns["CreatedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["CreatedDate"].AllowNull = true;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("CreatedBy");
                syncSchemaDisposition.Tables["Disposition"].Columns["CreatedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["CreatedBy"].AllowNull = true;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("ModifiedDate");
                syncSchemaDisposition.Tables["Disposition"].Columns["ModifiedDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["ModifiedDate"].AllowNull = true;

                syncSchemaDisposition.Tables["Disposition"].Columns.Add("ModifiedBy");
                syncSchemaDisposition.Tables["Disposition"].Columns["ModifiedBy"].ProviderDataType = SqlDbType.BigInt.ToString();
                syncSchemaDisposition.Tables["Disposition"].Columns["ModifiedBy"].AllowNull = true;

                clientSyncProvider.CreateSchema(tblDispositionTable,syncSchemaDisposition);
                return tblDispositionTable;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Creating Disposition Table");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateDispositionTable()", "AutoDialer_ClsUserDataService.xaml.cs");

                return null;
            }
        }


        private SyncTable CreateCallBackTable()
        {
            try
            {

             //   if (!IsTableExits("CallBack"))
                {
            //        SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    tblCallBackTable = new SyncTable("CallBack");
                    tblCallBackTable.SyncDirection = SyncDirection.UploadOnly;

                    SyncSchema syncSchemaCallBack = new SyncSchema();
                    syncSchemaCallBack.Tables.Add("CallBack");

                    syncSchemaCallBack.Tables["CallBack"].Columns.Add("ID");
                    syncSchemaCallBack.Tables["CallBack"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCallBack.Tables["CallBack"].PrimaryKey = new string[] { "ID" };
                    syncSchemaCallBack.Tables["CallBack"].Columns["ID"].AutoIncrement = true;
                    syncSchemaCallBack.Tables["CallBack"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaCallBack.Tables["CallBack"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaCallBack.Tables["CallBack"].Columns["ID"].AllowNull = false;

                    syncSchemaCallBack.Tables["CallBack"].Columns.Add("CallID");
                    syncSchemaCallBack.Tables["CallBack"].Columns["CallID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaCallBack.Tables["CallBack"].Columns["CallID"].AllowNull = false;


                    syncSchemaCallBack.Tables["CallBack"].Columns.Add("CallBackDate");
                    syncSchemaCallBack.Tables["CallBack"].Columns["CallBackDate"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaCallBack.Tables["CallBack"].Columns["CallBackDate"].AllowNull = false;

                    syncSchemaCallBack.Tables["CallBack"].Columns.Add("Comment");
                    syncSchemaCallBack.Tables["CallBack"].Columns["Comment"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaCallBack.Tables["CallBack"].Columns["Comment"].MaxLength = 50;
                    syncSchemaCallBack.Tables["CallBack"].Columns["Comment"].AllowNull = true;

                    syncSchemaCallBack.Tables["CallBack"].Columns.Add("IsPublic");
                    syncSchemaCallBack.Tables["CallBack"].Columns["IsPublic"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaCallBack.Tables["CallBack"].Columns["IsPublic"].AllowNull = false;

                    syncSchemaCallBack.Tables["CallBack"].Columns.Add("IsDeleted");
                    syncSchemaCallBack.Tables["CallBack"].Columns["IsDeleted"].ProviderDataType = SqlDbType.Bit.ToString();
                    syncSchemaCallBack.Tables["CallBack"].Columns["IsDeleted"].AllowNull = false;

                   // sync.CreateSchema(tblCallBackTable, syncSchemaCallBack);
                    clientSyncProvider.CreateSchema(tblCallBackTable, syncSchemaCallBack);
                    return tblCallBackTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Creating CallBack Table");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateCallBackTable()", "AutoDialer_ClsUserDataService.xaml.cs");
                return null;
            }
        }

        private bool IsTableExits(string strTableName)
        {
           
            try
            {
                string str = "SELECT COUNT(*) FROM Information_Schema.Tables WHERE (TABLE_NAME ='" + strTableName + "')";                
                SqlCeCommand cmd = new SqlCeCommand(str, ce);

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
                MessageBox.Show("Exception In Table Checking Table Status");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsTableExits()", "AutoDialer_ClsUserDataService.xaml.cs");
                return false;
            }
            //finally
            //{
            //    CloseConnection(ceConn);
            //}

        }

        public void SynchronizeWithServer()
        {
            try
            {
                SyncStatistics syncStatics = syncAgent.Synchronize(); 
                RemoveSyncData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //RemoveSyncData();
        }

        public void RemoveSyncData()
        {
            try
            {
                // Delete From Lead Table
                SqlCeCommand cmd = new SqlCeCommand("Delete From Leads Where Status='Called'", ce);
                cmd.ExecuteNonQuery();

                // Delete From Call Table
                cmd = new SqlCeCommand("Delete From Call", ce);
                cmd.ExecuteNonQuery();

                // Delete From CallTable Table
                cmd = new SqlCeCommand("Delete From CallBack", ce); 
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void fncInsertIntoCallTable(out long parCallID,long leadID, DateTime calledDate, DateTime startDate, DateTime startTime, long durationInSecond, long despositionID, long campaingnID, long confID, string callNote, bool isDNC, bool isGlobal, long userID, string RecordedFileName)
        {
            try
            {
                DateTime AdminCallBackTime = Common.ClsConstants.NullDateTime;
                parCallID = -1;
                if (!IsRecordExists(leadID.ToString(), "Call", "LeadID"))
                {
                    string SetLeadStatusTo = Common.ClsConstants.NullString;
                    SqlCommand cmd;
                    ExecuteDataSet(out cmd, "spAECall",
                        CreateParameter("@pID",SqlDbType.Int,-1),
                        CreateParameter("@pLeadID", SqlDbType.BigInt, leadID),
                        CreateParameter("@pCalledDate", SqlDbType.DateTime, calledDate),
                        CreateParameter("@pStartDate", SqlDbType.DateTime, startDate),
                        CreateParameter("@pStartTime", SqlDbType.DateTime, startTime),
                        CreateParameter("@pDurationInSecond", SqlDbType.BigInt, durationInSecond),
                        CreateParameter("@pDespositionID", SqlDbType.BigInt, despositionID),
                        CreateParameter("@pCampaignID", SqlDbType.BigInt, campaingnID),
                        CreateParameter("@pConfID", SqlDbType.BigInt, confID),
                        CreateParameter("@pCallNote", SqlDbType.NVarChar, callNote),
                        CreateParameter("@pIsDNC", SqlDbType.Bit, isDNC),
                        CreateParameter("@pIsGlobal", SqlDbType.Bit, isGlobal),
                        CreateParameter("@pUserID", SqlDbType.BigInt, userID),
                        CreateParameter("@pRecordedFileName", SqlDbType.NVarChar, RecordedFileName),
                        CreateParameter("@pCallID", SqlDbType.BigInt, -1,ParameterDirection.Output),
                        CreateParameter("@pCallBackTime", SqlDbType.DateTime, Convert.ToDateTime("05/05/2005"), ParameterDirection.Output),
                        CreateParameter("@pSetLeadStatusTo", SqlDbType.NVarChar, "Called",30, ParameterDirection.Output));

                    parCallID = long.Parse(cmd.Parameters["@pCallID"].Value.ToString());

                    if (cmd.Parameters["@pCallBackTime"].Value.ToString() != "")
                    {
                        SetLeadStatusTo = cmd.Parameters["@pSetLeadStatusTo"].Value.ToString();
                        AdminCallBackTime = (DateTime)(cmd.Parameters["@pCallBackTime"].Value);
                    }
                    cmd.Dispose();

                //    string strInsertStatement = "INSERT INTO Call (LeadID,CalledDate,ModifiedDate,ModifiedBy,"
                //+ "GeneratedBy,StartDate,StartTime,DurationInSecond,DespositionID,"
                //+ "CampaignID,ConfID,IsDeleted,CallNote,IsDNC,IsGlobal,RecordedFileName)" +
                //"VALUES(" + leadID + ",'" + calledDate + "','" + calledDate + "'," + userID + ","
                //+ userID + ",'" + startDate + "','" + startTime + "'," + durationInSecond + "," + despositionID + ","
                //+ campaingnID + "," + confID + ",0,'" + callNote + "','" + isDNC + "','" + isGlobal + "','"+ RecordedFileName +"')";

                //    SqlCeCommand cmd = new SqlCeCommand(strInsertStatement, ce);
                //    cmd.ExecuteNonQuery();

                    if (despositionID == 6)
                    {
                        fncUpdateLeadStatus(leadID, "CallBack");
                    }
                    else if (SetLeadStatusTo == "CallBackByAdmin")
                    {
                        fncUpdateLeadStatus(leadID, "CallBack");
                        fncInsertIntoCallBackTable(parCallID, "Treatment", AdminCallBackTime, false, false);
                    }
                    else
                    {
                        fncUpdateLeadStatus(leadID, "Called");
                    }
                }
            }
            catch (Exception ex)
            {
                parCallID = -1;
                MessageBox.Show("Error In Inserting Value in Call Table.");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertIntoCallTable()", "AutoDialer_ClsUserDataService.xaml.cs");

            }
            //finally
            //{
            //  CloseConnection(ceConn);
            //}
        }

        public void fncUpdateLeadStatus(long leadID, string status)
        
        {
         //   SqlCeConnection ceConn = null;
            try
            {

              //  ceConn = new SqlCeConnection(strClientConnectionString);
             //   ceConn.Open();
                if (IsRecordExists(leadID.ToString(), "Leads", "ID"))
                {
                    SqlCeCommand cmd = new SqlCeCommand("Update Leads Set Status='"+status+"' where ID=" + leadID, ce);                   
                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Updating Lead Status.");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncUpdateLeadStatus()", "AutoDialer_ClsUserDataService.xaml.cs");

            }
           // finally
           // {
           //     CloseConnection(ceConn);
           // }
        }

        public void fncDeleteFromLead(long leadID)
        {
            //   SqlCeConnection ceConn = null;
            try
            {

                //  ceConn = new SqlCeConnection(strClientConnectionString);
                //   ceConn.Open();
                if (IsRecordExists(leadID.ToString(), "Leads", "ID"))
                {
                    SqlCeCommand cmd = new SqlCeCommand("Delete From Leads Where ID=" + leadID, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Updating Lead Status.");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncDeleteFromLead()", "AutoDialer_ClsUserDataService.xaml.cs");

            }
            // finally
            // {
            //     CloseConnection(ceConn);
            // }
        }

        public void CloseConnection()
        {
            try
            {
                RemoveSyncData();
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void fncInsertIntoCallBackTable(long callID, string callBackNote, DateTime callBackDate, bool isPublic, bool isDeleted)
        {
           // SqlCeConnection ceConn = null;
            try
            {

                //ceConn = new SqlCeConnection(strClientConnectionString);
                //ceConn.Open();
                if (!IsRecordExists(callID.ToString(), "CallBack", "CallID"))
                {
                    string strInsertStatement = "INSERT INTO CallBack(CallID, callBackDate, Comment, IsPublic, IsDeleted )" +
                    "VALUES(" + callID + ",'" + callBackDate + "','" + callBackNote + "','" + isPublic + "' , 0 )";

                    SqlCeCommand cmd = new SqlCeCommand(strInsertStatement, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Inserting Value of CallBack Table.");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertIntoCallBackTable()", "AutoDialer_ClsUserDataService.xaml.cs");

            }
            //finally
            //{
            //    CloseConnection(ce);
            //}
        }

        public void fncUpdateDNCStatus(long leadID, long userID, bool isDNC)
        {
            //SqlCeConnection ceConn = null;
            try
            {

                //ceConn = new SqlCeConnection(strClientConnectionString);
                //ceConn.Open();
                if (IsRecordExists(leadID.ToString(), "Leads", "ID"))
                {
                    string strUpdateStatement = "UPDATE Leads SET DNCBy=" + userID + ",DNCFlag=1 WHERE ID=" + leadID + "";

                    SqlCeCommand cmd = new SqlCeCommand(strUpdateStatement, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In Updating Value of DNC in Leads Table.");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncUpdateDNCStatus()", "AutoDialer_ClsUserDataService.xaml.cs");

            }
            //finally
            //{
            //    CloseConnection(ce);
            //}
        }

        private bool IsRecordExists(string strValue, string strTableName, string strKey)
        {
         //   SqlCeConnection ce = null;
            try
            {

                //ce = new SqlCeConnection(strClientConnectionString);
                //ce.Open();
                string str = "SELECT COUNT(*) FROM " + strTableName + " WHERE (" + strKey + " ='" + strValue + "')";
                SqlCeCommand cmd = new SqlCeCommand(str, ce);

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
                MessageBox.Show("Exception In Finding Row in table Entry");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsRecordExists()", "AutoDialer_ClsUserDataService.xaml.cs");

                return false;
            }
            //finally
            //{
            //    CloseConnection(ce);
            //}

        }

        private long GetCallID(long leadID)
        {
          //  SqlCeConnection ce = null;
            try
            {

                //ce = new SqlCeConnection(strClientConnectionString);
                //ce.Open();
                string str = "select ID from Call where LeadID=" + leadID + "";
                SqlCommand cmd = new SqlCommand(str,serverConnection);
                serverConnection.Open();
                object i = cmd.ExecuteScalar();
                serverConnection.Close();
                return long.Parse(i.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception In Finding Row in table Entry");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCallID()", "AutoDialer_ClsUserDataService.xaml.cs");

                return 0;
            }
            //finally
            //{
            //    CloseConnection(ce);
            //}

        }

        public void fncInsertIntoLeadTable(long ID, long PhoneNo, int LeadFormatID, DateTime CreatedDate, int CreatedBy,
            //bool IsDeleted, DateTime ModifiedDate, int ModifiedBy, bool DNCFlag, int DNCBy, long ListID, long LocationID, int RecycleCount, string Status, bool IsGlobalDNC, DateTime LastEditDate, DateTime CreationDate)
            bool IsDeleted, DateTime ModifiedDate, int ModifiedBy, bool DNCFlag, int DNCBy, long ListID, long LocationID, int RecycleCount, string Status, bool IsGlobalDNC)
        {
            try
            {
                string s = CreatedDate.ToString("MM/dd/yyyy");
                CreatedDate = DateTime.Parse(s);
                if (!IsRecordExists(ID.ToString(), "Leads", "ID"))
                {
                    string sInsertStatement = "INSERT INTO Leads"
                          + "(ID, PhoneNo, LeadFormatID, CreatedDate, CreatedBy, IsDeleted, ModifiedDate, ModifiedBy, DNCFlag, DNCBy, ListID,"
                          + "LocationID, RecycleCount, Status, IsGlobalDNC)"
                          + "VALUES(" + ID + "," + PhoneNo + "," + LeadFormatID + ",'" + CreatedDate + "'," + CreatedBy + ",'" + IsDeleted + "','" + ModifiedDate + "'," + ModifiedBy + ",'" + DNCFlag + "',"
                          + DNCBy + "," + ListID + "," + LocationID + "," + RecycleCount + ", '" + Status + "','" + IsGlobalDNC + "')";

                    SqlCeCommand cmd = new SqlCeCommand(sInsertStatement, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


#endregion

        public void fncInsertDispositionTable(long CampaingID)
        {
            try
            {
                string delStr = "delete from Disposition";
                SqlCeCommand cmdDel = new SqlCeCommand (delStr, ce);
                cmdDel.ExecuteNonQuery();

                string selectStr = "Select Disposition.ID,Disposition.DespositionName,Disposition.Description,Disposition.IsActive,Disposition.IsDeleted,Disposition.CreatedDate,Disposition.CreatedBy,Disposition.ModifiedDate,Disposition.ModifiedBy from Disposition,DispListDisp where DispListDisp.DispositionID=Disposition.ID and DispListDisp.DispositionListID in (select DespositionListID from CampaignDespoList where CampaignID='"+ CampaingID + "')";
                SqlDataAdapter adp = new SqlDataAdapter(selectStr, VMuktiAPI.VMuktiInfo.MainConnectionString);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string insertStr = "insert into Disposition (ID,DespositionName,Description,IsActive,IsDeleted,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) values ('" + ds.Tables[0].Rows[i][0] + "','" + ds.Tables[0].Rows[i][1] + "','" + ds.Tables[0].Rows[i][2] + "','" + ds.Tables[0].Rows[i][3] + "','" + ds.Tables[0].Rows[i][4] + "','" + ds.Tables[0].Rows[i][5] + "','" + ds.Tables[0].Rows[i][6] + "','" + ds.Tables[0].Rows[i][7] + "','" + ds.Tables[0].Rows[i][8] + "')";

                        SqlCeCommand cmdInsert = new SqlCeCommand(insertStr, ce);
                        cmdInsert.ExecuteNonQuery();
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception In Finding Row in table Entry");
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertDispositonTable()", "AutoDialer_ClsUserDataService.xaml.cs");

            }

        }

        public string GetZoneName(long LeadID)
        {
            try
            {
                string selectStr = "select timezone.timezonename from timezone,location,leads where location.id = leads.locationid and timezone.id = location.timezoneid and leads.id='" + LeadID + "'";
                SqlDataAdapter adp = new SqlDataAdapter(selectStr, VMuktiAPI.VMuktiInfo.MainConnectionString);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                string strZoneName = ds.Tables[0].Rows[0][0].ToString();
                return strZoneName;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
                return null;

            }
        }

        //public string GetDispositionName(long DispositionID)
        //{
        //    SqlCeConnection con = null;
        //    con = new SqlCeConnection(strClientConnectionString);
        //    con.Open();           

        //    try
        //    {  
        //        string str = "select DespositionName from disposition where id = '" + DispositionID + "'";
        //        SqlCeCommand cmd = new SqlCeCommand(str, ce);

        //        object i = cmd.ExecuteScalar();               
        //        return i.ToString();

        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        con.Close();
        //        VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
                
        //    }
        //}

        //public string GetPhoneNo(long LeadID)
        //{
        //    try
        //    { 
        //        string selectStr = "select PhoneNo from Leads where id='" + LeadID + "'";
        //        SqlCeDataAdapter adp = new SqlCeDataAdapter(selectStr, ce);
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);

        //        string strPhoneNo = ds.Tables[0].Rows[0][0].ToString();
        //        return strPhoneNo;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
        //    }
        //}

        public string fncGetDispositionName(long DispositionID)
        {
            try
            {  
                string str = "select DespositionName from disposition where id = '" + DispositionID + "'";
                SqlCeCommand cmd = new SqlCeCommand(str, ce);

                object i = cmd.ExecuteScalar();               
                return i.ToString();                
            }
            catch (Exception ex)
            {
                
                VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
                return null;

                
            }
            
        }

        public string fncGetPhoneNo(long LeadID)
        {
            try
            {
                string selectStr = "select PhoneNo from Leads where id='" + LeadID + "'";
                SqlCeDataAdapter adp = new SqlCeDataAdapter(selectStr, ce);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return (ds.Tables[0].Rows[0][0].ToString());                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
                return null;

            }
        }
    }
}
