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
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using VMukti.Business;
using VMuktiAPI;
using System.Text;


namespace VMukti.Presentation.Controls
{

    public partial class CtlBuddyList : System.Windows.Controls.UserControl
    {
 
        ClsBuddyReqCollection objBuddyReqCollection = null;        

        public CtlBuddyList()
        {
            try
            {
                InitializeComponent();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SucessfulLogin").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(CtlBuddyList_VMuktiEvent_SucessfulLogin);
                VMuktiAPI.VMuktiHelper.RegisterEvent("LogoutBuddyList").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(CtlBuddyList_VMuktiEvent_LogoutBuddyList);
                this.Unloaded += new RoutedEventHandler(CtlBuddyList_Unloaded);
                Application.Current.Exit += new ExitEventHandler(Current_Exit);
               
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBuddyList()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                LogOutBuddyList();
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SucessfulLogin");
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("LogoutBuddyList");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void CtlBuddyList_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LogOutBuddyList();
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SucessfulLogin");
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("LogoutBuddyList");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBuddyList_UnLoaded", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void LogOutBuddyList()
        {
            try
            {
                if (dt_RefreshBuddyList != null)
                {
                    dt_RefreshBuddyList.Stop();
                }
                if (lstBuddies != null && lstBuddies.Items.Count > 0)
                {
                    lstBuddies.Items.Clear();
                    lstBuddies.Items.Refresh();
                }

            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LogOutBuddyList()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }
        void CtlBuddyList_VMuktiEvent_LogoutBuddyList(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                LogOutBuddyList();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBuddyList_VMuktiEvent_LogoutBuddyList()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void CtlBuddyList_VMuktiEvent_SucessfulLogin(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                //  fncOpenClient();
                fncStartTimer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBuddyList_VmuktiEvent_successfullogin()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        public void LoadBuddyList()
        {
            try
            {
                //m_DelJoin = new DelJoin(EntsvcJoin);
                //m_DelIamOnline = new DelIamOnline(EntsvcIamOnline);
                //m_DelReply = new DelReply(EntsvcReply);

                objBuddyReqCollection = ClsBuddyReqCollection.GetByUserID(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                if (objBuddyReqCollection != null)
                {
                    for (int i = 0; i < objBuddyReqCollection.Count; i++)
                    {
                        Canvas cnvBuddy = new Canvas();
                        cnvBuddy.Width = 150;
                        cnvBuddy.Height = 50;

                        Label lblHeader = new Label();
                        lblHeader.Height = 25;
                        lblHeader.Width = 150;
                        lblHeader.SetValue(Canvas.TopProperty, 0.0);
                        lblHeader.SetValue(Canvas.LeftProperty, 0.0);
                        lblHeader.Content = objBuddyReqCollection[i].ReqDisplayName + " Wants to Join You!!";

                        cnvBuddy.Children.Add(lblHeader);

                        Label lblQuestion = new Label();
                        lblQuestion.Height = 25;
                        lblQuestion.Width = 50;
                        lblQuestion.Content = "Add ?";
                        lblQuestion.SetValue(Canvas.TopProperty, 25.0);
                        lblQuestion.SetValue(Canvas.LeftProperty, 0.0);

                        cnvBuddy.Children.Add(lblQuestion);

                        Button btnAccept = new Button();
                        btnAccept.Content = "Accept";
                        btnAccept.Width = 50;
                        btnAccept.Tag = i;
                        btnAccept.SetValue(Canvas.TopProperty, 25.0);
                        btnAccept.SetValue(Canvas.LeftProperty, 50.0);
                        btnAccept.Click += new RoutedEventHandler(ctlBuddyGrid_btnAcceptClicked);
                        cnvBuddy.Children.Add(btnAccept);

                        Button btnReject = new Button();
                        btnReject.Content = "Reject";
                        btnReject.Width = 50;
                        btnReject.Tag = i;
                        btnReject.SetValue(Canvas.TopProperty, 25.0);
                        btnReject.SetValue(Canvas.LeftProperty, 100.0);
                        btnReject.Click += new RoutedEventHandler(ctlBuddyGrid_btnRejectClicked);
                        cnvBuddy.Children.Add(btnReject);

                        ListBoxItem lstiBuddy = new ListBoxItem();
                        lstiBuddy.Content = cnvBuddy;
                        lstiBuddy.AllowDrop = false;
                        lstiBuddy.Tag = -1;

                        lstBuddies.Items.Add(lstiBuddy);
                    }
                }

                ClsBuddyCollection MyBuddies = ClsBuddyCollection.GetByMyUserID(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                if (MyBuddies != null)
                {
                    //((ClsPeerCommunicationChannel)objPeerCommuChannel).EntsvcJoin += new ClsPeerCommunicationChannel.DelsvcJoin(CtlBuddyList_EntsvcJoin);
                    //((ClsPeerCommunicationChannel)objPeerCommuChannel).EntsvcIamOnline += new ClsPeerCommunicationChannel.DelsvcIamOnline(CtlBuddyList_EntsvcIamOnline);
                    //((ClsPeerCommunicationChannel)objPeerCommuChannel).EntsvcReply += new ClsPeerCommunicationChannel.DelsvcReply(CtlBuddyList_EntsvcReply);
                    //((ClsPeerCommunicationChannel)objPeerCommuChannel).EntsvcStartConference+=new ClsPeerCommunicationChannel.DelsvcStartConference(CtlBuddyList_EntsvcStartConference);
                    //npcPeerCommuServer = new NetPeerService.NetPeerClient();
                    //PeerCommuService = (IPeerCommunication)npcPeerCommuServer.OpenClient<IPeerCommunication>("net.tcp://"+App.CreMachName+":2500/PeerCommunication", VMuktiAPI.VMuktiInfo.CurrentPeer.MeshID, ref objPeerCommuChannel);
                    //PeerCommuService.svcJoin();

                    for (int i = 0; i < MyBuddies.Count; i++)
                    {
                        CtlExpanderItem objExpanderItem = new CtlExpanderItem();
                        objExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                        objExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;
                        objExpanderItem.Image = @"\Images\Buddy.Png";
                        objExpanderItem.Caption = MyBuddies[i].DisplayName;

                        ListBoxItem lstiBuddy = new ListBoxItem();
                        lstiBuddy.Content = objExpanderItem;
                        lstiBuddy.AllowDrop = true;
                        lstiBuddy.Tag = MyBuddies[i].ID.ToString() + "," + MyBuddies[i].DisplayName;

                        lstiBuddy.PreviewMouseDown += new MouseButtonEventHandler(lstiBuddy_PreviewMouseDown);
                        lstBuddies.Items.Add(lstiBuddy);

                    }
                }


                //ctlBuddyGrid.Bind(objBuddyReqCollection);

                //ctlBuddyGrid.btnAcceptClicked += new CtlBuddyGrid.ButtonClicked(ctlBuddyGrid_btnAcceptClicked);
                //ctlBuddyGrid.btnRejectClicked += new CtlBuddyGrid.ButtonClicked(ctlBuddyGrid_btnRejectClicked);
                // btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
                //this.PreviewMouseDown += new MouseButtonEventHandler(CtlBuddyList_PreviewMouseDown);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadBuddyList()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void lstiBuddy_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.DragDrop.DoDragDrop((DependencyObject)((ListBoxItem)sender), ((ListBoxItem)sender), DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ListBuddy_PreviewMouseDown()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void ctlBuddyGrid_btnAcceptClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClsBuddy objBuddy = new ClsBuddy();
                objBuddy.ID = -1;
                objBuddy.UserID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                objBuddy.MyUserID = objBuddyReqCollection[int.Parse(((Button)sender).Tag.ToString())].ReqUserID;
                objBuddy.Save();

                objBuddyReqCollection[int.Parse(((Button)sender).Tag.ToString())].Delete();

                ClsBuddyCollection MyBuddies = ClsBuddyCollection.GetByMyUserID(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                if (MyBuddies != null)
                {
                    lstBuddies.Items.Clear();
                    for (int i = 0; i < MyBuddies.Count; i++)
                    {
                        CtlExpanderItem objExpanderItem = new CtlExpanderItem();
                        objExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                        objExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;
                        objExpanderItem.Image = @"\Images\Buddy.Png";
                        objExpanderItem.Caption = MyBuddies[i].DisplayName;

                        ListBoxItem lstiBuddy = new ListBoxItem();
                        lstiBuddy.Content = objExpanderItem;
                        lstiBuddy.AllowDrop = true;
                        lstiBuddy.Tag = MyBuddies[i].ID.ToString() + "," + MyBuddies[i].DisplayName;

                        lstiBuddy.PreviewMouseDown += new MouseButtonEventHandler(lstiBuddy_PreviewMouseDown);
                        lstBuddies.Items.Add(lstiBuddy);
                    }
                }

                lstBuddies.Items.Remove(((Canvas)((Button)sender).Parent).Parent);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlBuddyGrid_btnAcceptClicked()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        void ctlBuddyGrid_btnRejectClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                objBuddyReqCollection[int.Parse(((Button)sender).Tag.ToString())].Delete();

                lstBuddies.Items.Remove(((Canvas)((Button)sender).Parent).Parent);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBuddyGrid_btnRejectClicked()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }

        //void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    //int Result = 0;

        //    //ClsBuddyReq objBuddyReq = new ClsBuddyReq();
        //    //objBuddyReq.ID = -1;
        //    //objBuddyReq.UserID = -1;
        //    //objBuddyReq.DisplayName = txtName.Text;
        //    //objBuddyReq.ReqUserID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
        //    //objBuddyReq.ReqDisplayName = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

        //    //objBuddyReq.Save(ref Result);

        //    //if (Result == -1)
        //    //{
        //    //    lblMessages.Content = "User not found!!";
        //    //}
        //    //else if (Result >= 0)
        //    //{
        //    //    lblMessages.Content = "Request has been send to user " + txtName.Text;
        //    //    txtName.Text = "";
        //    //    txtName.Focus();
        //    //}


        //}

        public void startConference(string strConfInfo, object Tag)
        {
            try
            {
                //if (SIPConfID != "")
                //{
                //    objRTCAudio.DisConnect();
                //}
                //VMukti.Business.ClsConfInfo.AllocateSIPConfId(out SIPConfID, out SIPConfPass);
                //objRTCAudio.Connect(SIPConfID);

                //NetPeerService.NetPeerClient npcPeerCommuServerBuddies = new NetPeerService.NetPeerClient();
                //object objPeerCommuServerBuddiesChannel = new ClsPeerCommunicationChannel();
                //IPeerCommunication PeerCommuServiceBuddies = (IPeerCommunication)npcPeerCommuServerBuddies.OpenClient<IPeerCommunication>("net.tcp://" + App.CreMachName + ":2500/PeerCommunication", ((ClsBuddy)Tag).MeshID, ref objPeerCommuServerBuddiesChannel);

                //PeerCommuServiceBuddies.svcJoin();
                //PeerCommuServiceBuddies.svcStartConference(strConfInfo);
                //npcPeerCommuServerBuddies.CloseClient<IPeerCommunication>();
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartConference()", "Controls\\CtlBuddyList.xaml.cs");
            }
        }


        #region Thread functions

        //void CheckBuddy(object MeshID)
        //{
        //    try
        //    {
        //        NetPeerService.NetPeerClient npcPeerCommuServerBuddies = new NetPeerService.NetPeerClient();
        //        object objPeerCommuServerBuddiesChannel = new ClsPeerCommunicationChannel();
        //        IPeerCommunication PeerCommuServiceBuddies = (IPeerCommunication)npcPeerCommuServerBuddies.OpenClient<IPeerCommunication>("net.tcp://" + App.CreMachName + ":2500/PeerCommunication", MeshID.ToString(), ref objPeerCommuServerBuddiesChannel);

        //        PeerCommuServiceBuddies.svcJoin();
        //        PeerCommuServiceBuddies.svcIamOnline(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.Status);
        //        npcPeerCommuServerBuddies.CloseClient<IPeerCommunication>();

        //        thrCounter++;
        //    }
        //    catch (Exception exp)
        //    {
        //        MessageBox.Show("MeshID = " + MeshID.ToString() + ", Error = " + exp.Message);
        //    }
        //}

        //void ReplyBuddy(object MeshID)
        //{
        //    try
        //    {
        //        NetPeerService.NetPeerClient npcPeerCommuServerBuddies = new NetPeerService.NetPeerClient();
        //        object objPeerCommuServerBuddiesChannel = new ClsPeerCommunicationChannel();
        //        IPeerCommunication PeerCommuServiceBuddies = (IPeerCommunication)npcPeerCommuServerBuddies.OpenClient<IPeerCommunication>("net.tcp://" + App.CreMachName + ":2500/PeerCommunication", MeshID.ToString(), ref objPeerCommuServerBuddiesChannel);

        //        PeerCommuServiceBuddies.svcJoin();
        //        PeerCommuServiceBuddies.svcReply(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.Status);
        //        npcPeerCommuServerBuddies.CloseClient<IPeerCommunication>();

        //        thrCounter++;
        //    }
        //    catch (Exception exp)
        //    {
        //        MessageBox.Show(exp.Message);
        //    }
        //}

        #endregion

        #region WCF functions with Delegate functions

        //void CtlBuddyList_EntsvcJoin()
        //{
        //    //this.Dispatcher.BeginInvoke(DispatcherPriority.Background, m_DelJoin);
        //}

        //public void EntsvcJoin()
        //{

        //}

        //void CtlBuddyList_EntsvcIamOnline(string UDispName, string UStatus)
        //{
        //    //this.Dispatcher.BeginInvoke(DispatcherPriority.Background, m_DelIamOnline, null, UDispName, UStatus);
        //}

        //public void EntsvcIamOnline(string UDispName, string UStatus)
        //{
        //    for (int i = 0; i < lstBuddies.Items.Count; i++)
        //    {
        //        if (((ListBoxItem)lstBuddies.Items[i]).Content.ToString() == UDispName)
        //        {
        //            ((ListBoxItem)lstBuddies.Items[i]).IsEnabled = true;
        //            ((ClsBuddy)((ListBoxItem)lstBuddies.Items[i]).Tag).Status = UStatus;

        //            Thread thrTemp = new Thread(new ParameterizedThreadStart(ReplyBuddy));
        //            thrMyBuddies[thrMyBuddies.Count - 1].Priority = ThreadPriority.Lowest;
        //            thrMyBuddies[thrMyBuddies.Count - 1].Start(((ClsBuddy)((ListBoxItem)lstBuddies.Items[i]).Tag).MeshID);

        //            break;
        //        }
        //    }
        //}

        //void CtlBuddyList_EntsvcReply(string UDispName, string UStatus)
        //{
        //    //this.Dispatcher.BeginInvoke(DispatcherPriority.Background, m_DelReply, null, UDispName, UStatus);
        //}

        //public void EntsvcReply(string UDispName, string UStatus)
        //{
        //    for (int i = 0; i < lstBuddies.Items.Count; i++)
        //    {
        //        if (((ListBoxItem)lstBuddies.Items[i]).Content.ToString() == UDispName)
        //        {
        //            ((ListBoxItem)lstBuddies.Items[i]).IsEnabled = true;
        //            ((ClsBuddy)((ListBoxItem)lstBuddies.Items[i]).Tag).Status = UStatus;

        //            break;
        //        }
        //    }
        //}

        #endregion

        # region New Functions for Buddylist

        string strFilePath = string.Empty;
        //public static IHTTPBootStrapService channelBS = null;
        //public static IHTTPSuperNodeServiceChannel channelHttpSuperNode = null;
        string appPath = string.Empty;
        System.Windows.Threading.DispatcherTimer dt_RefreshBuddyList = null;
        //void fncOpenClient()
        //{
        //    BasicHttpClient bhcBootStrap = new BasicHttpClient();
        //    channelBS = (IHTTPBootStrapService)bhcBootStrap.OpenClient<IHTTPBootStrapChannel>("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0]+ ":80/HTTPBSLoginService");
        //    App.chHttpBootStrapService.
        //    BasicHttpClient bhcSuperNodeClient = new BasicHttpClient();
        //    channelHttpSuperNode = (IHTTPSuperNodeServiceChannel)bhcSuperNodeClient.OpenClient<IHTTPSuperNodeServiceChannel>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP+ ":80/SuperNode");


        //}
        void fncStartTimer()
        {
            try
            {
                dt_RefreshBuddyList = new System.Windows.Threading.DispatcherTimer();
                dt_RefreshBuddyList.Interval = TimeSpan.FromSeconds(5);
                dt_RefreshBuddyList.Tick += new EventHandler(dt_RefreshBuddyList_Tick);
                dt_RefreshBuddyList.Start();

                fncGetBuddyList();
                appPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncStartTimer()", "Controls\\CtlBuddyList.xaml.cs");
            }
            //  strFilePath = AppDomain.CurrentDomain.BaseDirectory +"\\VMuktiMP_SuperNode_" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName+ "_BuddyList.txt";

            //  FileInfo objFileInfo = new FileInfo(strFilePath);

            ////  if (VMuktiAPI.VMuktiInfo.IsFileExists)//objSNodeInfo.FileExists) //User is Already registered with Application
            //  if (objFileInfo.Exists)
            //  {
            //      fncGetBuddyList(objFileInfo);
            //  }

            //  else //New User
            //  {
            //      FileStream objFileStream = new FileStream(strFilePath, FileMode.Create, FileAccess.ReadWrite);
            //      objFileStream.Close();
            //  }

        }
        // For SuperNode Buddy RefreshTick.
        List<string> fncGetBuddyStatus(List<string> BuddyList)
        {
            try
            {
                string ClientConnectionString = @"Data Source=" + System.IO.Path.GetDirectoryName(appPath) + "\\SuperNodeBuddyInfo.sdf";
                SqlCeConnection conn = new SqlCeConnection(ClientConnectionString);
                conn.Open();
                string str = "SELECT * FROM Node_Status";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, conn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                List<string> lstOnlineBuddies = new List<string>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstOnlineBuddies.Add(dataTable.Rows[i][1].ToString() + "-" + dataTable.Rows[i][2].ToString());
                }

                //List<string> lstStatus = new List<string>();

                //// Generatig Status List
                //for (int i = 0; i < BuddyList.Count; i++)
                //{
                //    if (lstOnlineBuddies.Contains(BuddyList[i].ToString()))
                //    {
                //        lstStatus.Add("Online");
                //    }
                //    else
                //    {
                //        lstStatus.Add("OffLine");
                //    }
                //}
                conn.Close();
                return lstOnlineBuddies;
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetBuddyStatus()", "Controls\\CtlBuddyList.xaml.cs");                
                return null;
            }
        }

        void dt_RefreshBuddyList_Tick(object sender, EventArgs e)
        {
            try
            {
                List<string> lstCheckBuddyStatus = new List<string>();
                for (int i = 0; i < lstBuddies.Items.Count; i++)
                {

                    string str = ((ListBoxItem)lstBuddies.Items[i]).Content.ToString();
                    if (str != "")
                    {
                        string[] str1 = str.Split('-');
                        lstCheckBuddyStatus.Add(str1[0]);
                    }
                }

                List<string> lstuddyStatus = new List<string>();
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.SuperNode && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.BootStrap)
                {
                    try
                    {


                        // lstuddyStatus = channelHttpSuperNode.GetBuddyStatus(lstCheckBuddyStatus);
                        lstuddyStatus = App.chHttpSuperNodeService.GetBuddyStatus(lstCheckBuddyStatus);
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dt_RefreshBuddyList_Tick()", "Controls\\CtlBuddyList.xaml.cs");
                    }
                }
                else
                {
                    try
                    {

                        //lstuddyStatus = channelHttpSuperNode.GetBuddyStatus(lstCheckBuddyStatus);
                        lstuddyStatus = fncGetBuddyStatus(lstCheckBuddyStatus);
                    }

                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dt_RefreshBuddyList_Tick()", "Controls\\CtlBuddyList.xaml.cs");
                    }

                }


                lstCheckBuddyStatus.Clear();
                for (int i = 0; i < lstuddyStatus.Count; i++)
                {
                    string[] strBuddyStatus = lstuddyStatus[i].Split('-');
                    if (strBuddyStatus[0] != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        bool isItemAdded = false;
                        int ItemIndex = 0;
                        for (int j = 0; j < lstBuddies.Items.Count; j++)
                        {
                            if (((ListBoxItem)lstBuddies.Items[j]).Content.ToString() == strBuddyStatus[0])
                            {
                                isItemAdded = true;
                                ItemIndex = j;
                                break;
                            }
                        }
                        if (isItemAdded)
                        {
                            if (strBuddyStatus[1].ToString().ToLower() == "offline")
                            {
                                ((ListBoxItem)lstBuddies.Items[ItemIndex]).IsEnabled = false;
                            }
                            else
                            {
                                ((ListBoxItem)lstBuddies.Items[ItemIndex]).IsEnabled = true;
                            }
                        }
                        else
                        {
                            ListBoxItem lbiBuddyInfo = new ListBoxItem();
                            lbiBuddyInfo.Content = strBuddyStatus[0];

                            if (strBuddyStatus[1] == "Offline")
                            {
                                lbiBuddyInfo.IsEnabled = false;
                            }
                            else
                            {
                                lbiBuddyInfo.IsEnabled = true;
                            }
                            lstBuddies.Items.Add(lbiBuddyInfo);
                        }
                    }
                }

                if (lstBuddies.SelectedIndex != -1 && !((ListBoxItem)lstBuddies.Items[lstBuddies.SelectedIndex]).IsEnabled)
                {
                    lstBuddies.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dt_RefreshBuddyList_Tick()--3", "Controls\\CtlBuddyList.xaml.cs");
            }
        }
        // Timer Which will check buddy status.
        //void dt_RefreshBuddyList_Tick(object sender, EventArgs e)
        //{
        //    List<string> lstCheckBuddyStatus = new List<string>();
        //    for (int i = 0; i < lstBuddies.Items.Count; i++)
        //    {

        //        string str = ((ListBoxItem)lstBuddies.Items[i]).Content.ToString();
        //        if (str != "")
        //        {
        //            string[] str1 = str.Split('-');
        //            lstCheckBuddyStatus.Add(str1[0]);
        //        }
        //    }

        //    List<string> lstuddyStatus = new List<string>();
        //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.SuperNode && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.BootStrap)
        //    {
        //        try
        //        {


        //           // lstuddyStatus = channelHttpSuperNode.GetBuddyStatus(lstCheckBuddyStatus);
        //            lstuddyStatus = App.chHttpSuperNodeService.GetBuddyStatus(lstCheckBuddyStatus);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {

        //            //lstuddyStatus = channelHttpSuperNode.GetBuddyStatus(lstCheckBuddyStatus);
        //            lstuddyStatus = fncGetBuddyStatus(lstCheckBuddyStatus);
        //        }
        //        catch (Exception exp)
        //        {
        //            MessageBox.Show("dt_RefreshBuddyList_Tick" + exp.Message);
        //        }
        //    }


        //    lstCheckBuddyStatus.Clear();
        //    for (int i = 0; i < lstuddyStatus.Count; i++)
        //    {
        //        if (lstuddyStatus[i].ToString().ToLower() == "offline")
        //        {
        //            ((ListBoxItem)lstBuddies.Items[i]).IsEnabled = false;
        //        }
        //        else
        //        {
        //            ((ListBoxItem)lstBuddies.Items[i]).IsEnabled = true;
        //        }
        //    }
        //    if (lstBuddies.SelectedIndex != -1 && !((ListBoxItem)lstBuddies.Items[lstBuddies.SelectedIndex]).IsEnabled)
        //    {
        //        lstBuddies.SelectedIndex = -1;
        //    }
        //}

        // Fill List at first time when login.
        private void fncGetBuddyList()//FileInfo objFileInfo)
        {
            try
            {
                lstBuddies.Items.Clear();
                //if (objFileInfo.Exists)
                //{
                //    FileStream objFileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.ReadWrite);
                //    StreamReader SrReader = new StreamReader(objFileStream);
                //    string strBuddyNameStream = SrReader.ReadToEnd();
                //    string[] strBuddyName = strBuddyNameStream.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                //    SrReader.Close();
                //    objFileStream.Close();
                //    if (strBuddyName[0] != "")
                //    {
                //        for (int i = 0; i < strBuddyName.Length; i++)
                //        {
                //            ListBoxItem lbiBuddyInfo = new ListBoxItem();
                //            lbiBuddyInfo.Content = strBuddyName[i];
                //            lstBuddies.Items.Add(lbiBuddyInfo);
                //        }
                //    }
                //}
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    //Ask to BootStrapNode to send buddy information. using http service. 
                    //And call svcjoin function of bootstartp of net.tcp service
                    List<string> lsBuddies = new List<string>();
                    lsBuddies = App.chHttpBootStrapService.svcHttpGetSuperNodeBuddyList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    //lsBuddies = channelBS.svcHttpGetSuperNodeBuddyList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

                    for (int i = 0; i < lsBuddies.Count; i++)
                    {
                        string[] strBuddyStatus = lsBuddies[i].Split('-');
                        if (strBuddyStatus[0] != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName) //&& !lstBuddies.Items.Contains(lsBuddies[i].ToString()))
                        {
                            //ListBoxItem lbiBuddyInfo = new ListBoxItem();
                            //lbiBuddyInfo.Content = lsBuddies[i];
                            //lstBuddies.Items.Add(lbiBuddyInfo);
                            ListBoxItem lbiBuddyInfo = new ListBoxItem();
                            lbiBuddyInfo.Content = strBuddyStatus[0];
                            if (strBuddyStatus[1] == "Offline")
                            {
                                lbiBuddyInfo.IsEnabled = false;
                            }
                            else
                            {
                                lbiBuddyInfo.IsEnabled = true;
                            }
                            lstBuddies.Items.Add(lbiBuddyInfo);

                            //lstBuddies.Items.Add(lsBuddies[i].ToString());
                        }
                    }
                }
                else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.SuperNode && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.BootStrap)
                {
                    try
                    {
                        //FileStream objFileStream = new FileStream(strFilePath, FileMode.Create, FileAccess.ReadWrite);
                        //objFileStream.Close();
                        //StreamWriter SrWriter = new StreamWriter(strFilePath, true);
                        List<string> lsBuddies = new List<string>();
                        try
                        {
                            //lsBuddies = channelHttpSuperNode.GetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            lsBuddies = App.chHttpSuperNodeService.GetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetBuddyList()", "Controls\\CtlBuddyList.xaml.cs");                            
                            //return null;
                        }
                        for (int i = 0; i < lsBuddies.Count; i++)
                        {
                            string[] strBuddyStatus = lsBuddies[i].Split('-');
                            if (lsBuddies[i].ToString() != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName) //&& !lstBuddies.Items.Contains(lsBuddies[i].ToString()))
                            {
                                //lstBuddies.Items.Add(lsBuddies[i].ToString());
                                //ListBoxItem lbiBuddyInfo = new ListBoxItem();
                                //lbiBuddyInfo.Content = lsBuddies[i].ToString();
                                //lstBuddies.Items.Add(lbiBuddyInfo);

                                ListBoxItem lbiBuddyInfo = new ListBoxItem();
                                lbiBuddyInfo.Content = strBuddyStatus[0];
                                if (strBuddyStatus[1] == "Offline")
                                {
                                    lbiBuddyInfo.IsEnabled = false;
                                }
                                else
                                {
                                    lbiBuddyInfo.IsEnabled = true;
                                }
                                lstBuddies.Items.Add(lbiBuddyInfo);
                                //SrWriter.Write(lsBuddies[i].ToString() + ";");
                            }
                        }
                        //SrWriter.Close();

                        //Ask to SuperNode to send buddy information. using http service.
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetBuddyList()--2", "Controls\\CtlBuddyList.xaml.cs");                        
                        //return null;
                    }
                }
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncGetBuddyList()--3", "Controls\\CtlBuddyList.xaml.cs");              
                //return null;
            }

            // For Adding new buddy.
            //private void fncAddBuddy()
            //{
            //    lstBuddies.Items.Clear();
            //    FileStream objFileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.ReadWrite);
            //    StreamReader SrReader = new StreamReader(objFileStream);
            //    string strBuddyNameStream = SrReader.ReadToEnd();
            //    string[] strBuddyName = strBuddyNameStream.Split(';');
            //    SrReader.Close();
            //    objFileStream.Close();

            //    if (strBuddyName[0] != "")
            //    {
            //        for (int i = 0; i < strBuddyName.Length; i++)
            //        {
            //            ListBoxItem lbiBuddyInfo = new ListBoxItem();
            //            lbiBuddyInfo.Content = strBuddyName[i].ToString();
            //            lstBuddies.Items.Add(lbiBuddyInfo);
            //            //lstBuddies.Items.Add(strBuddyName[i]);
            //        }
            //    }
            //}

            //private void btnAdd_Click(object sender, RoutedEventArgs e)
            //{
            //    try
            //    {
            //        if (txtName.Text.Trim() == "")
            //        {                    
            //            //txtDisplay.Text = "";
            //            //txtDisplay.Text = "Enter Buddy Name";
            //            lblMessages.Content = "";
            //            lblMessages.Content = "Enter Buddy Name";
            //            return;
            //        }
            //        else if (txtName.Text.ToLower() == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName.ToLower())
            //        {
            //            //txtDisplay.Text = "";
            //            //txtDisplay.Text = "Not Allowed";
            //            lblMessages.Content = "";
            //            lblMessages.Content = "Not Allowed";
            //            return;
            //        }
            //        else
            //        {

            //            for (int i = 0; i < lstBuddies.Items.Count; i++)
            //            {
            //                if (((ListBoxItem)lstBuddies.Items[i]).Content.ToString().ToLower().Split('-')[0] == txtName.Text.ToLower())
            //                {
            //                    //txtDisplay.Text = "";
            //                    //txtDisplay.Text = "Buddy Already Added";
            //                    lblMessages.Content = "";
            //                    lblMessages.Content = "Buddy Already Added";
            //                    return;
            //                }
            //            }                    
            //            //string SIPNumber = channelBS.svcHttpAddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtName.Text);
            //            string SIPNumber =  App.chHttpBootStrapService.svcHttpAddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtName.Text);
            //            if (SIPNumber != string.Empty)
            //            {
            //                StreamWriter SrWriter = new StreamWriter(strFilePath, true);
            //                SrWriter.Write(txtName.Text + "-" + SIPNumber + ";");
            //                SrWriter.Close();
            //                try
            //                {
            //                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.SuperNode && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != VMuktiAPI.PeerType.BootStrap)
            //                    {                                
            //                        //channelHttpSuperNode.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtName.Text + "-" + SIPNumber);
            //                        App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtName.Text + "-" + SIPNumber);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                }
            //                txtName.Text = "";
            //                fncAddBuddy();
            //            }
            //            else
            //            {
            //                //txtDisplay.Text = "";
            //                //txtDisplay.Text = "User Not Found";
            //                lblMessages.Content = "";
            //                lblMessages.Content = "User Not Found";
            //            }
            //            //Send new buddy name to supernode and add to supernode's list and machine.
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("btnAdd_Click" + ex.Message);
            //    }
            //}

        #endregion
        }

        ~CtlBuddyList()
        {
            if (objBuddyReqCollection != null)
            {
                objBuddyReqCollection = null;
            }
        }

    }
}
