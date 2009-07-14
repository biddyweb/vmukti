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
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using AutoProgressivePhone.Business;
using System.Collections;
using VMuktiAPI;
using System.ServiceModel;
using System.Collections.Generic;
using AutoProgressivePhone.Business.Service.NetP2P;
using AutoProgressivePhone.Business.Service.BasicHttp;

using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Reflection;
using System.Globalization;
using System.Runtime.Remoting;
using VMuktiService;

namespace AutoProgressivePhone.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlDialer : UserControl
    {
        RTCClient RClient = null;
        string strNumber = "";
        string strSIPServer = "";
        string strBtnChClick = "0";
        string strLastNumber = "1110";
        string strLastConfNumber = "860000";
        int intChannelNumber = 1;
        Button[] btnChannels = null;
        string strCallingType = "AutoMatic";
        System.Timers.Timer t1;
        float HourCounter, MinCounter, SecondCounter = 00;
        Hashtable TimeCounterCollection = new Hashtable();
        bool blIsOnHold = false;
        bool blShiftPress = false;
        bool blIsVista;
        bool blStopAutoDialing = false;
        string DetectedTone = "Human";
        int ToneDetectedChannel = 0;
        public string myStatus = string.Empty;
        System.Diagnostics.Process ProcStartAudioVistaExe = null;

        object objNetP2PRTCVista = null;
        INetTcpRTCVistaServiceChannel ClientNetP2PRTCVistaChannel;
        VMuktiService.NetPeerClient npcRTCVistaClient = null;

        private delegate void TimeChangeDelegate(float Secondes);
        ModulePermissions[] _MyPermissions;

        #region Declaring variables for Net.Tcp Clients.
        System.Threading.Thread ThOpenClinet = null;
        public static IService SNChannel = null;

        System.Windows.Threading.DispatcherTimer dtGetConference = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        object objNetTcpAudio = new clsNetTcpAudio();
        INetTcpAudioChannel NetP2PChannel;
        int temp = 0;
        public string strUri = string.Empty;
        object objHttpAudio = new clsHttpAudio();
        IHttpAudio Httpchannel;
        int tempcounter = 0;

        /// <summary>
        ///For Active Call Reports, variable uses as activeagent 
        /// </summary>
        string strUriforActiveagent;
        string strSetNumberForActiveCallReport = string.Empty;
        int intTransferConfNumber = 1;
        bool isHangUpAdmin = false;
        System.Threading.Thread thostActiveAgent = null;
        object objNetTcpActiveAgent = new NetP2PBootStrapActiveAgentReportDelegates();
        INetP2PBootStrapActiveAgentReportChannel channelNetTcp = null;
        #endregion

        VMuktiAPI.PeerType objPeerType;
        public delegate void DelDisplayName(List<string> lstUserName);
        public DelDisplayName objDelDisplayName;

		 public delegate void DelHangupAdmin(string phNumber);
        public DelHangupAdmin objHangupAdmin;

        public delegate void DelBargeReqest(string phNumber);
        public DelBargeReqest objBargeRequest;


        public enum CallStatus
        {
            NotInCall, CallInProgress, DialingToDest, RingingToDest,
            BusyTone, AMD, DTMF, DestUserStratListen, CallHangUp, CallHoldByAgent,
            CallHoldBySys, CallDispose, ReadyState
        };


        public ctlDialer(VMuktiAPI.PeerType objLocalPeerType, string uri, ModulePermissions[] MyPermissions,string Role)
        {
            InitializeComponent();
             System.OperatingSystem osInfo = System.Environment.OSVersion;
             if (osInfo.Version.Major.ToString() == "6")
             {
                 blIsVista = true;
                 ProcStartAudioVistaExe = new System.Diagnostics.Process();
                 ProcStartAudioVistaExe.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\VistaAudio";
                 ProcStartAudioVistaExe.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                 ProcStartAudioVistaExe.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\VistaAudio\VMuktiAudio.VistaService.exe";
                 ProcStartAudioVistaExe.Start();
             }
             else
             {
                 blIsVista = false;
             }
            objPeerType = objLocalPeerType;
            _MyPermissions = MyPermissions;
            FncPermissionsReview();
            try
            {
                btnPad.Click += new RoutedEventHandler(btnPad_Click);
                btnReject.Click += new RoutedEventHandler(btnReject_Click);
                btnAccept.Click += new RoutedEventHandler(btnAccept_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                btnTransfer.Click += new RoutedEventHandler(btnTransfer_Click);
                btnDTMF.Click += new RoutedEventHandler(btnDTMF_Click);
                btnCall.Click += new RoutedEventHandler(btnCall_Click);
                btnHangup.Click += new RoutedEventHandler(btnHangup_Click);
                btnHold.Click += new RoutedEventHandler(btnHold_Click);
                btnConference.Click += new RoutedEventHandler(btnConference_Click);
                btnChannels = new Button[6];

                int btnLeft = 10;
                int btnTop = 60;
                for (int i = 0; i < 6; i++)
                {
                    (btnChannels[i]) = new Button();
                    btnChannels[i].Height = 20;
                    btnChannels[i].Width = 34;
                    btnChannels[i].Content = (i + 1).ToString();
                    btnChannels[i].Name = "btnCh" + (i + 1).ToString();
                    btnChannels[i].FontSize = 11;
                    btnChannels[i].BorderThickness = new Thickness(0, 0, 0, 0);
                    btnChannels[i].Margin = new Thickness(btnLeft, btnTop, 0, 0);
                    btnLeft = btnLeft + 39;
                    btnChannels[i].Click += new RoutedEventHandler(ChClick);
                    btnChannels[i].Tag = "Free";
                    btnChannels[i].IsEnabled = false;
                    CnvPhoneProperty.Children.Add(btnChannels[i]);
                    TimeCounterCollection.Add(i, "0:00:00");
                }
                VMuktiHelper.RegisterEvent("RegisterAgent").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(FncRegister);
                VMuktiHelper.RegisterEvent("Dial").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventDial);
                VMuktiHelper.RegisterEvent("HangUp").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventHangUp);
                VMuktiHelper.RegisterEvent("StartManualDialing").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventStartManualDialing);
                VMuktiHelper.RegisterEvent("SetSoftPhoneEnable").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEvent_SetSoftPhoneEnable);
                VMuktiHelper.RegisterEvent("CallHangUPFromRender").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEvent);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventSignOut);
                t1 = new System.Timers.Timer(1000);
                t1.Elapsed += new System.Timers.ElapsedEventHandler(t1_Elapsed);
                btnHold.IsEnabled = false;
                btnTransfer.IsEnabled = false;
                objDelDisplayName = new DelDisplayName(DisplayName);
                objHangupAdmin = new DelHangupAdmin(HangupAdmin);
                objBargeRequest = new DelBargeReqest(BargeRequest);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);
                //FncRegister(null, null);

                if (blIsVista)
                {
                    System.Threading.Thread.Sleep(30000);
                    npcRTCVistaClient = new VMuktiService.NetPeerClient();
                    objNetP2PRTCVista = new clsNetTcpRTCVistaService();
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcJoin += new clsNetTcpRTCVistaService.DelsvcJoin(ctlDialer_entsvcJoin);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcCreateRTCClient += new clsNetTcpRTCVistaService.DelsvcCreateRTCClient(ctlDialer_entsvcCreateRTCClient);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcRegisterSIPPhone += new clsNetTcpRTCVistaService.DelsvcRegisterSIPPhone(ctlDialer_entsvcRegisterSIPPhone);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcAnswer += new clsNetTcpRTCVistaService.DelsvcAnswer(ctlDialer_entsvcAnswer);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcDial += new clsNetTcpRTCVistaService.DelsvcDial(ctlDialer_entsvcDial);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcHangup += new clsNetTcpRTCVistaService.DelsvcHangup(ctlDialer_entsvcHangup);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcHold += new clsNetTcpRTCVistaService.DelsvcHold(ctlDialer_entsvcHold);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcTransfer += new clsNetTcpRTCVistaService.DelsvcTransfer(ctlDialer_entsvcTransfer);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcRTCEvent += new clsNetTcpRTCVistaService.DelsvcRTCEvent(ctlDialer_entsvcRTCEvent);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcUnJoin += new clsNetTcpRTCVistaService.DelsvcUnJoin(ctlDialer_entsvcUnJoin);
                    ClientNetP2PRTCVistaChannel = (INetTcpRTCVistaServiceChannel)npcRTCVistaClient.OpenClient<INetTcpRTCVistaServiceChannel>("net.tcp://localhost:6060/NetP2PRTCVista", "NetP2PRTCVistaMesh", ref objNetP2PRTCVista);
                    ClientNetP2PRTCVistaChannel.svcJoin();
                }


                ThOpenClinet = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FncOpenClinet));
                System.Collections.Generic.List<object> lstParameters = new System.Collections.Generic.List<object>();
                lstParameters.Add(objLocalPeerType);
                lstParameters.Add(uri);
                ThOpenClinet.Start(lstParameters);

                thostActiveAgent = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostActiveAgentService));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveAgentReport");
                lstParams.Add("P2PActiveAgentMesh");
                thostActiveAgent.Start(lstParams);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer()", "ctlDialer.xaml.cs");
            }
        }

       

        public ctlDialer(string agentNumber, string agentPass, string SIPServer)
        { }

        #region Button Click Events
        void btnPad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnPad.Content.ToString() == "<")
                {
                    cnvNumbers.Visibility = Visibility.Visible;
                    btnPad.Content = ">";
                }
                else if (btnPad.Content.ToString() == ">")
                {
                    cnvNumbers.Visibility = Visibility.Hidden;
                    btnPad.Content = "<";
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnPad_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcAnswer();
                }
                else
                {
                    RClient.Anser();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAccept_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcHangup(0);
                }
                else
                {
                    RClient.HangUp(0);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnReject_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblNumber.Text.ToString() != "Enter Number" && lblNumber.Text.ToString() != string.Empty)
                {
                    lblNumber.Text = lblNumber.Text.ToString().Substring(0, lblNumber.Text.ToString().Length - 1);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblNumber.Text.ToString() == "")
                {
                    lblNumber.Text = "Enter Number";
                }

                else
                {
                    if (strCallingType == "Manual")
                    {
                        btnChannels[intChannelNumber - 1].Tag = "Free";
                        btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                        string s = "###*3" + lblNumber.Text.ToString() + "#";
                        char[] ch = s.ToCharArray();
                        for (int i = 0; i < ch.Length; i++)
                        {
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcSendDTMF(ch[i].ToString(), intChannelNumber);
                                System.Threading.Thread.Sleep(100);
                            }
                            else
                            {
                                RClient.SendDTMF(ch[i].ToString(), intChannelNumber);
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                    else if (strCallingType == "AutoMatic")
                    {
                        //FncTransfer(lblNumber.Content.ToString(), long.Parse("1"));
                        btnChannels[intChannelNumber - 1].Tag = "Free";
                        btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                        string s = "###*3" + lblNumber.Text.ToString() + "#";
                        char[] ch = s.ToCharArray();
                        for (int i = 0; i < ch.Length; i++)
                        {
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcSendDTMF(ch[i].ToString(), intChannelNumber);                                
                            }
                            else
                            {
                                RClient.SendDTMF(ch[i].ToString(), intChannelNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnTransfer_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnDTMF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblNumber.Text.ToString() != "")
                {
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcSendDTMF(lblNumber.Text.ToString(), intChannelNumber);
                    }
                    else
                    {
                        RClient.SendDTMF(lblNumber.Text.ToString(), intChannelNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDTMF_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnCall.Tag.ToString() == "Free" && strCallingType=="Manual")
                {
                    if (lblNumber.Text.ToString() == string.Empty || lblNumber.Text.ToString() == "Enter Number")
                    {
                        lblNumber.Text = "Enter Number";
                    }
                    else
                    {
                        bool isFreeChannelAvail = false;
                        for (int i = 0; i < btnChannels.Length; i++)
                        {
                            if (btnChannels[i].Tag.ToString() == "Free")
                            {
                                ChClick(btnChannels[i], null);
                                btnChannels[Convert.ToInt16(strBtnChClick)].Tag = "Running";
                                //btnChannels[Convert.ToInt16(strBtnChClick)].Background = (Brush)this.Resources["GreenBaseBrush"];
                                VMuktiHelper.CallEvent("SetChannelValues", this, new VMuktiEventArgs(lblNumber.Text.ToString(), DateTime.Now, DateTime.Now.ToString()));
                                FncCall(lblNumber.Text.ToString().Trim(), long.Parse(intChannelNumber.ToString()));
                                isFreeChannelAvail = true;
                                btnHangup.IsEnabled = true;
                                break;
                            }
                        }
                        if (!isFreeChannelAvail)
                        {
                            MessageBox.Show("No Free Channels Available");
                        }
                    }
                }
                else if (btnCall.Tag.ToString() == "Running" && strCallingType=="Manual")
                {
                    btnHangup_Click(null, null);
                }
                else if (btnCall.Tag.ToString() == "Running" && strCallingType == "AutoMatic")
                {
                    btnHangup_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCall_click()", "ctlDialer.xaml.cs");
            }
        }

        void btnHangup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnChannels[intChannelNumber - 1].Tag = "Free";
                btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcHangup(Convert.ToInt32(intChannelNumber.ToString()));
                }
                else
                {
                    RClient.HangUp(Convert.ToInt32(intChannelNumber.ToString()));
                }
                //FncHangUp(long.Parse(intChannelNumber.ToString()));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnHangup_Click()", "ctlDialer.xaml.cs");
            }
        }

        void btnHold_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strCallingType == "Manual")
                {
                    btnChannels[intChannelNumber - 1].Tag = "Hold";
                    //btnChannels[intChannelNumber - 1].Background = (Brush)this.Resources["RedBaseBrush"];
                    FncHold(long.Parse(intChannelNumber.ToString()));
                    btnHold.IsEnabled = false;
                }
                else if (strCallingType == "AutoMatic")
                {
                    FncHold(long.Parse("1"));
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnHold_Click()", "ctlDialer.xaml.cs");
            }
        }

        private void btnConference_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] strBuddyList = new string[2];
                string strConfNumber = "";
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    strConfNumber = SNChannel.svcGetConferenceNumber();
                }
                else
                {
                    strConfNumber = fncGetConferenceNumber();
                }

                strConfNumber = strConfNumber + "@" + strSIPServer;
                bool isFreeChannelAvail = false;
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Free")
                    {
                        ChClick(btnChannels[i], null);
                        btnChannels[Convert.ToInt16(strBtnChClick)].Tag = "Running";
                        //VMuktiHelper.CallEvent("SetChannelValues", this, new VMuktiEventArgs(lblNumber.Content.ToString(), DateTime.Now, DateTime.Now.ToString()));
                        FncCall(strConfNumber, long.Parse(intChannelNumber.ToString()));
                        isFreeChannelAvail = true;
                        btnHangup.IsEnabled = true;
                        break;
                    }
                }
                if (objPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || objPeerType == VMuktiAPI.PeerType.BootStrap || objPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    if (NetP2PChannel != null)
                    {
                        NetP2PChannel.svcP2PStartConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strConfNumber, strBuddyList);
                    }
                }
                else
                {
                    if (Httpchannel != null)
                    {
                        Httpchannel.svcStartConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strConfNumber, strBuddyList);
                    }
                }

                if (!isFreeChannelAvail)
                {
                    MessageBox.Show("No Free Channels Available");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnConference_Click()", "ctlDialer.xaml.cs");
            }
        }

        private void NumClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                try
                {
                    string strWaveFilePath = System.Reflection.Assembly.GetAssembly(typeof(ctlDialer)).Location.Replace("Audio.Presentation.dll", "ding.wav");
                    System.Media.SoundPlayer SoundPalyer = new System.Media.SoundPlayer(strWaveFilePath);
                    SoundPalyer.Play();
                }
                catch
                { }

                if (lblNumber.Text.ToString().StartsWith("Enter Number"))
                {
                    lblNumber.Text = "";
                }

                lblNumber.Text = lblNumber.Text.ToString() + ((Button)sender).Content.ToString();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NumClick()", "ctlDialer.xaml.cs");
            }
        }

        private void ChClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                strBtnChClick = (Convert.ToInt16(((Button)sender).Content.ToString()) - 1).ToString();
                intChannelNumber = (Convert.ToInt16(((Button)sender).Content.ToString()));
                if (((Button)sender).Tag.ToString() == "Free")
                {
                    btnHold.IsEnabled = false;
                    btnTransfer.IsEnabled = false;
                    btnHangup.IsEnabled = false;
                    for (int i = 0; i < 6; i++)
                    {
                        if (btnChannels[i].Tag.ToString() == "Running")
                        {
                            btnChannels[i].Tag = "Hold";
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(i + 1), "Hold");
                            }
                            else
                            {
                                RClient.Hold(Convert.ToInt16(i + 1), "Hold");
                            }
                            // btnChannels[i].Background = (Brush)this.Resources["RedBaseBrush"];

                            lblTime.Content = "0:00:00";
                            t1.Stop();
                            SecondCounter = 0;
                            MinCounter = 0;
                            HourCounter = 0;
                        }
                    }
                }
                else if (((Button)sender).Tag.ToString() == "Running")
                {

                    ((Button)sender).Tag = "Hold";
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcHold(intChannelNumber, "Hold");
                    }
                    else
                    {
                        RClient.Hold(Convert.ToInt16(intChannelNumber), "Hold");
                    }
                    //((Button)sender).Background = (Brush)this.Resources["RedBaseBrush"];
                    btnHold.IsEnabled = false;
                }
                else if (((Button)sender).Tag.ToString() == "Hold")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (btnChannels[i].Tag.ToString() == "Running" && btnChannels[i].Content != ((Button)sender).Content.ToString())
                        {
                            btnChannels[i].Tag = "Hold";
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(i + 1), "Hold");
                            }
                            else
                            {
                                RClient.Hold(Convert.ToInt16(i + 1), "Hold");
                            }
                            // btnChannels[i].Background = (Brush)this.Resources["RedBaseBrush"];
                        }
                    }

                    TimeSpan CallDurationTime = DateTime.Now - DateTime.Parse(TimeCounterCollection[intChannelNumber - 1].ToString());

                    t1.Start();
                    SecondCounter = CallDurationTime.Seconds;
                    MinCounter = CallDurationTime.Minutes;
                    HourCounter = CallDurationTime.Hours;


                    ((Button)sender).Tag = "Running";
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcHold(intChannelNumber, "UnHold");
                    }
                    else
                    {
                        RClient.Hold(Convert.ToInt16(intChannelNumber), "UnHold");
                    }
                    btnHold.IsEnabled = true;
                    // ((Button)sender).Background = (Brush)this.Resources["GreenBaseBrush"];
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "chClick()", "ctlDialer.xaml.cs");
            }
        }
        #endregion

        #region VMuktiAPI Events

        void ctlDialer_VMuktiEventDial(object sender, VMuktiEventArgs e)
        {
            try
            {
                FncCall(e._args[0].ToString(), long.Parse(e._args[1].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEventDial()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_VMuktiEventHangUp(object sender, VMuktiEventArgs e)
        {
            try
            {
                FncHangUp(long.Parse(e._args[0].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEventHangUp()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_VMuktiEventStartManualDialing(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (e._args[1].ToString() == "AutoMatic")
                {
                    if (e._args[0].ToString().ToLower() == "false")
                    {
                        blStopAutoDialing = true;
                    }
                    else
                    {
                        blStopAutoDialing = false;
                        CnvDialer.IsEnabled = Convert.ToBoolean(e._args[0]);
                        btnCall.IsEnabled = false;
                        btnHangup.IsEnabled = true;
                    }
                   
                }

                else if (e._args[1].ToString() == "Manual")
                {
                    CnvDialer.IsEnabled = Convert.ToBoolean(e._args[0]);
                    btnCall.IsEnabled = Convert.ToBoolean(e._args[0]);
                    btnHangup.IsEnabled = false;
                    for (int i = 0; i < 6; i++)
                    {
                        btnChannels[i].IsEnabled = Convert.ToBoolean(e._args[0]);
                    }
                }
                strCallingType = e._args[1].ToString();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEventStartManualDialing()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_VMuktiEvent_SetSoftPhoneEnable(object sender, VMuktiEventArgs e)
        {
            try
            {
                CnvDialer.IsEnabled = bool.Parse(e._args[0].ToString());
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEvent_SetSoftPhoneEnable()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_VMuktiEventSignOut(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("RegisterAgent");
                VMuktiHelper.UnRegisterEvent("Dial");
                VMuktiHelper.UnRegisterEvent("HangUp");
                VMuktiHelper.UnRegisterEvent("StartManualDialing");
                VMuktiHelper.UnRegisterEvent("SetSoftPhoneEnable");
                VMuktiHelper.UnRegisterEvent("CallHangUPFromRender");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEventSignOut()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                btnChannels[intChannelNumber - 1].Tag = "Free";
                btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                RClient.HangUp(Convert.ToInt32(intChannelNumber.ToString()));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEvent()", "ctlDialer.xaml.cs");
            }
        }

        private delegate void HumanDetectedDelegate(int channel, string tone);

        public void FncRegisterfromEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcRegisterSIPPhone(e._args[0].ToString(), e._args[1].ToString(), e._args[2].ToString());
                }
                else
                {
                    RClient = new RTCClient(e._args[0].ToString(), e._args[1].ToString(), e._args[2].ToString());
                    RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
                }
                lblUserNumber.Content = e._args[0].ToString() + "@" + e._args[2].ToString();

                //RClient = new RTCClient("3000", "3000", "59.165.20.15");
                //lblUserName.Content=@"3000@59.165.20.15";
				RClient.NonHumanDetected += OnNonHumanDetected;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncRegisterfromEvent()", "ctlDialer.xaml.cs");
            }
        }

        public void FncRegister(object sender, VMuktiEventArgs e)
        {
            try
            {
                DataSet da = new DataSet();
                if (VMuktiAPI.VMuktiInfo.strExternalPBX == "true")
                {
                    try
                    {
                        //MessageBox.show("Sip Number is: " + e._args[0].ToString() + "and Sip server is: " + e._args[2].ToString());
                        if (blIsVista)
                        {
                            ClientNetP2PRTCVistaChannel.svcRegisterSIPPhone(e._args[0].ToString(), e._args[1].ToString(), e._args[2].ToString());
                        }
                        else
                        {
                            RClient = new RTCClient(e._args[0].ToString(), e._args[1].ToString(), e._args[2].ToString());
                            RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
                            RClient.NonHumanDetected += OnNonHumanDetected;
                        }
                        lblUserNumber.Content = e._args[0].ToString() + "@" + e._args[2].ToString();
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncRegister()--1", "ctlDialer.xaml.cs");
                    }
                }
                else
                {
                    //SNChannel = (IService)bhcSuperNode.OpenClient<IService>("http://210.211.254.132:80/SNService");
                    //string strSIPNumber = "2118";
                    ///string strSIPServerIP = "210.211.254.132";

                    string strSIPNumber = "";
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                    {
                        VMuktiService.BasicHttpClient bhcSuperNode = new VMuktiService.BasicHttpClient();
                        SNChannel = (IService)bhcSuperNode.OpenClient<IService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/SNService");
                        strSIPServer = VMuktiInfo.CurrentPeer.SuperNodeIP;
                        strSIPNumber = SNChannel.svcAddSIPUser();
                    }
                    else
                    {
                        strSIPNumber = FncAddSIPUser();
                        strSIPServer = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP.ToString();
                    }

                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcRegisterSIPPhone(strSIPNumber, strSIPNumber, strSIPServer);
                    }
                    else
                    {
                        RClient = new RTCClient(strSIPNumber, strSIPNumber, strSIPServer);
                        RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
                        RClient.NonHumanDetected += OnNonHumanDetected;
                    }
                    lblUserNumber.Content = strSIPNumber + "@" + strSIPServer;
                }
             
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncRegister()--2", "ctlDialer.xaml.cs");
            }
        }

        #endregion

        #region Local Functions for RTC
        private void HumanDetectedFun(int channel, string tone)
        {
            DetectedTone = tone;
            ToneDetectedChannel = channel;
            FncHangUp((long)channel);
        }

        private void OnNonHumanDetected(object sender, int channel, string tone)
        {
            Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new HumanDetectedDelegate(HumanDetectedFun), channel, tone);
        }

        public void FncCall(string number, long channelID)
        {
            try
            {
                strNumber = number.ToString();
                lblStatus_Copy.Content = "Dialing......" + strNumber;
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcDial(number, Convert.ToInt32(channelID));
                }
                else
                {
                    RClient.Dial(number.ToString(), Convert.ToInt32(channelID)); ;
                }

                //txtStatus.Focus();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCall()", "ctlDialer.xaml.cs");
            }
        }

        public void FncHangUp(long channelID)
        {
            try
            {
                if (strCallingType == "Manual")
                {
                    for (int i = 0; i < btnChannels.Length; i++)
                    {
                        if (btnChannels[i].Tag.ToString() == "Hold" || btnChannels[i].Tag.ToString() == "Running")
                        {
                            btnChannels[i].Tag = "Free";
                            btnChannels[i].Background = Brushes.Transparent;
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcHangup(i + 1);
                            }
                            else
                            {
                                RClient.HangUp(i + 1);
                            }
                        }
                    }
                }
                else if (strCallingType == "AutoMatic" && (lblStatus_Copy.Content.ToString() == "OnHold" || lblStatus_Copy.Content.ToString().StartsWith("Connected") || lblStatus_Copy.Content.ToString().StartsWith("Dialing") || lblStatus_Copy.Content.ToString().StartsWith("Disconnected")))
                {
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcHangup(Convert.ToInt32(channelID));
                    }
                    else
                    {
                        RClient.HangUp(Convert.ToInt32(channelID));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncHangUp()", "ctlDialer.xaml.cs");
            }
        }

        public void FncHold(long channelID)
        {
            try
            {
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt32(channelID), btnHold.Content.ToString());
                }
                else
                {
                    RClient.Hold(Convert.ToInt32(channelID), btnHold.Content.ToString());
                }
                if (btnHold.Content.ToString() == "Hold" && strCallingType == "AutoMatic")
                {
                    btnHold.Content = "Unhold";
                }
                else
                {
                    btnHold.Content = "Hold";
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncHold()", "ctlDialer.xaml.cs");
            }
        }

        public void FncTransfer(string Number, long channelID)
        {
            try
            {
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcTransfer(Number, Convert.ToInt16(channelID));
                }
                else
                {
                    RClient.Transfer(Number, Convert.ToInt16(channelID));
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncTransfer()", "ctlDialer.xaml.cs");
            }
        }

        public void FncBarge(string Number, long channelID)
        {
            try
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcSendDTMF(Number[j].ToString(), Convert.ToInt16(channelID));
                    }
                    else
                    {
                        RClient.SendDTMF(Number[j].ToString(), Convert.ToInt16(channelID));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncBarge()", "ctlDialer.xaml.cs");
            }
        }
        #endregion

        void RClient_entstatus(int ChannelId, string status)
        {
            try
            {
                switch (status)
                {
                    case "InPorgress":
                        btnCall.Tag = "Running";
                        if (strCallingType == "Predictive")
                        {
                            VMuktiHelper.CallEvent("SetPredictiveChannelStatus", this, new VMuktiEventArgs(CallStatus.RingingToDest, ChannelId));
                        }
                        else
                        {
                            VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.RingingToDest, ChannelId));
                        }
                        myStatus = "InProgrss";
                        strSetNumberForActiveCallReport = strNumber;
                        //channelNetTcpCall.svcSetDuration(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Inprogress", "00:00:00", strNumber);
                        channelNetTcp.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "InProgress", strNumber, "00:00:00",false);
                        break;

                    case "Connected":
                        btnCall.Tag = "Running";
                        lblNumber.Text = "";
                        lblStatus_Copy.Content = "Connected " + strNumber;
                        myStatus = "Connected";
                        strSetNumberForActiveCallReport = strNumber;
                        btnHold.IsEnabled = true;
                        btnHangup.IsEnabled = true;
                        btnTransfer.IsEnabled = true;
                        //if (!blIsOnHold)
                        if (intChannelNumber == (ChannelId))
                        {
                            VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.CallInProgress, ChannelId));
                        }
                        t1.Start();
                        if (TimeCounterCollection[intChannelNumber - 1].ToString() == "0:00:00")
                        {
                            TimeCounterCollection[intChannelNumber - 1] = DateTime.Now;
                        }
                        VMuktiHelper.CallEvent("SetDispositionEnable", this, new VMuktiEventArgs(false, strNumber, (ChannelId - 1), strCallingType));
                        channelNetTcp.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Connected", strNumber, "Calculating...",false);
                        //channelNetTcpCall.svcSetDuration(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Connected", "Calculating...", strNumber);
                        break;

                    case "Disconnected":
                        lock (this)
                        {
                            btnCall.Tag = "Free";
                            if (strCallingType == "AutoMatic")
                            {
                                if (DetectedTone != "Human" && ChannelId == ToneDetectedChannel)
                                {
                                    VMuktiHelper.CallEvent("SetDispositiForDetectedTone", this, new VMuktiEventArgs(DetectedTone, strNumber, (ChannelId - 1), strCallingType));
                                    //VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(false));
                                    DetectedTone = "Human";
                                    ToneDetectedChannel = 0;
                                }
                                else
                                {
                                    VMuktiHelper.CallEvent("SetDispositionEnable", this, new VMuktiEventArgs(true, strNumber, (ChannelId - 1), strCallingType));
                                    VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(false));
                                    //VMuktiHelper.CallEvent("SetSoftPhoneEnable", this, new VMuktiEventArgs(false));
                                    CnvDialer.IsEnabled = false;
                                    //CnvDialer.IsEnabled = true;
                                    //VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs("7", "Answering", false, "", "1"));
                                    //VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(true));
                                }

                            }
                            lblNumber.Text = "";
                            lblStatus_Copy.Content = "Disconnected";
                            btnHold.IsEnabled = false;
                            if (strCallingType != "AutoMatic")
                            {
                                btnHangup.IsEnabled = false;
                            }
                            btnTransfer.IsEnabled = false;
                            //                    VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.CallHangUp));
                            btnChannels[ChannelId - 1].Tag = "Free";
                            btnChannels[ChannelId - 1].Background = Brushes.Transparent;
                            TimeCounterCollection[ChannelId - 1] = "0:00:00";
                            if (intChannelNumber != (ChannelId - 1))
                            {
                                t1.Stop();
                                HourCounter = 0;
                                MinCounter = 0;
                                SecondCounter = 0;
                                //  lblTime.Content = "0:00:00";
                            }
                            if (strCallingType == "Manual" && intChannelNumber == (ChannelId - 1))
                            {
                                VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.CallHangUp, (ChannelId - 1)));
                            }
                            else if (strCallingType == "AutoMatic")
                            {
                                VMuktiHelper.CallEvent("SetChannelStatus", this, new VMuktiEventArgs(CallStatus.CallHangUp, ChannelId));
                            }
                            else if (strCallingType == "Predictive")
                            {
                                VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallHangUp, ChannelId));
                            }
                        }
                        myStatus = "DisConnected";
                        strSetNumberForActiveCallReport = strNumber;
                        channelNetTcp.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Disconnected", strNumber, lblTime.Content.ToString(),false);
                        if (blStopAutoDialing)
                        {
                            channelNetTcp.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Stopped", strNumber, lblTime.Content.ToString(), false);
                        }
                        //channelNetTcpCall.svcSetDuration(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "DisConnected", lblTime.Content.ToString(), strNumber);
                        break;

                    case "Incoming":
                        btnCall.Tag = "Incoming";
                        lblStatus_Copy.Content = "Incoming";
                        myStatus = "Incoming";
                        strSetNumberForActiveCallReport = strNumber;
                        channelNetTcp.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Incoming", strNumber, "00:00:00",false);
                        break;

                    case "Hold":
                        lblStatus_Copy.Content = "OnHold";
                        blIsOnHold = true;
                        strSetNumberForActiveCallReport = strNumber;
                        myStatus = "Hold";
                        channelNetTcp.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "Hold", strNumber, "Calculating...",false);
                        break;

                    case "Registered":
                        break;

                    case "Rejected":
                        lblUserNumber.Content = "Rejected";
                        break;

                    case "RegistrationError":
                        lblUserNumber.Content = "RegistrationError";
                        break;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RClient_entstatus()", "ctlDialer.xaml.cs");
            }
        }

        #region Other Functions used in Code
        void lstItem_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.DragDrop.DoDragDrop((DependencyObject)((ListBoxItem)sender), ((ListBoxItem)sender), DragDropEffects.Copy);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "lstItem_PreviewMouseDown()", "ctlDialer.xaml.cs");
            }
        }

        void CnvMain_PreviewDrop(object sender, DragEventArgs e)
        { }

        public void BuddySelected(List<string> uname)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uname);
        }

        public void DisplayName(List<string> lstUserName)
        {
            try
            {
                string strLocalNumber = lstUserName[2].ToString();
                bool isFreeChannelAvail = false;
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Free")
                    {
                        ChClick(btnChannels[i], null);
                        btnChannels[Convert.ToInt16(strBtnChClick)].Tag = "Running";
                        //VMuktiHelper.CallEvent("SetChannelValues", this, new VMuktiEventArgs(lblNumber.Content.ToString(), DateTime.Now, DateTime.Now.ToString()));
                        FncCall(strLocalNumber.Trim().ToString(), long.Parse(intChannelNumber.ToString()));
                        isFreeChannelAvail = true;
                        btnHangup.IsEnabled = true;
                        break;
                    }
                }
                if (!isFreeChannelAvail)
                {
                    MessageBox.Show("No Free Channels Available");
                }               
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DisplayName()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                try
                {
                    string strWaveFilePath = System.Reflection.Assembly.GetAssembly(typeof(ctlDialer)).Location.Replace("Audio.Presentation.dll", "ding.wav");
                    System.Media.SoundPlayer SoundPalyer = new System.Media.SoundPlayer(strWaveFilePath);
                    SoundPalyer.Play();
                }
                catch
                { }

                if (lblNumber.Text.ToString().StartsWith("Enter Number"))
                { lblNumber.Text = ""; }

                if (e.Key == System.Windows.Input.Key.RightShift || e.Key == System.Windows.Input.Key.LeftShift)
                { blShiftPress = true; }

                if ((e.Key >= System.Windows.Input.Key.D0 && e.Key <= System.Windows.Input.Key.D9) || (e.Key >= System.Windows.Input.Key.NumPad0 && e.Key <= System.Windows.Input.Key.NumPad9))
                {
                    #region Switch Case
                    string KeyValue = "";
                    switch (e.Key.ToString())
                    {
                        case "NumPad1":
                        case "D1":
                            {
                                KeyValue = "1";
                                break;
                            }
                        case "NumPad2":
                        case "D2":
                            {
                                KeyValue = "2";
                                break;
                            }

                        case "NumPad3":
                        case "D3":
                            {
                                if (blShiftPress == true)
                                {
                                    KeyValue = "#";
                                    blShiftPress = false;
                                }
                                else
                                {
                                    KeyValue = "3";
                                }
                                break;
                            }

                        case "NumPad4":
                        case "D4":
                            {
                                KeyValue = "4";
                                break;
                            }

                        case "NumPad5":
                        case "D5":
                            {
                                KeyValue = "5";
                                break;
                            }

                        case "NumPad6":
                        case "D6":
                            {
                                KeyValue = "6";
                                break;
                            }

                        case "NumPad7":
                        case "D7":
                            {
                                KeyValue = "7";
                                break;
                            }

                        case "NumPad8":
                        case "D8":
                            {
                                if (blShiftPress == true)
                                {
                                    KeyValue = "*";
                                    blShiftPress = false;
                                }
                                else
                                {
                                    KeyValue = "8";
                                }
                                break;
                            }

                        case "NumPad9":
                        case "D9":
                            {
                                KeyValue = "9";
                                break;
                            }

                        case "NumPad0":
                        case "D0":
                            {
                                KeyValue = "0";
                                break;
                            }
                    }
                    #endregion

                    lblNumber.Text = lblNumber.Text.ToString() + KeyValue;
                }

                else if (e.Key == System.Windows.Input.Key.Multiply)
                { lblNumber.Text = lblNumber.Text.ToString() + "*"; }

                else if (e.Key == System.Windows.Input.Key.Enter)
                {
                    if (strCallingType == "Manual")
                    {
                        btnCall_Click(null, null);
                    }
                }

                else if (e.Key == System.Windows.Input.Key.Back)
                { btnCancel_Click(null, null); }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_PreviewKeyDown()", "ctlDialer.xaml.cs");
            }
        }

        string FncAddSIPUser()
        {
            try
            {
                int Number = int.Parse(strLastNumber);
                Number = Number + 1;
                strLastNumber = Number.ToString();
                return strLastNumber;
            }
            catch
            {
                return null;
            }
        }

        string fncGetConferenceNumber()
        {
            try
            {
                int ConfNumber = int.Parse(strLastConfNumber);
                ConfNumber = ConfNumber + 1;
                strLastConfNumber = ConfNumber.ToString();
                return strLastConfNumber;
            }
            catch
            {
                return null;
            }
        }

        void FncPermissionsReview()
        {
            try
            {
                this.Visibility = Visibility.Visible;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.View)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionReview()", "ctlDialer.xaml.cs");
            }
        }

        void t1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                SecondCounter += 1;
                TimeChangeDelegate TChange = new TimeChangeDelegate(ChangeTime);
                lblTime.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, TChange, SecondCounter);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "t1_Elapsed()", "ctlDialer.xaml.cs");
            }
        }

        void ChangeTime(float Seconds)
        {
            try
            {
                if (Seconds == 60)
                {
                    SecondCounter = 00;
                    MinCounter += 1;
                    if (MinCounter == 59)
                    {
                        MinCounter = 00;
                        HourCounter += 1;
                    }
                }
                if (Seconds < 10)
                {
                    if (MinCounter < 10)
                    { lblTime.Content = (HourCounter.ToString() + ":0" + MinCounter.ToString() + ":0" + SecondCounter.ToString()); }
                    else
                    { lblTime.Content = (HourCounter.ToString() + ":" + MinCounter.ToString() + ":0" + SecondCounter.ToString()); }
                }
                else
                {
                    if (MinCounter < 10)
                    { lblTime.Content = (HourCounter.ToString() + ":0" + MinCounter.ToString() + ":" + SecondCounter.ToString()); }
                    else
                    { lblTime.Content = (HourCounter.ToString() + ":" + MinCounter.ToString() + ":" + SecondCounter.ToString()); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ChangeTime()", "ctlDialer.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("RegisterAgent");
                VMuktiHelper.UnRegisterEvent("Dial");
                VMuktiHelper.UnRegisterEvent("HangUp");
                VMuktiHelper.UnRegisterEvent("StartManualDialing");
                VMuktiHelper.UnRegisterEvent("SetSoftPhoneEnable");
                VMuktiHelper.UnRegisterEvent("CallHangUPFromRender");
            
                if (channelNetTcp != null)
                {
                    channelNetTcp.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    channelNetTcp.Close();
                    channelNetTcp = null;
                }
                if (ClientNetP2PRTCVistaChannel != null)
                {
                    if (ProcStartAudioVistaExe != null)
                    {
                        ProcStartAudioVistaExe.Kill();
                        ProcStartAudioVistaExe.CloseMainWindow();
                        ProcStartAudioVistaExe.Close();
                        ProcStartAudioVistaExe.Dispose();
                        ProcStartAudioVistaExe = null;
                    }
                    ClientNetP2PRTCVistaChannel.svcUnJoin();
                    npcRTCVistaClient.CloseClient<INetTcpRTCVistaServiceChannel>();
                }
                if (ClientNetP2PRTCVistaChannel != null)
                {
                    ClientNetP2PRTCVistaChannel.Close();
                    ClientNetP2PRTCVistaChannel.Dispose();
                    ClientNetP2PRTCVistaChannel = null;
                }
                if (!blIsVista)
                {
                    try
                    {
                        RClient.StopSniffing();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlDialer.xaml.cs");
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("RegisterAgent");
                VMuktiHelper.UnRegisterEvent("Dial");
                VMuktiHelper.UnRegisterEvent("HangUp");
                VMuktiHelper.UnRegisterEvent("StartManualDialing");
                VMuktiHelper.UnRegisterEvent("SetSoftPhoneEnable");
                VMuktiHelper.UnRegisterEvent("CallHangUPFromRender");
                //SNChannel.svcRemoveSIPUser(lblUserNumber.Content.ToString().Split('@')[0].ToString());

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "ctlDialer.xaml.cs");
            }

        }

        #endregion

        #region Opening Basic P2P or Http Client
        private void FncOpenClinet(object lstParameters)
        {
            try
            {
                System.Collections.Generic.List<object> lstTempObj = (System.Collections.Generic.List<object>)lstParameters;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    VMuktiService.NetPeerClient ncpAudio = new VMuktiService.NetPeerClient();
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PJoin += new AutoProgressivePhone.Business.Service.NetP2P.clsNetTcpAudio.DelsvcP2PJoin(ctlDialer_EntsvcP2PJoin);
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PStartConference += new AutoProgressivePhone.Business.Service.NetP2P.clsNetTcpAudio.DelsvcP2PStartConference(ctlDialer_EntsvcP2PStartConference);
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PUnJoin += new AutoProgressivePhone.Business.Service.NetP2P.clsNetTcpAudio.DelsvcP2PUnJoin(ctlDialer_EntsvcP2PUnJoin);
                    NetP2PChannel = (INetTcpAudioChannel)ncpAudio.OpenClient<INetTcpAudioChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpAudio);

                    while (temp < 20)
        {
            try
            {
                            NetP2PChannel.svcP2PJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            temp = 20;
                        }
                        catch
                        {
                            temp++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    VMuktiService.BasicHttpClient bhcAudio = new VMuktiService.BasicHttpClient();
                    Httpchannel = (IHttpAudio)bhcAudio.OpenClient<IHttpAudio>(strUri);
                    while (tempcounter < 20)
        {
            try
            {
                            Httpchannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    dtGetConference.Interval = TimeSpan.FromSeconds(2);
                    dtGetConference.Tick += new EventHandler(dtGetConference_Tick);
                    dtGetConference.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncOpenClient()", "ctlDialer.xaml.cs");
            }
        }

        void dtGetConference_Tick(object sender, EventArgs e)
        {
            try
            {
                string ConfNumber = Httpchannel.svcGetConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        bool isFreeChannelAvail = false;
                        for (int i = 0; i < btnChannels.Length; i++)
                        {
                            if (btnChannels[i].Tag.ToString() == "Free")
                            {
                                ChClick(btnChannels[i], null);
                                btnChannels[Convert.ToInt16(strBtnChClick)].Tag = "Running";
                        //VMuktiHelper.CallEvent("SetChannelValues", this, new VMuktiEventArgs(lblNumber.Content.ToString(), DateTime.Now, DateTime.Now.ToString()));
                        FncCall(ConfNumber, long.Parse(intChannelNumber.ToString()));
                                isFreeChannelAvail = true;
                                btnHangup.IsEnabled = true;
                                break;
                            }
                        }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dtGetConference_Tick()", "ctlDialer.xaml.cs");
            }

        }

        /// Events of Basic Client
        void ctlDialer_EntsvcP2PJoin(string uName)
                { }

        void ctlDialer_EntsvcP2PStartConference(string uName, string strConfNumber, string[] GuestInfo)
        {
            try
            {
                if (GuestInfo.Contains(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName))
                {
                bool isFreeChannelAvail = false;
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Free")
                    {
                        ChClick(btnChannels[i], null);
                        btnChannels[Convert.ToInt16(strBtnChClick)].Tag = "Running";
                        //VMuktiHelper.CallEvent("SetChannelValues", this, new VMuktiEventArgs(lblNumber.Content.ToString(), DateTime.Now, DateTime.Now.ToString()));
                        FncCall(strConfNumber, long.Parse(intChannelNumber.ToString()));
                        isFreeChannelAvail = true;
                        btnHangup.IsEnabled = true;
                        break;
                    }
                }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcP2PStartConference()", "ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_EntsvcP2PUnJoin(string uName)
        { }
        #endregion

        #region Opening RptActive Agent Client
        public void hostActiveAgentService(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUriforActiveagent = lstTempObj[0].ToString();

                NetPeerClient npcActiveAgent = new NetPeerClient();
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcJoin(ctlrptActiveAgent_EntsvcJoin);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcGetAgentInfo += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcGetAgentInfo(ctlrptActiveAgent_EntsvcGetAgentInfo);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcSetAgentInfo += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcSetAgentInfo(ctlrptActiveAgent_EntsvcSetAgentInfo);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcSetAgentStatus += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcSetAgentStatus(ctlDialer_EntsvcSetAgentStatus);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcBargeRequest += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcBargeRequest(ctlDialer_EntsvcBargeRequest);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcBargeReply += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcBargeReply(ctlDialer_EntsvcBargeReply);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcHangUp += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcHangUp(ctlDialer_EntsvcHangUp);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcUnJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcUnJoin(ctlrptActiveAgent_EntsvcUnJoin);

                channelNetTcp = (INetP2PBootStrapActiveAgentReportChannel)npcActiveAgent.OpenClient<INetP2PBootStrapActiveAgentReportChannel>(strUriforActiveagent, lstTempObj[1].ToString(), ref objNetTcpActiveAgent);
                channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString());

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostActiveAgentService()", "ctlDialer.xaml.cs");
            }
        }

        /// WCF Events
        void ctlDialer_EntsvcJoined(string uName, string campaignId)
        {
            
        }

        void ctlrptActiveAgent_EntsvcJoin(string uName, string campaignId)
        {
           
        }

        void ctlrptActiveAgent_EntsvcGetAgentInfo(string uNameHost)
        {
            try
            {
                if (uNameHost != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    string status = "";
                    string Phone_No = "";
                    if (myStatus != "")
                    {
                        status = myStatus;
                    }
                    else
                    {
                        status = "ready";
                    }
                    if (strSetNumberForActiveCallReport != "")
                    {
                        Phone_No = strSetNumberForActiveCallReport;
                    }
                    else
                    {
                        Phone_No = "NotRegisterd";
                    }
					 string CallDuration = "00:00:00";                   
                     channelNetTcp.svcSetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString(),status,VMuktiAPI.VMuktiInfo.CurrentPeer.GroupName,Phone_No,CallDuration,false);                   
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptActiveAgent_EntsvcGetAgentInfo()", "ctlDialer.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcSetAgentInfo(string uName,string campaignId,string status,string groupName,string phoneNo,string CallDuration,bool isPredictive)
        {
        }

        void ctlDialer_EntsvcSetAgentStatus(string uName, string Status,string Phone_No,string CallDuration,bool isPredictive)
        {
        }

        void ctlDialer_EntsvcBargeRequest(string uName, string phoneNo)
        {
            try
            {
                // Fire conference on requested channel n return conf no to admin
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uName)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objBargeRequest, phoneNo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcBargeRequest()", "ctlDialer.xaml.cs");
            }
        }
        
        void BargeRequest(string phNumber)
        {
            try
            {
                if (btnCall.Tag.ToString() == "Running" && phNumber == strNumber)
                    {

                        FncBarge("###*7" + intTransferConfNumber.ToString() + "#", 1);

                        string strConf = "7" + intTransferConfNumber.ToString() + "@" + strSIPServer;

                        if (intTransferConfNumber > 10)
                        {
                            intTransferConfNumber = 0;
                        }
                        else
                        {
                            intTransferConfNumber += 1;
                        }

                        channelNetTcp.svcBargeReply(strConf);
                    }
                }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BargeRequest()", "ctlDialer.xaml.cs");
            }
        }
      
        void ctlDialer_EntsvcBargeReply(string confNo)
        {
            ///Do nothing here...
        }

        void ctlDialer_EntsvcHangUp(string uName, string phoneNo)
        {
            try
            {
                //Hang up d perticular call n apply auto disposition
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uName)
                {
                  this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objHangupAdmin, phoneNo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcHangUp()", "ctlDialer.xaml.cs");
            }
        }
        
        void HangupAdmin(string phNumber)
        {
            try
            {
                if (btnCall.Tag.ToString() == "Running" && phNumber == strNumber)
                {
                    //isHangUpAdmin = true;
                    btnChannels[intChannelNumber - 1].Tag = "Free";
                    btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcHangup(Convert.ToInt32(intChannelNumber.ToString()));
                    }
                    else
                    {
                    RClient.HangUp(Convert.ToInt32(intChannelNumber.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HangupAdmin()", "ctlDialer.xaml.cs");
            }
        }

        void ctlrptActiveAgent_EntsvcUnJoin(string uName)
        {           
        }

        #endregion              

        #region WCF Events for RTC on Vista
        void ctlDialer_entsvcJoin()
        {
        }

        void ctlDialer_entsvcCreateRTCClient()
        {
        }

        void ctlDialer_entsvcRegisterSIPPhone(string SIPUserName, string SIPPassword, string SIPServer)
        {
        }

        void ctlDialer_entsvcAnswer()
        {
        }

        void ctlDialer_entsvcDial(string PhoneNo, int Channel)
        {
        }

        void ctlDialer_entsvcHangup(int Channel)
        {
        }

        void ctlDialer_entsvcHold(int Channel, string HoldContent)
        {
        }

        void ctlDialer_entsvcSendDTMF(string DTMF, int Channel)
        {
        }

        void ctlDialer_entsvcTransfer(string PhoneNo, int Channel)
        {
        }

        void ctlDialer_entsvcRTCEvent(int ChannelId, string RTCEventName)
        {
            RClient_entstatus(ChannelId, RTCEventName);
        }

        void ctlDialer_entsvcUnJoin()
        {
        }
        #endregion

        #region Client For ActiveCall
        //public void hostActiveCallService(object lstParamsCall)
        //{
            //try
            //{
            //    List<object> lstTempObject = (List<object>)lstParamsCall;
            //    strUriforActiveCall = lstTempObject[0].ToString();
            //    NetPeerClient npcActiveCall=new NetPeerClient();
            //    ((NetP2PBootStrapActiveCallReportDelegates)objNetTcpActiveCall).EntsvcJoinCall += new NetP2PBootStrapActiveCallReportDelegates.DelsvcJoinCall(ctlDialer_EntsvcJoinCall);
            //    ((NetP2PBootStrapActiveCallReportDelegates)objNetTcpActiveCall).EntsvcGetCallInfo+=new NetP2PBootStrapActiveCallReportDelegates.DelsvcGetCallInfo(rptActiveCall_EntsvcGetCallInfo);
            //    ((NetP2PBootStrapActiveCallReportDelegates)objNetTcpActiveCall).EntsvcActiveCalls+=new NetP2PBootStrapActiveCallReportDelegates.DelsvcActiveCalls(rptActiveCall_EntsvcActiveCalls);
            //    ((NetP2PBootStrapActiveCallReportDelegates)objNetTcpActiveCall).EntsvcSetDuration+=new NetP2PBootStrapActiveCallReportDelegates.DelsvcSetDuration(rptActiveCall_EntsvcSetDuration);
            //    ((NetP2PBootStrapActiveCallReportDelegates)objNetTcpActiveCall).EntsvcUnJoinCall += new NetP2PBootStrapActiveCallReportDelegates.DelsvcUnJoinCall(ctlDialer_EntsvcUnJoinCall);
            //    channelNetTcpCall = (INetP2PBootStrapReportChannel)npcActiveCall.OpenClient<INetP2PBootStrapReportChannel>(strUriforActiveCall, lstTempObject[1].ToString(), ref objNetTcpActiveCall);
            //    channelNetTcpCall.svcJoinCall(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
          //}
        //void ctlDialer_EntsvcJoinCall(string uname)
        //{ }
        //void rptActiveCall_EntsvcGetCallInfo(string uName)
        //{
        //    try
        //    {
        //        if (myNumber != "")
        //        {
        //            //channelNetTcpCall.svcActiveCalls(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString(), VMuktiAPI.VMuktiInfo.CurrentPeer.GroupName, myStatus, "Calculating...", myNumber);
        //        }
        //        else
        //        {
        //            //channelNetTcpCall.svcActiveCalls(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString(), VMuktiAPI.VMuktiInfo.CurrentPeer.GroupName, myStatus, "00:00:00", "No Call");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rptActiveCall_EntsvcGetCallInfo()", "ctlDialer.xaml.cs");
        //    }
        //}
        //void rptActiveCall_EntsvcActiveCalls(string uName,string campaignId, string groupName, string Status, string callDuration, string phoneNo)
        //{}
        //void rptActiveCall_EntsvcSetDuration(string uName, string Status, string callDuration, string phoneNo)
        //{}
        //void ctlDialer_EntsvcUnJoinCall(string uname)
        //{ }
        #endregion
      
    }

}