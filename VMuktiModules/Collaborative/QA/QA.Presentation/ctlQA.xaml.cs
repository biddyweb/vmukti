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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using QA.Business.Service.BasicHttp;
using QA.Business.Service.DataContracts;
using QA.Business.Service.NetP2P;
using VMuktiService;

namespace QA.Presentation
{
    /// <summary>
    /// Interaction logic for ctlQA.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlQA : UserControl
    {
        //public ctlQA()
        //{
        //    InitializeComponent();
        //}

        int Counter = 0;
        int tempcounter = 0;
        static int idCounter = 1;
        string strRole = string.Empty;
        string strUri = string.Empty;

        object objNetTcpQA = new clsNetTcpQA();
        object objHttpQA = new clsHttpQA();

        INetTcpQAChannel NetP2PChannel;
        IHttpQA HttpChannel;

        public delegate void DelAskQuestion(List<object> lstQuestion);
        public delegate void DelReplyQuestion(List<object> lstAnswer);
        public delegate void DelDisplayName(string lstUserName);
       
        public DelAskQuestion objDelAskQuestion;
        public DelReplyQuestion objDelReplyQuestion;
        public DelDisplayName objDelDisplayName;

        System.Threading.Thread ThrHostQA = null;
        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);

        //string[] sBuddyList = new string[2];
        List<string> sBuddyList=new List<string>(); 

        public ctlQA(VMuktiAPI.PeerType bindingtype, string uri, ModulePermissions[] MyPermissions,string Role)
        {
            InitializeComponent();
            try
            {
                strRole = Role;
                //objDelDisplayName = new DelDisplayName(FncDisplayName);
                objDelAskQuestion = new DelAskQuestion(FncDelAskQuestion);
                objDelReplyQuestion = new DelReplyQuestion(FncDelReplyAnswer);
                if (Role == "Host")
                {
                    lstQuestionGuest.Visibility = Visibility.Collapsed;
                    lblQueation.Content = "Questions To Answer";
                    lbltype.Content = "Answer:";
                }
                else if (Role == "Guest")
                {
                    lstQuestionHost.Visibility = Visibility.Collapsed;
                    lblQueation.Content = "Asked Questions";
                    lbltype.Content = "Question:";
                }

                ThrHostQA = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(fncHostQAService));
                List<object> lstParams = new List<object>();
                lstParams.Add(bindingtype);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);
                ThrHostQA.Start(lstParams);

                txtQuestion.KeyDown += new KeyEventHandler(txtQuestion_KeyDown);
                btnSay.Click += new RoutedEventHandler(btnSay_Click);

                //if (bindingtype.ToString() == "Http")
                //{
                //    VMuktiAPI.PeerType = bindingtype;
                //}

                //VMukti.Global.VMuktiGlobal.strBootStrapIPs[0] = "192.168.0.107";
                //VMukti.Global.VMuktiGlobal.strSuperNodeIP = "192.168.0.107";
                //VMuktiAPI = Environment.MachineName;

                //ListBoxItem lstItem = null;
                //lstItem = new ListBoxItem();
                //lstItem.Content = "ADIANCE07";
                //lstTemp.Items.Add(lstItem);

                //ListBoxItem lstItem1 = null;
                //lstItem1 = new ListBoxItem();
                //lstItem1.Content = "ADIANCE09";
                //lstTemp.Items.Add(lstItem1);

                //ListBoxItem lstItem2 = null;
                //lstItem2 = new ListBoxItem();
                //lstItem2.Content = "ADIANCE";
                //lstTemp.Items.Add(lstItem2);


                
                //lstItem.AllowDrop=true;
                CnvMainFirst.AllowDrop = true;
                //lstItem.PreviewMouseDown += new MouseButtonEventHandler(lstItem_PreviewMouseDown);
                CnvMainFirst.PreviewDrop += new DragEventHandler(CnvMainFirst_PreviewDrop);

            }
            catch (Exception exp)
            {
                MessageBox.Show("ctlQA" + exp.Message);
            }
        }

        void CnvMainFirst_PreviewDrop(object sender, DragEventArgs e)
        {
            try
            {
                bool flg = true;
                if (e.Data.GetDataPresent(typeof(ListBoxItem)))
                {
                    //sBuddyList.Add(((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString());
                    for (int i = 0; i < sBuddyList.Count; i++)
                    {
                        if (sBuddyList[i].ToString() == ((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString())
                        {
                            //sBuddyList.Add(((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString());
                            //MessageBox.Show("all ready added");
                            flg = false;
                        }

                    }
                    if (flg)
                    {
                        sBuddyList.Add(((System.Windows.Controls.ContentControl)(e.Data.GetData(typeof(ListBoxItem)))).Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        //void lstItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    System.Windows.DragDrop.DoDragDrop((DependencyObject)((ListBoxItem)sender), ((ListBoxItem)sender), DragDropEffects.Copy);
        //}

       
        void txtQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtQuestion.Text != "")
                {
                    btnSay_Click(null, null);
                }
            }

        }

        void btnSay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strRole == "Host")
                {
                    if (lstQuestionHost.SelectedItems.Count == 1)
                    {
                        rtbAnswer.AppendText(Char.ConvertFromUtf32(13) + "Question:" + ((ListBoxItem)lstQuestionHost.SelectedItem).Content.ToString().Split(':')[1]);
                        rtbAnswer.AppendText(Char.ConvertFromUtf32(13) + "Answer:" + txtQuestion.Text);
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                        {
                            if (NetP2PChannel != null)
                            {
                                NetP2PChannel.svcP2PReplyQuestion(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstQuestionHost.SelectedItem.ToString().Split(':')[2], txtQuestion.Text, "Guest", sBuddyList);
                            }
                        }
                        else
                        {
                            if (HttpChannel != null)
                            {
                                HttpChannel.svcHttpReplyQuestion(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstQuestionHost.SelectedItem.ToString().Split(':')[2], txtQuestion.Text, "Guest", sBuddyList);
                            }
                        }
                        lstQuestionHost.Items.Remove(lstQuestionHost.SelectedItem);
                        txtQuestion.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Select One Question");
                    }
                }
                else if (strRole == "Guest")
                {
                    if (txtQuestion.Text != "")
                    {
                        ListBoxItem objListBoxItem = new ListBoxItem();
                        objListBoxItem.Content = txtQuestion.Text;
                        lstQuestionGuest.Items.Add(objListBoxItem);
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                        {
                            if (NetP2PChannel != null)
                            {
                                NetP2PChannel.svcP2PAskQuestion(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtQuestion.Text, "Host");
                            }
                        }
                        else
                        {
                            if (HttpChannel != null)
                            {
                                HttpChannel.svcHttpAskQuestion(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtQuestion.Text, "Host");
                            }
                        }
                        txtQuestion.Text = "";
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("btnSay_Click" + exp.Message);
            }
        }

        void ctlQA_EntsvcP2PJoin(string uName)
        {
        }

        void ctlQA_EntsvcP2PAskQuestion(string uName, string Question, string Role)
        {
            if (strRole == Role)
            {
                List<object> lstQuestion = new List<object>();
                lstQuestion.Add(uName);
                lstQuestion.Add(Question);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAskQuestion, lstQuestion);
            }
        }

        void ctlQA_entsvcP2PReplyQuestion(string uName, string Question, string Answer, string Role, List<string> strBuddyList)
        {
            if (strRole == Role)
            {
                List<object> lstAnswer = new List<object>();
                lstAnswer.Add(uName);
                lstAnswer.Add(Question);
                lstAnswer.Add(Answer);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelReplyQuestion, lstAnswer);
            }
        }

        private void fncHostQAService(object lstParams)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstParams;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcQA = new NetPeerClient();
                    ((clsNetTcpQA)objNetTcpQA).EntsvcP2PJoin += new clsNetTcpQA.DelsvcP2Pjoin(ctlQA_EntsvcP2PJoin);
                    ((clsNetTcpQA)objNetTcpQA).EntsvcP2PAskQuestion += new clsNetTcpQA.DelsvcP2PAskQuestion(ctlQA_EntsvcP2PAskQuestion);
                    ((clsNetTcpQA)objNetTcpQA).entsvcP2PReplyQuestion += new clsNetTcpQA.DelsvcP2PReplyQuestion(ctlQA_entsvcP2PReplyQuestion);
                    NetP2PChannel = (INetTcpQAChannel)npcQA.OpenClient<INetTcpQAChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[1], ref objNetTcpQA);
                    while (Counter < 20)
                    {
                        try
                        {
                            NetP2PChannel.svcP2PJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            Counter = 20;
                        }
                        catch
                        {
                            Counter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    //DummyClient dc = new DummyClient();
                    //string httpuri = dc.QAClient(idCounter, strUri, VMukti.Global.VMuktiGlobal.strSuperNodeIP);

                    BasicHttpClient BasicQAClient = new BasicHttpClient();
                    HttpChannel = (IHttpQA)BasicQAClient.OpenClient<IHttpQA>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            HttpChannel.svcHttpJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);;
                            tempcounter = 20;
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    dt.Interval = TimeSpan.FromSeconds(2);
                    dt.Tick += new EventHandler(dt_Tick);
                    dt.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("hostQAservice " + ex.Message);
                if (ex.InnerException != null)
                {
                    MessageBox.Show("hostQAservice" + ex.InnerException.Message);
                }
            }
        }

        //public void DisplayName(string lstUserName)
        //{
        //    VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = lstUserName;
        //}

        void FncDelAskQuestion(List<object> lstQuestion)
        {
            try
            {
                ListBoxItem objListBoxItem = new ListBoxItem();
                objListBoxItem.Content = lstQuestion[0] + ":" + lstQuestion[1];
                lstQuestionHost.Items.Add(objListBoxItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FncDelAskQuestion " + ex.Message);
            }
        }

        void FncDelReplyAnswer(List<object> lstAnswer)
        {
            try
            {
                rtbAnswer.AppendText(Char.ConvertFromUtf32(13) + "Question:" + lstAnswer[1].ToString());
                rtbAnswer.AppendText(Char.ConvertFromUtf32(13) + "Answer:" + lstAnswer[2].ToString());
                for (int i = 0; i < lstQuestionGuest.Items.Count; i++)
                {
                    if (lstQuestionGuest.Items[i].ToString().Split(':')[1].Trim() == lstAnswer[1].ToString())
                    {
                        lstQuestionGuest.Items.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("FncDelReplyAnswer " + ex.Message);
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            lock (this)
            {
                List<clsMessage> myMessages = HttpChannel.svcHttpGetMessage(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                if (myMessages != null && myMessages.Count!=0)
                {
                    //Collect Questions
                    List<object> lstQuestionDummy = new List<object>();
                    string strLocalRole = string.Empty;
                    for (int i = 0; i < myMessages.Count; i++)
                    {
                        if (myMessages[i].strRole == "Host" && strRole=="Host")
                        {
                            strLocalRole = myMessages[i].strRole;
                            lstQuestionDummy.Add(myMessages[i].struName);
                            lstQuestionDummy.Add(myMessages[i].strQuestion);
                        }
                        else if (myMessages[i].strRole == "Guest" && strRole=="Guest")
                        {
                            lstQuestionDummy.Add(myMessages[i].struName);
                            lstQuestionDummy.Add(myMessages[i].strQuestion);
                            lstQuestionDummy.Add(myMessages[i].strAnswer);
                        }
                    }
                    if (lstQuestionDummy.Count != 0 && strRole == "Host")
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAskQuestion, lstQuestionDummy);
                    }
                    else if (lstQuestionDummy.Count != 0 && strRole == "Guest")
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelReplyQuestion, lstQuestionDummy);
                    }
                }
            }

            //else if (myMessages != null && myMessages == "Guest")
            //{
            //    //Collect Answers
            //    List<object> lstAnswerDummy = new List<object>();
            //    for (int i = 0; i < myMessages.Count; i++)
            //    {
            //        lstAnswerDummy.Add(myMessages[i].struName);
            //        lstAnswerDummy.Add(myMessages[i].strQuestion);
            //        lstAnswerDummy.Add(myMessages[i].strAnswer);
            //    }
            //    if (lstAnswerDummy.Count != 0)
            //    {
            //        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelReplyQuestion, lstAnswerDummy);
            //    }
            //}
        }
    }
}
