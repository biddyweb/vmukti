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
using System.IO;
using System.Windows.Forms;
using VMuktiAPI;

namespace PredictiveDialler.Business
{
    public class ClsChannelManager
    {
        #region Fields
        private List<ClsChannel> _DiallerChannel = new List<ClsChannel>();

        long CurrentUserID = PredictiveDialler.Common.ClsConstants.NullLong;
        long CampaingID = PredictiveDialler.Common.ClsConstants.NullLong;
        string CurrentDialStatus = PredictiveDialler.Common.ClsConstants.NullString;
        string StrAgentNumber = PredictiveDialler.Common.ClsConstants.NullString;
        string StrAgentPassword = PredictiveDialler.Common.ClsConstants.NullString;
        string SIPServer = PredictiveDialler.Common.ClsConstants.NullString;
        public ClsLeadCollection objLeadCollection = null;
        public ClsChannel ActiveChannel = null;
        public long chManagerCountryId = 0;
        public string chManagerCamPrefix = "";
      
       // public PredictiveDialler.DataAccess.ClsUserDataService DataAccessService = null;
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

        //public ClsChannel ActiveChannel
        //{
        //  //  get { return channel1; }
        //    set { channel1 = value; }
        //}

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

        long userid = PredictiveDialler.Common.ClsConstants.NullLong;

        DataAccess.ClsUserDataService clsDataService = null;
        System.Timers.Timer timer4Sync = null;

        public ClsChannelManager()
        {
            for (int i = 0; i < 2; i++)
            {
                _DiallerChannel.Add(new ClsChannel());
                _DiallerChannel[_DiallerChannel.Count - 1].ChannelID = _DiallerChannel.Count.ToString();
            }
            clsDataService = new DataAccess.ClsUserDataService();
            CheckLocalDB();
            timer4Sync = new System.Timers.Timer(10000);
            timer4Sync.Elapsed += new System.Timers.ElapsedEventHandler(timer4Sync_Elapsed);

            userid = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
            CurrentUserID = userid;
            GetNextLeadList(true);
        }

        public void chManagerSetAgentNumber(string strAgentNumber, string strAgentPass, string strSIPServerIP)
        {
            SIPServer = strSIPServerIP;
            StrAgentNumber = strAgentNumber;
            StrAgentPassword = strAgentPass;
           
        }

        #region Local Sync Functions

        public void CheckLocalDB()
        {
           
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string strClientConnectionString = @"Data Source=" + System.IO.Path.GetDirectoryName(asm.Location) + "\\VMukti.sdf";
            string strFilePath = System.IO.Path.GetDirectoryName(asm.Location) + "\\VMukti.sdf";
           
            clsDataService.CreateInitialLocalDB(strClientConnectionString, File.Exists(strFilePath));
           
        }

        void timer4Sync_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            clsDataService.SynchronizeWithServer();
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveSyncData()", "ClsChannelManager.cs");
            }
        }

        public void DeleteLeadTabelData()
        {           
            timer4Sync.Stop();
            foreach (ClsLead cd in objLeadCollection)
            {
                clsDataService.UpdateLeadStatus(cd.ID, cd.Status);
            }
            timer4Sync_Elapsed(null, null);
            clsDataService.DeleteLeadTableData();
            LeadCollection.Clear();
        }
       
        private void InsertTransfteredLead(long leadID, long phoneNO, long campaignID)
        {
            clsDataService.fncInsertIntoLeadTable(leadID, phoneNO, LeadCollection[0].LeadFormatID, DateTime.Now,
                LeadCollection[0].CreatedBy, false, DateTime.Now, LeadCollection[0].ModifiedBy,
                false, LeadCollection[0].DNCBy, LeadCollection[0].ListID, LeadCollection[0].LocationID,
                0, "Fresh", false);
        }

        public void DeleteTransfteredLead(long leadID)
        {
            clsDataService.fncDeleteFromLead(leadID);
        }
        
        public void GetNextLeadList(bool isFirstTime)
        {
            try
            {
                //if (!isFirstTime)
            //{
                //    timer4Sync_Elapsed(null, null);
            //}
                if (objLeadCollection == null || objLeadCollection.Count == 0)
                {
                    objLeadCollection = ClsLeadCollection.GetAll(userid, out CampaingID, clsDataService);
                    InsertLeadsIntoTable();
                }                
                timer4Sync_Elapsed(null, null);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.Business--:--ClsChannelManager.cs--:--GetNextLeadList()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        private void InsertLeadsIntoTable()
        {
            foreach (ClsLead cl in objLeadCollection)
            {
                clsDataService.fncInsertIntoLeadTable(cl.ID, cl.PhoneNo, cl.LeadFormatID, cl.CreatedDate,
                    cl.CreatedBy, cl.IsDeleted, cl.ModifiedDate, cl.ModifiedBy,
                    cl.DNCFlag, cl.DNCBy, cl.ListID, cl.LocationID, cl.RecycleCount, cl.Status, false);

            }
        }
        #endregion
        
        public long GetCountryCode()
        {
            try
            {
                chManagerCountryId = clsDataService.FncGetCountryCode(CampaingID);
                return chManagerCountryId;
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
                chManagerCamPrefix = clsDataService.FncGetCampaginPrefix(CampaingID);
                return chManagerCamPrefix;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCampaginPrefix", "ClsChannelManagaer");
                return null;
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
                return null;

            }
        }

        public string GetPhoneNo(long LeadID)
        {
            try
            {
                return (clsDataService.fncGetPhoneNo(LeadID));
            }
            catch (Exception ex)
            {
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
                        foreach (ClsChannel cl in _DiallerChannel)
                        {
                            if (cl.CallResult == ClsChannel.CallStatus.CallInProgress)
                            {
                                VMuktiHelper.CallEvent("HangUp", this, new VMuktiEventArgs("PredictiveDialer", cl.ChannelID));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.Business--:--ClsChannelManager.cs--:--HangUp()--");
                        ClsException.LogError(ex);
                        ClsException.WriteToErrorLogFile(ex);
                    }
                    //if (objLeadCollection != null)
                    //{
                    foreach (ClsLead cd in objLeadCollection)
                    {
                        clsDataService.UpdateLeadStatus(cd.ID, cd.Status);
                        //  clsDataService.UpdateLeadStatus(cd.ID, "Fresh");
                    }
                    UpdateSyncAtExit();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--Dialer.Business--:--ClsChannelManager.cs--:--CallExit()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
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

        //public void SetChannelValues(VMuktiEventArgs e)
        //{
        //    int ChannelId = (Convert.ToInt32(e._args[e._args.Count - 1].ToString()) - 1);
        //    _DiallerChannel[ChannelId].CurrentPhoneNo = long.Parse(e._args[0].ToString());

        //    channel1.CurrentPhoneNo = long.Parse(e._args[0].ToString());
        //    channel1.StartDate = DateTime.Parse(e._args[1].ToString());
        //    channel1.StartTime = e._args[2].ToString();
        //    channel1.CurrentCampainID = CampaingID;
        // //   channel1.LeadID = long.Parse("1");
        //    channel1.ChannelID = "1";
        //    channel1.UserID = CurrentUserID;
        //    channel1.ConfID = long.Parse("1");
        //    channel1.IsDNC = false;
        //    channel1.IsGlobal = false;
        //    //  channel1.DispositionID = long.Parse("1");
        // //   channel1.CallNote = "Hello";
        //}

        public void SetDisposition(VMuktiEventArgs e)
        {
            //if (ActiveChannel.CurrentDialStatus == "Automatic")
            //{
            int channelNo = int.Parse(e._args[4].ToString());

            ActiveChannel = _DiallerChannel[channelNo];

            ActiveChannel.DispositionID = long.Parse(e._args[0].ToString());
            ActiveChannel.CallNote = e._args[1].ToString();
            int tmpID = -1;
            long parCallID = 0;

            if (ActiveChannel.DispositionID == 6)
            {
                ActiveChannel.IsDNC = false;
                clsDataService.User_Save(out parCallID, ActiveChannel.LeadID, ActiveChannel.CalledDate, ActiveChannel.StartDate, DateTime.Parse(ActiveChannel.StartTime), long.Parse(ActiveChannel.CallDuration.ToString()), ActiveChannel.DispositionID, ActiveChannel.CurrentCampainID, ActiveChannel.ConfID, ActiveChannel.CallNote, ActiveChannel.IsDNC, ActiveChannel.IsGlobal, ActiveChannel.UserID, "");
                clsDataService.SetCallBackNo(parCallID, ActiveChannel.LeadID, ActiveChannel.CallNote, DateTime.Parse(e._args[3].ToString()), bool.Parse(e._args[2].ToString()), false);
            }
            else if (ActiveChannel.DispositionID == 11)
            {
                ActiveChannel.IsDNC = true;
                clsDataService.User_Save(out parCallID, ActiveChannel.LeadID, ActiveChannel.CalledDate, ActiveChannel.StartDate, DateTime.Parse(ActiveChannel.StartTime), long.Parse(ActiveChannel.CallDuration.ToString()), ActiveChannel.DispositionID, ActiveChannel.CurrentCampainID, ActiveChannel.ConfID, ActiveChannel.CallNote, ActiveChannel.IsDNC, ActiveChannel.IsGlobal, ActiveChannel.UserID, "");
                clsDataService.UpdateDNCStatus(ActiveChannel.LeadID, ActiveChannel.UserID, true);
            }
            else
            {
                clsDataService.User_Save(out parCallID, ActiveChannel.LeadID, ActiveChannel.CalledDate, ActiveChannel.StartDate, DateTime.Parse(ActiveChannel.StartTime), long.Parse(ActiveChannel.CallDuration.ToString()), ActiveChannel.DispositionID, ActiveChannel.CurrentCampainID, ActiveChannel.ConfID, ActiveChannel.CallNote, ActiveChannel.IsDNC, ActiveChannel.IsGlobal, ActiveChannel.UserID, "");
            }
            ActiveChannel.CallResult = ClsChannel.CallStatus.NotInCall;
            ActiveChannel.StartTime = PredictiveDialler.Common.ClsConstants.NullString;
        }

        public bool SetCallResult(object strCallResult, int channelID)
        {
            bool isAnotherCallRunning = false;
            
            foreach (ClsChannel cl in _DiallerChannel)
            {
                if (cl.CallResult == ClsChannel.CallStatus.CallInProgress)
                {
                    // Transfter Call to Another Agent Who is Free
                    isAnotherCallRunning = true;
                    break;
                }
            }
            _DiallerChannel[channelID - 1].CallResult = (ClsChannel.CallStatus)strCallResult;
            return isAnotherCallRunning;
        }

        //public void GetNextLeadList()
        //{
        //    if (objLeadCollection == null || objLeadCollection.Count == 0)
        //    {
        //        objLeadCollection = ClsLeadCollection.GetAll(userid, out CampaingID,clsDataService);
        //    }
        //}

        public void FireCall()
        {
            lock (this)
            {
                if (LeadCollection.Count == 0)
                {
                    
                }
                else if (LeadCollection.Count >= 2)
                {
                    //_DiallerChannel[0].CallResult == ClsChannel.CallStatus.ReadyState ||
                    if (_DiallerChannel[0].CallResult == ClsChannel.CallStatus.NotInCall || _DiallerChannel[0].CallResult == ClsChannel.CallStatus.CallHangUp || _DiallerChannel[0].CallResult == ClsChannel.CallStatus.CallDispose)
                    {
                    
                        System.Threading.ThreadStart Call = delegate { FireCallinNewThread(0); };
                        new System.Threading.Thread(Call).Start();
                    }

                    if (_DiallerChannel[1].CallResult == ClsChannel.CallStatus.NotInCall || _DiallerChannel[1].CallResult == ClsChannel.CallStatus.CallHangUp || _DiallerChannel[1].CallResult == ClsChannel.CallStatus.CallDispose)
                    {
                       
                        System.Threading.ThreadStart Call1 = delegate { FireCallinNewThread(1); };
                        new System.Threading.Thread(Call1).Start();
                    }
                    //VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.ReadyState));
                }
                else if (LeadCollection.Count == 1)
                {
                   
                    System.Threading.ThreadStart Call = delegate { FireCallinNewThread(0); };
                    new System.Threading.Thread(Call).Start();
                }
            }
        }

        void FireCallinNewThread(int i)
        {
            lock (this)
            {
                if (_DiallerChannel[i].CallResult == ClsChannel.CallStatus.ReadyState || _DiallerChannel[i].CallResult == ClsChannel.CallStatus.NotInCall || _DiallerChannel[i].CallResult == ClsChannel.CallStatus.CallHangUp || _DiallerChannel[i].CallResult == ClsChannel.CallStatus.CallDispose)
                {
                    
                    int counter=0;
                    int temp = 0;
                    foreach (ClsLead cls in LeadCollection)
                    {
                        if (cls.IsAssign == false)
                        {
                            if (temp == 0)
                            {
                                counter = i;
                            }
                            else
                            {
                                counter = temp;
                            }
                            break;
                        }
                        else
                        {
                            temp += 1;
                        }
                    }
                    _DiallerChannel[i].CurrentDialStatus = "Predictive";
                    _DiallerChannel[i].CurrentPhoneNo = LeadCollection[counter].PhoneNo;
                    _DiallerChannel[i].CurrentCampainID = CurrentCampaingID;
                    _DiallerChannel[i].LeadID = LeadCollection[counter].ID;
                    LeadCollection[counter].IsAssign = true;
                    _DiallerChannel[i].ChannelID = (i + 1).ToString();

                    _DiallerChannel[i].UserID = UserID;
                    _DiallerChannel[i].ConfID = long.Parse("1");
                    _DiallerChannel[i].IsDNC = false;
                    _DiallerChannel[i].IsGlobal = false;
                    _DiallerChannel[i].CampaginPrefix = chManagerCamPrefix;
                    _DiallerChannel[i].CountryCode = chManagerCountryId;
                    // _DiallerChannel[i].CallNote = "Test";
                    _DiallerChannel[i].CallResult = ClsChannel.CallStatus.ReadyState;
                    // Imme. remove leads after dialing.
                    //LeadCollection.RemoveAt(i);
                    //if (LeadCollection.Count == 0)
                    //{
                    //    GetNextLeadList();
                    //}
                }
            }
        }

        public void FncRemoveDialLead(int channelID)
        {
            lock (this)
            {
                if (channelID == 0)
                {
                    foreach (ClsLead cl in LeadCollection)
                    {
                        for (int i = 0; i < _DiallerChannel.Count; i++)
                        {
                            if (long.Parse(cl.ID.ToString()) == _DiallerChannel[i].LeadID)
                            {
                                LeadCollection.Remove(cl);
                            }
                        }
                    }
                }
                else
                {
                    foreach (ClsLead cl in LeadCollection)
                    {
                        if (long.Parse(cl.ID.ToString()) == _DiallerChannel[channelID - 1].LeadID)
                        {
                            LeadCollection.Remove(cl);
                            _DiallerChannel[channelID - 1].CallResult = ClsChannel.CallStatus.NotInCall;
                            break;
                        }
                    }
                }
                if (LeadCollection.Count == 0)
                {
                 //   GetNextLeadList();
                    GetNextLeadList(true);
                }
            }
        }

        public long GetLeadID(int channelID)
        {
            return _DiallerChannel[channelID - 1].LeadID;
        }

        public bool CheckChannelStatus()
        {
            //bool isChannelsFree = true;
            //for (int i = 0; i < _DiallerChannel.Count; i++)
            //{
            //    if (_DiallerChannel[i].CallResult == ClsChannel.CallStatus.CallInProgress || _DiallerChannel[i].CallResult == ClsChannel.CallStatus.ReadyState)
            //    {
            //        isChannelsFree = false;
            //        break;
            //    }
            //}

            bool isChannelsFree = false;
            for (int i = 0; i < _DiallerChannel.Count; i++)
            {
                if (_DiallerChannel[i].CallResult == ClsChannel.CallStatus.CallHangUp)
                {
                    isChannelsFree = true;
                    break;
                }
            }
            return isChannelsFree;
        }

        public void getPhoneNumber(int channelID, out string leadID, out string phoneNumber)
        {
            phoneNumber = "";
            foreach (ClsChannel cl in _DiallerChannel)
            {
                if (cl.CallResult == ClsChannel.CallStatus.CallHoldBySys)
                {
                    channelID = int.Parse(cl.ChannelID);
                    break;
                }
            }
            leadID = _DiallerChannel[channelID - 1].LeadID.ToString();
            foreach (ClsLead cl in objLeadCollection)
            {
                if (cl.ID.ToString() == leadID)
                {
                    phoneNumber = cl.PhoneNo.ToString();
                    break;
                }
            }
        }

        public void SetTransferCallDetail(long leadID, long phoneNO, long campaignID)
        {
            try
            {
                //foreach (ClsChannel channel in _DiallerChannel)
                //{
                //    if (channel.CallResult==ClsChannel.CallStatus.ReadyState  || channel.CallResult == ClsChannel.CallStatus.NotInCall || channel.CallResult == ClsChannel.CallStatus.CallHangUp)
                //    {
                //        ClsException.WriteToLogFile("channel gt free for transfered call");
                //        channel.CurrentDialStatus = "Predictive";
                //        channel.LeadID = leadID;
                //        channel.CurrentCampainID = campaignID;
                //        channel.CurrentPhoneNo = phoneNO;
                //        channel.UserID = UserID;
                //        channel.ConfID = long.Parse("1");
                //        channel.IsDNC = false;
                //        channel.IsGlobal = false;
                //        channel.CalledDate = DateTime.Now;
                //        channel.StartDate = DateTime.Now;
                //        ActiveChannel = channel;
                //        InsertTransfteredLead(leadID, phoneNO, campaignID);
                //        break;
                //    }
                //}

                for (int i = 0; i < _DiallerChannel.Count; i++)
                {
                    if (_DiallerChannel[i].CallResult == ClsChannel.CallStatus.ReadyState || _DiallerChannel[i].CallResult == ClsChannel.CallStatus.NotInCall || _DiallerChannel[i].CallResult == ClsChannel.CallStatus.CallHangUp)
                    {
                        
                        _DiallerChannel[i].CurrentDialStatus = "Predictive";
                        _DiallerChannel[i].CurrentPhoneNo = phoneNO;
                        _DiallerChannel[i].CurrentCampainID = campaignID;
                        _DiallerChannel[i].LeadID = leadID;
                        _DiallerChannel[i].UserID = UserID;
                        _DiallerChannel[i].ConfID = long.Parse("1");
                        _DiallerChannel[i].IsDNC = false;
                        _DiallerChannel[i].IsGlobal = false;
                        _DiallerChannel[i].CalledDate = DateTime.Now;
                        _DiallerChannel[i].StartDate = DateTime.Now;
                        ActiveChannel = _DiallerChannel[i];
                        InsertTransfteredLead(leadID, phoneNO, campaignID);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetTransferCallDetail()", "ClsChannelManager.cs");
            }
        }

        public bool fncIsAnotherCallRunning()
        {
            bool isAnotherCallRunning = false;

            foreach (ClsChannel cl in _DiallerChannel)
            {
                if (cl.CallResult == ClsChannel.CallStatus.CallInProgress || cl.CallResult==ClsChannel.CallStatus.CallDispose)
                {
                    // Transfter Call to Another Agent Who is Free
                    isAnotherCallRunning = true;
                    break;
                }
            }
            return isAnotherCallRunning;
        }

        public int FreeChannelStatusNo()
        {
            int channelNO = 1;
            foreach (ClsChannel cl in _DiallerChannel)
            {
                if (cl.CallResult == ClsChannel.CallStatus.NotInCall || cl.CallResult == ClsChannel.CallStatus.CallHangUp || cl.CallResult == ClsChannel.CallStatus.CallDispose)
                {
                    channelNO = int.Parse(cl.ChannelID);
                    break;
                }
            }
            return channelNO;

        }

        public int HoldChannelID(out string PhNumber)
        {
            int HoldChannelID = 0;
            PhNumber = string.Empty;
            foreach (ClsChannel cl in _DiallerChannel)
            {
                if (cl.CallResult == ClsChannel.CallStatus.CallHoldBySys)
                {
                    HoldChannelID = int.Parse(cl.ChannelID);
                    PhNumber = cl.CurrentPhoneNo.ToString();
                    break;
                }
            }

            return HoldChannelID;
        }

        public void SetActiveChannel4Manual()
        {
            ActiveChannel = _DiallerChannel[1];
            ActiveChannel = _DiallerChannel[0];
        }

        public int FncRunningChannel()
        {
            int ChannelId = -1;
            foreach (ClsChannel cl in _DiallerChannel)
            {
                if (cl.CallResult == ClsChannel.CallStatus.CallInProgress)
                {
                    ChannelId = int.Parse(cl.ChannelID);
                    break;
                }
            }
            return ChannelId;
        }

        public void FncGetActiveCallInfo(int channelID,out long leadID,out long phoneNo)
        {
            phoneNo = _DiallerChannel[channelID - 1].CurrentPhoneNo;
            leadID = _DiallerChannel[channelID - 1].LeadID;
        }
    }

}
