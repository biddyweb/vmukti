using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using System.Data.SqlTypes;
using System.Windows.Forms;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data.SqlServerCe;
using System.Data.SqlServerCe;
using VMuktiAPI;

namespace PredictiveDialler.DataAccess
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
                myDS.Tables[0].Merge(myDS.Tables[1]);
                CampaingID = long.Parse(cmd.Parameters["@pCampaignID"].Value.ToString());
                return myDS;
            }
            catch (Exception ex)
            {

                if (myDS == null)
                {
                    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.DataAccess--:--ClsUserDataService.cs--:--GetLeadsList()--Error in getting Leads From StoreProcudure");
                }
                else if (CampaingID == Common.ClsConstants.NullLong)
                {
                    if (ex.Data.Contains("My Key"))
                    {
                        ex.Data.Add("My Key1", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.DataAccess--:--ClsUserDataService.cs--:--GetLeadsList()--CampaingID is not available");
                    }
                    else
                    {
                        ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.DataAccess--:--ClsUserDataService.cs--:--GetLeadsList()--CampaingID is not available");
                    }
                }
                else
                {
                    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.DataAccess--:--ClsUserDataService.cs--:--GetLeadsList()--");
                }
                ClsException.LogError(ex);
                //System.Collections.IEnumerator IEnum = ex.Data.GetEnumerator();
                //while(IEnum.MoveNext())
                //{
                //    System.Windows.Forms.MessageBox.Show(IEnum.Current.ToString());
                //}
                ClsException.WriteToErrorLogFile(ex);

                CampaingID = 0;
                return null;
            }
        }
        
        public void User_Save(out long parCallID, long leadID, DateTime calledDate, DateTime startDate, DateTime startTime, long durationInSecond, long despositionID, long campaingnID, long confID, string callNote, bool isDNC, bool isGlobal, long userID, string RecordedFileName)    
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
                VMuktiHelper.ExceptionHandler(ex, "User_Save()", "ClsUserDataService.cs");
            }
        }
        
        public void UpdateLeadStatus(long leadID, string Status)
        {
            try
            {
                //ExecuteDataSet("UPDATE Leads SET Status='Fresh' WHERE ID=" + leadID + "", CommandType.Text, null);
                fncUpdateLeadStatus(leadID, Status);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UpdateLeadStatus()", "ClsUserDataService.cs");
            }
        }
        public void UpdateDNCStatus(long leadID, long userID, bool isDNC)
        {
            try
            {
                // ExecuteDataSet("UPDATE Leads SET DNCBy=" + userID + ",DNCFlag=1 WHERE ID=" + leadID + "", CommandType.Text, null);
                fncUpdateDNCStatus(leadID, userID, isDNC);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_Save", "ClsUserDataService.cs");
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
        public void SetCallBackNo(long CallID, long leadID, string callBackNote, DateTime callBackDate, bool isPublic, bool isDeleted)
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
                VMuktiHelper.ExceptionHandler(ex, "SetCallBackNo()", "ClsUserDataService.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "User_Delete()", "ClsUserDataService.cs");
            }
        }

        public void SIPServerIP(out string SIPIP)
        {
            try
            {
            DataSet ds= ExecuteDataSet("select IPAddress from ServerInfo where ID=(select ServerID from ActiveServerInfo)", CommandType.Text, null);
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
                return null;
                VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
            }
        }

        public string fncGetDispositionName(long DispositionID)
        {
            SqlCeConnection ce = null;
            try
            {

                string str = "select DespositionName from disposition where id = '" + DispositionID + "'";
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
                SqlCeCommand cmd = new SqlCeCommand(str, ce);

                object i = cmd.ExecuteScalar();
                return i.ToString();
            }
            catch (Exception ex)
            {
                return null;
                VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");

            }
            finally
            {
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        public string fncGetPhoneNo(long LeadID)
        {
            SqlCeConnection ce = null;
            try
            {
                string selectStr = "select PhoneNo from Leads where id='" + LeadID + "'";
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
                SqlCeDataAdapter adp = new SqlCeDataAdapter(selectStr, ce);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return (ds.Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return null;
                VMuktiHelper.ExceptionHandler(ex, "GetZoneName()", "ClsUserDataService.cs");
            }
            finally
            {
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
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

        // SqlCeClientSyncProvider sync = null;
        SyncGroup myGroup = null;

        SqlSyncAdapterBuilder CallAdapter = null;
        SqlSyncAdapterBuilder CallBackAdapter = null;
        SqlSyncAdapterBuilder LeadAdapter = null;

        SyncAdapter CallAdapterSyncAdapter = null;
        SyncAdapter CallBackAdapterSyncAdapter = null;
        SyncAdapter LeadAdapterSyncAdapter = null;

      //  SqlCeConnection ce = null;

        public void CreateInitialLocalDB(string strConnectionString, bool isCreated)
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
                }
                else
                {
                    tblCallTable = new SyncTable("Call");
                    tblCallTable.SyncDirection = SyncDirection.UploadOnly;

                    tblLeadsTable = new SyncTable("Leads");
                    tblLeadsTable.SyncDirection = SyncDirection.UploadOnly;

                    tblCallBackTable = new SyncTable("CallBack");
                    tblCallBackTable.SyncDirection = SyncDirection.UploadOnly;
                }
                strClientConnectionString = strConnectionString;

                // sync = new SqlCeClientSyncProvider(strClientConnectionString);

                serverSyncProvider = new DbServerSyncProvider();

                syncAgent = new SyncAgent();
                //  syncAgent.ServerSyncProvider = serverSyncProvider;
                syncAgent.RemoteProvider = serverSyncProvider;

                serverConnection = new SqlConnection(VMuktiInfo.MainConnectionString);
                serverSyncProvider.Connection = serverConnection;
            
                //SqlCommand cmdAnchor = new SqlCommand();
                // cmdAnchor.CommandType = CommandType.Text;
                // cmdAnchor.CommandText = "SELECT @@DBTS";
                // serverSyncProvider.SelectNewAnchorCommand = cmdAnchor;

                // SqlCommand cmdClientId = new SqlCommand();
                // cmdClientId.CommandType = CommandType.Text;
                // cmdClientId.CommandText = "SELECT 1";
                // serverSyncProvider.SelectClientIdCommand = cmdClientId;                 



                //syncAgent.ClientSyncProvider = clientSyncProvider;
                syncAgent.LocalProvider = clientSyncProvider;
                myGroup = new SyncGroup("DialerGroup");
                tblCallTable.SyncGroup = myGroup;
                tblLeadsTable.SyncGroup = myGroup;
                tblCallBackTable.SyncGroup = myGroup;


                //syncAgent.SyncTables.Add(tblCallTable);
                //syncAgent.SyncTables.Add(tblLeadsTable);
                //syncAgent.SyncTables.Add(tblCallBackTable);

                syncAgent.Configuration.SyncTables.Add(tblCallTable);
                syncAgent.Configuration.SyncTables.Add(tblLeadsTable);
                syncAgent.Configuration.SyncTables.Add(tblCallBackTable);

                CallAdapter = new SqlSyncAdapterBuilder();
                CallAdapter.Connection = serverConnection;
                CallAdapter.SyncDirection = SyncDirection.UploadOnly;
                CallAdapter.TableName = "Call";
                CallAdapter.DataColumns.Add("ID");
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
                //LeadAdapterSyncAdapter.ColumnMappings.Add("Status", "Status");
                //LeadAdapterSyncAdapter.ColumnMappings.Add("DNCFlag", "DNCFlag");
                //LeadAdapterSyncAdapter.ColumnMappings.Add("DNCBy", "DNCBy");
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
                
                CheckPreviousSyncWithServer();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateInitialLocalDB()", "ClsUserDataService.cs");
                //MessageBox.Show("CreateInitialLocalDB: " + ex.Message);
            }

        }
        //public void OpenConnection(bool openConnection)
        //{
        //    try
        //    {
        //        if (openConnection)
        //        {
        //            if (ce != null && ce.State != ConnectionState.Open)
        //            {
        //                ce.Open();
        //            }
        //        }
        //        else
        //        {
        //            if (ce != null && ce.State == ConnectionState.Open)
        //            {
        //                ce.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error in Closing/Opening Connection");
        //    }
            
        //}
        public void CheckPreviousSyncWithServer()
        {
            SqlCeConnection ce = null;
            try
            {                
                if (IsTableExits("Leads"))
                {
                   // OpenConnection(true);
                   ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
                    SqlCeCommand cmd = new SqlCeCommand("Select Count(*) From Leads", ce);
                    object i = cmd.ExecuteScalar();
                   // OpenConnection(false);

                    if (int.Parse(i.ToString()) > 0)
                    {
                      //  OpenConnection(true);
                        cmd = new SqlCeCommand("Update Leads Set Status='Fresh' Where Status<>'Called' and Status<>'CallBack'", ce);
                        cmd.ExecuteNonQuery();
                      //  OpenConnection(false);
                        SynchronizeWithServer();

                        // Remove All Entries from lead table 
                      //  OpenConnection(true);
                        cmd = new SqlCeCommand("Delete From Leads", ce);
                        cmd.ExecuteNonQuery();
                      //  OpenConnection(false);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CheckPreviousSyncWithServer()", "ClsUserDataService.cs");
                //MessageBox.Show("CheckPreviousSyncWithServer: " + ex.Message);

            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }

        }

        public SyncTable CreateCallTable()
        {
            SqlCeConnection ce = null;
            try
            {
                //  if (!IsTableExits("Call"))
                {
                  //  OpenConnection(true);
                    
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();

                    tblCallTable = new SyncTable("Call");
                    tblCallTable.SyncDirection = SyncDirection.UploadOnly;


                    SyncSchema syncSchemaCall = new SyncSchema();
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


                    //sync.CreateSchema(tblCallTable, syncSchemaCall);
                    clientSyncProvider.CreateSchema(tblCallTable, syncSchemaCall);
                    return tblCallTable;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateCallTable()", "ClsUserDataService.cs");
                //System.Windows.Forms.MessageBox.Show("CreateCallTable: " + ex.Message);
                return null;
            }
            finally
            {
             //   OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        private SyncTable CreateLeadsTable()
        {
            SqlCeConnection ce = null;
            try
            {

                //  if (!IsTableExits("Leads"))
                {
                    //      SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);
                    //OpenConnection(true);
                    
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateLeadsTable()", "ClsUserDataService.cs");
                //MessageBox.Show("CreateLeadsTable: " + ex.Message);
                return null;
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        private SyncTable CreateCallBackTable()
        {
            SqlCeConnection ce = null;
            try
            {

                //   if (!IsTableExits("CallBack"))
                {
                    //        SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
                   // OpenConnection(true);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateCallBackTable()", "ClsUserDataService.cs");
                //MessageBox.Show("CreateCallBackTable: " + ex.Message);
                return null;
            }
            finally
            {
                //  OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        private bool IsTableExits(string strTableName)
        {
            SqlCeConnection ce = null;
            try
            {
                //  OpenConnection(true);
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsTableExists()", "ClsUserDataService.cs");
                //MessageBox.Show("IsTableExits: " + ex.Message);
                return false;
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
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
                //OpenConnection(true);

                SyncStatistics syncStatics = syncAgent.Synchronize();
                //OpenConnection(false);
                RemoveSyncData();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SynchronizeWithServer()", "ClsUserDataService.cs");
                //MessageBox.Show("SynchronizeWithServer: " + ex.Message);
            }
            //finally
            //{
            //    OpenConnection(false);
            //}
            //RemoveSyncData();
        }

        public void DeleteLeadTableData()
        {
            SqlCeConnection ce = null;
            try
            {
                // OpenConnection(true);

                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
                SqlCeCommand cmd = new SqlCeCommand("Delete From Leads", ce);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DeleteLeadTableData()", "ClsUserDataService.cs");
                //MessageBox.Show("DeleteLeadTableData: " + ex.Message);
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        public void RemoveSyncData()
        {
            SqlCeConnection ce = null;
            try
            {
                // OpenConnection(true);
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveSyncData()", "ClsUserDataService.cs");
                //MessageBox.Show("RemoveSyncData: " + ex.Message);
            }
            finally
            {
                //  OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        public void fncInsertIntoCallTable(out long parCallID, long leadID, DateTime calledDate, DateTime startDate, DateTime startTime, long durationInSecond, long despositionID, long campaingnID, long confID, string callNote, bool isDNC, bool isGlobal, long userID, string RecordedFileName)
        {
            //SqlCeConnection ce = null;
            //try
            //{
            //    if (!IsRecordExists(leadID.ToString(), "Call", "LeadID"))
            //    {
            //        //  OpenConnection(true);
            //        ce = new SqlCeConnection(strClientConnectionString);
            //        ce.Open();

            //        string strInsertStatement = "INSERT INTO Call (LeadID,CalledDate,ModifiedDate,ModifiedBy,"
            //    + "GeneratedBy,StartDate,StartTime,DurationInSecond,DespositionID,"
            //    + "CampaignID,ConfID,IsDeleted,CallNote,IsDNC,IsGlobal)" +
            //    "VALUES(" + leadID + ",'" + calledDate + "','" + calledDate + "'," + userID + ","
            //    + userID + ",'" + startDate + "','" + startTime + "'," + durationInSecond + "," + despositionID + ","
            //    + campaingnID + "," + confID + ",0,'" + callNote + "','" + isDNC + "','" + isGlobal + "')";

            //        SqlCeCommand cmd = new SqlCeCommand(strInsertStatement, ce);
            //        cmd.ExecuteNonQuery();

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertIntoCallTable()", "ClsUserDataService.cs");
                //MessageBox.Show("fncInsertIntoCallTable: " + ex.Message);
            }
        //    finally
        //    {
        //        //OpenConnection(false);
        //        if (ce != null && ce.State == ConnectionState.Open)
        //        {
        //            ce.Close();
        //        }
        //    }
        }

        public void fncUpdateLeadStatus(long leadID, string status)
        {
            SqlCeConnection ce = null;

            try
            {
                if (IsRecordExists(leadID.ToString(), "Leads", "ID"))
                {
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
                    //OpenConnection(true);
                    SqlCeCommand cmd = new SqlCeCommand("Update Leads Set Status='" + status + "' where ID=" + leadID, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncUpdateLeadStatus()", "ClsUserDataService.cs");
                //MessageBox.Show("fncUpdateLeadStatus: " + ex.Message);
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        public void fncDeleteFromLead(long leadID)
        {
            //   SqlCeConnection ceConn = null;
            SqlCeConnection ce = null;
            try
            {

                //  ceConn = new SqlCeConnection(strClientConnectionString);
                //   ceConn.Open();
                if (IsRecordExists(leadID.ToString(), "Leads", "ID"))
                {
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
                    //OpenConnection(true);
                    SqlCeCommand cmd = new SqlCeCommand("Delete From Leads Where ID=" + leadID, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncDeleteFromLead()", "ClsUserDataService.cs");
                //MessageBox.Show("fncDeleteFromLead: " + ex.Message);
            }
            finally
            {
                // OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        public void CloseConnection()
        {
            try
            {
                RemoveSyncData();
                //if (ce != null && ce.State == ConnectionState.Open)
                //{
                //    ce.Close();
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CloseConnection()", "ClsUserDataService.cs");
                //MessageBox.Show(ex.Message);
            }
        }

        public void fncInsertIntoCallBackTable(long callID, string callBackNote, DateTime callBackDate, bool isPublic, bool isDeleted)
        {
            SqlCeConnection ce = null;

            try
            {


                if (!IsRecordExists(callID.ToString(), "CallBack", "CallID"))
                {
                    //    OpenConnection(true);
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
                    string strInsertStatement = "INSERT INTO CallBack(CallID, callBackDate, Comment, IsPublic, IsDeleted )" +
                    "VALUES(" + callID + ",'" + callBackDate + "','" + callBackNote + "','" + isPublic + "' , 0 )";

                    SqlCeCommand cmd = new SqlCeCommand(strInsertStatement, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertIntoCallBackTable()", "ClsUserDataService.cs");
                //MessageBox.Show("Error In Inserting Value of CallBack Table.");
            }
            finally
            {
                //   OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        public void fncUpdateDNCStatus(long leadID, long userID, bool isDNC)
        {
            SqlCeConnection ce = null;
            try
            {
                
                
                if (IsRecordExists(leadID.ToString(), "Leads", "ID"))
                {
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open();
                    //OpenConnection(true);
                    string strUpdateStatement = "UPDATE Leads SET DNCBy=" + userID + ",DNCFlag=1 WHERE ID=" + leadID + "";

                    SqlCeCommand cmd = new SqlCeCommand(strUpdateStatement, ce);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncUpdateDNCStatus()", "ClsUserDataService.cs");
                //MessageBox.Show("Error In Updating Value of DNC in Leads Table.");
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        private bool IsRecordExists(string strValue, string strTableName, string strKey)
        {
               SqlCeConnection ce = null;
            try
            {
               // OpenConnection(true);
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsRecordExists()", "ClsUserDataService.cs");
                //MessageBox.Show("Exception In Finding Row in table Entry");
                return false;
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }
        }

        private long GetCallID(long leadID)
        {
              SqlCeConnection ce = null;
            try
            {
               // OpenConnection(true);
                ce = new SqlCeConnection(strClientConnectionString);
                ce.Open();
                string str = "select ID from Call where LeadID=" + leadID + "";
                SqlCeCommand cmd = new SqlCeCommand(str, ce);

                object i = cmd.ExecuteScalar();
                return long.Parse(i.ToString());

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCallID()", "ClsUserDataService.cs");
                //MessageBox.Show("Exception In Finding Row in table Entry");
                return 0;
            }
            finally
            {
                //OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }

        }

        public void fncInsertIntoLeadTable(long ID, long PhoneNo, int LeadFormatID, DateTime CreatedDate, int CreatedBy,
            bool IsDeleted, DateTime ModifiedDate, int ModifiedBy, bool DNCFlag, int DNCBy, long ListID, long LocationID, int RecycleCount, string Status, bool IsGlobalDNC)
        {
              SqlCeConnection ce = null;
            try
            {

               
                //string sInsertStatement = "INSERT INTO Leads"
                //      +"(ID, PhoneNo, LeadFormatID, CreatedDate, CreatedBy, DeletedDate, DeletedBy, IsDeleted, ModifiedDate, ModifiedBy, DNCFlag, DNCBy, ListID," 
                //      +"LocationID, RecycleCount, Status, IsGlobalDNC, LastEditDate, CreationDate)"
                //      +"VALUES("+ID+ ","+PhoneNo+","+LeadFormatID+",'"+CreatedDate+"',"+CreatedBy+",'"+ DeletedDate+"',"+DeletedBy+",'"+IsDeleted+"','"+ModifiedDate+"',"+ModifiedBy+ ",'"+DNCFlag+"',"
                //      + DNCBy + "," + ListID + "," + LocationID + ","+RecycleCount+ ", '"+Status+"','"+IsGlobalDNC+"', '"+LastEditDate+"', '"+CreationDate+"')";
                if (!IsRecordExists(ID.ToString(), "Leads", "ID"))
                {
                  
                 //   OpenConnection(true);
                    ce = new SqlCeConnection(strClientConnectionString);
                    ce.Open(); 
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncInsertIntoLeadTable()", "ClsUserDataService.cs");
                //MessageBox.Show("fncInsertIntoLeadTable:" + ex.Message);
            }
            finally
            {
               // OpenConnection(false);
                if (ce != null && ce.State == ConnectionState.Open)
                {
                    ce.Close();
                }
            }

        }

        #endregion

    }
}
