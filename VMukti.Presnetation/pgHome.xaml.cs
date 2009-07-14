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
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using VMukti.Business;
using VMukti.Business.CommonDataContracts;
using VMukti.Business.WCFServices.SuperNodeServices.BasicHttp;
using VMukti.Business.WCFServices.SuperNodeServices.NetP2P;
using VMukti.CtlResizer.Presentation;
using VMukti.Presentation.Controls;
using VMuktiAPI;
using VMuktiService;
using System.Net;
using System.Text;
using System.Diagnostics;
using VMukti.Business.WCFServices.BootStrapServices.NetP2P;
using VMuktiGrid.CustomGrid;
using VMuktiGrid.Buddy;
using System.ComponentModel;
using System.Reflection;
using VMukti.Presentation.Xml;
using System.Collections;
using VMukti.Business.CommonMessageContract;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using VMukti.Business.WCFServices.BootStrapServices.BasicHttp;
using WMEncoderLib;
using System.Globalization;
using System.Linq;
using VMuktiGrid.CustomMenu;


namespace VMukti.Presentation
{
    public partial class pgHome : Page, IDisposable
    {

        VMuktiGrid.ctlVMuktiGrid objVMuktiGrid;
        NetPeerClient npcP2PSuperNode;

        #region Win32

        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        [DllImport("Kernel32.dll")]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        [DllImport("user32.dll")]
        static extern bool FlashWindowEx(IntPtr hWnd);

        #endregion


        System.Windows.Threading.DispatcherTimer dispTimer4DomainLoading = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        private DispatcherTimer dispTmrCheckStatus = new DispatcherTimer();

        public delegate void DelGetMessage();
        public DelGetMessage objDelGetMsg;

        public delegate void DelPageSpecialMsg(clsPageInfo objPageInfo);
        public DelPageSpecialMsg objDelPageSpecialMsg;

        public delegate void DelPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo);
        public DelPageBuddyRetSetSpecialMsg objDelPageBuddyRetSetSpecialMsg;

        bool disposed;

        public delegate void DelRemoveDraggedBuddy(clsModuleInfo objPageInfo, string from);
        public DelRemoveDraggedBuddy objDelRemoveDraggedBuddy;

        public delegate void DelAsyncGetMessage(List<clsMessage> lstMsg);
        public DelAsyncGetMessage objDelAsyncGetMsg;

        CtlSettings objSetting;
        CtlViewProfile objViewProfile;

        Thread thGlobalVariable;


        #region Disaster Recovery

        public delegate void DelBandWidthUsage();
        DelBandWidthUsage delBandWidthUsage;

        #endregion

        #region Monitoring System

        object objNetTcpConsole;
        INetP2PConsoleChannel objChannel;

        bool blnISConsole;

        Process pConsole;
        ProcessStartInfo psiConsole;
        NetPeerClient npc;

        byte[] arr;
        byte[] Larr;

        Dictionary<string, Stream> dictConsoleLog;

        #endregion

        #region Download Zip For Nodes

        BackgroundWorker bwDownloadZips;
        ArrayList al;
        Thread thZipFiles;

        #endregion

        #region Bandwidth

        DispatcherTimer dtBandwidth = new DispatcherTimer();

        delegate void DelBandwidth();
        DelBandwidth objBandwidth;

        double dblbandwidthdl;
        double dblbandwidthul;
        SolidColorBrush bhOrig = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7C7C7C"));

        delegate void DelDownload(MContractRemoteFileInfo rfi);
        DelDownload objDelDownload;

        delegate void DelUpload();
        DelUpload objDelUpload;

        public IHttpFileUploadDownload clientHttpChannelBandwidth;
        BasicHttpClient bhcFileTransfer;

        DateTime dtDLStart;
        DateTime dtULStart;
        DateTime dtDLEnd;
        DateTime dtULEnd;

        int count;
        long lglength;

        #endregion

        #region Multiple Buddy Selection

        public delegate void DelSetSpecialMsg4Buddies(clsModuleInfo objModInfo);
        public DelSetSpecialMsg4Buddies objDelSetSpecialMsg4Buddies;

        public delegate void DelSetSpecialMsgBuddiesClick(clsModuleInfo objModInfo, clsPageInfo objPageInfo);
        public DelSetSpecialMsgBuddiesClick objDelSetSpecialMsgBuddiesClick;


        #region Floating
        public static bool ctlBuddyClosed = false;
        public static bool ctlModuleClosed = false;
        #endregion

        #endregion

        #region MeetingSchedular

        public delegate void DelJoinConf(string from, List<string> lstArgs);
        public DelJoinConf objDelJoinConf;

        public delegate void DelSendConfInfo(clsPageInfo pageinfo);
        public DelSendConfInfo objDelSendConfInfo;

        public delegate void DelUnJoinConf(string from, int confid);
        public DelUnJoinConf objDelUnJoinConf;

        public delegate void DelAddConfBuddy(string from, int confid);
        public DelAddConfBuddy objDelAddConfBuddy;

        public delegate void DelRemoveBuddyConf(string from, int confid);
        public DelRemoveBuddyConf objDelRemoveBuddyConf;

        public delegate void DelPodNavigation(string from, List<int> lstIDs);
        public DelPodNavigation objDelPodNavigation;

        #endregion

        #region Performance

        #region For Login

        BackgroundWorker bwLoginPerf;

        public delegate void DelAuthorized();
        public DelAuthorized objDelAuthorized;

        public delegate void DelAuthorizeLogin();
        public DelAuthorizeLogin objDelAuthorizeLogin;

        #endregion


        #region For Grid

        BackgroundWorker bwGridLoad;

        public delegate void DelGridLoad();
        public DelGridLoad objDelGridLoad;


        public delegate void DelStartLoadingGrid();
        public DelStartLoadingGrid objDelStartLoadingGrid;

        #endregion


        #region For widgets

        BackgroundWorker bwLoadWidget;

        public delegate void DelLoadWidget();
        public DelLoadWidget objDelLoadWidget;


        public delegate void DelStartLoadingWid();
        public DelStartLoadingWid objDelStartLoadingWid;

        #endregion

        #endregion

        #region UI

        CtlThemPopUp objCtlThemPopUp;
        
        #endregion

        #region upper
        bool blOpenMExp = true;
        #endregion

        #region ShowPopup

        wndVMuktiPopup objPopup;

        #endregion

        public pgHome()
        {
            try
            {
                try
                {
                    InitializeComponent();

                    thGlobalVariable = new Thread(new ThreadStart(GlobalVariable));
                    thGlobalVariable.Start();

                    #region upper

                    objAmit.EntClosemodule += new CtlModule.delCloseModule(objAmit_entClosemodule);
                    objAmit.EntAutherized += new CtlModule.DelAutherized(objAmit_EntAutherized);
                    rowModule.Height = new GridLength(230, GridUnitType.Pixel);
                    objAmit.Visibility = Visibility.Visible;

                    #endregion

                    dispTimer4DomainLoading.Tick += new EventHandler(dispTimer4DomainLoading_Tick);
                    dispTimer4DomainLoading.Interval = TimeSpan.FromSeconds(1);

                    dispTmrCheckStatus.Interval = TimeSpan.FromSeconds(3);
                    dispTmrCheckStatus.Tick += new EventHandler(dispTmrCheckStatus_Tick);

                    this.ShowsNavigationUI = false;
                    Uri uriForums = new Uri("http://vmukti.com/index.php?option=com_fireboard&Itemid=87");
                    Uri uriTutorials = new Uri("http://youtube.com/results?search_query=vmukti&search_type=");
                    Uri uriSupport = new Uri("http://vmukti.com/index.php?option=com_content&view=article&id=130&Itemid=152");
                    bughlink.NavigateUri = uriForums;
                    tutorialhlink.NavigateUri = uriTutorials;
                    Supporthlink.NavigateUri = uriSupport;
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome()", "pgHome.xaml.cs");
                }

                #region Performance

                #region For Login

                bwLoginPerf = new BackgroundWorker();
                bwLoginPerf.DoWork += new DoWorkEventHandler(bwLoginPerf_DoWork);
                bwLoginPerf.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoginPerf_RunWorkerCompleted);

                objDelAuthorized = new DelAuthorized(Authorized);
                objDelAuthorizeLogin = new DelAuthorizeLogin(AuthorizedLogin);

                #endregion

                #region For Grid

                bwGridLoad = new BackgroundWorker();
                bwGridLoad.DoWork += new DoWorkEventHandler(bwGridLoad_DoWork);
                bwGridLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwGridLoad_RunWorkerCompleted);

                objDelGridLoad = new DelGridLoad(LoadGrid);
                objDelStartLoadingGrid = new DelStartLoadingGrid(StartLoadingGrid);

                #endregion

                #region For Widget

                bwLoadWidget = new BackgroundWorker();
                bwLoadWidget.DoWork += new DoWorkEventHandler(bwLoadWidget_DoWork);
                bwLoadWidget.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadWidget_RunWorkerCompleted);
                objDelLoadWidget = new DelLoadWidget(LoadWidget);
                objDelStartLoadingWid = new DelStartLoadingWid(StartLoadingWid);

                #endregion

                #endregion

                this.Loaded += new RoutedEventHandler(pgHome_Loaded);
                this.Unloaded += new RoutedEventHandler(pgHome_Unloaded);
                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                bwGridLoad.RunWorkerAsync();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOutFlag").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SendConsoleMessage").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_SendConsoleMsg);
                VMuktiAPI.VMuktiHelper.RegisterEvent("StopTimer").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_StopTimer);
                VMuktiAPI.VMuktiHelper.RegisterEvent("ViewLog").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_ViewLog);
                VMuktiAPI.VMuktiHelper.RegisterEvent("ViewProfile").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_ViewProfile);

                #region Bandwidth

                dtBandwidth.Interval = TimeSpan.FromSeconds(2.0);
                dtBandwidth.Tick += new EventHandler(dtBandwidth_Tick);

                //objDelDownload = new DelDownload(delBandwidthDownload);
                //objDelUpload = new DelUpload(delBandwidthUpload);
                //objBandwidth = new DelBandwidth(BandwidthUploadSpeed);

                VMuktiAPI.VMuktiHelper.RegisterEvent("BandwidthUsage").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_BandwidthUsage);

                #endregion

                #region Multiple Buddy Selection

                objBuddies.EntMCodMultipleBuddies += new CtlBuddies.DelMCodMultipleBuddies(objBuddies_EntMCodMultipleBuddies);

                //objDelSetSpecialMsg4Buddies = new DelSetSpecialMsg4Buddies(delSetSpecialMsg4Buddies);
                //objDelSetSpecialMsgBuddiesClick = new DelSetSpecialMsgBuddiesClick(delSetSpecialMsgBuddiesClick);

                #endregion

                #region Disaster Recovery

                //delBandWidthUsage = new DelBandWidthUsage(fncBandWidthUsage);

                #endregion

                #region MeetingSchedular


                objDelJoinConf = new DelJoinConf(JoinConf);
                objDelSendConfInfo = new DelSendConfInfo(SendConfInfo);

                objDelUnJoinConf = new DelUnJoinConf(UnJoinConf);
                objDelAddConfBuddy = new DelAddConfBuddy(AddConfBuddy);
                objDelRemoveBuddyConf = new DelRemoveBuddyConf(RemoveBuddyConf);

                objDelPodNavigation = new DelPodNavigation(PodNavigation);

                VMuktiAPI.VMuktiHelper.RegisterEvent("Conference").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_Conference);
                VMuktiAPI.VMuktiHelper.RegisterEvent("InstantMeeting").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(pgHome_VMuktiEvent_InstantMeeting);



                #endregion

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome()", "pgHome.xaml.cs");
            }
        }

        #region Global Variable Initialization

        void GlobalVariable()
        {
            try
            {
                disposed = false;

                objDelGetMsg = new DelGetMessage(delGetMessage);

                objDelPageSpecialMsg = new DelPageSpecialMsg(delPageSpecialMsg);
                objDelPageBuddyRetSetSpecialMsg = new DelPageBuddyRetSetSpecialMsg(delPageBuddyRetSetSpecialMsg);

                objDelRemoveDraggedBuddy = new DelRemoveDraggedBuddy(delRemoveDraggedBuddy);
                objDelAsyncGetMsg = new DelAsyncGetMessage(delAsyncGetMessage);

                #region Monitoring System

                objNetTcpConsole = new ClsNetP2PConsoleDelegates();
                blnISConsole = false;
                arr = new byte[5000];
                dictConsoleLog = new Dictionary<string, Stream>();

                #endregion

                #region Disaster Recovery

                delBandWidthUsage = new DelBandWidthUsage(fncBandWidthUsage);

                #endregion

                #region Download Zip For Nodes

                al = new ArrayList();

                #endregion

                #region Bandwidth

                dblbandwidthdl = 0;
                dblbandwidthul = 0;
                count = 0;

                objDelDownload = new DelDownload(delBandwidthDownload);
                objDelUpload = new DelUpload(delBandwidthUpload);
                objBandwidth = new DelBandwidth(BandwidthUploadSpeed);

                #endregion

                #region Multiple Buddy Selection

                objDelSetSpecialMsg4Buddies = new DelSetSpecialMsg4Buddies(delSetSpecialMsg4Buddies);
                objDelSetSpecialMsgBuddiesClick = new DelSetSpecialMsgBuddiesClick(delSetSpecialMsgBuddiesClick);

                #endregion

                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GlobalVariable()", "pgHome.xaml.cs");
            }
        }

        #endregion

        #region UINEW

        void ThemepopUp_entChangeTheme(string tag)
        {
            try
            {

                //u  objModules.btnClose_Click(null, null);
                #region change  upper
                objAmit_entClosemodule();
                #endregion
                objBuddies.btnClose_Click(null, null);
                if (tag == "Call Center")
                {
                    //objModules.LoadPage("CallCenter");
                    objAmit.LoadPage("CallCenter");
                    //ClsException.WriteToLogFile("Home Page: Theam change to CallCenter");
                }
                else
                {
                    // objModules.LoadPage("Meeting");
                    objAmit.LoadPage("Meeting");
                    //ClsException.WriteToLogFile("Home Page: Theam changed to Meeting");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ThemepopUp_entChangeTheme()", "pgHome.xaml.cs");
            }

        }

        #endregion

        void cpt_EntChannelRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo cmi)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    App.chNetP2PSuperNodeChannel.svcSetRemoveDraggedBuddy(from, to, msg, cmi, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
                else
                {
                    App.chHttpSuperNodeService.svcSetRemoveDraggedBuddy(from, to, msg, cmi);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cpt_EntChannelRemoveDraggedBuddy()", "pgHome.xaml.cs");
            }
        }

        void pgHome_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_VmuktiEvent()", "pgHome.xaml.cs");
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (!isEventOccured)
                {
                    LogOut();

                    isEventOccured = true;
                }
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp && VMukti.Business.clsDataBaseChannel.objHttpDataBase != null)
                {
                    VMukti.Business.clsDataBaseChannel.objHttpDataBase.CloseClient<IHttpBootStrapDataBaseService>();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "pgHome.xaml.cs");
            }
        }

        bool isEventOccured = false;

        void pgHome_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isEventOccured)
                {
                    LogOut();
                    isEventOccured = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "phHome_Unloaded()", "pgHome.xaml.cs");
            }
        }

        void dispTimer4DomainLoading_Tick(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sReader = new StreamReader(fs);
                string strStatus = sReader.ReadToEnd();
                sReader.Close();
                fs.Close();
                if (strStatus == "Initializing")
                {
                    dispTimer4DomainLoading.Stop();

                    try
                    {

                        bwLoadWidget.RunWorkerAsync();

                        //u objModules.EntPageItemSelectionChanged += new VMukti.Presentation.Controls.CtlModules.DelPageItemSelectionChanged(objModules_EntPageItemSelectionChanged);
                        objAmit.EntPageItemSelectionChanged += new CtlModule.DelPageItemSelectionChanged(objAmit_EntPageItemSelectionChanged);
                        //grdMain.Children.Add(objVMuktiGrid);


                        //objVMuktiGrid.SetValue(Grid.RowProperty, 2);
                        //objVMuktiGrid.SetValue(Grid.ColumnProperty, 1);
                        //objVMuktiGrid.SetValue(Grid.RowSpanProperty, 2);
                        brdVMuktiGridHost.Child = objVMuktiGrid;
                        objVMuktiGrid.SetValue(Grid.MarginProperty, new Thickness(0, 0, 0, 0));

                        cnvSettings.SetValue(Grid.ZIndexProperty, 2);
                        btnPane.SetValue(Grid.ZIndexProperty, 1);
                        objVMuktiGrid.SetValue(Grid.ZIndexProperty, 0);
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispTimer4DomainLoading_Tick()", "pgHome.xaml.cs");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Timertick4Domain()", "pgHome.xaml.cs");

            }

        }

        void objModules_EntAutherized()
        {
            try
            {
                isEventOccured = false;
                btnBList.Visibility = Visibility.Visible;
                btnBList.IsEnabled = true;
                btnSettings.Visibility = Visibility.Visible;
                btnLogin.Visibility = Visibility.Visible;
                btnMExp.Visibility = Visibility.Visible;
                tblkUserName.Visibility = Visibility.Visible;
                tblkUserName.Text = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                objBuddies.Visibility = Visibility.Visible;

                #region Bandwidth

                try
                {
                    OpenBandwidthClient();
                    dtBandwidth.Start();
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Bandwidth Client Opening()", "pgHome.xaml.cs");
                }
                #endregion



                #region Monitoring System

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    OpenConsoleClient();
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                    {
                        btnConsole.Visibility = Visibility.Visible;
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LogFiles");
                    }
                }

                #endregion

                //ClsException.WriteToLogFile("opening sn p2p channel in pghome if node is bs,sn or p2p" + DateTime.Now);
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                {
                    OpenSuperNodeClient();
                }
                //ClsException.WriteToLogFile("opened SuperNode p2p channel in pghome if node is bs,sn or p2p");

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    //ClsException.WriteToLogFile("if http node, starting the timer");
                    dispTmrCheckStatus.Start();
                    //ClsException.WriteToLogFile("if http node, started the timer");
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objModules_EntAutherized()", "pgHome.xaml.cs");
            }
            try
            {
                //objBuddies.LoadBuddyList();
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                {
                    btnSettings.Visibility = Visibility.Visible;
                }
                btnLoginText.Text = "Sign Out";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objModules_EntAutherized()", "pgHome.xaml.cs");
            }


        }

        void pgHome_EntsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModInfo, string IPAddress)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcAddPageDraggedBuddy()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcAddDraggedBuddy()", "pgHome.xaml.cs");
            }
        }

        void pgHome_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                objVMuktiGrid = new VMuktiGrid.ctlVMuktiGrid();
                dispTimer4DomainLoading.Start();
                isEventOccured = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_Loaded()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcPageSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objPageInfo.strTo)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelPageSpecialMsg, objPageInfo);

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcPageSetSpecialMsg()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objBuddyRetPageInfo.strFrom)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelPageBuddyRetSetSpecialMsg, objBuddyRetPageInfo);
                }
                for (int i = 0; i < objBuddyRetPageInfo.objaTabs[0].objaPods[0].straPodBuddies.Length; i++)
                {
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objBuddyRetPageInfo.objaTabs[0].objaPods[0].straPodBuddies[i])
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelPageBuddyRetSetSpecialMsg, objBuddyRetPageInfo);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcPageBuddyRetSetSpecialMsg()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcUnJoin(string uname)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcUnJoin()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == to)
                {

                    if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                    {
                        ((Window)this.Parent).BringIntoView();
                        Beep(750, 300);
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcSetSpecialMsg()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                for (int j = 0; j < to.Count; j++)
                {
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == to[j])
                    {
                        #region Pod Type

                        if (objModInfo.strDropType == "Pod Type")
                        {
                            object[] objarr = new object[1];
                            objarr[0] = from;
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelRemoveDraggedBuddy, objModInfo, objarr);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcSetRemoveDraggedBuddy()", "pgHome.xaml.cs");
            }
        }


        public void delRemoveDraggedBuddy(clsModuleInfo objModInfo, string from)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delRemoveDraggedBuddy()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcJoin(string uname)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcJoin()", "pgHome.xaml.cs");
            }
        }

        void objModules_EntPageItemSelectionChanged(string strTagText, string strContent)
        {
            try
            {
                objVMuktiGrid.LoadPage(int.Parse(strTagText));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objModules_EntPageItemSelectionChanged()", "pgHome.xaml.cs");
            }
        }

        private void btnPane_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((ScaleTransform)((TransformGroup)btnPane.GetValue(Button.RenderTransformProperty)).Children[0]).ScaleX == -1)
                {
                    //this.BeginStoryboard((Storyboard)this.Resources["OnCloseClick"], HandoffBehavior.Compose, false);
                    //rowDef1.Height = new GridLength(0);
                    GridLengthAnimation objani = new GridLengthAnimation();
                    objani.From = new GridLength(35, GridUnitType.Pixel);
                    objani.To = new GridLength(0, GridUnitType.Pixel);
                    objani.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.50));
                    rowDef1.BeginAnimation(RowDefinition.HeightProperty, objani);
                    btnPane.ToolTip = "Show ToolBar";
                }
                else
                {
                    //this.BeginStoryboard((Storyboard)this.Resources["OnOpenClick"], HandoffBehavior.Compose, true);
                    //rowDef1.Height = new GridLength(34.0, GridUnitType.Pixel);
                    GridLengthAnimation objani = new GridLengthAnimation();
                    objani.From = new GridLength(0, GridUnitType.Pixel);
                    objani.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.50));
                    objani.To = new GridLength(35, GridUnitType.Pixel);

                    rowDef1.BeginAnimation(RowDefinition.HeightProperty, objani);
                    btnPane.ToolTip = "Hide ToolBar";
                }
                ((ScaleTransform)((TransformGroup)btnPane.GetValue(Button.RenderTransformProperty)).Children[0]).ScaleX *= -1;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnPane_Click()", "pgHome.xaml.cs");
            }
        }



        void cpt_EntChannelSetMsg(string from, string to, string msg, clsModuleInfo cmi)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(from, to, msg, cmi, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                    App.chNetP2PSuperNodeChannel.svcAddDraggedBuddy(from, to, "Newly Dragged Buddy", cmi, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
                else
                {
                    App.chHttpSuperNodeService.svcSetSpecialMsgs(from, to, msg, cmi);
                    App.chHttpSuperNodeService.svcAddDraggedBuddy(from, to, "Newly Dragged Buddy", cmi);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cpt_EntChannelSetMsg()", "pgHome.xaml.cs");
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvSettings.Visibility = Visibility.Visible;
                objSetting.Visibility = Visibility.Visible;
                objViewProfile.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSettings_Click()", "pgHome.xaml.cs");
            }
        }
        void dispTmrCheckStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                //ClsException.WriteToLogFile("starting thread from timer tick");
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispTmrCheckStatus_Tick()", "pgHome.xaml.cs");
            }
        }

        void StartThread()
        {
            try
            {
                //ClsException.WriteToLogFile("calling http sn BeginsvcGetSpecialMsgs");
                App.chHttpSuperNodeService.BeginsvcGetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnComp, null);
                //ClsException.WriteToLogFile("called http sn BeginsvcGetSpecialMsgs");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartThread()", "pgHome.xaml.cs");
            }
        }

        void OnComp(IAsyncResult result)
        {
            try
            {
                List<clsMessage> lstMsg = App.chHttpSuperNodeService.EndsvcGetSpecialMsgs(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, objDelAsyncGetMsg, lstMsg);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnComp()", "pgHome.xaml.cs");
            }
        }

        void delAsyncGetMessage(List<clsMessage> lstMsg)
        {
            try
            {
                if (lstMsg.Count > 0)
                {
                    for (int i = 0; i < lstMsg.Count; i++)
                    {
                        if (lstMsg[i].strMessage == "OPEN_PAGE")
                        {
                            objVMuktiGrid.LoadMeetingPage(lstMsg[i].objPageInfo);
                            #region popup
                            FncShowPopup(lstMsg[i].objPageInfo);
                            #endregion
                            if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                            {
                                Beep(750, 300);
                            }
                        }

                        else if (lstMsg[i].strMessage == "OPEN_PAGE4MultipleBuddies")
                        {
                            //objVMuktiGrid.LoadNewMultipleBuddyPage(lstMsg[i].objClsModuleInfo);
                            objVMuktiGrid.LoadMeetingPage(lstMsg[i].objPageInfo);
                            #region popup
                            FncShowPopup(lstMsg[i].strFrom, lstMsg[i].objClsModuleInfo);
                            #endregion
                            if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                            {
                                Beep(750, 300);
                            }
                        }

                        else if (lstMsg[i].strMessage == "JoinConf")
                        {
                            if (lstMsg[i].strTo[0] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                foreach (KeyValuePair<int, int> kvp in VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser)
                                {
                                    if (kvp.Key == lstMsg[i].confid)
                                    {
                                        ClsConferenceCollection objGetConfInfo = ClsConferenceCollection.GetUserConferences(lstMsg[i].confid);
                                        clsPageInfo objPageInfo = new clsPageInfo();
                                        for (int pCnt = 0; pCnt < objVMuktiGrid.pageControl.Items.Count; pCnt++)
                                        {
                                            if (objGetConfInfo[0].PageID == ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).ObjectID)
                                            {
                                                VMuktiGrid.ctlPage.TabItem objpage = ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]);
                                                objPageInfo = objVMuktiGrid.pageControl.SendPage(objpage, lstMsg[i].strFrom);
                                                break;
                                            }
                                        }
                                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                                        {
                                            App.chHttpSuperNodeService.svcSendConfInfo(lstMsg[i].strTo[0], lstMsg[i].strFrom, lstMsg[i].confid, objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                        }
                                        else
                                        {
                                            App.chNetP2PSuperNodeChannel.svcSendConfInfo(lstMsg[i].strTo[0], lstMsg[i].strFrom, lstMsg[i].confid, objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        else if (lstMsg[i].strMessage == "UnJoinConf")
                        {
                            try
                            {
                                //close the page with specified confid
                                //in different window display the message of closing the conference
                                for (int BCnt = 0; BCnt < lstMsg[i].strTo.Length; BCnt++)
                                {
                                    if (lstMsg[i].strTo[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelUnJoinConf, lstMsg[i].strFrom, lstMsg[i].confid);
                                        break;
                                    }
                                }
                            }
                            catch (Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage()", "pgHome.xaml.cs");
                            }
                        }


                        else if (lstMsg[i].strMessage == "SendConfInfo")
                        {
                            try
                            {
                                if (lstMsg[i].strTo[0] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    foreach (KeyValuePair<int, int> kvp in VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser)
                                    {
                                        if (kvp.Key == lstMsg[i].confid)
                                        {
                                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSendConfInfo, lstMsg[i].objPageInfo);
                                        }
                                    }
                                }
                            }
                            catch (Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SendConfInfo", "pgHome.xaml.cs");

                            }
                        }

                        else if (lstMsg[i].strMessage == "EnterConf")
                        {
                            try
                            {
                                if (lstMsg[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    VMuktiAPI.VMuktiEventArgs args = new VMuktiAPI.VMuktiEventArgs(new object[] { lstMsg[i].confid });
                                    VMuktiAPI.VMuktiHelper.CallEvent("EnterConf", this, args);

                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()", "pgHome.xaml.cs");
                            }
                        }

                        else if (lstMsg[i].strMessage == "AddConfBuddy")
                        {
                            try
                            {
                                for (int BCnt = 0; BCnt < lstMsg[i].strTo.Length; BCnt++)
                                {
                                    if (lstMsg[i].strTo[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelAddConfBuddy, lstMsg[i].strFrom, lstMsg[i].confid);
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage():-RemoveBuddyConf", "pgHome.xaml.cs");
                            }
                        }

                        else if (lstMsg[i].strMessage == "RemoveBuddyConf")
                        {
                            try
                            {
                                for (int BCnt = 0; BCnt < lstMsg[i].strTo.Length; BCnt++)
                                {
                                    if (lstMsg[i].strTo[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelRemoveBuddyConf, lstMsg[i].strFrom, lstMsg[i].confid);
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage():-RemoveBuddyConf", "pgHome.xaml.cs");
                            }
                        }

                        else if (lstMsg[i].strMessage == "PodNavigate")
                        {
                            try
                            {
                                //close the page with specified confid
                                //in different window display the message of closing the conference
                                for (int BCnt = 0; BCnt < lstMsg[i].strTo.Length; BCnt++)
                                {
                                    if (lstMsg[i].strTo[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        List<int> lstIDs = new List<int>();
                                        lstIDs.Add(int.Parse(lstMsg[i].objClsModuleInfo.strPageid));
                                        lstIDs.Add(int.Parse(lstMsg[i].objClsModuleInfo.strTabid));
                                        lstIDs.Add(int.Parse(lstMsg[i].objClsModuleInfo.strPodid));
                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelPodNavigation, lstMsg[i].strFrom, lstIDs);
                                        break;
                                    }
                                }
                            }

                            catch (Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage()--PodNavigate", "pgHome.xaml.cs");
                            }
                        }


                        if (lstMsg[i].lstClsModuleInfo != null && lstMsg[i].lstClsModuleInfo.Count > 0)
                        {
                            if (lstMsg[i].lstClsModuleInfo[0].strDropType == "Page Type" || lstMsg[i].lstClsModuleInfo[0].strDropType == "Tab Type")
                            {
                                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                {
                                    Beep(750, 300);
                                }
                            }
                        }
                        else if (lstMsg[i].objClsModuleInfo != null)
                        {
                            if (lstMsg[i].strMessage == "OPEN_MODULE")
                            {
                                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                {
                                    Beep(750, 300);
                                }
                            }
                            else if (lstMsg[i].strMessage == "Newly Dragged Buddy")
                            {
                                #region Pod Type

                                if (lstMsg[i].objClsModuleInfo.strDropType == "Pod Type")
                                {
                                }
                                #endregion
                                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                {
                                    Beep(750, 300);
                                }
                            }
                            else if (lstMsg[i].strMessage == "CLOSE MODULE")
                            {
                                HttpRemoveDraggedBuddy(lstMsg[i].strFrom, lstMsg[i].strTo, lstMsg[i].strMessage, lstMsg[i].objClsModuleInfo);
                                lstMsg.RemoveAt(i);
                            }
                        }
                    }
                }

                this.dispTmrCheckStatus.Start();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delgetMessage()", "pgHome.xaml.cs");
            }
        }

        void delGetMessage()
        {
            try
            {
                if (App.chHttpSuperNodeService != null)
                {
                    List<clsMessage> lstMsg = App.chHttpSuperNodeService.svcGetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    if (lstMsg.Count > 0)
                    {
                        for (int i = 0; i < lstMsg.Count; i++)
                        {
                            if (lstMsg[i].strMessage == "OPEN_PAGE")
                            {
                                objVMuktiGrid.LoadMeetingPage(lstMsg[i].objPageInfo);

                                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                {
                                    Beep(750, 300);
                                }
                            }

                            if (lstMsg[i].lstClsModuleInfo != null && lstMsg[i].lstClsModuleInfo.Count > 0)
                            {
                                if (lstMsg[i].lstClsModuleInfo[0].strDropType == "Page Type" || lstMsg[i].lstClsModuleInfo[0].strDropType == "Tab Type")
                                {
                                    if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                    {
                                        Beep(750, 300);
                                    }
                                }
                            }
                            else if (lstMsg[i].objClsModuleInfo != null)
                            {
                                if (lstMsg[i].strMessage == "OPEN_MODULE")
                                {
                                    if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                    {
                                        Beep(750, 300);
                                    }
                                }
                                else if (lstMsg[i].strMessage == "Newly Dragged Buddy")
                                {
                                    if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                                    {
                                        Beep(750, 300);
                                    }
                                }
                                else if (lstMsg[i].strMessage == "CLOSE MODULE")
                                {
                                    HttpRemoveDraggedBuddy(lstMsg[i].strFrom, lstMsg[i].strTo, lstMsg[i].strMessage, lstMsg[i].objClsModuleInfo);
                                    lstMsg.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
                this.dispTmrCheckStatus.Start();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delGetMessage()", "pgHome.xaml.cs");
            }
        }

        public void HttpRemoveDraggedBuddy(string from, string[] to, string msg, clsModuleInfo objModInfo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HttpRemoveDraggedBuddy()--delGetMessage", "pgHome.xaml.cs");
            }
        }

        public void delPageSpecialMsg(clsPageInfo objPageInfo)
        {
            try
            {
                #region popup
                FncShowPopup(objPageInfo);
                #endregion
                objVMuktiGrid.LoadMeetingPage(objPageInfo);
                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                {
                    Beep(750, 300);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delPageSpecialMsg()", "pgHome.xaml.cs");
            }
        }
        public void delPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo)
        {
            try
            {
                objVMuktiGrid.SetReturnBuddyStatus(objBuddyRetPageInfo);

                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                {
                    Beep(750, 300);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delPageBuddyRetSetSpecialMsg()", "pgHome.xaml.cs");
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "pgHome.xaml.cs");
            }
        }
        private void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        try
                        {
                            if (npcP2PSuperNode != null)
                            {
                                npcP2PSuperNode.CloseClient<INetP2PSuperNodeChannel>();
                                npcP2PSuperNode = null;
                            }
                            if (dispTmrCheckStatus != null)
                            {
                                dispTmrCheckStatus.Stop();
                            }

                            if (dtBandwidth != null)
                            {
                                dtBandwidth.Stop();
                                dtBandwidth = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "pgHome.xaml.cs");
                        }
                    }
                    disposed = true;
                }
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "pgHome.xaml.cs");
            }
        }
        ~pgHome()
        {
            try
            {
                Dispose(false);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~pgHome()", "pgHome.xaml.cs");
            }
        }
        #endregion

        #region DownloadZips For Nodes

        void DownloadZips4Nodes(object objCMCZips)
        {
            try
            {
                bwDownloadZips = new BackgroundWorker();
                bwDownloadZips.DoWork += new DoWorkEventHandler(bwDownloadZips_DoWork);
                bwDownloadZips.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDownloadZips_RunWorkerCompleted);
                if (!bwDownloadZips.IsBusy)
                {
                ClsModuleCollection obj = objCMCZips as ClsModuleCollection;
                
                bwDownloadZips.RunWorkerAsync(objCMCZips);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DownloadZips4Node()", "pgHome.xaml.cs");
            }
        }
        void bwDownloadZips_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "bwDownloadZips_RunWorkerCompleted()", "pgHome.xaml.cs");
            }
        }

        void bwDownloadZips_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ClsModuleCollection cmc = (ClsModuleCollection)e.Argument;
                Assembly ass = Assembly.GetAssembly(typeof(pgHome));

                for (int i = 0; i < cmc.Count; i++)
                {
                    #region Downloading ZipFile

                    string filename = cmc[i].ZipFile;
                    Uri u = new Uri(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + filename);
                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files"));
                    }
                    string destination = ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files");

                    try
                    {
                        if (!File.Exists(destination + "\\" + filename))
                        {
                            WebClient wc = new WebClient();
                            wc.DownloadFile(u, destination + "\\" + filename);
                        }

                    #endregion

                        #region Extracting

                        string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");
                        VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                        if (!Directory.Exists(strModPath + "\\" + filename.Split('.')[0]))
                        {
                            fz.ExtractZip(destination + "\\" + filename, strModPath, null);
                        }

                        string strXmlPath = strModPath + "\\" + filename.Split('.')[0] + "\\Control\\configuration.xml";
                        if (File.Exists(strXmlPath))
                        {
                            #region Parsing XML

                            XmlParser xp = new XmlParser();
                            xp.Parse(strXmlPath);

                            #endregion

                            try
                            {
                                if (!string.IsNullOrEmpty(xp.xMain.SWFFileName))
                                {
                                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.SWFFileName))
                                    {
                                        FileInfo fi = new FileInfo(strModPath + "\\" + filename.Split('.')[0] + "\\Control\\" + xp.xMain.SWFFileName);
                                        fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.SWFFileName);
                                    }
                                }

                                if (!string.IsNullOrEmpty(xp.xMain.TextFileName))
                                {
                                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName))
                                    {
                                        FileInfo fi = new FileInfo(strModPath + "\\" + filename.Split('.')[0] + "\\Control\\" + xp.xMain.TextFileName);
                                        fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "bwDownloadZips_DoWork--copying swf n txt files", "Domains\\SuperNodeServiceDomain.cs");
                            }

                        }

                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "bwDownloadZips_DoWork--File " + filename + " Not Found", "pgHome.xaml.cs");
                    }
                        #endregion



                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "beDownloadZips_DoWork()", "pgHome.xaml.cs");
            }
        }

        public void ShowDirectory(DirectoryInfo dir)
        {
            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    int hj = al.Add(file.FullName);
                }
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    ShowDirectory(subDir);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ShowDirectory()", "pgHome.xaml.cs");
            }
        }

        #endregion

        #region Monitoring System

        void btnConsole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blnISConsole = true;
                pConsole = new Process();
                psiConsole = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "\\VMuktiMonitor.exe");
                psiConsole.WindowStyle = ProcessWindowStyle.Normal;
                pConsole.Exited += new EventHandler(pConsole_Exited);
                pConsole.StartInfo = psiConsole;
                pConsole.EnableRaisingEvents = true;
                pConsole.Start();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnConsole_Click()", "pgHome.xaml.cs");
            }
        }

        void pConsole_Exited(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt");
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "pConsole_Exited()", "pgHome.xaml.cs");
            }

        }

        private void OpenConsoleClient()
        {
            try
            {
                npc = new NetPeerClient();

                ClsNetP2PConsoleDelegates objP2PConsoleDel = new ClsNetP2PConsoleDelegates();

                objP2PConsoleDel.EntsvcNetP2PConsoleJoin += new ClsNetP2PConsoleDelegates.DelsvcNetP2PConsoleJoin(pgHome_EntsvcNetP2PConsoleJoin);
                objP2PConsoleDel.EntsvcNetP2PConsoleSendMsg += new ClsNetP2PConsoleDelegates.DelsvcNetP2PConsoleSendMsg(pgHome_EntsvcNetP2PConsoleSendMsg);
                objP2PConsoleDel.EntsvcNetP2PConsoleGetLog += new ClsNetP2PConsoleDelegates.DelsvcNetP2PConsoleGetLog(pgHome_EntsvcNetP2PConsoleGetLog);
                objP2PConsoleDel.EntsvcNetP2PConsoleSendLog += new ClsNetP2PConsoleDelegates.DelsvcNetP2pConsoleSendLog(pgHome_EntsvcNetP2PConsoleSendLog);
                objP2PConsoleDel.EntsvcNetP2PConsoleUnJoin += new ClsNetP2PConsoleDelegates.DelsvcNetP2PConsoleUnJoin(pgHome_EntsvcNetP2PConsoleUnJoin);

                objNetTcpConsole = objP2PConsoleDel;

                objChannel = (INetP2PConsoleChannel)npc.OpenClient<INetP2PConsoleChannel>("net.tcp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":8080/NetP2PConsole", "ConsoleMesh", ref objNetTcpConsole);
                objChannel.svcNetP2ConsoleJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "openConsoleClient()", "pgHome.xaml.cs");
            }

        }

        private void CloseConsoleClient()
        {
            try
            {
                if (objChannel != null && objChannel.State == System.ServiceModel.CommunicationState.Opened)
                {
                    objChannel.Close();
                    npc.CloseClient<INetP2PConsoleChannel>();
                    objChannel = null;
                    npc = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CloseConsoleClient()", "pgHome.xaml.cs");
            }
        }

        void pgHome_VMuktiEvent_ViewProfile(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                long id = long.Parse(e._args[0].ToString());
                objViewProfile.showProfile(id);
                cnvSettings.Visibility = Visibility.Visible;
                objViewProfile.Visibility = Visibility.Visible;
                objSetting.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "mnuColl_Click()", "pgHome.xaml.cs");
            }
        }

        void pgHome_VMuktiEvent_ViewLog(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                if (objChannel != null)
                {
                    string[] strBName = null;
                    List<string> strsendBName = new List<string>();
                    if (sender != null)
                    {
                        strBName = sender.ToString().Split(',');
                    }

                    for (int i = 0; i < strBName.Length; i++)
                    {
                        if (strBName[i] != null && strBName[i] != "")
                        {
                            strsendBName.Add(strBName[i]);
                            dictConsoleLog.Add(strsendBName[i], null);

                        }
                    }
                    objChannel.svcNetP2ConsoleGetLog(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strsendBName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_VMuktivent_VieweLog()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcNetP2PConsoleSendLog(string from, string to, string key, byte[] barr, int size, bool flag)
        {
            try
            {
                if (to == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    if (dictConsoleLog[key] == null)
                    {
                        dictConsoleLog[key] = new MemoryStream();
                    }
                    dictConsoleLog[key].Position = dictConsoleLog[key].Length;
                    dictConsoleLog[key].Write(barr, 0, barr.Length);

                    if (flag)
                    {
                        int chunkSize = int.Parse(dictConsoleLog[key].Length.ToString());
                        byte[] buffer = new byte[chunkSize];
                        dictConsoleLog[key].Position = 0;

                        DirectoryInfo di = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LogFiles\\ " + from);

                        using (System.IO.FileStream writeStream = new System.IO.FileStream(di.FullName + "\\" + from + "_trace.log", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        {
                            do
                            {
                                int bytesRead = dictConsoleLog[key].Read(buffer, 0, chunkSize);
                                if (bytesRead == 0) break;
                                writeStream.Write(buffer, 0, bytesRead);
                            } while (true);
                        }

                        if (!blnISConsole)
                        {
                            blnISConsole = true;
                            pConsole = new Process();
                            psiConsole = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "\\VMuktiMonitor.exe");
                            psiConsole.WindowStyle = ProcessWindowStyle.Normal;
                            pConsole.Exited += new EventHandler(pConsole_Exited);
                            pConsole.StartInfo = psiConsole;
                            pConsole.EnableRaisingEvents = true;
                            pConsole.Start();
                        }
                        dictConsoleLog.Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcNetP2PConsoleSendLog()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcNetP2PConsoleGetLog(string from, List<string> to)
        {
            try
            {
                if (to.Contains(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName))
                {

                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "trace.log"))
                    {
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "trace.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        int i = 0;

                        if (fs.Length > 5000)
                        {
                            for (i = 0; i < fs.Length / 5000; i++)
                            {
                                fs.Read(arr, 0, 5000);
                                objChannel.svcNetP2ConsoleSendLog(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, from, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, arr, int.Parse(fs.Length.ToString()), false);
                            }
                            if (i * 5000 != fs.Length)
                            {
                                Larr = new byte[int.Parse(fs.Length.ToString()) - (i * 5000)];
                                fs.Read(Larr, 0, int.Parse(fs.Length.ToString()) - (i * 5000));
                                objChannel.svcNetP2ConsoleSendLog(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, from, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, Larr, int.Parse(fs.Length.ToString()), true);
                                fs.Close();
                                fs.Dispose();
                            }
                        }
                        else
                        {
                            Larr = new byte[int.Parse(fs.Length.ToString())];
                            fs.Read(Larr, 0, int.Parse(fs.Length.ToString()));
                            objChannel.svcNetP2ConsoleSendLog(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, from, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, Larr, int.Parse(fs.Length.ToString()), true);
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcNetP2PConsoleGetLog()", "pgHome.xaml.cs");
            }
        }

        void pgHome_VMuktiEvent_SendConsoleMsg(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                if (objChannel != null && objChannel.State == System.ServiceModel.CommunicationState.Opened)
                {
                    objChannel.svcNetP2ConsoleSendMsg((string)e._args[0]);
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_VMuktiEvent_sendConsoleMsg()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcNetP2PConsoleUnJoin(string uName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcNetP2PConsoleUnJoin()", "pgHome.xaml.cs");
            }
        }
        void pgHome_EntsvcNetP2PConsoleSendMsg(string msg)
        {
            try
            {

                Process[] pnew = Process.GetProcessesByName("VMuktiMonitor");

                if ((VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap) && pnew.Length > 0)
                {
                    FileStream fs;
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt"))
                    {
                        fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt", FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    else
                    {
                        fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    }
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(msg);
                    fs.Flush();
                    sw.Flush();
                    sw.Dispose();
                    sw = null;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "pgHome_EntsvcNetP2PConsoleSenfMsg()", "pgHome.xaml.cs");
            }
        }

        void pgHome_EntsvcNetP2PConsoleJoin(string uName)
        {
            try
            {

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "pgHome_EntsvcNetP2PConsoleJoin()", "pgHome.xaml.cs");
            }
        }
        #endregion

        #region Bandwidth

        void dtBandwidth_Tick(object sender, EventArgs e)
        {
            try
            {
                StreamReader sReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt");
                string strStatus = sReader.ReadToEnd();
                sReader.Close();
                if (strStatus == "Initializing")
                {
                    this.dtBandwidth.Stop();
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objBandwidth);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "dtBandwidth_Tick()", "pgHome.xaml.cs");
            }
        }


        public void BandwidthDownloadSpeed()
        {
            try
            {
                MContractDownloadRequest dr = new MContractDownloadRequest();
                dr.FileName = "ul.txt";
                dtDLStart = DateTime.Now;
                clientHttpChannelBandwidth.BeginDownloadFile(dr, DownloadCompleted, null);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BandwidthDoenloadSpeed()", "pgHome.xaml.cs");
                dblbandwidthdl = 0.0;
            }
        }

        void DownloadCompleted(IAsyncResult result)
        {
            try
            {
                dtDLEnd = DateTime.Now;
                MContractRemoteFileInfo rfi = clientHttpChannelBandwidth.EndDownloadFile(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, objDelDownload, rfi);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Downlaodcompleted()", "pgHome.xaml.cs");
            }
        }

        public void delBandwidthDownload(MContractRemoteFileInfo rfi)
        {
            try
            {
                double dblbps = (rfi.Length * 8) / (dtDLEnd - dtDLStart).TotalSeconds;
                dblbandwidthdl = dblbps / 1024;
                Debug.WriteLine("DOWNLOAD SPEED IS " + dblbandwidthdl);
                count++;
                UpdateUI(dblbandwidthdl, dblbandwidthul);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delBandwidthDownload()", "pgHome.xaml.cs");
            }
        }



        private void BandwidthUploadSpeed()
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "ul.txt");

                using (System.IO.FileStream stream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "ul.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
                {
                    MContractRemoteFileInfo rmtfileinfo = new MContractRemoteFileInfo();
                    rmtfileinfo.FileName = fileInfo.Name;
                    rmtfileinfo.Length = fileInfo.Length;
                    lglength = fileInfo.Length;
                    rmtfileinfo.FileByteStream = stream;
                    dtULStart = DateTime.Now;
                    clientHttpChannelBandwidth.BeginUploadFile(rmtfileinfo, UploadCompleted, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BandwidthUploadSpeed()", "pgHome.xaml.cs");
            }
        }

        void UploadCompleted(IAsyncResult result)
        {
            try
            {
                dtULEnd = DateTime.Now;
                clientHttpChannelBandwidth.EndUploadFile(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, objDelUpload);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UplocadCompleted()", "pgHome.xaml.cs");
            }
        }

        public void delBandwidthUpload()
        {
            try
            {
                double dblbps = (lglength * 8) / (dtULEnd - dtULStart).TotalSeconds;
                dblbandwidthul = dblbps / 1024;
                Debug.WriteLine("UPLOAD SPEED IS " + dblbandwidthul);
                BandwidthDownloadSpeed();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delBandwidthUpload()", "pgHome.xaml.cs");
            }
        }

        void UpdateUI(double dblbandwidthdl, double dblbandwidthul)
        {
            try
            {
                if (dblbandwidthdl > 0 || dblbandwidthul > 0)
                {
                    if (dblbandwidthul > 0 && dblbandwidthdl > 0)
                    {
                        if (dblbandwidthdl > 2000)
                        {
                            if (dblbandwidthul > 512)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = Brushes.Green;
                            }
                            else if (dblbandwidthul <= 512 && dblbandwidthul >= 20)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                        }

                        else if (dblbandwidthdl > 1000 && dblbandwidthdl <= 2000)
                        {
                            if (dblbandwidthul > 512)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else if (dblbandwidthul <= 512 && dblbandwidthul >= 20)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = bhOrig;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                        }

                        else if (dblbandwidthdl > 512 && dblbandwidthdl <= 1000)
                        {
                            if (dblbandwidthul > 512)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else if (dblbandwidthul <= 512 && dblbandwidthul >= 100)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                        }

                        else if (dblbandwidthdl > 256 && dblbandwidthdl <= 512)
                        {
                            if (dblbandwidthul > 512)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else if (dblbandwidthul <= 512 && dblbandwidthul >= 100)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                        }
                        else if (dblbandwidthdl > 0 && dblbandwidthdl <= 256)
                        {
                            if (dblbandwidthul > 512)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else if (dblbandwidthul <= 512 && dblbandwidthul >= 20)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            else
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = bhOrig;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                        }
                    }
                    else
                    {
                        if (dblbandwidthdl > 0)
                        {
                            //according to download speed

                            #region DL > 2000
                            if (dblbandwidthdl > 2000)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = Brushes.Green;
                            }
                            #endregion

                            #region DL > 1000

                            else if (dblbandwidthdl <= 2000 && dblbandwidthdl > 1000)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = Brushes.Green;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            #endregion

                            #region DL > 512

                            else if (dblbandwidthdl <= 1000 && dblbandwidthdl > 512)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = Brushes.Green;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }
                            #endregion

                            #region DL > 256

                            else if (dblbandwidthdl <= 512 && dblbandwidthdl > 256)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = Brushes.Green;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }

                            #endregion

                            #region DL > 0

                            else if (dblbandwidthdl <= 56 && dblbandwidthdl > 0)
                            {
                                sbiBandwidth1.Background = Brushes.Green;
                                sbiBandwidth2.Background = bhOrig;
                                sbiBandwidth3.Background = bhOrig;
                                sbiBandwidth4.Background = bhOrig;
                                sbiBandwidth5.Background = bhOrig;
                            }

                            #endregion

                        }
                        else
                        {
                            //according to upload speed

                            sbiBandwidth1.Background = bhOrig;
                            sbiBandwidth2.Background = bhOrig;
                            sbiBandwidth3.Background = bhOrig;
                            sbiBandwidth4.Background = bhOrig;
                            sbiBandwidth5.Background = bhOrig;

                        }
                    }
                }
                else
                {
                    sbiBandwidth1.Background = bhOrig;
                    sbiBandwidth2.Background = bhOrig;
                    sbiBandwidth3.Background = bhOrig;
                    sbiBandwidth4.Background = bhOrig;
                    sbiBandwidth5.Background = bhOrig;
                }
                if (count <= 3)
                {
                    this.dtBandwidth.Start();
                }
                if (count > 3)
                {
                    //ClsException.WriteToLogFile("Download Speed: " + dblbandwidthdl.ToString() + "\r\n Upload Speed: " + dblbandwidthul.ToString());
                    count = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void pgHome_VMuktiEvent_BandwidthUsage(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, delBandWidthUsage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }


        #endregion


        #region Multiple Buddy Selection

        void objBuddies_EntMCodMultipleBuddies(Dictionary<CtlExpanderItem, string> buddiesname, int modid)
        {
            try
            {
                objVMuktiGrid.LoadNewMultipleBuddyPage(buddiesname, modid);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void delSetSpecialMsg4Buddies(clsModuleInfo objModInfo)
        {
            try
            {

                #region popup
                FncShowPopup(objModInfo.lstUsersDropped[objModInfo.lstUsersDropped.Count - 1], objModInfo);
                #endregion
                objVMuktiGrid.LoadNewMultipleBuddyPage(objModInfo);
                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                {
                    Beep(750, 300);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delSetSpecialMsg4Buddies()", "pgHome.xaml.cs");
            }
        }
        void pgHome_EntsvcSetSpecialMsg4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                if (to.Contains(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName))
                {
                    objModInfo.lstUsersDropped.Remove(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    objModInfo.lstUsersDropped.Add(from);
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSetSpecialMsg4Buddies, objModInfo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcSetSpecialMsg4MuktipleBuddies()", "pgHome.xaml.cs");
            }
        }

        void delSetSpecialMsgBuddiesClick(clsModuleInfo objModInfo, clsPageInfo objPageInfo)
        {
            try
            {
                #region popup
                FncShowPopup(objModInfo.lstUsersDropped[objModInfo.lstUsersDropped.Count - 1], objModInfo);
                #endregion
                objVMuktiGrid.LoadMeetingPage(objPageInfo);
                //objVMuktiGrid.LoadNewMultipleBuddyPage(objModInfo, objPageInfo);
                if (((WindowState)(this.Parent.GetValue(Window.WindowStateProperty))) == WindowState.Minimized)
                {
                    Beep(750, 300);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delSetSpecialMsgBuddiesClick()", "pgHome.xaml.cs");
            }
        }
        void pgHome_EntsvcSetSpecialMsgBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo)
        {
            try
            {
                if (to.Contains(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName))
                {
                    objModInfo.lstUsersDropped.Remove(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    objModInfo.lstUsersDropped.Add(from);
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSetSpecialMsgBuddiesClick, objModInfo, objPageInfo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_EntsvcSetSpecialMsgBuddiesClick()", "pgHome.xaml.cs");
            }
        }
        #endregion


        #region Floating

        private void btnBList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objBuddies.cnvMain.Visibility = Visibility.Visible;
                objVMuktiGrid.FncControllPane(false);
                btnBList.IsEnabled = false;
                ctlBuddyClosed = false;
                objBuddy_AnimateGrid(0, 170);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnBList_Click()", "pgHome.xaml.cs");
            }
        }



        void objModule_AnimateGrid(double From, double To)
        {

            GridLengthAnimation gla = new GridLengthAnimation();
            gla.From = new GridLength(From, GridUnitType.Pixel);
            gla.To = new GridLength(To, GridUnitType.Pixel); ;
            gla.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.20));
            colDef1.BeginAnimation(ColumnDefinition.WidthProperty, gla);
        }

        void objBuddy_AnimateGrid(double From, double To)
        {
            GridLengthAnimation gla = new GridLengthAnimation();
            gla.From = new GridLength(From, GridUnitType.Pixel);
            gla.To = new GridLength(To, GridUnitType.Pixel);
            gla.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.20));
            colDef3.BeginAnimation(ColumnDefinition.WidthProperty, gla);

        }

        void objModule_CloseModule()
        {
            objModule_AnimateGrid(170, 0);
            ctlModuleClosed = true;
            btnMExp.IsEnabled = true;
        }

        void ObjBuddy_CloseBuddy()
        {
            objBuddies.txtAddBuddies.Text = "";
            objBuddy_AnimateGrid(170, 0);
            ctlBuddyClosed = true;
            btnBList.IsEnabled = true;
            
        }

        #endregion

        #region Disaster Management

        void pgHome_VMuktiEvent_StopTimer(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                if (((bool)e._args[0]))
                {
                    //ClsException.WriteToLogFile("Start Timer at " + DateTime.Now.ToString());

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        dispTmrCheckStatus.Stop();
                    }
                    VMuktiAPI.VMuktiHelper.CallEvent("BandwidthUsage", null, null);

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        CloseConsoleClient();
                        CloseSuperNodeClient();
                    }
                    CloseBandwidthClient();

                    //ClsException.WriteToLogFile("Stop Timer at " + DateTime.Now.ToString());
                }
                else
                {

                    if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                    {
                        Business.clsDataBaseChannel.chHttpDataBaseService = null;
                        Business.clsDataBaseChannel.OpenDataBaseClient();
                    }
                    else
                    {
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                        {
                            dispTmrCheckStatus.Start();
                            Business.clsDataBaseChannel.chHttpDataBaseService = null;
                            Business.clsDataBaseChannel.OpenDataBaseClient();
                        }
                    }
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        OpenConsoleClient();
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                        {
                            btnConsole.Visibility = Visibility.Visible;
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LogFiles");
                        }
                        OpenSuperNodeClient();
                    }
                    OpenBandwidthClient();
                    this.dtBandwidth.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_VMuktiEvent_stopTimer()", "pgHome.xaml.cs");
            }
        }

        void OpenSuperNodeClient()
        {
            try
            {
                //MessageBox.Show("callin supernode client");

                npcP2PSuperNode = new NetPeerClient();

                NetP2PSuperNodeDelegates objAppSNDel = new NetP2PSuperNodeDelegates();

                objAppSNDel.EntsvcJoin += new NetP2PSuperNodeDelegates.DelsvcJoin(pgHome_EntsvcJoin);
                objAppSNDel.EntsvcSetSpecialMsg += new NetP2PSuperNodeDelegates.DelsvcSetSpecialMsg(pgHome_EntsvcSetSpecialMsg);

                objAppSNDel.EntsvcPageSetSpecialMsg += new NetP2PSuperNodeDelegates.DelsvcPageSetSpecialMsg(pgHome_EntsvcPageSetSpecialMsg);
                objAppSNDel.EntsvcPageBuddyRetSetSpecialMsg += new NetP2PSuperNodeDelegates.DelsvcPageBuddyRetSetSpecialMsg(pgHome_EntsvcPageBuddyRetSetSpecialMsg);
                objAppSNDel.EntsvcSetSpecialMsg4MultipleBuddies += new NetP2PSuperNodeDelegates.DelsvcSetSpecialMsg4MultipleBuddies(pgHome_EntsvcSetSpecialMsg4MultipleBuddies);
                objAppSNDel.EntsvcSetSpecialMsgBuddiesClick += new NetP2PSuperNodeDelegates.DelsvcSetSpecialMsgBuddiesClick(pgHome_EntsvcSetSpecialMsgBuddiesClick);
                objAppSNDel.EntsvcAddDraggedBuddy += new NetP2PSuperNodeDelegates.DelsvcAddDraggedBuddy(pgHome_EntsvcAddDraggedBuddy);
                objAppSNDel.EntsvcAddPageDraggedBuddy += new NetP2PSuperNodeDelegates.DelsvcAddPageDraggedBuddy(pgHome_EntsvcAddPageDraggedBuddy);
                objAppSNDel.EntsvcSetRemoveDraggedBuddy += new NetP2PSuperNodeDelegates.DelsvcSetRemoveDraggedBuddy(pgHome_EntsvcSetRemoveDraggedBuddy);
                objAppSNDel.EntsvcJoinConf += new NetP2PSuperNodeDelegates.DelsvcJoinConf(objAppSNDel_EntsvcJoinConf);
                objAppSNDel.EntsvcSendConfInfo += new NetP2PSuperNodeDelegates.DelsvcSendConfInfo(objAppSNDel_EntsvcSendConfInfo);
                objAppSNDel.EntsvcUnJoinConf += new NetP2PSuperNodeDelegates.DelsvcUnJoinConf(objAppSNDel_EntsvcUnJoinConf);
                objAppSNDel.EntsvcAddConfBuddy += new NetP2PSuperNodeDelegates.DelsvcAddConfBuddy(objAppSNDel_EntsvcAddConfBuddy);
                objAppSNDel.EntsvcRemoveConf += new NetP2PSuperNodeDelegates.DelsvcRemoveConf(objAppSNDel_EntsvcRemoveConf);
                objAppSNDel.EntsvcEnterConf += new NetP2PSuperNodeDelegates.DelsvcEnterConf(objAppSNDel_EntsvcEnterConf);
                objAppSNDel.EntsvcPodNavigation += new NetP2PSuperNodeDelegates.DelsvcPodNavigation(objAppSNDel_EntsvcPodNavigation);
                objAppSNDel.EntsvcUnJoin += new NetP2PSuperNodeDelegates.DelsvcUnJoin(pgHome_EntsvcUnJoin);

                App.objNetP2PSuperNode = objAppSNDel;

                //MessageBox.Show("pghome:-" + VMuktiInfo.CurrentPeer.SuperNodeIP);

                App.chNetP2PSuperNodeChannel = (INetP2PSuperNodeChannel)npcP2PSuperNode.OpenClient<INetP2PSuperNodeChannel>("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode", "P2PSuperNodeMesh", ref App.objNetP2PSuperNode);
                App.chNetP2PSuperNodeChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //ClsException.WriteToLogFile("opened p2p sn channel with join");

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OpenSuperNodeClient()", "pgHome.xaml.cs");
            }
        }

        void CloseSuperNodeClient()
        {
            if (App.chNetP2PSuperNodeChannel != null && App.chNetP2PSuperNodeChannel.State == System.ServiceModel.CommunicationState.Opened)
            {
                try
                {
                    App.chNetP2PSuperNodeChannel.Close();
                    npcP2PSuperNode.CloseClient<INetP2PSuperNodeChannel>();
                    App.objNetP2PSuperNode = null;
                    npcP2PSuperNode = null;
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CloseSuperNodeClient()", "pgHome.xaml.cs");
                }
            }
        }

        void OpenBandwidthClient()
        {
            try
            {
                bhcFileTransfer = new BasicHttpClient();
                clientHttpChannelBandwidth = (IHttpFileUploadDownload)bhcFileTransfer.OpenClient<IHttpFileUploadDownload>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/HttpFileUploadDownload");
                clientHttpChannelBandwidth.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OpenBandwidthClient()", "pgHome.xaml.cs");
            }
        }

        void CloseBandwidthClient()
        {
            try
            {
                bhcFileTransfer.CloseClient<IHttpFileUploadDownload>();
                clientHttpChannelBandwidth = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CloseBandwidthClient()", "pgHome.xaml.cs");
            }
        }

        void fncBandWidthUsage()
        {
            try
            {
                sbiBandwidth1.Background = bhOrig;
                sbiBandwidth2.Background = bhOrig;
                sbiBandwidth3.Background = bhOrig;
                sbiBandwidth4.Background = bhOrig;
                sbiBandwidth5.Background = bhOrig;

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncBandwidthUsage()", "pgHome.xaml.cs");
            }
        }

        #endregion

        #region MeetingSchedular

        public void pgHome_VMuktiEvent_Conference(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                VMukti.App.blnIsTwoPanel = true;
                if (!VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.ContainsKey(int.Parse(e._args[0].ToString())))
                {

                    bool started = false;

                    List<string> lstParticipantUsers = new List<string>();

                    ClsConferenceUsersCollection objConfInfo = ClsConferenceUsersCollection.GetConfInfo(int.Parse(e._args[0].ToString()));


                    ClsConferenceUsers objConfStartedUser = new ClsConferenceUsers();


                    for (int CntConfUser = 0; CntConfUser < objConfInfo.Count; CntConfUser++)
                    {
                        if (objConfInfo[CntConfUser].Started)
                        {
                            objConfStartedUser = objConfInfo[CntConfUser];
                            started = true;
                            break;
                        }
                    }
                    if (!started)
                    {
                        objAmit.Visibility = Visibility.Collapsed;
                        objBuddies.btnClose_Click(null,null);
                        
                        System.Threading.Thread.Sleep(1000);
                       // ObjBuddy_CloseBuddy();
                        blOpenMExp = false;
                        btnMExp_Click(null, null);

                        ClsConferenceCollection objGetConfInfo = ClsConferenceCollection.GetUserConferences(int.Parse(e._args[0].ToString()));

                        objVMuktiGrid.LoadPage(objGetConfInfo[0].PageID, int.Parse(e._args[0].ToString()));
                        ClsConferenceUsers objUpdateStarted = new ClsConferenceUsers();
                        objUpdateStarted.UpdateStarted(int.Parse(e._args[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, true);
                        VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.Add(int.Parse(e._args[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID);

                        ClsConferenceUsersCollection objGetConParticipants = ClsConferenceUsersCollection.GetConfParticipants(int.Parse(e._args[0].ToString()));
                        for (int PartCnt = 0; PartCnt < objGetConParticipants.Count; PartCnt++)
                        {
                            ClsUserInfo objGetUserInfo = new ClsUserInfo().User_GetByID(objGetConParticipants[PartCnt].UserID);
                            lstParticipantUsers.Add(objGetUserInfo.DisplayName);
                        }
                        //ClsException.WriteToLogFile("lstParticipantUsers.Count " + lstParticipantUsers.Count.ToString());

                        if (lstParticipantUsers.Count > 0)
                        {
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {

                                App.chHttpSuperNodeService.svcEnterConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipantUsers, int.Parse(e._args[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                            else
                            {
                                App.chNetP2PSuperNodeChannel.svcEnterConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipantUsers, int.Parse(e._args[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                        }

                    }
                    else
                    {

                        if (objConfStartedUser != null)
                        {
                            ClsUserInfo objGetByID = new ClsUserInfo().User_GetByID(objConfStartedUser.UserID);
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                App.chHttpSuperNodeService.svcJoinConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, objGetByID.DisplayName, int.Parse(e._args[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                            else
                            {
                                App.chNetP2PSuperNodeChannel.svcJoinConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, objGetByID.DisplayName, int.Parse(e._args[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                        }
                        VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.Add(int.Parse(e._args[0].ToString()), objConfStartedUser.UserID);
                    }
                }
                else
                {
                    MessageBox.Show("Conference Already Started", "Conference", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pgHome_VMuktiEvent_Conference()", "pgHome.xaml.cs");

            }
        }

        void objAppSNDel_EntsvcJoinConf(string from, string to, int confid, string ipaddress)
        {
            //create the pageinfo object 
            //second parameter shud b string[] to
            //App.chNetP2PSuperNodeChannel.svcSendConfInfo(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, to, confid, objpageinfo);
            try
            {
                if (to == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<string> lstArgs = new List<string>();
                    lstArgs.Add(to);
                    lstArgs.Add(confid.ToString());
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelJoinConf, from, lstArgs);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objAppSNDel_EntsvcJoinConf()", "pgHome.xaml.cs");
            }
        }

        void objAppSNDel_EntsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress)
        {
            try
            {
                if (to == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    foreach (KeyValuePair<int, int> kvp in VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser)
                    {
                        if (kvp.Key == confid)
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSendConfInfo, pageinfo);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objAppSNDel_EntsvcSendConfInfo()", "pgHome.xaml.cs");

            }
        }

        void objAppSNDel_EntsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                for (int BCnt = 0; BCnt < lstBuddies.Count; BCnt++)
                {
                    if (lstBuddies[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelAddConfBuddy, from, confid);
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objAppSNDel_EntsvcUnJoinConf()", "pgHome.xaml.cs");
            }
        }

        void objAppSNDel_EntsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                for (int BCnt = 0; BCnt < lstBuddies.Count; BCnt++)
                {
                    if (lstBuddies[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelRemoveBuddyConf, from, confid);
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objAppSNDel_EntsvcUnJoinConf()", "pgHome.xaml.cs");
            }
        }

        void objAppSNDel_EntsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                //close the page with specified confid
                //in different window display the message of closing the conference
                for (int BCnt = 0; BCnt < lstBuddies.Count; BCnt++)
                {
                    if (lstBuddies[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelUnJoinConf, from, confid);
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objAppSNDel_EntsvcUnJoinConf()", "pgHome.xaml.cs");
            }
        }

        void objAppSNDel_EntsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {

                    VMuktiAPI.VMuktiEventArgs args = new VMuktiAPI.VMuktiEventArgs(new object[] { confid });
                    VMuktiAPI.VMuktiHelper.CallEvent("EnterConf", this, args);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objAppSNDel_EntsvcEnterConf()", "pgHome.xaml.cs");
            }
        }

        void objAppSNDel_EntsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid, string ipaddress)
        {
            try
            {

                for (int BCnt = 0; BCnt < lstBuddies.Count; BCnt++)
                {
                    if (lstBuddies[BCnt] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        List<int> lstIDs = new List<int>();
                        lstIDs.Add(pageid);
                        lstIDs.Add(tabid);
                        lstIDs.Add(podid);
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelPodNavigation, from, lstIDs);
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objAppSNDel_EntsvcUnJoinConf()", "pgHome.xaml.cs");
            }
        }

        void PodNavigation(string from, List<int> lstIDs)
        {
            try
            {
                for (int pgCnt = 0; pgCnt < objVMuktiGrid.pageControl.Items.Count; pgCnt++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pgCnt]).ObjectID == lstIDs[0])
                    {
                        VMuktiGrid.ctlPage.TabItem page = ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pgCnt]) as VMuktiGrid.ctlPage.TabItem;

                        for (int tCnt = 0; tCnt < ((VMuktiGrid.ctlTab.TabControl)page.Content).Items.Count; tCnt++)
                        {
                            VMuktiGrid.ctlTab.TabItem tab = ((VMuktiGrid.ctlTab.TabControl)page.Content).Items[tCnt] as VMuktiGrid.ctlTab.TabItem;
                            if (tab.ObjectID == lstIDs[1])
                            {
                                VMuktiGrid.CustomGrid.ctlGrid grid = tab.Content as VMuktiGrid.CustomGrid.ctlGrid;

                                for (int pCnt = 0; pCnt < grid.RightPanelContainer.Items.Count; pCnt++)
                                {
                                    if (grid.RightPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                                    {
                                        ctlPOD pod = grid.RightPanelContainer.Items[pCnt] as ctlPOD;
                                        pod.IsTwoPanel = true;

                                        VMuktiAPI.ClsException.WriteToLogFile(pod.ObjectID.ToString());
                                        if (int.Parse(pod.ObjectID.ToString()) == lstIDs[2])
                                        {
                                            grid.RightPanelContainer.Items.RemoveAt(pCnt);
                                            if (grid.CentralPanelContainer.Items.Count > 0)
                                            {
                                                ctlPOD podCenter = grid.CentralPanelContainer.Items[0] as ctlPOD;
                                                grid.CentralPanelContainer.Items.RemoveAt(0);
                                                grid.RightPanelContainer.Items.Insert(0, podCenter);
                                                podCenter.btnHidePanel_Click(null, null);
                                            }
                                            grid.CentralPanelContainer.Items.Insert(0, pod);
                                            if (pod.grdBody.Visibility == Visibility.Collapsed)
                                            {
                                                pod.btnHidePanel_Click(null, null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PodNavigation()", "pgHome.xaml.cs");
            }
        }

        void SendConfInfo(clsPageInfo pageinfo)
        {
            try
            {
                
                objAmit.Visibility = Visibility.Collapsed;
                objBuddies.btnClose_Click(null, null);
                //objBuddies.Visibility = Visibility.Collapsed;
                blOpenMExp = false;
                btnMExp_Click(null, null);
                //objVMuktiGrid.pageControl.Items.Clear();


                objVMuktiGrid.LoadMeetingPage(pageinfo);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SendConfInfo()", "pgHome.xaml.cs");
            }
        }

        void UnJoinConf(string from, int confid)
        {
            try
            {

                VMuktiAPI.VMuktiEventArgs args = new VMuktiAPI.VMuktiEventArgs(new object[] { confid });
                VMuktiAPI.VMuktiHelper.CallEvent("ExitConf", this, args);
                for (int pCnt = 0; pCnt < objVMuktiGrid.pageControl.Items.Count; pCnt++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).ConfID == confid)
                    {
                        ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).Close();
                        //objVMuktiGrid.CloseConfPage(confid);
                        Window wndCloseConf = new Window();
                        wndCloseConf.ShowInTaskbar = true;
                        wndCloseConf.Height = 100;
                        wndCloseConf.Width = 300;
                        wndCloseConf.Content = from.ToUpper() + " Has Ended the Conference" + Char.ConvertFromUtf32(13) + "Thank You for attending the Conference !!!";
                        wndCloseConf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        wndCloseConf.WindowState = WindowState.Normal;
                        wndCloseConf.Title = "VMukti Conference";
                        wndCloseConf.Show();
                        break;


                        // pageControl.Items.RemoveAt(pCnt);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "UnJoinConf()", "pgHome.xaml.cs");
            }
        }

        void AddConfBuddy(string from,int confid)
        {
            try
            {
                for (int pCnt = 0; pCnt < objVMuktiGrid.pageControl.Items.Count; pCnt++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).ConfID == confid)
                    {
                        ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).AddBuddy(from);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "AddConfBuddy()", "pgHome.xaml.cs");
            }
        }

        void RemoveBuddyConf(string from, int confid)
        {

            for (int pCnt = 0; pCnt < objVMuktiGrid.pageControl.Items.Count; pCnt++)
            {
                if (((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).ConfID == confid)
                {
                    ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[pCnt]).RemoveBuddy(from);

                    Window wndCloseConf = new Window();
                    wndCloseConf.ShowInTaskbar = true;
                    wndCloseConf.Height = 80;
                    wndCloseConf.Width = 200;
                    wndCloseConf.Content = from.ToUpper() + " Has Left the Conference" + Char.ConvertFromUtf32(13);
                    wndCloseConf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    wndCloseConf.WindowState = WindowState.Normal;
                    wndCloseConf.Title = "VMukti Conference";
                    wndCloseConf.Show();
                }
            }
        }

        void JoinConf(string from, List<string> lstArgs)
        {
            try
            {
                string to = lstArgs[0];
                int confid = int.Parse(lstArgs[1]);

                foreach (KeyValuePair<int, int> kvp in VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser)
                {
                    if (kvp.Key == confid)
                    {
                        List<string> lstBuddies2Inform = new List<string>();
                        ClsConferenceCollection objGetConfInfo = ClsConferenceCollection.GetUserConferences(confid);
                        clsPageInfo objPageInfo = new clsPageInfo();
                        for (int i = 0; i < objVMuktiGrid.pageControl.Items.Count; i++)
                        {
                            if (objGetConfInfo[0].PageID == ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[i]).ObjectID)
                            {
                                VMuktiGrid.ctlPage.TabItem objpage = ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[i]);

                                objPageInfo = objVMuktiGrid.pageControl.SendPage(objpage, from);

                                lstBuddies2Inform = ((ctlMenu)objpage.Template.FindName("objMenu", objpage)).GetBuddies();
                                //

                                break;
                            }
                        }
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                        {
                            App.chHttpSuperNodeService.svcSendConfInfo(to, from, confid, objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                        else
                        {
                            App.chNetP2PSuperNodeChannel.svcSendConfInfo(to, from, confid, objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                        //call wcf function to inform other buddy to of new buddy being added to conference.



                        if (lstBuddies2Inform.Contains(from))
                        {
                            lstBuddies2Inform.Remove(from);
                        }
                        else if (lstBuddies2Inform.Contains(to))
                        {
                            lstBuddies2Inform.Remove(to);
                        }
                        if (lstBuddies2Inform.Count > 0)
                        {
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                App.chHttpSuperNodeService.svcAddConfBuddy(from, lstBuddies2Inform, confid, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                            else
                            {
                                App.chNetP2PSuperNodeChannel.svcAddConfBuddy(from, lstBuddies2Inform, confid, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                        }

                        break;
                    }
                }

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "JoinConf()", "pgHome.xaml.cs");
            }
        }

        public void pgHome_VMuktiEvent_InstantMeeting(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                VMukti.App.blnIsTwoPanel = false;
                blOpenMExp = true;
                btnMExp_Click(null, null);
                btnBList_Click(null, null);
                objVMuktiGrid.pageControl.LoadPage();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "pgHome_VMuktiEvent_InstantMeeting()", "pgHome.xaml.cs");
            }
        }

        #endregion

        #region Performance

        #region For Login

        void bwLoginPerf_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        void bwLoginPerf_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AsyncCallback callback = new AsyncCallback(AuthorizedCallback);
                objDelAuthorized.BeginInvoke(callback, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void Authorized()
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelAuthorizeLogin);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        private void AuthorizedCallback(IAsyncResult ar)
        {
            try
            {
                objDelAuthorized.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }

        }

        void AuthorizedLogin()
        {
            try
            {
                Thread tSuperNodeWCFClients = new Thread(new ThreadStart(SuperNodeWCFClients));
                tSuperNodeWCFClients.Start();

                isEventOccured = false;
                btnBList.Visibility = Visibility.Visible;
                btnBList.IsEnabled = true;
                btnSettings.Visibility = Visibility.Visible;
                btnLogin.Visibility = Visibility.Visible;
                btnMExp.Visibility = Visibility.Visible;
                tblkUserName.Visibility = Visibility.Visible;
                btnRecord.Visibility = Visibility.Collapsed;
                tblkUserName.Text = "Welcome " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                objBuddies.Visibility = Visibility.Visible;

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                {
                    btnConsole.Visibility = Visibility.Visible;
                }

                objCtlThemPopUp = new CtlThemPopUp();
                objPopup = new wndVMuktiPopup();


                #region changes for the scheduler
                
                if (objVMuktiGrid.LoadPage(2))
                {
                    objVMuktiGrid.FncControllPane(true);
                    blOpenMExp = false;
                    btnMExp_Click(null, null);
                }

                #endregion

                try
                {
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                    {
                        btnSettings.Visibility = Visibility.Visible;
                    }
                    btnLoginText.Text = "Sign Out";
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objModules_EntAutherized()", "pgHome.xaml.cs");
                }


                Thread tWCFClients = new Thread(new ThreadStart(AdditionalWCFClients));
                tWCFClients.Start();


                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    dispTmrCheckStatus.Start();
                }

                #region LoadSettings

                objSetting = new CtlSettings();
                objSetting.Height = 480;
                objSetting.Width = 640;
                objSetting.Margin = new Thickness(0, 50, 0, 0);
                objSetting.Visibility = Visibility.Collapsed;
                objSetting.VerticalAlignment = VerticalAlignment.Top;
                objSetting.HorizontalAlignment = HorizontalAlignment.Center;
                cnvSettings.Children.Add(objSetting);

                objViewProfile = new CtlViewProfile();
                objViewProfile.Height = 480;
                objViewProfile.Width = 640;
                objViewProfile.Margin = new Thickness(0, 50, 0, 0);
                objViewProfile.Visibility = Visibility.Collapsed;
                objViewProfile.VerticalAlignment = VerticalAlignment.Center;
                objViewProfile.HorizontalAlignment = HorizontalAlignment.Center;
                cnvSettings.Children.Add(objViewProfile);

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void AdditionalWCFClients()
        {
            try
            {
                #region Bandwidth

                try
                {
                    OpenBandwidthClient();

                    dtBandwidth.Start();
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BandWidth Client Opening()", "pgHome.xaml.cs");
                }
                #endregion

                #region Monitoring System

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    OpenConsoleClient();
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                    {

                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LogFiles");
                    }
                }

                #endregion



            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }

        }

        void SuperNodeWCFClients()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                {
                    OpenSuperNodeClient();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        #endregion

        #region For Grid

        void bwGridLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        void bwGridLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AsyncCallback cbGridLoad = new AsyncCallback(LoadGridCallback);
                objDelGridLoad.BeginInvoke(cbGridLoad, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        private void LoadGridCallback(IAsyncResult ar)
        {
            try
            {
                objDelGridLoad.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void LoadGrid()
        {
            try
            {

                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, objDelStartLoadingGrid);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void StartLoadingGrid()
        {
            try
            {
                #region new UI
                ResourceDictionary obj = new ResourceDictionary();
                obj.Source = new Uri(@"\Skins\Orkut.xaml", UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Add(obj);
                // BackgroundImagePopUp.entChangeBackground += new CtlBackgroundImagePopUp.delChangeBackground(BackgroundImagePopUp_entChangeBackground);

                imgMExp.Source = new BitmapImage(new Uri(@"\Skins\Images1\ModulExplorer.gif", UriKind.RelativeOrAbsolute));
                imgBList.Source = new BitmapImage(new Uri(@"\Skins\Images1\Buddy.png", UriKind.RelativeOrAbsolute));
                imgBtnConsole.Source = new BitmapImage(new Uri(@"\Skins\Images1\Console.png", UriKind.RelativeOrAbsolute));
                imgBtnLogin.Source = new BitmapImage(new Uri(@"\Skins\Images1\Login.gif", UriKind.RelativeOrAbsolute));
                imgBtnSetting.Source = new BitmapImage(new Uri(@"\Skins\Images1\Setting.gif", UriKind.RelativeOrAbsolute));
                imgBtnRecord.Source = new BitmapImage(new Uri(@"\Skins\Images1\Recorder.ico", UriKind.RelativeOrAbsolute));
                string ObjNode = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString();
                tblkUserName.Visibility = Visibility.Visible;
                tblkUserName.Text = "WELCOME";
                switch (ObjNode)
                {

                    case "BootStrap":
                        rectNodeType.Fill = System.Windows.Media.Brushes.DarkMagenta;
                        rectNodeType.ToolTip = "Server";
                        break;

                    case "SuperNode":
                        rectNodeType.Fill = System.Windows.Media.Brushes.Green;
                        rectNodeType.ToolTip = "Open Network";
                        break;

                    case "NodeWithNetP2P":
                        rectNodeType.Fill = System.Windows.Media.Brushes.Yellow;
                        rectNodeType.ToolTip = "NAT";
                        break;

                    case "NodeWithHttp":
                        rectNodeType.Fill = System.Windows.Media.Brushes.OrangeRed;
                        rectNodeType.ToolTip = "Outgoing Firewall";
                        break;
                    case "NotDecided":
                        rectNodeType.Fill = System.Windows.Media.Brushes.Red;
                        rectNodeType.ToolTip = "Not Decided";
                        break;

                }
                #endregion

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        #endregion

        #region For Widget

        void bwLoadWidget_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void bwLoadWidget_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AsyncCallback cbWidLoad = new AsyncCallback(LoadWidgetCallback);
                objDelLoadWidget.BeginInvoke(cbWidLoad, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void LoadWidgetCallback(IAsyncResult ar)
        {
            try
            {
                objDelLoadWidget.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void LoadWidget()
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, objDelStartLoadingWid);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        void StartLoadingWid()
        {
            try
            {
                //ClsModuleCollection objCMC = new ClsModuleCollection();

                try
                {
                    VMukti.Business.clsDataBaseChannel.OpenDataBaseClient();
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartLoadingWid()--Database http channel", "pgHome.xaml.cs");
                }

                ClsModuleCollection objCMCZips = null;
                //if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                //{
                //    if (Business.clsDataBaseChannel.OpenDataBaseClient())
                //    {
                //        objCMCZips = ClsModuleCollection.GetNonAuthenticatedNonCollMod();
                //    }
                //}
                //else
                //{

                //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                //    {
                //        objCMCZips = ClsModuleCollection.GetNonAuthenticatedNonCollMod();

                //    }
                //    else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                //    {
                //        if (Business.clsDataBaseChannel.OpenDataBaseClient())
                //        {
                //            objCMCZips = ClsModuleCollection.GetNonAuthenticatedNonCollMod();
                //        }
                //    }
                //}

                objCMCZips = ClsModuleCollection.GetNonAuthenticatedNonCollMod();

                if (objCMCZips != null)
                {
                    for (int j = 0; j < objCMCZips.Count; j++)
                    {
                        if (objCMCZips[j].IsCollaborative.ToLower() == "false")
                        {
                            //u objModules.AddModule(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,False");
                            objAmit.AddItem(objCMCZips[j].ModuleTitle, objCMCZips[j].ModuleId + ",ModuleType,False", objAmit.GetImage(objCMCZips[j].ImageFile));
                        }
                    }
                }


                //Download Zip Files

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    if (objCMCZips != null)
                    {
                        thZipFiles = new Thread(new ParameterizedThreadStart(DownloadZips4Nodes));
                        thZipFiles.Start(objCMCZips);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "pgHome.xaml.cs");
            }
        }

        #endregion

        #endregion

        #region upper

        void objAmit_EntPageItemSelectionChanged(string strTagText, string strContent)
        {
            try
            {
                VMukti.App.blnIsTwoPanel = true;
                objVMuktiGrid.LoadPage(int.Parse(strTagText));
                blOpenMExp = false;
                btnMExp_Click(null, null);
                objVMuktiGrid.FncControllPane(true);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objAmit_EntPageItemSelectionChanged()", "pgHome.xaml.cs");
            }
        }

        void objAmit_EntAutherized()
        {
            //ClsException.WriteToLogFile("called objModules_EntAutherized from modules in pghome");
            try
            {

                bwLoginPerf.RunWorkerAsync();

                // isEventOccured = false;
                // btnBList.Visibility = Visibility.Visible;
                // btnBList.IsEnabled = true;
                // //SkinPopUp.Visibility = Visibility.Visible;
                // ThemepopUp.Visibility = Visibility.Visible;
                //// BackgroundImagePopUp.Visibility = Visibility.Visible;
                // btnSettings.Visibility = Visibility.Visible;
                // btnLogin.Visibility = Visibility.Visible;
                // btnMExp.Visibility = Visibility.Visible;
                // tblkUserName.Visibility = Visibility.Visible;
                // tblkUserName.Text = "Welcome "  + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                // objBuddies.Visibility = Visibility.Visible;



                // #region Bandwidth

                // try
                // {
                //     OpenBandwidthClient();

                //     dtBandwidth.Start();
                // }
                // catch (Exception ex)
                // {
                //     VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BandWidth Client Opening()", "pgHome.xaml.cs");
                // }
                // #endregion



                // #region Monitoring System

                // if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                // {
                //     OpenConsoleClient();
                //     if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
                //     {
                //         btnConsole.Visibility = Visibility.Visible;
                //         Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LogFiles");
                //     }
                // }

                // #endregion

                // //ClsException.WriteToLogFile("opening sn p2p channel in pghome if node is bs,sn or p2p" + DateTime.Now);
                // if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                // {
                //     OpenSuperNodeClient();
                // }
                // //ClsException.WriteToLogFile("opened sn p2p channel in pghome if node is bs,sn or p2p");

                // if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                // {
                //     //ClsException.WriteToLogFile("if http node, starting the timer");
                //     dispTmrCheckStatus.Start();
                //     //ClsException.WriteToLogFile("if http node, started the timer");
                // }

                // //sb1 = CreateTressInfo();
                // #region changes for the scheduler


                // objVMuktiGrid.pageControl.Items.Clear();
                // if (objVMuktiGrid.LoadPage(2))
                // {
                //     objVMuktiGrid.FncClosePane();
                //     blOpenMExp = false;
                //     btnMExp_Click(null, null);
                // }

                // #endregion

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objModules_EntAutherized()", "pgHome.xaml.cs");
            }
            //try
            //{
            //    //objBuddies.LoadBuddyList();
            //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
            //    {
            //        btnSettings.Visibility = Visibility.Visible;
            //    }
            //    btnLoginText.Text = "Sign Out";
            //}
            //catch (Exception ex)
            //{
            //    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objModules_EntAutherized()", "pgHome.xaml.cs");
            //}


        }

        private void btnMExp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (blOpenMExp)
                {
                    objAmit.Visibility = Visibility.Visible;
                    objVMuktiGrid.FncControllPane(false);
                    GridLengthAnimation gla = new GridLengthAnimation();
                    gla.To = new GridLength(230, GridUnitType.Pixel);
                    gla.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.00));
                    rowModule.BeginAnimation(RowDefinition.HeightProperty, gla);
                    blOpenMExp = false;

                }
                else
                {
                    GridLengthAnimation gla = new GridLengthAnimation();
                    gla.From = new GridLength(230, GridUnitType.Pixel);
                    gla.To = new GridLength(0, GridUnitType.Pixel);
                    gla.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.00));
                    rowModule.BeginAnimation(RowDefinition.HeightProperty, gla);
                    //rowModule.Height = new GridLength(0, GridUnitType.Pixel);
                    blOpenMExp = true;
                }


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMexp_Click()", "pgHome.xaml.cs");
            }
        }

        void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blOpenMExp = true;
                btnMExp_Click(null, null);
                ObjBuddy_CloseBuddy();


                if (btnLoginText.Text == "Sign Out")
                {
                    //App.SetCookie(BrowserInteropHelper.Source, "Login");

                    //ClsException.WriteToLogFile("btnLogin_Click 1 -- pgHome.xaml.cs  " + DateTime.Now);
                    //u objModules.LogOut();

                    #region upper

                    objAmit.LogOut();

                    #endregion

                    LogOut();
                    btnLoginText.Text = "Sign In";

                }
                objAmit.ShowLogin();
                //u  objModules.ShowLogin();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnLogin_Click()", "pgHome.xaml.cs");
            }

        }

        void LogOut()
        {
            try
            {
                //ClsException.WriteToLogFile("LogOut  -- pgHome.xaml.cs  " + DateTime.Now);
                
                
                if (!isEventOccured)
                {
                    isEventOccured = true;

                    try
                    {
                    //    foreach (KeyValuePair<int, int> kvp in VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser)
                    //    {
                    //        if (kvp.Value == VMuktiAPI.VMuktiInfo.CurrentPeer.ID)
                    //        {
                    //            //update this entry from database UPDATE ConferenceUsers set Started='true' where ConfID='3' and UserID='3'
                    //            ClsConferenceUsers objUpdateStarted = new ClsConferenceUsers();
                    //            objUpdateStarted.UpdateStarted(kvp.Key, kvp.Value, false);
                    //        }
                    //    }
                        //VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.Clear();

                        for (int cnt = 0; cnt <objVMuktiGrid.pageControl.Items.Count; cnt++)
                        {
                            if (((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[cnt]).ConfID != 0)
                            {
                                ((VMuktiGrid.ctlPage.TabItem)objVMuktiGrid.pageControl.Items[cnt]).ClosePage();
                            }
                        }

                        VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.Clear();
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LogOut()--MeetingSchedular", "pgHome.xaml.cs");
                    }


                    //ClsException.WriteToLogFile("LogOut  -- pgHome.xaml.cs  " + DateTime.Now);
                    VMuktiAPI.VMuktiHelper.CallEvent("LogoutBuddyList", this, new VMuktiAPI.VMuktiEventArgs());
                    if (App.chHttpSuperNodeService != null)
                    {
                        App.chHttpSuperNodeService.svcUnjoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        App.objHttpSuperNode.CloseClient<IHttpSuperNodeService>();
                    }
                    if (clientHttpChannelBandwidth != null)
                    {
                        clientHttpChannelBandwidth = null;
                    }

                    bool isSuperNode = false;
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode)
                    {
                        isSuperNode = true;
                    }


                    if (App.chHttpBootStrapService != null)
                    {

                        App.chHttpBootStrapService.svcHttpBSUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, isSuperNode);

                    }



                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        if (objChannel != null && objChannel.State == System.ServiceModel.CommunicationState.Opened)
                        {
                            objChannel.Close();
                            npc.CloseClient<INetP2PConsoleChannel>();
                            objChannel = null;
                            npc = null;
                        }

                        if (App.chNetP2PSuperNodeChannel != null && App.chNetP2PSuperNodeChannel.State == System.ServiceModel.CommunicationState.Opened)
                        {
                            App.chNetP2PSuperNodeChannel.Close();
                            npcP2PSuperNode.CloseClient<INetP2PSuperNodeChannel>();
                            App.objNetP2PSuperNode = null;
                            npcP2PSuperNode = null;
                        }
                    }

                    if (dispTmrCheckStatus != null)
                    {
                        dispTmrCheckStatus.Stop();
                    }

                    //VMuktiAPI.VMuktiInfo.CurrentPeer.ID = int.MinValue;

                    VMuktiAPI.VMuktiHelper.CallEvent("SignOut", null, null);
                   
                }

                btnBList.Visibility = Visibility.Collapsed;

                btnSettings.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Collapsed;

                objBuddies.Visibility = Visibility.Collapsed;

                btnConsole.Visibility = Visibility.Collapsed;
                tblkUserName.Text = String.Empty;
                tblkUserName.Visibility = Visibility.Collapsed;

                btnRecord.Visibility = Visibility.Collapsed;
                VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = "";

                #region UICHANGE

                // SkinPopUp.Visibility = Visibility.Collapsed;
                //ThemepopUp.Visibility = Visibility.Collapsed;
                // BackgroundImagePopUp.Visibility = Visibility.Collapsed;

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LogOut()", "pgHome.xaml.cs");

            }

        }

        void objAmit_entClosemodule()
        {
            try
            {
                GridLengthAnimation gla = new GridLengthAnimation();
                gla.From = new GridLength(210, GridUnitType.Pixel);
                gla.To = new GridLength(0, GridUnitType.Pixel);
                gla.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.80));
                rowModule.BeginAnimation(RowDefinition.HeightProperty, gla);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "entClosemodule()", "pgHome.xaml.cs");
            }


        }

        #endregion

        #region ShowPopup

        public void FncShowPopup(string Host, clsModuleInfo objModuleInfo)
        {

            try
            {

                objPopup.FnvLoadpopup(Host, objModuleInfo.strModuleName, objModuleInfo.lstUsersDropped);


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncShowPopup()", "pgHome.xaml.cs");

            }
        }

        public void FncShowPopup(clsPageInfo objPageInfo)
        {

            try
            {

                List<string> buddies = new List<string>();
                List<string> modules = new List<string>();
                foreach (clsTabInfo objtab in objPageInfo.objaTabs)
                {
                    foreach (clsPodInfo objpod in objtab.objaPods)
                    {


                        buddies.InsertRange(buddies.Count, objpod.straPodBuddies);
                        modules.Add(objpod.strPodTitle);

                    }
                }
                objPopup.FnvLoadpopup(objPageInfo.strFrom, modules, buddies);


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncShowPopup()", "pgHome.xaml.cs");

            }
        }

        #endregion

        public void ForDragdrop(string caption, string tag, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)(objVMuktiGrid.pageControl.SelectedItem as VMuktiGrid.ctlPage.TabItem).Content).SelectedItem).Content).FncDrop(caption, tag, e);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ForDragdrop()", "pgHome.xaml.cs");

            }
        }

    }
}
