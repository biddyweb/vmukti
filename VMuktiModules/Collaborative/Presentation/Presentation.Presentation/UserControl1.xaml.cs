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
using VMuktiService;
using System.IO;
using System.ServiceModel;
using System.DirectoryServices;
using Presentation.Bal;
using System.Reflection;
using VMuktiAPI;
using Microsoft.Office.Interop.PowerPoint;
using System.IO.Compression;
using Microsoft.Office.Core;
using System.Windows.Media.Animation;
//using unoidl.com.sun.star.uno;
//using uno.util;
//using unoidl.com.sun.star.lang;
//using unoidl.com.sun.star.frame;
//using unoidl.com.sun.star.util;
using Presentation.Bal.Service.MessageContract;
using System.Windows.Threading;
using System.Drawing;
using System.ComponentModel;


namespace Presentation.Control
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class UserControl1 : UserControl, IDisposable
    {
        #region Uploading Recorded Files
        
        byte[] arr;
        MemoryStream msBuffer;
                
        #endregion

        #region Variables Section
        
        public delegate void DelListBoxItem(string[] itemName);
        DelListBoxItem objListBoxItem;

        //System.Threading.Thread tHostPresentation = null;

        public delegate void DelSetSlide(clsMessageContract mcSetSlide);
        public DelSetSlide objDelSetSlide;

        public delegate void DelSetSlideList(List<object> lstMsg);
        public DelSetSlideList objDelSetSlideList;

        public delegate void DelGetMessage();
        public DelGetMessage objDelGetMsg;

        public delegate void DelRemoveUser(List<object> objData);
        public DelRemoveUser objRemoveUser;

//      ModulePermissions[] modPer;

        object objNetTcpPresentation = new clsNetTcpPresentation();
        INetTcpPresentationChannel channelNetTcp;
        IHttpPresentation channelHttp;

        System.Windows.Threading.DispatcherTimer dispTimerControl = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
       // List<clsMessageContract> lstMessage = new List<clsMessageContract>();

        int temp;
        int tempcounter;
        
        public string strUri;
        public string strRole;
        public Assembly ass;
        string apppath;
        string pptpath;

        public delegate void DelDisplayName(string lstUserName);
        public DelDisplayName objDelDisplayName;

        public delegate void DelSignOutMessage(List<object> lstMsg);
        public DelSignOutMessage objDelSignOutMsg;

        List<string> lstName;

        bool disposed;

        //public unoidl.com.sun.star.beans.PropertyValue aArgs;

        public delegate void DelAsyncGetMessage(clsMessageContract mcAsyncGetMessage);
        public DelAsyncGetMessage objDelAsyncGetMessage;

        Storyboard sbtemp;

        System.Threading.Thread thGlobalVariable;
        BackgroundWorker bgHostService;

        #endregion

        #region Constructor

        public UserControl1(VMuktiAPI.PeerType PeerType, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                
                InitializeComponent();

                thGlobalVariable = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalVariable));
                thGlobalVariable.Start();

                bgHostService = new BackgroundWorker();

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations"))
                {
                    ClsException.WriteToLogFile("Creating Directory Presentations");
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations");
                    ClsException.WriteToLogFile("Directory Presentations created successfully");
                }

                apppath = AppDomain.CurrentDomain.BaseDirectory + "\\Presentations";


                if (!Directory.Exists(apppath + "\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName))
                {
                    Directory.CreateDirectory(apppath + "\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                }
                pptpath = apppath + "\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                ClsException.WriteToLogFile("Going to start Loop");
                for (int i = 0; i < Directory.GetDirectories(pptpath).Length; i++)
                {
                    DirectoryInfo di = new DirectoryInfo(Directory.GetDirectories(pptpath)[i]);
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = di.Name.Replace(".JPG", "");

                    ContextMenu cntMenuCollMod = new ContextMenu();
                    System.Windows.Controls.MenuItem mnu;
                    mnu = new System.Windows.Controls.MenuItem();
                    mnu.Header = "Remove";
                    mnu.Click += new RoutedEventHandler(mnu_Click);
                    cntMenuCollMod.Items.Add(mnu);
                    lbi.ContextMenu = cntMenuCollMod;

                    listBox1.Items.Add(lbi);
                }
                ClsException.WriteToLogFile("Loop ended successfully");

                //objDelSetSlide = new DelSetSlide(delSetSlide);
                //objDelSetSlideList = new DelSetSlideList(delSetSlideList);
                //objDelGetMsg = new DelGetMessage(delGetMessage);
                //objDelDisplayName = new DelDisplayName(DisplayName);
                //objDelSignOutMsg = new DelSignOutMessage(delSignoutMessage);
                //objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);


                btnBrowse.Click += new RoutedEventHandler(btnBrowse_Click);
                btnUpload.Click += new RoutedEventHandler(btnUpload_Click);
                listBox1.SelectionChanged += new SelectionChangedEventHandler(listBox1_SelectionChanged);
                listBox2.SelectionChanged += new SelectionChangedEventHandler(listBox2_SelectionChanged);

                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(UserControl1_VMuktiEvent);

                System.Windows.Application.Current.Exit += new ExitEventHandler(Current_Exit);

                objListBoxItem = new DelListBoxItem(AddListBoxItem);

                if (Role != "Host")
                {
                    //listBox1.Visibility = Visibility.Collapsed;
                    btnUpload.Visibility = Visibility.Collapsed;
                    btnBrowse.Visibility = Visibility.Collapsed;
                    tbUpload.Visibility = Visibility.Collapsed;
                    listBox1.IsEnabled = false;
                    listBox2.IsEnabled = false;
                    label1.Visibility = Visibility.Collapsed;
                    listBox2.SetValue(Canvas.TopProperty, 5.0);
                    listBox1.Items.Clear();
                }
                //modPer = MyPermissions;

                bgHostService.DoWork += new DoWorkEventHandler(bgHostService_DoWork);

               // tHostPresentation = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostpresentationservice));
                List<object> lstParams = new List<object>();
                lstParams.Add(PeerType);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);

                bgHostService.RunWorkerAsync(lstParams);

                //tHostPresentation.Start(lstParams);
                strRole = Role;

                ClsException.WriteToLogFile("Presentation module:");
                ClsException.WriteToLogFile("loading Presentation module");

                this.Loaded += new RoutedEventHandler(UserControl1_Loaded);
                image1.PreviewMouseDown += new MouseButtonEventHandler(image1_PreviewMouseDown);
                txtkeys.PreviewKeyDown += new KeyEventHandler(txtkeys_PreviewKeyDown);

                sbtemp = (Storyboard)this.FindResource("sbEsc");
                ClsException.WriteToLogFile("Consturctor loaded successfully");

            }
            catch (System.Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "UserControl1()", "UserControl1.xaml.cs");
            }
        }

        #endregion

        #region Global Variable Initialization

        void GlobalVariable()
        {
            try
            {
                arr = new byte[16000];
                lstName = new List<string>();
                disposed = false;

                objDelSetSlide = new DelSetSlide(delSetSlide);
                objDelSetSlideList = new DelSetSlideList(delSetSlideList);
                objDelGetMsg = new DelGetMessage(delGetMessage);
                objDelDisplayName = new DelDisplayName(DisplayName);
                objDelSignOutMsg = new DelSignOutMessage(delSignoutMessage);
                objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);
            }
            catch(System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GlobalVariable()", "UserControl1.xaml.cs");
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
                    NetPeerClient npcPresentation = new NetPeerClient();
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcJoin += new clsNetTcpPresentation.delsvcJoin(UserControl1_EntsvcJoin);
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetSlide += new clsNetTcpPresentation.delsvcSetSlide(UserControl1_EntsvcSetSlide);
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetSlideList += new clsNetTcpPresentation.delsvcSetSlideList(UserControl1_EntsvcSetSlideList);
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcGetUserList += new clsNetTcpPresentation.delsvcGetUserList(UserControl1_EntsvcGetUserList);
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetUserList += new clsNetTcpPresentation.delsvcSetUserList(UserControl1_EntsvcSetUserList);
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSignOutPPT += new clsNetTcpPresentation.delsvcSignOutPPT(UserControl1_EntsvcSignOutPPT);
                    ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcUnJoin += new clsNetTcpPresentation.delsvcUnJoin(UserControl1_EntsvcUnJoin);

                    channelNetTcp = (INetTcpPresentationChannel)npcPresentation.OpenClient<INetTcpPresentationChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpPresentation);

                    while (temp < 20)
                    {
                        try
                        {
                            clsMessageContract objContract = new clsMessageContract();
                            objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContract.strMsg = "";
                            objContract.lstTo = new List<string>();
                            objContract.slideArr = new string[0];
                            objContract.SlideID = 0;
                            objContract.SlideStream = new MemoryStream();
                            channelNetTcp.svcJoin(objContract);

                            temp = 20;

                            clsMessageContract objContract1 = new clsMessageContract();
                            objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContract1.strMsg = "get";
                            objContract1.lstTo = new List<string>();
                            objContract1.slideArr = new string[0];
                            objContract1.SlideID = 0;
                            objContract1.SlideStream = new MemoryStream();
                            channelNetTcp.svcGetUserList(objContract1);
                        }
                        catch
                        {
                            temp++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    ClsException.WriteToLogFile("Presentation module:");
                    ClsException.WriteToLogFile("Opened presenation P2P Client");
                    ClsException.WriteToLogFile("Uri for opening the client is : " + strUri);
                    ClsException.WriteToLogFile("MeshId for opening the client is : " + strUri.ToString().Split(':')[2].Split('/')[1]);
                }
                else
                {
                    BasicHttpClient bhcPresentation = new BasicHttpClient();
                    channelHttp = (IHttpPresentation)bhcPresentation.OpenClient<IHttpPresentation>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            clsMessageContract objContract = new clsMessageContract();
                            objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContract.strMsg = "";
                            objContract.lstTo = new List<string>();
                            objContract.slideArr = new string[0];
                            objContract.SlideID = 0;
                            objContract.SlideStream = new MemoryStream();
                            channelHttp.svcJoin(objContract);

                            tempcounter = 20;

                            clsMessageContract objContract1 = new clsMessageContract();
                            objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContract1.strMsg = "get";
                            objContract1.lstTo = new List<string>();
                            objContract1.slideArr = new string[0];
                            objContract1.SlideID = 0;
                            objContract1.SlideStream = new MemoryStream();
                            channelHttp.svcGetUserList(objContract1);
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    dispTimerControl.Interval = TimeSpan.FromSeconds(2);
                    dispTimerControl.Tick += new EventHandler(dispTimerControl_Tick);
                    dispTimerControl.Start();

                    ClsException.WriteToLogFile("Presentation module:");
                    ClsException.WriteToLogFile("Opened Http Presentation Client with Timers");
                    ClsException.WriteToLogFile("Uri for opening the client is : " + strUri);
                    ClsException.WriteToLogFile("MeshId for opening the client is : " + strUri.ToString().Split(':')[2].Split('/')[1]);
                }


            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "bgHostService_DoWork()", "UserControl1.xaml.cs");
            }
        }

        #endregion

        #region Resize

        void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)) != null)
                {
                    ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(UserControl1_SizeChanged);
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (System.Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "UserControl1_Loaded()", "UserControl1.xaml.cs");
            }

        }

        void UserControl1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)(this.Parent)).ActualWidth;
            }
            catch (System.Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "UserControl1_SizeChanged()", "UserControl1.xaml.cs");
            }
        }

        #endregion

        #region UI Event Handlers

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null && channelNetTcp.State == CommunicationState.Opened)
                {
                    channelNetTcp = null;
                }
                if (dispTimerControl != null)
                {
                    dispTimerControl.Stop();
                }
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SignOut");
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "UserControl1.xaml.cs");
            }
        }

        void UserControl1_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_VMuktiEvent()", "UserControl1.xaml.cs");
            }
        }

        void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int i = 0;
                if (listBox2.SelectedItem != null && strRole == "Host")
                {

                    Bitmap BM = new Bitmap(pptpath + "\\" + ((ListBoxItem)listBox2.SelectedItem).Tag + "\\" + ((ListBoxItem)listBox2.SelectedItem).Content.ToString() + ".JPG");
                    MemoryStream mms = new MemoryStream();
                    BM.Save(mms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    mms.Position = 0;

                    byte[] byteTemp = fncStreamToByteArry(mms);
                    MemoryStream mmsImage = new MemoryStream(byteTemp);
                    mmsImage.Position = 0;

                    clsMessageContract objContract = new clsMessageContract();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strMsg = "";
                    objContract.SlideID = 0;
                    objContract.lstTo = new List<string>();
                    objContract.slideArr = new string[0];
                    mms.Position = 0;
                    objContract.SlideStream = mmsImage;
                        
                    if (channelNetTcp != null)
                    {                        
                        channelNetTcp.svcSetSlide(objContract);
                    }
                            if (channelHttp != null)
                            {
                        channelHttp.svcSetSlide(objContract);
                    }
                    ClsException.WriteToLogFile("Presenatation Module: user click slide selection");
                    ClsException.WriteToLogFile("PPT Name :" + ((ListBoxItem)listBox2.SelectedItem).Tag.ToString());
                    ClsException.WriteToLogFile("slide Name:" + ((ListBoxItem)listBox2.SelectedItem).Content.ToString());
                    ClsException.WriteToLogFile("Receiver's Name : ");
                    for (i = 0; i < lstName.Count; i++)
                    {
                        ClsException.WriteToLogFile(lstName[i]);
                    }
                    mms.Position = 0;

                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = mms;
                    bmi.EndInit();

                    image1.Source = bmi;                    
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "listBox2_SelectionChanged()", "UserControl1.xaml.cs");
            }
        }

        void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null && strRole == "Host")
                {
                    string[] slidearr = Directory.GetFiles(pptpath + "\\" + ((ListBoxItem)listBox1.SelectedItem).Content.ToString(), "*.JPG");
                    string[] newslidearr = new string[slidearr.Length+1];
                    string[] newarr = null;
                    listBox2.Items.Clear();
                    image1.Source = null;

                    for (int i = 0; i < slidearr.Length; i++)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        newarr = slidearr[i].Split('\\');
                        lbi.Content = newarr[newarr.Length - 1].Replace(".JPG", "").Trim();//presentation name
                        newslidearr[i] = lbi.Content.ToString();
                        lbi.Tag = newarr[newarr.Length - 2];
                        listBox2.Items.Add(lbi);
                    }
                    
                    int count = newslidearr.Length;
                    string str1 = null;
                    string str2 = null;
                    str1 = listBox1.SelectedItem.ToString();
                    string[] strarray = str1.Split(':');
                    str2 = strarray[1].Trim();
                    newslidearr[count - 1] = str2;

                    clsMessageContract objContract = new clsMessageContract();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.slideArr = newslidearr;
                    objContract.lstTo = lstName;
                    objContract.SlideID = 0;
                    objContract.SlideStream = new MemoryStream();
                    objContract.strMsg = "SET SLIDE LIST";

                    if (channelHttp != null)
                    {
                        channelHttp.svcSetSlideList(objContract);
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcSetSlideList(objContract);
                    }

                    ClsException.WriteToLogFile("Presentation module:");
                    ClsException.WriteToLogFile("user click presenation selection");
                    ClsException.WriteToLogFile("presentation name :" + newarr[newarr.Length - 1].Replace(".JPG", "").Trim());
                    ClsException.WriteToLogFile("Receiver's Name : ");

                    for (int i = 0; i < lstName.Count; i++)
                    {
                        ClsException.WriteToLogFile(lstName[i]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "listBox1_SelectionChanged()", "UserControl1.xaml.cs");
            }
        }

        void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo diPPT = null;
            string str1 = null;
            try
            {
               // if (tbUpload.Text != "")
                if(!string.IsNullOrEmpty(tbUpload.Text))
                {
                    if (File.Exists(tbUpload.Text))
                    {
                        string[] strarray = tbUpload.Text.Split('\\');

                        if(strarray[strarray.Length-1].Contains("pptx"))
                        {
                            str1 = strarray[strarray.Length - 1].Replace(".pptx", "").Trim();
                        }
                        else
                        {
                        str1 = strarray[strarray.Length - 1].Replace(".ppt", "").Trim();
                        }


                        if (!str1.Contains("."))
                        {
                            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "\\" + str1))
                            {
                                Microsoft.Office.Interop.PowerPoint.Application oPPT;
                                Microsoft.Office.Interop.PowerPoint.Presentations objPresSet;
                                Microsoft.Office.Interop.PowerPoint.Presentation objPres;

                                oPPT = new Microsoft.Office.Interop.PowerPoint.ApplicationClass();                                

                                objPresSet = oPPT.Presentations;

                                diPPT = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                File.Copy(tbUpload.Text, diPPT.FullName + "\\" + str1 + ".ppt");

                                objPres = objPresSet.Open(diPPT.FullName + "\\" + str1 + ".ppt", MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoFalse);
                                objPres.SaveAs(diPPT.FullName + "\\" + str1, PpSaveAsFileType.ppSaveAsJPG, MsoTriState.msoFalse);
                                File.Move(diPPT.FullName + "\\" + str1 + ".ppt", diPPT.FullName + "\\" + str1 + "\\" + str1 + ".ppt");

                                string[] strJPG = Directory.GetFiles(diPPT.FullName + "\\" + str1, "*.JPG");

                                for (int k = 0; k < strJPG.Length; k++)
                                {

                                    FileInfo f = new FileInfo(strJPG[k]);
                                    string strName = "";
                                    string newstr1 = "";
                                    if (f.Name.Split('.')[0].Length < 7)
                                    {
                                        strName = f.Name.Replace("Slide", "Slide0");
                                        newstr1 = strJPG[k].Replace(f.Name, "");
                                        File.Move(strJPG[k], newstr1 + strName);
                                    }                                  
                                }

                                ListBoxItem lbi = new ListBoxItem();

                                ContextMenu cntMenuCollMod = new ContextMenu();

                                System.Windows.Controls.MenuItem mnu;
                                mnu = new System.Windows.Controls.MenuItem();
                                mnu.Header = "Remove";
                                mnu.Click += new RoutedEventHandler(mnu_Click);
                                cntMenuCollMod.Items.Add(mnu);
                                
                                lbi.Content = str1;
                                lbi.ContextMenu = cntMenuCollMod;
                                
                                listBox1.Items.Add(lbi);
                                ClsException.WriteToLogFile("Presentation module:");
                                ClsException.WriteToLogFile("user click pptupload button");
                                ClsException.WriteToLogFile("ppt name :" + str1);
                                MessageBox.Show("File Uploaded");
                                tbUpload.Text = "";

                            }
                            else
                            {
                                MessageBox.Show("File with same name already exists!!");
                                tbUpload.Text = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show(@"FileName contains ""."" Rename the File ", "Rename File", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                            tbUpload.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("File Does Not Exists!", "NonExisting File", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                        tbUpload.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Select File to Upload!", "File Upload", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    tbUpload.Text = "";
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnUpload_Click()/Microsoft Office", "UserControl1.xaml.cs");

                //try
                //{

                //    XComponentContext localContext = Bootstrap.bootstrap();

                //    XMultiServiceFactory multiServiceFactory = (XMultiServiceFactory)localContext.getServiceManager();

                //    XComponentLoader componentLoader = (XComponentLoader)multiServiceFactory.createInstance("com.sun.star.frame.Desktop");

                //    string[] strTemp = tbUpload.Text.Split('\\');

                //    diPPT = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                //    File.Copy(tbUpload.Text, diPPT.FullName + "\\" + str1 + ".ppt");

                //    DirectoryInfo diPPTNew = Directory.CreateDirectory(diPPT.FullName + "\\" + strTemp[strTemp.Length - 1].Split('.')[0]);
                //    File.Move(diPPT.FullName + "\\" + strTemp[strTemp.Length - 1], diPPTNew.FullName + "\\" + strTemp[strTemp.Length - 1]);


                //    string strpath = diPPTNew.FullName + "\\" + strTemp[strTemp.Length - 1];
                //    string newstr = strpath.Replace('\\', '/');
                //    XComponent xComponent = componentLoader.loadComponentFromURL("file:///" + newstr, "_blank", 0, new unoidl.com.sun.star.beans.PropertyValue[1] { MakePropertyValue("Hidden", new uno.Any(true)) });

                //    int newslide = ((unoidl.com.sun.star.drawing.XDrawPagesSupplier)xComponent).getDrawPages().getCount();

                //    int nHighestPageNumber = newslide - 1;

                //    DeleteAllPagesExcept(xComponent, 0);
                //    string path = diPPTNew.FullName.Replace('\\', '/');
                //    string cNewName = "file:///" + newstr;

                //    ((XStorable)xComponent).storeToURL(cNewName + ".html", new unoidl.com.sun.star.beans.PropertyValue[1] { MakePropertyValue("FilterName", new uno.Any("impress_html_Export")) });

                //    ((XCloseable)xComponent).close(true);

                //    xComponent.dispose();


                //    string[] strFiles = Directory.GetFiles(diPPTNew.FullName, "*.html");
                //    for (int j = 0; j < strFiles.Length; j++)
                //    {
                //        File.Delete(strFiles[j]);
                //    }

                //    string[] strJPG = Directory.GetFiles(diPPTNew.FullName, "*.JPG");

                //    for (int k = 0; k < strJPG.Length; k++)
                //    {

                //        FileInfo f = new FileInfo(strJPG[k]);
                //        string strName = "";
                //        string newstr1 = "";
                //        if (f.Name.Split('.')[0].Length < 5)
                //        {
                //            strName = f.Name.Replace("img", "Slide0");
                //            newstr1 = strJPG[k].Replace(f.Name, "");
                //            File.Move(strJPG[k], newstr1 + strName);
                //        }
                //        else
                //        {
                //            strName = f.Name.Replace("img", "Slide");
                //            newstr1 = strJPG[k].Replace(f.Name, "");
                //            File.Move(strJPG[k], newstr1 + strName);
                //        }
                //    }
                //    ListBoxItem lbi = new ListBoxItem();

                //    ContextMenu cntMenuCollMod = new ContextMenu();

                //    System.Windows.Controls.MenuItem mnu;
                //    mnu = new System.Windows.Controls.MenuItem();
                //    mnu.Header = "Remove";
                //    mnu.Click += new RoutedEventHandler(mnu_Click);
                //    cntMenuCollMod.Items.Add(mnu);

                //    //lbi.ContextMenu = mnu;

                //    lbi.Content = str1;
                //    lbi.ContextMenu = cntMenuCollMod;
                    
                //    listBox1.Items.Add(lbi);

                //    MessageBox.Show("File Uploaded");
                //    tbUpload.Text = "";
                //}
                //catch (System.Exception exp)
                //{
                //    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnUpload_Click()/Open Office", "UserControl1.xaml.cs");
                //    MessageBox.Show("You need Open Office or Microsoft Office to upload the ppt, MS office 2007 for pptx. Make sure PPT is not open or corrupted.", "Upload PPT", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                //}
            }
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (image1.Source != null)
                {
                    cd1.Width = new GridLength(0, GridUnitType.Pixel);
                    cd1.MinWidth = 0;
                    rd2.Height = new GridLength(0, GridUnitType.Pixel);
                    brdEsc.Visibility = Visibility.Visible;
                    sbtemp.Begin(brdEsc);
                    txtkeys.Focus();
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnFullScreen_Click()", "UserControl1.xaml.cs");
            }
        }

        void txtkeys_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {                
                if (e.Key == Key.Right || e.Key == Key.Down)
                {
                    if (listBox2.SelectedIndex == listBox2.Items.Count - 1)
                    {
                        listBox2.SelectedIndex = listBox2.Items.Count - 1;
                    }
                    else
                    {
                        listBox2.SelectedIndex += 1;
                    }
                }
                if (e.Key == Key.Left || e.Key == Key.Up)
                {
                    if (listBox2.SelectedIndex == 0)
                    {
                        listBox2.Items.Refresh();
                        listBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        listBox2.SelectedIndex -= 1;

                    }
                }
                if (e.Key == Key.Escape)
                {
                    cd1.Width = new GridLength(0.9, GridUnitType.Star);
                    cd1.MinWidth = 80;
                    rd2.Height = new GridLength(1, GridUnitType.Star);
                }
            }
            catch (System.Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "txtkeys_PreviewKeyDown()", "UserControl1.xaml.cs");
            }

        }

        void image1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtkeys.Focus();
        }       

        void mnu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str1=null;
                string str2 = null;
                str1 = listBox1.SelectedItem.ToString();
                string[] strarray = str1.Split(':');                
                str2 = strarray[1].Trim();

                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "\\" + str2))
                {
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\Presentations\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "\\" + str2, true);
                 
                    listBox1.Items.Remove(listBox1.SelectedItem);
                    listBox2.Items.Clear();
                    image1.Source = new BitmapImage();
                }
            }
            catch(System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "mnu_Click()", "UserControl1.xaml.cs");
            }
        }

        void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();                
                ofd.Filter = "Presentation Files | *.ppt;*.pptx;";
                
                if (ofd.ShowDialog().Value)
                {
                    tbUpload.Text = ofd.FileName;

                    ClsException.WriteToLogFile("Presentation module:");
                    ClsException.WriteToLogFile("user click brouse button");
                    ClsException.WriteToLogFile("fullpath :" + tbUpload.Text);                    
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnBrowse_Click()", "UserControl1.xaml.cs");
            }
        }

        #endregion

        #region Timers and its Related Methods

        void dispTimerControl_Tick(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();

                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispTimerControl_Tick()", "UserControl1.xaml.cs");
            }
        }

        void StartThread()
        {
            try
            {
                if (channelHttp != null)
                {
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.lstTo = new List<string>();
                    objContract.slideArr = new string[0];
                    objContract.SlideID = 0;
                    objContract.SlideStream = new MemoryStream();
                    objContract.strMsg = "";

                    channelHttp.BeginsvcGetSlide(objContract, OnCompletion, null);
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartThread()", "UserControl1.xaml.cs");
            }
        }

        void OnCompletion(IAsyncResult result)
        {
            try
            {
                clsMessageContract objMessage = new clsMessageContract();
                objMessage = channelHttp.EndsvcGetSlide(result);
                result.AsyncWaitHandle.Close();

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetMessage, objMessage);
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnCompletion()", "UserControl1.xaml.cs");
            }

        }

        public void hostpresentationservice(object lstParams)
        {
            //List<object> lstTempObj = (List<object>)lstParams;
            //strUri = lstTempObj[1].ToString();
            //try
            //{
            //    if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
            //    {
            //        NetPeerClient npcPresentation = new NetPeerClient();
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcJoin += new clsNetTcpPresentation.delsvcJoin(UserControl1_EntsvcJoin);
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetSlide += new clsNetTcpPresentation.delsvcSetSlide(UserControl1_EntsvcSetSlide);
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetSlideList += new clsNetTcpPresentation.delsvcSetSlideList(UserControl1_EntsvcSetSlideList);
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcGetUserList += new clsNetTcpPresentation.delsvcGetUserList(UserControl1_EntsvcGetUserList);
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetUserList += new clsNetTcpPresentation.delsvcSetUserList(UserControl1_EntsvcSetUserList);
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSignOutPPT += new clsNetTcpPresentation.delsvcSignOutPPT(UserControl1_EntsvcSignOutPPT);
            //        ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcUnJoin += new clsNetTcpPresentation.delsvcUnJoin(UserControl1_EntsvcUnJoin);

            //        channelNetTcp = (INetTcpPresentationChannel)npcPresentation.OpenClient<INetTcpPresentationChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[1], ref objNetTcpPresentation);

            //        while (temp < 20)
            //        {
            //            try
            //            {                           
            //                clsMessageContract objContract = new clsMessageContract();
            //                objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
            //                objContract.strMsg = "";
            //                objContract.lstTo = new List<string>();
            //                objContract.slideArr = new string[0];
            //                objContract.SlideID = 0;
            //                objContract.SlideStream = new MemoryStream();
            //                channelNetTcp.svcJoin(objContract);

            //                temp = 20;

            //                clsMessageContract objContract1 = new clsMessageContract();
            //                objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
            //                objContract1.strMsg = "get";
            //                objContract1.lstTo = new List<string>();
            //                objContract1.slideArr = new string[0];
            //                objContract1.SlideID = 0;
            //                objContract1.SlideStream = new MemoryStream();
            //                channelNetTcp.svcGetUserList(objContract1);
            //            }
            //            catch
            //            {
            //                temp++;
            //                System.Threading.Thread.Sleep(1000);
            //            }
            //        }

            //        ClsException.WriteToLogFile("Presentation module:");
            //        ClsException.WriteToLogFile("Opened presenation P2P Client");
            //        ClsException.WriteToLogFile("Uri for opening the client is : " + strUri);
            //        ClsException.WriteToLogFile("MeshId for opening the client is : " + strUri.ToString().Split(':')[2].Split('/')[1]);
            //    }
            //    else
            //    {
            //        BasicHttpClient bhcPresentation = new BasicHttpClient();
            //        channelHttp = (IHttpPresentation)bhcPresentation.OpenClient<IHttpPresentation>(strUri);

            //        while (tempcounter < 20)
            //        {
            //            try
            //            {                          
            //                clsMessageContract objContract = new clsMessageContract();
            //                objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
            //                objContract.strMsg = "";
            //                objContract.lstTo = new List<string>();
            //                objContract.slideArr=new string[0];
            //                objContract.SlideID = 0;
            //                objContract.SlideStream = new MemoryStream();
            //                channelHttp.svcJoin(objContract);
                            
            //                tempcounter = 20;

            //                clsMessageContract objContract1 = new clsMessageContract();
            //                objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
            //                objContract1.strMsg = "get";
            //                objContract1.lstTo = new List<string>();
            //                objContract1.slideArr = new string[0];
            //                objContract1.SlideID = 0;
            //                objContract1.SlideStream = new MemoryStream();
            //                channelHttp.svcGetUserList(objContract1);
            //            }
            //            catch
            //            {
            //                tempcounter++;
            //                System.Threading.Thread.Sleep(1000);
            //            }
            //        }

            //        dispTimerControl.Interval = TimeSpan.FromSeconds(2);
            //        dispTimerControl.Tick += new EventHandler(dispTimerControl_Tick);
            //        dispTimerControl.Start();

            //        ClsException.WriteToLogFile("Presentation module:");
            //        ClsException.WriteToLogFile("Opened Http Presentation Client with Timers");
            //        ClsException.WriteToLogFile("Uri for opening the client is : " + strUri);
            //        ClsException.WriteToLogFile("MeshId for opening the client is : " + strUri.ToString().Split(':')[2].Split('/')[1]);
            //    }


            //}
            //catch (System.Exception ex)
            //{
            //    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostpresentationservice()", "UserControl1.xaml.cs");
            //}
        }

        #endregion

        #region WCF NetP2P Event Handlers

        void UserControl1_EntsvcSetSlideList(clsMessageContract mcSetSlideList)
        {
            try
            {
                if (mcSetSlideList.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstStr = new List<object>();
                    lstStr.Add(mcSetSlideList.strFrom);
                    lstStr.Add(mcSetSlideList.slideArr);
                    lstStr.Add(mcSetSlideList.lstTo);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSetSlideList, lstStr);
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcSetSlideList()", "UserControl1.xaml.cs");
            }
        }

        void UserControl1_EntsvcSignOutPPT(clsMessageContract mcSignOutPPT)
        {
            try
            {
                if (mcSignOutPPT.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstStr = new List<object>();
                    lstStr.Add(mcSignOutPPT.strFrom);
                    lstStr.Add(mcSignOutPPT.lstTo);
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcSignOutPPT()", "UserControl1.xaml.cs");
            }
        }
                      
        void UserControl1_EntsvcSetSlide(clsMessageContract mcSetSlide)
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSetSlide, mcSetSlide);
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcSetSlide()", "UserControl1.xaml.cs");
            }
        }

        void UserControl1_EntsvcJoin(clsMessageContract mcJoin)
        {           
        }

        void UserControl1_EntsvcGetUserList(clsMessageContract mcGetUserList)
        {
            try
            {
                if (mcGetUserList.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, mcGetUserList.strFrom);

                    clsMessageContract objContract = new clsMessageContract();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strMsg = "set";
                    objContract.lstTo = new List<string>();
                    objContract.slideArr = new string[0];
                    objContract.SlideID = 0;
                    objContract.SlideStream = new MemoryStream();

                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcSetUserList(objContract);
                    }
                    if (channelHttp != null)
                    {
                        channelHttp.svcSetUserList(objContract);
                    }
                }

            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcGetUserList()", "UserControl1.xaml.cs");
            }
        }

        void UserControl1_EntsvcSetUserList(clsMessageContract mcSetUserList)
        {
            try
            {
                if (mcSetUserList.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    try
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, mcSetUserList.strFrom);
                    }
                    catch (System.Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcSetUserList()", "UserControl1.xaml.cs");
                    }
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcSetUserList()", "UserControl1.xaml.cs");
            }
        }

        

        void UserControl1_EntsvcUnJoin(clsMessageContract mcUnJoin)
        {
            try
            {
                if (mcUnJoin.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstData = new List<object>();
                    lstData.Add(mcUnJoin.strFrom);

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl1_EntsvcUnJoin()", "UserControl1.xaml.cs");
            }
        }
        
        #endregion

        #region Delegates Event Handlers

        private void delSetSlide(clsMessageContract mcSetSlide)
        {
            try
            {
                if (mcSetSlide.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {                    
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = mcSetSlide.SlideStream;
                    bmi.EndInit();

                    image1.Source = bmi;
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delSetSlide()", "UserControl1.xaml.cs");
            }
        }

        void delSetSlideList(List<object> lstStr)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            image1.Source = null;
            try
            {
                string[] tempArray = (string[])lstStr[1];
                
                for (int i = 0; i < tempArray.Length-1; i++)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    string[] newarr = tempArray[i].Split('\\');
                    lbi.Content = newarr[newarr.Length - 1].Replace(".JPG", "").Trim();
                    //lbi.Tag = newarr[newarr.Length - 2];
                    listBox2.Items.Add(lbi);
                }
                
                ListBoxItem lbl1 = new ListBoxItem();
                lbl1.Content = tempArray[tempArray.Length - 1];
                listBox1.Items.Add(lbl1);
                listBox1.SelectedIndex = 0;                
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delSetSlideList()", "UserControl1.xaml.cs");
            }
        }

        void delSignoutMessage(List<object> lstStr)
        {
            try
            {
                lstName.Remove(lstStr[0].ToString());
                ClsException.WriteToLogFile("Presentation module:");
                ClsException.WriteToLogFile("Buddy Leaving presentation");
                ClsException.WriteToLogFile("Sender is (Buddy Leaving presentation): " + lstStr[0].ToString());
                ClsException.WriteToLogFile("Time is : " + System.DateTime.Now.ToString());
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delSignoutMessage()", "UserControl1.xaml.cs");
            }
        }

        public void DisplayName(string lstUserName)//same as chat
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
                }


                ClsException.WriteToLogFile("Presentation module:");
                ClsException.WriteToLogFile("Buddy To Be Added In the Buddylist");
                ClsException.WriteToLogFile("Receiver's Name : ");
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DisplayName()", "UserControl1.xaml.cs");
            }
        }

        private void AddListBoxItem(string[] itemName)
        {
            try
            {
                listBox1.Items.Clear();
                for (int i = 0; i < itemName.Length; i++)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    string pName = itemName[i].Split('\\')[itemName[i].Split('\\').Length - 1];
                    lbi.Content = pName;
                    listBox1.Items.Add(lbi);
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddListBoxItem()", "UserControl1.xaml.cs");
            }
        }

        void delGetMessage()
        {
            try
            {
                try
                {
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strMsg = "";
                    objContract.lstTo = new List<string>();
                    objContract.slideArr = new string[0];
                    objContract.SlideID = 0;
                    objContract.SlideStream = new MemoryStream();
                    
                    clsMessageContract myMessages = channelHttp.svcGetSlide(objContract);

                if (myMessages != null)
                {
                        if (myMessages.strMsg == "SET SLIDE LIST")
                        {
                            listBox2.Items.Clear();
                            string[] tempArray = myMessages.slideArr;
                            for (int j = 0; j < tempArray.Length; j++)
                            {
                                ListBoxItem lbi = new ListBoxItem();
                                string[] newarr = tempArray[j].Split('\\');
                                lbi.Content = newarr[newarr.Length - 1].Replace(".JPG", "").Trim();
                                listBox2.Items.Add(lbi);
                            }
                        }
                        else if (myMessages.strMsg == "SHOW SLIDE")
                        {
                            BitmapImage bmi = new BitmapImage();
                            bmi.BeginInit();
                            bmi.StreamSource = myMessages.SlideStream;
                            bmi.EndInit();

                            image1.Source = bmi;
                        }

                        else if (myMessages.strMsg == "GetUserList")
                        {
                            try
                            {
                                if (myMessages.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages.strFrom);

                                    if (channelHttp != null)
                                    {
                                        clsMessageContract objContract1 = new clsMessageContract();
                                        objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                                        objContract1.strMsg = "";
                                        objContract1.lstTo = new List<string>();
                                        objContract1.slideArr = new string[0];
                                        objContract1.SlideID = 0;
                                        objContract1.SlideStream = new MemoryStream();
                                        channelHttp.svcSetUserList(objContract1);
                                    }
                                }
                            }
                            catch (System.Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delGetMessage()", "UserControl1.xaml.cs");
                            }
                        }
                        else if (myMessages.strMsg == "SetUserList")
                        {
                            if (myMessages.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages.strFrom);
                            }
                        }
                        else if (myMessages.strFrom == "SignOut")
                        {
                            if (myMessages.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                List<object> lstStr = new List<object>();
                                lstStr.Add(myMessages.strFrom);
                                lstStr.Add(myMessages.lstTo);
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                            }
                        }
                        //     }
                    }
                }
                catch (System.Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delGetMessage()", "UserControl1.xaml.cs");
                }


            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delGetMessage()", "UserControl1.xaml.cs");
            }
            finally
            {
                this.dispTimerControl.Start();
            }
        }          
        

        void delAsyncGetMessage(clsMessageContract myMessages)
        {
            try
            {
                        if (myMessages.strMsg == "SET SLIDE LIST")
                        {
                            listBox1.Items.Clear();
                            listBox2.Items.Clear();
                            image1.Source = null;
                            string[] tempArray = myMessages.slideArr;
                            for (int j = 0; j < tempArray.Length-1; j++)
                            {
                                ListBoxItem lbi = new ListBoxItem();
                                string[] newarr = tempArray[j].Split('\\');
                                lbi.Content = newarr[newarr.Length - 1].Replace(".JPG", "").Trim();
                                listBox2.Items.Add(lbi);
                            }
                            ListBoxItem lbl1 = new ListBoxItem();
                            lbl1.Content = tempArray[tempArray.Length - 1];
                            listBox1.Items.Add(lbl1);
                            listBox1.SelectedIndex = 0;
                        }
                        else if (myMessages.strMsg == "SHOW SLIDE")
                        {
                            BitmapImage bmi = new BitmapImage();
                            bmi.BeginInit();
                            bmi.StreamSource = myMessages.SlideStream;
                            bmi.EndInit();

                            image1.Source = bmi;
                        }

                        else if (myMessages.strMsg == "GetUserList")
                        {
                            try
                            {
                                if (myMessages.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages.strFrom);

                                    if (channelNetTcp != null)
                                    {
                                        clsMessageContract objContract1 = new clsMessageContract();
                                        objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                                        objContract1.strMsg = "";
                                        objContract1.lstTo = new List<string>();
                                        objContract1.slideArr = new string[0];
                                        objContract1.SlideID = 0;
                                        objContract1.SlideStream = new MemoryStream();

                                        channelNetTcp.svcSetUserList(objContract1);
                                    }

                                    if(channelHttp != null)
                                    {
                                        clsMessageContract objContract1 = new clsMessageContract();
                                        objContract1.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                                        objContract1.strMsg = "";
                                        objContract1.lstTo = new List<string>();
                                        objContract1.slideArr = new string[0];
                                        objContract1.SlideID = 0;
                                        objContract1.SlideStream = new MemoryStream();

                                        channelHttp.svcSetUserList(objContract1);
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()", "UserControl1.xaml.cs");
                            }
                        }
                        else if (myMessages.strMsg == "SetUserList")
                        {
                            if (myMessages.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages.strFrom);
                            }
                        }
                        else if (myMessages.strMsg == "SignOut")
                        {
                            if (myMessages.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                List<object> lstStr = new List<object>();
                                lstStr.Add(myMessages.strFrom);
                                lstStr.Add(myMessages.lstTo);
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                            }
                        }
                    }
                
            
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetMessage()", "UserControl1.xaml.cs");
            }
            finally
            {
                this.dispTimerControl.Start();
            }
        }

        #endregion

        #region User Defined Methods

        public void ClosePod()
        {
            try
            {
                clsMessageContract objContract = new clsMessageContract();
                objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                objContract.lstTo = lstName;
                objContract.slideArr = new string[0];
                objContract.SlideID = 0;
                objContract.SlideStream = new MemoryStream();
                objContract.strMsg = "";

                if (channelNetTcp != null)
                {
                    channelNetTcp.svcSignOutPPT(objContract);
                }
                else if (channelHttp != null)
                {
                    channelHttp.svcSignOutPPT(objContract);
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

                if (dispTimerControl != null)
                {
                    dispTimerControl.Stop();
                }
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "UserControl1.xaml.cs");
            }
        }

        private byte[] ConvertStreamToByteBuffer(Stream stream)
        {
            try
            {
                int num;
                MemoryStream tempStream = new MemoryStream();

                while ((num = stream.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)num));
                }

                return tempStream.ToArray();
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ConvertStreamToByteBuffer()", "UserControl1.xaml.cs");
                return null;
            }
        }

        //private void DeleteAllPagesExcept(object oDoc, object nPageToKeep)
        //{
        //    try
        //    {
        //        int nNumPages = ((unoidl.com.sun.star.drawing.XDrawPagesSupplier)oDoc).getDrawPages().getCount();
        //        int nHighestPageNumber = nNumPages - 1;

        //        // Delete the last page, then the page before that,
        //        // then the page before that, until we get to the
        //        // page to keep.
        //        // This deletes all pages AFTER the page to keep.
        //        int nPageToDelete = nHighestPageNumber;
        //        while (nPageToDelete > (int)nPageToKeep)
        //        {
        //            // Get the page.
        //            object oPage = ((unoidl.com.sun.star.drawing.XDrawPagesSupplier)oDoc).getDrawPages().getByIndex(nPageToDelete);

        //            // Tell the document to remove it.              

        //            //((unoidl.com.sun.star.drawing.XDrawPagesSupplier)oDoc).getDrawPages().remove((unoidl.com.sun.star.drawing.XDrawPage)oPage);

        //            nPageToDelete = nPageToDelete - 1;
        //        }

        //        // Delete all the pages before the page to keep.
        //        for (int i = 0; i <= (int)nPageToKeep - 1; i++)
        //        {
        //            // Delete the first page.
        //            nPageToDelete = 0;
        //            // Get the page.
        //            object oPage = ((unoidl.com.sun.star.drawing.XDrawPagesSupplier)oDoc).getDrawPages().getByIndex(nPageToDelete);
        //            // Tell the document to remove it.
        //            //((unoidl.com.sun.star.drawing.XDrawPagesSupplier)oDoc).getDrawPages().remove((unoidl.com.sun.star.drawing.XDrawPage)oPage);

        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DeleteAllPagesExcept()", "UserControl1.xaml.cs");
        //    }
        //}

        //private unoidl.com.sun.star.beans.PropertyValue MakePropertyValue(string p, uno.Any p_2)
        //{
        //    try
        //    {
        //        aArgs = new unoidl.com.sun.star.beans.PropertyValue();
        //        aArgs.Name = p;
        //        aArgs.Value = p_2;
        //        return aArgs;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MakePropertyValue()", "UserControl1.xaml.cs");
        //        return null;
        //    }
        //}

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "UserControl1.xaml.cs");
            }
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
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
                        if (msBuffer != null)
                        {
                            msBuffer = null;
                        }
                        if (dispTimerControl != null)
                        {
                            dispTimerControl = null;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "UserControl1.xaml.cs");
                    }
                }
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~UserControl1()
        {
            try
            {
                Dispose(false);
            }

            catch (System.Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Destructor/UserControl1()", "UserControl1.xaml.cs");
            }
        }
        #endregion
               
        #region supported functions

        Stream fncStringToStream(string strInput)
        {
            try
            {
                int length = strInput.Length;
                byte[] resultBytes = new byte[length];

                for (int i = 0; i < length; i++)
                {
                    resultBytes[i] = (byte)strInput[i];
                }

                Stream mmsConvert = new MemoryStream(resultBytes);
                mmsConvert.Position = 0;
                return mmsConvert;
            }
            catch
            {

                //ClsException.WriteToLogFile("ctlVideo.xaml.cs:-fncStringToStream():-" + ex.Message);
                return null;
            }
        }

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

        string fncStreamToString(Stream streamInput)
        {
            try
            {
                byte[] byteArry = fncStreamToByteArry(streamInput);

                char[] buffer = new char[byteArry.Length];

                for (int j = 0; j < byteArry.Length; j++)
                {
                    buffer[j] = (char)byteArry[j];
                }

                return new string(buffer);
            }
            catch
            {
                //ClsException.WriteToLogFile("ctlVideo.xaml.cs:-fncStreamToString():-" + ex.Message);
                return null;
            }
        }

        #endregion
    }
}