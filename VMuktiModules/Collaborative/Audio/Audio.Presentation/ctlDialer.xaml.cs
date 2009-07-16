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
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using Audio.Business;
using Audio.Business.Service.BasicHttp;
using Audio.Business.Service.DataContracts;
using Audio.Business.Service.NetP2P;
using VMuktiAPI;

namespace Audio.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlDialer : IDisposable
    {
        RTCClient RClient;
        Button[] btnChannels;
        ModulePermissions[] _MyPermissions;
        System.Timers.Timer t1;
        Hashtable TimeCounterCollection;
        static XmlDocument doc = new XmlDocument();
        string strNumber;
        string strDefaultConfNumber;
        string strMyRole;
        string strBtnChClick = "0";
        int intChannelNumber = 1;
        bool blIsOnHold;
        bool blShiftPress;
        bool blIsVista;
        float HourCounter, MinCounter, SecondCounter = 00;
        private delegate void TimeChangeDelegate(float Secondes);
        List<string> lstGlobalBuddyList;

        static string AppPath = AppDomain.CurrentDomain.BaseDirectory + "Audio_Configuration.xml";
        long a = 10;
        XmlElement DomainElement;
        bool blConfigEnable;
        //bool blIsSoftPhoneLoaded = false;
        string GIPAddress;
        string strSoftPhoneSIPServer;
        string strSoftPhoneSIPNumber;
        string strSoftPhoneSIPPassword;
        System.ComponentModel.BackgroundWorker bwRegisterSIPUser;

        System.Threading.Thread ThOpenClinet;
        public static IService SNChannel;

        public delegate void DelStartconference(List<object> lstAnswer);
        public DelStartconference objDelStartconference;

        public delegate void DelSetConferenceUsers(List<object> lstUsers);
        public DelSetConferenceUsers objDelSetConferenceUsers;

        public delegate void DelStartInitialConference();
        public DelStartInitialConference objDelStartIntitialConfrence;

        object objNetP2PRTCVista = null;
        INetTcpRTCVistaServiceChannel ClientNetP2PRTCVistaChannel;

        System.Windows.Threading.DispatcherTimer dtGetConference = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        object objNetTcpAudio = new clsNetTcpAudio();
        INetTcpAudioChannel NetP2PChannel;
        int temp;
        public string strUri;
        object objHttpAudio = new clsHttpAudio();
        IHttpAudio Httpchannel;
        int tempcounter ;
        System.Diagnostics.Process p;
        VMuktiService.NetPeerClient npcRTCVistaClient;
        public delegate void DelAsyncGetMessage(List<clsMessage> myMessages);
        public DelAsyncGetMessage objDelAsyncGetMessage;

        /// <summary>
        /// Variable decalration to keep call information of each user.
        /// </summary>
        Audio.Business.ClsCallInfo objCallInfo;
        DateTime dtCallStartDate;
        System.Windows.Threading.DispatcherTimer TenMinTimer;
        bool isExternalCall;
        bool isExternalCallConnected;
        bool isAuthorisedToMakeCall;
        long ExternalCallChannelId = 0;
        

        public ctlDialer(VMuktiAPI.PeerType bindingtype, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                InitializeComponent();
                ClsException.WriteToLogFile("Audio module Constructor has been Started");
                lstGlobalBuddyList = new List<string>();
                System.OperatingSystem osInfo = System.Environment.OSVersion;
                if (osInfo.Version.Major.ToString() == "6")
                {
                    blIsVista = true;
                    p = new System.Diagnostics.Process();
                    p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\VistaAudio";
                    p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\VistaAudio\VMuktiAudio.VistaService.exe";
                    p.Start();
                }
                else
                {
                    blIsVista = false;
                }


                _MyPermissions = MyPermissions;
                //FncPermissionsReview();            
                strMyRole = Role;
                btnTransfer.IsEnabled = false;
                btnDTMF.IsEnabled = false;
                btnHangup.IsEnabled = false;
                btnHold.IsEnabled = false;
                btnAccept.IsEnabled = false;
                btnReject.IsEnabled = false;
                btnCall.IsEnabled = true;
                btnConference.IsEnabled = true;
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
                objDelStartconference = new DelStartconference(FncStartConference);
                objDelSetConferenceUsers = new DelSetConferenceUsers(FncSetConferenceUsers);
                objDelStartIntitialConfrence = new DelStartInitialConference(FbcStartInitialConference);
                btnChannels = new Button[6];
                int btnLeft = 5;
                int btnTop = 47;
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
                    btnChannels[i].IsEnabled = true;
                    CnvPhoneProperty.Children.Add(btnChannels[i]);
                    
                }
                t1 = new System.Timers.Timer(1000);
                t1.Elapsed += new System.Timers.ElapsedEventHandler(t1_Elapsed);
                UcPredictiveSoftPhone.AllowDrop = true;
                this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(ctlDialer_PreviewKeyDown);
                Application.Current.Exit += new ExitEventHandler(Current_Exit);

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
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcSendDTMF += new clsNetTcpRTCVistaService.DelsvcSendDTMF(ctlDialer_entsvcSendDTMF);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcTransfer += new clsNetTcpRTCVistaService.DelsvcTransfer(ctlDialer_entsvcTransfer);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcRTCEvent += new clsNetTcpRTCVistaService.DelsvcRTCEvent(ctlDialer_entsvcRTCEvent);
                    ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcUnJoin += new clsNetTcpRTCVistaService.DelsvcUnJoin(ctlDialer_entsvcUnJoin);
                    ClientNetP2PRTCVistaChannel = (INetTcpRTCVistaServiceChannel)npcRTCVistaClient.OpenClient<INetTcpRTCVistaServiceChannel>("net.tcp://localhost:6060/NetP2PRTCVista", "NetP2PRTCVistaMesh", ref objNetP2PRTCVista);
                    ClientNetP2PRTCVistaChannel.svcJoin();
                }

                bwRegisterSIPUser = new System.ComponentModel.BackgroundWorker();
                bwRegisterSIPUser.DoWork += new System.ComponentModel.DoWorkEventHandler(bwRegisterSIPUser_DoWork);
                bwRegisterSIPUser.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bwRegisterSIPUser_RunWorkerCompleted);

                FncRegister(null, null);
                ClsException.WriteToLogFile("Going to start thread.");
                ThOpenClinet = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FncOpenClinet));
                System.Collections.Generic.List<object> lstParameters = new System.Collections.Generic.List<object>();
                lstParameters.Add(bindingtype);
                lstParameters.Add(uri);
                ThOpenClinet.Start(lstParameters);

                //VMukti.Global.VMuktiGlobal.strBootStrapIPs[0] = "210.211.254.132";
                //VMukti.Global.VMuktiGlobal.strSuperNodeIP = "210.211.254.132";
                //VMukti.Global.VMuktiGlobal.strUserName = Environment.MachineName;
                
                ClsException.WriteToLogFile("Registering Singout event");
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_SignOut_VMuktiEvent);                
                objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);
                this.Loaded += new RoutedEventHandler(ctlDialer_Loaded);

                ///Object Creation to keep call information.
                objCallInfo = new ClsCallInfo();
                dtCallStartDate = new DateTime();
                TenMinTimer = new System.Windows.Threading.DispatcherTimer();
                TenMinTimer.Tick += new EventHandler(TenMinTimer_Tick);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer", "Audio\\ctlDialer.xaml.cs");
            }
        }	   

        #region resize

        void ctlDialer_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)) != null)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                    ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(ctlDialer_SizeChanged);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_Loaded", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)).ActualWidth > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_SizeChanged()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        #endregion

        #region Events for Vista system
        void ctlDialer_entsvcJoin()
        { }

        void ctlDialer_entsvcCreateRTCClient()
        { }

        void ctlDialer_entsvcRegisterSIPPhone(string SIPUserName, string SIPPassword, string SIPServer)
        { }

        void ctlDialer_entsvcAnswer()
        { }

        void ctlDialer_entsvcDial(string PhoneNo, int Channel)
        { }

        void ctlDialer_entsvcHangup(int Channel)
        { }

        void ctlDialer_entsvcHold(int Channel, string HoldContent)
        { }

        void ctlDialer_entsvcSendDTMF(string DTMF, int Channel)
        { }

        void ctlDialer_entsvcTransfer(string PhoneNo, int Channel)
        { }

        void ctlDialer_entsvcRTCEvent(int ChannelId, string RTCEventName)
        {
            RClient_entstatus(ChannelId, RTCEventName);
        }

        void ctlDialer_entsvcUnJoin()
        { }
        #endregion

        #region Button Click Events
        void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (btnChannels[0].Tag.ToString() == "Free")
                {
                    //**Incoming call on channel 0
                    ChClick(btnChannels[0], null);
                    btnChannels[Convert.ToInt16(0)].Tag = "Running";
                    btnChannels[Convert.ToInt16(0)].Background = Brushes.Green;
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcAnswer();
                    }
                    else
                    {
                        RClient.Anser();
                    }
                    btnHold.IsEnabled = true;
                    btnHangup.IsEnabled = true;
                    btnDTMF.IsEnabled = true;
                    ClsException.WriteToLogFile("Audio module: Incoming call on channel 0");
                }
                else
                {
                    MessageBox.Show("Channel 1 is Busy");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAccept_Click()", "Audio\\ctlDialer.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void btnCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblNumber.Text.ToString() == string.Empty || lblNumber.Text.ToString() == "Enter Number")
                {
                    btnConference_Click(null, null);
                    //lblNumber.Text = "Enter Number";
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
                            btnChannels[Convert.ToInt16(strBtnChClick)].Background = Brushes.Green;
                            FncCall(lblNumber.Text.ToString().Trim(), long.Parse(intChannelNumber.ToString()));
                            FncCallInformation(lblNumber.Text.ToString().Trim(),long.Parse(intChannelNumber.ToString()));
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
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCall_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void ChClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                strBtnChClick = (Convert.ToInt16(((Button)sender).Content.ToString()) - 1).ToString();
                intChannelNumber = (Convert.ToInt16(((Button)sender).Content.ToString()));
                if (((Button)sender).Tag.ToString() == "Free")
                {
                    btnHold.IsEnabled = false;
                    btnHangup.IsEnabled = false;
                    btnDTMF.IsEnabled = false;
                    btnConference.IsEnabled = true;
                    btnCall.IsEnabled = true;
                    for (int i = 0; i < 6; i++)
                    {
                        if (btnChannels[i].Tag.ToString() == "Running" || btnChannels[i].Tag.ToString() == "Conference")
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
                                btnChannels[i].Background = Brushes.Red;
                            }
                            else if (btnChannels[i].Tag.ToString() == "Conference")
                            {
                                btnChannels[i].Tag = "ConfHold";
                                if (blIsVista)
                                {
                                    ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(i + 1), "Hold");
                                }
                                else
                                {
                                    RClient.Hold(Convert.ToInt16(i + 1), "Hold");
                                }
                                btnChannels[i].Background = Brushes.Goldenrod;
                            }
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
                        ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(intChannelNumber), "Hold");
                    }
                    else
                    {
                        RClient.Hold(Convert.ToInt16(intChannelNumber), "Hold");
                    }
                    btnChannels[int.Parse(strBtnChClick)].Background = Brushes.Red;
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
                            btnChannels[i].Background = Brushes.Red;
                        }
                        else if (btnChannels[i].Tag.ToString() == "Conference" && btnChannels[i].Content != ((Button)sender).Content.ToString())
                        {
                            btnChannels[i].Tag = "ConfHold";
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(i + 1), "Hold");
                            }
                            else
                            {
                                RClient.Hold(Convert.ToInt16(i + 1), "Hold");
                            }
                            btnChannels[i].Background = Brushes.Goldenrod;
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
                        ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(intChannelNumber), "UnHold");
                    }
                    else
                    {
                        RClient.Hold(Convert.ToInt16(intChannelNumber), "UnHold");
                    }
                    btnHold.IsEnabled = true;
                    btnChannels[int.Parse(strBtnChClick)].Background = Brushes.Green;
                }
                else if (((Button)sender).Tag.ToString() == "Conference")
                {
                    if (((Button)sender).Background == Brushes.Blue)
                    {
                        ((Button)sender).Tag = "ConfHold";
                        if (blIsVista)
                        {
                            ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(intChannelNumber), "Hold");
                        }
                        else
                        {
                            RClient.Hold(Convert.ToInt16(intChannelNumber), "Hold");
                        }
                        ((Button)sender).Background = Brushes.Goldenrod;
                        btnHangup.IsEnabled = true;
                        btnHold.IsEnabled = true;
                        btnDTMF.IsEnabled = false;
                        btnConference.IsEnabled = false;
                        btnCall.IsEnabled = false;
                    }
                    else
                    {
                        ((Button)sender).Background = Brushes.Blue;
                    }
                }

                else if (((Button)sender).Tag.ToString() == "ConfHold")
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
                            btnChannels[i].Background = Brushes.Red;
                        }
                        else if (btnChannels[i].Tag.ToString() == "Conference" && btnChannels[i].Content != ((Button)sender).Content.ToString())
                        {
                            btnChannels[i].Tag = "ConfHold";
                            if (blIsVista)
                            {
                                ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt16(intChannelNumber), "Hold");
                            }
                            else
                            {
                                RClient.Hold(Convert.ToInt16(intChannelNumber), "Hold");
                            }
                            btnChannels[i].Background = Brushes.Goldenrod;
                        }
                    }

                    TimeSpan CallDurationTime = DateTime.Now - DateTime.Parse(TimeCounterCollection[intChannelNumber - 1].ToString());
                    t1.Start();
                    SecondCounter = CallDurationTime.Seconds;
                    MinCounter = CallDurationTime.Minutes;
                    HourCounter = CallDurationTime.Hours;
                    ((Button)sender).Tag = "Conference";
                    btnHold.IsEnabled = true;
                    btnHangup.IsEnabled = true;
                    btnDTMF.IsEnabled = true;
                    btnChannels[int.Parse(strBtnChClick)].Background = Brushes.Blue;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ChClick()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void btnConference_Click(object sender, RoutedEventArgs e)
        {
            //string[] strBuddyList = new string[lstBuddyList.SelectedItems.Count];
            //for (int j = 0; j < lstBuddyList.SelectedItems.Count; j++)
            //{
            //    strBuddyList[j] = ((ListBoxItem)lstBuddyList.SelectedItems[j]).Content.ToString();
            //}
            try
            {
                string strConfNumber = SNChannel.svcGetConferenceNumber();
                //** Get conference number from supernode strConfNumber

                //strConfNumber = strConfNumber + "@" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP.ToString();
                strConfNumber = strConfNumber + "@" + strSoftPhoneSIPServer;
                strDefaultConfNumber = strConfNumber;
                //** Going to fire a conference on conference number strConfNumber

                bool isFreeChannelAvail = false;
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Free")
                    {
                        ChClick(btnChannels[i], null);
                        btnChannels[Convert.ToInt16(i)].Tag = "Conference";
                        btnChannels[Convert.ToInt16(i)].Background = Brushes.Blue;
                        FncCall(strConfNumber, long.Parse(intChannelNumber.ToString()));
                        isFreeChannelAvail = true;
                        btnHangup.IsEnabled = true;
                        break;
                    }
                }
                string[] sTemp = new string[lstGlobalBuddyList.Count];
                for (int i = 0; i < lstGlobalBuddyList.Count; i++)
                {
                    sTemp[i] = lstGlobalBuddyList[i];
                }
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    if (NetP2PChannel != null)
                    {
                        NetP2PChannel.svcP2PStartConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strConfNumber, sTemp);
                    }
                }
                else
                {
                    if (Httpchannel != null)
                    {
                        Httpchannel.svcStartConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strConfNumber, sTemp);
                    }
                }

                if (!isFreeChannelAvail)
                {
                    MessageBox.Show("No Free Channels Available");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnConference_click()", "Audio\\ctlDialer.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDTMF_Click()", "Audio\\ctlDialer.xaml.cs");
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
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnHangup_click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void btnHold_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnChannels[intChannelNumber - 1].Tag = "Hold";
                FncHold(long.Parse(intChannelNumber.ToString()));
                btnHold.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnHold_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void NumClick(object sender, System.Windows.RoutedEventArgs e)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NumClick()", "Audio\\ctlDialer.xaml.cs");
            }
        }

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnPad_Click()", "Audio\\ctlDialer.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnReject_Click()", "Audio\\ctlDialer.xaml.cs");
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
                    btnChannels[intChannelNumber - 1].Tag = "Free";
                    btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                    string s = "###*3" + lblNumber.Text.ToString() + "#";
                    char[] ch = s.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        RClient.SendDTMF(ch[i].ToString(), intChannelNumber);
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnTransfer_CLick()", "Audio\\ctlDialer.xaml.cs");
            }
        }
        #endregion

        #region Local Functions
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ChangeTime()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void delAsyncGetMessage(List<clsMessage> myMessages)
        {
            try
            {
                #region original
                //try
                //{
                //    if (myMessages != null && myMessages.Count != 0 && myMessages[0].strUserList.Count > 0 && myMessages[0].strUserName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                //    {
                //        lstGlobalBuddyList = myMessages[0].strUserList;
                //        for (int count = 0; count < lstGlobalBuddyList.Count; count++)
                //        {
                //            if (lstGlobalBuddyList[count].ToString() == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                //            {
                //                lstGlobalBuddyList.Remove(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //            }
                //            else
                //            {
                //                List<object> LstConfUsers = new List<object>();
                //                LstConfUsers.Add(lstGlobalBuddyList[count].ToString());
                //                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);
                //            }
                //        }
                //        //if (!lstGlobalBuddyList.Contains(uName) && uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                //        //{
                //        //    lstGlobalBuddyList.Add(uName);
                //        //}
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("delAsyncGetMessage: " + ex.Message);
                //}


                //if (myMessages != null && myMessages.Count != 0 && myMessages[0].strUserName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName && myMessages[0].strConfNumber.ToString() != "")
                //{
                //    for (int i = 0; i < btnChannels.Length; i++)
                //    {
                //        if (btnChannels[i].Tag.ToString() == "Free")
                //        {
                //            ChClick(btnChannels[i], null);
                //            btnChannels[Convert.ToInt16(i)].Tag = "Conference";
                //            btnChannels[Convert.ToInt16(i)].Background = Brushes.Blue;
                //            FncCall(myMessages[0].strConfNumber.ToString(), long.Parse(intChannelNumber.ToString()));
                //            btnDTMF.IsEnabled = true;
                //            btnHangup.IsEnabled = true;
                //            btnHold.IsEnabled = true;
                //            break;
                //        }
                //    }
                //}
                #endregion

                if (myMessages != null)
                {
                    for (int i = 0; i < myMessages.Count; i++)
                    {
                        if (myMessages[i].msgType == "getuserlist")
                        {
                            try
                            {
                                if (myMessages[i].strUserName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    try
                                    {
                                        if (!lstGlobalBuddyList.Contains(myMessages[i].strUserName))
                                        {
                                            lstGlobalBuddyList.Add(myMessages[i].strUserName);
                                            List<object> LstConfUsers = new List<object>();
                                            LstConfUsers.Add(myMessages[i].strUserName);
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);


                                            if (myMessages[i].strConfNumber != "" && myMessages[i].strConfNumber != null)
                                            {
                                                for (int j = 0; j < btnChannels.Length; j++)
                                                {
                                                    if (btnChannels[j].Tag.ToString() == "Free")
                                                    {
                                                        ChClick(btnChannels[j], null);
                                                        btnChannels[Convert.ToInt16(j)].Tag = "Conference";
                                                        btnChannels[Convert.ToInt16(j)].Background = Brushes.Blue;
                                                        FncCall(myMessages[i].strConfNumber.ToString(), long.Parse(intChannelNumber.ToString()));
                                                        btnDTMF.IsEnabled = true;
                                                        btnHangup.IsEnabled = true;
                                                        btnHold.IsEnabled = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    catch
                                    { }




                                    if (Httpchannel != null)
                                    {
                                        if (strMyRole == "Host")
                                        {
                                            Httpchannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strDefaultConfNumber);
                                        }
                                        else
                                        {
                                            Httpchannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, null);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsynGetMessage()--1", "Audio\\ctlDialer.xaml.cs");
                            }
                        }

                        else if (myMessages[i].msgType == "setuserlist")
                        {
                            try
                            {
                                if (myMessages[i].strUserName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    if (!lstGlobalBuddyList.Contains(myMessages[i].strUserName))
                                    {
                                        lstGlobalBuddyList.Add(myMessages[i].strUserName);
                                        List<object> LstConfUsers = new List<object>();
                                        LstConfUsers.Add(myMessages[i].strUserName);
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);

                                        if (myMessages[i].strConfNumber != "" && myMessages[i].strConfNumber != null)
                                        {
                                            for (int j = 0; j < btnChannels.Length; j++)
                                            {
                                                if (btnChannels[j].Tag.ToString() == "Free")
                                                {
                                                    ChClick(btnChannels[j], null);
                                                    btnChannels[Convert.ToInt16(j)].Tag = "Conference";
                                                    btnChannels[Convert.ToInt16(j)].Background = Brushes.Blue;
                                                    FncCall(myMessages[i].strConfNumber.ToString(), long.Parse(intChannelNumber.ToString()));
                                                    btnDTMF.IsEnabled = true;
                                                    btnHangup.IsEnabled = true;
                                                    btnHold.IsEnabled = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()--2", "Audio\\ctlDialer.xaml.cs");
                            }
                        }

                        else if (myMessages[i].msgType == "conf")
                        {
                            try
                            {
                                if (myMessages != null && myMessages.Count != 0 && myMessages[i].strConfNumber.ToString() != "")
                                {
                                    for (int j = 0; j < btnChannels.Length; j++)
                                    {
                                        if (btnChannels[j].Tag.ToString() == "Free")
                                        {
                                            ChClick(btnChannels[j], null);
                                            btnChannels[Convert.ToInt16(j)].Tag = "Conference";
                                            btnChannels[Convert.ToInt16(j)].Background = Brushes.Blue;
                                            FncCall(myMessages[i].strConfNumber.ToString(), long.Parse(intChannelNumber.ToString()));
                                            btnDTMF.IsEnabled = true;
                                            btnHangup.IsEnabled = true;
                                            btnHold.IsEnabled = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()--3", "Audio\\ctlDialer.xaml.cs");
                            }
                        }

                        else if (myMessages[i].msgType == "unjoin")
                        {
                            try
                            {
                                if (myMessages[i].strUserName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    if (lstGlobalBuddyList.Contains(myMessages[i].strUserName))
                                    {
                                        lstGlobalBuddyList.Remove(myMessages[i].strUserName);
                                        List<object> LstConfUsers = new List<object>();
                                        LstConfUsers.Add(myMessages[i].strUserName);
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsynGetMessage()--4", "Audio\\ctlDialer.xaml.cs");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()--5", "Audio\\ctlDialer.xaml.cs");
            }
            finally
            {
                dtGetConference.Start();
            }
        }

        void dtGetConference_Tick(object sender, EventArgs e)
        {
            try
            {
                // List<clsMessage> myMessages = Httpchannel.svcGetConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //if (myMessages != null && myMessages.Count != 0)
                //{
                //    for (int i = 0; i < btnChannels.Length; i++)
                //    {
                //        if (btnChannels[i].Tag.ToString() == "Free")
                //        {
                //            ChClick(btnChannels[i], null);
                //            btnChannels[Convert.ToInt16(i)].Tag = "Conference";
                //            btnChannels[Convert.ToInt16(i)].Background = Brushes.Blue;
                //            FncCall(myMessages[0].strConfNumber.ToString(), long.Parse(intChannelNumber.ToString()));
                //            btnDTMF.IsEnabled = true;
                //            btnHangup.IsEnabled = true;
                //            btnHold.IsEnabled = true;
                //            break;
                //        }
                //    }
                //}
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();

                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dtGetConference_Tick()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void OnCompletion(IAsyncResult result)
        {
            try
            {
                List<clsMessage> objMsgs = Httpchannel.EndsvcGetConference(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetMessage, objMsgs);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnCompletion()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void StartThread()
        {
            try
            {
                // this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelGetMsg);
                if (Httpchannel != null)
                {
                    //channelHttp.BeginsvcGetMessages(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnCompletion, null);
                    Httpchannel.BeginsvcGetConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnCompletion, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartThread()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void FncOpenClinet(object lstParameters)
        {
            try
            {
                System.Collections.Generic.List<object> lstTempObj = (System.Collections.Generic.List<object>)lstParameters;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    VMuktiService.NetPeerClient ncpAudio = new VMuktiService.NetPeerClient();
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PJoin += new Audio.Business.Service.NetP2P.clsNetTcpAudio.DelsvcP2PJoin(ctlDialer_EntsvcP2PJoin);
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PStartConference += new Audio.Business.Service.NetP2P.clsNetTcpAudio.DelsvcP2PStartConference(ctlDialer_EntsvcP2PStartConference);
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcGetUserList += new clsNetTcpAudio.DelsvcGetUserList(ctlDialer_EntsvcGetUserList);
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcSetUserList += new clsNetTcpAudio.DelsvcSetUserList(ctlDialer_EntsvcSetUserList);
                    ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PUnJoin += new Audio.Business.Service.NetP2P.clsNetTcpAudio.DelsvcP2PUnJoin(ctlDialer_EntsvcP2PUnJoin);
                    NetP2PChannel = (INetTcpAudioChannel)ncpAudio.OpenClient<INetTcpAudioChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpAudio);

                    while (temp < 20)
                    {
                        try
                        {
                            ClsException.WriteToLogFile("Going to call svcJoin");
                            NetP2PChannel.svcP2PJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            ClsException.WriteToLogFile("svcJoin has been called.");
                            temp = 20;
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelStartIntitialConfrence);

                            if (strMyRole == "Host")
                            {
                                NetP2PChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strDefaultConfNumber);
                            }
                            else
                            {
                                NetP2PChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "");
                            }
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
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelStartIntitialConfrence);

                            if (strMyRole == "Host")
                            {

                                Httpchannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strDefaultConfNumber);
                            }
                            else
                            {
                                Httpchannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "");
                            }
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

                ClsException.WriteToLogFile("Opening client Completed successfully");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncOpenClient()", "Audio\\ctlDialer.xaml.cs");
            }
        }

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
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissioReview()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void FncRegister(object sender, VMuktiEventArgs e)
        {
            bwRegisterSIPUser.RunWorkerAsync();
        }


        void bwRegisterSIPUser_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                TimeCounterCollection = new Hashtable();
                for (int i = 0; i < 6; i++)
                {
                    TimeCounterCollection.Add(i, "0:00:00");
                }
                VMuktiService.BasicHttpClient bhcSuperNode = new VMuktiService.BasicHttpClient();
                SNChannel = (IService)bhcSuperNode.OpenClient<IService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/SNService");
                //**creating Supernode channel for SIP Numbers and SIP Conferencees
                string strSIPServerIP = VMuktiInfo.CurrentPeer.SuperNodeIP;
                string strSIPNumber = SNChannel.svcAddSIPUser();

                strSoftPhoneSIPNumber = strSIPNumber;
                strSoftPhoneSIPPassword = strSIPNumber;
                strSoftPhoneSIPServer = strSIPServerIP;

                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncRegister()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void bwRegisterSIPUser_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (blIsVista)
            {
                ClientNetP2PRTCVistaChannel.svcRegisterSIPPhone(strSoftPhoneSIPPassword, strSoftPhoneSIPPassword, strSoftPhoneSIPServer);
            }
            else
            {
                RClient = new RTCClient(strSoftPhoneSIPNumber, strSoftPhoneSIPPassword, strSoftPhoneSIPServer);
                RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
            }
            lblUserNumber.Content = strSoftPhoneSIPNumber + "@" + strSoftPhoneSIPServer;
            ClsException.WriteToLogFile("Audio module: creating Supernode channel for SIP Numbers and SIP Conferencees \r\n User Number Is: " + lblUserNumber.Content);
        }

        void FbcStartInitialConference()
        {
            if (strMyRole == "Host")
            {
                ClsException.WriteToLogFile("Firing new conference");
                btnConference_Click(null, null);
            }
        }
        #endregion

        #region Events
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

                    //lblNumber.Text = lblNumber.Text.ToString() + KeyValue;
                }

                else if (e.Key == System.Windows.Input.Key.Multiply)
                { lblNumber.Text = lblNumber.Text.ToString() + "*"; }

                else if (e.Key == System.Windows.Input.Key.Enter)
                {
                    btnCall_Click(null, null);
                }

                else if (e.Key == System.Windows.Input.Key.Back)
                { btnCancel_Click(null, null); }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_PreviewKeyDown()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void lstItem_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.DragDrop.DoDragDrop((DependencyObject)((ListBoxItem)sender), ((ListBoxItem)sender), DragDropEffects.Copy);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "lstItem_PreviewMouseDown()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void RClient_entstatus(int ChannelId, string status)
        {
            try
            {
                switch (status)
                {
                    case "InPorgress":
                        break;

                    case "Connected":
                        lblNumber.Text = "";
                        if (isExternalCall)
                        {
                            isExternalCallConnected = true;
                            dtCallStartDate = DateTime.Now;
                            TenMinTimer.Start();
                        }
                        lblStatus_Copy.Content = "Connected " + strNumber;
                        //**User Connected with number=lblStatus_Copy.Content
                        ClsException.WriteToLogFile("User Connected with number:" + lblStatus_Copy.Content);                        
                        btnHold.IsEnabled = true;
                        btnDTMF.IsEnabled = true;
                        btnHangup.IsEnabled = true;
                        t1.Start();
                        if (TimeCounterCollection[intChannelNumber - 1].ToString() == "0:00:00")
                        {
                            TimeCounterCollection[intChannelNumber - 1] = DateTime.Now;
                        }
                        break;

                    case "Disconnected":
                        lock (this)
                        {
                            lblNumber.Text = "";
                            lblStatus_Copy.Content = "Disconnected";
                            btnHold.IsEnabled = false;
                            btnDTMF.IsEnabled = false;
                            btnHangup.IsEnabled = false;
                            btnCall.IsEnabled = true;
                            btnConference.IsEnabled = true;
                            btnChannels[ChannelId - 1].Tag = "Free";
                            btnChannels[ChannelId - 1].Background = Brushes.Transparent;
                            TimeCounterCollection[ChannelId - 1] = "0:00:00";
                            if (intChannelNumber != (ChannelId - 1))
                            {
                                t1.Stop();
                                HourCounter = 0;
                                MinCounter = 0;
                                SecondCounter = 0;
                            }

                            if (isExternalCall && isExternalCallConnected)
                            {
                                string endtime = DateTime.Now.ToString();
                                TimeSpan duration = (DateTime.Parse(endtime) - dtCallStartDate);
                                int CallDuration = int.Parse(duration.TotalSeconds.ToString().Split('.')[0]);
                                objCallInfo.Save(0, DateTime.Now, dtCallStartDate, dtCallStartDate, long.Parse(CallDuration.ToString()), 0, 0, 0, "", false, false, long.Parse(VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString()), "");
                                isExternalCall = false;
                                isExternalCallConnected = false;
                            }
                        }

                        break;

                    case "Incoming":
                        btnAccept.IsEnabled = true;
                        btnReject.IsEnabled = true;
                        if (btnChannels[0].Tag.ToString() == "Free")
                        {
                            lblStatus_Copy.Content = "Incoming";
                        }
                        else
                        {
                            btnReject_Click(null, null);
                        }
                        break;

                    case "Hold":
                        lblStatus_Copy.Content = "OnHold";
                        blIsOnHold = true;
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RClient_entstatus()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void UcPredictiveSoftPhone_PreviewDrop(object sender, DragEventArgs e)
        {
            try
            {
                //if (e.Data.GetDataPresent(typeof(ListBoxItem)))
                //{
                lstGlobalBuddyList.Add(((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString());

                //if (btnChannels[int.Parse(strBtnChClick)].Tag.ToString() == "Conference")
                //{
                //    //Send Conference Information
                //    string[] strBuddyList = new string[1];
                //    strBuddyList[0]=((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString();
                //    string temp = lblStatus_Copy.Content.ToString().Split(' ')[1].ToString();
                //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                //    {
                //        if (NetP2PChannel != null)
                //        {
                //            System.Threading.Thread.Sleep(10000);
                //            NetP2PChannel.svcP2PStartConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, temp, strBuddyList);
                //        }
                //    }
                //    else
                //    {
                //        if (Httpchannel != null)
                //        {
                //            System.Threading.Thread.Sleep(10000);
                //            Httpchannel.svcStartConference(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, temp, strBuddyList);
                //        }
                //    }
                //}
                //else if (btnChannels[int.Parse(strBtnChClick)].Tag.ToString() == "Free")
                //{
                //    string strLocalNumber = ((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString();
                //    bool isFreeChannelAvail = false;
                //    for (int i = 0; i < btnChannels.Length; i++)
                //    {
                //        if (btnChannels[i].Tag.ToString() == "Free")
                //        {
                //            btnChannels[Convert.ToInt16(i)].Tag = "Running";
                //            btnChannels[Convert.ToInt16(i)].Background = Brushes.Green;
                //            FncCall(strLocalNumber.Trim().ToString(), long.Parse(intChannelNumber.ToString()));
                //            isFreeChannelAvail = true;
                //            btnDTMF.IsEnabled = true;
                //            btnHangup.IsEnabled = true;
                //            btnHold.IsEnabled = true;
                //            break;
                //        }
                //    }
                //    if (!isFreeChannelAvail)
                //    {
                //        MessageBox.Show("No Free Channels Available");
                //    }
                //}
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UcPredictiveSoftPhone_PreviewDrop()", "Audio\\ctlDialer.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "t1_Elapsed()", "Audio\\ctlDialer.xaml.cs");
            }
        }
        #endregion

        public enum CallStatus
        {
            NotInCall, CallInProgress, DialingToDest, RingingToDest,
            BusyTone, AMD, DTMF, DestUserStratListen, CallHangUp, CallHoldByAgent,
            CallHoldBySys, CallDispose, ReadyState
        };

        #region Net.tcp functions
        void ctlDialer_EntsvcP2PJoin(string uName)
        { }

        void ctlDialer_EntsvcP2PStartConference(string uName, string strConfNumber, string[] GuestInfo)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uName)
                {
                    List<object> LstConfInfo = new List<object>();
                    LstConfInfo.Add(uName);
                    LstConfInfo.Add(strConfNumber);
                    LstConfInfo.Add(GuestInfo);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelStartconference, LstConfInfo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcP2PStartConference()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_EntsvcGetUserList(string uName, string strConf)
        {
            try
            {
                if (!lstGlobalBuddyList.Contains(uName) && uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    lstGlobalBuddyList.Add(uName);
                    List<object> LstConfUsers = new List<object>();
                    LstConfUsers.Add(uName);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);

                    if (strConf != "" && strConf != null)
                    {

                        List<object> LstConfInfo = new List<object>();
                        LstConfInfo.Add(uName);
                        LstConfInfo.Add(strConf);
                        LstConfInfo.Add(null);
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelStartconference, LstConfInfo);
                    }
                }

                if (strMyRole == "Host")
                {
                    NetP2PChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strDefaultConfNumber);
                }
                else
                {
                    NetP2PChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, "");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcGetUserList()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_EntsvcSetUserList(string uName, string strConf)

        {
            try
            {
                lock (this)
                {
                    if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        bool flag = true;
                        for (int i = 0; i < lstGlobalBuddyList.Count; i++)
                        {
                            if (lstGlobalBuddyList[i] == uName)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            lstGlobalBuddyList.Add(uName);
                            List<object> LstConfUsers = new List<object>();
                            LstConfUsers.Add(uName);
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);

                            if (strConf != "" && strConf != null)
                            {

                                List<object> LstConfInfo = new List<object>();
                                LstConfInfo.Add(uName);
                                LstConfInfo.Add(strConf);
                                LstConfInfo.Add(null);
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelStartconference, LstConfInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcSetUserList()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_EntsvcP2PUnJoin(string uName)
        {
            try
            {
                if (lstGlobalBuddyList.Contains(uName))
                {
                    lstGlobalBuddyList.Remove(uName);
                    List<object> LstConfUsers = new List<object>();
                    LstConfUsers.Add(uName);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetConferenceUsers, LstConfUsers);

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_EntsvcP2PUnJoin()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        public void FncStartConference(List<object> ConfInfo)
        {
            try
            {
                if (ConfInfo[2] == null)
                {
                    for (int j = 0; j < btnChannels.Length; j++)
                    {
                        bool isFreeChannelAvail = false;
                        if (btnChannels[j].Tag.ToString() == "Free")
                        {
                            ChClick(btnChannels[j], null);
                            btnChannels[Convert.ToInt16(j)].Tag = "Conference";
                            btnChannels[Convert.ToInt16(j)].Background = Brushes.Blue;
                            FncCall(ConfInfo[1].ToString(), long.Parse(intChannelNumber.ToString()));
                            isFreeChannelAvail = true;
                            btnDTMF.IsEnabled = true;
                            btnHangup.IsEnabled = true;
                            btnHold.IsEnabled = true;
                            break;
                        }
                    }
                }
                else
                {
                    string[] sTempGuestList = (string[])ConfInfo[2];
                    for (int j = 0; j < sTempGuestList.Length; j++)
                    {
                        if (sTempGuestList[j] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                        {
                            bool isFreeChannelAvail = false;
                            for (int i = 0; i < btnChannels.Length; i++)
                            {
                                if (btnChannels[i].Tag.ToString() == "Free")
                                {
                                    ChClick(btnChannels[i], null);
                                    btnChannels[Convert.ToInt16(i)].Tag = "Conference";
                                    btnChannels[Convert.ToInt16(i)].Background = Brushes.Blue;
                                    FncCall(ConfInfo[1].ToString(), long.Parse(intChannelNumber.ToString()));
                                    isFreeChannelAvail = true;
                                    btnDTMF.IsEnabled = true;
                                    btnHangup.IsEnabled = true;
                                    btnHold.IsEnabled = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncStartConference()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        public void FncSetConferenceUsers(List<object> ConfUser)
        {
            try
            {
                lblConferenceUsers.Content = "";
                for (int i = 0; i < lstGlobalBuddyList.Count; i++)
                {
                    if (lblConferenceUsers.Content.ToString() == "")
                    {
                        lblConferenceUsers.Content = lstGlobalBuddyList[i].ToString();
                    }
                    else
                    {
                        lblConferenceUsers.Content = lblConferenceUsers.Content.ToString() + ", " + lstGlobalBuddyList[i].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncSetConferenceUsers()", "Audio\\ctlDialer.xaml.cs");
            }
        }
        #endregion

        #region Local Functions for RTC
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
                    RClient.Dial(number.ToString(), Convert.ToInt32(channelID));
                }
                //txtStatus.Focus();
                ClsException.WriteToLogFile("Audio module: Firing call on number and channelID \r\n Number is: " + strNumber + "\r\n ChannelID: " + channelID.ToString());
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCall()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        public void FncHangUp(long channelID)
        {
            try
            {
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Hold" || btnChannels[i].Tag.ToString() == "Running")
                    {
                        btnChannels[i].Tag = "Free";
                        btnChannels[i].Background = Brushes.Transparent;
                        //**Call hangup on channel=channelID
                        ClsException.WriteToLogFile("Call hangup on channel: " + channelID);
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
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncHangUp()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        public void FncHold(long channelID)
        {
            try
            {
                //**Putting Call on Hold on channel=channelId
                ClsException.WriteToLogFile("Audio module \r\n Putting Call on Hold on channel: " + channelID);
                if (blIsVista)
                {
                    ClientNetP2PRTCVistaChannel.svcHold(Convert.ToInt32(channelID), btnHold.Content.ToString());
                }
                else
                {
                    RClient.Hold(Convert.ToInt32(channelID), btnHold.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncHold()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        public void FncTransfer(string Number, long channelID)
        {
            try
            {
                //**Transfering Call on number=Number and channel=channelId
                StringBuilder sb = new StringBuilder();
                ClsException.WriteToLogFile("Audio module:  Transfering Call on \r\n Number: " + Number + "channel: " + channelID);                
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncTransfer()", "Audio\\ctlDialer.xaml.cs");
            }
        }
        #endregion

        #region ForSoftPhone
        private void btnSoftPhone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvSoftPhone.Children.Clear();
                a = 10;
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Running" || btnChannels[i].Tag.ToString() == "Hold" || btnChannels[i].Tag.ToString() == "Conference" || btnChannels[i].Tag.ToString() == "ConfHold")
                    {
                        MessageBox.Show("Hang up All calls");
                    }
                    else
                    {
                        CnvDialer.Visibility = Visibility.Collapsed;
                        SoftPhoneScroll.Visibility = Visibility.Visible;
                        cnvSoftPhoneTemp.Visibility = Visibility.Visible;
                        //if (!blIsSoftPhoneLoaded)
                        //{
                        if (!File.Exists(AppPath))
                        {
                            //Stream xmlFile = null;
                            //xmlFile = new FileStream(AppPath, FileMode.Create, FileAccess.ReadWrite);
                            // XmlDocument doc = new XmlDocument();
                            doc.LoadXml(("<Domains></Domains>"));
                            doc.Save(AppPath);
                            // blIsSoftPhoneLoaded = true;
                        }
                        else
                        {
                            //XmlDocument doc = new XmlDocument();
                            //if (doc.ChildNodes.Count != 0)
                            //{
                            doc.Load(AppPath);
                            XmlNodeList DomainList = doc.GetElementsByTagName("Domain");

                            foreach (XmlNode node in DomainList)
                            {
                                DomainElement = (XmlElement)node;
                                string UserName = DomainElement.GetElementsByTagName("User")[0].InnerText;
                                string Password = DomainElement.GetElementsByTagName("Password")[0].InnerText;
                                string IPAddress = "";

                                if (DomainElement.HasAttributes)
                                {
                                    IPAddress = DomainElement.Attributes["IP"].InnerText;

                                }
                                FncCreateBtnAndLbl(UserName, Password, IPAddress);
                            }
                            //}
                            //  blIsSoftPhoneLoaded = true;
                        }
                        //}
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSoftPhone_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void btnAddSipInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoftPhoneScroll.Visibility = Visibility.Collapsed;
                cnvSoftPhoneTemp.Visibility = Visibility.Collapsed;
                cnvSoftConfig.Visibility = Visibility.Visible;
                //showCvanvas(cnvSoftConfig);
                txtDomain.Text = "";
                txtPassWord.Password = "";
                txtUser.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddSipInfo_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void btnSoftPhoneOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CnvDialer.Visibility = Visibility.Visible;
                SoftPhoneScroll.Visibility = Visibility.Collapsed;
                cnvSoftPhoneTemp.Visibility = Visibility.Collapsed;
                lblNumber.Text = "";
                if (strSoftPhoneSIPNumber != "")
                {
                    if (blIsVista)
                    {
                        ClientNetP2PRTCVistaChannel.svcRegisterSIPPhone(strSoftPhoneSIPNumber, strSoftPhoneSIPPassword, strSoftPhoneSIPServer);
                    }
                    else
                    {
                        RClient = new RTCClient(strSoftPhoneSIPNumber, strSoftPhoneSIPPassword, strSoftPhoneSIPServer);
                    }
                    lblUserNumber.Content = strSoftPhoneSIPNumber + "@" + strSoftPhoneSIPServer;
                    RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
                    //**User Registered on new account using sip number=strSoftPhoneSIPNumber, password=strSoftPhoneSIPPassword on server=strSoftPhoneSIPServer
                    //StringBuilder sb = new StringBuilder();
                    ClsException.WriteToLogFile("Audio module: User Registered on new account using \r\n sip number:" + strSoftPhoneSIPNumber + "\r\nserver:" + strSoftPhoneSIPServer);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSoftPhoneOK_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void btnSoftPhoneCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoftPhoneScroll.Visibility = Visibility.Collapsed;
                cnvSoftPhoneTemp.Visibility = Visibility.Collapsed;
                CnvDialer.Visibility = Visibility.Visible;
                lblNumber.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSoftPhoneCancel_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void btnSoftConfigSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUser.Text != "" && txtPassWord.Password != "" && txtDomain.Text != "")
                {
                    if (!blConfigEnable)
                    {
                        FncWrite();
                    }
                    else if (blConfigEnable)
                    {
                        FncUpdate();
                    }
                }
                else
                    MessageBox.Show("Entry Can not be added U must have to enter every field.");
                cnvSoftConfig.Visibility = Visibility.Collapsed;
                SoftPhoneScroll.Visibility = Visibility.Visible;
                cnvSoftPhoneTemp.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSoftConfigSave_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void btnSoftConfigok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvSoftConfig.Visibility = Visibility.Collapsed;
                SoftPhoneScroll.Visibility = Visibility.Visible;
                cnvSoftPhoneTemp.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSoftConfigok_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void FncCreateBtnAndLbl(string UserName, string Password, string IPAddress)
        {
            try
            {
                Label lbIPAdd = new Label();
                Canvas.SetTop(lbIPAdd, a);
                lbIPAdd.Content = IPAddress;
                lbIPAdd.Margin = new System.Windows.Thickness(10, a, 10, 10);
                lbIPAdd.Name = "Domain";
                lbIPAdd.Tag = IPAddress;
                cnvSoftPhone.Children.Add(lbIPAdd);

                Label lbUserName = new Label();
                lbUserName.Content = UserName;
                lbUserName.Tag = IPAddress;
                lbUserName.Name = "userName";
                lbUserName.Visibility = Visibility.Hidden;
                cnvSoftPhone.Children.Add(lbUserName);

                Label lbPassword = new Label();
                lbPassword.Content = Password;
                lbPassword.Name = "password";
                lbPassword.Tag = IPAddress;
                lbPassword.Visibility = Visibility.Hidden;
                cnvSoftPhone.Children.Add(lbPassword);

                Button btnUse = new Button();
                Canvas.SetTop(btnUse, a);
                btnUse.Height = 25;
                btnUse.Width = 50;
                //aplyColour(btnUse);
                btnUse.Margin = new System.Windows.Thickness(116, a, 10, 10);
                btnUse.Content = "Default";
                btnUse.Tag = IPAddress;
                cnvSoftPhone.Children.Add(btnUse);

                Button btnConfig = new Button();
                Canvas.SetTop(btnConfig, a);
                btnConfig.Height = 25;
                btnConfig.Width = 60;
                btnConfig.Margin = new System.Windows.Thickness(170, a, 10, 10);
                btnConfig.Content = "Configure";
                btnConfig.Tag = IPAddress;
                cnvSoftPhone.Children.Add(btnConfig);

                Button btnRemove = new Button();
                Canvas.SetTop(btnRemove, a);
                btnRemove.Height = 25;
                btnRemove.Width = 50;
                btnRemove.Margin = new System.Windows.Thickness(240, a, 10, 10);
                btnRemove.Content = "Remove";
                btnRemove.Tag = IPAddress;
                cnvSoftPhone.Children.Add(btnRemove);

                btnUse.Click += new RoutedEventHandler(btnUse_Click);
                btnConfig.Click += new RoutedEventHandler(btnConfig_Click);
                btnRemove.Click += new RoutedEventHandler(btnRemove_Click);
                cnvSoftPhone.Height += 40;
                a += 15;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCreateBtnAndLbl()", "Audio\\ctlDialer.xaml.cs");
            }
        }
       
        void btnUse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sTag = ((Button)sender).Tag.ToString();
                foreach (UIElement lb in cnvSoftPhone.Children)
                {
                    if (lb.GetType().ToString() == "System.Windows.Controls.Label")
                    {
                        if (sTag == ((Label)lb).Tag.ToString())
                        {
                            if (((Label)lb).Name.ToString() == "password")
                            {
                                strSoftPhoneSIPPassword = ((Label)lb).Content.ToString();
                            }
                            else if (((Label)lb).Name.ToString() == "userName")
                            {
                                strSoftPhoneSIPNumber = ((Label)lb).Content.ToString();
                            }
                            else if (((Label)lb).Name.ToString() == "Domain")
                            {
                                strSoftPhoneSIPServer = ((Label)lb).Content.ToString();
                            }
                            ((Label)lb).Foreground = Brushes.Green;//Color.FromRgb(88,244,22); //"for changing the colour of lable wich is currently in use."
                        }
                        else
                            ((Label)lb).Foreground = Brushes.Red;
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnUse_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }
       
        void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoftPhoneScroll.Visibility = Visibility.Collapsed;
                cnvSoftPhoneTemp.Visibility = Visibility.Collapsed;
                cnvSoftConfig.Visibility = Visibility.Visible;
                string sTag = ((Button)sender).Tag.ToString();
                blConfigEnable = true;
                foreach (UIElement lb in cnvSoftPhone.Children)
                {
                    if (lb.GetType().ToString() == "System.Windows.Controls.Label")
                    {
                        if (sTag == ((Label)lb).Tag.ToString())
                        {
                            if (((Label)lb).Name.ToString() == "password")
                            {
                                txtPassWord.Password = ((Label)lb).Content.ToString();
                            }
                            else if (((Label)lb).Name.ToString() == "userName")
                            {
                                txtUser.Text = ((Label)lb).Content.ToString();
                            }
                            else if (((Label)lb).Name.ToString() == "Domain")
                            {
                                txtDomain.Text = ((Label)lb).Content.ToString();
                                GIPAddress = ((Label)lb).Content.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnConfig_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }
       
        void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            string sTag = ((Button)sender).Tag.ToString();
            try
            {
                //foreach (UIElement lb in cnvHome.Children)
                int flag = 0;
                for (int i = 0; i < cnvSoftPhone.Children.Count; i++)
                {

                    if (((UIElement)cnvSoftPhone.Children[i]).GetType().ToString() == "System.Windows.Controls.Label")
                    {
                        if (((Label)cnvSoftPhone.Children[i]).Tag.ToString() == sTag)
                        {
                            cnvSoftPhone.Children.RemoveAt(i);
                            XmlDocument doc = new XmlDocument();
                            doc.Load(AppPath);
                            XmlNodeList DomainList = doc.GetElementsByTagName("Domain");

                            foreach (XmlNode node in DomainList)
                            {
                                XmlElement DomainElement = (XmlElement)node;
                                if (DomainElement.HasAttributes)
                                {
                                    if (DomainElement.Attributes["IP"].InnerText == sTag)
                                    {
                                        //MessageBox.Show(DomainElement.InnerText.ToString());
                                        node.ParentNode.RemoveChild(node);


                                        doc.Save(AppPath);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (((UIElement)cnvSoftPhone.Children[i]).GetType().ToString() == "System.Windows.Controls.Button")
                    {
                        if (((Button)cnvSoftPhone.Children[i]).Tag != null && ((Button)cnvSoftPhone.Children[i]).Tag.ToString() == sTag)
                        {
                            cnvSoftPhone.Children.RemoveAt(i);
                            i -= 1;
                            flag = 1;//if this is the last button then we have to reposition  the next controlls
                        }
                    }

                    if (flag == 1) // for repositionong the controll
                    {

                        double hig = Canvas.GetTop(((UIElement)cnvSoftPhone.Children[i]));
                        //MessageBox.Show(hig.ToString());
                        Canvas.SetTop(((UIElement)cnvSoftPhone.Children[i]), hig - 30);

                    }
                }
                flag = 0;
                a -= 15;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnRemove_Click()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void FncWrite()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppPath);

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(CreateDomainnode(txtDomain.Text, txtUser.Text, txtPassWord.Password));
                XmlNode nd = xmlDoc.ImportNode(xd.SelectSingleNode("//Domain"), true);
                xmlDoc.DocumentElement.AppendChild(nd);
                xmlDoc.Save(AppPath);
                ClsException.WriteToLogFile("Audio module: Writting new account for sip user:" + txtUser.Text + "sip password:" + txtPassWord.Password + "Domain:" + txtDomain.Text);                
                FncCreateBtnAndLbl(txtUser.Text, txtPassWord.Password, txtDomain.Text);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncWrite()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private void FncUpdate()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppPath);
                XmlNodeList DomainList = doc.GetElementsByTagName("Domain");

                foreach (XmlNode node in DomainList)
                {
                    XmlElement DomainElement = (XmlElement)node;
                    if (DomainElement.HasAttributes)
                    {
                        if (DomainElement.Attributes["IP"].InnerText == GIPAddress)
                        {
                            DomainElement.GetElementsByTagName("User")[0].InnerText = txtUser.Text; ;
                            DomainElement.GetElementsByTagName("Password")[0].InnerText = txtPassWord.Password;
                            DomainElement.Attributes["IP"].InnerText = txtDomain.Text;
                            break;
                        }
                    }
                }
                doc.Save(AppPath);
                blConfigEnable = false;
                foreach (UIElement lb in cnvSoftPhone.Children)
                {
                    if (lb.GetType().ToString() == "System.Windows.Controls.Label")
                    {
                        if (((Label)lb).Tag.ToString() == GIPAddress)
                        {
                            ((Label)lb).Tag = txtDomain.Text;
                            if (((Label)lb).Name.ToString() == "password")
                            {
                                ((Label)lb).Content = txtPassWord.Password;
                            }
                            else if (((Label)lb).Name.ToString() == "userName")
                            {
                                ((Label)lb).Content = txtUser.Text;
                            }
                            else if (((Label)lb).Name.ToString() == "Domain")
                            {
                                ((Label)lb).Content = txtDomain.Text;
                            }
                        }
                    }
                    else if (lb.GetType().ToString() == "System.Windows.Controls.Button")
                    {
                        if (((Button)lb).Tag != null)
                        {
                            if (((Button)lb).Tag.ToString() == GIPAddress)
                            {
                                ((Button)lb).Tag = txtDomain.Text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncUpdate()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        private string CreateDomainnode(string strDomainName, string strUserName, string strPassword)
        {
            try
            {
                StringBuilder strSB = new StringBuilder();
                strSB.Append("<Domain IP='" + strDomainName + "'>");
                strSB.Append("<User>" + strUserName + "</User>");
                strSB.Append("<Password>" + strPassword + "</Password>");
                strSB.Append("</Domain>");
                return strSB.ToString();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreateDomainnode()", "Audio\\ctlDialer.xaml.cs");                
                return null;
            }
        }


        #endregion

        #region For UserCall Information

        void FncCallInformation(string PhoneNumber,long ExternalId)
        {
            try
            {
                if (PhoneNumber.StartsWith("5555"))
                {
                    int Talktime = objCallInfo.GetTalkTime(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                    if (Talktime >= 600 || Talktime >= 599)
                    {
                        isAuthorisedToMakeCall = false;
                        FncHangUp(ExternalId);
                    }
                    else
                    {
                        isAuthorisedToMakeCall = true;
                        isExternalCall = true;
                        TenMinTimer.Interval = new TimeSpan(0, 0, 600 - Talktime);
                        ExternalCallChannelId = ExternalId;

                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCallInformation()", "Audio.Presentation--ctlDialer.xaml.cs");
            }
        }

        void TenMinTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                FncHangUp(ExternalCallChannelId);
                TenMinTimer.Stop();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TenMinTimer_Tick()", "Audio.Presentation--ctlDialer.xaml.cs");
            }
        }
        #endregion

        #region Closing
        void ctlDialer_Unloaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    for (int i = 0; i < btnChannels.Length; i++)
            //    {
            //        if (btnChannels[i].Tag.ToString() == "Running" || btnChannels[i].Tag.ToString() == "Hold" || btnChannels[i].Tag.ToString() == "Conference" || btnChannels[i].Tag.ToString() == "ConfHold")
            //        {
            //            btnChannels[i].Tag = "Free";
            //            btnChannels[i].Background = Brushes.Transparent;
            //            RClient.HangUp(i + 1);
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{
            //    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Common--:--Audio--:--Audio.Presentation--:--ctlDialer.xaml.cs--:--ctlDialer_Unloaded()--");
            //    ClsException.LogError(ex);
            //    ClsException.WriteToErrorLogFile(ex);
            //}
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                ClosePod();
                VMuktiHelper.UnRegisterEvent("SignOut");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        void ctlDialer_SignOut_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDialer_SignOut_VMuktiEvent()", "Audio\\ctlDialer.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() == "Running" || btnChannels[i].Tag.ToString() == "Hold" || btnChannels[i].Tag.ToString() == "Conference" || btnChannels[i].Tag.ToString() == "ConfHold")
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



                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    if (NetP2PChannel != null)
                    {
                        NetP2PChannel.svcP2PUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }
                else
                {
                    if (Httpchannel != null)
                    {
                        Httpchannel.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }


                if (NetP2PChannel != null)
                {
                    NetP2PChannel.Close();
                    NetP2PChannel.Dispose();
                    NetP2PChannel = null;
                }
                if (Httpchannel != null)
                {
                    Httpchannel = null;
                }
                if (ClientNetP2PRTCVistaChannel != null)
                {
                    if (p != null)
                    {
                        p.Kill();
                        p.CloseMainWindow();
                        p.Close();
                        p.Dispose();
                        p = null;
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
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePOD()", "Audio\\ctlDialer.xaml.cs");
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
           // GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            //if (!this.disposed)
           // {
               // if (disposing)
               // {
            try
            {
                RClient = null;
                TimeCounterCollection = null;
                strNumber = string.Empty;
                strBtnChClick = string.Empty;
                intChannelNumber = 0;
                lstGlobalBuddyList = null;
                DomainElement = null;
                ThOpenClinet = null;
                SNChannel = null;
                objNetTcpAudio = null;
                NetP2PChannel.Close();
                NetP2PChannel.Dispose();
                NetP2PChannel = null;
                objHttpAudio = null;
                Httpchannel = null;
                t1.Stop();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Audio\\ctlDialer.xaml.cs");
            }
               // }
               // disposed = true;
            //}
           // GC.SuppressFinalize(this);
        }

        ~ctlDialer()
        { Dispose(false); }

        #endregion

    }
}