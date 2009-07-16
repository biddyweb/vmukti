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
using VMuktiAPI;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using VMukti.Business;
using System.IO;
using System.ComponentModel;

namespace VMukti.Presentation.Controls
{
    public partial class CtlBuddies : System.Windows.Controls.UserControl, IDisposable
    {
        public static StringBuilder sb1 = new StringBuilder();
      
        bool isUserLogin = false;
        string temp = null;
        
        delegate void DelAddNewBuddy();
        DelAddNewBuddy delAddNewBuddy = null;

        delegate void DelFindBuddy();
        DelFindBuddy delFindBuddy = null;

        delegate void DelBuddyOffline();
        DelBuddyOffline delBuddyOffline = null;        

        #region Multiple Buddy Selection

        public delegate void DelMCodMultipleBuddies(Dictionary<CtlExpanderItem, string> buddiesname, int modid);
        public event DelMCodMultipleBuddies EntMCodMultipleBuddies;

        #endregion

        #region BuddySingleClick + Widget        

        #endregion

        #region Floating Declaration

        public delegate void DelCloseBuddy();
        public event DelCloseBuddy EntCloseBuddy;

        public delegate void DelBeginAnimation(double From, double To);
        public event DelBeginAnimation EntBeginAnination;

        bool loaded;
        bool bWantsFloat = false;
        bool FirstClick = true;
       

        #endregion

        #region Performance

        BackgroundWorker bwBuddiesWid = new BackgroundWorker();

        List<ClsModuleLogic> lstWid = new List<ClsModuleLogic>();


        #endregion

        public CtlBuddies()
        {
                #region Multiple Buddy Selection

            try
            {
                this.InitializeComponent();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SucessfulLogin").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(CtlBuddyList_VMuktiEvent_SucessfulLogin);
                VMuktiAPI.VMuktiHelper.RegisterEvent("LogoutBuddyList").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(CtlBuddyList_VMuktiEvent_LogoutBuddyList);
                this.Unloaded += new RoutedEventHandler(CtlBuddyList_Unloaded);
                Application.Current.Exit += new ExitEventHandler(Current_Exit);
                this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
                btnFind.Click += new RoutedEventHandler(btnFind_Click);
                txtAddBuddies.KeyDown += new System.Windows.Input.KeyEventHandler(txtAddBuddies_KeyDown);
                delAddNewBuddy = new DelAddNewBuddy(AddNewBuddy);
                delFindBuddy = new DelFindBuddy(FindBuddy);
                

                objExpConBuddies.EntCModBuddies += new CtlExpanderContent.DelCModBuddies(objExpConBuddies_EntCModBuddies);

                #endregion                

                #region Buddy
                this.MouseEnter += new System.Windows.Input.MouseEventHandler(CtlModules_MouseEnter);
                this.MouseLeave += new System.Windows.Input.MouseEventHandler(CtlModules_MouseLeave);
                btnFloting1.Click += new RoutedEventHandler(btnFloting_Click);
                btnClose.Click += new RoutedEventHandler(btnClose_Click);
                #endregion

                bwBuddiesWid.DoWork += new DoWorkEventHandler(bwBuddiesWid_DoWork);
                bwBuddiesWid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwBuddiesWid_RunWorkerCompleted);

                bwBuddiesWid.RunWorkerAsync();


                VMuktiAPI.VMuktiHelper.RegisterEvent("FindBuddy").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlBuddies_VMuktiEvent_FindBuddy);
                VMuktiAPI.VMuktiHelper.RegisterEvent("StopTimer").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlBuddies_VMuktiEvent);
                delBuddyOffline = new DelBuddyOffline(fncBuddyOffline);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CtlBuddies()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void btnFind_Click(object sender, RoutedEventArgs e)
        {
            Find_Buddy fb = new Find_Buddy();
            if (txtAddBuddies.Text != "")
            {
                if (txtAddBuddies.Text.Contains("@"))
                {
                    fb.txtEMailID.Text = txtAddBuddies.Text;
                    fb.btnSearch_Click(null, null);
                }
                else
                {
                    fb.txtUserName.Text = txtAddBuddies.Text;
                    fb.btnSearch_Click(null, null);
                }
            }
               fb.Show();
        }

        void txtAddBuddies_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    btnAdd_Click(null, null);
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "txtAddBuddies_KeyDown()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }


        /// <summary>
        /// This region is used for adding new buddy after sucessful login into the system.
        /// </summary>
        #region Adding New Buddy

        bool isAlreadyClick = false;
        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (isUserLogin)
            {
                if (txtAddBuddies.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Buddy Name");
                    return;
                }
                else if (txtAddBuddies.Text.ToLower() == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName.ToLower())
                {
                    MessageBox.Show("You can not add yourself in your buddylist");
                    txtAddBuddies.Text = string.Empty;
                    return;
                }
                else if (!isAlreadyClick)
                {
                    isAlreadyClick = true;
                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread4Adding));
                    t.IsBackground = false;
                    t.Priority = System.Threading.ThreadPriority.Normal;
                    //t.SetApartmentState(System.Threading.ApartmentState.MTA);
                    t.Start();
                }
            }
            else
            {
                MessageBox.Show("Please login for adding buddies");
            }

            //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delAddNewBuddy);
        }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "btnAdd_Click", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }


        void StartThread4Adding()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delAddNewBuddy);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "StartThread4Adding()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }      

        void AddNewBuddy()
        {
            try
            {


                isAlreadyClick = true;
                for (int i = 0; i < objExpConBuddies.trvExpanderContent.Items.Count; i++)
                {

                    string str = ((CtlExpanderItem)((TreeViewItem)objExpConBuddies.trvExpanderContent.Items[i]).Header).Caption;
                    if (str.Split('-')[0].ToLower() == txtAddBuddies.Text.Trim().ToLower())
                    {
                        MessageBox.Show("Requested Buddy already added to your buddylist");
                        return;
                    }
                }

                //dt_RefreshBuddyList.Stop();

                // ** Adding New buddy 

                //System.Text.StringBuilder sb6 = new System.Text.StringBuilder();
                //sb6.AppendLine("Buddy:");
                //sb6.AppendLine("Adding New buddy .");
                //sb6.AppendLine("Buddy name :" + txtAddBuddies.Text);
                //sb6.AppendLine(sb1.ToString());
                //ClsLogging.WriteToTresslog(sb6);
                

                string sRequestedUser = App.chHttpBootStrapService.svcHttpAddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtAddBuddies.Text.Trim());
                if (sRequestedUser != string.Empty)
                {
                    // ** New buddy Added successfully
                   
                    // Buddy name :  txtAddBuddies.Text
                    // Buddy Status = strStatus
                    string strStatus = sRequestedUser.Substring(sRequestedUser.LastIndexOf('-') + 1);
                    
                    //System.Text.StringBuilder sb7 = new System.Text.StringBuilder();
                    //sb7.AppendLine("Buddy:");
                    //sb7.AppendLine("New buddy Added successfully .");
                    //sb7.AppendLine("Buddy name :" + txtAddBuddies.Text);
                    //sb7.AppendLine("Buddy Status :" + strStatus);
                    //sb7.AppendLine(sb1.ToString());
                    //ClsLogging.WriteToTresslog(sb7);
                    objExpConBuddies.AddItem(txtAddBuddies.Text.Trim(), true, ImageType.MaleBuddy, strStatus.ToLower());
                    try
                    {
                        App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtAddBuddies.Text.Trim(), strStatus);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException exp)
                    {
                        VMuktiHelper.ExceptionHandler(exp, "AddNewBuddy", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (!App.blnNodeOff)
                        {
                        App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtAddBuddies.Text.Trim(), strStatus);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException exp)
                    {
                        VMuktiHelper.ExceptionHandler(exp, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (!App.blnNodeOff)
                        {
                        App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, txtAddBuddies.Text.Trim(), strStatus);
                        }
                    }

                    txtAddBuddies.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("User have to login atleast once after registration");
                }

            }

            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
            finally
            {
                //  dt_RefreshBuddyList.Start();
                isAlreadyClick = false;
            }

        }

        #endregion

        void FindBuddy()
        {
            try
            {


                isAlreadyClick = true;
                for (int i = 0; i < objExpConBuddies.trvExpanderContent.Items.Count; i++)
                {

                    string str = ((CtlExpanderItem)((TreeViewItem)objExpConBuddies.trvExpanderContent.Items[i]).Header).Caption;
                    // if (str.Split('-')[0].ToLower() == txtAddBuddies.Text.Trim().ToLower())
                    if (str.Split('-')[0].ToLower() == temp)
                    {
                        MessageBox.Show("Requested Buddy already added to your buddylist");
                        return;
                    }
                }

                //dt_RefreshBuddyList.Stop();

                // ** Adding New buddy 

                //sb1 = CreateTressInfo();
                //System.Text.StringBuilder sb6 = new System.Text.StringBuilder();
                //sb6.AppendLine("Buddy:");
                //sb6.AppendLine("Adding New buddy .");
                //sb6.AppendLine("Buddy name :" + temp);
                //sb6.AppendLine(sb1.ToString());
                //ClsLogging.WriteToTresslog(sb6);
                // Buddy name :  txtAddBuddies.Text


                string sRequestedUser = App.chHttpBootStrapService.svcHttpAddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, temp.Trim());
                if (sRequestedUser != string.Empty)
                {
                    // ** New buddy Added successfully

                    // Buddy name :  txtAddBuddies.Text
                    // Buddy Status = strStatus
                    string strStatus = sRequestedUser.Substring(sRequestedUser.LastIndexOf('-') + 1);
                    //sb1 = CreateTressInfo();
                    //System.Text.StringBuilder sb7 = new System.Text.StringBuilder();
                    //sb7.AppendLine("Buddy:");
                    //sb7.AppendLine("New buddy Added successfully .");
                    //sb7.AppendLine("Buddy name :" + temp);
                    //sb7.AppendLine("Buddy Status :" + strStatus);
                    //sb7.AppendLine(sb1.ToString());
                    //ClsLogging.WriteToTresslog(sb7);
                    objExpConBuddies.AddItem(temp.Trim(), true, ImageType.MaleBuddy, strStatus.ToLower());
                    try
                    {
                        App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, temp.Trim(), strStatus);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException exp)
                    {
                        VMuktiHelper.ExceptionHandler(exp, "FindBuddy", "CtlBuddies.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (!App.blnNodeOff)
                        {
                            App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, temp.Trim(), strStatus);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException exp)
                    {
                        VMuktiHelper.ExceptionHandler(exp, "Find_Buddy", "CtlBuddies.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (!App.blnNodeOff)
                        {
                            App.chHttpSuperNodeService.AddBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, temp.Trim(), strStatus);
                        }
                    }

                    //txtAddBuddies.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("User have to login atleast once after registration");
                }

            }

            catch (Exception exp)
            {
               VMuktiHelper.ExceptionHandler(exp,"FindBuddy","CtlBuddies.xaml.cs");


            }
            finally
            {
                //  dt_RefreshBuddyList.Start();
                isAlreadyClick = false;
            }

        }

        /// <summary>
        /// This function will execute when user signout from the system or close the applicaiton
        /// </summary>
        public void LoadBuddyList()
        {
            //objBuddyList.LoadBuddyList();
        }

        /// <summary>
        /// this function will be executed when user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                //ClsException.WriteToLogFile("Current_Exit  -- CtlBuddies.xaml.cs  " + DateTime.Now); 
                LogOutBuddyList();
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SucessfulLogin");
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("LogoutBuddyList");
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("RemoveBuddy");

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "Current_Exit()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void CtlBuddyList_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ClsException.WriteToLogFile("ctlBuddyList_Unloaded  -- CtlBuddies.xaml.cs  " + DateTime.Now); 
                LogOutBuddyList();
               // VMuktiAPI.VMuktiHelper.UnRegisterEvent("SucessfulLogin");
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("LogoutBuddyList");
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("RemoveBuddy");
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "ctlBuddyList_Unload()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
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
                isUserLogin = false;
                objExpConBuddies.trvExpanderContent.Items.Clear();
                

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "LogOutBuddyList()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        /// <summary>
        /// This is Vmukti Event which will fire at the time of LogOut application. In this event buddylist refresh timer will stop and all buddy list will be clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtlBuddyList_VMuktiEvent_LogoutBuddyList(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                //ClsException.WriteToLogFile("CtlBuddyList_VMuktiEvent_LogoutBuddyList  -- CtlBuddies.xaml.cs  " + DateTime.Now); 
                LogOutBuddyList();

                

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CtlBuddyList_VMuktiEvent_LogoutBuddyList()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        /// <summary>
        /// This is Vmukti Event which will fire at the time of sucessfull login.It will start the refresh buddy list timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtlBuddyList_VMuktiEvent_SucessfulLogin(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            
            try
            {
                //ClsException.WriteToLogFile("called successful login event in buddies");
                isUserLogin = true;
                fncStartTimer();
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CtlBuddyList_VMuktiEvent_SuccessfullLogin", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        # region New Functions for Buddylist

        string strFilePath = string.Empty;
        string appPath = string.Empty;
        System.Windows.Threading.DispatcherTimer dt_RefreshBuddyList = null;

        delegate void DelTimerTick();
        DelTimerTick delDelTimerTick = null;

        delegate void DelRefreshBuddyList();
        DelRefreshBuddyList delRefreshBuddyList = null;

        public delegate void DelAsyncGetBuddy(List<string> lstMsg);
        public DelAsyncGetBuddy objDelAsyncGetBuddy;

        void fncStartTimer()
        {
            try
            {
                //ClsException.WriteToLogFile("fncStartTimer called from successful login event in buddies");
                // ** Starting BuddyList Refreshing Timer
                //System.Text.StringBuilder sb7 = new System.Text.StringBuilder();
                //sb7.AppendLine("Buddy:");
                //sb7.AppendLine("Starting BuddyList Refreshing Timer .");
                //sb7.AppendLine(sb1.ToString());
                //ClsLogging.WriteToTresslog(sb7);

                dt_RefreshBuddyList = new System.Windows.Threading.DispatcherTimer();
                dt_RefreshBuddyList.Interval = TimeSpan.FromSeconds(30);
                dt_RefreshBuddyList.Tick += new EventHandler(dt_RefreshBuddyList_Tick);
                dt_RefreshBuddyList.IsEnabled = true;
              
                delDelTimerTick = new DelTimerTick(fncGetBuddyList);
                //ClsException.WriteToLogFile("calling fncGetBuddyList of delegate delDelTimerTick");
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delDelTimerTick);
                
                appPath = AppDomain.CurrentDomain.BaseDirectory;
                delRefreshBuddyList = new DelRefreshBuddyList(RefreshBuddyList);
                objDelAsyncGetBuddy = new DelAsyncGetBuddy(delAsyncGetBuddy);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "fncStartTimer()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }


        }

        #region Refreshing Buddy Status
        // For SuperNode Buddy RefreshTick.
        List<string> fncGetBuddyStatus(string uName)
        {
            string ClientConnectionString = @"Data Source=" + System.IO.Path.GetDirectoryName(appPath) + "\\SuperNodeBuddyInfo.sdf";
            SqlCeConnection conn = new SqlCeConnection(ClientConnectionString);
            conn.Open();
            List<string> lstOnlineBuddies = new List<string>();
            try
            {
                string str = "SELECT * FROM User_BuddyList Where UserName ='" + uName + "'";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, conn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstOnlineBuddies.Add(dataTable.Rows[i]["BuddyName"].ToString() + "-" + dataTable.Rows[i]["BuddyStatus"].ToString());
                }
                dataAdapter.Dispose();
                conn.Close();
                conn.Dispose();
                return lstOnlineBuddies;
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "fncGetBuddyStatus()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");                
                conn.Close();
                conn.Dispose();
                return lstOnlineBuddies;
            }

        }

        /// <summary>
        /// Timer with tick every 5 seconds for refreshing the buddy status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dt_RefreshBuddyList_Tick(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                //System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(StartThread));
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;                
                //t.SetApartmentState(System.Threading.ApartmentState.MTA);
                t.Start();


            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "fncStartTimer()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }


        /// <summary>
        /// Thread to fetch the buddy status asynchronously.
        /// </summary>
        void StartThread()
        {
            try
            {
                //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delRefreshBuddyList);
                //ClsException.WriteToLogFile("Calling BeginGetBuddies for :" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                App.chHttpSuperNodeService.BeginGetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnBuddyComp, null);
                //ClsException.WriteToLogFile("Called BeginGetBuddies for :" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (System.ServiceModel.CommunicationException exCumm)
            {
                VMuktiHelper.ExceptionHandler(exCumm, "StartThread()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                if (!App.blnNodeOff)
                    {
                App.chHttpSuperNodeService.BeginGetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnBuddyComp, null);
                    }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "StartThread()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }


        /// <summary>
        /// Return value of asynchronous http wcf function.
        /// </summary>
        /// <param name="result"></param>
        void OnBuddyComp(IAsyncResult result)
        {
            try
            {
                //ClsException.WriteToLogFile("Calling EndGetBuddies ");
                List<string> lstMsg = App.chHttpSuperNodeService.EndGetBuddies(result);
                //ClsException.WriteToLogFile("Called EndGetBuddies ");
                result.AsyncWaitHandle.Close();
                //ClsException.WriteToLogFile("calling dispatcher for get buddies");
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objDelAsyncGetBuddy, lstMsg);
                //ClsException.WriteToLogFile("called dispatcher for get buddies");
            }
            catch (System.ServiceModel.EndpointNotFoundException exEndPnt)
            {
                VMuktiHelper.ExceptionHandler(exEndPnt, "OnBuddyComp()--EndPoint", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                //ClsException.WriteToLogFile("OnBuddyComp -:- EndpointNotFoundException -:- 10004 " + exEndPnt.Message.Contains("10004").ToString());
                if (exEndPnt.Message.Contains("10004"))
                {
                    dt_RefreshBuddyList.Start();
                }

                else
                {
                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                }


               // App.chHttpSuperNodeService.BeginGetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnBuddyComp, null);
            }
            catch (System.ServiceModel.CommunicationException exCumm)
            {
                VMuktiHelper.ExceptionHandler(exCumm, "OnBuddyComp()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                //ClsException.WriteToLogFile("OnBuddyComp -:- CommunicationException -:- 10004 " + exCumm.Message.Contains("10004").ToString());
                if (exCumm.Message.Contains("10004"))
                {
                    dt_RefreshBuddyList.Start();
                }

                else
                {
                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                }
               // App.chHttpSuperNodeService.BeginGetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnBuddyComp, null);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "OnBuddyComp()--Exception", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        /// <summary>
        /// Dispatcher to update the status of buddy with gui.
        /// </summary>
        /// <param name="lstuddyStatus"></param>
        void delAsyncGetBuddy(List<string> lstuddyStatus)
        {
            try
            {


                for (int j = 0; j < lstuddyStatus.Count; j++)
                {
                    string[] strBuddyStatus = lstuddyStatus[j].Split('-');
                    if (strBuddyStatus[0] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        lstuddyStatus.RemoveAt(j);
                        break;
                    }
                }


                //if (objExpConBuddies.trvExpanderContent.Items.Count > lstuddyStatus.Count)
                //{
                //    objExpConBuddies.RemoveBuddy(lstuddyStatus);
                //}
                for (int i = 0; i < lstuddyStatus.Count; i++)
                {
                    string[] strBuddyStatus = lstuddyStatus[i].Split('-');
                    if (strBuddyStatus[0] != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        objExpConBuddies.IsItemAdded(strBuddyStatus[0], strBuddyStatus[1].ToLower());
                    }

                }


            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "dt_RefreshBuddyList_Tick() BootStrap or Supernode is Down", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
            finally
            {
                dt_RefreshBuddyList.Start();
            }

        }

        //void StartThread()
        //{
        //    try
        //    {
        //        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delRefreshBuddyList);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Add("My Key", "fncStartTimer()--:--CtlBuddies.xmal.cs--:--" + ex.Message + " :--:--");
        //        ClsException.LogError(ex);
        //        ClsException.WriteToErrorLogFile(ex);
        //    }

        //}

        void RefreshBuddyList()
        {
            try
            {
                List<string> lstuddyStatus = new List<string>();
                //if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                //{
                //    lstuddyStatus = App.chHttpSuperNodeService.GetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //}
                //else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                //{
                //    // Change is no timer tick here only through its own database
                //    //lstuddyStatus = App.chHttpBootStrapService.svcHttpGetSuperNodeBuddyList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //    lstuddyStatus = fncGetBuddyStatus(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //}
                //lstCheckBuddyStatus.Clear();
                try
                {
                  //  ClsException.WriteToLogFile("Start Buddy Status Request :: " + DateTime.Now.ToString());
                    lstuddyStatus = App.chHttpSuperNodeService.GetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                 //   ClsException.WriteToLogFile("End Buddy Status Request :: " + DateTime.Now.ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");

                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (!App.blnNodeOff)
                    {
                        //ClsException.WriteToLogFile("Calling GetBuddies for endpoint not found");
                        lstuddyStatus = App.chHttpSuperNodeService.GetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        //ClsException.WriteToLogFile("Called GetBuddies for endpoint not found");
                    }
                }
                catch (System.ServiceModel.CommunicationException exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (!App.blnNodeOff)
                    {
                        //ClsException.WriteToLogFile("Calling GetBuddies for communication");
                        lstuddyStatus = App.chHttpSuperNodeService.GetBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        //ClsException.WriteToLogFile("Calling GetBuddies for communication");
                    }
                }
                

                for (int i = 0; i < lstuddyStatus.Count; i++)
                {
                    string[] strBuddyStatus = lstuddyStatus[i].Split('-');
                    if (strBuddyStatus[0] != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        objExpConBuddies.IsItemAdded(strBuddyStatus[0], strBuddyStatus[1].ToLower());
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "dt_RefreshBuddyList_Tick()--BootStrap or Supernode is down", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
            finally
            {
                if (!App.blnNodeOff)
                {
                    dt_RefreshBuddyList.Start();
                }
            }

        }

        #endregion

        // Fill List at first time when login.
        //first function called after login for buddy management.
        private void fncGetBuddyList()
        {
            try
            {
                //ClsException.WriteToLogFile("called fncGetBuddyList ");
                objExpConBuddies.trvExpanderContent.Items.Clear();

                List<string> lsBuddies = new List<string>();

                try
                {
                    // ** Taking My Buddy First time
                    
                    //System.Text.StringBuilder sb7 = new System.Text.StringBuilder();
                    //sb7.AppendLine("Buddy:");
                    //sb7.AppendLine("Taking My Buddy First time.");
                    //sb7.AppendLine(sb1.ToString());
                    //ClsLogging.WriteToTresslog(sb7);

                    //ClsException.WriteToLogFile("calling http bs svcHttpGetSuperNodeBuddyList");
                    lsBuddies = App.chHttpBootStrapService.svcHttpGetSuperNodeBuddyList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    //ClsException.WriteToLogFile("called http bs svcHttpGetSuperNodeBuddyList");
                    
                }
                catch (System.ServiceModel.EndpointNotFoundException ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (!App.blnNodeOff)
                    {
                    lsBuddies = App.chHttpBootStrapService.svcHttpGetSuperNodeBuddyList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }
                catch (System.ServiceModel.CommunicationException exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (!App.blnNodeOff)
                    {
                    lsBuddies = App.chHttpBootStrapService.svcHttpGetSuperNodeBuddyList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }

                for (int i = 0; i < lsBuddies.Count; i++)
                {
                    string[] strBuddyStatus = lsBuddies[i].Split('-');
                    if (strBuddyStatus[0] != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName) //&& !lstBuddies.Items.Contains(lsBuddies[i].ToString()))
                    {
                        objExpConBuddies.AddItem(strBuddyStatus[0], true, ImageType.MaleBuddy, strBuddyStatus[1].ToLower());
                    }
                }
                try
                {
                    //ClsException.WriteToLogFile("calling http sn AddNodeBuddies");
                    App.chHttpSuperNodeService.AddNodeBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lsBuddies);
                    //ClsException.WriteToLogFile("called http sn AddNodeBuddies");
                }
                catch (System.ServiceModel.EndpointNotFoundException ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (!App.blnNodeOff)
                    {
                    App.chHttpSuperNodeService.AddNodeBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lsBuddies);
                    }
                }
                catch (System.ServiceModel.CommunicationException exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "AddNewBuddy()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    App.chHttpSuperNodeService.AddNodeBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lsBuddies);
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "fncGetBuddyList()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");                
            }
        }

        #endregion

        #region Multiple Buddy Selection

        void objExpConBuddies_EntCModBuddies(Dictionary<CtlExpanderItem, string> buddiesname, int modid)
        {
            try
            {
                if (EntMCodMultipleBuddies != null)
                {
                    EntMCodMultipleBuddies(buddiesname, modid);
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "objExpConBuddies_EntCModBuddies()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }
        #endregion

        #region BuddySingleClick + Widgets

        void btnWidget_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<CtlExpanderItem, string> onlinebuddies = new Dictionary<CtlExpanderItem, string>();


            foreach (KeyValuePair<CtlExpanderItem, string> strBName in objExpConBuddies.selectedExItems)
            {
                if (((CtlExpanderItem)strBName.Key).Tag.ToString().Trim() == "online")
                {
                    onlinebuddies.Add((CtlExpanderItem)strBName.Key, strBName.Value);
                }
            }

            if (EntMCodMultipleBuddies != null)
            {
                if (onlinebuddies.Count > 0)
                {
                    EntMCodMultipleBuddies(onlinebuddies, int.Parse(((Button)e.Source).Tag.ToString()));
                }
            }
        }

        void WidgetToolBarVisibility(string text)
        {
            if (text == "Show")
            {
                rowDef3.MinHeight = 30.0;
                tbtWidgets.Visibility = Visibility.Visible;
            }
            else if (text == "Hide")
            {
                rowDef3.MinHeight = 0.0;
                tbtWidgets.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            if (EntCloseBuddy != null )
            {
                EntCloseBuddy = null;
            }
            if (EntBeginAnination != null)
            {
                EntBeginAnination = null;

            }
        }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "Dispose()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }
        #endregion      

        #region Floating

        void btnFloting_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (FirstClick)
                {
                    bWantsFloat = true;
                    FirstClick = false;
                    ((RotateTransform)(((TransformGroup)(cnvImage.GetValue(Canvas.RenderTransformProperty))).Children[2])).Angle = 270;
                }
                else
                {
                    bWantsFloat = false;
                    FirstClick = true;
                    ((RotateTransform)(((TransformGroup)(cnvImage.GetValue(Canvas.RenderTransformProperty))).Children[2])).Angle = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnFloting_Click()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void CtlModules_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (!pgHome.ctlBuddyClosed && bWantsFloat && EntBeginAnination != null)
                {

                    EntBeginAnination(170, 30);

                    cnvFind.Visibility = Visibility.Hidden;
                    cnvMain.Visibility = Visibility.Hidden;
                    spContent.Visibility = Visibility.Hidden;
                    brdMainGlass1.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModules_MouseLeave()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void CtlModules_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (!pgHome.ctlBuddyClosed)
                {
                    if (loaded && bWantsFloat)
                    {
                        brdMainGlass1.Visibility = Visibility.Hidden;
                        cnvFind.Visibility = Visibility.Visible;

                        if (EntBeginAnination != null)
                        {
                            EntBeginAnination(30, 170);
                        }

                        cnvMain.Visibility = Visibility.Visible;
                        spContent.Visibility = Visibility.Visible;
                        cnvFind.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        loaded = true;
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModules_MouseEnter()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }

        }

        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EntCloseBuddy != null)
                {
                    cnvMain.Visibility = Visibility.Collapsed;
                    EntCloseBuddy();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        #endregion

        #region Disaster Management

        void fncBuddyOffline()
        {
            try
            {
                //ClsException.WriteToLogFile("In the delegate for refreshing buddylist");
                for (int i = 0; i < objExpConBuddies.trvExpanderContent.Items.Count; i++)
                {
                    string[] strBuddyStatus = ((CtlExpanderItem)((TreeViewItem)objExpConBuddies.trvExpanderContent.Items[i]).Header).Caption.Split('-');
                    if (strBuddyStatus[0] != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        objExpConBuddies.IsItemAdded(strBuddyStatus[0], "offline");
                    }
                }
                //ClsException.WriteToLogFile("In the delegate for refreshing buddylist completed");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncBuddyOffline()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void CtlBuddies_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
               
                if (((bool)e._args[0]))
                {
                    if (dt_RefreshBuddyList != null)
                    {
                        //ClsException.WriteToLogFile("Going to stop the timer for refreshing buddylist");
                        dt_RefreshBuddyList.Stop();
                        //ClsException.WriteToLogFile("Stopped the timer for refreshing buddylist");

                    }
                    //ClsException.WriteToLogFile("Invoking delegate for refreshing buddylist");
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delBuddyOffline);
                    //ClsException.WriteToLogFile("Invoked delegate for refreshing buddylist");
                }
                else
                {
                    if (dt_RefreshBuddyList != null && (!dt_RefreshBuddyList.IsEnabled))
                    {
                        dt_RefreshBuddyList.Start();
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delDelTimerTick);

                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlBuddies_VMuktiEvent()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void CtlBuddies_VMuktiEvent_FindBuddy(object sender, VMuktiEventArgs e)
        {
            try
            {
                if ((string)(e._args[0]) != null)
                {
                    temp = (string)e._args[0];

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delFindBuddy);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlBuddies_VMuktiEvent_FindBuddy", "CtlBuddies.xaml.cs");
            }
        }

        #endregion

        #region Performance

        void bwBuddiesWid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try
            {
                for (int i = 0; i < App.lstCollOnly.Count; i++)
                {
                    if (App.lstCollOnly[i].ImageFile != null && App.lstCollOnly[i].ImageFile.Length > 0)
                    {
                        Button btnWidget = new Button();

                        BitmapImage bimg = new BitmapImage();

                        bimg.BeginInit();
                        bimg.StreamSource = new MemoryStream(App.lstCollOnly[i].ImageFile);
                        bimg.EndInit();

                        Image img = new Image();
                        img.Source = bimg;
                        img.Height = 20;
                        img.Width = 20;
                        btnWidget.ToolTip = App.lstCollOnly[i].ModuleTitle;
                        btnWidget.Content = img;

                        btnWidget.Tag = App.lstCollOnly[i].ModuleId;
                        btnWidget.Click += new RoutedEventHandler(btnWidget_Click);
                        btnWidget.Margin = new Thickness(2, 0, 0, 0);
                        tbWidgets.Items.Add(btnWidget);
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "bwBuddiesWid_RunWorkerCompleted()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
            }
        }

        void bwBuddiesWid_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region BuddySingleClick + Widget

                ClsModuleCollection cmc = null;


                //if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                //{
                //    if (Business.clsDataBaseChannel.OpenDataBaseClient())
                //    {
                //        cmc = ClsModuleCollection.GetOnlyCollMod();
                //    }
                //}
                //else
                //{
                //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                //    {
                //        if (Business.clsDataBaseChannel.OpenDataBaseClient())
                //        {
                //            cmc = ClsModuleCollection.GetOnlyCollMod();
                //        }
                //    }
                //    else
                //    {
                //        cmc = ClsModuleCollection.GetOnlyCollMod();
                //    }
                //}

                cmc = ClsModuleCollection.GetOnlyCollMod();

                if (cmc != null)
                {
                    for (int i = 0; i < cmc.Count; i++)
                    {
                        try
                        {
                            //lstWid.Add(cmc[i]);
                            //ClsException.WriteToLogFile("BUDDIES " + DateTime.Now.ToString());
                            App.lstCollOnly.Add(cmc[i]);
                        }
                        catch (Exception exp)
                        {
                            VMuktiHelper.ExceptionHandler(exp, "CtlBuddies()", "Controls\\BuddyExplorer\\CtlBuddies.xaml.cs");
                        }
                    }
                }
                #endregion
            }
            catch (Exception exp)
            {
                MessageBox.Show("bwBuddiesWid_DoWork" + exp.Message);
            }
        }

        #endregion

    }
}
