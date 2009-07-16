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
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Chat.Business.Service.BasicHttp;
using Chat.Business.Service.DataContracts;
using Chat.Business.Service.NetP2P;
using VMuktiAPI;
using VMuktiService;
using System.ComponentModel;

namespace Chat.Presentation
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

    public partial class ctlChat : UserControl, IDisposable
    {
        #region Variables

        System.Threading.Thread staGlobal;
        
        object objNetTcpChat;
        INetTcpChatChannel channelNetTcp;
        IHttpChat channelHttp;

        System.Windows.Threading.DispatcherTimer dispTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);

        object objHttpChat;

        //List<clsMessage> lstMessage;

        //System.Threading.Thread tHostChat = null;

        public delegate void DelSendMessage(List<object> lstMsg);
        public DelSendMessage objDelSendMsg;

        int temp;
        int tempcounter;

        public delegate void DelDisplayName(string lstUserName);
        public DelDisplayName objDelDisplayName;

        public delegate void DelSignOutMessage(List<object> lstMsg);
        public DelSignOutMessage objDelSignOutMsg;

        public delegate void DelSendPeerMessage(string Msg);
        public DelSendPeerMessage objDelSendPeerMsg;

        public delegate void DelGetUserList();
        public DelGetUserList objDelGetUserList;

        public delegate void DelAsyncGetMessage(List<clsMessage> myMessages);
        public DelAsyncGetMessage objDelAsyncGetMessage;

        public delegate void DelShowUserTypeing(string uName, string keyStatus);
        public DelShowUserTypeing objDelUserTypeing;

        public delegate void DelTextDownEvent(bool isKeyDown);
        public DelTextDownEvent objDelTextDownEvent;

        public string strUri;

        //ModulePermissions[] modPer;

        System.Timers.Timer tTypingStatus = new System.Timers.Timer();

        bool blnTypingStatus;
        bool blnSendTypeMsg;

       // byte[] arr;

        List<string> lstName;
        List<string> lstTypingUsers;

        public string strRole;

        #region Win32

        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        [DllImport("Kernel32.dll")]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        [DllImport("user32.dll")]
        static extern bool FlashWindowEx(IntPtr hWnd);

        #endregion

        #region Chat History

        public string chatHistoryPath;
        StreamWriter chatHistoryWriter;
        public string chatHistoryFileName;
        bool isChatHistoryFolderCreated;
        bool isChatHistoryEnabled;
        ctlViewHistory objViewHistory;

        #endregion

        BackgroundWorker bgHostService;

        #endregion

        #region Constructor

        public ctlChat(VMuktiAPI.PeerType PeerType, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                InitializeComponent();

                staGlobal = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalVariable));
                staGlobal.Start();

                VMuktiAPI.ClsException.WriteToLogFile("chat:- " + uri); 

                bgHostService = new BackgroundWorker();

                this.Unloaded += new RoutedEventHandler(ctlChat_Unloaded);
                txtChat.KeyDown += new KeyEventHandler(txtChat_KeyDown);
                txtChat.TextChanged += new TextChangedEventHandler(txtChat_TextChanged);
                txtChat.AcceptsReturn = true;
                objDelDisplayName = new DelDisplayName(DisplayName);

                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(ctlChat_VMuktiEvent);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                //modPer = MyPermissions;

                //objDelSendMsg = new DelSendMessage(delSendMessage);
                //objDelSignOutMsg = new DelSignOutMessage(delSignoutMessage);

                //objDelSendPeerMsg = new DelSendPeerMessage(delSendPeerMessage);
                //objDelGetUserList = new DelGetUserList(delGetUserList);
                //objDelUserTypeing = new DelShowUserTypeing(delShowUserTypeMessage);
                //objDelTextDownEvent = new DelTextDownEvent(delTextDownEvent);
                //objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);
               
                tTypingStatus.Interval = 3000;
                tTypingStatus.Elapsed += new System.Timers.ElapsedEventHandler(tTypingStatus_Elapsed);
               

                strRole = Role;
                strUri = uri;

                bgHostService.DoWork += new DoWorkEventHandler(bgHostService_DoWork);

                // tHostChat = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostchatservice));
                List<object> lstParams = new List<object>();
                lstParams.Add(PeerType);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);

                bgHostService.RunWorkerAsync(lstParams);

                //tHostChat.Start(lstParams);

                #region Chat History

                chkChatHistory.Unchecked += new RoutedEventHandler(chkChatHistory_Unchecked);
                chkChatHistory.Checked += new RoutedEventHandler(chkChatHistory_Checked);
                btnViewHistory.Click += new RoutedEventHandler(btnViewHistory_Click);

                chatHistoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                Directory.CreateDirectory(chatHistoryPath + "\\VMukti\\Chat History\\Logs for " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                chatHistoryPath += "\\VMukti\\Chat History\\Logs for " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                #endregion


                #region resize

                this.Loaded += new RoutedEventHandler(ctlChat_Loaded);
              this.SizeChanged+=new SizeChangedEventHandler(this_SizeChanged);
                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlChat()", "ctlChat.xaml.cs");
            }
        }

        void GlobalVariable()
        {
            try
            {
                objNetTcpChat = new clsNetTcpChat();
                objHttpChat = new clsHttpChat();
                temp = 0;
                tempcounter = 0;
                blnTypingStatus = false;
                blnSendTypeMsg = true;
                //arr = new byte[5000];
                lstName = new List<string>();
                lstTypingUsers = new List<string>();
                isChatHistoryFolderCreated = false;
                isChatHistoryEnabled = true;


                objDelDisplayName = new DelDisplayName(DisplayName);

                objDelSendMsg = new DelSendMessage(delSendMessage);
                objDelSignOutMsg = new DelSignOutMessage(delSignoutMessage);

                objDelSendPeerMsg = new DelSendPeerMessage(delSendPeerMessage);
                objDelGetUserList = new DelGetUserList(delGetUserList);
                objDelUserTypeing = new DelShowUserTypeing(delShowUserTypeMessage);
                objDelTextDownEvent = new DelTextDownEvent(delTextDownEvent);
                objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("GlobalVariable" +  ex.Message);
            }
        }

        #endregion

        #region BWHostService

        void bgHostService_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> lstTempObj = (List<object>)e.Argument;
            strUri = lstTempObj[1].ToString();
            try
            {
                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcChat = new NetPeerClient();
                    ((clsNetTcpChat)objNetTcpChat).EntsvcJoin += new clsNetTcpChat.delsvcJoin(wndChat_EntsvcJoin);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcSendMessage += new clsNetTcpChat.delsvcSendMessage(wndChat_EntsvcSendMessage);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcGetUserList += new clsNetTcpChat.delsvcGetUserList(ctlChat_EntsvcGetUserList);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcSetUserList += new clsNetTcpChat.delsvcSetUserList(ctlChat_EntsvcSetUserList);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcSignOutChat += new clsNetTcpChat.delsvcSignOutChat(ctlChat_EntsvcSignOutChat);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcUnJoin += new clsNetTcpChat.delsvcUnJoin(wndChat_EntsvcUnJoin);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcShowStatus += new clsNetTcpChat.delsvcShowStatus(ctlChat_EntsvcShowStatus);

                    channelNetTcp = (INetTcpChatChannel)npcChat.OpenClient<INetTcpChatChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpChat);

                    while (temp < 20)
                    {
                        try
                        {
                            channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

                            temp = 20;

                            channelNetTcp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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

                    BasicHttpClient bhcChat = new BasicHttpClient();
                    channelHttp = (IHttpChat)bhcChat.OpenClient<IHttpChat>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            channelHttp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;

                            channelHttp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

                            txtChat.IsEnabled = true;
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }


                    dispTimer.Interval = TimeSpan.FromSeconds(2);
                    dispTimer.Tick += new EventHandler(dispTimer_Tick);
                    dispTimer.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "hostchatservice()", "ctlChat.xaml.cs");
            }
        }

        #endregion

        #region UI Event Handlers

        void txtChat_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    
                    string strTemp = txtChat.Text.Replace("\r\n", "");
                    if (strTemp.Length > 0)
                    {
                        btnChat_Click(null, null);
                        //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelTextDownEvent, false);
                        //blnTypingStatus = false;
                        //blnSendTypeMsg = true;
                        //tTypingStatus.Stop();
                    }
                    

                }

                else
                {
                    if (blnSendTypeMsg)
                    {
                        if (lstName != null && lstName.Count > 0)
                        {
                            //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelTextDownEvent, true);
                            //blnSendTypeMsg = false;
                            //tTypingStatus.Start();
                        }
                    }
                }
                blnTypingStatus = true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtChat_KeyDown", "ctlChat.xaml.cs");                
            }
        }

        void txtChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtChat.Text.EndsWith("\r\n"))
                {
                    string strTemp = txtChat.Text.Replace("\r\n", "");
                    if (strTemp.Length > 0)
                    {
                        btnChat_Click(null, null);
                        //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelTextDownEvent, false);
                        //blnTypingStatus = false;
                        //blnSendTypeMsg = true;
                        //tTypingStatus.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtChat_TextChanged()", "ctlChat.xaml.cs");
            }
        }

        void btnChat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (txtChat.Text != "")
                if(!string.IsNullOrEmpty(txtChat.Text))
                {
                    string strTemp = "";
                    if (txtChat.Text.EndsWith("\r\n"))
                    {
                        strTemp = txtChat.Text.Replace("\r\n", "");
                    }
                    else
                    {
                        strTemp = txtChat.Text;
                    }

                    if (strTemp.Length > 0)
                    {
                        rtbChat.AppendText(Char.ConvertFromUtf32(13) + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + " Says : " + strTemp);

                        //Added for Chat History
                        WriteChatLog(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + " Said at [" + DateTime.Now.ToString() + "] : " + strTemp);

                        rtbChat.ScrollToEnd();

                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objDelSendPeerMsg, strTemp);

                        //ClsException.WriteToLogFile("Chat Module: Chat Send Button Clicked");
                        //ClsException.WriteToLogFile("Msg is : " + strTemp);
                        //ClsException.WriteToLogFile("Receiver's Name : ");

                        for (int i = 0; i < lstName.Count; i++)
                        {
                            ClsException.WriteToLogFile(lstName[i]);
                        }
                      
                        txtChat.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "", "ctlChat.xaml.cs");
            }
        }

        void ctlChat_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (dispTimer != null)
                //{
                //    dispTimer.Stop();
                //}

                //if (channelHttp != null)
                //{
                //    channelHttp = null;
                //}
                //if (channelNetTcp != null && channelNetTcp.State == CommunicationState.Opened)
                //{
                //    channelNetTcp = null;
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlChat_Unloaded()", "ctlChat.xaml.cs");
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                }
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null && channelNetTcp.State == CommunicationState.Opened)
                {
                    channelNetTcp = null;
                }
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SignOut");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Current_Exit", "ctlChat.xaml.cs");                
            }
        }

        public void ctlChat_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlChat_VMuktiEvent", "ctlChat.xaml.cs");                
            }
        }

        #region Chat History

        void chkChatHistory_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                isChatHistoryEnabled = true;
                chkChatHistory.Content = "Chat History Enabled";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "chkChatHistoy_Checked", "ctlChat.xaml.cs");                
            }
        }

        void chkChatHistory_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                isChatHistoryEnabled = false;
                chkChatHistory.Content = "Chat History Disabled";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "chkChatHistoy_Unchecked", "ctlChat.xaml.cs");
            }
        }

        void btnViewHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                objViewHistory = new ctlViewHistory();
                objViewHistory.EntClose += new ctlViewHistory.DelClose(obj_EntClose);
             
                //changes
                rtbChat.Visibility = Visibility.Collapsed;
                lblDisplay.Visibility = Visibility.Collapsed;
                scrStatus.Visibility = Visibility.Collapsed;
                chkChatHistory.Visibility = Visibility.Collapsed;    
                grdMain.Children.Add(objViewHistory);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnViewHistory_Click()", "ctlChat.xaml.cs");
            }
        }

        void obj_EntClose()
        {
            try
            {
               
                //changes
                grdMain.Children.Remove(objViewHistory);
                rtbChat.Visibility = Visibility.Visible;
                lblDisplay.Visibility = Visibility.Visible;
                scrStatus.Visibility = Visibility.Visible;
                chkChatHistory.Visibility = Visibility.Visible;
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "obj_EntClose()", "ctlChat.xaml.cs");
            }
        }

        #endregion

        #endregion

        #region Timers,Threads and related Methods

        public void hostchatservice(object lstParams)
        {
            List<object> lstTempObj = (List<object>)lstParams;
            strUri = lstTempObj[1].ToString();
            try
            {
                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcChat = new NetPeerClient();
                    ((clsNetTcpChat)objNetTcpChat).EntsvcJoin += new clsNetTcpChat.delsvcJoin(wndChat_EntsvcJoin);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcSendMessage += new clsNetTcpChat.delsvcSendMessage(wndChat_EntsvcSendMessage);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcGetUserList += new clsNetTcpChat.delsvcGetUserList(ctlChat_EntsvcGetUserList);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcSetUserList += new clsNetTcpChat.delsvcSetUserList(ctlChat_EntsvcSetUserList);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcSignOutChat += new clsNetTcpChat.delsvcSignOutChat(ctlChat_EntsvcSignOutChat);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcUnJoin += new clsNetTcpChat.delsvcUnJoin(wndChat_EntsvcUnJoin);
                    ((clsNetTcpChat)objNetTcpChat).EntsvcShowStatus += new clsNetTcpChat.delsvcShowStatus(ctlChat_EntsvcShowStatus);

                    channelNetTcp = (INetTcpChatChannel)npcChat.OpenClient<INetTcpChatChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpChat);
                   
                    while (temp < 20)
                    {
                        try
                        {
                            channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                           
                            temp = 20;
                           
                            channelNetTcp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        }
                        catch
                        {
                            temp++;
                            System.Threading.Thread.Sleep(1000);
                        }

                    }

                    //ClsException.WriteToLogFile("Chat Module:Opened Chat P2P Client");
                    //ClsException.WriteToLogFile("Uri for opening the client is : " + strUri);
                    //ClsException.WriteToLogFile("MeshId for opening the client is : " + strUri.ToString().Split(':')[2].Split('/')[1]); 
                }
                else
                {
                   
                    BasicHttpClient bhcChat = new BasicHttpClient();
                    channelHttp = (IHttpChat)bhcChat.OpenClient<IHttpChat>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            channelHttp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;
                            
                            channelHttp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            
                            txtChat.IsEnabled = true;
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    dispTimer.Interval = TimeSpan.FromSeconds(2);
                    dispTimer.Tick += new EventHandler(dispTimer_Tick);
                    dispTimer.Start();

                    //ClsException.WriteToLogFile("chat module: Opened Http Chat Client with Timers");
                    //ClsException.WriteToLogFile("Uri for opening the client is : " + strUri);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "hostchatservice()", "ctlChat.xaml.cs");
            }
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();

                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();

               
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "dispTimer_Tick()", "ctlChat.xaml.cs");
            }
        }

        void OnCompletion(IAsyncResult result)
        {
            try
            {
                List<clsMessage> objMsgs = channelHttp.EndsvcGetMessages(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetMessage, objMsgs);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OnCompletion()", "ctlChat.xaml.cs");
            }

        }

        void StartThread()
        {
            try
            {
               
                if (channelHttp != null)
                {
                    channelHttp.BeginsvcGetMessages(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnCompletion, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "StartThread()", "ctlChat.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                //call unjoin method

                if (channelNetTcp != null)
                {
                    channelNetTcp.svcSignOutChat(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName);
                }
                else if (channelHttp != null)
                {
                    channelHttp.svcSignOutChat(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName);
                }

                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                }

                Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlChat.xaml.cs");
            }
        }

        public void DisplayName(string lstUserName)
        {
            try
            {
                bool flag = true;
                for (int i = 0; i < lstName.Count; i++)
                {
                    if (lstName[i] == lstUserName)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    lstName.Add(lstUserName);

                    #region Chat History

                    if (!isChatHistoryFolderCreated)
                    {
                        if (System.IO.Directory.Exists(chatHistoryPath + "\\" + lstUserName))
                        {
                            chatHistoryPath += "\\" + lstUserName;
                            isChatHistoryFolderCreated = true;
                        }

                        else
                        {
                            System.IO.Directory.CreateDirectory(chatHistoryPath + "\\" + lstUserName);
                            chatHistoryPath += "\\" + lstUserName;
                            isChatHistoryFolderCreated = true;
                        }
                    }

                    chatHistoryFileName = DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt";

                    if (File.Exists(chatHistoryPath + "\\" + chatHistoryFileName))
                    {
                        chatHistoryWriter = File.AppendText(chatHistoryPath + "\\" + chatHistoryFileName);
                    }
                    else
                    {
                        chatHistoryWriter = File.CreateText(chatHistoryPath + "\\" + chatHistoryFileName);
                    }

                    if (lstName.Count.Equals(1))
                    {
                        chatHistoryWriter.WriteLine("--------------------------------------------------");
                        chatHistoryWriter.WriteLine("Chat started at " + DateTime.Now.ToString() + " with " + lstName[lstName.Count - 1]);
                        chatHistoryWriter.WriteLine("--------------------------------------------------");
                        chatHistoryWriter.Flush();
                        chatHistoryWriter.Close();
                    }

                    else
                    {
                        chatHistoryWriter.WriteLine("--------------------------------------------------");
                        chatHistoryWriter.WriteLine(lstName[lstName.Count - 1] + " has joined the chat at " + DateTime.Now.ToString());
                        chatHistoryWriter.WriteLine("--------------------------------------------------");
                        chatHistoryWriter.Flush();
                        chatHistoryWriter.Close();
                    }

                    #endregion

                    //if (lstUserName != null || lstUserName != "")
                    if(!string.IsNullOrEmpty(lstUserName))
                    {
                        //if (lblDisplay.Content != null && lblDisplay.Content.ToString() != "")
                        if(lblDisplay.Content != null && !string.IsNullOrEmpty(lblDisplay.Content.ToString()))
                        {
                            lblDisplay.Content += "  " + lstUserName;
                        }
                        else
                        {
                            lblDisplay.Content = lstUserName;
                        }
                    }
                }

                Beep(800, 150);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DisplayName()", "ctlChat.xaml.cs");
            }
        }

        void OnCompletionShowStatus(IAsyncResult result)
        {
            try
            {
                channelHttp.EndsvcShowStatus(result);
                result.AsyncWaitHandle.Close();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OnCompletionShowStatus()", "ctlChat.xaml.cs");
            }

        }

        void tTypingStatus_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (lstName.Count > 0)
                {
                    if (!blnTypingStatus)
                    {
                        tTypingStatus.Stop();
                        if (channelHttp != null)
                        {
                            channelHttp.svcShowStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Entered");
                        }
                        if (channelNetTcp != null)
                        {
                            channelNetTcp.svcShowStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Entered");
                        }
                        blnSendTypeMsg = true;
                        blnTypingStatus = true;
                    }
                    else
                    {
                        blnTypingStatus = false;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "tTypingStatus_Elapsed()", "ctlChat.xaml.cs");
            }
        }

        #region Chat History

        public void WriteChatLog(string msg)
        {
            try
            {
                if (isChatHistoryEnabled && lstName.Count > 0)
                {
                    chatHistoryWriter = File.AppendText(chatHistoryPath + "\\" + chatHistoryFileName);
                    chatHistoryWriter.WriteLine(msg);
                    chatHistoryWriter.Flush();
                    chatHistoryWriter.Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "WriteChatLog()", "ctlChat.xaml.cs");
            }
        }

        #endregion

        #endregion

        #region Nettcp events

        void wndChat_EntsvcJoin(string uName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "wndChat_EntsvcJoin()", "ctlChat.xaml.cs");
            }
        }

        void wndChat_EntsvcSendMessage(string msg, string from, List<string> to)
        {
            try
            {
                if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstStr = new List<object>();
                    lstStr.Add(from);
                    lstStr.Add(msg);
                    lstStr.Add(to);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSendMsg, lstStr);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "wndChat_EntsvcSendMessage()", "ctlChat.xaml.cs");
            }
        }

        void ctlChat_EntsvcSetUserList(string uName)
        {
            try
            {
                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlChat_EntsvcSetUserList", "ctlChat.xaml.cs");
            }
        }

        void ctlChat_EntsvcGetUserList(string uName)
        {
            try
            {
                
                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                   
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uName);
                   
                    if (channelNetTcp != null)
                    {
                       
                        channelNetTcp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                       
                    }

                    if (channelHttp != null)
                    {
                        channelHttp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlCaht_EntsvcGetUserList()", "ctlChat.xaml.cs");
            }
        }

        void ctlChat_EntsvcShowStatus(string uname, List<string> to, string keydownstatus)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uname)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUserTypeing, uname, keydownstatus);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlChat_EntsvcShowStatus()", "ctlChat.xaml.cs");
            }
        }

        void ctlChat_EntsvcSignOutChat(string from, List<string> to)
        {
            try
            {
                if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstStr = new List<object>();
                    lstStr.Add(from);
                    lstStr.Add(to);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlChat_EntsvcSignOutChat", "ctlChat.xaml.cs");
            }
        }

        void wndChat_EntsvcUnJoin(string uName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "wndChat_EntsvcUnJoin()", "ctlChat.xaml.cs");
            }
        }

        #endregion

        #region Delegates

        void delSendMessage(List<object> lstStr)
        {
            try
            {
                List<string> tempArray = (List<string>)lstStr[2];
                
                for (int i = 0; i < tempArray.Count; i++)
                {
                    if (tempArray[i] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        rtbChat.AppendText(char.ConvertFromUtf32(13) + lstStr[0].ToString() + " Says : " + lstStr[1].ToString());

                        //Added for Chat History
                        WriteChatLog(lstStr[0].ToString() + " Said at [" + DateTime.Now.ToString() + "] : " + lstStr[1].ToString());

                        rtbChat.ScrollToEnd();
                        Beep(800, 150);

                        //ClsException.WriteToLogFile("chat Module: Message Received ");
                        
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delSendMessage()", "ctlChat.xaml.cs");
            }
        }

        void delSignoutMessage(List<object> lstStr)
        {
            try
            {
               

               // List<string> tempArray = (List<string>)lstStr[1];

             
                rtbChat.AppendText(char.ConvertFromUtf32(13) + lstStr[0].ToString() + " has left the chat at " + "[" + DateTime.Now.ToString() + "]");
                rtbChat.ScrollToEnd();

                //Added for Chat History
                WriteChatLog(lstStr[0].ToString() + " has left the chat at " + "[" + DateTime.Now.ToString() + "]");

                lstName.Remove(lstStr[0].ToString());
                lblDisplay.Content = lblDisplay.Content.ToString().Replace(lstStr[0].ToString(), "").Replace(",,", ",");
                Beep(800, 150);

                //ClsException.WriteToLogFile("Chat Module: Buddy Leaving Chat ");
                //ClsException.WriteToLogFile("Sender is (Buddy Leaving Chat): " + lstStr[0].ToString());
                //ClsException.WriteToLogFile("Time is : " + DateTime.Now.ToString()); 
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delSignoutMessage()", "ctlChat.xaml.cs");
            }
        }

        void delCloseChat(string from)
        {
            try
            {
                rtbChat.AppendText(Char.ConvertFromUtf32(13) + from + " has left the chat at " + "[" + DateTime.Now.ToString() + "]");
                rtbChat.ScrollToEnd();

                //Added for Chat History
                WriteChatLog(from + " has left the chat at " + "[" + DateTime.Now.ToString() + "]");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delCloseChat()", "ctlChat.xaml.cs");
            }
        }

        void delAsyncGetMessage(List<clsMessage> myMessages)
        {
            try
            {
                if (myMessages != null)
                {
                    for (int i = 0; i < myMessages.Count; i++)
                    {
                        if (myMessages[i].strMessage == "SignOut")
                        {
                            
                            if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                               
                                List<object> lstStr = new List<object>();
                                lstStr.Add(myMessages[i].strFrom);
                                lstStr.Add(myMessages[i].lstTo);
                               
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                            }
                        }
                        else if (myMessages[i].strMessage == "GetUserList")
                        {
                            try
                            {
                                if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages[i].strFrom);
                                    if (channelNetTcp != null)
                                    {
                                        channelNetTcp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                    }

                                    else
                                    {
                                        channelHttp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()", "ctlChat.xaml.cs");
                            }
                        }
                        else if (myMessages[i].strMessage == "SetUserList")
                        {
                            if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages[i].strFrom);
                            }
                        }

                        else if (myMessages[i].strMessage == "ShowTypeMessage")
                        {
                            if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUserTypeing, myMessages[i].strFrom, myMessages[i].strShowMsg);
                            }
                        }

                        else
                        {
                            rtbChat.AppendText(Char.ConvertFromUtf32(13) + myMessages[i].strFrom + " Says: " + myMessages[i].strMessage);
                            rtbChat.ScrollToEnd();

                            //Added for Chat History
                            WriteChatLog(myMessages[i].strFrom + " Said at [" + DateTime.Now.ToString() + "] : " + myMessages[i].strMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()", "ctlChat.xaml.cs");
            }
            this.dispTimer.Start();
        }

        void delTextDownEvent(bool isKeyDown)
        {
            try
            {
                if (isKeyDown)
                {



                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                    {
                        channelNetTcp.svcShowStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Typing");
                    }
                    else
                    {
                        
                        channelHttp.BeginsvcShowStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Typing", OnCompletionShowStatus, null);
                    }



                }
                else
                {
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                    {

                        channelNetTcp.svcShowStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Entered");

                    }
                    else
                    {

                        
                        channelHttp.BeginsvcShowStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Entered", OnCompletionShowStatus, null);

                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delTextDownEvent()", "ctlChat.xaml.cs");
            }
        }

        public void delSendPeerMessage(string msg)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcSendMessage(msg, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName);
                    }
                }
                else
                {
                    if (channelHttp != null)
                    {
                        channelHttp.svcSendMessage(msg, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName);
                    }
                }
            }
            catch (Exception ex)
            {                                                            
                VMuktiHelper.ExceptionHandler(ex, "delSendPeerMessage()", "ctlChat.xaml.cs");
            }
        }

        public void delGetUserList()
        {
            try
            {
                if (channelNetTcp != null)
                {
                    channelNetTcp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    
                    txtChat.IsEnabled = true;
                }
                if (channelHttp != null)
                {
                    channelHttp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    
                    txtChat.IsEnabled = true;
                }
              
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delGetUserList()", "ctlChat.xaml.cs");
            }
        }

        public void delShowUserTypeMessage(string uName, string keyStatus)
        {
            try
            {
                if (keyStatus == "Typing")
                {
                    bool isUserAdded = false;
                    for (int i = 0; i < lstTypingUsers.Count; i++)
                    {
                        if (lstTypingUsers[i] == uName)
                        {
                            isUserAdded = true;
                            break;
                        }
                    }
                    if (!isUserAdded)
                    {
                        lstTypingUsers.Add(uName);
                    }

                }
                else if (keyStatus == "Entered")
                {
                    for (int i = 0; i < lstTypingUsers.Count; i++)
                    {
                        if (lstTypingUsers[i] == uName)
                        {
                            lstTypingUsers.RemoveAt(i);
                            break;
                        }
                    }
                  
                }
                if (lstTypingUsers.Count == 0)
                {
                    lblStatus.Content = "";

                }
                else if (lstTypingUsers.Count == 1)
                {
                    lblStatus.Content = lstTypingUsers[0] + " is typing.";
                }
                else
                {
                    lblStatus.Content = "";
                    foreach (string str in lstTypingUsers)
                    {
                        lblStatus.Content += str + ", ";
                    }
                    lblStatus.Content = lblStatus.Content.ToString().Remove(lblStatus.Content.ToString().LastIndexOf(',')) + " are typing.";
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delShowUserTypeMessage()", "ctlChat.xaml.cs");

            }
        }

        #endregion

        

        #region IDisposable Members

        public void Dispose()
        {
           
            try
            {
                //ClsException.WriteToLogFile("Dispose of chat called");

                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //ClsException.WriteToLogFile("Uri is : " + strUri);
                
            }
            catch (Exception ex)
            {
              VMuktiHelper.ExceptionHandler(ex, "Dispose()", "ctlChat.xaml.cs");
            }
        }

        ~ctlChat()
        {
           
            try
            {

                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~cltChat()", "ctlChat.xaml.cs");
            }
        }

        #endregion

        #region resize

        void ctlChat_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(ctlChat_SizeChanged);
                this.Width = ((Grid)(this.Parent)).ActualWidth;
                grdMain.Width = this.Width - 25;
            }
            catch { }
        }

        void ctlChat_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Width > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch { }
        }

        void this_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Width > 0)
                {
                    grdMain.Width = this.Width - 25;
                }
            }
            catch
            { }
        }

        #endregion
       
    }
}
