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
using System.Windows.Threading;
using System.Threading;
using System.Reflection;
using VMukti.Business.VMuktiGrid;
using System.Net;
using System.IO;
using VMuktiAPI;
using System.Collections;
using VMukti.Business.CommonDataContracts;
using VMukti.Presentation.Controls;
using VMuktiGrid.Buddy;
using VMukti;
using System.Xml;
using System.Windows.Markup;
using VMukti.Presentation.Xml;

//This is pod control which loads module in it and will display it to user
//This is place where in entire grid type of nodes net.p2p and http is having important

namespace VMuktiGrid.CustomGrid
{
    public partial class ctlPOD : UserControl, IDisposable
    {

        #region POD Variables
        private bool disposed = false;
        bool blnIsFirstTimeLoded = true;
        bool blnIsAdornerAdded = false;
        private Point mouseOffsetForDrag;
        bool IsDraggingPOD = false;

        public int ObjectID = int.MinValue;
        public bool IsSaved = false;

        public Rectangle rectSuggetion = new Rectangle();
        AdornerLayer Myadornlayer;

        private string strTitle = "";
        public string Title
        {
            get { return strTitle; }
            set
            {
                strTitle = value;
                lblTitle.Content = value;
            }
        }

        private int intColNo = -1;
        public int ColNo
        {
            get { return intColNo; }
            set { intColNo = value; }
        }

        bool IsThreadStarted = false;

        List<string> lstUsersDropped = new List<string>();
        
        public int intCurIndex = 0;

        public clsPageInfo _objPageInfo = null;


        public bool blnIsTwoPanel = false;
        public bool IsTwoPanel
        {
            get { return blnIsTwoPanel; }
            set
            {
                blnIsTwoPanel = value;
            }
        }

        #endregion
        
        #region Module variables

        private int intModuleID = -1;
        public int ModuleID
        {
            get { return intModuleID; }
            set { intModuleID = value; }
        }

        private int _OwnerPodIndex = -1;
        public int OwnerPodIndex
        {
            get { return _OwnerPodIndex; }
            set { _OwnerPodIndex = value; }
        }

        bool _IsBuddyListVisible;
        public bool IsBuddyListVisible
        {
            get { return _IsBuddyListVisible; }
            set
            {
                if (value && _IsCollaborative)
                {
                    objBuddyList.Visibility = Visibility.Visible;
                }
                else if (!value)
                {
                    objBuddyList.Visibility = Visibility.Collapsed;
                }
                _IsBuddyListVisible = value;
            }
        }


        Assembly ass = null;
        Assembly a;
        public ArrayList al = new ArrayList();

        bool _IsCollaborative;
        int[] _arrPermissionValue;

        public string IsCollaborative
        {
            get { return _IsCollaborative.ToString(); }
        }

        public int[] ModulePermissions
        {
            get { return _arrPermissionValue; }
        }

        bool _flag;

        string _strURI;
        public ItemsControl _objParent;
        string _strFromWhere;

        string[] _WCFUri;
        public string[] WCFUri
        {
            get
            {
                return _WCFUri;
            }
            set
            {
                _WCFUri = value;
            }
        }

        public delegate void delLoadModule(object objModuleParams);
        public delLoadModule objDelLoadModule = null;
        
        private bool _LoadModule;
        private bool _IsOnItempanel = false;
        #endregion

        #region Multiple Buddy Selection

        Dictionary<CtlExpanderItem, string> bname;

        public delegate void DelLoadPod4MultipleBuddies(object objPodVariable);
        public DelLoadPod4MultipleBuddies objDelLoadPod4MultipleBuddies = null;

        List<string> onlineBuddy = new List<string>();

        #endregion

        public ctlPOD(int intModID, string strModTitle, string strIsCollaborative, string strURI, int[] arrPermissionValue, bool flag, string strFromWhere, ItemsControl objParent, bool LoadModule, clsPageInfo objPageInfo)
        {
           
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlpod--1", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
            try
            {
                _LoadModule = LoadModule;
                _IsCollaborative = bool.Parse(strIsCollaborative);
                _strURI = strURI;
                _arrPermissionValue = arrPermissionValue;
                _flag = flag;
                _objParent = objParent;
                _strFromWhere = strFromWhere;

                if (objPageInfo != null)
                {
                    _objPageInfo = objPageInfo;
                }

                objDelLoadModule = new delLoadModule(dispMethLoadModule);

                rectSuggetion.Fill = Brushes.Transparent;
                rectSuggetion.Stroke = Brushes.Red;

                DoubleCollection dblCol = new DoubleCollection();
                dblCol.Add(5.0);
                dblCol.Add(5.0);

                rectSuggetion.StrokeDashArray = dblCol;
                rectSuggetion.StrokeDashCap = PenLineCap.Round;
                rectSuggetion.StrokeDashOffset = 50;
                rectSuggetion.StrokeEndLineCap = PenLineCap.Square;
                rectSuggetion.StrokeLineJoin = PenLineJoin.Miter;
                rectSuggetion.StrokeMiterLimit = 50;
                rectSuggetion.RadiusX = 16;
                rectSuggetion.RadiusY = 16;
                rectSuggetion.Height = 100;

                Title = strModTitle;
                ModuleID = intModID;

                ass = Assembly.GetAssembly(typeof(ctlPOD));              
              

                this.Loaded += new RoutedEventHandler(ctlPOD_Loaded);
                if (_IsCollaborative)
                {
                    this.Drop += new DragEventHandler(ctlPOD_Drop);
                }
                else
                {
                    objBuddyList.Height = 0.0;
                    objBuddyList.Width = 0.0;
                }
                if (_objParent != null)
                {
                    ((ItemsControl)_objParent).Items.Insert(0, this);
                    this.ColNo = (int)objParent.Tag;
                }
                if (_strFromWhere == "fromLeftPane")
                {
                    this.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                }
                else if (_strFromWhere == "fromDatabase")
                {
                    this.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                }
                else if (_strFromWhere == "fromOtherPeer")
                {
                    this.AddBuddy(_objPageInfo.strFrom, 0);
                    this.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                }

                //if (_objPageInfo != null)
                //{
                //    if (_objPageInfo.ConfID != 0)
                //    {
                //        btnMinPanel_Click(null, null);
                //    }
                //}
                
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Ctlpod--2", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }


        public ctlPOD(int intModID, string strModTitle, string strIsCollaborative, string strURI, int[] arrPermissionValue, bool flag, string strFromWhere, ItemsControl objParent, bool LoadModule, clsPageInfo objPageInfo,int InsertAT)
        {

            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlpod--1", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
            try
            {
                _LoadModule = LoadModule;
                _IsCollaborative = bool.Parse(strIsCollaborative);
                _strURI = strURI;
                _arrPermissionValue = arrPermissionValue;
                _flag = flag;
                _objParent = objParent;
                _strFromWhere = strFromWhere;

                if (objPageInfo != null)
                {
                    _objPageInfo = objPageInfo;
                }

                objDelLoadModule = new delLoadModule(dispMethLoadModule);

                rectSuggetion.Fill = Brushes.Transparent;
                rectSuggetion.Stroke = Brushes.Red;

                DoubleCollection dblCol = new DoubleCollection();
                dblCol.Add(5.0);
                dblCol.Add(5.0);

                rectSuggetion.StrokeDashArray = dblCol;
                rectSuggetion.StrokeDashCap = PenLineCap.Round;
                rectSuggetion.StrokeDashOffset = 50;
                rectSuggetion.StrokeEndLineCap = PenLineCap.Square;
                rectSuggetion.StrokeLineJoin = PenLineJoin.Miter;
                rectSuggetion.StrokeMiterLimit = 50;
                rectSuggetion.RadiusX = 16;
                rectSuggetion.RadiusY = 16;
                rectSuggetion.Height = 100;

                Title = strModTitle;
                ModuleID = intModID;

                ass = Assembly.GetAssembly(typeof(ctlPOD));


                this.Loaded += new RoutedEventHandler(ctlPOD_Loaded);
                if (_IsCollaborative)
                {
                    this.Drop += new DragEventHandler(ctlPOD_Drop);
                }
                else
                {
                    objBuddyList.Height = 0.0;
                    objBuddyList.Width = 0.0;
                }
                if (_objParent != null)
                {
                    ((ItemsControl)_objParent).Items.Insert(InsertAT,this);
                    this.ColNo = (int)objParent.Tag;
                }
                if (_strFromWhere == "fromLeftPane")
                {
                    this.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                }
                else if (_strFromWhere == "fromDatabase")
                {
                    this.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                }
                else if (_strFromWhere == "fromOtherPeer")
                {
                    this.AddBuddy(_objPageInfo.strFrom, 0);
                    this.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Ctlpod--2", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        //This event is to capture the dropped buddy action and extracts the required information and will send a message to dropped buddy. This will generate one message to transmite in network up to required buddy machine. will be having information of wcf service and design of page.
        void ctlPOD_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Handled = true;
                bool blnBuddyType = true;

                if (e.Data.GetData("VMukti.Presentation.Controls.CtlExpanderItem") != null && e.Data.GetData("VMukti.Presentation.Controls.CtlExpanderItem").GetType()==typeof(CtlExpanderItem))
                {
                    CtlExpanderItem elt = e.Data.GetData("VMukti.Presentation.Controls.CtlExpanderItem") as CtlExpanderItem;

                    #region Check whether it is module or buddy dropped

                    string[] strTag = elt.Tag.ToString().Split(',');
                    List<string> lstTag = new List<string>();
                    for (int i = 0; i < strTag.Length; i++)
                    {
                        if (strTag[i] == "ModuleType")
                        {
                            blnBuddyType = false;
                            break;
                        }
                    }

                    #endregion

                    if (blnBuddyType && this.AddBuddy(((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption))
                    {
                        VMuktiGrid.ctlPage.TabItem objSelectedPage = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent);
                        VMuktiGrid.ctlTab.TabItem objSelectedTab = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent);

                        clsPageInfo objPageInfo = new clsPageInfo();
                        objPageInfo.intPageID = objSelectedPage.ObjectID;
                        objPageInfo.strPageTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedPage.Header).Title;

                        objPageInfo.intOwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                        objPageInfo.intOwnerPageIndex = objSelectedPage.OwnerPageIndex;

                        objPageInfo.strDropType = "OnPod";

                        List<clsTabInfo> lstTabInfos = new List<clsTabInfo>();
                        lstTabInfos.Add(new clsTabInfo());
                        int tabinfoscount = lstTabInfos.Count - 1;
                        lstTabInfos[tabinfoscount].intTabID = objSelectedTab.ObjectID;
                        lstTabInfos[tabinfoscount].strTabTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedTab.Header).Title;
                        VMuktiGrid.CustomGrid.ctlGrid objSelectedGrid = (VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content;
                        List<string> lstBuddyList = new List<string>();

                        lstTabInfos[tabinfoscount].intOwnerTabIndex = objSelectedTab.OwnerTabIndex;
                        lstTabInfos[tabinfoscount].dblC1Width = objSelectedGrid.LeftPanelContainer.ActualWidth;
                        lstTabInfos[tabinfoscount].dblC2Width = objSelectedGrid.CentralPanelContainer.ActualWidth;
                        lstTabInfos[tabinfoscount].dblC3Width = objSelectedGrid.RightPanelContainer.ActualWidth;

                        lstTabInfos[tabinfoscount].dblC4Height = objSelectedGrid.TopPanelContainer.ActualHeight;
                        lstTabInfos[tabinfoscount].dblC5Height = objSelectedGrid.BottomPanelContainer.ActualHeight;

                        List<clsPodInfo> lstPodInfo = new List<clsPodInfo>();
                       

                        lstPodInfo.Add(new clsPodInfo());

                        lstPodInfo[lstPodInfo.Count - 1].intModuleId = this.ModuleID;
                        lstPodInfo[lstPodInfo.Count - 1].strPodTitle = this.Title;
                        lstPodInfo[lstPodInfo.Count - 1].strUri = this.WCFUri;
                        lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = this.ColNo;
                        lstPodInfo[lstPodInfo.Count - 1].intOwnerPodIndex = this.OwnerPodIndex;

                        lstBuddyList.Clear();
                        StackPanel stPodBuddyList = this.objBuddyList.stPanel;
                        for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                        {
                            if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                            {
                                lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                            }
                        }
                        lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                        lstTabInfos[lstTabInfos.Count - 1].objaPods = lstPodInfo.ToArray();
                        objPageInfo.objaTabs = lstTabInfos.ToArray();

                        objPageInfo.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objPageInfo.strTo = ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption;
                        objPageInfo.strMsg = "OPEN_PAGE";

                        this.SetMaxCounter(1, ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption);

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                        {
                            VMukti.App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                        else
                        {
                            try
                            {
                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.EndpointNotFoundException exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CtlPod_Drop()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.CommunicationException exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CtlPOD_Drop()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod_Drop()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        //thread will be started to load the desired module in the pod body, to make entire screen not hanged
        void ctlPOD_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Myadornlayer = AdornerLayer.GetAdornerLayer(this);
                if (Myadornlayer != null)
                {
                    //if (!blnIsAdornerAdded)
                    //{
                        Myadornlayer.Add(new VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner(this));
                    //    blnIsAdornerAdded = true;
                    //}
                    if (blnIsFirstTimeLoded)
                    {
                        if (_objParent == null)
                        {
                            if (this.Parent != null && this.Parent.GetType() == typeof(ItemsControl))
                            {
                                this.ColNo = (int)((ItemsControl)this.Parent).Tag;
                            }
                        }
                        if (!IsThreadStarted && _LoadModule)
                        {
                            IsThreadStarted = true;
                            //thread function
                            LoadModule();
                        }
                        //this.Loaded -= new RoutedEventHandler(ctlPOD_Loaded);
                        blnIsFirstTimeLoded = false;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod_Loaded()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        //function which starts the wcf service for respective widget
        private void LoadModule()
        {
            try
            {
                string[] tempUris = null;
                if (_IsCollaborative && _strURI == null)
                {
                    Thread thLoadModule = new Thread(new ParameterizedThreadStart(thMethLoadModule));
                    object[] objModuleParams = new object[3];
                    try
                    {
                        tempUris = VMukti.App.chHttpSuperNodeService.svcStartAService(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType, intModuleID.ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoaModule()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        tempUris = VMukti.App.chHttpSuperNodeService.svcStartAService(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType, intModuleID.ToString());
                    }
                    catch (System.ServiceModel.CommunicationException ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadModule()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        tempUris = VMukti.App.chHttpSuperNodeService.svcStartAService(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType, intModuleID.ToString());
                    }

                    this.WCFUri = tempUris;
                    if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        objModuleParams[0] = tempUris[0];
                    }
                    else if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        objModuleParams[0] = tempUris[1];
                    }

                    objModuleParams[1] = _arrPermissionValue;
                    objModuleParams[2] = _flag;
                    thLoadModule.Start(objModuleParams);
                }
                else if (_IsCollaborative && _strURI != null)
                {
                    Thread thLoadModule = new Thread(new ParameterizedThreadStart(thMethLoadModule));
                    object[] objModuleParams = new object[3];
                    objModuleParams[0] = _strURI;
                    objModuleParams[1] = _arrPermissionValue;
                    objModuleParams[2] = _flag;
                    if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        this.WCFUri = new string[2];
                        this.WCFUri[0] = _strURI;
                    }
                    else if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        this.WCFUri = new string[2];
                        this.WCFUri[1] = _strURI;
                    }
                    thLoadModule.Start(objModuleParams);
                }
                else
                {
                    this.WCFUri = null;

                    Thread thLoadModule = new Thread(new ParameterizedThreadStart(thMethLoadModule));
                    object[] objModuleParams = new object[2];
                    objModuleParams[0] = _arrPermissionValue;
                    objModuleParams[1] = _flag;
                    thLoadModule.Start(objModuleParams);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadModule()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        #region Module events

        //a thread which is started from loadmodule function will start one dispatcher function to interact with UI
        public void thMethLoadModule(object objModuleParams)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objDelLoadModule, objModuleParams);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PageDispatcher()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        //dispatture function which is started from thread which is started from loadmodule function.
        public void dispMethLoadModule(object objModuleParams)
        {
            try
            {
                object[] obj = (object[])objModuleParams;

                #region Collaborative

                if (obj.Length == 3) // it is true than objModuleParams is having http uri, else it is of net.p2p
                {
                    VMukti.Business.VMuktiGrid.ClsModule objModule = ClsModule.GetPodModule(this.ModuleID);

                    #region Downloading ZipFile

                    Uri zipFileURL = new Uri(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + objModule.ZipFile);
                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files"));
                    }
                    string destination = ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files");
                    if (!File.Exists(destination + "\\" + objModule.ZipFile))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(zipFileURL, destination + "\\" + objModule.ZipFile);
                    }

                    #endregion

                    //if we add new identifiers for POD in a zip file u can find it over here and have to change accordingly
                    #region Extracting

                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");
                    VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                    if (!Directory.Exists(strModPath + "\\" + objModule.ZipFile.Split('.')[0]))
                    {
                        fz.ExtractZip(destination + "\\" + objModule.ZipFile, strModPath, null);
                    }

                    string strXmlPath = strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\configuration.xml";
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
                                    FileInfo fi = new FileInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\" + xp.xMain.SWFFileName);
                                    fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.SWFFileName);
                                }
                            }

                            if (!string.IsNullOrEmpty(xp.xMain.TextFileName))
                            {
                                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName))
                                {
                                    FileInfo fi = new FileInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\" + xp.xMain.TextFileName);
                                    fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispMethLoadModule--copying swf n txt files", "Domains\\SuperNodeServiceDomain.cs");
                        }

                    }


                    #endregion

                    #region Loading ReferencedAssemblies

                    //DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath);
                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0]);
                    ShowDirectory(dirinfomodule);


                    #region extracting imagefile
                    string imagefilename = strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\" + "Control\\" + objModule.ZipFile.Split('.')[0] + ".png";
                    if (File.Exists(imagefilename))
                    {
                        BitmapImage objimage = new BitmapImage();
                        objimage.BeginInit();
                        objimage.UriSource = new Uri(imagefilename);
                        objimage.EndInit();
                        imgPODIcon.BeginInit();
                        imgPODIcon.Source = objimage;
                        imgPODIcon.EndInit();
                    }
                    #endregion

                    //following code will load bal and dal from reflaction in RAM
                    for (int j = 0; j < al.Count; j++)
                    {
                        string[] arraysplit = al[j].ToString().Split('\\');
                        if (arraysplit[arraysplit.Length - 1].ToString() == objModule.AssemblyFile)
                        {
                            a = Assembly.LoadFrom(al[j].ToString());
                            AssemblyName[] an = a.GetReferencedAssemblies();

                            for (int alcount = 0; alcount < al.Count; alcount++)
                            {
                                string strsplit = al[alcount].ToString();
                                string[] strold = strsplit.Split('\\');
                                string strnew = strold[strold.Length - 1].Substring(0, strold[strold.Length - 1].Length - 4);

                                for (int asscount = 0; asscount < an.Length; asscount++)
                                {
                                    if (an[asscount].Name == strnew)
                                    {
                                        Assembly assbal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[alcount].ToString()).GetName());
                                        AssemblyName[] anbal = assbal.GetReferencedAssemblies();
                                        for (int andal = 0; andal < al.Count; andal++)
                                        {
                                            string strsplitdal = al[andal].ToString();
                                            string[] strolddal = strsplitdal.Split('\\');
                                            string strnewdal = strolddal[strolddal.Length - 1].Substring(0, strolddal[strolddal.Length - 1].Length - 4);

                                            for (int asscountdal = 0; asscountdal < anbal.Length; asscountdal++)
                                            {
                                                if (anbal[asscountdal].Name == strnewdal)
                                                {
                                                    Assembly assdal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[andal].ToString()).GetName());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Type[] t1 = a.GetTypes();

                            #region CreatingObject

                            for (int k = 0; k < t1.Length; k++)
                            {
                                if (t1[k].Name == objModule.ClassName)
                                {
                                    try
                                    {
                                        ConstructorInfo[] ci = t1[k].GetConstructors();

                                        for (int constcount = 0; constcount < ci.Length; constcount++)
                                        {
                                            ParameterInfo[] pi = ci[constcount].GetParameters();
                                            if (pi.Length == 4)
                                            {
                                                //if module is collaborative, it will be haveing more than one (nearly 4) parameters and so it will come here and will create object with required parameter.
                                                if (pi[0].ParameterType.Name == "PeerType")
                                                {
                                                    object[] objArg = new object[4];
                                                    objArg[0] = VMuktiInfo.CurrentPeer.CurrPeerType;
                                                    objArg[1] = obj[0].ToString();
                                                    objArg[2] = obj[1];
                                                    if (_strFromWhere == "fromOtherPeer")
                                                    {
                                                        objArg[3] = "Guest";
                                                    }
                                                    else if (_strFromWhere == "fromLeftPane" || _strFromWhere == "fromDatabase")
                                                    {
                                                        objArg[3] = "Host";
                                                    }

                                                    //Object of module will be created here by reflaction
                                                    object obj1 = Activator.CreateInstance(t1[k], BindingFlags.CreateInstance, null, objArg, new System.Globalization.CultureInfo("en-US"));
                                                    grdBody.Tag = t1[k].ToString();


                                                    grdBody.Children.Remove(animImage);
                                                    grdBody.Children.Add((UIElement)obj1);

                                                    if (!this.IsTwoPanel)
                                                    {
                                                        #region changes for pod resize
                                                        ItemsControl objItem = (ItemsControl)(this.Parent);
                                                        if (objItem.Name != "TopPanelContainer" && objItem.Name != "BottomPanelContainer")
                                                        {

                                                            ColumnDefinitionCollection ColCollection = ((Grid)(objItem.Parent)).ColumnDefinitions;
                                                            int colNo = Convert.ToInt32(objItem.Tag) - 1;
                                                            GridLength WidthToBeSet = new GridLength((((UserControl)obj1).Width) / 100, GridUnitType.Star);
                                                            double previousActualWidth = -1;
                                                            if (ColCollection[colNo].Width.Value > 100)
                                                            {
                                                                previousActualWidth = (ColCollection[colNo].Width.Value / 100);
                                                            }
                                                            else
                                                            {
                                                                previousActualWidth = ColCollection[colNo].Width.Value;
                                                                string strPAW = previousActualWidth.ToString();

                                                                int intPAW = Convert.ToInt32(previousActualWidth);
                                                                switch (intPAW)
                                                                {
                                                                    case 1:
                                                                        previousActualWidth = 4;
                                                                        break;
                                                                    case 2:
                                                                        previousActualWidth = 4;
                                                                        break;
                                                                    case 3:
                                                                        previousActualWidth = 4;
                                                                        break;
                                                                    case 4:
                                                                        previousActualWidth = 4;
                                                                        break;
                                                                    case 5:
                                                                        previousActualWidth = 5.5;
                                                                        break;
                                                                    case 6:
                                                                        previousActualWidth = 6.5;
                                                                        break;
                                                                    default:
                                                                        previousActualWidth = 7;
                                                                        break;
                                                                }


                                                            }

                                                            if (previousActualWidth < WidthToBeSet.Value)
                                                            {
                                                                ColCollection[1].Width = new GridLength(0, GridUnitType.Star);
                                                                ColCollection[2].Width = new GridLength(0, GridUnitType.Star);
                                                                ColCollection[0].Width = new GridLength(0, GridUnitType.Star);
                                                                switch (colNo)
                                                                {
                                                                    case 0:
                                                                        ColCollection[1].Width = new GridLength(1, GridUnitType.Star);
                                                                        ColCollection[2].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                    case 1:
                                                                        ColCollection[0].Width = new GridLength(1, GridUnitType.Star);
                                                                        ColCollection[2].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                    case 2:
                                                                        ColCollection[0].Width = new GridLength(1, GridUnitType.Star);
                                                                        ColCollection[1].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                }
                                                                int objint = Convert.ToInt32(WidthToBeSet.Value);
                                                                switch (objint)
                                                                {
                                                                    case 1:
                                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                    case 2:
                                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                    case 3:
                                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                    case 4:
                                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                        break;
                                                                    case 5:
                                                                        ColCollection[colNo].Width = new GridLength(2, GridUnitType.Star);
                                                                        break;
                                                                    case 6:
                                                                        ColCollection[colNo].Width = new GridLength(2.5, GridUnitType.Star);
                                                                        break;
                                                                    default:
                                                                        ColCollection[colNo].Width = new GridLength(2.5, GridUnitType.Star);
                                                                        break;
                                                                }

                                                            }

                                                        }
                                                        #endregion
                                                    }

                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                object obj1 = Activator.CreateInstance(t1[k]); ((UserControl)obj1).SetValue(Canvas.LeftProperty, 0.0);
                                                ((UserControl)obj1).SetValue(Canvas.TopProperty, 0.0);

                                                grdBody.Tag = t1[k].ToString();


                                                grdBody.Children.Remove(animImage);
                                                grdBody.Children.Add((UIElement)obj1);
                                                if (!this.IsTwoPanel)
                                                {
                                                    #region changes for pod resize
                                                    ItemsControl objItem = (ItemsControl)(this.Parent);
                                                    if (objItem.Name != "TopPanelContainer" && objItem.Name != "BottomPanelContainer")
                                                    {

                                                        ColumnDefinitionCollection ColCollection = ((Grid)(objItem.Parent)).ColumnDefinitions;
                                                        int colNo = Convert.ToInt32(objItem.Tag) - 1;
                                                        GridLength WidthToBeSet = new GridLength((((UserControl)obj1).Width) / 100, GridUnitType.Star);
                                                        double previousActualWidth = -1;
                                                        if (ColCollection[colNo].Width.Value > 100)
                                                        {
                                                            previousActualWidth = (ColCollection[colNo].Width.Value / 100);
                                                        }
                                                        else
                                                        {
                                                            previousActualWidth = ColCollection[colNo].Width.Value;
                                                            string strPAW = previousActualWidth.ToString();

                                                            int intPAW = Convert.ToInt32(previousActualWidth);
                                                            switch (intPAW)
                                                            {
                                                                case 1:
                                                                    previousActualWidth = 4;
                                                                    break;
                                                                case 2:
                                                                    previousActualWidth = 4;
                                                                    break;
                                                                case 3:
                                                                    previousActualWidth = 4;
                                                                    break;
                                                                case 4:
                                                                    previousActualWidth = 4;
                                                                    break;
                                                                case 5:
                                                                    previousActualWidth = 5.5;
                                                                    break;
                                                                case 6:
                                                                    previousActualWidth = 6.5;
                                                                    break;
                                                                default:
                                                                    previousActualWidth = 7;
                                                                    break;
                                                            }


                                                        }

                                                        if (previousActualWidth < WidthToBeSet.Value)
                                                        {
                                                            ColCollection[1].Width = new GridLength(0, GridUnitType.Star);
                                                            ColCollection[2].Width = new GridLength(0, GridUnitType.Star);
                                                            ColCollection[0].Width = new GridLength(0, GridUnitType.Star);
                                                            switch (colNo)
                                                            {
                                                                case 0:
                                                                    ColCollection[1].Width = new GridLength(1, GridUnitType.Star);
                                                                    ColCollection[2].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                                case 1:
                                                                    ColCollection[0].Width = new GridLength(1, GridUnitType.Star);
                                                                    ColCollection[2].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                                case 2:
                                                                    ColCollection[0].Width = new GridLength(1, GridUnitType.Star);
                                                                    ColCollection[1].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                            }
                                                            int objint = Convert.ToInt32(WidthToBeSet.Value);
                                                            switch (objint)
                                                            {
                                                                case 1:
                                                                    ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                                case 2:
                                                                    ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                                case 3:
                                                                    ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                                case 4:
                                                                    ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                                    break;
                                                                case 5:
                                                                    ColCollection[colNo].Width = new GridLength(2, GridUnitType.Star);
                                                                    break;
                                                                case 6:
                                                                    ColCollection[colNo].Width = new GridLength(2.5, GridUnitType.Star);
                                                                    break;
                                                                default:
                                                                    ColCollection[colNo].Width = new GridLength(2.5, GridUnitType.Star);
                                                                    break;
                                                            }

                                                        }

                                                    }
                                                    #endregion
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreatingObject--5", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                    }
                                    break;
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                #endregion

                #region NonCollaborative
                //it will create an object of p2p node
                else
                {
                    VMukti.Business.VMuktiGrid.ClsModule objModule = ClsModule.GetPodModule(this.ModuleID);

                    #region Downloading ZipFile

                    Uri zipFileURL = new Uri(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + objModule.ZipFile);
                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files"));
                    }
                    string destination = ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files");
                    if (!File.Exists(destination + "\\" + objModule.ZipFile))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(zipFileURL, destination + "\\" + objModule.ZipFile);
                    }

                    #endregion

                    #region Extracting

                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");
                    VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();

                    if (!Directory.Exists(strModPath + "\\" + objModule.ZipFile.Split('.')[0]))
                    {
                        fz.ExtractZip(destination + "\\" + objModule.ZipFile, strModPath, null);
                    }

                    string strXmlPath = strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\configuration.xml";
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
                                    FileInfo fi = new FileInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\" + xp.xMain.SWFFileName);
                                    fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.SWFFileName);
                                }
                            }

                            if (!string.IsNullOrEmpty(xp.xMain.TextFileName))
                            {
                                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName))
                                {
                                    FileInfo fi = new FileInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\" + xp.xMain.TextFileName);
                                    fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispMethLoadModule-NonCol--copying swf n txt files", "Domains\\SuperNodeServiceDomain.cs");
                        }

                    }


                    #endregion

                    #region Loading ReferencedAssemblies

                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0]);
                    ShowDirectory(dirinfomodule);


                    #region extracting imagefile
                    string imagefilename = strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\" + "Control\\" + objModule.ZipFile.Split('.')[0] + ".png";
                    if (File.Exists(imagefilename))
                    {
                        BitmapImage objimage = new BitmapImage();
                        objimage.BeginInit();
                        objimage.UriSource = new Uri(imagefilename);
                        objimage.EndInit();
                        imgPODIcon.BeginInit();
                        imgPODIcon.Source = objimage;
                        imgPODIcon.EndInit();
                    }
                    #endregion


                    for (int j = 0; j < al.Count; j++)
                    {
                        string[] arraysplit = al[j].ToString().Split('\\');
                        if (arraysplit[arraysplit.Length - 1].ToString() == objModule.AssemblyFile)
                        {
                            a = Assembly.LoadFrom(al[j].ToString());
                            AssemblyName[] an = a.GetReferencedAssemblies();

                            for (int alcount = 0; alcount < al.Count; alcount++)
                            {
                                string strsplit = al[alcount].ToString();
                                string[] strold = strsplit.Split('\\');
                                string strnew = strold[strold.Length - 1].Substring(0, strold[strold.Length - 1].Length - 4);

                                for (int asscount = 0; asscount < an.Length; asscount++)
                                {
                                    if (an[asscount].Name == strnew)
                                    {
                                        Assembly assbal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[alcount].ToString()).GetName());
                                        AssemblyName[] anbal = assbal.GetReferencedAssemblies();
                                        for (int andal = 0; andal < al.Count; andal++)
                                        {
                                            string strsplitdal = al[andal].ToString();
                                            string[] strolddal = strsplitdal.Split('\\');
                                            string strnewdal = strolddal[strolddal.Length - 1].Substring(0, strolddal[strolddal.Length - 1].Length - 4);

                                            for (int asscountdal = 0; asscountdal < anbal.Length; asscountdal++)
                                            {
                                                if (anbal[asscountdal].Name == strnewdal)
                                                {
                                                    Assembly assdal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[andal].ToString()).GetName());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Type[] t1 = a.GetTypes();

                            #region CreatingObject

                            for (int k = 0; k < t1.Length; k++)
                            {
                                if (t1[k].Name == objModule.ClassName)
                                {
                                    try
                                    {
                                        object[] objArg = new object[1];
                                        objArg[0] = obj[0];

                                        object obj1 = Activator.CreateInstance(t1[k], objArg);
                                        ((UserControl)obj1).SetValue(Canvas.LeftProperty, 0.0);
                                        ((UserControl)obj1).SetValue(Canvas.TopProperty, 0.0);

                                        grdBody.Tag = t1[k].ToString();
                                        grdBody.Children.Remove(animImage);
                                        grdBody.Children.Add((UIElement)obj1);

                                        if (!this.IsTwoPanel)
                                        {
                                            #region changes for pod resize
                                            ItemsControl objItem = (ItemsControl)(this.Parent);
                                            ColumnDefinitionCollection ColCollection = ((Grid)(objItem.Parent)).ColumnDefinitions;
                                            int colNo = Convert.ToInt32(objItem.Tag) - 1;
                                            GridLength WidthToBeSet = new GridLength((((UserControl)obj1).Width) / 100, GridUnitType.Star);
                                            double previousActualWidth = -1;
                                            if (ColCollection[colNo].Width.Value > 100)
                                            {
                                                previousActualWidth = (ColCollection[colNo].Width.Value / 100);
                                            }
                                            else
                                            {
                                                previousActualWidth = ColCollection[colNo].Width.Value;
                                                string strPAW = previousActualWidth.ToString();

                                                int intPAW = Convert.ToInt32(previousActualWidth);
                                                switch (intPAW)
                                                {
                                                    case 1:
                                                        previousActualWidth = 4;
                                                        break;
                                                    case 2:
                                                        previousActualWidth = 4;
                                                        break;
                                                    case 3:
                                                        previousActualWidth = 4;
                                                        break;
                                                    case 4:
                                                        previousActualWidth = 4;
                                                        break;
                                                    case 5:
                                                        previousActualWidth = 5.5;
                                                        break;
                                                    case 6:
                                                        previousActualWidth = 6.5;
                                                        break;
                                                    default:
                                                        previousActualWidth = 7;
                                                        break;
                                                }


                                            }

                                            if (previousActualWidth < WidthToBeSet.Value)
                                            {
                                                ColCollection[1].Width = new GridLength(0, GridUnitType.Star);
                                                ColCollection[2].Width = new GridLength(0, GridUnitType.Star);
                                                ColCollection[0].Width = new GridLength(0, GridUnitType.Star);
                                                switch (colNo)
                                                {
                                                    case 0:
                                                        ColCollection[1].Width = new GridLength(1, GridUnitType.Star);
                                                        ColCollection[2].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                    case 1:
                                                        ColCollection[0].Width = new GridLength(1, GridUnitType.Star);
                                                        ColCollection[2].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                    case 2:
                                                        ColCollection[0].Width = new GridLength(1, GridUnitType.Star);
                                                        ColCollection[1].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                }
                                                int objint = Convert.ToInt32(WidthToBeSet.Value);
                                                switch (objint)
                                                {
                                                    case 1:
                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                    case 2:
                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                    case 3:
                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                    case 4:
                                                        ColCollection[colNo].Width = new GridLength(1, GridUnitType.Star);
                                                        break;
                                                    case 5:
                                                        ColCollection[colNo].Width = new GridLength(2, GridUnitType.Star);
                                                        break;
                                                    case 6:
                                                        ColCollection[colNo].Width = new GridLength(2.5, GridUnitType.Star);
                                                        break;
                                                    default:
                                                        ColCollection[colNo].Width = new GridLength(2.5, GridUnitType.Star);
                                                        break;
                                                }

                                            }
                                            #endregion
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreatingObject--7", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                        grdBody.Children.Add(new UIElement());
                                    }

                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                }
                #endregion

                al.Clear();
                //this part of the code is for infoming back the user who droped this user that module is loaded so that u can now show the user in ur buddy list.
                //if (_objPageInfo != null)
                //{

                //    clsBuddyRetPageInfo objBuddyRetPageInfo = new clsBuddyRetPageInfo();
                //    List<clsBuddyRetTabInfo> lstBuddyRetTabInfo = new List<clsBuddyRetTabInfo>();
                //    List<clsBuddyRetPodInfo> lstBuddyRetPodInfo = new List<clsBuddyRetPodInfo>();

                //    objBuddyRetPageInfo.intOwnerID = _objPageInfo.intOwnerID;
                //    objBuddyRetPageInfo.intOwnerPageIndex = _objPageInfo.intOwnerPageIndex;
                //    objBuddyRetPageInfo.intPageID = _objPageInfo.intPageID;
                //    objBuddyRetPageInfo.strDropType = _objPageInfo.strDropType;
                //    int ownerindex = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).OwnerTabIndex;
                //    for (int i = 0; i < _objPageInfo.objaTabs.Length; i++)
                //    {
                //        if (ownerindex == _objPageInfo.objaTabs[i].intOwnerTabIndex)
                //        {
                //            int lstBuddyRetTabInfoCount = lstBuddyRetTabInfo.Count - 1;
                //            lstBuddyRetTabInfo.Add(new clsBuddyRetTabInfo());
                //            lstBuddyRetTabInfo[lstBuddyRetTabInfoCount].intOwnerTabIndex = _objPageInfo.objaTabs[i].intOwnerTabIndex;
                //            lstBuddyRetTabInfo[lstBuddyRetTabInfoCount].intTabID = _objPageInfo.objaTabs[i].intTabID;

                //            for (int j = 0; j < _objPageInfo.objaTabs[i].objaPods.Length; j++)
                //            {
                //                if (this.OwnerPodIndex == _objPageInfo.objaTabs[i].objaPods[j].intOwnerPodIndex)
                //                {
                //                    lstBuddyRetPodInfo.Add(new clsBuddyRetPodInfo());
                //                    lstBuddyRetPodInfo[lstBuddyRetTabInfoCount].intColumnNumber = _objPageInfo.objaTabs[i].objaPods[j].intColumnNumber;
                //                    lstBuddyRetPodInfo[lstBuddyRetTabInfoCount].intModuleId = _objPageInfo.objaTabs[i].objaPods[j].intModuleId;
                //                    lstBuddyRetPodInfo[lstBuddyRetTabInfoCount].intOwnerPodIndex = _objPageInfo.objaTabs[i].objaPods[j].intOwnerPodIndex;
                //                    lstBuddyRetPodInfo[lstBuddyRetTabInfoCount].straPodBuddies = _objPageInfo.objaTabs[i].objaPods[j].straPodBuddies;
                //                }
                //            }

                //            lstBuddyRetTabInfo[lstBuddyRetTabInfoCount].objaPods = lstBuddyRetPodInfo.ToArray();
                //        }
                //    }

                //    objBuddyRetPageInfo.objaTabs = lstBuddyRetTabInfo.ToArray();
                //    objBuddyRetPageInfo.strFrom = _objPageInfo.strTo;
                //    objBuddyRetPageInfo.strTo = _objPageInfo.strFrom;
                //    objBuddyRetPageInfo.strMsg = "MODULE_LOADED";


                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispMethoadModule()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public void ShowDirectory(DirectoryInfo dir)
        {
            try
            {
                foreach (FileInfo file in dir.GetFiles("*.dll"))
                {
                    int hj = al.Add(file.FullName);
                }
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    ShowDirectory(subDir);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowDirectory()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public void SetReturnBuddyStatus(string strBuddyName)
        {
            try
            {
                this.ShowBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetReturnBuddyStatus()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        #endregion

        public bool AddBuddy(string strBuddyName)
        {
            try
            {
                return objBuddyList.AddBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");                
                return false;
            }
        }

        public bool RemoveBuddy(string strBuddyName)
        {
            try
            {
                return objBuddyList.RemoveBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                return false;
            }
        }

        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                objBuddyList.SetMaxCounter(intMaxCounter, strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetMaxCounter()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public bool AddBuddy(string strBuddyName, int intMaxCounter)
        {
            try
            {
                return objBuddyList.AddBuddy(strBuddyName, intMaxCounter);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");                
                return false;
            }
        }

        public bool CheckBuddy(string strBuddyName)
        {
            try
            {
                return objBuddyList.CheckBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");                
                return false;
            }
        }

        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                objBuddyList.ShowBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }
        
        //when module is dragging from leftpan but 
        //when pod is being dragged from already exitance position over another pod then this event will occur and is working fine.
        void ctlPOD_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.IsDraggingPOD && VMuktiHelper.IsDraggingPOD && e.GetPosition(rowDef2 as IInputElement).Y > 25.0)
                {
                    if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null)
                    {
                        VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                        foreach (object UIItem in ((ItemsControl)this.Parent).Items)
                        {
                            if (UIItem.GetType() == typeof(Rectangle))
                            {
                                ((ItemsControl)this.Parent).Items.Remove(UIItem);
                                break;
                            }
                        }
                        ((ItemsControl)this.Parent).Items.Insert(((ItemsControl)this.Parent).Items.IndexOf(this) + 1, VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                        this.MouseMove -= new MouseEventHandler(ctlPOD_MouseMove);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod_MouseMove()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        void brdPOD_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.IsDraggingPOD && VMuktiHelper.IsDraggingPOD && VMuktiHelper.objPOD != null)
                {
                    this.MouseMove += new MouseEventHandler(ctlPOD_MouseMove);

                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    ((ItemsControl)this.Parent).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    ((ItemsControl)this.Parent).Items.Insert(((ItemsControl)this.Parent).Items.IndexOf(this), VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "brdPod_MouseEnter()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }
        
        #region grid events

        public void grdTitle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //this change is done for  shifting the pod on the central panel when user is using his saved pages 
                //if (VMukti.App.blnIsTwoPanel == true && this.Parent.GetType() == typeof(ItemsControl) && ((ItemsControl)this.Parent).Name == "RightPanelContainer")
                if (this.IsTwoPanel && this.Parent.GetType() == typeof(ItemsControl) && ((ItemsControl)this.Parent).Name == "RightPanelContainer")
                {
                    //shifting the pod on the central panel

                    VMuktiGrid.ctlPage.TabItem objSelectedPage = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent);


                    //If page from Schedular than shifting the pod on the central panel and shiting centeral panel code to Right Panel.
                    if (objSelectedPage.ConfID != 0)
                    {
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser[objSelectedPage.ConfID] == VMuktiAPI.VMuktiInfo.CurrentPeer.ID)
                        {

                            ctlPOD objpod = this as ctlPOD;
                            ItemsControl CentralIC = ((ItemsControl)((Grid)((ItemsControl)this.Parent).Parent).FindName("CentralPanelContainer"));
                            ItemsControl RighteIC = ((ItemsControl)((Grid)((ItemsControl)this.Parent).Parent).FindName("RightPanelContainer"));
                            int tabid = 0;
                            int RightPanItemCount = RighteIC.Items.Count;
                            if (RightPanItemCount > 0)
                            {
                                ctlPOD objTestPod = RighteIC.Items[0] as ctlPOD;
                                tabid = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)objTestPod.Parent).Parent).Parent).Parent).ObjectID;
                            }

                            ((ItemsControl)this.Parent).Items.RemoveAt(((ItemsControl)this.Parent).Items.IndexOf(this));

                            for (int i = 0; i < CentralIC.Items.Count; i++)
                            {
                                if (CentralIC.Items[i].GetType() == typeof(ctlPOD))
                                {
                                    ctlPOD objtemppod = CentralIC.Items[i] as ctlPOD;
                                    CentralIC.Items.RemoveAt(i);
                                    RighteIC.Items.Insert(RightPanItemCount - 1, objtemppod);
                                    objtemppod.IsTwoPanel = true;
                                    objtemppod.btnHidePanel_Click(null, null);
                                    //call svcPodNavigation event with host name and buddies list and podid
                                }
                            }

                            //int tabid = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)objtemppod.Parent).Parent).Parent).Parent).ObjectID;
                            List<string> lstBuddies = new List<string>();

                            for (int BCnt = 0; BCnt < objpod.objBuddyList.stPanel.Children.Count; BCnt++)
                            {
                                if (objpod.objBuddyList.stPanel.Children[BCnt].GetType() == typeof(ctlBuddy))
                                {
                                    string buddyname = ((ctlBuddy)objpod.objBuddyList.stPanel.Children[BCnt]).Title;
                                    if (buddyname != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        lstBuddies.Add(buddyname);
                                    }
                                }
                            }
                            if (lstBuddies.Count > 0)
                            {
                                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                                {
                                    App.chHttpSuperNodeService.svcPodNavigation(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddies, objSelectedPage.ObjectID, tabid, objpod.ObjectID, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                }
                                else
                                {
                                    App.chNetP2PSuperNodeChannel.svcPodNavigation(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddies, objSelectedPage.ObjectID, tabid, objpod.ObjectID, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                }
                            }

                            CentralIC.Items.Insert(0, objpod);
                            if (grdBody.Visibility == Visibility.Collapsed)
                            {
                                objpod.btnHidePanel_Click(null, null);
                            }
                        }
                    }
                }
                else
                {
                    this.Opacity = 0.5;

                    this.IsDraggingPOD = true;
                    VMuktiHelper.IsDraggingPOD = true;
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(Object state)
                    {
                        VMuktiHelper.RectSuggestHeight = this.ActualHeight;
                        return null;
                    }), null);

                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD = this;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    if (this.Parent.GetType() == typeof(ItemsControl))
                    {
                        ((ItemsControl)this.Parent).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                        ((ItemsControl)this.Parent).Items.Insert(((ItemsControl)this.Parent).Items.IndexOf(this), VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);

                        if (e.Timestamp == -99)
                        {
                            mouseOffsetForDrag = new Point();
                        }
                        else
                        {
                            mouseOffsetForDrag = e.MouseDevice.GetPosition(this as IInputElement);
                        }

                        Application.Current.MainWindow.MouseMove += new System.Windows.Input.MouseEventHandler(Parent_MouseMove);
                        Application.Current.MainWindow.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(Parent_MouseLeftButtonUp);

                        base.RaiseEvent(new RoutedEventArgs(ctlPOD.OnPODDragEvent, this));
                        e.MouseDevice.Capture(this as IInputElement);
                        e.MouseDevice.Capture(null);
                    }
                    else
                    {
                        mouseOffsetForDrag = new Point(-1, -1);

                        Application.Current.MainWindow.MouseMove += new System.Windows.Input.MouseEventHandler(Parent_MouseMove);
                        Application.Current.MainWindow.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(Parent_MouseLeftButtonUp);

                        base.RaiseEvent(new RoutedEventArgs(ctlPOD.OnPODDragEvent, this));
                        e.MouseDevice.Capture(this as IInputElement);
                        e.MouseDevice.Capture(null);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "grdTitle_PreviewMouseDown()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        void Parent_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                this.Opacity = 1.0;

                this.MouseMove += new MouseEventHandler(ctlPOD_MouseMove);

                IsDraggingPOD = false;
                VMuktiHelper.IsDraggingPOD = false;

                this.RenderTransform = null;
                FrameworkElement header = sender as FrameworkElement;

                Application.Current.MainWindow.MouseMove -= Parent_MouseMove;
                Application.Current.MainWindow.MouseLeftButtonUp -= Parent_MouseLeftButtonUp;

                base.RaiseEvent(new RoutedEventArgs(ctlPOD.OnPODDropEvent, this));

                try
                {
                    if (this.Parent != null && this.Parent.GetType() == typeof(ItemsControl) && ((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).IsDragEnter == false)
                    {
                        ((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).IsDragEnter = true;
                        }
                    else if (this.Parent == null)
                    {
                        base.RaiseEvent(new RoutedEventArgs(ctlPOD.OnPODDropEvent, intCurIndex));
                    }

                    if (this.Parent != null && !_LoadModule)
                    {
                        VisualTreeHelper.HitTest(this.Parent as System.Windows.Media.Visual, new HitTestFilterCallback(this.FilterHitTestResultsCallback), new HitTestResultCallback(this.HitTestResultCallback), new PointHitTestParameters(Mouse.PrimaryDevice.GetPosition(this.Parent as IInputElement)));
                    }
                    if (!IsThreadStarted && _IsOnItempanel)
                    {
                        IsThreadStarted = true;
                        LoadModule();
                        _LoadModule = true;
                    }

                    if (!_IsOnItempanel && this.Parent != null && this.Parent.GetType() == typeof(Grid))
                    {
                        ((Grid)this.Parent).Children.Clear();
                    }
                }
                catch
                {
                    if (this.Parent == null)
                    {
                        base.RaiseEvent(new RoutedEventArgs(ctlPOD.OnPODDropEvent, intCurIndex));
                    }

                    if (this.Parent != null && !_LoadModule)
                    {
                        VisualTreeHelper.HitTest(this.Parent as System.Windows.Media.Visual, new HitTestFilterCallback(this.FilterHitTestResultsCallback), new HitTestResultCallback(this.HitTestResultCallback), new PointHitTestParameters(Mouse.PrimaryDevice.GetPosition(this.Parent as IInputElement)));
                    }
                    if (!IsThreadStarted && _IsOnItempanel)
                    {
                        IsThreadStarted = true;
                        LoadModule();
                        _LoadModule = true;
                    }

                    if (!_IsOnItempanel && this.Parent != null && this.Parent.GetType() == typeof(Grid))
                    {
                        ((Grid)this.Parent).Children.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Parent_MouseLeftButtonUp", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        private HitTestFilterBehavior FilterHitTestResultsCallback(DependencyObject target)
        {
            if (!target.GetType().IsAssignableFrom(typeof(ItemsControl)))
            {
                _IsOnItempanel = false;
            }
            else
            {
                _IsOnItempanel = true;
            }
            return HitTestFilterBehavior.Stop;
        }


        private HitTestResultBehavior HitTestResultCallback(HitTestResult result)
        {
            return HitTestResultBehavior.Stop;
        }

        void Parent_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                e.MouseDevice.Synchronize();

                Point mouseOffsetRelativeToParent = e.MouseDevice.GetPosition(this.Parent as IInputElement);
                Point offsetTansformDistance = new Point(mouseOffsetRelativeToParent.X - mouseOffsetForDrag.X, mouseOffsetRelativeToParent.Y - mouseOffsetForDrag.Y);

                this.RenderTransform = new TranslateTransform(offsetTansformDistance.X, offsetTansformDistance.Y);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Parent_MouseMove()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        #endregion

        public event RoutedEventHandler OnPODDrop
        {
            add
            {
                base.AddHandler(ctlPOD.OnPODDropEvent, value);
            }
            remove
            {
                base.RemoveHandler(ctlPOD.OnPODDropEvent, value);
            }
        }

        public static readonly RoutedEvent OnPODDropEvent = EventManager.RegisterRoutedEvent("OnPanelDrop", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ctlPOD));

        public event RoutedEventHandler OnPODDrag
        {
            add
            {
                base.AddHandler(ctlPOD.OnPODDragEvent, value);
            }
            remove
            {
                base.RemoveHandler(ctlPOD.OnPODDragEvent, value);
            }
        }

        public static readonly RoutedEvent OnPODDragEvent = EventManager.RegisterRoutedEvent("OnPanelDrag", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ctlPOD));

        #region Pod title bar events

        // to minimize the pod
        private void btnMinPanel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((grdTitle.Visibility == Visibility.Visible) && ((RotateTransform)((TransformGroup)btnMinPanel.GetValue(Button.RenderTransformProperty)).Children[2]).Angle == 90)
                {
                    grdTitle.Visibility = Visibility.Collapsed;
                    ((RotateTransform)((TransformGroup)btnMinPanel.GetValue(Button.RenderTransformProperty)).Children[2]).Angle = 270;
                    btnHidePanel.Visibility = Visibility.Collapsed;
                    btnMinPanel.Visibility = Visibility.Collapsed;
                    rowDef1.Height = new GridLength(0, GridUnitType.Pixel);
                    btnMinPanel1.Margin = new Thickness(0, 5, 0, 0);
                    btnMinPanel1.Width = 16.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMinPanel_Click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        //while closing the pod... current buddy should be removed from all other participants' buddy list, which is handled below.
        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.grdBody.Children.Count >= 2)
                {
                    MethodInfo mi = this.grdBody.Children[1].GetType().GetMethod("ClosePod");
                    if (mi != null)
                    {
                        mi.Invoke(this.grdBody.Children[1], null);
                    }


                    if (sender != null)
                    {
                        for (int i = 0; i < this.objBuddyList.stPanel.Children.Count; i++)
                        {
                            if (this.objBuddyList.stPanel.Children[i].GetType() == typeof(ctlBuddy))
                            {
                                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != ((ctlBuddy)this.objBuddyList.stPanel.Children[i]).Title)
                                {
                                    lstUsersDropped.Add(((ctlBuddy)this.objBuddyList.stPanel.Children[i]).Title);
                                }
                            }
                        }
                        if (this.lstUsersDropped.Count > 0)
                        {
                            clsModuleInfo cmi = new clsModuleInfo();
                            cmi.strPageid = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent).ObjectID.ToString();
                            cmi.strTabid = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).ObjectID.ToString();
                            cmi.strPodid = this.ObjectID.ToString();
                            cmi.strDropType = "Pod Type";

                            try
                            {
                                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                                {
                                    App.chNetP2PSuperNodeChannel.svcSetRemoveDraggedBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstUsersDropped, "CLOSE MODULE", cmi, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                }
                                else
                                {
                                    App.chHttpSuperNodeService.svcSetRemoveDraggedBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstUsersDropped, "CLOSE MODULE", cmi);
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                            }
                        }
                    }
                }

                if (ObjectID != int.MinValue)
                {
                    ((VMuktiGrid.CustomGrid.ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).DeletePOD(ObjectID);
                }


               ((ItemsControl)this.Parent).Items.Remove(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }

        }

        //Call this method when closing page or tab
        public void btnCloseFromPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.grdBody.Children.Count >= 2)
                {
                    MethodInfo mi = this.grdBody.Children[1].GetType().GetMethod("ClosePod");
                    if (mi != null)
                    {
                        mi.Invoke(this.grdBody.Children[1], null);
                    }


                    if (sender != null)
                    {
                        for (int i = 0; i < this.objBuddyList.stPanel.Children.Count; i++)
                        {
                            if (this.objBuddyList.stPanel.Children[i].GetType() == typeof(ctlBuddy))
                            {
                                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != ((ctlBuddy)this.objBuddyList.stPanel.Children[i]).Title)
                                {
                                    lstUsersDropped.Add(((ctlBuddy)this.objBuddyList.stPanel.Children[i]).Title);
                                }
                            }
                        }
                        if (this.lstUsersDropped.Count > 0)
                        {
                            clsModuleInfo cmi = new clsModuleInfo();
                            cmi.strPageid = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent).ObjectID.ToString();
                            cmi.strTabid = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).ObjectID.ToString();
                            cmi.strPodid = this.ObjectID.ToString();
                            cmi.strDropType = "Pod Type";

                            try
                            {
                                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                                {
                                    App.chNetP2PSuperNodeChannel.svcSetRemoveDraggedBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstUsersDropped, "CLOSE MODULE", cmi, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                }
                                else
                                {
                                    App.chHttpSuperNodeService.svcSetRemoveDraggedBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstUsersDropped, "CLOSE MODULE", cmi);
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                            }
                        }
                    }
                }

                if (ObjectID != int.MinValue)
                {
                    ((VMuktiGrid.CustomGrid.ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).DeletePOD(ObjectID);
                }


                // ((ItemsControl)this.Parent).Items.Remove(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCloseFromPage_Click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }

        }

        //titlebar will be hide by raising this event.
        public void btnHidePanel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((grdBody.Visibility == Visibility.Visible) && ((RotateTransform)((TransformGroup)btnHidePanel.GetValue(Button.RenderTransformProperty)).Children[2]).Angle == 0)
                {
                    grdBody.Visibility = Visibility.Collapsed;
                    ((RotateTransform)((TransformGroup)btnHidePanel.GetValue(Button.RenderTransformProperty)).Children[2]).Angle = 270;
                    btnMinPanel.Visibility = Visibility.Collapsed;
                    btnClose.Margin = new Thickness(0, 5, 5, 5);
                    btnMinPanel.Margin = new Thickness(0, -100, 0, 0);
                    brdPOD.CornerRadius = new CornerRadius(7, 7, 7, 7);
                   // Myadornlayer.Visibility = Visibility.Visible;
                    
                }
                else
                {
                    grdBody.Visibility = Visibility.Visible;
                    ((RotateTransform)((TransformGroup)btnHidePanel.GetValue(Button.RenderTransformProperty)).Children[2]).Angle = 0;
                    btnMinPanel.Visibility = Visibility.Visible;
                    btnClose.Margin = new Thickness(0, 5, 17, 5);
                    btnMinPanel.Margin = new Thickness(0, 0, 0, 0);
                    brdPOD.CornerRadius = new CornerRadius(7, 7, 0, 0);
                    //Myadornlayer.Visibility = Visibility.Collapsed;
                    
                }
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(Object state)
                {
                    VMuktiHelper.RectSuggestHeight = this.ActualHeight;
                    return null;
                }), null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnHidePanel_click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }
        //titlebar will be unhide by raising this event.
        private void btnMinPanel1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdTitle.Visibility = Visibility.Visible;
                ((RotateTransform)((TransformGroup)btnMinPanel.GetValue(Button.RenderTransformProperty)).Children[2]).Angle = 90;
                btnHidePanel.Visibility = Visibility.Visible;
                btnMinPanel.Visibility = Visibility.Visible;
                rowDef1.Height = new GridLength(25, GridUnitType.Pixel);
                btnMinPanel1.Visibility = Visibility.Collapsed;
                btnMinPanel1.Margin = new Thickness(0, -100, 0, 0);
                btnMinPanel1.Width = 0.0;
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(Object state)
                {
                    VMuktiHelper.RectSuggestHeight = this.ActualHeight;
                    return null;
                }), null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMinPanel1_Click()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        #endregion
        // this is only cunsumed by page object there is no UI for this function. It saves pod related information to the database.
        public void Save(int tabID)
        {
            try
            {
                if (!IsSaved)
                {
                    if (ObjectID == int.MinValue)
                    {
                        ObjectID = VMukti.Business.VMuktiGrid.ClsPod.AddPod(-1, tabID, strTitle, intColNo, "", ModuleID, VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                    }
                    else
                    {
                        VMukti.Business.VMuktiGrid.ClsPod.AddPod(ObjectID, tabID, strTitle, intColNo, "", ModuleID, VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                    }
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public void Delete()
        {
          
        }

        #region Multiple Buddy Selection

        public ctlPOD(int intModID, string strModTitle, string strIsCollaborative, string strURI, int[] arrPermissionValue, bool flag, string strFromWhere, ItemsControl objParent, Dictionary<CtlExpanderItem, string> buddiesname)
        {
            try
            {
                InitializeComponent();

                bname = new Dictionary<CtlExpanderItem, string>();
                bname = buddiesname;
                foreach (KeyValuePair<CtlExpanderItem, string> kvp in bname)
                {
                    if (((CtlExpanderItem)kvp.Key).Tag.ToString().Trim() == "online")
                    {
                        onlineBuddy.Add(kvp.Value);
                    }
                }
                _IsCollaborative = bool.Parse(strIsCollaborative);
                _strURI = strURI;
                _arrPermissionValue = arrPermissionValue;
                _flag = flag;
                _objParent = objParent;
                _strFromWhere = strFromWhere;

                objDelLoadPod4MultipleBuddies = new DelLoadPod4MultipleBuddies(LoadPod4MultipleBuddies);

                rectSuggetion.Fill = Brushes.Transparent;
                rectSuggetion.Stroke = Brushes.Red;

                DoubleCollection dblCol = new DoubleCollection();
                dblCol.Add(5.0);
                dblCol.Add(5.0);

                rectSuggetion.StrokeDashArray = dblCol;
                rectSuggetion.StrokeDashCap = PenLineCap.Round;
                rectSuggetion.StrokeDashOffset = 50;
                rectSuggetion.StrokeEndLineCap = PenLineCap.Square;
                rectSuggetion.StrokeLineJoin = PenLineJoin.Miter;
                rectSuggetion.StrokeMiterLimit = 50;
                rectSuggetion.RadiusX = 16;
                rectSuggetion.RadiusY = 16;
                rectSuggetion.Height = 100;

                Title = strModTitle;
                ModuleID = intModID;

                ass = Assembly.GetAssembly(typeof(ctlPOD));

                this.Loaded += new RoutedEventHandler(ctlPOD_Loaded4MultipleBuddies);
                this.Drop += new DragEventHandler(ctlPOD_Drop4MultipleBuddies);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod--2", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public ctlPOD(int intModID, string strModTitle, string strIsCollaborative, string strURI, int[] arrPermissionValue, bool flag, string strFromWhere, ItemsControl objParent, List<string> buddiesname)
        {
            try
            {
                InitializeComponent();

                onlineBuddy = buddiesname;
                _IsCollaborative = bool.Parse(strIsCollaborative);
                _strURI = strURI;
                _arrPermissionValue = arrPermissionValue;
                _flag = flag;
                _objParent = objParent;
                _strFromWhere = strFromWhere;

                objDelLoadPod4MultipleBuddies = new DelLoadPod4MultipleBuddies(LoadPod4MultipleBuddies);

                rectSuggetion.Fill = Brushes.Transparent;
                rectSuggetion.Stroke = Brushes.Red;

                DoubleCollection dblCol = new DoubleCollection();
                dblCol.Add(5.0);
                dblCol.Add(5.0);

                rectSuggetion.StrokeDashArray = dblCol;
                rectSuggetion.StrokeDashCap = PenLineCap.Round;
                rectSuggetion.StrokeDashOffset = 50;
                rectSuggetion.StrokeEndLineCap = PenLineCap.Square;
                rectSuggetion.StrokeLineJoin = PenLineJoin.Miter;
                rectSuggetion.StrokeMiterLimit = 50;
                rectSuggetion.RadiusX = 16;
                rectSuggetion.RadiusY = 16;
                rectSuggetion.Height = 100;

                Title = strModTitle;
                ModuleID = intModID;

                ass = Assembly.GetAssembly(typeof(ctlPOD));

                this.Loaded += new RoutedEventHandler(ctlPOD_Loaded4MultipleBuddies);
                this.Drop += new DragEventHandler(ctlPOD_Drop4MultipleBuddies);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod--2", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        private void LoadPod()
        {
            try
            {
                string[] tempUris = null;
                Thread thLoadPod4MultipleBuddies = new Thread(new ParameterizedThreadStart(thMethPod4MultipleBuddies));
                object[] objModuleParams = new object[3];

                if (_IsCollaborative && _strURI == null)
                {
                    try
                    {
                        tempUris = VMukti.App.chHttpSuperNodeService.svcStartAService(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType, intModuleID.ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadModule()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        tempUris = VMukti.App.chHttpSuperNodeService.svcStartAService(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType, intModuleID.ToString());
                    }
                    catch (System.ServiceModel.CommunicationException ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPod()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        tempUris = VMukti.App.chHttpSuperNodeService.svcStartAService(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType, intModuleID.ToString());
                    }

                    this.WCFUri = tempUris;
                    if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        objModuleParams[0] = tempUris[0];
                    }
                    else if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        objModuleParams[0] = tempUris[1];
                    }
                }

                else if (_IsCollaborative && (_strURI != null || _strURI != ""))
                {
                    this.WCFUri = new string[2];
                    this.WCFUri[1] = _strURI;
                    objModuleParams[0] = _strURI;
                }

                objModuleParams[1] = _arrPermissionValue;
                objModuleParams[2] = _flag;
                thLoadPod4MultipleBuddies.Start(objModuleParams);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadModule()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public void thMethPod4MultipleBuddies(object objModuleParams)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objDelLoadPod4MultipleBuddies, objModuleParams);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PageDispatcher()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        public void LoadPod4MultipleBuddies(object objModuleParams)
        {
            try
            {
                object[] obj = (object[])objModuleParams;

                #region Collaborative

                if (obj.Length == 3)
                {
                    VMukti.Business.VMuktiGrid.ClsModule objModule = ClsModule.GetPodModule(this.ModuleID);

                    #region Downloading ZipFile

                    Uri zipFileURL = new Uri(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + objModule.ZipFile);
                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files"));
                    }
                    string destination = ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files");
                    if (!File.Exists(destination + "\\" + objModule.ZipFile))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(zipFileURL, destination + "\\" + objModule.ZipFile);
                    }

                    #endregion

                    #region Extracting

                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");
                    VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                    if (!Directory.Exists(strModPath + "\\" + objModule.ZipFile.Split('.')[0]))
                    {
                        fz.ExtractZip(destination + "\\" + objModule.ZipFile, strModPath, null);
                    }

                    string strXmlPath = strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\configuration.xml";
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
                                    FileInfo fi = new FileInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\" + xp.xMain.SWFFileName);
                                    fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.SWFFileName);
                                }
                            }

                            if (!string.IsNullOrEmpty(xp.xMain.TextFileName))
                            {
                                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName))
                                {
                                    FileInfo fi = new FileInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\Control\\" + xp.xMain.TextFileName);
                                    fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + xp.xMain.TextFileName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPod4MultipleBuddies--copying swf n txt files", "Domains\\SuperNodeServiceDomain.cs");
                        }

                    }


                    #endregion

                    #region Loading ReferencedAssemblies

                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + objModule.ZipFile.Split('.')[0]);
                    ShowDirectory(dirinfomodule);


                    #region extracting imagefile
                    string imagefilename = strModPath + "\\" + objModule.ZipFile.Split('.')[0] + "\\" + "Control\\" + objModule.ZipFile.Split('.')[0] + ".png";
                    if (File.Exists(imagefilename))
                    {
                        BitmapImage objimage = new BitmapImage();
                        objimage.BeginInit();
                        objimage.UriSource = new Uri(imagefilename);
                        objimage.EndInit();
                        imgPODIcon.BeginInit();
                        imgPODIcon.Source = objimage;
                        imgPODIcon.EndInit();
                    }
                    #endregion

                    for (int j = 0; j < al.Count; j++)
                    {
                        string[] arraysplit = al[j].ToString().Split('\\');
                        if (arraysplit[arraysplit.Length - 1].ToString() == objModule.AssemblyFile)
                        {
                            a = Assembly.LoadFrom(al[j].ToString());
                            AssemblyName[] an = a.GetReferencedAssemblies();

                            for (int alcount = 0; alcount < al.Count; alcount++)
                            {
                                string strsplit = al[alcount].ToString();
                                string[] strold = strsplit.Split('\\');
                                string strnew = strold[strold.Length - 1].Substring(0, strold[strold.Length - 1].Length - 4);

                                for (int asscount = 0; asscount < an.Length; asscount++)
                                {
                                    if (an[asscount].Name == strnew)
                                    {
                                        Assembly assbal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[alcount].ToString()).GetName());
                                        AssemblyName[] anbal = assbal.GetReferencedAssemblies();
                                        for (int andal = 0; andal < al.Count; andal++)
                                        {
                                            string strsplitdal = al[andal].ToString();
                                            string[] strolddal = strsplitdal.Split('\\');
                                            string strnewdal = strolddal[strolddal.Length - 1].Substring(0, strolddal[strolddal.Length - 1].Length - 4);

                                            for (int asscountdal = 0; asscountdal < anbal.Length; asscountdal++)
                                            {
                                                if (anbal[asscountdal].Name == strnewdal)
                                                {
                                                    Assembly assdal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[andal].ToString()).GetName());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Type[] t1 = a.GetTypes();

                            #region CreatingObject

                            for (int k = 0; k < t1.Length; k++)
                            {
                                if (t1[k].Name == objModule.ClassName)
                                {
                                    try
                                    {
                                        ConstructorInfo[] ci = t1[k].GetConstructors();

                                        for (int constcount = 0; constcount < ci.Length; constcount++)
                                        {
                                            ParameterInfo[] pi = ci[constcount].GetParameters();
                                            if (pi.Length == 4)
                                            {
                                                if (pi[0].ParameterType.Name == "PeerType")
                                                {
                                                    object[] objArg = new object[4];
                                                    objArg[0] = VMuktiInfo.CurrentPeer.CurrPeerType;
                                                    objArg[1] = obj[0].ToString();
                                                    objArg[2] = obj[1];
                                                    if (_strFromWhere == "fromOtherPeer")
                                                    {
                                                        objArg[3] = "Guest";
                                                    }
                                                    else if (_strFromWhere == "fromLeftPane" || _strFromWhere == "fromDatabase")
                                                    {
                                                        objArg[3] = "Host";
                                                    }

                                                    object obj1 = Activator.CreateInstance(t1[k], BindingFlags.CreateInstance, null, objArg, new System.Globalization.CultureInfo("en-US"));
                                                    grdBody.Tag = t1[k].ToString();

                                                    grdBody.Children.Remove(animImage);
                                                    grdBody.Children.Add((UIElement)obj1);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                object obj1 = Activator.CreateInstance(t1[k]);
                                                ((UserControl)obj1).SetValue(Canvas.LeftProperty, 0.0);
                                                ((UserControl)obj1).SetValue(Canvas.TopProperty, 0.0);

                                                grdBody.Tag = t1[k].ToString();

                                                grdBody.Children.Remove(animImage);
                                                grdBody.Children.Add((UIElement)obj1);
                                                break;

                                            }
                                        }
                                    }
                                    catch (Exception exp)
                                    {
                                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CreatingObject--5", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                }
                #endregion

                al.Clear();

                #region dhaval
                if (_strFromWhere == "fromPeer")
                {
                    for (int onlineBuddyCnt = 0; onlineBuddyCnt < onlineBuddy.Count; onlineBuddyCnt++)
                    {
                        this.AddBuddy(onlineBuddy[onlineBuddyCnt]);
                    }
                }
                #endregion dhaval

                if (_strFromWhere != "fromPeer")
                {
                    InformMultipleBuddy();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PageThread()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        void ctlPOD_Drop4MultipleBuddies(object sender, DragEventArgs e)
        {
            try
            {
                e.Handled = true;
                bool blnBuddyType = true;

                if (e.Data.GetDataPresent(typeof(CtlExpanderItem)))
                {
                    #region Check whether it is module or buddy dropped

                    string[] strTag = ((CtlExpanderItem)e.Data.GetData(typeof(CtlExpanderItem))).Tag.ToString().Split(',');
                    List<string> lstTag = new List<string>();
                    for (int i = 0; i < strTag.Length; i++)
                    {
                        if (strTag[i] == "ModuleType")
                        {
                            blnBuddyType = false;
                            break;
                        }
                    }

                    #endregion

                    if (blnBuddyType && this.AddBuddy(((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption))
                    {
                        VMuktiGrid.ctlPage.TabItem objSelectedPage = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent);
                        VMuktiGrid.ctlTab.TabItem objSelectedTab = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent);

                        clsPageInfo objPageInfo = new clsPageInfo();
                        objPageInfo.intPageID = objSelectedPage.ObjectID;
                        objPageInfo.strPageTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedPage.Header).Title;

                        objPageInfo.intOwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                        objPageInfo.intOwnerPageIndex = ((VMuktiGrid.ctlPage.TabControl)objSelectedPage.Parent).SelectedIndex;

                        objPageInfo.strDropType = "OnPod";

                        List<clsTabInfo> lstTabInfos = new List<clsTabInfo>();
                        lstTabInfos.Add(new clsTabInfo());
                        int lstTabInfoCount = lstTabInfos.Count - 1;
                        lstTabInfos[lstTabInfoCount].intTabID = objSelectedTab.ObjectID;
                        lstTabInfos[lstTabInfoCount].strTabTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedTab.Header).Title;
                        VMuktiGrid.CustomGrid.ctlGrid objSelectedGrid = (VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content;
                        List<string> lstBuddyList = new List<string>();

                        lstTabInfos[lstTabInfoCount].intOwnerTabIndex = ((VMuktiGrid.ctlTab.TabControl)objSelectedTab.Parent).SelectedIndex;
                        lstTabInfos[lstTabInfoCount].dblC1Width = objSelectedGrid.LeftPanelContainer.ActualWidth;
                        lstTabInfos[lstTabInfoCount].dblC2Width = objSelectedGrid.CentralPanelContainer.ActualWidth;
                        lstTabInfos[lstTabInfoCount].dblC3Width = objSelectedGrid.RightPanelContainer.ActualWidth;

                        List<clsPodInfo> lstPodInfo = new List<clsPodInfo>();
                       

                        lstPodInfo.Add(new clsPodInfo());

                        lstPodInfo[lstTabInfoCount].intModuleId = this.ModuleID;
                        lstPodInfo[lstTabInfoCount].strPodTitle = this.Title;
                        lstPodInfo[lstTabInfoCount].strUri = this.WCFUri;
                        lstPodInfo[lstTabInfoCount].intColumnNumber = this.ColNo;
                        lstPodInfo[lstTabInfoCount].intOwnerPodIndex = this.OwnerPodIndex;

                        lstBuddyList.Clear();
                        StackPanel stPodBuddyList = this.objBuddyList.stPanel;
                        for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                        {
                            if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                            {
                                lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                            }
                        }
                        lstPodInfo[lstTabInfoCount].straPodBuddies = lstBuddyList.ToArray();

                        lstTabInfos[lstTabInfoCount].objaPods = lstPodInfo.ToArray();
                        objPageInfo.objaTabs = lstTabInfos.ToArray();

                        objPageInfo.strTo = ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption;
                        objPageInfo.strMsg = "OPEN_PAGE";
                        objPageInfo.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                        {
                            VMukti.App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                        else
                        {
                            try
                            {
                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.EndpointNotFoundException ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod_Drop()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.CommunicationException ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPOD_Drop()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod_Drop()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        void ctlPOD_Loaded4MultipleBuddies(object sender, RoutedEventArgs e)
        {
            try
            {
                Myadornlayer = AdornerLayer.GetAdornerLayer(this);
                if (Myadornlayer != null)
                {
                    //if (!blnIsAdornerAdded)
                    //{
                        Myadornlayer.Add(new VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner(this));
                    //    blnIsAdornerAdded = true;
                    //}

                    if (_objParent == null)
                    {
                        if (this.Parent != null && this.Parent.GetType() == typeof(ItemsControl))
                        {
                            this.ColNo = (int)((ItemsControl)this.Parent).Tag;
                        }
                    }
                    if (!IsThreadStarted)
                    {
                        IsThreadStarted = true;
                        LoadPod();
                    }
                    this.Loaded -= new RoutedEventHandler(ctlPOD_Loaded);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPod_Loaded()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
            }
        }

        void InformMultipleBuddy()
        {
            //if condition only for online buddies

            for (int onlineBuddyCnt = 0; onlineBuddyCnt < onlineBuddy.Count; onlineBuddyCnt++)
            {
                this.AddBuddy(onlineBuddy[onlineBuddyCnt]);
            }
            if (onlineBuddy.Count > 0)
            {
                VMuktiGrid.ctlPage.TabItem objSelectedPage = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent);
                VMuktiGrid.ctlTab.TabItem objSelectedTab = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent);

                List<string> lstBuddyList = new List<string>();
                StackPanel stPodBuddyList = this.objBuddyList.stPanel;
                for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                {
                    if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                    {
                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                    }
                }

                clsModuleInfo objModInfo = new clsModuleInfo();
                objModInfo.intModuleId = this.ModuleID;
                objModInfo.strDropType = "OnPod";
                objModInfo.strModuleName = this.Title;
                objModInfo.strPageid = objSelectedPage.ObjectID.ToString();
                objModInfo.strPodid = objSelectedTab.ObjectID.ToString();
                objModInfo.strUri = this.WCFUri;
                objModInfo.ModPermissions = this._arrPermissionValue;
                objModInfo.lstUsersDropped = this.onlineBuddy;

                clsPageInfo objPageInfo = new clsPageInfo();
                objPageInfo.intPageID = objSelectedPage.ObjectID;
                objPageInfo.strPageTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedPage.Header).Title;
                if (objSelectedPage.OwnerID != VMuktiAPI.VMuktiInfo.CurrentPeer.ID)
                {
                    objPageInfo.intOwnerID = objSelectedPage.OwnerID;
                    objPageInfo.intOwnerPageIndex = objSelectedPage.OwnerPageIndex;
                }
                else
                {
                    objPageInfo.intOwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                    objPageInfo.intOwnerPageIndex = objSelectedPage.OwnerPageIndex;
                }
                //objPageInfo.strDropType = "OnPod";
                objPageInfo.strDropType = "OnBuddyClick";

                List<clsTabInfo> lstTabInfos = new List<clsTabInfo>();
                lstTabInfos.Add(new clsTabInfo());
                int tabinfoscount = lstTabInfos.Count - 1;
                lstTabInfos[tabinfoscount].intTabID = objSelectedTab.ObjectID;
                lstTabInfos[tabinfoscount].strTabTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedTab.Header).Title;
                VMuktiGrid.CustomGrid.ctlGrid objSelectedGrid = (VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content;

                lstTabInfos[tabinfoscount].intOwnerTabIndex = objSelectedTab.OwnerTabIndex;
                lstTabInfos[tabinfoscount].dblC1Width = objSelectedGrid.LeftPanelContainer.ActualWidth;
                lstTabInfos[tabinfoscount].dblC2Width = objSelectedGrid.CentralPanelContainer.ActualWidth;
                lstTabInfos[tabinfoscount].dblC3Width = objSelectedGrid.RightPanelContainer.ActualWidth;
                lstTabInfos[tabinfoscount].dblC4Height = objSelectedGrid.TopPanelContainer.ActualHeight;
                lstTabInfos[tabinfoscount].dblC5Height = objSelectedGrid.BottomPanelContainer.ActualHeight;

                List<clsPodInfo> lstPodInfo = new List<clsPodInfo>();
                lstPodInfo.Add(new clsPodInfo());
                lstPodInfo[lstPodInfo.Count - 1].intModuleId = this.ModuleID;
                lstPodInfo[lstPodInfo.Count - 1].strPodTitle = this.Title;
                lstPodInfo[lstPodInfo.Count - 1].strUri = this.WCFUri;
                lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = this.ColNo;
                if (this.ColNo == 1)
                {
                    this.OwnerPodIndex = objSelectedGrid.LeftPanelContainer.Items.Count;
                }
                else if (this.ColNo == 2)
                {
                    this.OwnerPodIndex = objSelectedGrid.CentralPanelContainer.Items.Count;
                }
                else if (this.ColNo == 3)
                {
                    this.OwnerPodIndex = objSelectedGrid.RightPanelContainer.Items.Count;
                }

                lstPodInfo[lstPodInfo.Count - 1].intOwnerPodIndex = this.OwnerPodIndex;                
                lstBuddyList.Clear();
                lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();                
                lstTabInfos[lstTabInfos.Count - 1].objaPods = lstPodInfo.ToArray();

                objPageInfo.objaTabs = lstTabInfos.ToArray();
                objPageInfo.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                //objPageInfo.strTo = ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption;
                objPageInfo.strTo = this.onlineBuddy[0].ToString();
                objPageInfo.strMsg = "OPEN_PAGE";

                //this.SetMaxCounter(1, ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption);
                this.SetMaxCounter(1, this.onlineBuddy[0].ToString());

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    //VMukti.App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                    VMukti.App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP, objPageInfo);
                }
                else
                {
                    try
                    {
                        //VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo);
                        VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo, objPageInfo);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InformMultipleBuddy()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");                        
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        //VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo);
                        VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo,objPageInfo);
                    }
                    catch (System.ServiceModel.CommunicationException ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InformMultipleBuddy--2", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");                        
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        //VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo);
                        VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, onlineBuddy, "OPEN_PAGE4MultipleBuddies", objModInfo,objPageInfo);
                    }

                }
            }
        }


        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
			{
				if (disposing)
				{
					try
					{
                        lstUsersDropped.Clear();
					}
					catch (Exception ex)
					{
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlPOD()
        {
            Dispose(false);
        }

        #endregion

        private void grdTitle_PreviewDragEnter(object sender, DragEventArgs e)
        {

        }
    }
}
