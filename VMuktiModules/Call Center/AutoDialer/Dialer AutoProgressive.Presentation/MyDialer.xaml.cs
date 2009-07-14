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
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
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
using Dialer_AutoProgressive.Business;
using Dialer_AutoProgressive.Common;
using VMuktiAPI;
using VMuktiService;
using Dialer_AutoProgressive.Business.Services;
using Dialer_AutoProgressive.Business.Services.MessageContract;
using Dialer_AutoProgressive.Business.Services.RecordedFileServices;
using System.IO;



namespace Dialer_AutoProgressive.Presentation
{
    /// <summary>
    /// Interaction logic for MyUserControl.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        AutoDial=1,
        ManualDial=2,
        View = 3
    }
    public partial class MyDialer : System.Windows.Controls.UserControl
    {
        

        #region Thread and variable declaration for WCF services and to open a client

        object objDialer = new NetP2PBootStrapDashBoardDelegate();
        INetP2PBootStrapdashBoardChannel channelNetTcpDialer = null;
        System.Threading.Thread tHostDialer = null;
        string strUri;

        //Uploading Recoreded Files
        //string machineIP, userName, bootstrapIP;        
        object objRecordedFiles = new NetP2PBootStrapRecordedFileDelegate();
        INetP2PBootStrapRecordedFileChannel channelNetTcpUploadRecorededFiles = null;
        System.Threading.Thread tHostRecordedFiles = null;

        #endregion
       
        ClsChannelManager channelManager = null;
        ModulePermissions[] _MyPermissions;
        bool isUserAvailable = false;
        string StrOrkaInstallDirectory = "";

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

        public MyDialer(ModulePermissions[] MyPermissions)
        {
            try
            {
                channelManager = new ClsChannelManager();
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
                
                #region Starting Thread for DashBoard and uploading recorded files.

                tHostDialer = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(HostDialerServices));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapDashBoard");
                lstParams.Add("P2PDashBoardMesh");
                tHostDialer.Start(lstParams);
               
                //File Recoreding
                tHostRecordedFiles = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(HostRecordedFiles));
                List<object> lstParams1 = new List<object>();
                lstParams1.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapRecordedFiles");
                lstParams1.Add("P2PRecordedFiles");
                tHostRecordedFiles.Start(lstParams1);

                #endregion

                btnAutomaticDial.Visibility = Visibility.Hidden;
                btnManualDial.Visibility = Visibility.Hidden;
                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                btnManualDial.Click += new RoutedEventHandler(btnManualDial_Click);
                btnAutomaticDial.Click += new RoutedEventHandler(btnAutomaticDial_Click);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);
                //this.Unloaded += new RoutedEventHandler(MyDialer_Unloaded);

                VMuktiHelper.RegisterEvent("SetChannelValues").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_SetChannelValues);
                VMuktiHelper.RegisterEvent("Logoff").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent_Logoff);
                VMuktiHelper.RegisterEvent("AllModulesLoaded").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent_AllCtlLoaded);
                VMuktiHelper.RegisterEvent("SetDialerEnable").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent_SetMyDialerEnable);
                VMuktiHelper.RegisterEvent("SetChannelStatus").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent_SetChannelStatus);
                VMuktiHelper.RegisterEvent("SetDisposition").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent_SetDisposition);
                VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent);
				//VMuktiHelper.RegisterEvent("SetRecordedFiles").VMuktiEvent +=new VMuktiEvents.VMuktiEventHandler(MyDialer_VMuktiEvent_SetRecordedFiles);
                //VMuktiHelper.CallEvent("AllModulesLoaded", this, null);
                try
                {
                    if (VMuktiAPI.VMuktiInfo.strExternalPBX == "true")
                    {
                    if (!channelManager.RegisterSIPUser())
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
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer()", "MyDialer.xaml.cs");
            }

        }
        #region Hosting Dialer and uploading record Service (added by Alpa)

        //Hosting Dialer Service 
        public void HostDialerServices(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUri = lstTempObj[0].ToString();

                NetPeerClient npcDialer = new NetPeerClient();
                ((NetP2PBootStrapDashBoardDelegate)objDialer).EntsvcUnJoin += new NetP2PBootStrapDashBoardDelegate.delsvcUnJoin(MyDialer_EntsvcUnJoin);
                ((NetP2PBootStrapDashBoardDelegate)objDialer).EntsvcJoin += new NetP2PBootStrapDashBoardDelegate.delsvcJoin(MyDialer_EntsvcJoin);
                ((NetP2PBootStrapDashBoardDelegate)objDialer).EntsvcGetCallInfo += new NetP2PBootStrapDashBoardDelegate.DelsvcGetCallInfo(MyDialer_EntsvcGetCallInfo);
                ((NetP2PBootStrapDashBoardDelegate)objDialer).EntsvcGetAgents += new NetP2PBootStrapDashBoardDelegate.DelsvcGetAgents(MyDialer_EntsvcGetAgents);
                ((NetP2PBootStrapDashBoardDelegate)objDialer).EntsvcSetAgents += new NetP2PBootStrapDashBoardDelegate.DelsvcSetAgents(MyDialer_EntsvcSetAgents);
                channelNetTcpDialer = (INetP2PBootStrapdashBoardChannel)npcDialer.OpenClient<INetP2PBootStrapdashBoardChannel>(strUri, lstTempObj[1].ToString(), ref objDialer);                
                channelNetTcpDialer.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HostDialerServices()", "MyDialer.xaml.cs");
            }
        }

       

        //DialerServices Function.
        void MyDialer_EntsvcJoin(string uname)
        {
           
        }

        void MyDialer_EntsvcGetCallInfo(long LeadID, DateTime CalledDate, DateTime ModifiedDate, long ModifiedBy, long GeneratedBy, DateTime StartDate, DateTime StartTime, long DurationInSecond, long DispositionID, long CampaignID, long ConfID, bool IsDeleted, string CallNote, bool isDNC, bool isGlobal)
        {
           
        }

        void MyDialer_EntsvcUnJoin(string uname)
        {
            
        }

        void MyDialer_EntsvcGetAgents(int intCampaignID, string uname)
        {
            if (intCampaignID.ToString() == VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString())
            {
                channelNetTcpDialer.svcSetAgents(VMuktiAPI.VMuktiInfo.CurrentPeer.ID, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, intCampaignID, uname);
            }
        }

        void MyDialer_EntsvcSetAgents(int intAgentID, string strAgentName, int intCampaignID, string to)
        {
        }

        //Hosting RecordedFiles Service
        public void HostRecordedFiles(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUri = lstTempObj[0].ToString();

                NetPeerClient npcRecordedFiles = new NetPeerClient();
                ((NetP2PBootStrapRecordedFileDelegate)objRecordedFiles).EntsvcRecordedFileUnJoin +=new NetP2PBootStrapRecordedFileDelegate.delsvcRecordedFileUnJoin(MyDialer_EntsvcRecordedFileUnJoin);
                ((NetP2PBootStrapRecordedFileDelegate)objRecordedFiles).EntsvcSendRecordedFiles+=new NetP2PBootStrapRecordedFileDelegate.delsvcSendRecordedFiles(MyDialer_EntsvcSendRecordedFiles);
                ((NetP2PBootStrapRecordedFileDelegate)objRecordedFiles).EntsvcRecordedFileJoin+=new NetP2PBootStrapRecordedFileDelegate.delsvcRecordedFileJoin(MyDialer_EntsvcRecordedFileJoin);

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

        //Upload Recorded File Functions
        void MyDialer_EntsvcRecordedFileJoin(clsMessageContract mcRFJoin)
        { }

        void MyDialer_EntsvcSendRecordedFiles(clsMessageContract mcSendRecordedFiles)
        {
            try
            {
                //if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP == VMuktiAPI.VMuktiInfo.BootStrapIPs[0])
                //{
                //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != mcSendRecordedFiles.uname)
                //    {
                //        System.Xml.XmlDocument ConfDoc = new System.Xml.XmlDocument();
                //        ConfDoc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Configuration.xml");
                //        System.Xml.XmlNodeList xmlNodes = null;
                //        xmlNodes = ConfDoc.GetElementsByTagName("PhysicalPathOfVirtualDirectory");
                //        string VirtualDirectoryPath = xmlNodes[0].Attributes["Value"].Value.ToString();

                //        if (!Directory.Exists(VirtualDirectoryPath + "\\AudioRecordedFiles"))
                //        {
                //            Directory.CreateDirectory(VirtualDirectoryPath + "\\AudioRecordedFiles");
                //        }

                //        byte[] byteArray = fncStreamToByteArry(mcSendRecordedFiles.fStream);

                //        FileStream fs = new FileStream(VirtualDirectoryPath, FileMode.OpenOrCreate, FileAccess.Write);
                //        fs.Seek(0, SeekOrigin.Begin);
                //        fs.Write(byteArray, 0, byteArray.Length);

                //        fs.Close();

                //        //MessageBox.Show("File has been uploaded successfully on server");

                //    }
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlSearch_EntsvcJoin()", "ctlSearch.xaml.cs");
            }
        }

        void MyDialer_EntsvcRecordedFileUnJoin(clsMessageContract mcRFUnJoin)
        { }

        byte[] fncStreamToByteArry(Stream streamInput)
        {
            try
            {
                List<byte> myBytes = new List<byte>();
                int num;
                while ((num = streamInput.ReadByte()) != -1)
                {
                    myBytes.Add((byte)num);
                }
                return myBytes.ToArray();
            }
            catch
            {
                //ClsException.WriteToLogFile("ctlVideo.xaml.cs:-fncStreamToByteArry():-" + ex.Message);
                return null;
            }
        }

        #endregion

        void FncPermissionsReview()
        {
            try
            {
                this.Visibility = Visibility.Hidden;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.View)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                    else if (_MyPermissions[i] == ModulePermissions.AutoDial)
                    {
                        btnAutomaticDial.Visibility = Visibility.Visible;
                    }
                    else if (_MyPermissions[i] == ModulePermissions.ManualDial)
                    {
                        btnManualDial.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "MyDialer.xaml.cs");
            }
        }

        void MyDialer_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_Unloaded()", "MyDialer.xaml.cs");
            }
        }

        #region VMukti Events
        void MyDialer_VMuktiEvent_SetChannelStatus(object sender, VMuktiEventArgs e)
        {
            try
            {
                channelManager.SetCallResult(e._args[0]);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_VMuktiEvent_SetChannelStatus()", "MyDialer.xaml.cs");
            }
        }       

       

        void fncRecordedFileUpload(long LeadID, DateTime CalledDate, long DispositionID)
        {
            try
            {
                string ZoneName = Dialer_AutoProgressive.Business.ClsDisposition.GetZoneName(LeadID);
                string DispositionName = channelManager.GetDispositionName(DispositionID);
                string PhoneNo = channelManager.GetPhoneNo(LeadID);
                
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



                    channelManager.ActiveChannel.RecordedFileName = newFileName;
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex,"fncRecordedFileUpload","AutoDialer-MyDialer.xaml.cs");
            }
        }
        
        #endregion

        void MyDialer_VMuktiEvent_SetDisposition(object sender, VMuktiEventArgs e)
        {
            try
            {
                fncRecordedFileUpload(channelManager.ActiveChannel.LeadID, channelManager.ActiveChannel.CalledDate, long.Parse(e._args[0].ToString()));

                channelManager.SetDisposition(e);

                #region To Inform DashBoard (Added by Alpa)
                long LeadId = channelManager.ActiveChannel.LeadID;
                DateTime CalledDate = channelManager.ActiveChannel.CalledDate;
                DateTime ModifiedDate = channelManager.ActiveChannel.CalledDate;
                long ModifiedBy = channelManager.ActiveChannel.UserID;
                long GeneratedBy = channelManager.ActiveChannel.UserID;
                DateTime StartDate = channelManager.ActiveChannel.StartDate;
                DateTime StartTime = channelManager.ActiveChannel.StartDate;
                long DurationInSec = channelManager.ActiveChannel.CallDuration;
                long DispositionID = channelManager.ActiveChannel.DispositionID;
                long CampaignID = channelManager.ActiveChannel.CurrentCampainID;
                long ConfID = channelManager.ActiveChannel.ConfID;
                bool IsDeleted = false;
                string CallNote = channelManager.ActiveChannel.CallNote;
                bool IsDNC = channelManager.ActiveChannel.IsDNC;
                bool IsGlobal = channelManager.ActiveChannel.IsGlobal;
                channelNetTcpDialer.svcGetCallInfo(LeadId, CalledDate, ModifiedDate, ModifiedBy, GeneratedBy, StartDate, Convert.ToDateTime(StartTime), DurationInSec, DispositionID, CampaignID, ConfID, IsDeleted, CallNote, IsDNC, IsGlobal);
                #endregion

                if (btnAutomaticDial.Content.ToString() == "Stop Automatic Dialing")
                {
                    FncRemoveDialLead();
                    FireCall();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_VMuktiEvent_SetDisposition()", "MyDialer.xaml.cs");
            }
        }

        void MyDialer_VMuktiEvent_SetMyDialerEnable(object sender, VMuktiEventArgs e)
        {
            try
            {
                cnvMain.IsEnabled = bool.Parse(e._args[0].ToString());
                if (btnAutomaticDial.Content.ToString().StartsWith("Start") && btnManualDial.Content.ToString().StartsWith("Start"))
                {
                    VMuktiHelper.CallEvent("SetSoftPhoneEnable", this, new VMuktiEventArgs(false));
                }
                else
                {
                    VMuktiHelper.CallEvent("SetSoftPhoneEnable", this, new VMuktiEventArgs(true));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_VMuktiEvent_SetMyDialerEnable()", "MyDialer.xaml.cs");
            }
        }

        void MyDialer_VMuktiEvent_AllCtlLoaded(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMuktiHelper.CallEvent("RegisterAgent", this, new VMuktiEventArgs(channelManager.AgentNumber, channelManager.AgentPassWord, channelManager.SIPServerAddress));
                VMuktiHelper.CallEvent("SetCampaignID", this, new VMuktiEventArgs(channelManager.CurrentCampaingID));
                btnManualDial_Click(null, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_VMuktiEvent_AllCtlLoaded()", "MyDialer.xaml.cs");
            }
        }

        void MyDialer_SetChannelValues(object sender, VMuktiEventArgs e)
        {
            try
            {
                channelManager.SetChannelValues(e);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_SetChannelValues()", "MyDialer.xaml.cs");
            }
        }

        void SetAllChannelStatus_myEvent(object sender, VMuktiEventArgs e)
        {

        }

        void MyDialer_VMuktiEvent_Logoff(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(100, "None", false));
                channelManager.CallExit();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_VMuktiEvent_Logoff()", "MyDialer.xaml.cs");
            }
        }

        

        public void ClosePod()
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("SetChannelValues");
                VMuktiHelper.UnRegisterEvent("Logoff");
                VMuktiHelper.UnRegisterEvent("AllModulesLoaded");
                VMuktiHelper.UnRegisterEvent("SetDialerEnable");
                VMuktiHelper.UnRegisterEvent("SetChannelStatus");
                VMuktiHelper.UnRegisterEvent("SetDisposition");
                channelManager.CallExit();
                if (channelNetTcpDialer != null)
                {
                    channelNetTcpDialer.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    channelNetTcpDialer.Close();
                    channelNetTcpDialer = null;
                }
                if (channelNetTcpUploadRecorededFiles != null)
                {
                    channelNetTcpUploadRecorededFiles.Close();
                    channelNetTcpUploadRecorededFiles = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception under ClosePod: " + ex.Message);
                VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "MyDialer.xaml.cs");
            }
        }

        void MyDialer_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("SetChannelValues");
                VMuktiHelper.UnRegisterEvent("Logoff");
                VMuktiHelper.UnRegisterEvent("AllModulesLoaded");
                VMuktiHelper.UnRegisterEvent("SetDialerEnable");
                VMuktiHelper.UnRegisterEvent("SetChannelStatus");
                VMuktiHelper.UnRegisterEvent("SetDisposition");
                channelManager.CallExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception under ClosePod: " + ex.Message);
                VMuktiHelper.ExceptionHandler(ex, "MyDialer_VMuktiEvent()", "MyDialer.xaml.cs");
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("SetChannelValues");
                VMuktiHelper.UnRegisterEvent("Logoff");
                VMuktiHelper.UnRegisterEvent("AllModulesLoaded");
                VMuktiHelper.UnRegisterEvent("SetDialerEnable");
                VMuktiHelper.UnRegisterEvent("SetChannelStatus");
                VMuktiHelper.UnRegisterEvent("SetDisposition");
                channelManager.CallExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception under Current_Exit: " + ex.Message);
                VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "MyDialer.xaml.cs");
            }
        }

        public enum CallStatus
        {
            NotInCall, CallInProgress, DialingToDest, RingingToDest,
            BusyTone, AMD, DTMF, DestUserStratListen, CallHangUp, CallHoldByUser,
            CallHoldBySys, CallDispose, ReadyState
        };   
     
        void btnManualDial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                channelManager.StartSyncProcess(false);
                if (btnManualDial.Content.ToString() == "Start Manual Dialing")
                {
                    channelManager.ActiveChannel.CurrentDialStatus = "Manual";
                    btnManualDial.Content = "Stop Manual Dialing";
                    btnAutomaticDial.IsEnabled = false;
                    VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(true, "Manual"));
                }
                else if (btnManualDial.Content.ToString() == "Stop Manual Dialing")
                {
                    
                    btnManualDial.Content = "Start Manual Dialing";
                    btnAutomaticDial.IsEnabled = true;
                    VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(false, "Manual"));

                    if (channelManager.ActiveChannel.CallResult != (ClsChannel.CallStatus)CallStatus.NotInCall && channelManager.ActiveChannel.CallResult != (ClsChannel.CallStatus)CallStatus.CallHangUp)
                    {
                        VMuktiHelper.CallEvent("HangUp", this, new VMuktiEventArgs(channelManager.ActiveChannel.ChannelID));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnManualDial_Click()", "MyDialer.xaml.cs");
            }
        }

        void btnAutomaticDial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAutomaticDial.Content.ToString() == "Start Automatic Dialing")
                {
                    if (channelManager.CurrentCampaingID == 0)
                    {
                        lblMessage.Content = "No Campaing available";
                        //MessageBox.Show("No Campaing available");
                    }
                    else if (channelManager.LeadCollection == null || channelManager.LeadCollection.Count==0)
                    {
                        lblMessage.Content = "No Fresh Leads available";
                        //MessageBox.Show("No Fresh Leads available");
                    }
                    else
                    {
                        channelManager.ActiveChannel.CurrentDialStatus = "Automatic";
                        channelManager.ActiveChannel.CountryCode = channelManager.GetCountryCode();
                        channelManager.ActiveChannel.CampaginPrefix = channelManager.GetCampaginPrefix();                        
                        btnAutomaticDial.Content = "Stop Automatic Dialing";
                        btnManualDial.IsEnabled = false;
                        if (channelManager.LeadCollection.Count != 0)
                        {
                            VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(true, "AutoMatic"));
                        }
                        FireCall();
                        channelManager.StartSyncProcess(true);
                    }
                }
                else if (btnAutomaticDial.Content.ToString() == "Stop Automatic Dialing")
                {

                    btnAutomaticDial.Content = "Start Automatic Dialing";
                    btnManualDial.IsEnabled = true;
                    //channel1.CallResult = (ClsChannel.CallStatus)CallStatus.CallHangUp;
                    VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(false, "AutoMatic"));
                    //VMuktiHelper.CallEvent("HangUp", this, new VMuktiEventArgs(channel1.ChannelID,channel1.CallResult.ToString()));
                    cnvMain.IsEnabled = false;
                    FncRemoveDialLead();
                    channelManager.StartSyncProcess(false);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnAutomaticDial_Click()", "MyDialer.xaml.cs");
            }
        }

        public void FncRemoveDialLead()
        {
            try
            {
                // if (CallResult == channel1.CallResult)
                {
                    channelManager.LeadCollection.RemoveAt(0);
                }
                if (channelManager.LeadCollection.Count == 0)
                {
                    //objLeadCollection = ClsLeadCollection.GetAll(long.Parse(txtID.Text.ToString()));
                    channelManager.GetNextLeadList(false);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncRemoveDialLead()", "MyDialer.xaml.cs");
            }
        }
         
        public void FireCall()
        {
            try
            {
                //lock (this)
                //{
                if (channelManager.LeadCollection.Count == 0)
                {
                    lblMessage.Content = "Leads Over";
                    //MessageBox.Show("Leads Over");
                    btnAutomaticDial.Content = "Start Automatic Dialing";
                    btnManualDial.IsEnabled = true;
                }
                else
                {
                    // CallResult = CallStatus.CallHangUp;
                    VMuktiHelper.CallEvent("SetLeadIDCRM", this, new VMuktiEventArgs(channelManager.LeadCollection[0].ID));
                    //VMuktiHelper.CallEvent("SetLeadIDScript", this, new VMuktiEventArgs(channelManager.LeadCollection[0].ID));
                    VMuktiHelper.CallEvent("SetLeadIDScript", this, new VMuktiEventArgs(channelManager.LeadCollection[0].ID, channelManager.LeadCollection[0].ID, "Automatic"));
                    channelManager.ActiveChannel.CurrentPhoneNo = channelManager.LeadCollection[0].PhoneNo;
                    channelManager.ActiveChannel.CurrentCampainID = channelManager.CurrentCampaingID;
                    channelManager.ActiveChannel.LeadID = channelManager.LeadCollection[0].ID;
                    channelManager.ActiveChannel.ChannelID = "1";

                    channelManager.ActiveChannel.UserID = channelManager.UserID;
                    channelManager.ActiveChannel.ConfID = long.Parse("1");
                    channelManager.ActiveChannel.IsDNC = false;
                    channelManager.ActiveChannel.IsGlobal = false;
                    // channelManager.ActiveChannel.DispositionID = long.Parse("1");
                    channelManager.ActiveChannel.CallNote = "Hello";
                    //MessageBox.Show(objLeadCollection[0].PhoneNo.ToString());
                    VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.ReadyState));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FireCall()", "MyDialer.xaml.cs");
            }
            //}
        }
    }
}
