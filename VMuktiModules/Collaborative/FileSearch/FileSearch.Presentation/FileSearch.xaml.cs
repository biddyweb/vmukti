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
using System.Windows.Media;
using System.IO;
using System.Xml;
using System.Diagnostics;
using FileSearch.Business;
using FileSearch.Business.Service.NetP2P;
using VMuktiService;
using FileSearch.Business.Service.BasicHttp;
using FileSearch.Business.Service.DataContracts;
using VMuktiAPI;
using System.Text;

namespace FileSearch.Presentation
{
    /// <summary>
    /// Interaction logic for FileSearch.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class FileSearch : UserControl,IDisposable
    {
      //  public static StringBuilder sb1;
        //public StringBuilder sb1 = CreateTressInfo();
        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}
        byte[] arr = new byte[5000];
        byte[] Larr;
        double smallPart;
        int i;
        int Counter;
        int tempcounter;
        string Rolee = string.Empty;
        string TempRemoteFilePath = string.Empty;
        string strUri = string.Empty;
        string strdtSearchQuery = string.Empty;
        string strdtFromUser = string.Empty;
        string strBrowsingLocation = string.Empty;
        List<string> lstDownloadFileList = new List<string>();
        //Before Performance Rule.
        //private bool disposed = false;
        //After applyin Rule.
        private bool disposed;
        long fileSize;
       
        Business_FileSearch objBusiness_FileSearch;
        FileStream objFileStreamWriter;
        public delegate void DelThread();

        public delegate void DelUIStatus(List<object> lstMessage);
        public delegate void DelUIFileSearchEntsvcSearchResult(List<object> lstItem);
        public delegate void DelDisplayName(string lstUserName);
        public DelUIStatus objDelUIStatus;
        public DelUIFileSearchEntsvcSearchResult objDelUIFileSearchEntsvcSearchResult; 
        public DelDisplayName objDelDisplayName;
        
        object objNetTcpFileSearch = new clsNetTcpFileSearch();
        IFileTransfer NetP2PChannel;

        object objHttpFileSearch = new clsHttpFileSearch();
        IHttpFileSearch HttpChannel;

        System.Threading.Thread ThrHostFileSearch;
        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        System.Windows.Threading.DispatcherTimer dtGetFileList = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        System.Windows.Threading.DispatcherTimer dtDownloadFile = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);

        List<string> lstBuddyList = new List<string>();
        //StringBuilder sb1 = CreateTressInfo();

        public delegate void DelAsyncGetMessage(List<clsMessage> myMessages);
        public DelAsyncGetMessage objDelAsyncGetMessage;

        public delegate void DelAsyncGetMessage4GetFileList(string[] myMessages);
        public DelAsyncGetMessage4GetFileList objDelAsyncGetMessage4GetFileList;


        public FileSearch(VMuktiAPI.PeerType bindingtype, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                InitializeComponent();

                objDelUIStatus = new DelUIStatus(FncDelUIStatus);
                objDelUIFileSearchEntsvcSearchResult = new DelUIFileSearchEntsvcSearchResult(FncDelUIFileSearchEntsvcSearchResult);
                objDelDisplayName = new DelDisplayName(DisplayName);
                DelThread dt = new DelThread(FncMain);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, dt);
                this.listViewResults.MouseDoubleClick += new MouseButtonEventHandler(listViewResults_MouseDoubleClick);

                ThrHostFileSearch = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FncHostFileSearchService));
                List<object> lstParams = new List<object>();
                lstParams.Add(bindingtype);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);
                ThrHostFileSearch.Start(lstParams);
             
                Rolee = Role;
                //lstBuddyList.Add("ADIANCE-222");
                //lstBuddyList.Add("ADIANCE07");
                //cnvMain.AllowDrop = true;
                //cnvMain.PreviewDrop += new DragEventHandler(cnvMain_PreviewDrop);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(FileSearch_VMuktiEvent);

                ClsException.WriteToLogFile("Loading FileSearch Module");

                objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);
                objDelAsyncGetMessage4GetFileList = new DelAsyncGetMessage4GetFileList(delAsyncGetMessage4GetFileList);

                this.Loaded += new RoutedEventHandler(FileSearch_Loaded);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch", "FileSearch.xaml.cs");
            }
        }
        
        #region Resize
        void FileSearch_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)) != null)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                    ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(FileSearch_SizeChanged);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_Loaded", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Width > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_SizeChanged", "FileSearch.xaml.cs");
            }

        }
        #endregion
      

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            XmlTextWriter VxmlWriter = null;
            try
            {
                 listViewResults.Items.Clear();
                
                 textBoxQuery.Clear();
                string AppPath = AppDomain.CurrentDomain.BaseDirectory;
                AppPath = AppPath + "\\FileSearch_Configuration.xml";
                VxmlWriter = new XmlTextWriter(AppPath, null);

                System.Windows.Forms.FolderBrowserDialog folderdialog = new System.Windows.Forms.FolderBrowserDialog();
                folderdialog.SelectedPath = this.textBoxPath.Text;

                if (folderdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBoxPath.Text = folderdialog.SelectedPath;
                    strBrowsingLocation = textBoxPath.Text;
                    //**New browsing Path=strBrowsingLocation
                }

                VxmlWriter.Formatting = Formatting.Indented;
                VxmlWriter.Indentation = 6;
                VxmlWriter.Namespaces = false;

                VxmlWriter.WriteStartDocument();
                VxmlWriter.WriteStartElement("", "Path", "");
                VxmlWriter.WriteString(folderdialog.SelectedPath);
                VxmlWriter.WriteEndElement();
                VxmlWriter.Flush();
                VxmlWriter.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "buttonBrowse_Click", "FileSearch.xaml.cs");
            }
            finally
            {
                if (VxmlWriter != null)
                {
                    VxmlWriter.Close();
                }
            }
        }

        private void buttonIndex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fileSize = 0;
                if (FncGetfoldersize(strBrowsingLocation) <= 15728640)
                {
                enableControls(false);
                objBusiness_FileSearch.FncRebuildIndex(this.textBoxPath.Text);
                 //**Rebuilding Index.
                enableControls(true );
            }
                else
                {
                    MessageBox.Show("Selected Folder size is more than 15MB");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "buttonIndex_Click", "FileSearch.xaml.cs");
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.listViewResults.Items.Clear();
                string[] sTemp = objBusiness_FileSearch.Fncsearch(textBoxQuery.Text);
                //**Searching Files named=textBoxQuery.Text
                enableControls(true);
                for (int i = 0; i < sTemp.Length; i++)
                {
                    sTemp[i] = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "     " + sTemp[i];
                    this.listViewResults.Items.Add(sTemp[i].ToString());
                }

                string[] sGetFileList = Directory.GetFiles(textBoxPath.Text, textBoxQuery.Text + ".*", SearchOption.AllDirectories);
                if (sGetFileList.Length >= 1)
                {
                    for (int j = 0; j < sGetFileList.Length; j++)
                    {
                        sGetFileList[j] = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "     " + sGetFileList[j].Split('\\')[sGetFileList[j].Split('\\').Length - 1] + "     " + sGetFileList[j];
                        this.listViewResults.Items.Add(sGetFileList[j].ToString());
                    }
                }
                //string[] sFinalList = new string[sTemp.Length + sGetFileList.Length];
                //sTemp.CopyTo(sFinalList, 0);
                //sGetFileList.CopyTo(sFinalList, sTemp.Length);
                System.Windows.Forms.Application.DoEvents();

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    if (NetP2PChannel != null)
                    {
                        NetP2PChannel.svcSearchQuery(textBoxQuery.Text, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddyList);
                    }
                }
                else
                {
                    if (HttpChannel != null)
                    {
                        HttpChannel.svcHttpSendQuery(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, textBoxQuery.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "buttonSearch_Click", "FileSearch.xaml.cs");
            }
        }

        private void enableControls(bool enable)
        {
            try
            {
            this.textBoxPath.IsEnabled = enable;
            this.buttonIndex.IsEnabled = enable;
            this.buttonBrowse.IsEnabled = enable;
            //this.buttonClean.IsEnabled = enable;
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "enableControls", "FileSearch.xaml.cs");
            }
        }

        private void FncMain()
        {
            try
            {
            string strSpecialPath = string.Empty;
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            AppPath = AppPath + "\\FileSearch_Configuration.xml";
            if (File.Exists(AppPath))
            {
                XmlTextReader xtr = new XmlTextReader(AppPath);
                while (xtr.Read())
                {
                    if (xtr.NodeType == XmlNodeType.Element)
                    {
                        if (xtr.LocalName.Equals("Path"))
                        {
                            strSpecialPath = xtr.ReadString();
                        }
                    }
                }
                xtr.Close();
            }
            else
            {
                strSpecialPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                strSpecialPath = strSpecialPath + "\\VMukti";
                if (!Directory.Exists(strSpecialPath))
                {
                    Directory.CreateDirectory(strSpecialPath);
                }
            }
            this.textBoxPath.Text = strSpecialPath;
            strBrowsingLocation = strSpecialPath;
            string strTemp = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DesktopSearch");
            objBusiness_FileSearch = new Business_FileSearch(strTemp, strSpecialPath);
            objBusiness_FileSearch.EntStatus += new DelStatus(objBusiness_FileSearch_EntStatus);
            objBusiness_FileSearch.checkIndex();
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncMain", "FileSearch.xaml.cs");
            }
        }

        private void FncHostFileSearchService(object lstParams)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstParams;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcFileSearch = new NetPeerClient();
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcJoin+=new clsNetTcpFileSearch.delsvcJoin(FileSearch_EntsvcJoin);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcRequestFile+=new clsNetTcpFileSearch.delsvcRequestFile(FileSearch_EntsvcRequestFile);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcSearchQuery+=new clsNetTcpFileSearch.delsvcSearchQuery(FileSearch_EntsvcSearchQuery);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcSearchResult+=new clsNetTcpFileSearch.delsvcSearchResult(FileSearch_EntsvcSearchResult);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcSendFileBlock+=new clsNetTcpFileSearch.delsvcSendFileBlock(FileSearch_EntsvcSendFileBlock);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcGetUserList += new clsNetTcpFileSearch.delsvcGetUserList(FileSearch_EntsvcGetUserList);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcSetUserList += new clsNetTcpFileSearch.delsvcSetUserList(FileSearch_EntsvcSetUserList);
                    ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcUnJoin+=new clsNetTcpFileSearch.delsvcUnJoin(FileSearch_EntsvcUnJoin);
                    NetP2PChannel = (IFileTransferChannel)npcFileSearch.OpenClient<IFileTransferChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpFileSearch);
                    //**Opening NetP2p Client for FileSearch.

                    while (Counter < 20)
                    {
                        try
                        {
                            NetP2PChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            Counter = 20;
                            NetP2PChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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
                    BasicHttpClient BasicFileSearchClient = new BasicHttpClient();
                    HttpChannel = (IHttpFileSearchChannel)BasicFileSearchClient.OpenClient<IHttpFileSearchChannel>(strUri);
                    //**opening Http client for FileSearch module

                    while (tempcounter < 20)
                    {
                        try
                        {
                            HttpChannel.svcHttpJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    dt.Interval = TimeSpan.FromSeconds(5);
                    dt.Tick += new EventHandler(dt_Tick);
                    dt.Start();

                    dtGetFileList.Interval = TimeSpan.FromSeconds(10);
                    dtGetFileList.Tick += new EventHandler(dtGetFileList_Tick);
                    dtGetFileList.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncHostFileSearchService", "FileSearch.xaml.cs");
                if (ex.InnerException != null)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncHostFileSearchService", "FileSearch.xaml.cs");

                }
            }
        }

        void listViewResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.listViewResults.SelectedItems.Count != 1)
                    return;
                string path = (string)this.listViewResults.SelectedItems[0].ToString();
                //**Request to oopen localfile named =listViewResults.SelectedItems[0].ToString()

                

                string[] strArray = { "     " };
                string[] temp = path.Split(strArray, StringSplitOptions.None);
                if (temp[0] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    Process.Start(temp[2].ToString());
                    //**Starting localprocess temp[2].ToString()

                }
                else
                {
                    TempRemoteFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    TempRemoteFilePath = TempRemoteFilePath + "\\VMukti\\" + temp[1];
                    //**storing requested file from remote machine at location FilePath = TempRemoteFilePath

                    if (File.Exists(TempRemoteFilePath))
                    {
                        File.Delete(TempRemoteFilePath);
                    }
                    FileStream objFileStream = new FileStream(TempRemoteFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    objFileStream.Close();
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                    {
                        if (NetP2PChannel != null)
                        {
                            NetP2PChannel.svcRequestFile(temp[2].ToString(), temp[0].ToString(), VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            //**Requesting remote file using Netp2p FileName=temp[2].ToString(), To = temp[0].ToString(), From= VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                            
                                                
                            //dtDownloadFile.Tick += new EventHandler(dtDownloadFile_Tick);
                            //dtDownloadFile.Interval= TimeSpan.FromSeconds(7);
                            //dtDownloadFile.Start();
                        }
                    }
                    else
                    {
                        if (HttpChannel != null)
                        {
                            lstDownloadFileList.Add(temp[2].ToString());
                            HttpChannel.svcHttpFileRequest(temp[2].ToString(), temp[0].ToString(), VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            //**Requesting remote file using Http FileName=temp[2].ToString(), To = temp[0].ToString(), From= VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                            
                                                      
                            dtDownloadFile.Tick += new EventHandler(dtDownloadFile_Tick);
                            dtDownloadFile.Interval = TimeSpan.FromSeconds(7);
                            dtDownloadFile.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "listViewResults_MouseDoubleClick", "FileSearch.xaml.cs");
            }
        }

        void objBusiness_FileSearch_EntStatus(string strMessage, bool Error)
        {
            try
            {
                List<object> lstMessage = new List<object>();
                lstMessage.Add(strMessage);
                lstMessage.Add(Error);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUIStatus, lstMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objBusiness_FileSearch_EntStatus", "FileSearch.xaml.cs");
            }
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_VMuktiEvent", "FileSearch.xaml.cs");
            }
        }

        private void ClosePod()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    if (NetP2PChannel != null)
                    {
                        NetP2PChannel.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        
                    }
                }
                else
                {
                    if (HttpChannel != null)
                    {
                        HttpChannel.svcHttpUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }

                if (NetP2PChannel != null)
                {
                    
                    NetP2PChannel = null;
                }

                if (HttpChannel != null)
                {
                    HttpChannel = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod", "FileSearch.xaml.cs");
            }
        }

        #region WCF Events Starts
        void FileSearch_EntsvcJoin(string uName)
         {
            try
            {
            //ListBoxItem ListItem = new ListBoxItem();
            //ListItem.Content = uName;
                if (!lstBuddyList.Contains(uName))
                {
                    lstBuddyList.Add(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcJoin", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_EntsvcSearchQuery(string strQuery, string uName, List<string> lstBuddyList)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uName)
                {
                    string[] sResult = objBusiness_FileSearch.Fncsearch(strQuery);
                    for (int i = 0; i < sResult.Length; i++)
                    {
                        sResult[i] = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "     " + sResult[i];
                    }

                    string[] sGetFileList = Directory.GetFiles(strBrowsingLocation, strQuery + ".*", SearchOption.AllDirectories);
                    if (sGetFileList.Length >= 1)
                    {
                        for (int j = 0; j < sGetFileList.Length; j++)
                        {
                            sGetFileList[j] = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "     " + sGetFileList[j].Split('\\')[sGetFileList[j].Split('\\').Length - 1] + "     " + sGetFileList[j];
                        }
                    }
                    string[] sFinalList = new string[sResult.Length + sGetFileList.Length];
                    sResult.CopyTo(sFinalList, 0);
                    sGetFileList.CopyTo(sFinalList, sResult.Length);
                    NetP2PChannel.svcSearchResult(sFinalList, uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcSearchQuery", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_EntsvcSearchResult(string[] strResult, string uName)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uName)
                {
                    for (int i = 0; i < strResult.Length; i++)
                    {
                        List<object> lstItem = new List<object>();
                        lstItem.Add(strResult[i].ToString());
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUIFileSearchEntsvcSearchResult, lstItem);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcSearchResult", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_EntsvcRequestFile(string FileName, string To, string From)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == To)
                {
                    FileStream fst = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
                    if (!File.Exists(FileName))
                    {
                        //System.Windows.MessageBox.Show("Specify correct file to share!!", "Share File");
                        return;
                    }
                    else
                    {
                        if (fst.Length > 5000)
                        {
                            smallPart = fst.Length / 5000;

                            for (i = 0; i < fst.Length / 5000; i++)
                            {
                                fst.Read(arr, 0, 5000);
                                NetP2PChannel.svcSendFileBlock(arr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,FileName, 0);
                                //HttpChannel.svcHttpFileReply(arr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,FileName, 0);
                            }

                            if (i * 5000 != fst.Length)
                            {
                                Larr = new byte[int.Parse(fst.Length.ToString()) - (i * 5000)];
                                fst.Read(Larr, 0, int.Parse(fst.Length.ToString()) - (i * 5000));
                                // set signal = 1;
                                NetP2PChannel.svcSendFileBlock(Larr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,FileName, 1);
                                //HttpChannel.svcHttpFileReply(Larr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,FileName, 1);
                                fst.Close();
                                fst.Dispose();
                            }
                        }
                        else
                        {
                            Larr = new byte[int.Parse(fst.Length.ToString())];
                            fst.Read(Larr, 0, int.Parse(fst.Length.ToString()));
                            // set signal = 1;
                            NetP2PChannel.svcSendFileBlock(Larr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,FileName, 1);
                            //HttpChannel.svcHttpFileReply(Larr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,FileName, 1);
                            fst.Close();
                            fst.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcRequestFile", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_EntsvcSendFileBlock(byte[] arr, string To, string From,string FileFrom, int signal)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == To)
                {
                    if (objFileStreamWriter == null)
                    {
                        objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);
                        objFileStreamWriter.Position = objFileStreamWriter.Length;
                        objFileStreamWriter.Write(arr, 0, arr.Length);
                        objFileStreamWriter.Close();
                        if (signal == 1)
                        {
                            objFileStreamWriter.Dispose();
                            objFileStreamWriter = null;
                            Process.Start(TempRemoteFilePath);
                        }
                    }
                    else if (objFileStreamWriter != null)
                    {
                        objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);

                        objFileStreamWriter.Position = objFileStreamWriter.Length;
                        objFileStreamWriter.Write(arr, 0, arr.Length);
                        objFileStreamWriter.Close();
                        if (signal == 1)
                        {
                            objFileStreamWriter.Dispose();
                            objFileStreamWriter = null;
                            Process.Start(TempRemoteFilePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcSendFileBlock", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_EntsvcGetUserList(string uName)
        {
            try
            {
                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uName);
                    if (NetP2PChannel != null)
                    {
                        NetP2PChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }

                    //if (HttpChannel != null)
                    //{
                    //    HttpChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    //}
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcGetUserList", "FileSearch.xaml.cs");
            }
        }

        void FileSearch_EntsvcSetUserList(string uName)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcSetUserList", "FileSearch.xaml.cs");
            }

        }
       
        void FileSearch_EntsvcUnJoin(string uName)
        {
            try
            {
                //ListBoxItem ListItem = new ListBoxItem();
                //ListItem.Content = uName;
                if (lstBuddyList.Contains(uName))
                {
                    lstBuddyList.Remove(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearch_EntsvcUnJoin", "FileSearch.xaml.cs");
            }
        }
        #endregion

        #region UI related Delegates
        private void FncDelUIStatus(List<object> lstMessage)
        {
            try
            {
            this.labelStatus.Content = lstMessage[0];

            if (bool.Parse(lstMessage[1].ToString()))
                this.labelStatus.Foreground = Brushes.Red;
            else
                this.labelStatus.Foreground = Brushes.Black;
            System.Windows.Forms.Application.DoEvents();
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncDelUIStatus", "FileSearch.xaml.cs");
            }
        }

        private void FncDelUIFileSearchEntsvcSearchResult(List<object> lstItem)
        {
            try
            {
            this.listViewResults.Items.Add(lstItem[0].ToString());
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncDelUIFileSearchEntsvcSearchResult", "FileSearch.xaml.cs");
            }
        }

        public void DisplayName(string lstUserName)
        {
            try
            {
                bool flag = true;
                for (int i = 0; i < lstBuddyList.Count; i++)
                {
                    if (lstBuddyList[i] == lstUserName)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    lstBuddyList.Add(lstUserName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DisplayName", "FileSearch.xaml.cs");
            }
        }
        #endregion

        #region For Get/Set Messages Timer
        
        void delAsyncGetMessage(List<clsMessage> myMessages)
        {
            try
            {
                if (myMessages != null && myMessages.Count != 0)
                {
                    for (int m = 0; m < myMessages.Count; m++)
                    {
                                       

                        #region Searching File
                        strdtSearchQuery = "";
                        strdtFromUser = "";
                        if ((myMessages[m].strSendQuery != null && myMessages[m].strSendQuery != "") && strdtSearchQuery != myMessages[m].strSendQuery && strdtFromUser != myMessages[m].struName)
                        {
                            strdtSearchQuery = myMessages[m].strSendQuery.ToString();
                            strdtFromUser = myMessages[m].struName.ToString();
                            string[] sTemp = objBusiness_FileSearch.Fncsearch(myMessages[m].strSendQuery.ToString());
                        for (int i = 0; i < sTemp.Length; i++)
                        {
                            sTemp[i] = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "     " + sTemp[i];
                        }
                            string[] sGetFileList = Directory.GetFiles(strBrowsingLocation, myMessages[m].strSendQuery.ToString() + ".*", SearchOption.AllDirectories);
                        if (sGetFileList.Length >= 1)
                        {
                            for (int j = 0; j < sGetFileList.Length; j++)
                            {
                                sGetFileList[j] = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "     " + sGetFileList[j].Split('\\')[sGetFileList[j].Split('\\').Length - 1] + "     " + sGetFileList[j];
                            }
                        }
                        string[] sFinalList = new string[sTemp.Length + sGetFileList.Length];
                        sTemp.CopyTo(sFinalList, 0);
                        sGetFileList.CopyTo(sFinalList, sTemp.Length);
                        if (HttpChannel != null)
                        {
                                HttpChannel.svcHttpReplyQuery(myMessages[m].struName.ToString(), sFinalList);
                        }
                    }
                    #endregion

                    #region Sending File
                        else if (myMessages[m].FilePath != null && myMessages[m].FilePath != "")
                    {
                        //Split particular file and send
                            FileStream fst = new FileStream(myMessages[m].FilePath, FileMode.Open, FileAccess.ReadWrite);
                        if (fst.Length > 5000)
                        {
                            smallPart = fst.Length / 5000;

                            for (i = 0; i < fst.Length / 5000; i++)
                            {
                                fst.Read(arr, 0, 5000);
                                    HttpChannel.svcHttpFileReply(arr, myMessages[m].FileFrom, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myMessages[m].FilePath, 0);
                                //NetP2PChannel.svcSendFileBlock(arr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 0);
                            }

                            if (i * 5000 != fst.Length)
                            {
                                Larr = new byte[int.Parse(fst.Length.ToString()) - (i * 5000)];
                                fst.Read(Larr, 0, int.Parse(fst.Length.ToString()) - (i * 5000));
                                // set signal = 1;
                                    HttpChannel.svcHttpFileReply(arr, myMessages[m].FileFrom, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myMessages[m].FilePath, 1);
                                //NetP2PChannel.svcSendFileBlock(Larr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 1);
                                fst.Close();
                                fst.Dispose();
                            }
                        }
                        else
                        {
                            Larr = new byte[int.Parse(fst.Length.ToString())];
                            fst.Read(Larr, 0, int.Parse(fst.Length.ToString()));
                            // set signal = 1;
                                HttpChannel.svcHttpFileReply(Larr, myMessages[m].FileFrom, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, myMessages[m].FilePath, 1);
                            //NetP2PChannel.svcSendFileBlock(Larr, From, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, 1);
                            fst.Close();
                            fst.Dispose();
                        }
                    }
                    #endregion
                    }
                }

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage", "FileSearch.xaml.cs");
            }
            finally
            {
                dt.Start();
            }
        }

        void OnCompletion(IAsyncResult result)
        {
            try
            {
                List<clsMessage> objMsgs = HttpChannel.EndsvcHttpGetMessage(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetMessage, objMsgs);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "OnCompletion", "FileSearch.xaml.cs");
            }

        }

        void StartThread()
        {
            try
            {
                // this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelGetMsg);
                if (HttpChannel != null)
                {
                    HttpChannel.BeginsvcHttpGetMessage(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnCompletion, null);                    
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "StartThread", "FileSearch.xaml.cs");
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
           // lock (this)
            {
                try
                {
                    dt.Stop();
                   // List<clsMessage> myMessages = HttpChannel.svcHttpGetMessage(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                    t.IsBackground = true;
                    t.Priority = System.Threading.ThreadPriority.Normal;
                    t.Start();     
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dt_Tick", "FileSearch.xaml.cs");
                }
                //finally
                //{
                //    dt.Start();
                //}
            }
        }

        #endregion

        #region For GetFileList Timer

        void StartThread4GetFileList()
        {
            try
            {
                // this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelGetMsg);
                if (HttpChannel != null)
                {
                    HttpChannel.BeginsvcHttpGetFileList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnCompletionGetFileList, null);                    
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "StartThread4GetFileList", "FileSearch.xaml.cs");
            }
        }

        void OnCompletionGetFileList(IAsyncResult result)
        {
            try
            {
                string[] objMsgs = HttpChannel.EndsvcHttpGetFileList(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetMessage4GetFileList, objMsgs);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "OnCompletionGetFileList", "FileSearch.xaml.cs");
            }

        }
      
        void delAsyncGetMessage4GetFileList(string[] myMessages)
        {
            try
            {
                if (myMessages != null && myMessages.Length != 0 && Rolee=="Host")
                {
                    for (int i = 0; i < myMessages.Length; i++)
                    {
                        List<object> lstItem = new List<object>();
                        lstItem.Add(myMessages[i]);
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUIFileSearchEntsvcSearchResult, lstItem);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage4GetFileList", "FileSearch.xaml.cs");
            }
            finally
            {
                dtGetFileList.Start();
            }
        }

        void dtGetFileList_Tick(object sender, EventArgs e)
        {
            lock (this)
            {
                try
                {
                    dtGetFileList.Stop();

                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread4GetFileList));
                    t.IsBackground = true;
                    t.Priority = System.Threading.ThreadPriority.Normal;
                    t.Start();  
                    //string[] myMessages = HttpChannel.svcHttpGetFileList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);                    
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dtGetFileList_Tick", "FileSearch.xaml.cs");
                }
                //finally
                //{
                //    dtGetFileList.Start();
                //}
            }
        }

        #endregion

        void dtDownloadFile_Tick(object sender, EventArgs e)
        {
            try
            {
                List<byte[]> FileData = new List<byte[]>();

                if (lstDownloadFileList.Count > 0)
                {
                    //FileData = HttpChannel.svcHttpDownloadFile(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstDownloadFileList[0].ToString());
                    FileData = HttpChannel.svcHttpDownloadFile(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstDownloadFileList[lstDownloadFileList.Count-1].ToString());
                    //string[] FileNameArr = lstDownloadFileList[0].Split('\\');
                    string[] FileNameArr = lstDownloadFileList[lstDownloadFileList.Count - 1].Split('\\');
                    if (FileData != null)
                    {
                        dtDownloadFile.Stop();
                        TempRemoteFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        TempRemoteFilePath = TempRemoteFilePath + "\\VMukti\\" + FileNameArr[FileNameArr.Length - 1];
                        //**Downloading File at location Path=TempRemoteFilePath

                        ClsException.WriteToLogFile("Fileserch module:");
                        ClsException.WriteToLogFile("Downloading File at location Path:" + TempRemoteFilePath);
                        if (File.Exists(TempRemoteFilePath))
                        {
                            File.Delete(TempRemoteFilePath);
                        }
                        FileStream objFileStream = new FileStream(TempRemoteFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        objFileStream.Close();
                        FileStream objFileStreamWriter = null;
                        for (int i = 0; i < FileData.Count; i++)
                        {
                            if (objFileStreamWriter == null)
                            {
                                objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);
                                objFileStreamWriter.Position = objFileStreamWriter.Length;
                                objFileStreamWriter.Write(FileData[i], 0, FileData[i].Length);
                                objFileStreamWriter.Close();

                            }
                            else if (objFileStreamWriter != null)
                            {
                                objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);
                                objFileStreamWriter.Position = objFileStreamWriter.Length;
                                objFileStreamWriter.Write(FileData[i], 0, FileData[i].Length);
                                objFileStreamWriter.Close();
                            }
                        }
                        objFileStreamWriter.Dispose();
                        Process.Start(TempRemoteFilePath.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dtDownloadFile_Tick", "FileSearch.xaml.cs");
            }
        }
        
        private long FncGetfoldersize(string path)
        {
            try
            {
                System.IO.DirectoryInfo DirFileSizeInfo = new DirectoryInfo(path);
                foreach (FileInfo fi in DirFileSizeInfo.GetFiles())
                {
                    if (fileSize <= 15728640)
                    {
                        fileSize = fileSize + fi.Length;
                    }
                    else
                    {
                        return 15728640;
                    }
                }
                foreach (DirectoryInfo DirSubFolder in DirFileSizeInfo.GetDirectories())
                {
                    if (fileSize <= 15728640)
                    {
                        FncGetfoldersize(DirSubFolder.FullName);
                    }
                    else
                    {
                        return 15728640;
                    }
                }
                return fileSize;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncGetfoldersize", "FileSearch.xaml.cs");
                return 0;
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "FileSearch.xaml.cs");
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
                        arr = null;
                        Larr = null;
                        i = 0;
                        Counter = 0;
                        tempcounter = 0;
                        TempRemoteFilePath = string.Empty;
                        strUri = string.Empty;
                        strdtSearchQuery = string.Empty;
                        strdtFromUser = string.Empty;
                        objFileStreamWriter = null;
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "FileSearch.xaml.cs");
                    }
                }
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

         ~FileSearch()
        {
            Dispose(false);
        }

        #endregion

         #region logging function

         //public static StringBuilder CreateTressInfo()
         //{
         //    StringBuilder sb = new StringBuilder();
         //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
         //    sb.AppendLine();
         //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
         //    sb.AppendLine();
         //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
         //    sb.AppendLine();
         //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
         //    sb.AppendLine();
         //    sb.AppendLine("----------------------------------------------------------------------------------------");
         //    return sb;
         //}

         #endregion
    }
}
