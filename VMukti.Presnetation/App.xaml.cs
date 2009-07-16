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
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Net;
using System.Reflection;
using VMukti.Business.WCFServices.BootStrapServices.BasicHttp;
using VMuktiService;
using VMukti.Business.WCFServices.SuperNodeServices.BasicHttp;
using VMukti.Business.WCFServices.BootStrapServices.NetP2P;
using VMukti.Business.WCFServices.SuperNodeServices.NetP2P;
using System.Security.Permissions;
using VMuktiAPI;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data.SqlServerCe;
using System.Data.SqlServerCe;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
//using VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P;
//using VMukti.Business.WCFServices.BootStrapServices.NetP2P;
using VMukti.ZipUnzip.Zip;
using System.Threading;
using System.IO;
using System.Windows.Interop;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;
//using LumiSoft.Net.STUN.Client;
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using VMukti.Business.WCFServices.SuperNodeServices.DataContract;
using VMukti.StunFireWallDetector; 
using VMukti.Presentation;
using System.Windows.Threading;
using VMukti.Business;
using System.ComponentModel;

namespace VMukti
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	/// 
	public partial class App : Application, IDisposable
	{
        public static StringBuilder sb1 = new StringBuilder();

		#region Http Clients

        public static IHTTPBootStrapService chHttpBootStrapService;
        public static BasicHttpClient objHttpBootStrap;
		
        public static IHttpSuperNodeService chHttpSuperNodeService;
        public static BasicHttpClient objHttpSuperNode;

		#endregion

		#region NetP2P Clients

        public static object objNetP2PSuperNode;
        public static INetP2PSuperNodeChannel chNetP2PSuperNodeChannel;

        public static int pageCounter;
        public static int tabCounter;
        public static int podCounter;
        public static bool blnIsTwoPanel;

		#endregion	

        System.Xml.XmlNodeList xmlNodes;
        AppDomainSetup DomainSetUp;

        Thread thBootStrapDomain;
        Thread thSuperNodeDomain;
        string strFilePath;
        private bool disposed;

        public static bool blnNodeOff;
        bool IsAbleToBecomeSuperNode;

        public static List<ClsModuleLogic> lstCollOnly = new List<ClsModuleLogic>();

        BackgroundWorker bgLoadBootstrapDomain = new BackgroundWorker();
        BackgroundWorker bgLoadSupernode = new BackgroundWorker();
        BackgroundWorker bgOpenHttpBootStrapClient = new BackgroundWorker();

        #region Disaster Recovery

        bool NetAvail;
        public DispatcherTimer dtWebReqBS = new DispatcherTimer();

        #endregion

        System.Threading.Thread thGlobalVariable;

		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
                try
                {
                    try
                    {

                        thGlobalVariable = new Thread(new ThreadStart(GlobalVariable));
                        thGlobalVariable.Start();
                        
                        bgLoadBootstrapDomain.DoWork += new DoWorkEventHandler(bgLoadBootstrapDomain_DoWork);
                        bgLoadBootstrapDomain.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgLoadBootstrapDomain_RunWorkerCompleted);
                        bgLoadSupernode.DoWork += new DoWorkEventHandler(bgLoadSupernode_DoWork);
                        bgOpenHttpBootStrapClient.DoWork += new DoWorkEventHandler(bgOpenHttpBootStrapClient_DoWork);

                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter sWriter = null;

                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt"))
                        {
                            sWriter = new StreamWriter(File.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt"));
                        }
                        else
                        {
                            sWriter = new StreamWriter(fs);
                            fs.SetLength(0);
                        }
                        sWriter.Write("Initializing");
                        sWriter.Flush();
                        sWriter.Close();
                        fs.Close();
                       
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "App.xaml.cs");
                    }


                    string str = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.ActivationUri.AbsoluteUri.ToString();
                    string strTemp = str;
                    str = str.ToLower();
                    int i = str.IndexOf("vmukti.presentation.xbap".ToLower());
                    VMuktiAPI.VMuktiInfo.ZipFileDownloadLink = strTemp.Substring(0, i);
                    //new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "Configuration.xml", AppDomain.CurrentDomain.BaseDirectory.ToString() + "Configuration.xml");
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceds35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceds35.dll");

                    System.Xml.XmlDocument ConfDoc = new System.Xml.XmlDocument();
                    ConfDoc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceds35.dll");
                    System.Xml.XmlNodeList xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("BootStrpIP");
                    List<string> tempLst = new List<string>();
                    tempLst = VMuktiAPI.VMuktiInfo.BootStrapIPs;
                    tempLst.Add(DecodeBase64String(xmlNodes[0].Attributes["Value"].Value.ToString()));
                    VMuktiAPI.VMuktiInfo.BootStrapIPs = tempLst;

                    xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("VMuktiVersion");
                    VMuktiAPI.VMuktiInfo.VMuktiVersion = DecodeBase64String(xmlNodes[0].Attributes["Value"].Value.ToString());

                     xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("ExternalPBX");
                    VMuktiAPI.VMuktiInfo.strExternalPBX = DecodeBase64String(xmlNodes[0].Attributes["Value"].Value.ToString());

                    xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("Port80");
                    VMuktiAPI.VMuktiInfo.Port80 = DecodeBase64String(xmlNodes[0].Attributes["Value"].Value.ToString());

                    xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("connstring");
                    VMuktiAPI.VMuktiInfo.MainConnectionString = xmlNodes[0].Attributes["Value"].Value.ToString();
                    VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = VMuktiAPI.AuthType.SQLAuthentication;

                }
                catch
                { 
                    List<string> tempLst = new List<string>();
                    tempLst.Add("192.168.191.212");

                    VMuktiAPI.VMuktiInfo.BootStrapIPs.Add("192.168.191.212");
                    VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = "192.168.191.212";

                    VMuktiAPI.VMuktiInfo.BootStrapIPs = tempLst;
                    
                    VMuktiAPI.VMuktiInfo.ZipFileDownloadLink = "http://192.168.191.212/vmuktimeetingplace/";
                    VMuktiAPI.VMuktiInfo.VMuktiVersion = "1.0";
                    VMuktiAPI.VMuktiInfo.strExternalPBX = "false";
                    VMuktiAPI.VMuktiInfo.Port80 = "false";
                    VMuktiAPI.VMuktiInfo.MainConnectionString = @"Data Source=192.168.191.212;Initial Catalog=vmukti;User ID=sa;PassWord=mahavir";
                    VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = VMuktiAPI.AuthType.SQLAuthentication;
                }

                IPAddress[] ipList = System.Net.Dns.GetHostEntry(System.Environment.MachineName).AddressList; 

				for (int i = 0; i < ipList.Length; i++)
				{
					int j = 0;

					List<string> tempLst = new List<string>();
					tempLst = VMuktiAPI.VMuktiInfo.CurrentPeer.NodeIPs;

					tempLst.Add(ipList[i].ToString());

					VMuktiAPI.VMuktiInfo.CurrentPeer.NodeIPs = tempLst;

					for (j = 0; j < VMuktiAPI.VMuktiInfo.BootStrapIPs.Count; j++)
					{
						if (VMuktiAPI.VMuktiInfo.CurrentPeer.NodeIPs[i] == VMuktiAPI.VMuktiInfo.BootStrapIPs[j])
						{
							break;
						}
					}
					if (j < VMuktiAPI.VMuktiInfo.BootStrapIPs.Count)
					{
						VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.BootStrap;
						VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = VMuktiAPI.VMuktiInfo.BootStrapIPs[j];
                    }
				}
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "OnStartup", "App.xaml.cs");
			}

			#region Functions which will make decision whether node/supernode

			if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
			{
				DomainSetUp = new AppDomainSetup();
				DomainSetUp.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
				DomainSetUp.ConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config";
                ClsException.WriteToLogFile("System Type:" + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
                VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP = VMuktiAPI.VMuktiInfo.BootStrapIPs[0];
         
                bgLoadBootstrapDomain.RunWorkerAsync();
			}

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NotDecided)
            {
                try
                {
                    if (FncInLan(VMuktiAPI.VMuktiInfo.BootStrapIPs[0]))
                    {
                        //System In LAN
                        if (FncCheckFireWallStatus())
                        {
                            ClsException.WriteToLogFile("BootStrap In LAN :: Fire Wall is on in FncCheckFireWallStatus means node with http");
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithHttp;
                        }
                        else
                        {
                            if (FncPort4000Free())
                            {
                                if (FncPort80Free() && IsAbleToBecomeSuperNode)
                                {
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.SuperNode;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;

                                    DomainSetUp = new AppDomainSetup();
                                    DomainSetUp.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                                    DomainSetUp.ConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config";

                                    bgLoadSupernode.RunWorkerAsync();

                                    ClsException.WriteToLogFile(" BootStrap In LAN :: FncPort80Free is true means supernode");
                                }
                                else
                                {
                                    ClsException.WriteToLogFile("BootStrap In LAN :: FncPort80Free returns false  means node with P2P");
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithNetP2P;
                                }
                            }
                            else
                            {
                                ClsException.WriteToLogFile("BootStrap In LAN :: FncPort4000Free returns false  means node with http");
                                VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithHttp;
                            }
                        }
                    }
                    else
                    {
                        #region Definding PeerType for LiveIP system
                        if (FncFireWall())
                        {
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithHttp;
                            ClsException.WriteToLogFile("In LIve IP :: Fire Wall is true in LiveIP  in FncFireWall means node with http");
                            AssignMachineIP();
                        }
                        else
                        {
                            if (FncIsLiveIP())
                            {
                                if (FncPort4000Free())
                                {
                                    if (FncPort80Free())
                                    {
                                        //System is SUPERNODE

                                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.SuperNode;
                                        VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;//GetIP4Address();
                                        DomainSetUp = new AppDomainSetup();
                                        DomainSetUp.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                                        DomainSetUp.ConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config";

                                        bgLoadSupernode.RunWorkerAsync();
                                        ClsException.WriteToLogFile("In Live IP :: FncPort80Free is true means supernode");
                                    }
                                    else
                                    {
                                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithNetP2P;
                                        ClsException.WriteToLogFile("In Live IP :: FncPort80Free is false that means node with NodeWithNetP2P");
                                    }
                                }
                                else
                                {
                                    //PeerType nodewithhttp
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithHttp;
                                    ClsException.WriteToLogFile("In Live IP :: FncPort4000Free is false that means node with NodeWithHttp");
                                }

                        #endregion

                            }
                            else
                            {
                                VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType = VMuktiAPI.PeerType.NodeWithNetP2P;
                                ClsException.WriteToLogFile("This is not live IP and firewall is off that means node with NodeWithNetP2P");
                                AssignMachineIP();
                            }
                        }
                    }
					ClsException.WriteToLogFile("System Type:" + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
				}
				catch (Exception ex)
				{
                    VMuktiHelper.ExceptionHandler(ex, "OnStartUp", "App.xaml.cs");
				}
			}

			#endregion

            #region Downlaod sqlceme35 files for call center version whose system type is Nodewithp2p or nodewith http
            try
            {
                if ((VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp) && VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.1")
                {
                    if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceme35.dll"))
                    {
                        new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceme35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceme35.dll");
                        new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceqp35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceqp35.dll");
                        new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlcese35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlcese35.dll");

                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OnStartUp()--DownloadingSQLCE35Files", "App.xaml.cs");
            }
            #endregion

            #region Starting Background Worker thread, To Downlaod VistaAudio.zip
            try
            {
                System.OperatingSystem osInfo = System.Environment.OSVersion;
                if (osInfo.Version.Major.ToString() == "6")
                {
                    System.ComponentModel.BackgroundWorker DownloadVistaAudioBW = new System.ComponentModel.BackgroundWorker();
                    DownloadVistaAudioBW.DoWork += new System.ComponentModel.DoWorkEventHandler(DownloadVistaAudioBW_DoWork);
                    DownloadVistaAudioBW.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Starting BackGround worker thread for vista Audio", "App.xaml.cs");
            }
            #endregion

            #region Starting Background Worker thread, To Downlaod ReportViewer Dlls
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.1")
                {
                    System.ComponentModel.BackgroundWorker DownloadReportViewerFileBW = new System.ComponentModel.BackgroundWorker();
                    DownloadReportViewerFileBW.DoWork += new DoWorkEventHandler(DownloadReportViewerFileBW_DoWork);
                    DownloadReportViewerFileBW.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Starting BackGround worker thread for vista Audio", "App.xaml.cs");
            }
            #endregion

            #region Starting Background Worker thread, To Downlaod RecoredingProfile.zip
            //try
            //{
            //        System.ComponentModel.BackgroundWorker DownloadRecoredingProfileBW = new System.ComponentModel.BackgroundWorker();
            //        DownloadRecoredingProfileBW.DoWork+=new System.ComponentModel.DoWorkEventHandler(DownloadRecoredingProfileBW_DoWork);
            //        DownloadRecoredingProfileBW.RunWorkerAsync();
            //}
            //catch (Exception ex)
            //{
            //    VMuktiHelper.ExceptionHandler(ex, "Starting BackGround worker thread for Downlaoding Recoreding Profiles", "App.xaml.cs");
            //}
            #endregion
            
			if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
			{
                //OpenBootStrapHttpClient();
                bgOpenHttpBootStrapClient.RunWorkerAsync();

                
                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                {

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService = null;
                    VMukti.Business.clsDataBaseChannel.OpenDataBaseClient();
                }
                else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P && VMuktiAPI.VMuktiInfo.VMuktiVersion=="1.1")
                {
                    try
                    {
                        YatePBX.Presentation.YatePBX objPBX = new YatePBX.Presentation.YatePBX();
                        objPBX.FncStartPBX(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP);
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "App.xaml.cs--:--OnStartup()", "Starting PBX For P2P systems");
                    }
                }
            }

            VMuktiAPI.VMuktiHelper.RegisterEvent("GetSuperNodeIP").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(App_VMuktiEvent_GetSuperNodeIP);

			Application.Current.MainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            #region Disaster Recovery
            dtWebReqBS.Interval = TimeSpan.FromSeconds(15);
            dtWebReqBS.Tick += new EventHandler(dtWebReqBS_Tick);
            #endregion

			base.OnStartup(e);
		}

        #region Global Variable Initialization

        void GlobalVariable()
        {
            try
            {
                objHttpBootStrap = new BasicHttpClient();
                objHttpSuperNode = new BasicHttpClient();

                pageCounter = 0;
                tabCounter = 0;
                podCounter = 0;
                blnIsTwoPanel = true;

                disposed = false;
                blnNodeOff = false;

                NetAvail = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GlobalVariable", "App.xaml.cs");
            }
        }

        #endregion

        void bgOpenHttpBootStrapClient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                chHttpBootStrapService = (IHTTPBootStrapService)objHttpBootStrap.OpenClient<IHTTPBootStrapChannel>("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");
                clsBootStrapInfo objBootStrapInfo = null;
                objBootStrapInfo = App.chHttpBootStrapService.svcHttpBSJoin("", null);

                switch (objBootStrapInfo.AuthType)
                {
                    case "SIPAuthentication":
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SIPAuthentication;
                        break;

                    case "SQLAuthentication":
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SQLAuthentication;
                        break;

                    default:
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.NotDecided;
                        break;

                }

                bool isSuperNode = false;

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    isSuperNode = true;
                }
                else
                {
                    isSuperNode = false;
                }

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == null || VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == string.Empty)
                {
                    VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;
                }

                clsSuperNodeDataContract objSuperNodeDataContract = App.chHttpBootStrapService.svcHttpBsGetSuperNodeIP(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, isSuperNode);
                
                VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = objSuperNodeDataContract.SuperNodeIP;

                //VMuktiAPI.ClsException.WriteToLogFile("supernodeip:-" + objSuperNodeDataContract.SuperNodeIP);
                //MessageBox.Show("supernodeip:-" + objSuperNodeDataContract.SuperNodeIP);

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP == null || VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP == string.Empty)
                {
                    ClsException.WriteToLogFile("Somehow supernode is not available so assigning bootstrap IP");
                    VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = VMuktiAPI.VMuktiInfo.BootStrapIPs[0];
                }

                ClsException.WriteToLogFile("My SuperNode IP is :: " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "bgOpenHttpBootStrapClient_DoWork", "App.xaml.cs");
            }
        }

        

        void bgLoadBootstrapDomain_DoWork(object sender, DoWorkEventArgs e)
		{
			try
		{
			try
			{
				VMuktiAPI.VMuktiInfo.BootStrapDomain = AppDomain.CreateDomain("BootStrapDomain", null, DomainSetUp, new System.Security.PermissionSet(PermissionState.Unrestricted));
				VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
				VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
				VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.IsApplicationTrustedToRun = true;
				VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.Persist = true;
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "LoadBootStrap()", "App.xaml.cs");
			}
			try
			{
				WriteDomainStatus("BootStrapDomain");
				VMuktiAPI.VMuktiInfo.BootStrapDomain.CreateInstance("VMukti.Presentation", "VMukti.Presentation.BootstrapServiceDomain");


                   // OpenBootStrapHttpClient();
                    bgOpenHttpBootStrapClient.RunWorkerAsync();
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "OnStartUp()", "App.xaml.cs");
			}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "bgLoadBootstrapDomain_DoWork", "App.xaml.cs");
            }
        }

        void bgLoadBootstrapDomain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
            {
                
                bgLoadSupernode.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void bgLoadSupernode_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                VMuktiAPI.VMuktiInfo.SuperNodeDomain = AppDomain.CreateDomain("SuperNodeDomain", null, DomainSetUp, new System.Security.PermissionSet(PermissionState.Unrestricted));
				VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
				VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
				VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.IsApplicationTrustedToRun = true;
				VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.Persist = true;
            WriteDomainStatus("SuperNodeDomain");
				VMuktiAPI.VMuktiInfo.SuperNodeDomain.CreateInstance("VMukti.Presentation", "VMukti.Presentation.SuperNodeServiceDomain");
				WriteDomainStatus("Initializing");
                
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "LoadSuperNode()--GetIP4Address", "App.xaml.cs");
                MessageBox.Show(ex.Message);
            }
        }

        void DownloadVistaAudioBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "VistaAudio.zip"))
                {
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "VistaAudio.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "VistaAudio.zip");
                    FastZip fz = new FastZip();
                    fz.ExtractZip(AppDomain.CurrentDomain.BaseDirectory.ToString() + "VistaAudio.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "VistaAudio", null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DownloadVistaAudioBW_DoWork()", "App.xaml.cs");
            }
        }

        void DownloadReportViewerFileBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Microsoft.ReportViewer.Common.dll"))
                {
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "Microsoft.ReportViewer.Common.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "Microsoft.ReportViewer.Common.dll");
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "Microsoft.ReportViewer.ProcessingObjectModel.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "Microsoft.ReportViewer.ProcessingObjectModel.dll");
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "Microsoft.ReportViewer.WinForms.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "Microsoft.ReportViewer.WinForms.dll");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DownloadReportViewerFileBW_DoWork()", "App.xaml.cs");
            }
        }

		private void WriteDomainStatus(string strStatus)
		{
			try
			{
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt", FileMode.Open, FileAccess.Write, FileShare.ReadWrite);

                StreamWriter sWriter = new StreamWriter(fs);

                fs.SetLength(0);

				sWriter.Write(strStatus);
                sWriter.Flush();
				sWriter.Close();
                fs.Close();
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "WriteDomainStatus()", "App.xaml.cs");
			}
		}

        //private void LoadBootStrap()
        //{
        //    //try
        //    //{
        //    //    VMuktiAPI.VMuktiInfo.BootStrapDomain = AppDomain.CreateDomain("BootStrapDomain", null, DomainSetUp, new System.Security.PermissionSet(PermissionState.Unrestricted));
        //    //    VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
        //    //    VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
        //    //    VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.IsApplicationTrustedToRun = true;
        //    //    VMuktiAPI.VMuktiInfo.BootStrapDomain.ApplicationTrust.Persist = true;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    VMuktiHelper.ExceptionHandler(ex, "LoadBootStrap()", "App.xaml.cs");
        //    //}
        //    //try
        //    //{
        //    //    WriteDomainStatus("BootStrapDomain");
        //    //    VMuktiAPI.VMuktiInfo.BootStrapDomain.CreateInstance("VMukti.Presentation", "VMukti.Presentation.BootstrapServiceDomain");

        //    //    OpenBootStrapHttpClient();               
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    VMuktiHelper.ExceptionHandler(ex, "OnStartUp()", "App.xaml.cs");
        //    //}
			
        //    ////thSuperNodeDomain = new Thread(new ThreadStart(LoadSuperNode));
        //    ////thSuperNodeDomain.Priority = ThreadPriority.Highest;
        //    ////thSuperNodeDomain.Start();

        //    //bgLoadSupernode.RunWorkerAsync();
        //}

        //private void LoadSuperNode()
        //{
        //    try
        //    {
        //        VMuktiAPI.VMuktiInfo.SuperNodeDomain = AppDomain.CreateDomain("SuperNodeDomain", null, DomainSetUp, new System.Security.PermissionSet(PermissionState.Unrestricted));
        //        VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
        //        VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
        //        VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.IsApplicationTrustedToRun = true;
        //        VMuktiAPI.VMuktiInfo.SuperNodeDomain.ApplicationTrust.Persist = true;
        //        WriteDomainStatus("SuperNodeDomain");
        //        VMuktiAPI.VMuktiInfo.SuperNodeDomain.CreateInstance("VMukti.Presentation", "VMukti.Presentation.SuperNodeServiceDomain");
        //        WriteDomainStatus("Initializing");
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiHelper.ExceptionHandler(ex, "LoadSuperNode()--GetIP4Address", "App.xaml.cs");
        //    }
        //}

		void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
                ClsException.WriteToLogFile("MainWindow_Closing  -- App.xaml.cs  " + DateTime.Now); 
				LogOut();
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "MainWindow_Closing", "App.xaml.cs");
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			try
			{
				System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("Yate");
				for (int i = 0; i < p.Length; i++)
				{
					p[i].Kill();
				}
                System.Diagnostics.Process[] pConsole = System.Diagnostics.Process.GetProcessesByName("VMuktiMonitor");
                for (int j = 0; j < pConsole.Length; j++)
                {
                    pConsole[j].Kill();
                }
                ClsException.WriteToLogFile("OnExit()--:--App.xaml.cs  " + DateTime.Now); 
				LogOut();
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "OnExit()", "App.xaml.cs");
			}

			base.OnExit(e);

		}

		void LogOut()
		{
			try
			{
                ClsException.WriteToLogFile("LogOut  -- App.xaml.cs  " + DateTime.Now); 
				VMuktiAPI.VMuktiHelper.CallEvent("LogoutBuddyList", this, new VMuktiEventArgs());				
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "LogOut()", "App.xaml.cs");
			}
		}		

        public static void OpenBootStrapHttpClient()
        {
            try
            {
                chHttpBootStrapService = (IHTTPBootStrapService)objHttpBootStrap.OpenClient<IHTTPBootStrapChannel>("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");
                clsBootStrapInfo objBootStrapInfo = null;
                objBootStrapInfo = App.chHttpBootStrapService.svcHttpBSJoin("", null);

                switch (objBootStrapInfo.AuthType)
                {
                    case "SIPAuthentication":
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SIPAuthentication;
                        break;

                    case "SQLAuthentication":
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SQLAuthentication;
                        break;

                    default:
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.NotDecided;
                        break;

                }

                bool isSuperNode = false;
              
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    isSuperNode = true;
                }
                else
                {
                    isSuperNode = false;
                }

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == null || VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == string.Empty)
                {
                    VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;
                }

                clsSuperNodeDataContract objSuperNodeDataContract = App.chHttpBootStrapService.svcHttpBsGetSuperNodeIP(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, isSuperNode);
                VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = objSuperNodeDataContract.SuperNodeIP;

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP == null || VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP == string.Empty)
                {
                    ClsException.WriteToLogFile("Somehow supernode is not available so assigning bootstrap IP");
                    VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = VMuktiAPI.VMuktiInfo.BootStrapIPs[0];
                }

                ClsException.WriteToLogFile("My SuperNode IP is :: " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                  
            }
           
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OpenBootStrapHttpClient", "App.xaml.cs");
            }
        }

        private string DecodeBase64String(string strValue)
        {
            try
            {
                System.Text.UTF32Encoding objUTF32 = new System.Text.UTF32Encoding();
                byte[] objbytes = Convert.FromBase64String(strValue);
                return objUTF32.GetString(objbytes);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DecodeBase64String()", "App.xaml.cs");
                return null;
            }
        }

		#region Detection of Node / SuperNode

        bool FncInLan(string serverIP)
        {
            try
            {
                List<IPAddressRange> lstLanIPAddrRange = new List<IPAddressRange>();


                IPAddressRange ipRange = new IPAddressRange();
                ipRange.lo = IPAddress.Parse("10.0.0.0");
                ipRange.hi = IPAddress.Parse("10.255.255.255");
                lstLanIPAddrRange.Add(ipRange);

                ipRange = new IPAddressRange();
                ipRange.lo = IPAddress.Parse("172.16.0.0");
                ipRange.hi = IPAddress.Parse("172.31.255.255");
                lstLanIPAddrRange.Add(ipRange);

                ipRange = new IPAddressRange();
                ipRange.lo = IPAddress.Parse("192.168.0.0");
                ipRange.hi = IPAddress.Parse("192.168.255.255");
                lstLanIPAddrRange.Add(ipRange);

                if (IsAddressInRange(IPAddress.Parse(serverIP), lstLanIPAddrRange))
                {
                    foreach (IPAddress ipaddress in (System.Net.Dns.GetHostEntry(System.Environment.MachineName)).AddressList)
                    {
                        if (ipaddress.AddressFamily.ToString() == "InterNetwork" && ipaddress.ToString() != "127.0.0.1")
                        {
                            string myIP = ipaddress.ToString().Substring(0, ipaddress.ToString().LastIndexOf("."));
                            string serIP = serverIP.Substring(0, serverIP.LastIndexOf("."));
                            if (myIP == serIP)
                            {
                                VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP = ipaddress.ToString();
                                IsAbleToBecomeSuperNode = true;
                                break;
                            }
                            else
                            {
                                VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP = ipaddress.ToString();
                                IsAbleToBecomeSuperNode = false;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }                             
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncInLan()", "App.xaml.cs");
                return false;
            }           
        }

        bool FncIsLiveIP()
        {
            try
            {
                WebRequest myRequest = WebRequest.Create("http://www.whatismyip.com/");
                WebResponse res = myRequest.GetResponse();
                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                string html = sr.ReadToEnd();
                Regex regex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                string OipString = regex.Match(html).Value;
                ClsException.WriteToLogFile("ip Address is ::" + OipString);
                string hostName = Dns.GetHostName();
                IPHostEntry local = Dns.GetHostEntry(hostName);
                foreach (IPAddress ipaddress in local.AddressList)
                {
                    if (ipaddress.AddressFamily.ToString() == "InterNetwork" && ipaddress.ToString() != "127.0.0.1")
                    {
                        if (OipString == ipaddress.ToString())
                        {
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP = OipString;
                          return true;
                        }
                    }
                }               

                return false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncLiveIP()", "App.xaml.cs");
                return false;
            }
        }

		bool FncVistaOS()
		{
			try
			{
				System.OperatingSystem osInfo = System.Environment.OSVersion;
				if (osInfo.Version.Major.ToString() == "6")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "FncVistaOS()", "App.xaml.cs");
				return false;
			}
		}      

		bool FncFireWall()
		{
			
			try
			{
				string NatType;
				m_pGet_Click(out  NatType);

                ClsException.WriteToLogFile("Stun has detected your systemtype :: " + NatType);

                if (NatType == "UdpBlocked")
                {
                    try
                    {
                        System.Xml.XmlDocument ConfDoc = new System.Xml.XmlDocument();
                        ConfDoc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceds35.dll");
                        System.Xml.XmlNodeList tempxmlNodes = ConfDoc.GetElementsByTagName("SQLInfo");
                        string tempConnection = "Data Source=" + DecodeBase64String(tempxmlNodes[0].Attributes["SQLInstanceName"].Value.ToString()) + ";Initial Catalog=" + DecodeBase64String(tempxmlNodes[0].Attributes["DatabaseName"].Value.ToString()) + ";User ID=" + DecodeBase64String(tempxmlNodes[0].Attributes["SQLUserName"].Value.ToString()) + ";PassWord=" + DecodeBase64String(tempxmlNodes[0].Attributes["SQLPassword"].Value.ToString());

                        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(tempConnection);
                        conn.Open();
                        conn.Close();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        ClsException.WriteToLogFile("FncFireWall:- Connection failed to open");
                        return true;
                    }

                }

                else if (NatType == "HttpPort80" || NatType == "NoInternetSupport" || NatType == string.Empty) //|| NatType == "RestrictedCone" || NatType == "UdpBlocked"
                {
                    return true;
                }
                else
                {
                    return false;
                }
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "FncFireWall()", "App.xaml.cs");
				return false;
			}
		}

		bool FncLocalFirewall()
		{
			try
			{
				VMukti.Presentation.WinXPSP2FireWall fw = new VMukti.Presentation.WinXPSP2FireWall();
				fw.Initialize();
				Boolean b = new Boolean();
				fw.IsWindowsFirewallOn(ref b);
				if (b == false)
					return b; //FireWall Off
				else
					//FireWall ON
					return b;
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "FncLocalFirewall()", "App.xaml.cs");
				return false;
			}
		}

		bool FncVistaLocalFireWall()
		{
			try
			{
				Microsoft.Win32.RegistryKey regKey;
				regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile");
				string[] s = regKey.GetValueNames();
				string s1 = (string)regKey.GetValue(s[0].ToString()).ToString();
				if (s1 == "0")
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception ex)
			{
                VMuktiHelper.ExceptionHandler(ex, "FncVistaLocalFireWall()", "App.xaml.cs");
				return true;
			}
		}

        bool FncPort80Free()
        {
            try
            {
                object objHttpBootStrap = null;
                objHttpBootStrap = new BootStrapDelegates();
                BasicHttpServer bhsHttpBootStrap = new BasicHttpServer(ref objHttpBootStrap, "http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP + ":80/HttpBootStrap");
                bhsHttpBootStrap.AddEndPoint<IHTTPBootStrapService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP + ":80/HttpBootStrap");
                bhsHttpBootStrap.OpenServer();
                bhsHttpBootStrap.CloseServer();
                return true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPort80busy()", "App.xaml.cs");
                return false;
            }

        }

        bool FncPort4000Free()
        {
            try
            {
                //NetPeerServer npsTestServer = new NetPeerServer("net.tcp://" + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP + ":4000/NetP2PTest");
                //npsTestServer.AddEndPoint("net.tcp://" + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP + ":4000/NetP2PTest");
                //npsTestServer.OpenServer();
                //npsTestServer.CloseServer();

                //System.Net.Sockets.Socket sock = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                //sock.Connect(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, 4000);
                //if (sock.Connected == true)  // Port is in use and connection is successful
                //    MessageBox.Show("Port is Closed");
                //sock.Close();

                TcpListener tcpListener = new TcpListener(IPAddress.Parse(VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP), 4000);
                tcpListener.Start();
                tcpListener.Stop();

                return true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPort4000Free()", "App.xaml.cs");
                return false;
            }
        }

        public static void m_pGet_Click(out string NatType)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(new IPEndPoint(IPAddress.Any, 0));

                STUN_Result result = null;
                result = STUN_Client.Query("stunserver.org", 3478, socket);/*3478*///www.stunserver.org
                NatType = result.NetType.ToString();
            }
            catch
            {
                NatType = string.Empty;
            }
        }


        public struct IPAddressRange
        {
            public IPAddress hi;
            public IPAddress lo;
        }


        bool IsAddressInRange(IPAddress addrSrc, List<IPAddressRange> lstrange)
        {
            try
            {
                bool isIPIsInRange = false;

                foreach (IPAddressRange range in lstrange)
                {
                    long addr = IPAddress.NetworkToHostOrder(getLongFromAddress(addrSrc));
                    isIPIsInRange = (addr >= IPAddress.NetworkToHostOrder(getLongFromAddress(range.lo))
                        && addr <= IPAddress.NetworkToHostOrder(getLongFromAddress(range.hi)));
                    if (isIPIsInRange)
                    {
                        return true;
                    }
                }
                return isIPIsInRange;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "IsAddressInRange()", "App.xaml.cs");
                return false;
            }

        }

        long getLongFromAddress(IPAddress ipa)
        {
            try
            {
                byte[] b = ipa.GetAddressBytes();
                long l = 0;
                for (int i = 0; i < b.Length; i++)
                {
                    l += (long)(b[i] * Math.Pow(256, i));
                }
                return l;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "getLomgFromAddress()", "App.xaml.cs");
                return 0;
            }
        }

      

        bool FncCheckFireWallStatus()
        {
            try
            {
                if (FncVistaOS())
                {
                    return FncVistaLocalFireWall();
                }
                else
                {
                    return FncLocalFirewall();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncCheckFireWallStatus()", "App.xaml.cs");                
                return false;
            }
        }

        void AssignMachineIP()
        {
            try
            {
                foreach (IPAddress ipaddress in (System.Net.Dns.GetHostEntry(System.Environment.MachineName)).AddressList)
                {
                    if (ipaddress.AddressFamily.ToString() == "InterNetwork" && ipaddress.ToString() != "127.0.0.1")
                    {
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP = ipaddress.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AssignMachineIP()", "App.xaml.cs");                
            }

        }

		#endregion

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
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "App.xaml.cs");
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
						if (chHttpBootStrapService != null)
						{
							chHttpBootStrapService = null;
						}
						if (objHttpBootStrap != null)
						{
							objHttpBootStrap = null;
						}

						if (App.chNetP2PSuperNodeChannel != null)
						{
							((NetPeerClient)App.objNetP2PSuperNode).CloseClient<INetP2PSuperNodeChannel>();
							App.chNetP2PSuperNodeChannel = null;
						}
						if (App.objNetP2PSuperNode != null)
						{
							App.objNetP2PSuperNode = null;
						}						
						if (chHttpSuperNodeService != null)
						{
							chHttpSuperNodeService = null;
						}
						if (objHttpSuperNode != null)
						{
							objHttpSuperNode = null;
						}
						if (objNetP2PSuperNode != null)
						{
							objNetP2PSuperNode = null;
						}
						if (chNetP2PSuperNodeChannel != null)
						{
							chNetP2PSuperNodeChannel = null;
						}
						
						if (DomainSetUp != null)
						{
							DomainSetUp = null;
						}
						if (thBootStrapDomain != null)
						{
							thBootStrapDomain = null;
						}
						if (thSuperNodeDomain != null)
						{
							thSuperNodeDomain = null;
						}
					}
					catch (Exception ex)
					{
                        VMuktiHelper.ExceptionHandler(ex, "Dispose()", "App.xaml.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		~App()
		{
            try
            {
			Dispose(false);
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~App()", "App.xaml.cs");
            }

        }
		#endregion

        #region Diaster Recovery for Node

        void App_VMuktiEvent_GetSuperNodeIP(object sender, VMuktiEventArgs e)
        {
            //ClsException.WriteToLogFile("calling WebReqSuperNode  at " + DateTime.Now.ToString());
            if (!WebReqSuperNode())
            {
                //ClsException.WriteToLogFile("WebReqSuperNode result is false ");
                string PreviousSuperNode = VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP;

                string SuperNodeName = null;
                if (PreviousSuperNode != null || PreviousSuperNode != string.Empty)
                {
                    try
                    {
                        //ClsException.WriteToLogFile("calling bs http  svcGetNodeNameByIP");
                        SuperNodeName = App.chHttpBootStrapService.svcGetNodeNameByIP(PreviousSuperNode);
                        /********** SuperNode Down *************/
                        //ClsException.WriteToLogFile("called bs http  svcGetNodeNameByIP");
                        //ClsException.WriteToLogFile("calling bs http svcHttpBsGetSuperNodeIP");
                        clsSuperNodeDataContract objSuperNodeDataContract = App.chHttpBootStrapService.svcHttpBsGetSuperNodeIP(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, false);
                        //ClsException.WriteToLogFile("called bs http svcHttpBsGetSuperNodeIP");
                        VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = objSuperNodeDataContract.SuperNodeIP;


                        //ClsException.WriteToLogFile("calling Stop timer with true");
                        VMuktiAPI.VMuktiHelper.CallEvent("StopTimer", null, new VMuktiEventArgs(true));
                        //ClsException.WriteToLogFile("Stop timer called with true");


                        App.chHttpSuperNodeService = (IHttpSuperNodeService)App.objHttpSuperNode.OpenClient<IHttpSuperNodeService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                        App.chHttpSuperNodeService.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());                        
                        

                        if (SuperNodeName != null || SuperNodeName != string.Empty)
                        {
                            App.chHttpSuperNodeService.svcGetNodeNameByIP(SuperNodeName, PreviousSuperNode);
                        }

                        //ClsException.WriteToLogFile("calling Stop timer with false");
                        VMuktiAPI.VMuktiHelper.CallEvent("StopTimer", null,new VMuktiEventArgs(false));
                        //ClsException.WriteToLogFile("Stop timer called with false");
                    }
                    catch(Exception exp)
                    {
                        /********* Internet of Node is Down *********/
                        /********* Start Healing Component for Node *********/
                        //ClsException.WriteToLogFile("IN EXCEPTION " + exp.Message.ToString());
                        VMuktiHelper.CallEvent("StopTimer", null, new VMuktiEventArgs(true));
                        //ClsException.WriteToLogFile("starting timer to ping supernode");
                        dtWebReqBS.Start();
                        //ClsException.WriteToLogFile("started timer to ping supernode");
                        blnNodeOff = true;
                        return;
                    }
                }
            }
        }

        bool WebReqSuperNode()
        {
            Uri objuri = new Uri("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + "80/HttpSuperNode");
            System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(objuri);
            System.Net.WebResponse objResp;
            try
            {
                objResp = objWebReq.GetResponse();
                objResp.Close();
                return true;
            }
            catch
            {               
                return false;
            }
        }

        public static void OpenBootStrapHttpClients()
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Initializing: Opening Bootstrap client ");
                sb.AppendLine("Client Is " + VMuktiAPI.VMuktiInfo.BootStrapIPs[0]);
                sb.AppendLine(sb1.ToString());
                ClsLogging.WriteToTresslog(sb);
                chHttpBootStrapService = (IHTTPBootStrapService)objHttpBootStrap.OpenClient<IHTTPBootStrapChannel>("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");
                clsBootStrapInfo objBootStrapInfo = null;
                objBootStrapInfo = App.chHttpBootStrapService.svcHttpBSJoin("", null);

                VMuktiAPI.VMuktiInfo.MainConnectionString = objBootStrapInfo.ConnectionString;

                switch (objBootStrapInfo.AuthType)
                {
                    case "SIPAuthentication":
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SIPAuthentication;
                        break;

                    case "SQLAuthentication":
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SQLAuthentication;
                        break;

                    default:
                        VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.NotDecided;
                        break;

                }
                // ** Client Open Sucessfully
                StringBuilder sb3 = new StringBuilder();
                sb3.AppendLine("Initializing: Client Open Sucessfully ");
                sb3.AppendLine("BootStrap Client Is: " + VMuktiAPI.VMuktiInfo.BootStrapIPs[0]);
                //sb3.AppendLine("Main Connection string is : " + VMuktiAPI.VMuktiInfo.MainConnectionString);
                sb3.AppendLine("Authentation type is : " + objBootStrapInfo.AuthType);
                sb3.AppendLine(sb1.ToString());
                //ClsLogging.WriteToTresslog(sb3);

                bool isSuperNode = false;
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                {
                    isSuperNode = false;
                }
                else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                {
                    isSuperNode = true;
                }

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == null || VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == string.Empty)
                {
                    VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;
                }


                 while (true)
                {
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sReader = new StreamReader(fs);
                    string strStatus = sReader.ReadToEnd();
                    sReader.Close();
                    fs.Close();
                    if (strStatus == "Initializing")
                    {
                        clsSuperNodeDataContract objSuperNodeDataContract = App.chHttpBootStrapService.svcHttpBsGetSuperNodeIP(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, isSuperNode);
                        VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = objSuperNodeDataContract.SuperNodeIP;

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP == null || VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP == string.Empty)
                        {
                            //ClsException.WriteToLogFile("Somehow supernode is not available so assigning bootstrap IP");
                            VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = VMuktiAPI.VMuktiInfo.BootStrapIPs[0];
                        }

                        //ClsException.WriteToLogFile("My SuperNode IP is :: " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        OpenSuperNodeClients();
                        break;
                    }
                 }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OpenBootStrapHttpClients()", "App.xaml.cs");
            }
        }

        public static void OpenSuperNodeClients()
        {
            try
            {
                App.chHttpSuperNodeService = (IHttpSuperNodeService)App.objHttpSuperNode.OpenClient<IHttpSuperNodeService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                App.chHttpSuperNodeService.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
                //ClsException.WriteToLogFile("SvcJoin of SuperNode called for Disaster Recovery at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OpenSuperNodeClients()", "App.xaml.cs");
            }
        }

        void dtWebReqBS_Tick(object sender, EventArgs e)
        {
            //ClsException.WriteToLogFile("Uri formation to ping to bs");
            Uri objuri = new Uri("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":" + "80/HttpSuperNode");
            System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(objuri);
            System.Net.WebResponse objResp;
            try
            {
                objWebReq.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                //ClsException.WriteToLogFile("requesting the bs for response");
                objResp = objWebReq.GetResponse();
                //ClsException.WriteToLogFile("got the response from bs");
                objResp.Close();
                //ClsException.WriteToLogFile("NetAvail(dtWebReqBS_Tick--1--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);

                if (NetAvail)
                {
                    //ClsException.WriteToLogFile("NetAvail(dtWebReqBS_Tick--2--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
                    NetAvail = false;
                    //ClsException.WriteToLogFile("NetAvail(dtWebReqBS_Tick--3--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
                    dtWebReqBS.Stop();
                    //ClsException.WriteToLogFile("opening bs clients after net retrival");
                    //OpenBootStrapHttpClients();
                    bgOpenHttpBootStrapClient.RunWorkerAsync();
                    if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService = null;
                        VMukti.Business.clsDataBaseChannel.OpenDataBaseClient();
                    }
                    //ClsException.WriteToLogFile("opened bs clients after net retrival");
                    //ClsException.WriteToLogFile("calling stop timer with false");
                    VMuktiAPI.VMuktiHelper.CallEvent("StopTimer", null, new VMuktiEventArgs(false));
                    //ClsException.WriteToLogFile("called stop timer with false");
                    blnNodeOff = false;
                }
                else if (blnNodeOff)
                {
                    NetAvail = false;
                    dtWebReqBS.Stop();
                    //ClsException.WriteToLogFile("opening bs clients after net retrival with node off");
                    //OpenBootStrapHttpClients();
                    bgOpenHttpBootStrapClient.RunWorkerAsync();
                    if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService = null;
                        VMukti.Business.clsDataBaseChannel.OpenDataBaseClient();
                    }
                    //ClsException.WriteToLogFile("opened bs clients after net retrival with node off");
                    //ClsException.WriteToLogFile("calling stop timer with false n with node off");
                    VMuktiAPI.VMuktiHelper.CallEvent("StopTimer", null, new VMuktiEventArgs(false));
                    //ClsException.WriteToLogFile("called stop timer with false n with node off");
                    blnNodeOff = false;
                }
            }
            catch
            {                
                NetAvail = true;
                //ClsException.WriteToLogFile("NetAvail(dtWebReqBS_Tick--4--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
            }
        }

        #endregion
	}
}
