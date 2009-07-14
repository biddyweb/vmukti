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
using PredictivePhone.Business;
using System.Collections;
using VMuktiAPI;
using System.Threading;
using PredictivePhone.Business.service;
using System.Collections.Generic;
using VMuktiService;
using System.Windows.Threading;

namespace PredictivePhone.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

	public partial class ctlPredictivePhone
	{
        RTCClient RClient = null;
        string strNumber = "";
        string strSIPServerIP;
        string strSIPNumber;
        string strBtnChClick = "0";
        string strLastNumber = "1110";
        string strLastConfNumber = "860000";
        int intChannelNumber = 1;
        int intExtraCallChannelNumber = -1;
        bool blTransferRequired = false;
        Button[] btnChannels = null;
        bool blIsOnHold = false;
        string strCallingType = "Predictive";
        System.Timers.Timer t1;
        float HourCounter, MinCounter, SecondCounter = 00;
        Hashtable TimeCounterCollection = new Hashtable();
        string DetectedTone = "Human";
        int ToneDetectedChannel = 0;
        int PredictiveChannelNo = 0;

        private delegate void TimeChangeDelegate(float Secondes);
        private delegate void DiallingDelegate(string number,long channelID);
        public static IService SNChannelPredictive = null;
        string sIncomingNumber = string.Empty;

        #region Parameters for ActiveAgent/Call Report
        
        Thread thostActiveAgent = null;
        static int flag = 0;
        string strUriforActiveagent;
        public string myNumber = string.Empty;
        public string myStatus = string.Empty;
        int intTransferConfNumber = 1;
        string curNumber = string.Empty;
        string curStatus = string.Empty;
        bool isDashBoardLoaded = false;

        object objNetTcpActiveAgent = new NetP2PBootStrapActiveAgentReportDelegates();
        INetP2PBootStrapActiveAgentReportChannel channelNetTcpActiveAgent = null;

        public delegate void DelHangupAdmin(string phNumber);
        public DelHangupAdmin objHangupAdmin;

        public delegate void DelBargeReqest(string phNumber);
        public DelBargeReqest objBargeRequest;

        Hashtable hashNumberID = new Hashtable();
        Hashtable hashConfNumber = new Hashtable();
        
        #endregion


        VMuktiAPI.PeerType objPeerType;

        public enum CallStatus
        {
            NotInCall, CallInProgress, DialingToDest, RingingToDest,
            BusyTone, AMD, DTMF, DestUserStratListen, CallHangUp, CallHoldByAgent,
            CallHoldBySys, CallDispose, ReadyState
        };

        #region Constructor n Related Event

        public ctlPredictivePhone(VMuktiAPI.PeerType objLocalPeerType, string uri, ModulePermissions[] MyPermissions,string Role)
        {
            InitializeComponent();
            objPeerType = objLocalPeerType;
            try
            {
                btnReject.Click += new RoutedEventHandler(btnReject_Click);
                btnAccept.Click += new RoutedEventHandler(btnAccept_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                btnConference.Click += new RoutedEventHandler(btnConference_Click);
                btnTransfer.Click += new RoutedEventHandler(btnTransfer_Click);
                btnDTMF.Click += new RoutedEventHandler(btnDTMF_Click);
                btnCall.Click += new RoutedEventHandler(btnCall_Click);
                btnHangup.Click += new RoutedEventHandler(btnHangup_Click);
                btnHold.Click += new RoutedEventHandler(btnHold_Click);

                #region To add Channel Buttons
                btnChannels = new Button[6];
                int btnLeft = 20;
                int btnTop = 60;
                for (int i = 0; i < 6; i++)
                {
                    (btnChannels[i]) = new Button();
                    btnChannels[i].Style = (Style)this.Resources["SimpleButton"];
                    btnChannels[i].Height = 20;
                    btnChannels[i].Width = 35;
                    btnChannels[i].Content = (i + 1).ToString();
                    btnChannels[i].Name = "btnCh" + (i + 1).ToString();
                    btnChannels[i].Background = Brushes.Transparent;
                    btnChannels[i].Foreground = Brushes.Black;
                    btnChannels[i].FontSize = 13;
                    btnChannels[i].BorderThickness = new Thickness(0, 0, 0, 0);
                    btnChannels[i].Margin = new Thickness(btnLeft, btnTop, 0, 0);
                    btnLeft = btnLeft + 39;
                    btnChannels[i].Click += new RoutedEventHandler(ChClick);
                    btnChannels[i].Tag = "Free";
                    btnChannels[i].IsEnabled = false;
                    CnvPhoneProperty.Children.Add(btnChannels[i]);
                    TimeCounterCollection.Add(i, "0:00:00");
                }
                
                #endregion

                VMuktiHelper.RegisterEvent("RegisterAgent").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(FncRegister);
                VMuktiHelper.RegisterEvent("Dial").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventDial);
                VMuktiHelper.RegisterEvent("HangUp").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventHangUp);
                VMuktiHelper.RegisterEvent("TransferCall").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlPredictivePhoneTransferCall_VMuktiEvent);
                VMuktiHelper.RegisterEvent("BargeCall").VMuktiEvent+=new VMuktiEvents.VMuktiEventHandler(ctlPredictivePhoneBargeCall_VMuktiEvent);
                VMuktiHelper.RegisterEvent("HijeckCall").VMuktiEvent+=new VMuktiEvents.VMuktiEventHandler(ctlPredictivePhoneHijeckCal_VMuktiEvent);
                VMuktiHelper.RegisterEvent("UnMuteCall").VMuktiEvent+=new VMuktiEvents.VMuktiEventHandler(ctlPredictivePhoneUnMuteCall_VMuktiEvent);
                VMuktiHelper.RegisterEvent("StartManualDialing").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEventStartManualDialing);
                VMuktiHelper.RegisterEvent("SetSoftPhoneEnable").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEvent_SetSoftPhoneEnable);
                VMuktiHelper.RegisterEvent("SetIncomingNumber").VMuktiEvent+=new VMuktiEvents.VMuktiEventHandler(ctlDialer_VMuktiEvent_SetIncomingNumber);
                VMuktiHelper.RegisterEvent("HoldCall").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlPredictivePhone_VMuktiEvent);
                
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent +=new VMuktiEvents.VMuktiEventHandler(ctlPredictivePhone_VMuktiEvent_SingOut); 

                t1 = new System.Timers.Timer(1000);
                t1.Elapsed += new System.Timers.ElapsedEventHandler(t1_Elapsed);
                btnHold.IsEnabled = false;
                btnTransfer.IsEnabled = false;
                //this.Unloaded += new RoutedEventHandler(ctlDialer_Unloaded);
                
                objHangupAdmin = new DelHangupAdmin(HangupAdmin);
                objBargeRequest = new DelBargeReqest(BargeRequest);

                #region Thread for Register Client of ActiveAgent Report
                
                thostActiveAgent = new Thread(new ParameterizedThreadStart(hostActiveAgentService));
                List<object> lstParams = new List<object>();
                lstParams.Add("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveAgentReport");
                lstParams.Add("P2PActiveAgentMesh");
                thostActiveAgent.Start(lstParams);

                #endregion

                
                //calling its own registered event to disable soft phone at initial time
                VMuktiHelper.CallEvent("StartManualDialing", this, new VMuktiEventArgs(false, "Manual"));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlPredictivePhone()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void ctlDialer_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //VMuktiHelper.UnRegisterEvent("Dial");
                //VMuktiHelper.UnRegisterEvent("HangUp");
                //VMuktiHelper.UnRegisterEvent("StartManualDialing");
                //VMuktiHelper.UnRegisterEvent("SetSoftPhoneEnable");
                //VMuktiHelper.UnRegisterEvent("TransferCall");
                //VMuktiHelper.UnRegisterEvent("SetIncomingNumber");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDialer_Unloaded()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public ctlPredictivePhone(string agentNumber, string agentPass, string SIPServer)
        { 
        }

        #endregion

        #region Register client for ActiveAgent

        public void hostActiveAgentService(object lstparam)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstparam;
                strUriforActiveagent = lstTempObj[0].ToString();

                NetPeerClient npcActiveAgent = new NetPeerClient();
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcJoin(ctlPredictivePhone_EntsvcJoin);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcGetAgentInfo += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcGetAgentInfo(ctlPredictivePhone_EntsvcGetAgentInfo);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcSetAgentInfo += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcSetAgentInfo(ctlPredictivePhone_EntsvcSetAgentInfo);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcSetAgentStatus += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcSetAgentStatus(ctlPredictivePhone_EntsvcSetAgentStatus);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcBargeRequest += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcBargeRequest(ctlPredictivePhone_EntsvcBargeRequest);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcBargeReply += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcBargeReply(ctlPredictivePhone_EntsvcBargeReply);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcHangUp += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcHangUp(ctlPredictivePhone_EntsvcHangUp);
                ((NetP2PBootStrapActiveAgentReportDelegates)objNetTcpActiveAgent).EntsvcUnJoin += new NetP2PBootStrapActiveAgentReportDelegates.DelsvcUnJoin(ctlPredictivePhone_EntsvcUnJoin);

                channelNetTcpActiveAgent = (INetP2PBootStrapActiveAgentReportChannel)npcActiveAgent.OpenClient<INetP2PBootStrapActiveAgentReportChannel>(strUriforActiveagent, lstTempObj[1].ToString(), ref objNetTcpActiveAgent);
                channelNetTcpActiveAgent.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString());

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostActiveAgentService()", "ctlDialer.xaml.cs");
            }
        }

        #endregion

        #region UI Events

        void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strCallingType == "Manual")
                {
                    bool isFreeChannelAvail = false;
                    for (int i = 0; i < btnChannels.Length; i++)
                    {
                        if (btnChannels[i].Tag.ToString() == "Free")
                        {
                            ChClick(btnChannels[i], null);
                            btnChannels[Convert.ToInt16(strBtnChClick)].Tag = "Running";
                            btnChannels[Convert.ToInt16(strBtnChClick)].Background = Brushes.Green;
                            RClient.SetIncomingPhoneNumber(sIncomingNumber, intChannelNumber);
                            RClient.Anser(intChannelNumber);
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
                else if (strCallingType == "Predictive")
                {
                    RClient.SetIncomingPhoneNumber(sIncomingNumber, intChannelNumber);
                    RClient.Anser(intChannelNumber);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnAccept_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void btnReject_Click(object sender, RoutedEventArgs e)
        {
            
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
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void btnConference_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strConf = SNChannelPredictive.svcGetConferenceNumber();
                //Sending DTMF
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() != "Free")
                    {
                        for (int j = 0; j < strConf.Length; j++)
                        {
                            RClient.SendDTMF(strConf[j].ToString(), i + 1);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnConference_Click()", "ctlPredictivePhone.xaml.cs");
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
                        FncTransfer(lblNumber.Text.ToString(), long.Parse(intChannelNumber.ToString()));
                    }
                    else if (strCallingType == "Predictive")
                    {
                        for (int i = 0; i < btnChannels.Length; i++)
                        {
                            if (btnChannels[i].Tag.ToString() != "Free")
                            {
                                RClient.SendDTMF(lblNumber.Text.ToString(), i + 1);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnTransfer_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void btnDTMF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblNumber.Text.ToString() != "")
                {
                    if (strCallingType == "Manual")
                    {
                        RClient.SendDTMF(lblNumber.Text.ToString(), intChannelNumber);
                    }
                    else if (strCallingType == "Predictive")
                    {
                        for (int i = 0; i < btnChannels.Length; i++)
                        {
                            if (btnChannels[i].Tag.ToString() != "Free")
                            {
                                RClient.SendDTMF(lblNumber.Text.ToString(), i + 1);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnDTMF_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void btnCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnCall.Tag.ToString() == "Free" && strCallingType == "Manual")
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
                                btnChannels[Convert.ToInt16(strBtnChClick)].Background = Brushes.Green;
                                VMuktiHelper.CallEvent("SetChannelValues", this, new VMuktiEventArgs(lblNumber.Text.ToString(), DateTime.Now, DateTime.Now.ToString()));
                                FncCall(long.Parse(lblNumber.Text.ToString().Trim()), long.Parse(intChannelNumber.ToString()));
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
                else if (btnCall.Tag.ToString() == "Running" && strCallingType == "Manual")
                {
                    btnHangup_Click(null, null);
                }
                else if (btnCall.Tag.ToString() == "Running" && strCallingType == "Predictive")
                {
                    btnHangup_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCall_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void btnHangup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strCallingType == "Manual")
                {
                    btnChannels[intChannelNumber - 1].Tag = "Free";
                    btnChannels[intChannelNumber - 1].Background = Brushes.Transparent;
                    RClient.HangUp(Convert.ToInt32(intChannelNumber.ToString()));
                }
                else if (strCallingType == "Predictive")
                {
                    for (int i = 0; i < btnChannels.Length; i++)
                    {
                        if (btnChannels[i].Tag.ToString() != "Free")
                        {

                            RClient.HangUp(i + 1);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnHangup_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void btnHold_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strCallingType == "Manual")
                {
                    btnChannels[intChannelNumber - 1].Tag = "Hold";
                    btnChannels[intChannelNumber - 1].Background = Brushes.Orange;
                    FncHold(long.Parse(intChannelNumber.ToString()));
                    btnHold.IsEnabled = false;
                }
                else if (strCallingType == "Predictive")
                {
                    for (int i = 0; i < btnChannels.Length; i++)
                    {
                        if (btnChannels[i].Tag.ToString() != "Free")
                        {
                            FncHold(long.Parse((i + 1).ToString()));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnHold_Click()", "ctlPredictivePhone.xaml.cs");
            }
        }

        private void NumClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lblNumber.Text.ToString().StartsWith("Enter Number"))
                {
                    lblNumber.Text = "";
                }

                lblNumber.Text = lblNumber.Text.ToString() + ((Button)sender).Content.ToString();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NumClick()", "ctlPredictivePhone.xaml.cs");
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
                            RClient.Hold(Convert.ToInt16(i + 1), "Hold");
                            btnChannels[i].Background = Brushes.Orange;

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
                    RClient.Hold(Convert.ToInt16(intChannelNumber), "Hold");
                    ((Button)sender).Background = Brushes.Orange;
                    btnHold.IsEnabled = false;
                }
                else if (((Button)sender).Tag.ToString() == "Hold")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (btnChannels[i].Tag.ToString() == "Running" && btnChannels[i].Content != ((Button)sender).Content.ToString())
                        {
                            btnChannels[i].Tag = "Hold";
                            RClient.Hold(Convert.ToInt16(i + 1), "Hold");
                            btnChannels[i].Background = Brushes.Orange;
                        }
                    }

                    TimeSpan CallDurationTime = DateTime.Now - DateTime.Parse(TimeCounterCollection[intChannelNumber - 1].ToString());

                    t1.Start();
                    SecondCounter = CallDurationTime.Seconds;
                    MinCounter = CallDurationTime.Minutes;
                    HourCounter = CallDurationTime.Hours;


                    ((Button)sender).Tag = "Running";
                    RClient.Hold(Convert.ToInt16(intChannelNumber), "UnHold");
                    btnHold.IsEnabled = true;
                    ((Button)sender).Background = Brushes.Green;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ChClick()", "ctlPredictivePhone.xaml.cs");
            }
        }

        private void btnPad_Click(object sender, RoutedEventArgs e)
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
                VMuktiHelper.ExceptionHandler(ex, "btnPad_Click()", "ctlPredictivePhone.xaml.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "t1_Elapsed()", "ctlPredictivePhone.xaml.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "ChangeTime()", "ctlPredictivePhone.xaml.cs");
            }
        }

        #endregion

        #region WCF Events for ActiveAgent

        void ctlPredictivePhone_EntsvcJoin(string uName, string campaignId)
        {
            try
            { }
            catch (Exception ex)
            { }

        }

        void ctlPredictivePhone_EntsvcGetAgentInfo(string uNameHost)
        {
            try
            {
                if (uNameHost != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName && !(isDashBoardLoaded))
                {
                    string status = "";
                    string Phone_No = "";
                    if (myStatus != "")
                    {
                        status = curStatus;
                    }
                    else
                    {
                        status = "ready";
                    }
                    if (myNumber != "")
                    {
                        Phone_No = curNumber;
                    }
                    else
                    {
                        Phone_No = "NotRegisterd";
                    }
                    string CallDuration = "00:00:00";

                    channelNetTcpActiveAgent.svcSetAgentInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID.ToString(), status, VMuktiAPI.VMuktiInfo.CurrentPeer.GroupName, Phone_No, CallDuration,true);
                    
                    isDashBoardLoaded = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPredictivePhone_EntsvcGetAgentInfo()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void ctlPredictivePhone_EntsvcSetAgentInfo(string uName, string campaignId, string status, string groupName, string phoneNo, string CallDuration,bool isPredictive)
        {
            try
            { }
            catch { }
        }

        void ctlPredictivePhone_EntsvcSetAgentStatus(string uName, string Status, string Phone_No, string CallDuration,bool isPredictive)
        {
            try
            { }
            catch { }
        }

        void ctlPredictivePhone_EntsvcBargeRequest(string uName, string phoneNo)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uName)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objBargeRequest, phoneNo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPredictivePhone_EntsvcBargeRequest()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void ctlPredictivePhone_EntsvcBargeReply(string confNo)
        {
            try
            { }
            catch { }
        }

        void ctlPredictivePhone_EntsvcHangUp(string uName, string phoneNo)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uName)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objHangupAdmin, phoneNo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPredictivePhone_EntsvcHangUp()", "ctlPredictivePhone.xaml.cs");
            }
        }

        void ctlPredictivePhone_EntsvcUnJoin(string uName)
        {
            try
            { }
            catch { }
        }

        #endregion

        #region Cross Events n functions

        void RClient_entstatus(int ChannelId, string status, string PhNumber, string strHangUpModule)
        {
            try
            {
                switch (status)
                {
                    case "InPorgress":
                        btnCall.Tag = "Running";
                        if (strCallingType == "Predictive")
                        {
                            fncMute("Mute", ChannelId);
                            bool blIsCallRunning = false;
                            for (int i = 0; i < btnChannels.Length; i++)
                            {
                                if (btnChannels[i].Tag.ToString() != "Free")
                                {
                                    blIsCallRunning = true;
                                    break;
                                }
                            }
                            if (strCallingType == "Predictive" && blIsCallRunning)
                            {
                                intExtraCallChannelNumber = ChannelId - 1;
                            }

                            if (!hashNumberID.Contains(PhNumber))
                            {
                                hashNumberID.Add(PhNumber, ChannelId);
                            }
                        }

                        myStatus = "InProgress";
                        
                        if (PhNumber.Contains("@"))
                        {
                            myNumber = sIncomingNumber;
                        }
                        else
                        {
                        myNumber = PhNumber;
                        }
                        if (!isDashBoardLoaded)
                        {
                            if (PhNumber.Contains("@"))
                            {
                                curNumber = sIncomingNumber;
                            }
                            else
                            {
                            curNumber = PhNumber;
                            }
                            
                            curStatus = "InProgress";
                        }


                        channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, "00:00:00",true);
                        //System.Threading.Thread.Sleep(500);

                        break;


                    case "Connected":
                        btnCall.Tag = "Running";
                        btnCall.IsEnabled = true;
                        if (strCallingType == "Predictive")
                        {
                            bool IsCallRunning = false;
                            for (int i = 0; i < btnChannels.Length; i++)
                            {
                                if (btnChannels[i].Tag.ToString() != "Free")
                                {
                                    IsCallRunning = true;
                                    break;
                                }
                            }

                            if (!IsCallRunning)
                            {
                                myStatus = "Connected";

                                if (!isDashBoardLoaded)
                                {
                                    curNumber = PhNumber;
                                    curStatus = "Connected";
                                }

                                btnCall.IsEnabled = false;
                                fncMute("UnMute", ChannelId);
                                if (PhNumber.Contains("@"))
                                {
                                    lblStatus_Copy.Content = "Connected " + sIncomingNumber;
                                    myNumber = sIncomingNumber;
                                }
                                else
                                {
                                    lblStatus_Copy.Content = "Connected " + PhNumber;
                                    myNumber = PhNumber;
                                }
                                t1.Start();
                                if (TimeCounterCollection[ChannelId - 1].ToString() == "0:00:00")
                                {
                                    btnChannels[ChannelId - 1].Tag = "Running";
                                    TimeCounterCollection[ChannelId - 1] = DateTime.Now;
                                    PredictiveChannelNo = ChannelId;
                                    VMuktiHelper.CallEvent("SetDispositionEnable", this, new VMuktiEventArgs(false, PhNumber, (ChannelId - 1), strCallingType));
                                    VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallInProgress, ChannelId));

                                    if (PhNumber.Contains("@"))
                                    {
                                        if (!hashNumberID.Contains(sIncomingNumber))
                                        {
                                            hashNumberID.Add(sIncomingNumber, ChannelId);
                                        }
                                    }
                                    else
                                    {
                                    if (!hashNumberID.Contains(PhNumber))
                                    {
                                        hashNumberID.Add(PhNumber, ChannelId);
                                    }
                                }
                                }

                                channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, "Calculating...",true);
                                System.Threading.Thread.Sleep(500);
                            }
                            else
                            {
                                intExtraCallChannelNumber = ChannelId - 1;
                                blTransferRequired = true;
                                VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallInProgress, ChannelId));
                                myStatus = "Hold";
                                myNumber = PhNumber;

                                if (!hashNumberID.Contains(PhNumber))
                                {
                                    hashNumberID.Add(PhNumber, ChannelId);
                                }
                                
                                channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, "00:00:00", true);
                                System.Threading.Thread.Sleep(500);
                            }
                        }

                        else if (strCallingType == "Manual")
                        {
                            myStatus = "Connected";
                            myNumber = PhNumber;

                            fncMute("UnMute", ChannelId);
                            lblStatus_Copy.Content = "Connected " + PhNumber;
                            t1.Start();

                            if (TimeCounterCollection[ChannelId - 1].ToString() == "0:00:00")
                            {
                                TimeCounterCollection[ChannelId - 1] = DateTime.Now;
                            }

                            channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, "00:00:00",true);
                            System.Threading.Thread.Sleep(500);

                            if (!hashNumberID.Contains(PhNumber))
                            {
                                hashNumberID.Add(PhNumber, ChannelId);
                            }
                        }
                        break;


                    case "Disconnected":
                        bool IsConnected = false;
                        btnCall.Tag = "Free";
                        btnCall.IsEnabled = false;
                        myStatus = "Disconnected";

                        //channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, lblTime.Content.ToString(), true);
                        //System.Threading.Thread.Sleep(500);

                        if (strCallingType == "Predictive")
                        {
                            if (ChannelId - 1 != intExtraCallChannelNumber)
                            {
                                if (btnChannels[ChannelId - 1].Tag.ToString() == "Running")
                                {
                                    IsConnected = true;
                                }

                                if (strHangUpModule == "PredictiveDialer")
                                {
                                    VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallDispose, ChannelId));
                                    if (IsConnected)
                                    {
                                        btnChannels[ChannelId - 1].Background = Brushes.Transparent;
                                        TimeCounterCollection[ChannelId - 1] = "0:00:00";
                                        btnChannels[ChannelId - 1].Tag = "Free";
                                        lblNumber.Text = "";
                                        lblStatus_Copy.Content = "Disconnected";
                                        this.IsEnabled = false;
                                        t1.Stop();
                                        HourCounter = 0;
                                        MinCounter = 0;
                                        SecondCounter = 0;
                                        if (PhNumber.Contains("@"))
                                        {
                                            VMuktiHelper.CallEvent("SetDispositionEnable", this, new VMuktiEventArgs(true, sIncomingNumber, (ChannelId - 1), strCallingType));
                                            myNumber = sIncomingNumber;
                                            hashNumberID.Remove(sIncomingNumber);
                                        }
                                        else
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
                                            VMuktiHelper.CallEvent("SetDispositionEnable", this, new VMuktiEventArgs(true, PhNumber, (ChannelId - 1), strCallingType));
                                            myNumber = PhNumber;
                                            hashNumberID.Remove(PhNumber);
                                        }
                                    }
                                    }
                                    else
                                    {
                                        VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(8, "", false, null, (ChannelId - 1).ToString()));
                                    }
                                }
                                else
                                {
                                    VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallDispose, ChannelId));
                                    VMuktiHelper.CallEvent("SetDispositionEnable", this, new VMuktiEventArgs(false, PhNumber, (ChannelId - 1), strCallingType));
                                    VMuktiHelper.CallEvent("FireNextCallEvent", this, new VMuktiEventArgs());

                                    hashNumberID.Remove(PhNumber);
                                }
                            }
                            else
                            {
                                if (blTransferRequired)
                                {
                                    intExtraCallChannelNumber = -1;
                                    VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallHangUp, ChannelId));
                                    hashNumberID.Remove(PhNumber);
                                }
                                else
                                {
                                    //Enter Static dispositon(because Hangup type are Answering machine, busy fax etc)
                                    intExtraCallChannelNumber = -1;
                                    VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(8, "", false, null, (ChannelId - 1).ToString()));
                                    VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallHangUp, ChannelId));
                                }
                            }
                        }
                        else if (strCallingType == "Manual")
                        {
                            lblNumber.Text = "";
                            lblStatus_Copy.Content = "Disconnected";
                            btnHold.IsEnabled = false;
                            btnHangup.IsEnabled = false;
                            btnTransfer.IsEnabled = false;
                            btnCall.IsEnabled = true;
                            btnChannels[ChannelId - 1].Tag = "Free";
                            btnChannels[ChannelId - 1].Background = Brushes.Transparent;
                            TimeCounterCollection[ChannelId - 1] = "0:00:00";
                            
                            if (intChannelNumber == ChannelId)
                            {
                                t1.Stop();
                                HourCounter = 0;
                                MinCounter = 0;
                                SecondCounter = 0;
                            }

                            hashNumberID.Remove(PhNumber);
                        }

                        channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, lblTime.Content.ToString(),true);
                       // System.Threading.Thread.Sleep(500);

                        break;

                    case "Incoming":

                        btnCall.Tag = "Incoming";
                        btnAccept_Click(null, null);

                        myStatus = "Incoming";
                        myNumber = PhNumber;

                        channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, lblTime.Content.ToString(),true);
                        System.Threading.Thread.Sleep(500);

                        break;

                    case "Hold":

                        lblStatus_Copy.Content = "OnHold";
                        blIsOnHold = true;

                        myStatus = "Hold";
                        myNumber = PhNumber;

                        channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, lblTime.Content.ToString(),true);
                        System.Threading.Thread.Sleep(500);

                        break;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RClient_entstatus()", "ctlPredictivePhone.xaml.cs");
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
            catch (Exception ex)
            {
                return null;
            }
        }

        void HangupAdmin(string phNumber)
        {
            try
            {
                    if (hashNumberID.Contains(phNumber))
                    {
                        string strChannelID = hashNumberID[phNumber].ToString();
                        RClient.HangUp(Convert.ToInt32(strChannelID));
                    }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HangupAdmin()", "ctlDialer.xaml.cs");
            }
        }

        void BargeRequest(string phNumber)
        {
            try
            {
                string strPhNumber = string.Empty;

                if (strNumber.Contains("@"))
                {
                    strPhNumber = sIncomingNumber;
                }
                else
                {
                    strPhNumber = phNumber;
                }

                
                if (phNumber == strPhNumber)
                {
                    
                    //check if requested no for barg is transfered no or direct no
                    ///If its transfered no then directly return conf number to requested admin
                    ///If its direct no then fire one conf on same channel n return conf no to requested admin

                    if (hashConfNumber.Contains(phNumber))
                    {

                        channelNetTcpActiveAgent.svcBargeReply(hashConfNumber[phNumber].ToString());
                    }
                    else
                    {


                        long channelID = long.Parse(hashNumberID[phNumber].ToString());

                        FncBarge("####7" + intTransferConfNumber.ToString() + "#", channelID);

                        string strConf = "7" + intTransferConfNumber.ToString() + "@" + strSIPServerIP;

                        if (intTransferConfNumber > 10)
                        {
                            intTransferConfNumber = 0;
                        }
                        else
                        {
                            intTransferConfNumber += 1;
                        }

                        channelNetTcpActiveAgent.svcBargeReply(strConf);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void FncBarge(string Number, long channelID)
        {
            //RClient.Mute("UnMute", Convert.ToInt16(channelID));
            for (int j = 0; j < Number.Length; j++)
            {
                RClient.SendDTMF(Number[j].ToString(), Convert.ToInt16(channelID));
            }

            //RClient.Mute("Mute", Convert.ToInt16(channelID));
            //RClient.HangUp(Convert.ToInt16(channelID));
        }

        #endregion

        #region Predictive Dialer Register Events

        private delegate void HumanDetectedDelegate(int channel, string tone);

        void ctlDialer_VMuktiEventDial(object sender, VMuktiEventArgs e)
        {
            RClient.SetHangUpModuleName(int.Parse(e._args[1].ToString()), "PredictiveDialer");
            
            FncCall1(e._args[0].ToString(), long.Parse(e._args[1].ToString()));
        }

        void ctlDialer_VMuktiEventHangUp(object sender, VMuktiEventArgs e)
        {
            string strModuleName = e._args[0].ToString();
            RClient.SetHangUpModuleName(int.Parse(e._args[1].ToString()), strModuleName);

            if (strCallingType == "Manual")
            {
                for (int i = 0; i < btnChannels.Length; i++)
                {
                    if (btnChannels[i].Tag.ToString() != "Free")
                    {
                        RClient.HangUp(i + 1);                        
                    }
                }
            }
            else
            {
                FncHangUp(long.Parse(e._args[1].ToString()));
            }
           
        }

        void ctlPredictivePhone_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            FncHold(long.Parse(e._args[0].ToString()));
        }

        void ctlPredictivePhoneTransferCall_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            FncTransfer(e._args[1].ToString(), long.Parse(e._args[0].ToString()));
        }

        void ctlPredictivePhoneBargeCall_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            FncBargeCall(e._args[1].ToString(), long.Parse(e._args[0].ToString()));
        }

        void ctlPredictivePhoneHijeckCal_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            FncHijeckCall(e._args[1].ToString(), long.Parse(e._args[0].ToString()));
        }

        void ctlPredictivePhoneUnMuteCall_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            fncMute("UnMute", long.Parse(e._args[0].ToString()));
            intExtraCallChannelNumber = -1;
            btnCall.Tag = "Running";
            btnCall.IsEnabled = true;
            lblStatus_Copy.Content = "Connected " + e._args[1].ToString();
            t1.Start();
            if (TimeCounterCollection[int.Parse(e._args[0].ToString()) - 1].ToString() == "0:00:00")
            {
                btnChannels[int.Parse(e._args[0].ToString()) - 1].Tag = "Running";
                TimeCounterCollection[int.Parse(e._args[0].ToString()) - 1] = DateTime.Now;
                PredictiveChannelNo = int.Parse(e._args[0].ToString());
            }
            
            VMuktiHelper.CallEvent("SetChannelStatusForPredictive", this, new VMuktiEventArgs(CallStatus.CallInProgress, e._args[0].ToString()));

            myStatus = "Connected";
            myNumber = e._args[1].ToString();

            channelNetTcpActiveAgent.svcSetAgentStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myStatus, myNumber, "Calculating...", true);
        }

        void ctlDialer_VMuktiEventStartManualDialing(object sender, VMuktiEventArgs e)
        {
            if (e._args[1].ToString() == "Manual")
            {
                CnvDialer.IsEnabled = Convert.ToBoolean(e._args[0]);
                //CnvDialer.IsEnabled = true;
                btnCall.IsEnabled = Convert.ToBoolean(e._args[0]);
                btnHangup.IsEnabled = true;
               // btnStatus.IsEnabled = true;
                
                for (int i = 0; i < 6; i++)
                {
                    btnChannels[i].IsEnabled = Convert.ToBoolean(e._args[0]);
                }
            }
            else if (e._args[1].ToString() == "Predictive") 
            {
                    CnvDialer.IsEnabled = Convert.ToBoolean(e._args[0]);
                    btnHangup.IsEnabled = true;
            }
            strCallingType = e._args[1].ToString();
        }

        void ctlDialer_VMuktiEvent_SetSoftPhoneEnable(object sender, VMuktiEventArgs e)
        {
            //btnChannels[int.Parse(e._args[1].ToString())].Tag = "Free";
            this.IsEnabled = bool.Parse(e._args[0].ToString());
        }

        void ctlPredictivePhone_VMuktiEvent_SingOut(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("RegisterAgent");
                VMuktiHelper.UnRegisterEvent("Dial");
                VMuktiHelper.UnRegisterEvent("HangUp");
                VMuktiHelper.UnRegisterEvent("TransferCall");
                VMuktiHelper.UnRegisterEvent("BargeCall");
                VMuktiHelper.UnRegisterEvent("HijeckCall");
                VMuktiHelper.UnRegisterEvent("UnMuteCall");
                VMuktiHelper.UnRegisterEvent("StartManualDialing");
                VMuktiHelper.UnRegisterEvent("SetSoftPhoneEnable");
                VMuktiHelper.UnRegisterEvent("SetIncomingNumber");
                VMuktiHelper.UnRegisterEvent("HoldCall");

                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPredictivePhone_VMuktiEvent_SingOut()", "ctlDialer.xaml.cs");
            }
        }
        
        void ctlDialer_VMuktiEvent_SetIncomingNumber(object sender, VMuktiEventArgs e)
        {
            try
            {
                sIncomingNumber = e._args[0].ToString();

                intChannelNumber = int.Parse(e._args[1].ToString());

                if (!hashConfNumber.Contains(e._args[0].ToString()))
                {
                    hashConfNumber.Add(e._args[0].ToString(), e._args[2].ToString());
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDialer_VMuktiEvent_SetIncomingNumber()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                VMuktiHelper.UnRegisterEvent("RegisterAgent");
                VMuktiHelper.UnRegisterEvent("Dial");
                VMuktiHelper.UnRegisterEvent("HangUp");
                VMuktiHelper.UnRegisterEvent("TransferCall");
                VMuktiHelper.UnRegisterEvent("BargeCall");
                VMuktiHelper.UnRegisterEvent("HijeckCall");
                VMuktiHelper.UnRegisterEvent("UnMuteCall");
                VMuktiHelper.UnRegisterEvent("StartManualDialing");
                VMuktiHelper.UnRegisterEvent("SetSoftPhoneEnable");
                VMuktiHelper.UnRegisterEvent("SetIncomingNumber");
                VMuktiHelper.UnRegisterEvent("HoldCall");

                channelNetTcpActiveAgent.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

                if (channelNetTcpActiveAgent != null)
                {
                    channelNetTcpActiveAgent.Close();
                    channelNetTcpActiveAgent.Dispose();
                    channelNetTcpActiveAgent = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlDialer.xaml.cs");
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

        public void FncRegister(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.strExternalPBX == "true")
                {
                    try
                    {
                        RClient = new RTCClient(e._args[0].ToString(), e._args[1].ToString(), e._args[2].ToString());
                        lblUserNumber.Content = e._args[0].ToString() + "@" + e._args[2].ToString();
                        RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
                        RClient.NonHumanDetected += OnNonHumanDetected;
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncRegister()--1", "ctlDialer.xaml.cs");
                    }
                }
                else
                {
                    string strSIPNumber = "";
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                    {
                        VMuktiService.BasicHttpClient bhcSuperNode = new VMuktiService.BasicHttpClient();
                        SNChannelPredictive = (IService)bhcSuperNode.OpenClient<IService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/SNServicePredictive");
                        strSIPServerIP = VMuktiInfo.CurrentPeer.SuperNodeIP;
                        strSIPNumber = SNChannelPredictive.svcAddSIPUser();
                    }
                    else
                    {
                        strSIPNumber = FncAddSIPUser();
                        strSIPServerIP = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP.ToString();
                    }
                    VMuktiHelper.CallEvent("SetAgentNumber", this, new VMuktiEventArgs(strSIPNumber, strSIPNumber, strSIPServerIP));
                    RClient = new RTCClient(strSIPNumber, strSIPNumber, strSIPServerIP);
                    lblUserNumber.Content = strSIPNumber + "@" + strSIPServerIP;
                    RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
                    RClient.NonHumanDetected += OnNonHumanDetected;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncRegister()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void FncCall(long number, long channelID)
        {
            DiallingDelegate DialChange = new DiallingDelegate(UpdateDialing);
            try
            {
                strNumber = number.ToString();
                lblStatus_Copy.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, DialChange, strNumber, channelID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncCall()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void FncCall1(string number, long channelID)
        {
            DiallingDelegate DialChange = new DiallingDelegate(UpdateDialing);
            try
            {
                strNumber = number.ToString();
                lblStatus_Copy.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, DialChange, strNumber, channelID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncCall1()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void UpdateDialing(string number, long channelID)
        {
            try
            {
                RClient.Dial(number.ToString(), Convert.ToInt32(channelID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UpdateDialing()", "ctlPredictivePhone.xaml.cs");
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
                            RClient.HangUp(i + 1);
                        }
                    }
                }
                else if (strCallingType == "Predictive" && (lblStatus_Copy.Content.ToString() == "OnHold" || lblStatus_Copy.Content.ToString().StartsWith("Connected")))
                {
                    RClient.HangUp(Convert.ToInt32(channelID));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncHangUp()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void fncMute(string Status, long ChannelID)
        {
            try
            {
                RClient.Mute(Status, Convert.ToInt32(ChannelID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncMute()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void FncHold(long channelID)
        {
            try
            {
                RClient.Hold(Convert.ToInt32(channelID), btnHold.Content.ToString());
                if (btnHold.Content.ToString() == "Hold" && strCallingType == "Predictive")
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
                VMuktiHelper.ExceptionHandler(ex, "FncHold()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void FncTransfer(string Number,long channelID)
        {
            try
            {
                RClient.Mute("UnMute", Convert.ToInt16(channelID));
                for (int j = 0; j < Number.Length; j++)
                {
                    RClient.SendDTMF(Number[j].ToString(), Convert.ToInt16(channelID));
                    //System.Threading.Thread.Sleep(1000);
                }
                System.Threading.Thread.Sleep(500);
                RClient.Mute("Mute", Convert.ToInt16(channelID));
                RClient.HangUp(Convert.ToInt16(channelID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncTransfer()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void FncBargeCall(string Number, long channelID)
        {
            try
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    RClient.SendDTMF(Number[j].ToString(), Convert.ToInt16(channelID));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncBargeCall()", "ctlPredictivePhone.xaml.cs");
            }
        }

        public void FncHijeckCall(string Number, long channelID)
        {
            try
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    RClient.SendDTMF(Number[j].ToString(), Convert.ToInt16(channelID));
                }
                RClient.HangUp(Convert.ToInt16(channelID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncHijeckCall()", "ctlPredictivePhone.xaml.cs");
            }
        }
        #endregion

    }
}