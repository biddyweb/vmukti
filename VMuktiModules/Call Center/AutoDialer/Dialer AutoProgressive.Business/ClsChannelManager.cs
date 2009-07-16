/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using Dialer_AutoProgressive.Common;
using Dialer_AutoProgressive.DataAccess;
//using VMuktiAPI;
using System.IO;
using VMuktiAPI;

namespace Dialer_AutoProgressive.Business
{
    public class ClsChannelManager
    {
        #region Fields
        ClsChannel channel1 = null;
        long CurrentUserID = Dialer_AutoProgressive.Common.ClsConstants.NullLong;
        long CampaingID = Dialer_AutoProgressive.Common.ClsConstants.NullLong;
        string CurrentDialStatus = Dialer_AutoProgressive.Common.ClsConstants.NullString;
        string StrAgentNumber = Dialer_AutoProgressive.Common.ClsConstants.NullString;
        string StrAgentPassword = Dialer_AutoProgressive.Common.ClsConstants.NullString;
        string SIPServer = Dialer_AutoProgressive.Common.ClsConstants.NullString;
        public ClsLeadCollection objLeadCollection = null;
        bool blExit = false;

        public enum CallStatus
        {
            NotInCall, CallInProgress, DialingToDest, RingingToDest,
            BusyTone, AMD, DTMF, DestUserStratListen, CallHangUp, CallHoldByUser,
            CallHoldBySys, CallDispose, ReadyState
        };
        #endregion

        #region Property
        public string AgentNumber
        {
            get { return StrAgentNumber; }
            set { StrAgentNumber = value; }
        }
        public string AgentPassWord
        {
            get { return StrAgentPassword; }
            set { StrAgentPassword = value; }
        }
        public string SIPServerAddress
        {
            get { return SIPServer; }
            set { SIPServer = value; }
        }
        public long CurrentCampaingID
        {
            get { return CampaingID; }
            set { CampaingID = value; }
        }

        public ClsChannel ActiveChannel
        {
            get { return channel1; }
            set { channel1 = value; }
        }
        public ClsLeadCollection LeadCollection
        {
            get { return objLeadCollection; }
            set { objLeadCollection = value; }
        }
        public long UserID
        {
            get { return CurrentUserID; }
            set { CurrentUserID = value; }


        }
#endregion

        long userid = Dialer_AutoProgressive.Common.ClsConstants.NullLong;
        public long chManagerLocalCampId = Dialer_AutoProgressive.Common.ClsConstants.NullLong;
        DataAccess.ClsUserDataService clsDataService = null;
        System.Timers.Timer timer4Sync = null;
        public ClsChannelManager()
        {
            try
            {
                clsDataService = new DataAccess.ClsUserDataService();
                // Initialize Local DB........
                CheckLocalDB();
                timer4Sync = new System.Timers.Timer(100000);
                timer4Sync.Elapsed += new System.Timers.ElapsedEventHandler(timer4Sync_Elapsed);

                // End DB


               // VMuktiAPI.VMuktiInfo.CurrentPeer.ID = 2;
                userid = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                //VMuktiAPI.VMuktiInfo.MainConnectionString = "Data Source=210.211.254.132\\SqlExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir;";

                ///
                CurrentUserID = userid;
                channel1 = new ClsChannel();
                //objLeadCollection = ClsLeadCollection.GetAll(userid, out CampaingID,clsDataService);
                //if (objLeadCollection != null && objLeadCollection.Count > 0)
                //{
                //    InsertLeadsIntoTable();
                //    clsDataService.SynchronizeWithServer();
                //}
                GetNextLeadList(true);

                //if (CampaingID == 0 )
                //{
                //    System.Windows.Forms.MessageBox.Show("No Campaing available");
                //}
                //else if (objLeadCollection == null)
                //{
                //    System.Windows.Forms.MessageBox.Show("No Fresh Leads available");
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClsChannelManager()", "ClsChannelManager.cs");
            }
        }

        #region Local Sync Functions

        public void CheckLocalDB()
        {
            try
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                string strClientConnectionString = @"Data Source=" + System.IO.Path.GetDirectoryName(asm.Location) + "\\VMukti.sdf";
                //strServerConnectionString = @"Data Source=192.168.1.186\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir";
                string strFilePath = System.IO.Path.GetDirectoryName(asm.Location) + "\\VMukti.sdf";
                clsDataService.CreateInitialLocalDB(strClientConnectionString, File.Exists(strFilePath));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckLocalDB()", "ClsChannelManager.cs");
            }
            
        }

        void timer4Sync_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                clsDataService.SynchronizeWithServer();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "timer4Sync_Elapsed()", "ClsChannelManager.cs");
            }
        }

        public void StartSyncProcess(bool status)
        {
            try
            {
                if (status)
                {
                    if (!timer4Sync.Enabled)
                    {
                        timer4Sync.Enabled = true;
                        timer4Sync.Start();
                    }
                }
                else
                {
                    timer4Sync.Stop();
                    timer4Sync_Elapsed(null, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "StartSyncProcess()", "ClsChannelManager.cs");
            }
        }

        private void InsertLeadsIntoTable()
        {
            try
            {
                foreach (ClsLead cl in objLeadCollection)
                {
                    //clsDataService.fncInsertIntoLeadTable(cl.ID, cl.PhoneNo, cl.LeadFormatID, cl.CreatedDate,
                    //    cl.CreatedBy, cl.IsDeleted, cl.ModifiedDate, cl.ModifiedBy,
                    //    cl.DNCFlag, cl.DNCBy, cl.ListID, cl.LocationID, cl.RecycleCount, cl.Status, false, DateTime.Now, DateTime.Now);

                    //clsDataService.fncInsertIntoLeadTable(cl.ID, cl.PhoneNo, cl.LeadFormatID, cl.CreatedDate,
                    //    cl.CreatedBy, cl.IsDeleted, cl.ModifiedDate, cl.ModifiedBy,
                    //    cl.DNCFlag, cl.DNCBy, cl.ListID, cl.LocationID, cl.RecycleCount, cl.Status, false);

                    clsDataService.fncInsertIntoLeadTable(cl.ID, cl.PhoneNo, cl.LeadFormatID, cl.CreatedDate,
                       cl.CreatedBy, cl.IsDeleted, cl.ModifiedDate, cl.ModifiedBy,
                       cl.DNCFlag, cl.DNCBy, cl.ListID, cl.LocationID, cl.RecycleCount,cl.Status,false);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "InsertLeadsIntoTable()", "ClsChannelManager.cs");
            }
        }
        
        #endregion

        public long GetCountryCode()
        {
            try
            {
                return clsDataService.FncGetCountryCode(CampaingID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetCountryCode()", "ClsChannelManager.cs");
                return 0;
            }
        }

        public string GetCampaginPrefix()
        {
            try
            {
                return clsDataService.FncGetCampaginPrefix(CampaingID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCampaginPrefix", "ClsChannelManagaer");
                return null;
            }
        }
        public bool RegisterSIPUser()
        {
            try
            {
                clsDataService.SIPServerIP(out SIPServer);
                clsDataService.AgentSIPInformation(out StrAgentNumber, out StrAgentPassword);
                if (StrAgentNumber == "")
                {
                    System.Windows.Forms.MessageBox.Show("No SIP User Available.");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RegisterSIPUser()", "ClsChannelManager.cs");
                return false;
            }

        }

        public void CallExit()
        {
            try
            {
                if (!blExit)
                {
                    blExit = true;
                    if (StrAgentNumber != "")
                    {
                        clsDataService.DeallocateSIPInformation(long.Parse(StrAgentNumber));
                    }

                    try
                    {
                        if (channel1.ChannelID != "")
                        {
                            VMuktiHelper.CallEvent("HangUp", this, new VMuktiEventArgs(channel1.ChannelID));
                        }
                    }
                    catch
                    {

                    }
                    foreach (ClsLead cd in objLeadCollection)
                    {
                        clsDataService.UpdateLeadStatus(cd.ID,cd.Status);
                       
                    }
                    UpdateSyncAtExit();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CallExit()", "ClsChannelManager.cs");
            }
				   finally
            {
                if (!blExit)
                {
                    UpdateSyncAtExit();
                }
            }
        }

        public void UpdateSyncAtExit()
        {
            timer4Sync.Stop();
            timer4Sync_Elapsed(null, null);
            clsDataService.CloseConnection();
        }

        public void SetChannelValues(VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                channel1.CurrentPhoneNo = long.Parse(e._args[0].ToString());
                channel1.StartDate = DateTime.Parse(e._args[1].ToString());
                channel1.StartTime = e._args[2].ToString();
                channel1.CurrentCampainID = CampaingID;
                //   channel1.LeadID = long.Parse("1");
                channel1.ChannelID = "1";
                channel1.UserID = CurrentUserID;
                channel1.ConfID = long.Parse("1");
                channel1.IsDNC = false;
                channel1.IsGlobal = false;
                //  channel1.DispositionID = long.Parse("1");
                //   channel1.CallNote = "Hello";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetChannelValues()", "ClsChannelManager.cs");
            }
        }

        public void SetDisposition(VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                if (ActiveChannel.CurrentDialStatus == "Automatic")
                {
                    ActiveChannel.DispositionID = long.Parse(e._args[0].ToString());
                    ActiveChannel.CallNote = e._args[1].ToString();

                    long parCallID = 0;

                    if (ActiveChannel.DispositionID == 6)
                    {
                        ActiveChannel.IsDNC = false;
                        clsDataService.User_Save(out parCallID, ActiveChannel.LeadID, ActiveChannel.CalledDate, ActiveChannel.StartDate, DateTime.Parse(ActiveChannel.StartTime), long.Parse(ActiveChannel.CallDuration.ToString()), ActiveChannel.DispositionID, ActiveChannel.CurrentCampainID, ActiveChannel.ConfID, ActiveChannel.CallNote, ActiveChannel.IsDNC, ActiveChannel.IsGlobal, ActiveChannel.UserID, ActiveChannel.RecordedFileName);
                        clsDataService.SetCallBackNo(parCallID, ActiveChannel.LeadID, ActiveChannel.CallNote, DateTime.Parse(e._args[3].ToString()), bool.Parse(e._args[2].ToString()), false);

                    }
                    else if (ActiveChannel.DispositionID == 11)
                    {
                        ActiveChannel.IsDNC = true;
                        clsDataService.User_Save(out parCallID, ActiveChannel.LeadID, ActiveChannel.CalledDate, ActiveChannel.StartDate, DateTime.Parse(ActiveChannel.StartTime), long.Parse(ActiveChannel.CallDuration.ToString()), ActiveChannel.DispositionID, ActiveChannel.CurrentCampainID, ActiveChannel.ConfID, ActiveChannel.CallNote, ActiveChannel.IsDNC, ActiveChannel.IsGlobal, ActiveChannel.UserID,ActiveChannel.RecordedFileName);
                        clsDataService.UpdateDNCStatus(ActiveChannel.LeadID, ActiveChannel.UserID, true);

                    }
                    else
                    {
                        clsDataService.User_Save(out parCallID, ActiveChannel.LeadID, ActiveChannel.CalledDate, ActiveChannel.StartDate, DateTime.Parse(ActiveChannel.StartTime), long.Parse(ActiveChannel.CallDuration.ToString()), ActiveChannel.DispositionID, ActiveChannel.CurrentCampainID, ActiveChannel.ConfID, ActiveChannel.CallNote, ActiveChannel.IsDNC, ActiveChannel.IsGlobal, ActiveChannel.UserID, ActiveChannel.RecordedFileName);
                    }
                    ActiveChannel.CallResult = ClsChannel.CallStatus.NotInCall;
                    ActiveChannel.StartTime = Dialer_AutoProgressive.Common.ClsConstants.NullString;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetDisposition()", "ClsChannelManager.cs");
            }
        }

        public void SetCallResult(object strCallResult)
        {
            try
            {
                ActiveChannel.CallResult = (ClsChannel.CallStatus)strCallResult;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetCallResult()", "ClsChannelManager.cs");
            }
        }

        public void GetNextLeadList(bool isFirstTime)
        {
            try
            {
                //if (!isFirstTime)
                //{
                //    timer4Sync_Elapsed(null, null);
                //}
                
                objLeadCollection = ClsLeadCollection.GetAll(userid, out CampaingID,clsDataService);
                if (objLeadCollection != null && objLeadCollection.Count > 0)
                {
                    InsertLeadsIntoTable();
                    if (chManagerLocalCampId != CampaingID)
                    {
                        chManagerLocalCampId = CampaingID;
                        clsDataService.fncInsertDispositionTable(CampaingID);

                        
                        //Delete and Fill up Disposition Table and 
                    }

                }
                timer4Sync_Elapsed(null, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetNextLeadList()", "ClsChannelManager.cs");
            }
        }







        public string GetDispositionName(long DispositionID)
        {
            try
            {
               return (clsDataService.fncGetDispositionName(DispositionID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDispositionName()", "AutoDialer-ClsChanelManager.cs");
                return null;

            }
        }

        

        //public static string GetPhoneNo(long LeadID)
        //{
        //    try
        //    {
        //        return (new Dialer_AutoProgressive.DataAccess.ClsUserDataService().GetPhoneNo(LeadID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public string GetPhoneNo(long LeadID)
        {
            try
            {
                return (clsDataService.fncGetPhoneNo(LeadID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetPhoneNo()", "AutoDialer-ClsChanelManager.cs");
                return null;
            }
        }
    }
}
