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
using System.ServiceModel;
using System.Threading;
using System.Windows;
using PredictiveDialler.Business;
using VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P;
using VMuktiAPI;
using VMuktiService;
using System.Collections;
using PredictiveDialler.Business.DashBoard;
using PredictiveDialler.Business.Services;
using PredictiveDialler.Business.Services.RecordedFileServices;
using PredictiveDialler.Business.Services.MessageContract;

namespace PredictivDialer.Presentation
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
   
    public enum ModulePermissions
    {
        PredictiveDial = 1,
        ManualDial = 2,
        View = 3
    }

    public partial class PredictiveDialer : System.Windows.Controls.UserControl
    {
        ClsChannelManager objChannelManager = null;
        NetPeerClient CallInfoClient = null;
        object objCallInfo = new svcCallInfo();
        string StrOrkaInstallDirectory = "";
        
        public static INetP2PBootStrapPredictiveServiceChannel CallInfoChannel;
        
        List<string> lstExtraCallInfo = new List<string>();
        
        string ExtraCallChannelId = "";
        int intTransferConfNumber = 1;

        NetPeerClient ncActiveCallInfo = null;
        object objNetP2PActiveCallInfo = new NetP2PBootStrapActiveCallReportDelegates();
        public static INetP2PBootStrapActiveCallReportChannel chNetP2PBootStrapActiveCallReportChannel;

        System.Threading.Thread thHostDashBoard = null;
        object objDashBoard = new NetP2PBootStrapDashBoardDelegate();
        INetP2PBootStrapdashBoardChannel channelP2PDashBoard;
        string strUri = string.Empty;

        object objRecordedFiles = new NetP2PBootStrapRecordedFileDelegate();
        INetP2PBootStrapRecordedFileChannel channelNetTcpUploadRecorededFiles = null;
        System.Threading.Thread tHostRecordedFiles = null;


        bool isUserAvailable = false;
        public bool SIPUserAvailable
        {
            get
            {
                return isUserAvailable;
            }
            set
            {
                isUserAvailable = value;
            }
        }

        public PredictiveDialer(ModulePermissions[] MyPermissions)// done
        {
            InitializeComponent();

            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\Orkaudio");
                if (sk1 != null)
                {
                    StrOrkaInstallDirectory = (string)sk1.GetValue("Install_Dir");
                }
                else
                {
                    StrOrkaInstallDirectory = "";
                    VMuktiAPI.ClsException.WriteToLogFile("Oreka is not Installed");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MyDialer()", "MyDialer.xaml.cs");
            } 

            try
            {
                objChannelManager = new ClsChannelManager();

                btnManualDial.Click += new RoutedEventHandler(btnManualDial_Click);
                btnPredictiveDial.Click += new RoutedEventHandler(btnPredictiveDial_Click);
                //this.Unloaded += new RoutedEventHandler(PredictiveDialer_Unloaded);
                Application.Current.MainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                VMuktiHelper.RegisterEvent("SetDispositionForPredictive").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(SetDispositionForPredictive_VMuktiEvent);
                VMuktiHelper.RegisterEvent("SetChannelStatusForPredictive").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(SetChannelStatusForPredictive_VMuktiEvent);
                VMuktiHelper.RegisterEvent("AllModulesLoadedForPredictive").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(AllCtlLoaded_VMuktiEvent);
                VMuktiHelper.RegisterEvent("SetPredictiveDialerEnable").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(SetPredictivePhoneDEnable_VMuktiEvent);
                VMuktiHelper.RegisterEvent("SetAgentNumber").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(SetAgentNumber_VMuktiEvent);

                try
                {
                    if (VMuktiAPI.VMuktiInfo.strExternalPBX == "true")
                    {
                        if (!objChannelManager.RegisterSIPUser())
                        {
                            SIPUserAvailable = false;
                        }
                        else
                        {
                            SIPUserAvailable = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                

                ((svcCallInfo)objCallInfo).EntsvcJoin += new svcCallInfo.DelsvcJoin(PredictiveDialer_EntsvcJoin);
                ((svcCallInfo)objCallInfo).EntAddExtraCall += new svcCallInfo.DelsvcAddExtraCall(PredictiveDialer_EntAddExtraCall);
                ((svcCallInfo)objCallInfo).EntRequestExtraCall += new svcCallInfo.DelsvcRequestExtraCall(PredictiveDialer_EntRequestExtraCall);
                ((svcCallInfo)objCallInfo).EntSendExtraCall += new svcCallInfo.DelsvcSendExtraCall(PredictiveDialer_EntSendExtraCall);
                ((svcCallInfo)objCallInfo).EntRemoveExtraCall += new svcCallInfo.DelsvcRemoveExtraCall(PredictiveDialer_EntRemoveExtraCall);
                ((svcCallInfo)objCallInfo).EntRequestFunctionToExecute += new svcCallInfo.DelsvcRequestFunctionToExecute(PredictiveDialer_EntRequestFunctionToExecute);
                ((svcCallInfo)objCallInfo).EntReplyFunctionExecuted += new svcCallInfo.DelsvcReplyFunctionExecuted(PredictiveDialer_EntReplyFunctionExecuted);
                ((svcCallInfo)objCallInfo).EntHangUpCall += new svcCallInfo.DelsvcHangUpCall(PredictiveDialer_EntHangUpCall);
                ((svcCallInfo)objCallInfo).EntUnJoin += new svcCallInfo.DelsvcUnJoin(PredictiveDialer_EntUnJoin);

                if (CallInfoChannel != null && CallInfoChannel.State == CommunicationState.Opened)
                {
                    CallInfoChannel.Close();
                    CallInfoChannel = null;
                }
                CallInfoClient = new VMuktiService.NetPeerClient();
                CallInfoChannel = (INetP2PBootStrapPredictiveServiceChannel)CallInfoClient.OpenClient<INetP2PBootStrapPredictiveServiceChannel>("net.tcp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapPredictive", "P2PBootStrapPredictiveMesh", ref objCallInfo);
                CallInfoChannel.svcJoin(objChannelManager.AgentNumber.ToString(), objChannelManager.CurrentCampaingID.ToString());
                
                #region Starting Thread for DashBoard and uploading recorded files.

                thHostDashBoard = new Thread(new ParameterizedThreadStart(HostDashBoard));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapDashBoard");
                lstParams.Add("P2PDashBoardMesh");
                thHostDashBoard.Start(lstParams);

                //File Recoreding
                tHostRecordedFiles = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(HostRecordedFiles));
                List<object> lstParams1 = new List<object>();
                lstParams1.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapRecordedFiles");
                lstParams1.Add("P2PRecordedFiles");
                tHostRecordedFiles.Start(lstParams1);

                #endregion

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer(Outer)", "PredictiveDialer.xaml.cs");
            }
        }

        #region Host DashBoard Service n Events

        public void HostDashBoard(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUri = lstTempObj[0].ToString();

                NetPeerClient npcDashBoard = new NetPeerClient();
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcUnJoin += new NetP2PBootStrapDashBoardDelegate.delsvcUnJoin(PredictiveDialer_EntsvcUnJoin);
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcJoin += new NetP2PBootStrapDashBoardDelegate.delsvcJoin(PredictiveDialer_EntsvcJoin);
                ((NetP2PBootStrapDashBoardDelegate)objDashBoard).EntsvcGetCallInfo += new NetP2PBootStrapDashBoardDelegate.DelsvcGetCallInfo(PredictiveDialer_EntsvcGetCallInfo);

                channelP2PDashBoard = (INetP2PBootStrapdashBoardChannel)npcDashBoard.OpenClient<INetP2PBootStrapdashBoardChannel>(strUri, lstTempObj[1].ToString(), ref objDashBoard);
                channelP2PDashBoard.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HostDialerServices()", "MyDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntsvcJoin(string uname)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        void PredictiveDialer_EntsvcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        void PredictiveDialer_EntsvcUnJoin(string uname)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }


        #endregion

        #region WCF Events For Recording Calls
        public void HostRecordedFiles(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUri = lstTempObj[0].ToString();

                NetPeerClient npcRecordedFiles = new NetPeerClient();
                ((NetP2PBootStrapRecordedFileDelegate)objRecordedFiles).EntsvcRecordedFileJoin += new NetP2PBootStrapRecordedFileDelegate.delsvcRecordedFileJoin(PredictiveDialer_EntsvcRecordedFileJoin);
                ((NetP2PBootStrapRecordedFileDelegate)objRecordedFiles).EntsvcSendRecordedFiles += new NetP2PBootStrapRecordedFileDelegate.delsvcSendRecordedFiles(PredictiveDialer_EntsvcSendRecordedFiles);
                ((NetP2PBootStrapRecordedFileDelegate)objRecordedFiles).EntsvcRecordedFileUnJoin += new NetP2PBootStrapRecordedFileDelegate.delsvcRecordedFileUnJoin(PredictiveDialer_EntsvcRecordedFileUnJoin);


                channelNetTcpUploadRecorededFiles = (INetP2PBootStrapRecordedFileChannel)npcRecordedFiles.OpenClient<INetP2PBootStrapRecordedFileChannel>(strUri, lstTempObj[1].ToString(), ref objRecordedFiles);

                clsMessageContract objContract = new clsMessageContract();
                objContract.uname = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                objContract.fName = "";
                objContract.fExtension = "";
                objContract.fLength = 0;
                objContract.fStream = new MemoryStream();

                channelNetTcpUploadRecorededFiles.svcRecordedFileJoin(objContract);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HostRecordedFiles()", "MyDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntsvcRecordedFileJoin(clsMessageContract mcRFJoin)
        {
        }

        void PredictiveDialer_EntsvcSendRecordedFiles(clsMessageContract mcSendRecordedFiles)
        {
        }

        void PredictiveDialer_EntsvcRecordedFileUnJoin(clsMessageContract mcRFUnJoin)
        {
        }

        void fncRecordedFileUpload(long LeadID, DateTime CalledDate, long DispositionID)
        {
            try
            {
                string ZoneName = ClsDisposition.GetZoneName(LeadID);
                string DispositionName = objChannelManager.GetDispositionName(DispositionID);
                string PhoneNo = objChannelManager.GetPhoneNo(LeadID);

                //Point out to the file from a specific folder

                DateTime dt = DateTime.Now;
                string FilePath = StrOrkaInstallDirectory + @"\AudioRecordings\" + dt.ToString("yyyy") + "\\" + dt.ToString("MM") + "\\" + dt.ToString("dd") + "\\" + dt.ToString("HH");
                string[] FileName = Directory.GetFiles(FilePath);

                //string fname = System.IO.Path.GetFileName(FileName[0]
                string oldFilePath = FilePath + "\\" + System.IO.Path.GetFileName(FileName[0]);
                string fext = System.IO.Path.GetExtension(FileName[0]);

                string date = CalledDate.ToString("yyyy") + CalledDate.ToString("MM") + CalledDate.ToString("dd");
                string time = CalledDate.ToString("HH") + "." + CalledDate.ToString("mm") + "." + CalledDate.ToString("ss");

                string newFileName = DispositionName + "_" + ZoneName + "_" + date + "_" + time + "_" + PhoneNo + fext;
                string newFilePath = FilePath.Trim() + "\\" + newFileName.Trim();

                FileInfo fi = new FileInfo(oldFilePath);
                if (File.Exists(newFilePath))
                    File.Delete(newFilePath);
                else
                    File.Move(oldFilePath, newFilePath);


                objChannelManager.ActiveChannel.RecordedFileName = newFileName;
                FileStream fs = new FileStream(newFilePath, FileMode.Open, FileAccess.Read);
                byte[] FileData = new byte[fs.Length];
                fs.Read(FileData, 0, (int)fs.Length);
                fs.Close();
                //fs.Flush();

                MemoryStream mms = new MemoryStream(FileData);
                mms.Position = 0;

                clsMessageContract objContract = new clsMessageContract();
                objContract.uname = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                objContract.fName = System.IO.Path.GetFileName(newFilePath);
                objContract.fExtension = System.IO.Path.GetExtension(newFilePath);
                objContract.fLength = mms.Length;
                objContract.fStream = mms;

                channelNetTcpUploadRecorededFiles.svcSendRecordedFiles(objContract);
                File.Delete(newFilePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncRecordedFileUpload", "AutoDialer-MyDialer.xaml.cs");
            }
        }

        #endregion

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                onExit();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "PredictiveDialer.xaml.cs");
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                onExit();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MainWindow_Closing()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                onExit();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_Unloaded()", "PredictiveDialer.xaml.cs");
            }
        }

        bool isExitCalled = false;

        void onExit()
        {
            try
            {
                if (!isExitCalled)
                {
                    VMuktiHelper.UnRegisterEvent("SetDispositionForPredictive");
                    VMuktiHelper.UnRegisterEvent("SetChannelStatusForPredictive");

                    if (CallInfoChannel != null)
                    {
                        CallInfoChannel.Close();
                        CallInfoChannel.Dispose();
                        CallInfoChannel = null;
                    }
                    isExitCalled = true;
                }
                objChannelManager.CallExit();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "onExit()", "PredictiveDialer.xaml.cs");
            }
        }

        # region WCF Services For Predictive Dialer

        void PredictiveDialer_EntsvcJoin(string UserNumber, string CampaignID)
        {
            try { }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntsvcJoin()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntAddExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber)
        {
            try
            {
                if (objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress != SenderUserNumber && objChannelManager.CurrentCampaingID.ToString() == CampaignID)
                {
                    lstExtraCallInfo.Add(SenderUserNumber);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntAddExtraCall()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntRequestExtraCall(string SenderUserNumber, string CampaignID, string CallRequestedUserNumber)
        {
            try
            {
                if (objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress == SenderUserNumber && objChannelManager.CurrentCampaingID.ToString() == CampaignID)
                {

                    string strPhNumber = "";
                    string strLeadId = "";

                    objChannelManager.getPhoneNumber(int.Parse(ExtraCallChannelId), out strLeadId, out strPhNumber);

                    VMuktiHelper.CallEvent("TransferCall", this, new VMuktiEventArgs(int.Parse(ExtraCallChannelId), "###*7" + intTransferConfNumber.ToString() + "#"));
                    //System.Threading.Thread.Sleep(1000);

                    CallInfoChannel.svcSendExtraCall(objChannelManager.AgentNumber.ToString(), objChannelManager.CurrentCampaingID.ToString(), strPhNumber, CallRequestedUserNumber, strLeadId, "7" + intTransferConfNumber.ToString() + "@" + objChannelManager.SIPServerAddress);

                    if (intTransferConfNumber > 20)
                    {
                        intTransferConfNumber = 1;
                    }
                    else
                    {
                        intTransferConfNumber = intTransferConfNumber + 1;
                    }
                    objChannelManager.DeleteTransfteredLead(long.Parse(strLeadId));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntRequestExtraCall()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntSendExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber, string CallRequesedUserNumber, string LeadID,string ConfNumber)
        {
            try
            {
                if (objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress == CallRequesedUserNumber && objChannelManager.CurrentCampaingID.ToString() == CampaignID)
                {

                    string strPhNumber = objChannelManager.chManagerCountryId+ PhoneNumber;

                    VMuktiHelper.CallEvent("SetIncomingNumber", this, new VMuktiEventArgs(strPhNumber, objChannelManager.FreeChannelStatusNo(), ConfNumber));

                    objChannelManager.SetTransferCallDetail(long.Parse(LeadID), long.Parse(PhoneNumber), long.Parse(CampaignID));

                    System.Threading.Thread.Sleep(1000);

                    VMuktiHelper.CallEvent("Dial", this, new VMuktiEventArgs(ConfNumber, objChannelManager.FreeChannelStatusNo()));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntSendExtraCall()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntRemoveExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber)
        {
            try
            {
                if (objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress != SenderUserNumber && objChannelManager.CurrentCampaingID.ToString() == CampaignID)
                {
                    lstExtraCallInfo.Remove(SenderUserNumber);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntRemoveExtraCall()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntRequestFunctionToExecute(string FunctionType, string To, string From)
        {
            try
            {
                if (objChannelManager.AgentNumber + "@" + objChannelManager.SIPServerAddress == To)
                {
                    if (FunctionType == "Barge")
                    {
                        VMuktiHelper.CallEvent("BargeCall", this, new VMuktiEventArgs(objChannelManager.FreeChannelStatusNo(), "####" + intTransferConfNumber.ToString() + "#"));
                    }
                    else if (FunctionType == "Hijeck")
                    {
                        VMuktiHelper.CallEvent("HijeckCall", this, new VMuktiEventArgs(objChannelManager.FreeChannelStatusNo(), "####" + intTransferConfNumber.ToString() + "#"));
                    }
                    System.Threading.Thread.Sleep(2000);
                    CallInfoChannel.svcReplyFunctionExecuted(FunctionType, From, objChannelManager.AgentNumber + "@" + objChannelManager.SIPServerAddress, intTransferConfNumber.ToString());
                    if (intTransferConfNumber > 20)
                    {
                        intTransferConfNumber = 1;
                    }
                    else
                    {
                        intTransferConfNumber = intTransferConfNumber + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntRequestFunctionToExecute()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntReplyFunctionExecuted(string FunctionType, string To, string From, string ConfNumber)
        {
            try
            {
                if (objChannelManager.AgentNumber + "@" + objChannelManager.SIPServerAddress == To)
                {
                    VMuktiHelper.CallEvent("Dial", this, new VMuktiEventArgs(ConfNumber, objChannelManager.FreeChannelStatusNo()));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntReplyFunctionExecuted()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntHangUpCall(string AgentNumber)
        {
            try
            {
                if (AgentNumber == objChannelManager.AgentNumber)
                {
                    int temp = objChannelManager.FncRunningChannel();
                    if (temp != -1)
                    {
                        VMuktiHelper.CallEvent("HangUp", this, new VMuktiEventArgs(temp));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntHangUpCall()", "PredictiveDialer.xaml.cs");
            }
        }

        void PredictiveDialer_EntUnJoin()
        {
            try { }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PredictiveDialer_EntUnJoin()", "PredictiveDialer.xaml.cs");
            }
        }

        #endregion

        void btnManualDial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnManualDial.Content.ToString() == "Start Manual Dialing")
                {
                    objChannelManager.SetActiveChannel4Manual();
                    objChannelManager.ActiveChannel.CurrentDialStatus = "Manual";
                    btnManualDial.Content = "Stop Manual Dialing";
                    btnPredictiveDial.IsEnabled = false;
                    VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(true, "Manual"));
                }
                else if (btnManualDial.Content.ToString() == "Stop Manual Dialing")
                {
                    btnManualDial.Content = "Start Manual Dialing";
                    btnPredictiveDial.IsEnabled = true;
                    VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(false, "Manual"));

                    // For Hanging all calls that are running currently.
                    VMuktiHelper.CallEvent("HangUp", this, new VMuktiEventArgs("PredictiveDialer",1));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnManualDial_Click()", "PredictiveDialer.xaml.cs");
            }
        }

        void btnPredictiveDial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnPredictiveDial.Content.ToString() == "Start Predictive Dialing")
                {
                    long CountryCode = objChannelManager.GetCountryCode();
                    string CampPrefix = objChannelManager.GetCampaginPrefix();
                    btnPredictiveDial.Content = "Stop Predictive Dialing";
                    btnManualDial.IsEnabled = false;
                    objChannelManager.StartSyncProcess(true);

                    //if (objChannelManager.LeadCollection.Count != 0)
                    //{
                        VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(true, "Predictive"));
                  //  }

                    if (lstExtraCallInfo.Count > 0)
                    {

                        CallInfoChannel.svcRequestExtraCall(lstExtraCallInfo[0].ToString(), objChannelManager.CurrentCampaingID.ToString(), objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress);
                        CallInfoChannel.svcRemoveExtraCall(lstExtraCallInfo[0].ToString(), objChannelManager.CurrentCampaingID.ToString(), "");
                    }
                    
                    else
                    {
                    
                        objChannelManager.GetNextLeadList(true);
                        if (objChannelManager.CurrentCampaingID == 0)
                        {
                            MessageBox.Show("No Campaing available");
                            btnPredictiveDial.Content = "Start Predictive Dialing";
                        }
                        else if (objChannelManager.LeadCollection == null || objChannelManager.LeadCollection.Count == 0)
                        {
                            MessageBox.Show("No Fresh Leads available");
                            btnPredictiveDial.Content = "Start Predictive Dialing";
                        }
                        else
                        {
                            objChannelManager.FireCall();
                           // StartTimer(true);
                        }
                    }
                }
                else
                {
                    btnPredictiveDial.Content = "Start Predictive Dialing";
                    btnManualDial.IsEnabled = true;

                    VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(false, "Predictive"));

                    cnvMain.IsEnabled = true;
                    objChannelManager.StartSyncProcess(false);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnPredictiveDial_Click()", "PredictiveDialer.xaml.cs");
            }
        }

        #region VMukti Events

        void SetAgentNumber_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                objChannelManager.chManagerSetAgentNumber(e._args[0].ToString(), e._args[1].ToString(), e._args[2].ToString());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "setagentnumber_vmuktievent()", "predictivedialer.xaml.cs");
            }
        }

        void SetDispositionForPredictive_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                fncRecordedFileUpload(objChannelManager.ActiveChannel.LeadID, objChannelManager.ActiveChannel.CalledDate, long.Parse(e._args[0].ToString()));

                objChannelManager.SetDisposition(e);


                #region To Inform DashBoard 
                long LeadId = objChannelManager.ActiveChannel.LeadID;
                DateTime CalledDate = objChannelManager.ActiveChannel.CalledDate;
                DateTime ModifiedDate = objChannelManager.ActiveChannel.CalledDate;
                long ModifiedBy = objChannelManager.ActiveChannel.UserID;
                long GeneratedBy = objChannelManager.ActiveChannel.UserID;
                DateTime StartDate = objChannelManager.ActiveChannel.StartDate;
                DateTime StartTime = objChannelManager.ActiveChannel.StartDate;
                long DurationInSec = objChannelManager.ActiveChannel.CallDuration;
                long DispositionID = objChannelManager.ActiveChannel.DispositionID;
                long CampaignID = objChannelManager.ActiveChannel.CurrentCampainID;
                long ConfID = objChannelManager.ActiveChannel.ConfID;
                bool IsDeleted = false;
                string CallNote = objChannelManager.ActiveChannel.CallNote;
                bool IsDNC = objChannelManager.ActiveChannel.IsDNC;
                bool IsGlobal = objChannelManager.ActiveChannel.IsGlobal;
                if (channelP2PDashBoard != null)
                {
                    channelP2PDashBoard.svcGetCallInfo(LeadId, CalledDate, ModifiedDate, ModifiedBy, GeneratedBy, StartDate, Convert.ToDateTime(StartTime), DurationInSec, DispositionID, CampaignID, ConfID, IsDeleted, CallNote, IsDNC, IsGlobal);
                }
                #endregion

                objChannelManager.SetCallResult(ClsChannel.CallStatus.CallHangUp, int.Parse(e._args[e._args.Count - 1].ToString()) + 1);
                
                if (objChannelManager.CheckChannelStatus())
                {
                    objChannelManager.FncRemoveDialLead(int.Parse(e._args[e._args.Count - 1].ToString()) + 1);
                    if (btnPredictiveDial.Content.ToString().StartsWith("Stop"))
                    {
                        string PhNumber = string.Empty;
                        int HoltedChannelID = objChannelManager.HoldChannelID(out PhNumber);
                        if (HoltedChannelID != 0)
                        {
                            CallInfoChannel.svcRemoveExtraCall(objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress, objChannelManager.CurrentCampaingID.ToString(), "");
                            VMuktiHelper.CallEvent("UnMuteCall", this, new VMuktiEventArgs(HoltedChannelID.ToString(), PhNumber));
                            objChannelManager.SetCallResult(ClsChannel.CallStatus.CallInProgress, HoltedChannelID);
                        }
                        else
                        {
                            if (lstExtraCallInfo.Count > 0)
                            {
                                CallInfoChannel.svcRequestExtraCall(lstExtraCallInfo[0].ToString(), objChannelManager.CurrentCampaingID.ToString(), objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress);
                                CallInfoChannel.svcRemoveExtraCall(lstExtraCallInfo[0].ToString(), objChannelManager.CurrentCampaingID.ToString(), "");
                            }
                            else
                            {
                                svcFireAnotherCall();
                            }
                        }
                    }
                    else
                    {
                        objChannelManager.DeleteLeadTabelData();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetDispositionForPredictive_VMuktiEvent()", "PredictiveDialer.xaml.cs");
            }
        }

        // For Enabling/Disabling predictive phone.
        void SetPredictivePhoneDEnable_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                cnvMain.IsEnabled = bool.Parse(e._args[0].ToString());
                if (btnPredictiveDial.Content.ToString().StartsWith("Start") && btnManualDial.Content.ToString().StartsWith("Start"))
                {
                    VMuktiHelper.CallEvent("SetSoftPhoneEnable", this, new VMuktiEventArgs(false, e._args[1]));
                }
                else
                {
                    VMuktiHelper.CallEvent("SetSoftPhoneEnable", this, new VMuktiEventArgs(true, e._args[1]));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetPredictivePhoneDEnable_VMuktiEvent()", "PredictiveDialer.xaml.cs");
            }
        }
       
        // For Channel Status
        void SetChannelStatusForPredictive_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (e._args[0].ToString() == ClsChannel.CallStatus.CallInProgress.ToString())
                {
                    bool isTransferRequired = objChannelManager.SetCallResult(e._args[0], int.Parse(e._args[e._args.Count - 1].ToString()));
                    //InsertFilePathIntoDB(int.Parse(e._args[e._args.Count - 1].ToString()));
                    if (isTransferRequired)
                    {

                        ExtraCallChannelId = e._args[e._args.Count - 1].ToString();
                        CallInfoChannel.svcAddExtraCall(objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress, objChannelManager.CurrentCampaingID.ToString(), e._args[e._args.Count - 1].ToString());
                        // TransferCall to Agent Who is Free throgh WCF.
                        objChannelManager.SetCallResult(ClsChannel.CallStatus.CallHoldBySys, int.Parse(e._args[e._args.Count - 1].ToString()));
                    }
                    else
                    {
                        int channelNo = int.Parse(e._args[e._args.Count - 1].ToString());
                        
                        VMuktiHelper.CallEvent("SetLeadIDScript", this, new VMuktiEventArgs(objChannelManager.GetLeadID(channelNo), channelNo - 1));
                        VMuktiHelper.CallEvent("SetLeadIDCRM", this, new VMuktiEventArgs(objChannelManager.GetLeadID(int.Parse(e._args[e._args.Count - 1].ToString()))));

                    }
                }

                else if (e._args[0].ToString() == ClsChannel.CallStatus.CallHangUp.ToString())
                {
                    //Calling Function of WCF to get any active available calls.
                    //WCF Will call another functon of all agents with agentname and one parameter saying that you will recive on call or not.
                    objChannelManager.SetCallResult(e._args[0], int.Parse(e._args[e._args.Count - 1].ToString()));
                    CallInfoChannel.svcRemoveExtraCall(objChannelManager.AgentNumber.ToString() + "@" + objChannelManager.SIPServerAddress, objChannelManager.CurrentCampaingID.ToString(), "");
                    
                    if (objChannelManager.CheckChannelStatus())
                    {
                        if (btnPredictiveDial.Content.ToString().StartsWith("Stop"))
                        {
                            objChannelManager.FncRemoveDialLead(int.Parse(e._args[e._args.Count - 1].ToString()));
                        }
                        if (!objChannelManager.fncIsAnotherCallRunning())
                        {
                            svcFireAnotherCall();
                        }
                    }
                }
                else if (e._args[0].ToString() == ClsChannel.CallStatus.CallDispose.ToString())
                {
                    objChannelManager.SetCallResult(e._args[0], int.Parse(e._args[e._args.Count - 1].ToString()));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetChannelStatusForPredictive_VMuktiEvent()", "PredictiveDialer.xaml.cs");
            }
        }

        void AllCtlLoaded_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMuktiHelper.CallEvent("RegisterAgent", this, new VMuktiEventArgs(objChannelManager.AgentNumber, objChannelManager.AgentPassWord, objChannelManager.SIPServerAddress));
                VMuktiHelper.CallEvent("SetCampaignID", this, new VMuktiEventArgs(objChannelManager.CurrentCampaingID));
                btnManualDial_Click(null, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AllCtlLoaded_VMuktiEvent()", "PredictiveDialer.xaml.cs");
            }
        }

        public void svcFireAnotherCall()
        {
            try
            {
                if (btnPredictiveDial.Content.ToString().StartsWith("Stop"))
                {
                    objChannelManager.FireCall();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcFireAnotherCall()", "PredictiveDialer.xaml.cs");
            }
        }

        #endregion

        #region Fire Active Call Status

        //private void FireActiveCallReportStatus(bool isConnected, int channelNO)
        //{
        //    try
        //    {
        //        VMukti.Bussiness.CommonDataContracts.ActiveCalls clsActiveCallInfo = new VMukti.Bussiness.CommonDataContracts.ActiveCalls();
        //        clsActiveCallInfo.CurrentCallStartTime = DateTime.Now;
        //        if (isConnected)
        //        {
        //            clsActiveCallInfo.CurrentCallState = ClsChannel.CallStatus.CallInProgress.ToString();
        //        }
        //        else
        //        {
        //            clsActiveCallInfo.CurrentCallState = ClsChannel.CallStatus.CallHangUp.ToString();
        //        }
        //        clsActiveCallInfo.CurrentCampID = objChannelManager.CurrentCampaingID;

        //        objChannelManager.FncGetActiveCallInfo(channelNO, out clsActiveCallInfo.CurrentLeadID, out clsActiveCallInfo.CurrentPhoneNo);
                
        //        chNetP2PBootStrapActiveCallReportChannel.svcActiveCalls(clsActiveCallInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiHelper.ExceptionHandler(ex, "FireActiveCallReportStatus()", "PredictiveDialer.xaml.cs");
        //    }
        //}

        //void CloseActiveCallReportChannel()
        //{
        //    try
        //    {
        //        if (chNetP2PBootStrapActiveCallReportChannel != null && chNetP2PBootStrapActiveCallReportChannel.State != CommunicationState.Opened)
        //        {
        //            chNetP2PBootStrapActiveCallReportChannel.Close();
        //            chNetP2PBootStrapActiveCallReportChannel = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiHelper.ExceptionHandler(ex, "CloseActiveCallReportChannel()", "PredictiveDialer.xaml.cs");
        //    }

        //}

        #endregion
    }
}
