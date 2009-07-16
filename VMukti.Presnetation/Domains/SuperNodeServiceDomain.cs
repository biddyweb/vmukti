
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServerCe;
using VMukti.Business;
using VMukti.Business.CommonDataContracts;
using VMukti.Business.WCFServices.BootStrapServices.BasicHttp;
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using VMukti.Business.WCFServices.BootStrapServices.NetP2P;
using VMukti.Business.WCFServices.SuperNodeServices.BasicHttp;
using VMukti.Business.WCFServices.SuperNodeServices.NetP2P;
using VMukti.Presentation.Xml;
using VMuktiAPI;
using VMuktiService;
using System.Data.SqlClient;
using System.ServiceModel;
using VMukti.Bussiness.WCFServices.BootStrapServices.NetP2P;
using VMukti.Business.CommonMessageContract;
using System.Text;
using System.Windows.Threading;
using System.Timers;

namespace VMukti.Presentation
{
    public class SuperNodeServiceDomain : IDisposable
    {
        object objHttpSuperNode;
        object objHttpDatabaseSuperNode;
        NetPeerServer npsSuperNodeServer;

        //List<NetPeerServer> npsModules = new List<NetPeerServer>();
        //public static List<clsNodeInfo> lstmyNodes = new List<clsNodeInfo>();
        
        List<NetPeerServer> npsModules;
        public static List<clsNodeInfo> lstmyNodes;
        FileStream objFileStream;
        string[] strBuddyName;

        int intModule;

        object objNetP2PBootStrapPredictive;
        INetP2PBootStrapPreditiveService ClientNetP2PPredictiveChannel;

        object objNetP2PBootStrap;
        INetP2PBootStrapChannel clientNetP2pChannelBS;

        public static IHTTPBootStrapService clientHttpChannelBS;
        public static clsBootStrapInfo objSIPInformation=null;

        object objNetP2PSuperNode;
        public INetP2PSuperNodeChannel clientNetP2PChannelSN;

        string strMessageFor;

        //List<string> lstSchmaInformation = new List<string>();
        List<string> lstSchmaInformation;

        bool blIsSNListPresent;
        //public ArrayList al = new ArrayList();
        public ArrayList al;
        Assembly a;
        Assembly ass;
        Assembly assDownload;
        //private List<clsRTCAuthClient> lstObjRTCAuthClient = new List<clsRTCAuthClient>();
        private List<clsRTCAuthClient> lstObjRTCAuthClient;
        //List<object> lstObjSuperNode = new List<object>();
        List<object> lstObjSuperNode;


        SqlCeConnection LocalSQLConn;
        //string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "SuperNodeBuddyInfo.sdf";
        //string strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "SuperNodeBuddyInfo.sdf";

        string ClientConnectionString;
        string strLocalDBPath;
        #region BuddyManagement

        //List<string> lstOfflinebuddies = new List<string>();
        List<string> lstOfflinebuddies;
        #endregion

        #region Disaster Recovery

        static BasicHttpServer bhsHttpSuperNode;
        BasicHttpServer bhsHttpFileUplaodDownload;
        NetPeerClient npcBootStrapClient;
        NetPeerClient npcSuperNode;
        BasicHttpClient bhcBootStrap;
        NetPeerClient npcBootStrapPredictiveClient;

        bool NetAvail;
        //Timer dtWebReqBS4SN = new Timer(15000);
        //Timer timerBuzzSN = new Timer(18000);
        //Timer timerBuddylist = new Timer(120000);


        Timer dtWebReqBS4SN;
        Timer timerBuzzSN;
        Timer timerBuddylist;
        #endregion

        #region Bandwidth

        //string uploadPath = AppDomain.CurrentDomain.BaseDirectory + "UploadDownload";
        string uploadPath;
        object objFileUploadDownload;

        #endregion

        #region This Code has been added by Nisarg
        //YatePBX.Presentation.YatePBX objPBX = new YatePBX.Presentation.YatePBX();
        YatePBX.Presentation.YatePBX objPBX;
        #endregion


        public SuperNodeServiceDomain()
        {
            try
            {
                #region Initialize global Variables
                npsModules = new List<NetPeerServer>();
                lstmyNodes = new List<clsNodeInfo>();
                intModule = 1;
                lstSchmaInformation = new List<string>();
                al = new ArrayList();
                lstObjRTCAuthClient = new List<clsRTCAuthClient>();
                lstObjSuperNode = new List<object>();
                NetAvail = false;
                blIsSNListPresent = false;

                dtWebReqBS4SN = new Timer(15000);
                timerBuzzSN = new Timer(18000);
                timerBuddylist = new Timer(120000);

                uploadPath = AppDomain.CurrentDomain.BaseDirectory + "UploadDownload";

                ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "SuperNodeBuddyInfo.sdf";
                strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "SuperNodeBuddyInfo.sdf";
                lstOfflinebuddies = new List<string>();

                objPBX = new YatePBX.Presentation.YatePBX();
                #endregion


                #region Disaster Recovery

                timerBuzzSN.Elapsed += new ElapsedEventHandler(timerBuzzSN_Elapsed);
                dtWebReqBS4SN.Elapsed += new System.Timers.ElapsedEventHandler(dtWebReqBS4SN_Elapsed);
                timerBuddylist.Elapsed += new ElapsedEventHandler(timerBuddylist_Elapsed);
                #endregion

                if (VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                {
                    if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceme35.dll"))
                    {
                        new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceme35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceme35.dll");
                        new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceqp35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceqp35.dll");
                        new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlcese35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlcese35.dll");
                    }

                    if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "SuperNodeBuddyInfo.sdf"))
                    {
                        SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                        clientEngine.CreateDatabase();

                        LocalSQLConn = new SqlCeConnection();
                        LocalSQLConn.ConnectionString = ClientConnectionString;
                        LocalSQLConn.Open();


                        fncCreateBuddyStatusTable();
                        fncCreateSNNodeInfoTable();
                        fncCreateUserBuddyListTable();
                    }
                    else
                    {
                        LocalSQLConn = new SqlCeConnection();
                        LocalSQLConn.ConnectionString = ClientConnectionString;
                        LocalSQLConn.Open();

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                        {
                            fncSNInsertBuddy(VMuktiAPI.VMuktiInfo.BootStrapIPs[0], "Online");
                        }
                    }


                    #region NetP2PServerHoster
                    try
                    {
                        npsSuperNodeServer = new NetPeerServer("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode");
                        npsSuperNodeServer.AddEndPoint("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode");
                        npsSuperNodeServer.OpenServer();
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()", "Domains\\SuperNodeServiceDomain.cs");
                    }
                    #endregion

                    #region Client

                    #region P2PBootStrap Client

                    try
                    {
                        npcBootStrapClient = new NetPeerClient();

                        NetP2PBootStrapDelegates objP2PBSDel = new NetP2PBootStrapDelegates();

                        objP2PBSDel.EntsvcNetP2PAddUser += new NetP2PBootStrapDelegates.DelsvcNetP2PAddUser(SuperNodeServiceDomain_EntsvcNetP2PAddUser);
                        objP2PBSDel.EntsvcNetP2PGetBuddyInfo += new NetP2PBootStrapDelegates.DelsvcNetP2PGetBuddyInfo(SuperNodeServiceDomain_EntsvcNetP2PGetBuddyInfo);
                        objP2PBSDel.EntsvcNetP2PGetSuperNodeBuddyStatus += new NetP2PBootStrapDelegates.DelsvcNetP2PGetSuperNodeBuddyStatus(SuperNodeServiceDomain_EntsvcNetP2PGetSuperNodeBuddyStatus);
                        objP2PBSDel.EntsvcNetP2PRemoveUser += new NetP2PBootStrapDelegates.DelsvcNetP2PRemoveUser(SuperNodeServiceDomain_EntsvcNetP2PRemoveUser);
                        objP2PBSDel.EntsvcNetP2PReturnBuddyInfo += new NetP2PBootStrapDelegates.DelsvcNetP2PReturnBuddyInfo(SuperNodeServiceDomain_EntsvcNetP2PReturnBuddyInfo);
                        objP2PBSDel.EntsvcNetP2PAddBuddies += new NetP2PBootStrapDelegates.DelsvcNetP2PAddBuddies(SuperNodeServiceDomain_EntsvcNetP2PAddBuddies);
                        objP2PBSDel.EntsvcNetP2PRemoveBuddies += new NetP2PBootStrapDelegates.DelsvcNetP2PRemoveBuddies(SuperNodeServiceDomain_EntsvcNetP2PRemoveBuddies);

                        objP2PBSDel.EntsvcNetP2PReturnSuperNodeBuddyStatus += new NetP2PBootStrapDelegates.DelsvcNetP2PReturnSuperNodeBuddyStatus(SuperNodeServiceDomain_EntsvcNetP2PReturnSuperNodeBuddyStatus);
                        objP2PBSDel.EntsvcNetP2PServiceJoin += new NetP2PBootStrapDelegates.DelsvcNetP2PServiceJoin(SuperNodeServiceDomain_EntsvcNetP2PServiceJoin);
                        objP2PBSDel.EntsvcNetP2PSetSpecialMsg += new NetP2PBootStrapDelegates.DelsvcNetP2PSetSpecialMsg(SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsg);
                        objP2PBSDel.EntsvcNetP2PSetSpecialMsg4MultipleBuddies += new NetP2PBootStrapDelegates.DelsvcNetP2PSetSpecialMsg4MultipleBuddies(SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsg4MultipleBuddies);
                        objP2PBSDel.EntsvcNetP2PSetSpecialMsgBuddiesClick += new NetP2PBootStrapDelegates.DelsvcNetP2PSetSpecialMsgBuddiesClick(SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsgBuddiesClick);
                        objP2PBSDel.EntsvcNetP2PPageSetSpecialMsg += new NetP2PBootStrapDelegates.DelsvcNetP2PPageSetSpecialMsg(SuperNodeServiceDomain_EntsvcNetP2PPageSetSpecialMsg);
                        objP2PBSDel.EntsvcPageBuddyRetSetSpecialMsg += new NetP2PBootStrapDelegates.DelsvcPageBuddyRetSetSpecialMsg(SuperNodeServiceDomain_EntsvcPageBuddyRetSetSpecialMsg);

                        objP2PBSDel.EntsvcAddDraggedBuddy += new NetP2PBootStrapDelegates.DelsvcAddDraggedBuddy(SuperNodeServiceDomain_EntsvcAddDraggedBuddy);
                        objP2PBSDel.EntsvcAddPageDraggedBuddy += new NetP2PBootStrapDelegates.DelsvcAddPageDraggedBuddy(SuperNodeServiceDomain_EntsvcAddPageDraggedBuddy);
                        objP2PBSDel.EntsvcSetRemoveDraggedBuddy += new NetP2PBootStrapDelegates.DelsvcSetRemoveDraggedBuddy(SuperNodeServiceDomain_EntsvcSetRemoveDraggedBuddy);
                        objP2PBSDel.EntsvcNetP2PUnJoin += new NetP2PBootStrapDelegates.DelsvcNetP2PUnJoin(SuperNodeServiceDomain_EntsvcNetP2PUnJoin);

                        objP2PBSDel.EntsvcBuzzSuperNode += new NetP2PBootStrapDelegates.DelsvcBuzzSuperNode(SuperNodeServiceDomain_EntsvcBuzzSuperNode);
                        objP2PBSDel.EntsvcSetNodeStatus += new NetP2PBootStrapDelegates.DelsvcSetNodeStatus(SuperNodeServiceDomain_EntsvcSetNodeStatus);
                        objP2PBSDel.EntsvcJoinConf += new NetP2PBootStrapDelegates.DelsvcJoinConf(SuperNodeServiceDomain_EntsvcJoinConf);
                        objP2PBSDel.EntsvcSendConfInfo += new NetP2PBootStrapDelegates.DelsvcSendConfInfo(SuperNodeServiceDomain_EntsvcSendConfInfo);
                        objP2PBSDel.EntsvcAddConfBuddy += new NetP2PBootStrapDelegates.DelsvcAddConfBuddy(SuperNodeServiceDomain_EntsvcAddConfBuddy);
                        objP2PBSDel.EntsvcRemoveConf += new NetP2PBootStrapDelegates.DelsvcRemoveConf(SuperNodeServiceDomain_EntsvcRemoveConf);
                        objP2PBSDel.EntsvcUnJoinConf += new NetP2PBootStrapDelegates.DelsvcUnJoinConf(SuperNodeServiceDomain_EntsvcUnJoinConf);
                        objP2PBSDel.EntsvcEnterConf += new NetP2PBootStrapDelegates.DelsvcEnterConf(SuperNodeServiceDomain_EntsvcEnterConf);
                        objP2PBSDel.EntsvcPodNavigation += new NetP2PBootStrapDelegates.DelsvcPodNavigation(SuperNodeServiceDomain_EntsvcPodNavigation);
                        objNetP2PBootStrap = objP2PBSDel;



                        clientNetP2pChannelBS = (INetP2PBootStrapChannel)npcBootStrapClient.OpenClient<INetP2PBootStrapChannel>("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrap", "P2PBootStrapMesh", ref objNetP2PBootStrap);
                        clientNetP2pChannelBS.svcNetP2PServiceJoin(VMuktiInfo.CurrentPeer.SuperNodeIP);

                        //IOnlineStatus ostat = clientNetP2pChannelBS.GetProperty<IOnlineStatus>();
                        //ostat.Online += new EventHandler(ostat_Online);
                        //ostat.Offline += new EventHandler(ostat_Offline);

                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()--1", "Domains\\SuperNodeServiceDomain.cs");
                        MessageBox.Show("Could not connect to Server please try after some time1");
                        Environment.Exit(0);
                    }
                    #endregion

                    #region P2PSuperNode Client

                    try
                    {
                        npcSuperNode = new NetPeerClient();
                        NetP2PSuperNodeDelegates objP2PSNDel = new NetP2PSuperNodeDelegates();

                        objP2PSNDel.EntsvcJoin += new NetP2PSuperNodeDelegates.DelsvcJoin(NetP2PSNDomain_EntsvcJoin);
                        objP2PSNDel.EntsvcSetSpecialMsg += new NetP2PSuperNodeDelegates.DelsvcSetSpecialMsg(NetP2PSNDomain_EntsvcSetSpecialMsg);
                        objP2PSNDel.EntsvcSetSpecialMsg4MultipleBuddies += new NetP2PSuperNodeDelegates.DelsvcSetSpecialMsg4MultipleBuddies(NetP2PSNDomain_EntsvcSetSpecialMsg4MultipleBuddies);
                        objP2PSNDel.EntsvcSetSpecialMsgBuddiesClick += new NetP2PSuperNodeDelegates.DelsvcSetSpecialMsgBuddiesClick(NetP2PSNDomain_EntsvcSetSpecialMsgBuddiesClick);
                        objP2PSNDel.EntsvcPageSetSpecialMsg += new NetP2PSuperNodeDelegates.DelsvcPageSetSpecialMsg(NetP2PSNDomain_EntsvcPageSetSpecialMsg);
                        objP2PSNDel.EntsvcPageBuddyRetSetSpecialMsg += new NetP2PSuperNodeDelegates.DelsvcPageBuddyRetSetSpecialMsg(NetP2PSNDomain_EntsvcPageBuddyRetSetSpecialMsg);

                        objP2PSNDel.EntsvcAddDraggedBuddy += new NetP2PSuperNodeDelegates.DelsvcAddDraggedBuddy(NetP2PSNDomain_EntsvcAddDraggedBuddy);
                        objP2PSNDel.EntsvcAddPageDraggedBuddy += new NetP2PSuperNodeDelegates.DelsvcAddPageDraggedBuddy(NetP2PSNDomain_EntsvcAddPageDraggedBuddy);
                        objP2PSNDel.EntsvcSetRemoveDraggedBuddy += new NetP2PSuperNodeDelegates.DelsvcSetRemoveDraggedBuddy(NetP2PSNDomain_EntsvcSetRemoveDraggedBuddy);
                        objP2PSNDel.EntsvcJoinConf += new NetP2PSuperNodeDelegates.DelsvcJoinConf(NetP2PSNDomain_EntsvcJoinConf);
                        objP2PSNDel.EntsvcSendConfInfo += new NetP2PSuperNodeDelegates.DelsvcSendConfInfo(NetP2PSNDomain_EntsvcSendConfInfo);
                        objP2PSNDel.EntsvcUnJoinConf += new NetP2PSuperNodeDelegates.DelsvcUnJoinConf(NetP2PSNDomain_EntsvcUnJoinConf);
                        objP2PSNDel.EntsvcAddConfBuddy += new NetP2PSuperNodeDelegates.DelsvcAddConfBuddy(NetP2PSNDomain_EntsvcAddConfBuddy);
                        objP2PSNDel.EntsvcRemoveConf += new NetP2PSuperNodeDelegates.DelsvcRemoveConf(NetP2PSNDomain_EntsvcRemoveConf);
                        objP2PSNDel.EntsvcEnterConf += new NetP2PSuperNodeDelegates.DelsvcEnterConf(NetP2PSNDomain_EntsvcEnterConf);
                        objP2PSNDel.EntsvcPodNavigation += new NetP2PSuperNodeDelegates.DelsvcPodNavigation(NetP2PSNDomain_EntsvcPodNavigation);
                        objP2PSNDel.EntsvcUnJoin += new NetP2PSuperNodeDelegates.DelsvcUnJoin(NetP2PSNDomain_EntsvcUnJoin);

                        objNetP2PSuperNode = objP2PSNDel;

                        clientNetP2PChannelSN = (INetP2PSuperNodeChannel)npcSuperNode.OpenClient<INetP2PSuperNodeChannel>("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode", "P2PSuperNodeMesh", ref objNetP2PSuperNode);
                        clientNetP2PChannelSN.svcJoin("Server");
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SupernodeServiceDomain()--P2PSuperNodeClient", "Domains\\SuperNodeServiceDomain.cs");
                    }
                    #endregion

                    #region HttpBootStrap Client

                    bhcBootStrap = new BasicHttpClient();
                    clientHttpChannelBS = (IHTTPBootStrapService)bhcBootStrap.OpenClient<IHTTPBootStrapService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");

                    clsPeerInfo objPeerInfo = new clsPeerInfo();
                    objPeerInfo.objPeerInforation = VMuktiInfo.CurrentPeer.GetPeerDataContract();
                    try
                    {
                        objSIPInformation = clientHttpChannelBS.svcHttpBSJoin(VMuktiInfo.CurrentPeer.DisplayName, objPeerInfo);
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()--2", "Domains\\SuperNodeServiceDomain.cs");
                        MessageBox.Show("Could not connect to Server please try after some time2");
                        Environment.Exit(0);
                    }
                    if (objSIPInformation != null)
                    {
                        if (objSIPInformation.ConnectionString != "")
                        {
                            VMuktiAPI.VMuktiInfo.MainConnectionString = objSIPInformation.ConnectionString;
                        }
                        if (objSIPInformation.AuthType == "NotDecided")
                        {
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.NotDecided;
                        }
                        else if (objSIPInformation.AuthType == "SQLAuthentication")
                        {
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SQLAuthentication;
                        }
                        else if (objSIPInformation.AuthType == "SIPAuthentication")
                        {

                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SIPAuthentication;
                        }
                    }

                    try
                    {
                        VMukti.Business.clsDataBaseChannel.OpenDataBaseClient();
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()--3", "Domains\\SuperNodeServiceDomain.cs");
                    }


                    System.Threading.Thread thstart = new System.Threading.Thread(new System.Threading.ThreadStart(DownloadZipFiles));
                    thstart.Start();
                    #endregion

                    #region WCF Servers For Call Center
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion.ToString() == "1.1")
                    {
                        #region P2PBootStrapPredictive Client

                        npcBootStrapPredictiveClient = new NetPeerClient();

                        NetP2PBootStrapPredictiveDelegate objP2PBSDel = new NetP2PBootStrapPredictiveDelegate();


                        objP2PBSDel.EntsvcJoin += new NetP2PBootStrapPredictiveDelegate.DelsvcJoin(SuperNodeServiceDomain_EntsvcJoin);
                        objP2PBSDel.EntAddExtraCall += new NetP2PBootStrapPredictiveDelegate.DelsvcAddExtraCall(SuperNodeServiceDomain_EntAddExtraCall);
                        objP2PBSDel.EntRequestExtraCall += new NetP2PBootStrapPredictiveDelegate.DelsvcRequestExtraCall(SuperNodeServiceDomain_EntRequestExtraCall);
                        objP2PBSDel.EntSendExtraCall += new NetP2PBootStrapPredictiveDelegate.DelsvcSendExtraCall(SuperNodeServiceDomain_EntSendExtraCall);
                        objP2PBSDel.EntRemoveExtraCall += new NetP2PBootStrapPredictiveDelegate.DelsvcRemoveExtraCall(SuperNodeServiceDomain_EntRemoveExtraCall);
                        objP2PBSDel.EntRequestFunctionToExecute += new NetP2PBootStrapPredictiveDelegate.DelsvcRequestFunctionToExecute(SuperNodeServiceDomain_EntRequestFunctionToExecute);
                        objP2PBSDel.EntReplyFunctionExecuted += new NetP2PBootStrapPredictiveDelegate.DelsvcReplyFunctionExecuted(SuperNodeServiceDomain_EntReplyFunctionExecuted);
                        objP2PBSDel.EntHangUpCall += new NetP2PBootStrapPredictiveDelegate.DelsvcHangUpCall(SuperNodeServiceDomain_EntHangUpCall);
                        objP2PBSDel.EntUnJoin += new NetP2PBootStrapPredictiveDelegate.DelsvcUnJoin(SuperNodeServiceDomain_EntUnJoin);

                        objNetP2PBootStrapPredictive = objP2PBSDel;

                        ClientNetP2PPredictiveChannel = (INetP2PBootStrapPreditiveService)npcBootStrapPredictiveClient.OpenClient<INetP2PBootStrapPreditiveService>("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapPredictive", "P2PBootStrapPredictiveMesh", ref objNetP2PBootStrapPredictive);
                        ClientNetP2PPredictiveChannel.svcJoin("", "");
                        #endregion

                    }
                    #endregion

                    #endregion

                    #region HttpServerHoster


                    try
                    {
                        SuperNodeDelegates objSNDel = new SuperNodeDelegates();
                        objSNDel.EntHttpAddBuddies += new SuperNodeDelegates.DelHttpAddBuddies(SuperNodeServiceDomain_EntHttpAddBuddies);
                        objSNDel.EntHttpRemoveBuddies += new SuperNodeDelegates.DelHttpRemoveBuddies(SuperNodeServiceDomain_EntHttpRemoveBuddies);
                        objSNDel.EntHTTPAddNodeBuddies += new SuperNodeDelegates.DelHTTPAddNodeBuddies(SuperNodeServiceDomain_EntHTTPAddNodeBuddies);


                        objSNDel.EntHttpGetBuddies += new SuperNodeDelegates.DelHttpGetBuddies(SuperNodeServiceDomain_EntHttpGetBuddies);
                        objSNDel.EntHttpsvcGetBuddyStatus += new SuperNodeDelegates.DelHttpsvcGetBuddyStatus(SuperNodeServiceDomain_EntHttpsvcGetBuddyStatus);
                        objSNDel.EntHttpsvcjoin += new SuperNodeDelegates.DelHttpsvcJoin(SuperNodeServiceDomain_EntHttpsvcjoin);
                        objSNDel.EntHttpsvcUnjoin += new SuperNodeDelegates.DelHttpsvcUnjoin(SuperNodeServiceDomain_EntHttpsvcUnjoin);
                        objSNDel.EntIsSuperNodeAvailable += new SuperNodeDelegates.DelIsSuperNodeAvailable(SuperNodeServiceDomain_EntIsSuperNodeAvailable);
                        objSNDel.EntsvcGetSpecialMsgs += new SuperNodeDelegates.DelsvcGetSpecialMsgs(SuperNodeServiceDomain_EntsvcGetSpecialMsgs);
                        objSNDel.EntsvcSetSpecialMsg += new SuperNodeDelegates.DelsvcSetSpecialMsgs(SuperNodeServiceDomain_EntsvcSetSpecialMsg);
                        objSNDel.EntsvcPageSetSpecialMsg += new SuperNodeDelegates.DelsvcPageSetSpecialMsgs(SuperNodeServiceDomain_EntsvcPageSetSpecialMsg);
                        objSNDel.EntsvcSetSpecialMsg4MultipleBuddies += new SuperNodeDelegates.DelsvcSetSpecialMsgs4MultipleBuddies(SuperNodeServiceDomain_EntsvcSetSpecialMsg4MultipleBuddies);
                        objSNDel.EntsvcSetSpecialMsgBuddiesClick += new SuperNodeDelegates.DelsvcSetSpecialMsgsBuddiesClick(SuperNodeServiceDomain_EntsvcSetSpecialMsgBuddiesClick);
                        objSNDel.EntsvcAddDraggedBuddy += new SuperNodeDelegates.DelsvcAddDraggedBuddy(SuperNodeServiceDomain_EntsvcAddDraggedBuddy);
                        objSNDel.EntsvcAddPageDraggedBuddy += new SuperNodeDelegates.DelsvcAddPageDraggedBuddy(SuperNodeServiceDomain_EntsvcAddPageDraggedBuddy);
                        objSNDel.EntsvcSetRemoveDraggedBuddy += new SuperNodeDelegates.DelsvcSetRemoveDraggedBuddy(SuperNodeServiceDomain_EntsvcSetRemoveDraggedBuddy);
                        objSNDel.EntsvcStartAService += new SuperNodeDelegates.DelsvcStartAService(SuperNodeServiceDomain_EntsvcStartAService);
                        objSNDel.EntIsAutherizedUser += new SuperNodeDelegates.DelIsAutherizedUser(SuperNodeServiceDomain_EntIsAuthorizedUser);
                        objSNDel.EntIsAutherized += new SuperNodeDelegates.DelIsAutherized(SuperNodeServiceDomain_EntIsAuthorized);
                        objSNDel.EntsvcAddSIPUser += new SuperNodeDelegates.DelsvcAddSIPUser(SuperNodeServiceDomain_EntsvcAddSIPUser);
                        objSNDel.EntsvcGetConferenceNumber += new SuperNodeDelegates.DelsvcGetConferenceNumber(SuperNodeServiceDomain_EntsvcGetConferenceNumber);
                        objSNDel.EntsvcRemoveSIPUser += new SuperNodeDelegates.DelsvcRemoveSIPUser(SuperNodeServiceDomain_EntsvcRemoveSIPUser);

                        objSNDel.EntsvcGetNodeNameByIP += new SuperNodeDelegates.DelsvcGetNodeNameByIP(SuperNodeServiceDomain_EntsvcGetNodeNameByIP);
                        objSNDel.EntsvcJoinConf += new SuperNodeDelegates.DelsvcJoinConf(objSNDel_EntsvcJoinConf);
                        objSNDel.EntsvcSendConfInfo += new SuperNodeDelegates.DelsvcSendConfInfo(objSNDel_EntsvcSendConfInfo);
                        objSNDel.EntsvcAddConfBuddy += new SuperNodeDelegates.DelsvcAddConfBuddy(objSNDel_EntsvcAddConfBuddy);
                        objSNDel.EntsvcRemoveConf += new SuperNodeDelegates.DelsvcRemoveConf(objSNDel_EntsvcRemoveConf);
                        objSNDel.EntsvcUnJoinConf += new SuperNodeDelegates.DelsvcUnJoinConf(objSNDel_EntsvcUnJoinConf);
                        objSNDel.EntsvcEnterConf += new SuperNodeDelegates.DelsvcEnterConf(objSNDel_EntsvcEnterConf);
                        objSNDel.EntsvcPodNavigation += new SuperNodeDelegates.DelsvcPodNavigation(objSNDel_EntsvcPodNavigation);

                        objHttpSuperNode = objSNDel;

                        bhsHttpSuperNode = new BasicHttpServer(ref objHttpSuperNode, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                        bhsHttpSuperNode.AddEndPoint<IHttpSuperNodeService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                        bhsHttpSuperNode.OpenServer();
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomian()--HttpSuperNode", "Domains\\SuperNodeServiceDomain.cs");
                    }


                    //#region HttpDataBase Server

                    //try
                    //{
                    //    DataBaseDelegates objSNDel = new DataBaseDelegates();
                    //    objSNDel.EntHttpsvcjoin += new DataBaseDelegates.DelHttpsvcJoin(SuperNodeServiceDomain_EntHttpsvcjoin);
                    //    objSNDel.EntHttpExecuteDataSet += new DataBaseDelegates.DelHttpExecuteDataSet(SuperNodeServiceDomain_EntHttpExecuteDataSet);
                    //    objSNDel.EntHttpExecuteStoredProcedure += new DataBaseDelegates.DelHttpExecuteStoredProcedure(SuperNodeServiceDomain_EntHttpExecuteStoredProcedure);
                    //    objSNDel.EntHttpExecuteNonQuery += new DataBaseDelegates.DelHttpExecuteNonQuery(SuperNodeServiceDomain_EntHttpExecuteNonQuery);
                    //    objSNDel.EntHttpExecuteReturnNonQuery += new DataBaseDelegates.DelHttpExecuteReturnNonQuery(SuperNodeServiceDomain_EntHttpExecuteReturnNonQuery);

                    //    objHttpDatabaseSuperNode = objSNDel;

                    //    bhsHttpDataBase = new BasicHttpServer(ref objHttpDatabaseSuperNode, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpDataBase");
                    //    bhsHttpDataBase.AddEndPoint<IHttpDataBaseService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpDataBase");
                    //    bhsHttpDataBase.OpenServer();
                    //}
                    //catch (Exception ex)
                    //{
                    //    VMuktiHelper.ExceptionHandler(ex, "SuperNoeServiceDomain()--HttpDataBase Server()", "Domains\\SuperNodeServiceDomain.cs");
                    //}

                    //#endregion

                    #region BandwidthServer

                    try
                    {
                        if (!System.IO.Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "UploadDownload");
                        }
                        FileUploadDownloadDelegates objFileUploadDownloadDel = new FileUploadDownloadDelegates();
                        objFileUploadDownloadDel.entsvcJoin += new FileUploadDownloadDelegates.delsvcJoin(SuperNodeServiceDomain_entsvcJoin);
                        objFileUploadDownloadDel.entsvcDownload += new FileUploadDownloadDelegates.delDownloadFile(SuperNodeServiceDomain_entsvcDownload);
                        objFileUploadDownloadDel.entsvcUpload += new FileUploadDownloadDelegates.delUploadFile(SuperNodeServiceDomain_entsvcUpload);

                        objFileUploadDownload = objFileUploadDownloadDel;

                        bhsHttpFileUplaodDownload = new BasicHttpServer(ref objFileUploadDownload, "http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/HttpFileUploadDownload");
                        bhsHttpFileUplaodDownload.AddEndPoint<IHttpFileUploadDownload>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/HttpFileUploadDownload");
                        bhsHttpFileUplaodDownload.OpenServer();
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()--BandwidthServer()", "Domains\\SuperNodeServiceDomain.cs");
                    }


                    #endregion

                    #endregion

                    #region Strating PBX Server (This code has been added by Nisarg)
                    try
                    {
                        objPBX.FncStartPBX(VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()--yetStart", "Domains\\SuperNodeServiceDomain.cs");
                    }
                    #endregion


                    //fncMakeAllBuddyOffline();

                    
                    dtWebReqBS4SN.Start();
                    timerBuzzSN.Start();
                    timerBuddylist.Start();


                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain()--4", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        #region Bandwidth Http WCF Handlers

        void SuperNodeServiceDomain_entsvcUpload(MContractRemoteFileInfo request)
        {
            try
            {
                string filePath = System.IO.Path.Combine(uploadPath, request.FileName);

                int chunkSize = 2048;
                byte[] buffer = new byte[chunkSize];

                using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, FileShare.ReadWrite))
                {
                    do
                    {
                        int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                        if (bytesRead == 0) break;

                        writeStream.Write(buffer, 0, bytesRead);
                    } while (true);

                    writeStream.Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_entsvcUpload()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        VMukti.Business.CommonMessageContract.MContractRemoteFileInfo SuperNodeServiceDomain_entsvcDownload(VMukti.Business.CommonMessageContract.MContractDownloadRequest request)
        {
            try
            {
                string filePath = System.IO.Path.Combine("UploadDownload", request.FileName);
                System.IO.FileStream stream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                MContractRemoteFileInfo result = new MContractRemoteFileInfo();
                result.FileName = request.FileName;
                result.Length = stream.Length;
                result.FileByteStream = stream;
                return result;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_entDownload", "Domains\\SuperNodeServiceDomain.cs");
                return null;
            }
        }

        void SuperNodeServiceDomain_entsvcJoin(string uName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_ebtsvcJoin()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }
        #endregion

        #region Http DataBase EventHandlers

        int SuperNodeServiceDomain_EntHttpExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
                int Result = -1;
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (objSParam != null && objSParam.objParam != null)
                {
                    for (int paramCnt = 0; paramCnt < objSParam.objParam.Count; paramCnt++)
                    {
                        SqlParameter sp = new SqlParameter();
                        sp.ParameterName = (string)objSParam.objParam[paramCnt].PName;
                        if (sp.Size != -1)
                        {
                            sp.Size = (int)objSParam.objParam[paramCnt].PSize;
                        }
                        sp.Value = (object)objSParam.objParam[paramCnt].PValue;

                        string direction = (string)objSParam.objParam[paramCnt].Direction;
                        string dbType = (string)objSParam.objParam[paramCnt].PDBType;

                        switch (direction)
                        {
                            case "Input":
                                {
                                    sp.Direction = ParameterDirection.Input;
                                    break;
                                }
                            case "InputOutput":
                                {
                                    sp.Direction = ParameterDirection.InputOutput;
                                    break;
                                }
                            case "Output":
                                {
                                    sp.Direction = ParameterDirection.Output;
                                    break;
                                }
                            case "ReturnValue":
                                {
                                    sp.Direction = ParameterDirection.ReturnValue;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }

                        switch (dbType)
                        {
                            case "VarChar":
                                {
                                    sp.SqlDbType = SqlDbType.VarChar;
                                    break;
                                }
                            case "NVarChar":
                                {
                                    sp.SqlDbType = SqlDbType.NVarChar;

                                    break;
                                }
                            case "Char":
                                {
                                    sp.SqlDbType = SqlDbType.Char;

                                    break;
                                }
                            case "NChar":
                                {
                                    sp.SqlDbType = SqlDbType.NChar;

                                    break;
                                }
                            case "Text":
                                {
                                    sp.SqlDbType = SqlDbType.Text;

                                    break;
                                }
                            case "DateTime":
                                {
                                    sp.SqlDbType = SqlDbType.DateTime;

                                    break;
                                }
                            case "Int":
                                {
                                    sp.SqlDbType = SqlDbType.Int;

                                    break;
                                }
                            case "UniqueIdentifier":
                                {
                                    sp.SqlDbType = SqlDbType.UniqueIdentifier;

                                    break;
                                }

                            case "Bit":
                                {
                                    sp.SqlDbType = SqlDbType.Bit;

                                    break;
                                }

                            case "Float":
                                {
                                    sp.SqlDbType = SqlDbType.Float;

                                    break;
                                }

                            case "Decimal":
                                {
                                    sp.SqlDbType = SqlDbType.Decimal;

                                    break;
                                }
                            case "BigInt":
                                {
                                    sp.SqlDbType = SqlDbType.BigInt;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        cmd.Parameters.Add(sp);
                    }

                }

                cmd.ExecuteNonQuery();
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    if (cmd.Parameters[i].Direction == ParameterDirection.InputOutput || cmd.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        Result = Convert.ToInt32(cmd.Parameters[i].Value);
                    }
                }
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return Result;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_entHttpExcuteReturnNonQuery()", "Domains\\SuperNodeServiceDomain.cs");
                return -1;
            }
        }

        void SuperNodeServiceDomain_EntHttpExecuteNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (objSParam != null && objSParam.objParam != null)
                {
                    for (int paramCnt = 0; paramCnt < objSParam.objParam.Count; paramCnt++)
                    {
                        SqlParameter sp = new SqlParameter();
                        sp.ParameterName = (string)objSParam.objParam[paramCnt].PName;
                        if (sp.Size != -1)
                        {
                            sp.Size = (int)objSParam.objParam[paramCnt].PSize;
                        }
                        sp.Value = (object)objSParam.objParam[paramCnt].PValue;

                        string direction = (string)objSParam.objParam[paramCnt].Direction;
                        string dbType = (string)objSParam.objParam[paramCnt].PDBType;

                        switch (direction)
                        {
                            case "Input":
                                {
                                    sp.Direction = ParameterDirection.Input;
                                    break;
                                }
                            case "InputOutput":
                                {
                                    sp.Direction = ParameterDirection.InputOutput;
                                    break;
                                }
                            case "Output":
                                {
                                    sp.Direction = ParameterDirection.Output;
                                    break;
                                }
                            case "ReturnValue":
                                {
                                    sp.Direction = ParameterDirection.ReturnValue;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }

                        switch (dbType)
                        {
                            case "VarChar":
                                {
                                    sp.SqlDbType = SqlDbType.VarChar;
                                    break;
                                }
                            case "NVarChar":
                                {
                                    sp.SqlDbType = SqlDbType.NVarChar;

                                    break;
                                }
                            case "Char":
                                {
                                    sp.SqlDbType = SqlDbType.Char;

                                    break;
                                }
                            case "NChar":
                                {
                                    sp.SqlDbType = SqlDbType.NChar;

                                    break;
                                }
                            case "Text":
                                {
                                    sp.SqlDbType = SqlDbType.Text;

                                    break;
                                }
                            case "DateTime":
                                {
                                    sp.SqlDbType = SqlDbType.DateTime;

                                    break;
                                }
                            case "Int":
                                {
                                    sp.SqlDbType = SqlDbType.Int;

                                    break;
                                }
                            case "UniqueIdentifier":
                                {
                                    sp.SqlDbType = SqlDbType.UniqueIdentifier;

                                    break;
                                }

                            case "Bit":
                                {
                                    sp.SqlDbType = SqlDbType.Bit;

                                    break;
                                }

                            case "Float":
                                {
                                    sp.SqlDbType = SqlDbType.Float;

                                    break;
                                }

                            case "Decimal":
                                {
                                    sp.SqlDbType = SqlDbType.Decimal;

                                    break;
                                }
                            case "BigInt":
                                {
                                    sp.SqlDbType = SqlDbType.BigInt;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        cmd.Parameters.Add(sp);


                    }

                }


                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpExecuteNonQuery()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        clsDataBaseInfo SuperNodeServiceDomain_EntHttpExecuteStoredProcedure(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (objSParam != null && objSParam.objParam != null)
                {
                    for (int paramCnt = 0; paramCnt < objSParam.objParam.Count; paramCnt++)
                    {
                        SqlParameter sp = new SqlParameter();
                        sp.ParameterName = (string)objSParam.objParam[paramCnt].PName;
                        if (sp.Size != -1)
                        {
                            sp.Size = (int)objSParam.objParam[paramCnt].PSize;
                        }
                        sp.Value = (object)objSParam.objParam[paramCnt].PValue;

                        string direction = (string)objSParam.objParam[paramCnt].Direction;
                        string dbType = (string)objSParam.objParam[paramCnt].PDBType;

                        switch (direction)
                        {
                            case "Input":
                                {
                                    sp.Direction = ParameterDirection.Input;
                                    break;
                                }
                            case "InputOutput":
                                {
                                    sp.Direction = ParameterDirection.InputOutput;
                                    break;
                                }
                            case "Output":
                                {
                                    sp.Direction = ParameterDirection.Output;
                                    break;
                                }
                            case "ReturnValue":
                                {
                                    sp.Direction = ParameterDirection.ReturnValue;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }

                        switch (dbType)
                        {
                            case "VarChar":
                                {
                                    sp.SqlDbType = SqlDbType.VarChar;
                                    break;
                                }
                            case "NVarChar":
                                {
                                    sp.SqlDbType = SqlDbType.NVarChar;

                                    break;
                                }
                            case "Char":
                                {
                                    sp.SqlDbType = SqlDbType.Char;

                                    break;
                                }
                            case "NChar":
                                {
                                    sp.SqlDbType = SqlDbType.NChar;

                                    break;
                                }
                            case "Text":
                                {
                                    sp.SqlDbType = SqlDbType.Text;

                                    break;
                                }
                            case "DateTime":
                                {
                                    sp.SqlDbType = SqlDbType.DateTime;

                                    break;
                                }
                            case "Int":
                                {
                                    sp.SqlDbType = SqlDbType.Int;

                                    break;
                                }
                            case "UniqueIdentifier":
                                {
                                    sp.SqlDbType = SqlDbType.UniqueIdentifier;

                                    break;
                                }

                            case "Bit":
                                {
                                    sp.SqlDbType = SqlDbType.Bit;

                                    break;
                                }

                            case "Float":
                                {
                                    sp.SqlDbType = SqlDbType.Float;

                                    break;
                                }

                            case "Decimal":
                                {
                                    sp.SqlDbType = SqlDbType.Decimal;

                                    break;
                                }
                            case "BigInt":
                                {
                                    sp.SqlDbType = SqlDbType.BigInt;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        cmd.Parameters.Add(sp);


                    }

                }


                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                clsDataBaseInfo dbContract = new clsDataBaseInfo();
                dbContract.dsInfo = new DataSet();
                sda.Fill(dbContract.dsInfo);
                sda.Dispose();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return dbContract;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpExecuteStoreProcedure()", "Domains\\SuperNodeServiceDomain.cs");
                return null;
            }

        }

        clsDataBaseInfo SuperNodeServiceDomain_EntHttpExecuteDataSet(string querystring)
        {
            try
            {
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                SqlCommand cmd = new SqlCommand(querystring, conn);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                clsDataBaseInfo dbContract = new clsDataBaseInfo();
                dbContract.dsInfo = new DataSet();
                sda.Fill(dbContract.dsInfo);
                sda.Dispose();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return dbContract;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpExecuteDataSet()", "Domains\\SuperNodeServiceDomain.cs");
                return null;
            }
        }

        void SuperNodeServiceDomain_EntHttpsvcjoin()
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpsvcJoin()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }
        #endregion

        #region WCF Functions for Call Center
        #region WCF Functions of PBX Server
        void BootstrapServiceDomain_EntNetP2PPBXsvcJoin()
        { }
        void BootstrapServiceDomain_EntNetP2PPBXsvcGetPBXschema(string SchemaName, string IPAddress)
        { }
        void BootstrapServiceDomain_EntNetP2PPBXsvcPutPBXSchema(string SchemaName, List<string> PBXInfo)
        { }
        void BootstrapServiceDomain_EntNetP2PPBXsvcInformOther(string SchemaName, string IPAddress)
        { }
        void BootstrapServiceDomain_EntNetP2PPBXsvcUnJoin(string schemaName)
        { }
        #endregion

        #region NetP2PBootStrapPredictive Event
        void SuperNodeServiceDomain_EntsvcJoin(string UserNumber, string CampaignID)
        { }
        void SuperNodeServiceDomain_EntAddExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber)
        { }

        void SuperNodeServiceDomain_EntRequestExtraCall(string SenderUserNumber, string CampaignID, string CallRequestedUserNumber)
        { }
        void SuperNodeServiceDomain_EntSendExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber, string CallRequesedUserNumber, string LeadID, string ConfNumber)
        { }
        void SuperNodeServiceDomain_EntRemoveExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber)
        { }
        void SuperNodeServiceDomain_EntRequestFunctionToExecute(string FunctionType, string To, string From)
        { }
        void SuperNodeServiceDomain_EntReplyFunctionExecuted(string FunctionType, string To, string From, string ConfNumber)
        { }
        void SuperNodeServiceDomain_EntHangUpCall(string AgentNumber)
        { }
        void SuperNodeServiceDomain_EntUnJoin()
        { }
        #endregion
        #endregion

        #region NetP2P BootStrap Event Handlers

        void SuperNodeServiceDomain_EntsvcNetP2PUnJoin(string uName)
        {
            try
            {
                if (uName != null && uName != string.Empty)
                {
                    fncSNInsertBuddy(uName, "Offline");
                    fncSNDeleteNode(uName);
                    fncUpdateUserBuddyStatus(uName, "Offline");
                }
                if (hsSuperNodeInfo.Contains(uName))
                {
                    hsSuperNodeInfo.Remove(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PUnjoin()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }
        void SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, "~" + IPAddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcNetP2PPageSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcSetSpecialMsg(objPageInfo, "~" + IPAddress);
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2pPageSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsg4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, "~" + IPAddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsgBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, "~" + IPAddress, objPageInfo);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PSetSpecialMsgBuddiesClick()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcPageBuddyRetSetSpecialMsg(objBuddyRetPageInfo, "~" + IPAddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcPageBuddyRetSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcAddDraggedBuddy(from, to, msg, objModInfo, "~" + IPAddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcAddDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModuleInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcAddDraggedBuddy(from, to, msg, lstModuleInfo, "~" + IPAddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcAddPageDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                if (IPAddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcSetRemoveDraggedBuddy(from, to, msg, objModInfo, "~" + IPAddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcSetRemoveDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcNetP2PServiceJoin(string sSuperNodeIP)
        { }

        Hashtable hsSuperNodeInfo = new Hashtable();
        void SuperNodeServiceDomain_EntsvcBuzzSuperNode(string uname)
        {
            lock (this)
            {
                try
                {
                    if (!hsSuperNodeInfo.Contains(uname) && uname != VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP)
                    {
                        hsSuperNodeInfo.Add(uname, DateTime.Now);

                        if (uname != null && uname != string.Empty)
                        {
                            fncSNInsertBuddy(uname, "Online");
                            fncSNInsertNode(uname);
                            fncUpdateUserBuddyStatus(uname, "Online");
                        }
                    }
                    else
                    {
                        IDictionaryEnumerator hsSuperNodeList = hsSuperNodeInfo.GetEnumerator();
                        while (hsSuperNodeList.MoveNext())
                        {
                            if (hsSuperNodeList.Key.ToString() != uname)
                            {
                                string sNodeName = hsSuperNodeList.Key.ToString();
                                //ClsException.WriteToLogFile("Node Name in timerBuzz " + sNodeName);
                                DateTime previousTime = DateTime.Parse(hsSuperNodeList.Value.ToString());
                                TimeSpan difference = DateTime.Now - previousTime;
                                int TimeInMin = difference.Minutes;

                                if (TimeInMin >= 1)
                                {
                                    if (sNodeName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        //ClsException.WriteToLogFile("Supernode :: " + sNodeName + " is not coming for 3 minitues so making offline.");
                                        clientNetP2pChannelBS.svcNetP2PUnJoin(sNodeName);
                                        clientHttpChannelBS.svcHttpBSUnJoin(sNodeName, "", true);
                                    }
                                }
                            }
                        }
                        if (hsSuperNodeInfo.Contains(uname))
                        {
                            hsSuperNodeInfo[uname] = DateTime.Now;
                        }
                    }
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcBuzzSuperNode()", "Domains\\SuperNodeServiceDomain.cs");
                }
            }

        }

        void SuperNodeServiceDomain_EntsvcSetNodeStatus(string buddyname)
        {
            try
            {
                //System.Text.StringBuilder sb2 = new StringBuilder();
                lock (this)
                {

                    //sb2.AppendLine("checking whether node_status contains entry : " + buddyname);
                    //sb2.AppendLine();
                    //sb2.AppendLine("Location : SuperNodeServiceDomain_EntsvcSetNodeStatus()");
                    //sb2.AppendLine();

                    OpenConnection();
                    SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM Node_Status WHERE (Buddy_Name = '" + buddyname + "')", LocalSQLConn);

                    object count = cmd.ExecuteScalar();

                    //record exists
                    if (int.Parse(count.ToString()) > 0)
                    {

                        //sb2.AppendLine("updating timestamp : " + buddyname);
                        //sb2.AppendLine();
                        //sb2.AppendLine("Location : SuperNodeServiceDomain_EntsvcSetNodeStatus()");
                        //sb2.AppendLine();


                        cmd = new SqlCeCommand("update Node_Status set Buddy_TimeStamp=GETDATE(), Buddy_Status='Online' where Buddy_Name= '" + buddyname + "'", LocalSQLConn);

                        //if (status.ToLower() != "online")
                        //{
                        //    SqlCeCommand cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = '" + DateTime.MinValue.ToString() + "' Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                        //}
                        //else
                        //{
                        //    SqlCeCommand cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = getdate() Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                        //}

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //sb2.AppendLine("inserting into node_status for buddy : " + buddyname);
                        //sb2.AppendLine();
                        //sb2.AppendLine("Location : SuperNodeServiceDomain_EntsvcSetNodeStatus()1");
                        //sb2.AppendLine();


                        string str = "insert into Node_Status(Buddy_Name,Buddy_Status,Buddy_TimeStamp) values(@Buddy_Name,@Buddy_Status,@Buddy_TimeStamp) ";
                        cmd = new SqlCeCommand(str, LocalSQLConn);


                        cmd.Parameters.AddWithValue("@Buddy_Name", buddyname);
                        cmd.Parameters.AddWithValue("@Buddy_Status", "Online");
                        cmd.Parameters.AddWithValue("@Buddy_TimeStamp", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }


                    //if (!hsOnlineBuddyTable.ContainsKey(buddyname))
                    //{

                    //    sb2.AppendLine("entry : " + buddyname + "not found. so adding it with timestamp" );
                    //    sb2.AppendLine();
                    //    sb2.AppendLine("Location : SuperNodeServiceDomain_EntsvcSetNodeStatus()");
                    //    sb2.AppendLine();


                    //    hsOnlineBuddyTable.Add(buddyname, DateTime.Now);

                    //}
                    //else
                    //{
                    //    sb2.AppendLine("entry : " + buddyname + " found. so updating the timestamp");
                    //    sb2.AppendLine();
                    //    sb2.AppendLine("Location : SuperNodeServiceDomain_EntsvcSetNodeStatus()");
                    //    sb2.AppendLine();


                    //    hsOnlineBuddyTable[buddyname] = DateTime.Now;
                    //}
                }
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb2);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcSetNodeStatus()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcJoinConf(string from, string to, int confid, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    //clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, "~" + IPAddress);
                    clientNetP2PChannelSN.svcJoinConf(from, to, confid, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcJoinConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcSendConfInfo(from, to, confid, pageinfo, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcSendConfInfo()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcAddConfBuddy(from, lstBuddies, confid, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcAddConfBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        void SuperNodeServiceDomain_EntsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcRemoveConf(from, lstBuddies, confid, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcRemoveConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        void SuperNodeServiceDomain_EntsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcUnJoinConf(from, lstBuddies, confid, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcUnJoinConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcEnterConf(from, lstBuddies, confid, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcEnter()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid, string ipaddress)
        {
            try
            {
                if (ipaddress != VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2PChannelSN.svcPodNavigation(from, lstBuddies,pageid,tabid, podid, "~" + ipaddress);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcPodNavigation()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        //Code Added by Nisarg For Predictive Dialer Start
        void SuperNodeServiceDomain_EntsvcNetP2PAddExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber)
        { }
        void SuperNodeServiceDomain_EntsvcNetP2PRequestExtraCall(string SenderUserNumber, string CampaignID, string CallRequestedUserNumber)
        { }
        void SuperNodeServiceDomain_EntsvcNetP2PSendExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber, string CallRequesedUserNumber, string LeadID)
        { }
        
        void SuperNodeServiceDomain_EntsvcNetP2PRemoveExtraCall(string SenderUserNumber, string CampaignID, string PhoneNumber)
        { }
        //code Added By Nisarg For Predictive Dialer Stop
        #endregion

        #region NetP2P SuperNode Event Handlers

        void NetP2PSNDomain_EntsvcUnJoin(string uname)
        { }
        void NetP2PSNDomain_EntsvcJoin(string uname)
        { }            
        
        void NetP2PSNDomain_EntsvcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {

                #region OPEN MODULE
                try
                {

                    bool blnNodePresent = false;

                    for (int i = 0; i < lstmyNodes.Count; i++)
                    {
                        if (lstmyNodes[i].uname == to)
                        {
                            if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                            {
                                clsMessage objmsg = new clsMessage();
                                objmsg.strFrom = from;
                                objmsg.strTo = new string[] { to };
                                objmsg.strMessage = msg;
                                string p2puri = objModInfo.strUri[0];
                                string httpuri = "";

                                System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                                ClsPod podObj = new ClsPod();
                                podObj = ClsPod.GetModInfo(objModInfo.intModuleId);

                                string modulename = podObj.ZipFile.Replace(".zip", "");

                                string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                                string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                                XmlParser xp = new XmlParser();
                                xp.Parse(strXmlPath);

                                #region Loading ReferencedAssemblies

                                al.Clear();

                                DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                                ShowDirectory(dirinfomodule);

                                for (int j = 0; j < al.Count; j++)
                                {
                                    string[] arraysplit = al[j].ToString().Split('\\');
                                    if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                        CopyToBase(modulename, podObj.AssemblyFile);

                                        #region CreatingObject

                                        for (int k = 0; k < t1.Length; k++)
                                        {
                                            if (t1[k].Name == xp.xMain.DummyClassName)
                                            {
                                                try
                                                {

                                                    object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                    MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                    object[] objparams = new object[3];
                                                    objparams[0] = intModule;
                                                    objparams[1] = p2puri;
                                                    objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                    httpuri = (mi.Invoke(obj1, objparams)).ToString();

                                                }

                                                catch (Exception exp)
                                                {
                                                    MessageBox.Show("CreatingObject " + exp.Message);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                                objModInfo.strUri[1] = httpuri;
                                objmsg.objClsModuleInfo = objModInfo;
                                lstmyNodes[i].lstMyMsgs.Add(objmsg);
                                blnNodePresent = true;
                                break;
                            }
                            else
                            {
                                blnNodePresent = true;
                                break;
                            }
                        }
                    }
                    if (!blnNodePresent && IPAddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                    }

                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
                }
                #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PSNDomain_EntsvcSetSpecialMsg()--1", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcPageSetSpecialMsg(clsPageInfo objPageInfo, string IPAddress)
        {
            try
            {
                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == objPageInfo.strTo)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = objPageInfo.strFrom;
                            objmsg.strTo = new string[] { objPageInfo.strTo };
                            objmsg.strMessage = objPageInfo.strMsg;

                            for (int tCnt = 0; tCnt < objPageInfo.objaTabs.Length; tCnt++)
                            {
                                for (int lstCnt = 0; lstCnt < objPageInfo.objaTabs[tCnt].objaPods.Length; lstCnt++)
                                {
                                    string p2puri = objPageInfo.objaTabs[tCnt].objaPods[lstCnt].strUri[0];
                                    string httpuri = "";

                                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                                    ClsPod podObj = new ClsPod();
                                    podObj = ClsPod.GetModInfo(objPageInfo.objaTabs[tCnt].objaPods[lstCnt].intModuleId);

                                    string modulename = podObj.ZipFile.Replace(".zip", "");

                                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                                    string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                                    XmlParser xp = new XmlParser();
                                    xp.Parse(strXmlPath);

                                    #region Loading ReferencedAssemblies

                                    al.Clear();

                                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                                    ShowDirectory(dirinfomodule);

                                    for (int j = 0; j < al.Count; j++)
                                    {
                                        string[] arraysplit = al[j].ToString().Split('\\');
                                        if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                            CopyToBase(modulename, podObj.AssemblyFile);

                                            #region CreatingObject

                                            for (int k = 0; k < t1.Length; k++)
                                            {
                                                if (t1[k].Name == xp.xMain.DummyClassName)
                                                {
                                                    try
                                                    {

                                                        object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                        MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                        object[] objparams = new object[3];
                                                        objparams[0] = intModule;
                                                        objparams[1] = p2puri;
                                                        objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                        httpuri = (mi.Invoke(obj1, objparams)).ToString();
                                                    }

                                                    catch (Exception exp)
                                                    {
                                                        MessageBox.Show("Error in creating a dynamic object - " + exp.Message);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    objPageInfo.objaTabs[tCnt].objaPods[lstCnt].strUri[1] = httpuri;
                                }
                            }
                            objmsg.objPageInfo = objPageInfo;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            break;
                        }
                    }
                }
                if (!blnNodePresent && IPAddress.Substring(0, 1) != "~")
                {
                    clientNetP2pChannelBS.svcSetSpecialMsg(objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PSNDomain_EntsvcPageSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcSetSpecialMsg4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < to.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == to[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.strTo = to.ToArray();
                        objmsg.strMessage = msg;
                        string p2puri = objModInfo.strUri[0];
                        string httpuri = "";

                        System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                        ClsPod podObj = new ClsPod();
                        podObj = ClsPod.GetModInfo(objModInfo.intModuleId);

                        string modulename = podObj.ZipFile.Replace(".zip", "");

                        string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                        string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                        XmlParser xp = new XmlParser();
                        xp.Parse(strXmlPath);

                        #region Loading ReferencedAssemblies

                        al.Clear();

                        DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                        ShowDirectory(dirinfomodule);

                        for (int j = 0; j < al.Count; j++)
                        {
                            string[] arraysplit = al[j].ToString().Split('\\');
                            if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                CopyToBase(modulename, podObj.AssemblyFile);

                                #region CreatingObject

                                for (int k = 0; k < t1.Length; k++)
                                {
                                    if (t1[k].Name == xp.xMain.DummyClassName)
                                    {
                                        try
                                        {

                                            object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                            MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                            object[] objparams = new object[3];
                                            objparams[0] = intModule;
                                            objparams[1] = p2puri;
                                            objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                            httpuri = (mi.Invoke(obj1, objparams)).ToString();

                                        }

                                        catch (Exception exp)
                                        {
                                            MessageBox.Show("CreatingObject " + exp.Message);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        objModInfo.strUri[1] = httpuri;
                        objmsg.objClsModuleInfo = objModInfo;
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == to.Count)
                {
                }
                else
                {
                    if (IPAddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PDomain_EntsvcSetSpecialMsg4MultipleBuddies()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcSetSpecialMsgBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress, clsPageInfo objPageInfo)
        {
            try
            {
                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < to.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == to[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.strTo = to.ToArray();
                        objmsg.strMessage = msg;
                        //string p2puri = objModInfo.strUri[0];
                        //string httpuri = "";

                        for (int tCnt = 0; tCnt < objPageInfo.objaTabs.Length; tCnt++)
                        {
                            for (int lstCnt = 0; lstCnt < objPageInfo.objaTabs[tCnt].objaPods.Length; lstCnt++)
                            {
                                string p2puri = objPageInfo.objaTabs[tCnt].objaPods[lstCnt].strUri[0];
                                string httpuri = "";

                                System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                                ClsPod podObj = new ClsPod();
                                podObj = ClsPod.GetModInfo(objModInfo.intModuleId);

                                string modulename = podObj.ZipFile.Replace(".zip", "");

                                string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                                string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                                XmlParser xp = new XmlParser();
                                xp.Parse(strXmlPath);

                                #region Loading ReferencedAssemblies

                                al.Clear();

                                DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                                ShowDirectory(dirinfomodule);

                                for (int j = 0; j < al.Count; j++)
                                {
                                    string[] arraysplit = al[j].ToString().Split('\\');
                                    if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                        CopyToBase(modulename, podObj.AssemblyFile);

                                        #region CreatingObject

                                        for (int k = 0; k < t1.Length; k++)
                                        {
                                            if (t1[k].Name == xp.xMain.DummyClassName)
                                            {
                                                try
                                                {

                                                    object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                    MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                    object[] objparams = new object[3];
                                                    objparams[0] = intModule;
                                                    objparams[1] = p2puri;
                                                    objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                    httpuri = (mi.Invoke(obj1, objparams)).ToString();

                                                }

                                                catch (Exception exp)
                                                {
                                                    MessageBox.Show("CreatingObject " + exp.Message);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                objPageInfo.objaTabs[tCnt].objaPods[lstCnt].strUri[1] = httpuri;
                            }
                        }
                        objmsg.objPageInfo = objPageInfo;
                        //objModInfo.strUri[1] = httpuri;
                        objmsg.objClsModuleInfo = objModInfo;
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == to.Count)
                {
                }
                else
                {
                    if (IPAddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP, objPageInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PSNDomain_EntsvcSetSpecialMsgBuddiesClick()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcPageBuddyRetSetSpecialMsg(clsBuddyRetPageInfo objBuddyRetPageInfo, string IPAddress)
        {
            try
            {
                bool blnNodePresent = false;
                int i = 0;
                for (i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == objBuddyRetPageInfo.strFrom)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = objBuddyRetPageInfo.strFrom;
                            objmsg.strTo = new string[] { objBuddyRetPageInfo.strTo };
                            objmsg.strMessage = objBuddyRetPageInfo.strMsg;

                            objmsg.objBuddyRetPageInfo = objBuddyRetPageInfo;

                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                        }
                        blnNodePresent = true;
                    }
                }
                if (i == lstmyNodes.Count)
                {
                    List<string> lstNotMyHttpNodes = new List<string>();

                    for (i = 0; i < lstmyNodes.Count; i++)
                    {
                        for (int j = 0; j < objBuddyRetPageInfo.objaTabs[0].objaPods[0].straPodBuddies.Length; j++)
                        {
                            if (lstmyNodes[i].uname == objBuddyRetPageInfo.objaTabs[0].objaPods[0].straPodBuddies[j])
                            {
                                if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                                {
                                    clsMessage objmsg = new clsMessage();
                                    objmsg.strFrom = objBuddyRetPageInfo.strFrom;
                                    objmsg.strTo = new string[] { objBuddyRetPageInfo.strTo };
                                    objmsg.strMessage = objBuddyRetPageInfo.strMsg;

                                    objmsg.objBuddyRetPageInfo = objBuddyRetPageInfo;

                                    lstmyNodes[i].lstMyMsgs.Add(objmsg);
                                }
                                blnNodePresent = true;
                            }
                            else
                            {
                                lstNotMyHttpNodes.Add(objBuddyRetPageInfo.objaTabs[0].objaPods[0].straPodBuddies[j]);
                            }
                        }
                    }
                    if (lstNotMyHttpNodes.Count > 0)
                    {
                        objBuddyRetPageInfo.objaTabs[0].objaPods[0].straPodBuddies = lstNotMyHttpNodes.ToArray();
                        blnNodePresent = false;
                    }
                }
                if (!blnNodePresent && IPAddress.Substring(0, 1) != "~")
                {
                    clientNetP2pChannelBS.svcPageBuddyRetSetSpecialMsg(objBuddyRetPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PSNDomain_EntsvcPageBuddyRetSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {

            try
            {
                bool blnIsNodeNotPresent = false;
                for (int BCnt = 0; BCnt < objModInfo.lstUsersDropped.Count; BCnt++) //Dragged Buddy List
                {
                    bool blnNodePresent = false;
                    for (int NCnt = 0; NCnt < lstmyNodes.Count; NCnt++) //Current Node's List
                    {
                        if (objModInfo.lstUsersDropped[BCnt] == lstmyNodes[NCnt].uname)
                        {
                            blnNodePresent = true;
                            if (lstmyNodes[NCnt].nodeBType == PeerType.NodeWithHttp)
                            {
                                clsMessage objMsg = new clsMessage();
                                objMsg.strFrom = to;
                                objMsg.strTo = new string[] { lstmyNodes[NCnt].uname };
                                objMsg.strMessage = "Newly Dragged Buddy";
                                objMsg.objClsModuleInfo = objModInfo;
                                lstmyNodes[NCnt].lstMyMsgs.Add(objMsg);
                                break;
                            }
                        }
                    }
                    if (!blnNodePresent)
                    {
                        blnIsNodeNotPresent = true;
                    }
                }
                if (blnIsNodeNotPresent && IPAddress.Substring(0, 1) != "~")
                {
                    clientNetP2pChannelBS.svcAddDraggedBuddy(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PSNDomain_EntsvcAddDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> lstModInfo, string IPAddress)
        {
            try
            {
                bool blnIsNodeNotPresent = false;
                for (int BCnt = 0; BCnt < lstModInfo[0].lstUsersDropped.Count; BCnt++) //Dragged Buddy List
                {
                    bool blnNodePresent = false;
                    for (int NCnt = 0; NCnt < lstmyNodes.Count; NCnt++) //Current Node's List
                    {
                        if (lstModInfo[0].lstUsersDropped[BCnt] == lstmyNodes[NCnt].uname)
                        {
                            blnNodePresent = true;
                            if (lstmyNodes[NCnt].nodeBType == PeerType.NodeWithHttp)
                            {
                                clsMessage objMsg = new clsMessage();
                                objMsg.strFrom = to;
                                objMsg.strTo = new string[] { lstmyNodes[NCnt].uname };
                                objMsg.strMessage = "Newly Dragged Buddy";
                                objMsg.lstClsModuleInfo = lstModInfo;
                                lstmyNodes[NCnt].lstMyMsgs.Add(objMsg);
                                break;
                            }
                        }
                    }
                    if (!blnNodePresent)
                    {
                        blnIsNodeNotPresent = true;
                    }
                }
                if (blnIsNodeNotPresent && IPAddress.Substring(0, 1) != "~")
                {
                    clientNetP2pChannelBS.svcAddDraggedBuddy(from, to, msg, lstModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PSNDomain_EntsvcAddPageDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo, string IPAddress)
        {
            try
            {
                #region CLOSE MODULE

                if (msg == "CLOSE MODULE")
                {
                    for (int j = 0; j < to.Count; j++)
                    {
                        bool blnNodePresent = false;
                        for (int i = 0; i < lstmyNodes.Count; i++)
                        {
                            if (lstmyNodes[i].uname == to[j])
                            {
                                if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                                {
                                    clsMessage objmsg = new clsMessage();
                                    objmsg.strFrom = from;
                                    objmsg.strTo = to.ToArray();
                                    objmsg.strMessage = msg;

                                    objmsg.objClsModuleInfo = objModInfo;
                                    lstmyNodes[i].lstMyMsgs.Add(objmsg);
                                    blnNodePresent = true;
                                    break;
                                }
                                else
                                {
                                    blnNodePresent = true;
                                    break;
                                }
                            }
                        }

                        if (!blnNodePresent && IPAddress.Substring(0, 1) != "~")
                        {
                            clientNetP2pChannelBS.svcSetRemoveDraggedBuddy(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NetP2PDomain_EntsvcSetRemoveDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcJoinConf(string from, string to, int confid, string ipaddress)
        {

            try
            {

                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == to)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = from;
                            objmsg.strTo = new string[] { to };
                            objmsg.strMessage = "JoinConf";

                            objmsg.confid = confid;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            break;
                        }
                    }
                }
                if (!blnNodePresent && ipaddress.Substring(0, 1) != "~")
                {
                    clientNetP2pChannelBS.svcJoinConf(from, to, confid, VMuktiInfo.CurrentPeer.SuperNodeIP);
                }

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcJoinConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        void NetP2PSNDomain_EntsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress)
        {
            try
            {

                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == to)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = from;
                            objmsg.strTo = new string[] { to };
                            objmsg.strMessage = "SendConfInfo";
                            //objmsg.objPageInfo = pageinfo;

                            for (int tCnt = 0; tCnt < pageinfo.objaTabs.Length; tCnt++)
                            {
                                for (int lstCnt = 0; lstCnt < pageinfo.objaTabs[tCnt].objaPods.Length; lstCnt++)
                                {
                                    string p2puri = pageinfo.objaTabs[tCnt].objaPods[lstCnt].strUri[0];
                                    string httpuri = "";

                                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                                    ClsPod podObj = new ClsPod();
                                    podObj = ClsPod.GetModInfo(pageinfo.objaTabs[tCnt].objaPods[lstCnt].intModuleId);

                                    string modulename = podObj.ZipFile.Replace(".zip", "");

                                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                                    string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                                    XmlParser xp = new XmlParser();
                                    xp.Parse(strXmlPath);

                                    #region Loading ReferencedAssemblies

                                    al.Clear();

                                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                                    ShowDirectory(dirinfomodule);

                                    for (int j = 0; j < al.Count; j++)
                                    {
                                        string[] arraysplit = al[j].ToString().Split('\\');
                                        if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                            CopyToBase(modulename, podObj.AssemblyFile);

                                            #region CreatingObject

                                            for (int k = 0; k < t1.Length; k++)
                                            {
                                                if (t1[k].Name == xp.xMain.DummyClassName)
                                                {
                                                    try
                                                    {

                                                        object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                        MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                        object[] objparams = new object[3];
                                                        objparams[0] = intModule;
                                                        objparams[1] = p2puri;
                                                        objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                        httpuri = (mi.Invoke(obj1, objparams)).ToString();
                                                    }

                                                    catch (Exception exp)
                                                    {
                                                        MessageBox.Show("Error in creating a dynamic object - " + exp.Message);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    pageinfo.objaTabs[tCnt].objaPods[lstCnt].strUri[1] = httpuri;
                                }
                            }
                            objmsg.objPageInfo = pageinfo;

                            objmsg.confid = confid;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            break;
                        }
                    }
                }
                if (!blnNodePresent && ipaddress.Substring(0, 1) != "~")
                {
                    clientNetP2pChannelBS.svcSendConfInfo(from, to, confid, pageinfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                }

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcSendConfInfo()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        void NetP2PSNDomain_EntsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "AddConfBuddy";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                }
                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        //clientNetP2pChannelBS.svcUnJoinConf(from, lstBuddies, confid, ipaddress);
                        clientNetP2pChannelBS.svcAddConfBuddy(from, lstBuddies, confid, ipaddress);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcAddConfBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }



        void NetP2PSNDomain_EntsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "RemoveBuddyConf";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                }
                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        //clientNetP2pChannelBS.svcUnJoinConf(from, lstBuddies, confid, ipaddress);
                        clientNetP2pChannelBS.svcRemoveConf(from, lstBuddies, confid, ipaddress);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcRemoveConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "UnJoinConf";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                }
                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcUnJoinConf(from, lstBuddies, confid, ipaddress);
                    }
                }


            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcUnJoinConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void NetP2PSNDomain_EntsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "EnterConf";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                }
                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcEnterConf(from, lstBuddies, confid, ipaddress);
                    }
                }

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcEnterconf()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void NetP2PSNDomain_EntsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid,string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();

                        clsModuleInfo obj = new clsModuleInfo();
                        obj.strPageid = pageid.ToString();
                        obj.strTabid = tabid.ToString();
                        obj.strPodid = podid.ToString();
                        objmsg.objClsModuleInfo = obj;
                        objmsg.strFrom = from;                        
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "PodNavigate";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                }
                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcPodNavigation(from, lstBuddies,pageid,tabid, podid, ipaddress);
                    }
                }

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PSNDomain_EntsvcPodNavigation()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        #endregion

        #region Buddy Related Function HTTP & NetP2P

        #region HTTP SuperNode Functions

        /// <summary>
        /// This function will execute when user sucessfully login to the system and got the Supernode IP.
        /// This function will make user online. and fire P2P function to all supernode, so other supernode will aslo get notification for this buddy.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="bindingType"></param>
        void SuperNodeServiceDomain_EntHttpsvcjoin(string uName, string bindingType)
        {
            try
            {
                bool blnIsSN = false;

                if (bindingType == PeerType.BootStrap.ToString())
                {
                    lstmyNodes.Add(new clsNodeInfo(uName, PeerType.BootStrap));
                    blnIsSN = true;
                }
                else if (bindingType == PeerType.SuperNode.ToString())
                {
                    lstmyNodes.Add(new clsNodeInfo(uName, PeerType.SuperNode));
                    blnIsSN = true;
                }
                else if (bindingType == PeerType.NodeWithNetP2P.ToString())
                {
                    lstmyNodes.Add(new clsNodeInfo(uName, PeerType.NodeWithNetP2P));
                    blnIsSN = false;
                }
                else if (bindingType == PeerType.NodeWithHttp.ToString())
                {
                    lstmyNodes.Add(new clsNodeInfo(uName, PeerType.NodeWithHttp));
                    blnIsSN = false;
                }
                if (uName != null && uName != string.Empty)
                {
                    fncSNInsertBuddy(uName, "Online");
                    fncSNInsertNode(uName);
                    fncUpdateUserBuddyStatus(uName, "Online");
                }
                clientHttpChannelBS.svcHttpBSAuthorizedUser(uName, string.Empty, blnIsSN);
                clientNetP2pChannelBS.svcNetP2PAddUser(uName);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpsvcJoin()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        /// <summary>
        /// This function will called when user has signout or close the application.
        /// user status will be offline from this method. this will aslo fire P2P Supernode function so all other supernode get notification
        /// which buddy is offline.
        /// </summary>
        /// <param name="uName"></param>
        void SuperNodeServiceDomain_EntHttpsvcUnjoin(string uName)
        {
            try
            {
                //System.Text.StringBuilder sb = new StringBuilder();
                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == uName)
                    {
                        lstmyNodes.RemoveAt(i);
                        break;
                    }
                }
                if (uName != null && uName != string.Empty)
                {
                    fncSNInsertBuddy(uName, "Offline");
                    fncSNDeleteNode(uName);
                    fncUpdateUserBuddyStatus(uName, "Offline");
                }
                clientNetP2pChannelBS.svcNetP2PRemoveUser(uName);

                //sb.AppendLine("checking whether entry : " + uName + "found or not");
                //sb.AppendLine();
                //sb.AppendLine("Location : SuperNodeServiceDomain_EntHttpsvcUnjoin()");
                //sb.AppendLine();


                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM Node_Status WHERE (Buddy_Name = '" + uName + "')", LocalSQLConn);

                object count = cmd.ExecuteScalar();

                //record exists
                if (int.Parse(count.ToString()) > 0)
                {

                    //sb.AppendLine("updating time set offline for : " + uName);
                    //sb.AppendLine();
                    //sb.AppendLine("Location : SuperNodeServiceDomain_EntHttpsvcUnjoin()");
                    //sb.AppendLine();


                    //cmd = new SqlCeCommand("update Node_Status set Buddy_TimeStamp = GETDATE()and Buddy_Status='Offline' WHERE (Buddy_Name = '" + uName + "')", LocalSQLConn);
                    cmd = new SqlCeCommand("update Node_Status set Buddy_TimeStamp = '" + DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) + "', Buddy_Status='Offline' WHERE Buddy_Name = '" + uName + "'", LocalSQLConn);
                    cmd.ExecuteNonQuery();
                }


                //if (hsOnlineBuddyTable.Contains(uName))
                //{
                //    sb.AppendLine("entry : " + uName + "found. so removing it");
                //    sb.AppendLine();
                //    sb.AppendLine("Location : SuperNodeServiceDomain_EntHttpsvcUnjoin()");
                //    sb.AppendLine();

                //    hsOnlineBuddyTable.Remove(uName);
                //}
                if (lstOfflinebuddies.Remove(uName))
                {
                    lstOfflinebuddies.Remove(uName);
                }
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpsvcUnJoin()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        List<string> SuperNodeServiceDomain_EntHttpsvcGetBuddyStatus(List<string> BuddyList)
        {
            try
            {
                OpenConnection();
                //ClsException.WriteToLogFile("BS NOde_status 14");
                string str = "SELECT * FROM Node_Status";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, LocalSQLConn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                List<string> lstOnlineBuddies = new List<string>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    //  lstOnlineBuddies.Add(dataTable.Rows[i][1].ToString());
                    lstOnlineBuddies.Add(dataTable.Rows[i][1].ToString() + "-" + dataTable.Rows[i][2].ToString());
                }
                dataAdapter.Dispose();

                return lstOnlineBuddies;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttsvcGetBuddyStatus()", "Domains\\SuperNodeServiceDomain.cs");
                return null;
            }
        }

        //Hashtable to maintain the online user

        /// <summary>
        /// This function will be used by Node to get its buddy status.
        /// This function will called by node in every 2 seconds to get his/her buddy status.
        /// </summary>
        /// <param name="uName"></param>
        /// <returns></returns>
        List<string> SuperNodeServiceDomain_EntHttpGetBuddies(string uName)
        {
            //System.Text.StringBuilder sb = new StringBuilder();
            lock (this)
            {


                //ClsException.WriteToLogFile("In the Get Buddies invoked by " + uName);
                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    //sb.AppendLine("INFORMING SNs OF UNAME");
                    clientNetP2pChannelBS.svcSetNodeStatus(uName);
                    //sb.AppendLine("INFORMED SNs OF UNAME");
                }

            }

            //sb.AppendLine("checking whether entry : " + uName + "found or not");
            //sb.AppendLine();
            //sb.AppendLine("Location : SuperNodeServiceDomain_EntHttpGetBuddies()");
            //sb.AppendLine();


            OpenConnection();
            SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM Node_Status WHERE (Buddy_Name = '" + uName + "')", LocalSQLConn);

            object count = cmd.ExecuteScalar();

            //record exists
            if (int.Parse(count.ToString()) > 0)
            {

               


                cmd = new SqlCeCommand("update Node_Status set Buddy_TimeStamp=GETDATE(), Buddy_Status='Online' WHERE (Buddy_Name = '" + uName + "')", LocalSQLConn);
                cmd.ExecuteNonQuery();

                //fncSNInsertBuddy(uname, "Online");
                fncSNInsertNode(uName);
                //fncUpdateUserBuddyStatus(uname, "Online");

                clientHttpChannelBS.svcHttpBSAuthorizedUser(uName, "", false);
                clientNetP2pChannelBS.svcNetP2PAddUser(uName);


            }


            //if (hsOnlineBuddyTable.Contains(uName))
            //{
            //    sb.AppendLine("entry : " + uName + "found. so updating timestamp");
            //    sb.AppendLine();
            //    sb.AppendLine("Location : SuperNodeServiceDomain_EntHttpGetBuddies()");
            //    sb.AppendLine();

            //    hsOnlineBuddyTable[uName] = DateTime.Now;
            //}

            List<string> lstOnlineBuddies = new List<string>();
            try
            {
                string str = "SELECT * FROM User_BuddyList Where UserName ='" + uName + "'";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, LocalSQLConn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    //adding for user_buddylist changes of removing status from this table

                    string querystat = "select Buddy_Status from Node_Status where Buddy_Name='" + dataTable.Rows[i]["BuddyName"].ToString() + "'";

                    SqlCeCommand cmd1 = new SqlCeCommand(querystat, LocalSQLConn);

                    object strBStat = cmd1.ExecuteScalar();

                    //lstOnlineBuddies.Add(dataTable.Rows[i]["BuddyName"].ToString() + "-" + dataTable.Rows[i]["BuddyStatus"].ToString());

                    if (strBStat != null)
                    {
                        lstOnlineBuddies.Add(dataTable.Rows[i]["BuddyName"].ToString() + "-" + ((string)strBStat));
                    }
                }

                dataAdapter.Dispose();
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                return lstOnlineBuddies;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpGetBuddies()", "Domains\\SuperNodeServiceDomain.cs");
                return lstOnlineBuddies;
            }
        }

        /// <summary>
        /// This function will add new buddies to user's buddylist.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="BuddyName"></param>
        /// <param name="BuddyStatus"></param>
        void SuperNodeServiceDomain_EntHttpAddBuddies(string username, string BuddyName, string BuddyStatus)
        {
            try
            {
                fncUpdateBuddyStatus(username, BuddyName, BuddyStatus);
                fncUpdateBuddyStatus(BuddyName, username, "Online");
                clientNetP2pChannelBS.svcAddBuddies(username, BuddyName, BuddyStatus);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpsAddBuddies()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        /// <summary>
        /// this function will remove perticular buddies from user's buddylist.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="buddyname"></param>
        void SuperNodeServiceDomain_EntHttpRemoveBuddies(string username, List<string> buddyname)
        {
            //SqlCeConnection Conn = new SqlCeConnection(ClientConnectionString);
            //	Conn.Open();
            try
            {
                OpenConnection();
                foreach (string strBuddiesName in buddyname)
                {
                    SqlCeCommand sqlcmd = new SqlCeCommand("delete from User_BuddyList where UserName='" + username + "' and BuddyName='" + strBuddiesName + "'", LocalSQLConn);
                    sqlcmd.ExecuteNonQuery();

                    SqlCeCommand sqlcmd1 = new SqlCeCommand("delete from User_BuddyList where UserName='" + strBuddiesName + "' and BuddyName='" + username + "'", LocalSQLConn);
                    sqlcmd1.ExecuteNonQuery();
                }
                //	Conn.Close();
                //	Conn.Dispose();
                clientNetP2pChannelBS.svcRemoveBuddies(username, buddyname);
                //  clientNetP2pChannelBS.svcRemoveBuddies(buddyname, username);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpRemoveBudies()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        /// <summary>
        /// This function will add node's buddy to supernode's sdf file.....this is because if some supernode has just come to system so it doesnt have
        /// node buddies to its sdf file. so this function will add buddies and its status.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="NodeBuddies"></param>
        void SuperNodeServiceDomain_EntHTTPAddNodeBuddies(string username, List<string> NodeBuddies)
        {
            try
            {
                foreach (string str in NodeBuddies)
                {
                    string[] mystr = str.Split('-');
                    fncUpdateBuddyStatus(username, mystr[0], mystr[1]);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntHttpAddNodeBuddies()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        /// <summary>
        /// This function is used in Diaster Recovery....for node......if nodes supernode is down then node will get new supernode from bootstrap.....using this
        /// function nodes' previous supernode will be made offline and remove from supernode list.
        /// </summary>
        /// <param name="NodeName"></param>
        /// <param name="NodeIP"></param>
        void SuperNodeServiceDomain_EntsvcGetNodeNameByIP(string NodeName, string NodeIP)
        {
            try
            {
                //call data access function to get budy name
                /*SqlCeConnection conn = new SqlCeConnection(ClientConnectionString);
                conn.Open();
                string cmdString = "Select Node_Name from  SuperNode_Info where SuperNode_Id='" + NodeIp + "' ";
                SqlCeCommand NodeSel = new SqlCeCommand(cmdString, conn);
                //ExecuteScalar("Select Node_Name from  SuperNode_Node_Info where SuperNode_Id='" + PreSupIP + "' ;", CommandType.Text, null);*/
                //string NodeName = clientHttpChannelBS.GetBudyByIp(NodeIp);
                if (NodeName != null && NodeName != string.Empty && NodeIP != VMuktiAPI.VMuktiInfo.BootStrapIPs[0])
                {
                    if (!checkSuperNodeAvailable(NodeIP))
                    {
                        //fncSNDeleteBuddy(uName);
                        fncSNInsertBuddy(NodeName, "Offline");
                        fncSNDeleteNode(NodeName);
                        fncUpdateUserBuddyStatus(NodeName, "Offline");

                        clientNetP2pChannelBS.svcNetP2PRemoveUser(NodeName);
                        clientHttpChannelBS.svcHttpBSUnJoin(NodeName, NodeIP, true);
                    }
                }//end if


            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcGetNodeNameByIP", "Domains\\SuperNodeServiceDomain.cs");
            }
        }
        #endregion

        #region NetP2P SuperNode Functions


        void SuperNodeServiceDomain_EntsvcNetP2PReturnSuperNodeBuddyStatus(string uName, List<string> lstSNBuddyList)
        {
            try
            {
                if (VMuktiInfo.CurrentPeer.DisplayName == uName && blIsSNListPresent == false)
                {
                    for (int i = 0; i < lstSNBuddyList.Count; i++)
                    {
                        fncSNInsertBuddy(lstSNBuddyList[i].ToString(), "Online");
                    }
                    blIsSNListPresent = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PReturnSuperNodeBuddyStatus", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcNetP2PReturnBuddyInfo(string uName, List<string> lstMyBuddyList)
        {

        }

        /// <summary>
        /// This NetP2P function will be used for Removing any buddy who is offline or logout from the system.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="lstSNBuddyList"></param>
        void SuperNodeServiceDomain_EntsvcNetP2PRemoveUser(string uName)
        {
            try
            {
                if (uName != null && uName != string.Empty)
                {
                    // fncSNDeleteBuddy(uName);
                    fncSNInsertBuddy(uName, "Offline");
                    fncSNDeleteNode(uName);
                    fncUpdateUserBuddyStatus(uName, "Offline");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PRemoveUser()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcNetP2PGetSuperNodeBuddyStatus(string uName)
        {
            try
            {
                if (VMuktiInfo.CurrentPeer.DisplayName != uName)
                {
                    List<string> lsttemp = fncGetOnlineBuddies();
                    clientNetP2pChannelBS.svcNetP2PReturnSuperNodeBuddyStatus(uName, lsttemp);
                    //channelPeerBootStrap.svcNetP2PReturnSuperNodeBuddyStatus(uName, lsttemp);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2pGetSuperNodeBuddyStatus()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        void SuperNodeServiceDomain_EntsvcNetP2PGetBuddyInfo(string uName)
        {
            SqlCeConnection conn = new SqlCeConnection(ClientConnectionString);
            conn.Open();
            try
            {
                //ClsException.WriteToLogFile("BS NOde_status 15");
                string str = "SELECT * FROM Node_Status";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, conn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                List<string> lstOnlineBuddies = new List<string>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstOnlineBuddies.Add(dataTable.Rows[i][1].ToString());
                }
                conn.Close();
                conn.Dispose();
                clientNetP2pChannelBS.svcNetP2PReturnBuddyInfo(uName, lstOnlineBuddies);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PGetBuddyInfo()", "Domains\\SuperNodeServiceDomain.cs");
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// This NetP2P function will add new buddy to its database and make Online.
        /// </summary>
        /// <param name="uName"></param>
        void SuperNodeServiceDomain_EntsvcNetP2PAddUser(string uName)
        {
            try
            {
                if (uName != null && uName != string.Empty)
                {
                    fncSNInsertBuddy(uName, "Online");
                    fncUpdateUserBuddyStatus(uName, "Online");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PAddUser()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        /// <summary>
        /// This NetP2P function will add user's new buddy to its sdf file.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="BuddyName"></param>
        /// <param name="BuddyStatus"></param>
        void SuperNodeServiceDomain_EntsvcNetP2PAddBuddies(string username, string BuddyName, string BuddyStatus)
        {

            try
            {
                fncUpdateBuddyStatus(username, BuddyName, BuddyStatus);
                fncUpdateBuddyStatus(BuddyName, username, "Online");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2pAddBuddies()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        /// <summary>
        /// This NetP2P function will remove selected buddies for perticular user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="BuddyName"></param>
        void SuperNodeServiceDomain_EntsvcNetP2PRemoveBuddies(string username, List<string> BuddyName)
        {

            try
            {
                OpenConnection();

                foreach (string strBuddiesName in BuddyName)
                {
                    SqlCeCommand sqlcmd = new SqlCeCommand("delete from User_BuddyList where UserName='" + username + "' and BuddyName='" + strBuddiesName + "'", LocalSQLConn);
                    sqlcmd.ExecuteNonQuery();

                    SqlCeCommand sqlcmd1 = new SqlCeCommand("delete from User_BuddyList where UserName='" + strBuddiesName + "' and BuddyName='" + username + "'", LocalSQLConn);
                    sqlcmd1.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcNetP2PRemoveBuddies()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        #endregion


        #endregion

        #region Http SuperNode Events Handlers

        string[] SuperNodeServiceDomain_EntsvcStartAService(VMuktiAPI.PeerType PeerType, string ModId)
        {
            string p2puri = "";
            string httpuri = "";
            string strClassName = "";
            string strMethodName = "";

            try
            {
                DateTime TimeStamp = DateTime.Now.ToUniversalTime();
                string TimeStampIp = TimeStamp.ToString().Replace(":", "").Replace("/", "").Replace(" ", "").Replace("AM", "").Replace("PM", "") + TimeStamp.Millisecond.ToString() + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP;

                string strPortNo = string.Empty;
                if (intModule.ToString().Length == 1)
                {
                    strPortNo = "400";
                }
                else if (intModule.ToString().Length == 2)
                {
                    strPortNo = "40";
                }
                else
                {
                    strPortNo = "4";
                }

                strPortNo = "4000";

                //npsModules.Add(new NetPeerServer("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + intModule.ToString() + "/" + TimeStampIp));
                //npsModules[npsModules.Count - 1].AddEndPoint("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + intModule.ToString() + "/" + TimeStampIp);
                //npsModules[npsModules.Count - 1].Name = "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + intModule.ToString() + "/" + TimeStampIp;
                //npsModules[npsModules.Count - 1].OpenServer();

                //p2puri = "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + intModule.ToString() + "/" + TimeStampIp;

                npsModules.Add(new NetPeerServer("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + "/VMukti/" + TimeStampIp));
                npsModules[npsModules.Count - 1].AddEndPoint("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + "/VMukti/" + TimeStampIp);
                npsModules[npsModules.Count - 1].Name = "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + "/VMukti/" + TimeStampIp;
                npsModules[npsModules.Count - 1].OpenServer();

                p2puri = "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":" + strPortNo + "/VMukti/" + TimeStampIp;


                if (PeerType == PeerType.NodeWithHttp || PeerType == PeerType.NodeWithNetP2P)
                {
                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                    ClsPod podObj = new ClsPod();
                    podObj = ClsPod.GetModInfo(int.Parse(ModId));

                    string modulename = podObj.ZipFile.Replace(".zip", "");

                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                    string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                    XmlParser xp = new XmlParser();
                    xp.Parse(strXmlPath);

                    if (PeerType == PeerType.NodeWithHttp)
                    {
                        strClassName = xp.xMain.DummyClassName;
                        strMethodName = xp.xMain.DummyMethodName;
                    }
                    else if (PeerType == PeerType.NodeWithNetP2P)
                    {
                        strClassName = xp.xMain.P2PDummyClassName;
                        strMethodName = xp.xMain.P2PDummyMethodName;
                    }

                    #region Loading ReferencedAssemblies

                    al.Clear();

                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                    ShowDirectory(dirinfomodule);

                    for (int j = 0; j < al.Count; j++)
                    {
                        string[] arraysplit = al[j].ToString().Split('\\');
                        if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                            CopyToBase(modulename, podObj.AssemblyFile);

                            #region CreatingObject

                            for (int k = 0; k < t1.Length; k++)
                            {
                                if (t1[k].Name == strClassName)
                                {
                                    try
                                    {

                                        object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                        MethodInfo mi = t1[k].GetMethod(strMethodName);

                                        if (PeerType == PeerType.NodeWithHttp)
                                        {
                                            object[] objparams = new object[3];
                                            objparams[0] = intModule;
                                            objparams[1] = p2puri;
                                            objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                            httpuri = (mi.Invoke(obj1, objparams)).ToString();
                                        }
                                        else if (PeerType == PeerType.NodeWithNetP2P)
                                        {
                                            object[] objparams = new object[2];
                                            objparams[0] = intModule.ToString();
                                            objparams[1] = p2puri;
                                            mi.Invoke(obj1, objparams);
                                        }

                                    }

                                    catch (Exception exp)
                                    {
                                        MessageBox.Show("CreatingObject " + exp.Message);
                                    }
                                }
                            }
                            #endregion


                        }
                    }
                    #endregion

                }

                intModule++;
                return new string[] { p2puri, httpuri };
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcStartAservice()--1", "Domains\\SuperNodeServiceDomain.cs");
                if (ex.InnerException != null)
                {
                    VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcStartAService()--2", "Domains\\SuperNodeServiceDomain.cs");
                }

                return null;
            }
        }

        void SuperNodeServiceDomain_EntsvcSetSpecialMsg(string from, string to, string msg, clsModuleInfo objModInfo)
        {
            try
            {

                #region OPEN MODULE


                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == to)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = from;
                            objmsg.strTo = new string[] { to };
                            objmsg.strMessage = msg;
                            string p2puri = objModInfo.strUri[0];
                            string httpuri = "";

                            System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                            ClsPod podObj = new ClsPod();
                            podObj = ClsPod.GetModInfo(objModInfo.intModuleId);

                            string modulename = podObj.ZipFile.Replace(".zip", "");

                            string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                            string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                            XmlParser xp = new XmlParser();
                            xp.Parse(strXmlPath);

                            #region Loading ReferencedAssemblies

                            al.Clear();

                            DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                            ShowDirectory(dirinfomodule);

                            for (int j = 0; j < al.Count; j++)
                            {
                                string[] arraysplit = al[j].ToString().Split('\\');
                                if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                    CopyToBase(modulename, podObj.AssemblyFile);

                                    #region CreatingObject

                                    for (int k = 0; k < t1.Length; k++)
                                    {
                                        if (t1[k].Name == xp.xMain.DummyClassName)
                                        {
                                            try
                                            {

                                                object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                object[] objparams = new object[3];
                                                objparams[0] = intModule;
                                                objparams[1] = p2puri;
                                                objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                httpuri = (mi.Invoke(obj1, objparams)).ToString();
                                            }

                                            catch (Exception exp)
                                            {
                                                MessageBox.Show("CreatingObject " + exp.Message);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            objModInfo.strUri[1] = httpuri;
                            objmsg.objClsModuleInfo = objModInfo;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            al.Clear();
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                    }
                }
                if (!blnNodePresent)
                {
                    clientNetP2pChannelBS.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                }


                #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcSetspecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcPageSetSpecialMsg(clsPageInfo objPageInfo)
        {
            try
            {
                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == objPageInfo.strTo)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = objPageInfo.strFrom;
                            objmsg.strTo = new string[] { objPageInfo.strTo };
                            objmsg.strMessage = objPageInfo.strMsg;

                            for (int tCnt = 0; tCnt < objPageInfo.objaTabs.Length; tCnt++)
                            {
                                for (int lstCnt = 0; lstCnt < objPageInfo.objaTabs[tCnt].objaPods.Length; lstCnt++)
                                {
                                    string p2puri = objPageInfo.objaTabs[tCnt].objaPods[lstCnt].strUri[0];
                                    string httpuri = "";

                                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                                    ClsPod podObj = new ClsPod();
                                    podObj = ClsPod.GetModInfo(objPageInfo.objaTabs[tCnt].objaPods[lstCnt].intModuleId);

                                    string modulename = podObj.ZipFile.Replace(".zip", "");

                                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                                    string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                                    XmlParser xp = new XmlParser();
                                    xp.Parse(strXmlPath);

                                    #region Loading ReferencedAssemblies

                                    al.Clear();

                                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                                    ShowDirectory(dirinfomodule);

                                    for (int j = 0; j < al.Count; j++)
                                    {
                                        string[] arraysplit = al[j].ToString().Split('\\');
                                        if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                            CopyToBase(modulename, podObj.AssemblyFile);

                                            #region CreatingObject

                                            for (int k = 0; k < t1.Length; k++)
                                            {
                                                if (t1[k].Name == xp.xMain.DummyClassName)
                                                {
                                                    try
                                                    {

                                                        object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                        MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                        object[] objparams = new object[3];
                                                        objparams[0] = intModule;
                                                        objparams[1] = p2puri;
                                                        objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                        httpuri = (mi.Invoke(obj1, objparams)).ToString();
                                                    }

                                                    catch (Exception exp)
                                                    {
                                                        MessageBox.Show("CreatingObject " + exp.Message);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                    objPageInfo.objaTabs[tCnt].objaPods[lstCnt].strUri[1] = httpuri;
                                }
                            }
                            objmsg.objPageInfo = objPageInfo;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            al.Clear();
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            clientNetP2PChannelSN.svcSetSpecialMsg(objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                    }
                }
                if (!blnNodePresent)
                {
                    clientNetP2pChannelBS.svcSetSpecialMsg(objPageInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcPageSetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void SuperNodeServiceDomain_EntsvcSetSpecialMsg4MultipleBuddies(string from, List<string> to, string msg, clsModuleInfo objModInfo)
        {
            clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        }

        void SuperNodeServiceDomain_EntsvcSetSpecialMsgBuddiesClick(string from, List<string> to, string msg, clsModuleInfo objModInfo, clsPageInfo objPageInfo)
        {
            clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP, objPageInfo);
        }

        void SuperNodeServiceDomain_EntsvcAddDraggedBuddy(string from, string to, string msg, clsModuleInfo objModInfo)
        {
            try
            {
                bool blnIsNodeNotPresent = false;
                for (int BCnt = 0; BCnt < objModInfo.lstUsersDropped.Count; BCnt++) //Dragged Buddy List
                {
                    bool blnNodePresent = false;
                    for (int NCnt = 0; NCnt < lstmyNodes.Count; NCnt++) //Current Node's List
                    {
                        if (objModInfo.lstUsersDropped[BCnt] == lstmyNodes[NCnt].uname)
                        {
                            blnNodePresent = true;
                            if (lstmyNodes[NCnt].nodeBType == PeerType.NodeWithHttp)
                            {
                                clsMessage objMsg = new clsMessage();
                                objMsg.strFrom = to;
                                objMsg.strTo = new string[] { lstmyNodes[NCnt].uname };
                                objMsg.strMessage = "Newly Dragged Buddy";
                                objMsg.objClsModuleInfo = objModInfo;
                                lstmyNodes[NCnt].lstMyMsgs.Add(objMsg);
                                break;
                            }
                            else
                            {
                                clientNetP2PChannelSN.svcAddDraggedBuddy(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                        }
                    }
                    if (!blnNodePresent)
                    {
                        blnIsNodeNotPresent = true;
                    }
                }
                if (blnIsNodeNotPresent)
                {
                    clientNetP2pChannelBS.svcAddDraggedBuddy(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcAddDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void SuperNodeServiceDomain_EntsvcAddPageDraggedBuddy(string from, string to, string msg, List<clsModuleInfo> objModInfo)
        {
            try
            {
                bool blnIsNodeNotPresent = false;
                for (int BCnt = 0; BCnt < objModInfo[0].lstUsersDropped.Count; BCnt++) //Dragged Buddy List
                {
                    bool blnNodePresent = false;
                    for (int NCnt = 0; NCnt < lstmyNodes.Count; NCnt++) //Current Node's List
                    {
                        if (objModInfo[0].lstUsersDropped[BCnt] == lstmyNodes[NCnt].uname)
                        {
                            blnNodePresent = true;
                            if (lstmyNodes[NCnt].nodeBType == PeerType.NodeWithHttp)
                            {
                                clsMessage objMsg = new clsMessage();
                                objMsg.strFrom = to;
                                objMsg.strTo = new string[] { lstmyNodes[NCnt].uname };
                                objMsg.strMessage = "Newly Dragged Buddy";
                                objMsg.lstClsModuleInfo = objModInfo;
                                lstmyNodes[NCnt].lstMyMsgs.Add(objMsg);
                                break;
                            }
                            else
                            {
                                clientNetP2PChannelSN.svcAddDraggedBuddy(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                            }
                        }
                    }
                    if (!blnNodePresent)
                    {
                        blnIsNodeNotPresent = true;
                    }
                }
                if (blnIsNodeNotPresent)
                {
                    clientNetP2pChannelBS.svcAddDraggedBuddy(from, to, msg, objModInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcAddPageDraggedBuddy()--2", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void SuperNodeServiceDomain_EntsvcSetRemoveDraggedBuddy(string from, List<string> to, string msg, clsModuleInfo objModInfo)
        {
            try
            {
                #region CLOSE MODULE

                if (msg == "CLOSE MODULE")
                {
                    for (int j = 0; j < to.Count; j++)
                    {
                        bool blnNodePresent = false;
                        for (int i = 0; i < lstmyNodes.Count; i++)
                        {
                            if (lstmyNodes[i].uname == to[j])
                            {
                                if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                                {
                                    clsMessage objmsg = new clsMessage();
                                    objmsg.strFrom = from;
                                    objmsg.strTo = to.ToArray();
                                    objmsg.strMessage = msg;


                                    objmsg.objClsModuleInfo = objModInfo;
                                    lstmyNodes[i].lstMyMsgs.Add(objmsg);
                                    blnNodePresent = true;
                                    break;
                                }
                                else
                                {
                                    blnNodePresent = true;
                                    clientNetP2PChannelSN.svcSetRemoveDraggedBuddy(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                                    break;
                                }
                            }
                        }

                        if (!blnNodePresent)
                        {
                            clientNetP2pChannelBS.svcSetRemoveDraggedBuddy(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }


                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcSetRemoveDraggedBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        List<VMukti.Business.CommonDataContracts.clsMessage> SuperNodeServiceDomain_EntsvcGetSpecialMsgs(string uName)
        {
            try
            {
                //lock (this)
                //{
                // ClsException.WriteToLogFile("Request coming for Module-GetSpMsg of buddy :: " + uName + "...StartTime :: " + DateTime.Now.ToString());
                List<clsMessage> tmpMsg = new List<clsMessage>();
                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == uName)
                    {
                        for (int j = 0; j < lstmyNodes[i].lstMyMsgs.Count; j++)
                        {
                            tmpMsg.Add(lstmyNodes[i].lstMyMsgs[j]);
                            lstmyNodes[i].lstMyMsgs.RemoveAt(j);
                        }
                        break;
                    }
                }
                // ClsException.WriteToLogFile("Request coming for Module-GetSpMsg of buddy :: " + uName + "...EndTime :: " + DateTime.Now.ToString());
                return tmpMsg;
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntsvcGetSpecialMsg()", "Domains\\SuperNodeServiceDomain.cs");
                return null;
            }
        }

        bool SuperNodeServiceDomain_EntIsSuperNodeAvailable()
        {
            return true;
        }

        string[] SuperNodeServiceDomain_EntIsAuthorizedUser(clsBootStrapInfo objBootStrapInfo, string strUserName, string strPassword)
        {
            try
            {
                bool AuthResult = false;
                VMukti.Business.ClsUser CurrenUser = null;

                if (objBootStrapInfo.AuthType == VMuktiAPI.AuthType.SQLAuthentication.ToString())
                {
                    VMuktiAPI.VMuktiInfo.MainConnectionString = objBootStrapInfo.ConnectionString;

                    CurrenUser = VMukti.Business.ClsUser.GetByUNamePass(strUserName, strPassword, ref AuthResult);
                    if (CurrenUser == null)
                    {
                        if (AuthResult)
                        {
                            return new string[] { "WrongPassword" };
                        }
                        else
                        {
                            return new string[] { "WrongUserName" };
                        }
                    }
                    else
                    {
                        List<string> UserInfo = new List<string>();
                        UserInfo.Add("ID=" + CurrenUser.ID.ToString());
                        UserInfo.Add("DisplayName=" + CurrenUser.DisplayName.ToString());
                        UserInfo.Add("RoleID=" + CurrenUser.RoleID.ToString());
                        UserInfo.Add("FName=" + CurrenUser.FName.ToString());
                        UserInfo.Add("LName=" + CurrenUser.LName.ToString());
                        UserInfo.Add("EMail=" + CurrenUser.EMail.ToString());
                        UserInfo.Add("PassWord=" + CurrenUser.PassWord.ToString());
                        UserInfo.Add("IsActive=" + CurrenUser.IsActive.ToString());
                        UserInfo.Add("Status=Online");
                        UserInfo.Add("RoleName=" + CurrenUser.RoleName.ToString());

                        fncSNInsertBuddy(CurrenUser.DisplayName.ToString(), "Online");
                        fncSNInsertNode(CurrenUser.DisplayName.ToString());
                        fncUpdateUserBuddyStatus(CurrenUser.DisplayName.ToString(), "Online");
                        clientNetP2pChannelBS.svcNetP2PAddUser(CurrenUser.DisplayName.ToString());

                        try
                        {
                            bool blnNodePresent = false;
                            for (int lstCnt = 0; lstCnt < lstmyNodes.Count; lstCnt++)
                            {
                                if (lstmyNodes[lstCnt].uname == CurrenUser.DisplayName)
                                {
                                    blnNodePresent = true;
                                    break;
                                }
                            }
                            if (!blnNodePresent)
                            {
                                lstmyNodes.Add(new clsNodeInfo(CurrenUser.DisplayName, PeerType.NodeWithHttp));
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "SuperNodeServiceDomain_EntIsAuthorizedUser()", "Domains\\SuperNodeServiceDomain.cs");
                        }


                        return UserInfo.ToArray();
                        //((Page)((ScrollViewer)((Canvas)this.Parent).Parent).Parent).NavigationService.Navigate(new Uri("pgHome.xaml", UriKind.RelativeOrAbsolute));
                        //LoginService.svcUnJoin(txtUserNameID.Text);
                        //LoginService.Close();
                    }
                }
                else if (objBootStrapInfo.AuthType == VMuktiAPI.AuthType.SIPAuthentication.ToString())
                {
                    lstObjRTCAuthClient.Add(new clsRTCAuthClient(objBootStrapInfo.SIPUserNumber, strPassword, objBootStrapInfo.AuthServerIP));
                    return new string[] { "CheckStatus" };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        string SuperNodeServiceDomain_EntIsAuthorized(string strUserName)
        {
            return null;
            //for (int i = 0; i < lstObjRTCAuthClient.Count; i++)
            //{
            //    return null;
            //}
        }

        string SuperNodeServiceDomain_EntsvcAddSIPUser()
        {
            return objPBX.FncCreateSIPUser();
        }

        string SuperNodeServiceDomain_EntsvcGetConferenceNumber()
        {
            return objPBX.FncCreateConference();
        }

        void SuperNodeServiceDomain_EntsvcRemoveSIPUser(string strSIPNumber)
        {
            objPBX.FncRemoveSIPUser(strSIPNumber);
        }

        void objSNDel_EntsvcSendConfInfo(string from, string to, int confid, clsPageInfo pageinfo, string ipaddress)
        {
            try
            {
                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == to)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = from;
                            objmsg.strTo = new string[] { to };
                            objmsg.strMessage = "SendConfInfo";
                            //objmsg.objPageInfo = pageinfo;

                            for (int tCnt = 0; tCnt < pageinfo.objaTabs.Length; tCnt++)
                            {
                                for (int lstCnt = 0; lstCnt < pageinfo.objaTabs[tCnt].objaPods.Length; lstCnt++)
                                {
                                    string p2puri = pageinfo.objaTabs[tCnt].objaPods[lstCnt].strUri[0];
                                    string httpuri = "";

                                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(SuperNodeServiceDomain));

                                    ClsPod podObj = new ClsPod();
                                    podObj = ClsPod.GetModInfo(pageinfo.objaTabs[tCnt].objaPods[lstCnt].intModuleId);

                                    string modulename = podObj.ZipFile.Replace(".zip", "");

                                    string strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"Modules");

                                    string strXmlPath = strModPath + "\\" + modulename + "\\Control\\configuration.xml";

                                    XmlParser xp = new XmlParser();
                                    xp.Parse(strXmlPath);

                                    #region Loading ReferencedAssemblies

                                    al.Clear();

                                    DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + modulename);
                                    ShowDirectory(dirinfomodule);

                                    for (int j = 0; j < al.Count; j++)
                                    {
                                        string[] arraysplit = al[j].ToString().Split('\\');
                                        if (arraysplit[arraysplit.Length - 1].ToString() == podObj.AssemblyFile)
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

                                            CopyToBase(modulename, podObj.AssemblyFile);

                                            #region CreatingObject

                                            for (int k = 0; k < t1.Length; k++)
                                            {
                                                if (t1[k].Name == xp.xMain.DummyClassName)
                                                {
                                                    try
                                                    {

                                                        object obj1 = Activator.CreateInstance(t1[k], VMuktiInfo.CurrentPeer.DisplayName);
                                                        MethodInfo mi = t1[k].GetMethod(xp.xMain.DummyMethodName);

                                                        object[] objparams = new object[3];
                                                        objparams[0] = intModule;
                                                        objparams[1] = p2puri;
                                                        objparams[2] = VMuktiInfo.CurrentPeer.SuperNodeIP;
                                                        httpuri = (mi.Invoke(obj1, objparams)).ToString();
                                                    }

                                                    catch (Exception exp)
                                                    {
                                                        MessageBox.Show("Error in creating a dynamic object - " + exp.Message);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    pageinfo.objaTabs[tCnt].objaPods[lstCnt].strUri[1] = httpuri;
                                }
                            }
                            objmsg.objPageInfo = pageinfo;


                            objmsg.confid = confid;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            clientNetP2PChannelSN.svcSendConfInfo(from, to, confid, pageinfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                    }
                }
                if (!blnNodePresent)
                {
                    clientNetP2pChannelBS.svcSendConfInfo(from, to, confid, pageinfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objSNDel_EntsvcSendConfInfo()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }


        void objSNDel_EntsvcJoinConf(string from, string to, int confid, string ipaddress)
        {

            try
            {
                bool blnNodePresent = false;

                for (int i = 0; i < lstmyNodes.Count; i++)
                {
                    if (lstmyNodes[i].uname == to)
                    {
                        if (lstmyNodes[i].nodeBType == PeerType.NodeWithHttp)
                        {
                            clsMessage objmsg = new clsMessage();
                            objmsg.strFrom = from;
                            objmsg.strTo = new string[] { to };
                            objmsg.strMessage = "JoinConf";
                            objmsg.confid = confid;
                            lstmyNodes[i].lstMyMsgs.Add(objmsg);
                            blnNodePresent = true;
                            break;
                        }
                        else
                        {
                            blnNodePresent = true;
                            //clientNetP2PChannelSN.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                            clientNetP2PChannelSN.svcJoinConf(from, to, confid, VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                    }
                }
                if (!blnNodePresent)
                {
                    //clientNetP2pChannelBS.svcSetSpecialMsg(from, to, msg, objModInfo, VMuktiInfo.CurrentPeer.SuperNodeIP);
                    clientNetP2pChannelBS.svcJoinConf(from, to, confid, VMuktiInfo.CurrentPeer.SuperNodeIP);
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objSNDel_EntsvcJoinConf()", "Domains\\SuperNodeServiceDomain.cs");
            }


        }

        void objSNDel_EntsvcAddConfBuddy(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "AddConfBuddy";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                    for (int tempCntP2P = 0; tempCntP2P < lsttempNodes.Count; tempCntP2P++)
                    {
                        if (lsttempNodes[tempCntP2P].nodeBType == PeerType.NodeWithNetP2P || lsttempNodes[tempCntP2P].nodeBType == PeerType.SuperNode || lsttempNodes[tempCntP2P].nodeBType == PeerType.BootStrap)
                        {
                            clientNetP2PChannelSN.svcAddConfBuddy(from, lstBuddies, confid, ipaddress);
                            break;
                        }
                    }
                }

                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcAddConfBuddy(from, lstBuddies, confid, ipaddress);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "objSNDel_EntsvcAddConfBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void objSNDel_EntsvcRemoveConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "RemoveBuddyConf";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                    for (int tempCntP2P = 0; tempCntP2P < lsttempNodes.Count; tempCntP2P++)
                    {
                        if (lsttempNodes[tempCntP2P].nodeBType == PeerType.NodeWithNetP2P || lsttempNodes[tempCntP2P].nodeBType == PeerType.SuperNode || lsttempNodes[tempCntP2P].nodeBType == PeerType.BootStrap)
                        {
                            clientNetP2PChannelSN.svcRemoveConf(from, lstBuddies, confid, ipaddress);
                            break;
                        }
                    }
                }

                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcRemoveConf(from, lstBuddies, confid, ipaddress);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "objSNDel_EntsvcRemoveConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        void objSNDel_EntsvcUnJoinConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {

                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "UnJoinConf";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                    for (int tempCntP2P = 0; tempCntP2P < lsttempNodes.Count; tempCntP2P++)
                    {
                        if (lsttempNodes[tempCntP2P].nodeBType == PeerType.NodeWithNetP2P || lsttempNodes[tempCntP2P].nodeBType == PeerType.SuperNode || lsttempNodes[tempCntP2P].nodeBType == PeerType.BootStrap)
                        {
                            clientNetP2PChannelSN.svcUnJoinConf(from, lstBuddies, confid, ipaddress);
                            break;
                        }
                    }
                }

                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcUnJoinConf(from, lstBuddies, confid, ipaddress);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "objSNDel_EntsvcUnJoinConf()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void objSNDel_EntsvcEnterConf(string from, List<string> lstBuddies, int confid, string ipaddress)
        {
            try
            {
                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        objmsg.confid = confid;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "EnterConf";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                    for (int tempCntP2P = 0; tempCntP2P < lsttempNodes.Count; tempCntP2P++)
                    {
                        if (lsttempNodes[tempCntP2P].nodeBType == PeerType.NodeWithNetP2P || lsttempNodes[tempCntP2P].nodeBType == PeerType.SuperNode || lsttempNodes[tempCntP2P].nodeBType == PeerType.BootStrap)
                        {
                            clientNetP2PChannelSN.svcEnterConf(from, lstBuddies, confid, ipaddress);
                            break;
                        }
                    }

                }

                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcEnterConf(from, lstBuddies, confid, ipaddress);
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objSNDel_EntsvcEnterConf", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void objSNDel_EntsvcPodNavigation(string from, List<string> lstBuddies, int pageid, int tabid, int podid, string ipaddress)
        {
            try
            {
                List<clsNodeInfo> lsttempNodes = new List<clsNodeInfo>();

                for (int i = 0; i < lstBuddies.Count; i++)
                {
                    for (int j = 0; j < lstmyNodes.Count; j++)
                    {
                        if (lstmyNodes[j].uname == lstBuddies[i])
                        {
                            lsttempNodes.Add(lstmyNodes[j]);
                            break;
                        }
                    }
                }

                for (int tempCnt = 0; tempCnt < lsttempNodes.Count; tempCnt++)
                {
                    if (lsttempNodes[tempCnt].nodeBType == PeerType.NodeWithHttp)
                    {
                        clsMessage objmsg = new clsMessage();
                        objmsg.strFrom = from;
                        clsModuleInfo obj = new clsModuleInfo();
                        obj.strPageid = pageid.ToString();
                        obj.strTabid = tabid.ToString();
                        obj.strPodid = podid.ToString();
                        objmsg.objClsModuleInfo = obj;
                        objmsg.strTo = lstBuddies.ToArray();
                        objmsg.strMessage = "PodNavigate";
                        lsttempNodes[tempCnt].lstMyMsgs.Add(objmsg);
                    }
                }

                if (lsttempNodes.Count == lstBuddies.Count)
                {
                    for (int tempCntP2P = 0; tempCntP2P < lsttempNodes.Count; tempCntP2P++)
                    {
                        if (lsttempNodes[tempCntP2P].nodeBType == PeerType.NodeWithNetP2P || lsttempNodes[tempCntP2P].nodeBType == PeerType.SuperNode || lsttempNodes[tempCntP2P].nodeBType == PeerType.BootStrap)
                        {
                            clientNetP2PChannelSN.svcPodNavigation(from, lstBuddies,pageid,tabid,podid, ipaddress);
                            break;
                        }
                    }

                }

                else
                {
                    if (ipaddress.Substring(0, 1) != "~")
                    {
                        clientNetP2pChannelBS.svcPodNavigation(from, lstBuddies,pageid,tabid, podid, ipaddress);
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objSNDel_EntsvcPodNavigation", "Domains\\SuperNodeServiceDomain.cs");
            }
        }


        #endregion

        #region DataBase Creation Function


        public void fncCreateBuddyStatusTable()
        {
            try
            {


                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                //ClsException.WriteToLogFile("BS NOde_status 16");
                if (!IsTableExits("Node_Status"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    //ClsException.WriteToLogFile("BS NOde_status 17");
                    SyncTable tblNode_Status = new SyncTable("Node_Status");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    //ClsException.WriteToLogFile("BS NOde_status 18");
                    syncSchemaLead.Tables.Add("Node_Status");
                    syncSchemaLead.Tables["Node_Status"].Columns.Add("Buddy_ID");
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Node_Status"].PrimaryKey = new string[] { "Buddy_ID" };

                    syncSchemaLead.Tables["Node_Status"].Columns.Add("Buddy_Name");
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_Name"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_Name"].MaxLength = 30;

                    syncSchemaLead.Tables["Node_Status"].Columns.Add("Buddy_Status");
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_Status"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_Status"].MaxLength = 30;

                    syncSchemaLead.Tables["Node_Status"].Columns.Add("Buddy_TimeStamp");
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_TimeStamp"].ProviderDataType = SqlDbType.DateTime.ToString();
                    syncSchemaLead.Tables["Node_Status"].Columns["Buddy_TimeStamp"].MaxLength = 30;


                    sync.CreateSchema(tblNode_Status, syncSchemaLead);
                }
                if (VMuktiInfo.CurrentPeer.DisplayName != null && VMuktiInfo.CurrentPeer.DisplayName != string.Empty)
                {
                    fncSNInsertBuddy(VMuktiInfo.CurrentPeer.DisplayName, "Online");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncCreateBuddyStatusTable()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        public void fncCreateSNNodeInfoTable()
        {
            try
            {

                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("Node_Info"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblNode_Info = new SyncTable("Node_Info");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("Node_Info");
                    syncSchemaLead.Tables["Node_Info"].Columns.Add("Node_ID");
                    syncSchemaLead.Tables["Node_Info"].Columns["Node_ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["Node_Info"].Columns["Node_ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["Node_Info"].Columns["Node_ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["Node_Info"].Columns["Node_ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["Node_Info"].PrimaryKey = new string[] { "Node_ID" };

                    syncSchemaLead.Tables["Node_Info"].Columns.Add("Node_Name");
                    syncSchemaLead.Tables["Node_Info"].Columns["Node_Name"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["Node_Info"].Columns["Node_Name"].MaxLength = 30;

                    sync.CreateSchema(tblNode_Info, syncSchemaLead);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncCreateSNNodeInfoTable()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        public void fncCreateUserBuddyListTable()
        {
            try
            {

                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("User_BuddyList"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblUser_BuddyList = new SyncTable("User_BuddyList");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("User_BuddyList");
                    syncSchemaLead.Tables["User_BuddyList"].Columns.Add("ID");
                    syncSchemaLead.Tables["User_BuddyList"].Columns["ID"].ProviderDataType = SqlDbType.BigInt.ToString();
                    syncSchemaLead.Tables["User_BuddyList"].Columns["ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["User_BuddyList"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["User_BuddyList"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["User_BuddyList"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["User_BuddyList"].Columns.Add("UserName");
                    syncSchemaLead.Tables["User_BuddyList"].Columns["UserName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["User_BuddyList"].Columns["UserName"].MaxLength = 30;
                    syncSchemaLead.Tables["User_BuddyList"].Columns["UserName"].AllowNull = false;


                    syncSchemaLead.Tables["User_BuddyList"].Columns.Add("BuddyName");
                    syncSchemaLead.Tables["User_BuddyList"].Columns["BuddyName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["User_BuddyList"].Columns["BuddyName"].MaxLength = 30;
                    syncSchemaLead.Tables["User_BuddyList"].Columns["BuddyName"].AllowNull = false;

                    //syncSchemaLead.Tables["User_BuddyList"].Columns.Add("BuddyStatus");
                    //syncSchemaLead.Tables["User_BuddyList"].Columns["BuddyStatus"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    //syncSchemaLead.Tables["User_BuddyList"].Columns["BuddyStatus"].MaxLength = 20;
                    //syncSchemaLead.Tables["User_BuddyList"].Columns["BuddyStatus"].AllowNull = false;


                    sync.CreateSchema(tblUser_BuddyList, syncSchemaLead);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncCreateUserBuddyListTable()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        public bool IsTableExits(string strTableName)
        {

            try
            {

                OpenConnection();
                string str = "SELECT COUNT(*) FROM Information_Schema.Tables WHERE (TABLE_NAME ='" + strTableName + "')";
                SqlCeCommand cmd = new SqlCeCommand(str, LocalSQLConn);
                object i = cmd.ExecuteScalar();

                if (int.Parse(i.ToString()) > 0)
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
                VMuktiHelper.ExceptionHandler(ex, "IsTableExits()", "Domains\\SuperNodeServiceDomain.cs");
                return false;
            }
        }

        private bool IsRecordExists(string strBuddyName, string strTableName, string strPrimaryKey)
        {
            try
            {
                OpenConnection();
                string str = "SELECT COUNT(*) FROM " + strTableName + " WHERE (" + strPrimaryKey + " ='" + strBuddyName + "')";
                SqlCeCommand cmd = new SqlCeCommand(str, LocalSQLConn);

                object i = cmd.ExecuteScalar();
                if (int.Parse(i.ToString()) > 0)
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
                VMuktiHelper.ExceptionHandler(ex, "IsRecordExists()", "Domains\\SuperNodeServiceDomain.cs");
                return false;
            }

        }

        public void fncSNInsertBuddy(string strBuddyName, string strBuddyStatus)
        {
            //	SqlCeConnection ceConn = new SqlCeConnection(ClientConnectionString);
            try
            {
                //StringBuilder sb2 = new StringBuilder();
                //ClsException.WriteToLogFile("BS NOde_status 19");
                if (!IsRecordExists(strBuddyName, "Node_Status", "Buddy_Name"))
                {
                    //	ceConn.Open();
                    OpenConnection();
                    //ClsException.WriteToLogFile("BS NOde_status 20");

                    //sb2.AppendLine("inserting nodestatus");
                    //sb2.AppendLine();
                    //sb2.AppendLine("Location : fncSNInsertBuddy()");
                    //sb2.AppendLine();


                    string str = "insert into Node_Status(Buddy_Name,Buddy_Status,Buddy_TimeStamp) values(@Buddy_Name,@Buddy_Status,@Buddy_TimeStamp) ";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Buddy_Name", strBuddyName);
                    cmd.Parameters.AddWithValue("@Buddy_Status", strBuddyStatus);
                    if (strBuddyStatus.ToLower() != "online")
                    {
                        cmd.Parameters.AddWithValue("@Buddy_TimeStamp", DateTime.Now.Subtract(TimeSpan.FromMinutes(5)));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Buddy_TimeStamp", DateTime.Now);
                    }
                    cmd.ExecuteNonQuery();
                    //	ceConn.Close();
                    //	ceConn.Dispose();
                }
                else
                {
                    //	ceConn.Open();
                    OpenConnection();
                    //ClsException.WriteToLogFile("BS NOde_status 21");

                    //sb2.AppendLine("updating status and timestamp");
                    //sb2.AppendLine();
                    //sb2.AppendLine("Location : fncSNInsertBuddy1()");
                    //sb2.AppendLine();


                    //string str = "Update Node_Status Set Buddy_Status='" + strBuddyStatus + "',Buddy_TimeStamp = getdate() Where Buddy_Name='" + strBuddyName + "'";
                    string str = "";
                    if (strBuddyStatus.ToLower() != "online")
                    {
                        str = "Update Node_Status Set Buddy_Status='" + strBuddyStatus + "',Buddy_TimeStamp = '" + DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) + "' Where Buddy_Name = '" + strBuddyName + "'";
                    }
                    else
                    {
                        str = "Update Node_Status Set Buddy_Status='" + strBuddyStatus + "',Buddy_TimeStamp = getdate() Where Buddy_Name = '" + strBuddyName + "'";
                    }


                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;
                    cmd.ExecuteNonQuery();
                    //	ceConn.Close();
                    //	ceConn.Dispose();
                }
                // MessageBox.Show("Buddy " + strBuddyName + " is " + strBuddyStatus,"SuperNode Domain");
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb2);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNInsertBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        private void fncSNDeleteBuddy(string strBuddyName)
        {
            //SqlCeConnection ceConn = new SqlCeConnection(ClientConnectionString);
            try
            {
                //ClsException.WriteToLogFile("BS NOde_status 22");
                if (IsRecordExists(strBuddyName, "Node_Status", "Buddy_Name"))
                {
                    //	ceConn.Open();
                    OpenConnection();
                    //ClsException.WriteToLogFile("BS NOde_status 23");
                    string str = "delete from Node_Status where Buddy_Name = @Buddy_Name";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Buddy_Name", strBuddyName);
                    cmd.ExecuteNonQuery();
                    //	ceConn.Close();
                    //	ceConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNDeleteBuddy()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        private void fncSNInsertNode(string uName)
        {
            //	SqlCeConnection ceConn = new SqlCeConnection(ClientConnectionString);
            try
            {
                if (!IsRecordExists(uName, "Node_Info", "Node_Name"))
                {

                    //		ceConn.Open();
                    OpenConnection();
                    string str = "insert into Node_Info(Node_Name) values(@Node_Name) ";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Node_Name", uName);
                    cmd.ExecuteNonQuery();
                    //		ceConn.Close();
                    //		ceConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNInsertNode()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        private void fncSNDeleteNode(string uName)
        {
            //	SqlCeConnection ceConn = new SqlCeConnection(ClientConnectionString);
            try
            {
                if (IsRecordExists(uName, "Node_Info", "Node_Name"))
                {
                    //	ceConn.Open();
                    OpenConnection();
                    string str = "delete from Node_Info where Node_Name = @Node_Name";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Node_Name", uName);
                    cmd.ExecuteNonQuery();
                    //	ceConn.Close();
                    //	ceConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNDeleteNode()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        private List<string> fncGetOnlineBuddies()
        {
            //SqlCeConnection SNConn = null;
            try
            {
                //	ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "SuperNodeBuddyInfo.sdf";
                //	strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "SuperNodeBuddyInfo.sdf";

                if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                //	SNConn = new SqlCeConnection(ClientConnectionString);
                //	SNConn.Open();
                OpenConnection();
                SqlCeDataAdapter sqlDataAdap = null;
                //ClsException.WriteToLogFile("BS NOde_status 24");
                sqlDataAdap = new SqlCeDataAdapter("SELECT * FROM Node_Status", LocalSQLConn);
                DataTable myDT = new DataTable();
                sqlDataAdap.Fill(myDT);
                List<string> lstOnlineBuddyList = new List<string>();
                for (int i = 0; i < myDT.Rows.Count; i++)
                {
                    lstOnlineBuddyList.Add(myDT.Rows[i][1].ToString());
                }
                sqlDataAdap.Dispose();
                //	SNConn.Close();
                //	SNConn.Dispose();
                return lstOnlineBuddyList;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncGetOnlineBuddies()", "Domains\\SuperNodeServiceDomain.cs");
                return null;
            }
        }

        private void fncUpdateUserBuddyStatus(string BuddName, string status)
        {
            //SqlCeConnection ceConn = new SqlCeConnection(ClientConnectionString);
            try
            {
                //ceConn.Open();
                //string strCommand = "Select Count(*) From User_BuddyList Where UserName = '" + userName + "' and BuddyName = '" + BuddName + "'";

                //SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM User_BuddyList WHERE (UserName = '" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "') AND (BuddyName = '" + BuddName + "')", ceConn);
                //object i = cmd.ExecuteScalar();
                //if (int.Parse(i.ToString()) > 0)
                //{
                //    cmd = new SqlCeCommand("Update User_BuddyList Set BuddyStatus='" + status + "' Where (UserName = '" + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "') AND (BuddyName = '" + BuddName + "')", ceConn);
                //    cmd.ExecuteNonQuery();
                //}
                OpenConnection();
                //SqlCeCommand cmd = new SqlCeCommand("Update User_BuddyList Set BuddyStatus='" + status + "' Where BuddyName = '" + BuddName + "'", LocalSQLConn);
                //StringBuilder sb2 = new StringBuilder();
                //sb2.AppendLine("updating status  ");
                //sb2.AppendLine();
                //sb2.AppendLine("Location : fncUpdateUserBuddyStatus()");
                //sb2.AppendLine();
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb2);

                //SqlCeCommand cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = getdate() Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                SqlCeCommand cmd;
                if (status.ToLower().ToString() != "online")
                {
                    cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = '" + DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) + "' Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                }
                else
                {
                    cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = getdate() Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                }


                cmd.ExecuteNonQuery();
                //	ceConn.Close();
                //	ceConn.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncUpdateUserBuddyStatus()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        private void fncUpdateBuddyStatus(string userName, string BuddName, string status) //done
        {
            //	SqlCeConnection ceConn = new SqlCeConnection(ClientConnectionString);
            try
            {
                //	ceConn.Open();
                //string strCommand = "Select Count(*) From User_BuddyList Where UserName = '" + userName + "' and BuddyName = '" + BuddName + "'";
                //StringBuilder sb2 = new StringBuilder();
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM User_BuddyList WHERE (UserName = '" + userName + "') AND (BuddyName = '" + BuddName + "')", LocalSQLConn);
                object i = cmd.ExecuteScalar();
                if (int.Parse(i.ToString()) > 0)
                {

                    //sb2.AppendLine("updating status ");
                    //sb2.AppendLine();
                    //sb2.AppendLine("Location : fncUpdateBuddyStatus()1");
                    //sb2.AppendLine();


                    //cmd = new SqlCeCommand("Update User_BuddyList Set BuddyStatus ='" + status + "' Where (UserName = '" + userName + "') AND (BuddyName = '" + BuddName + "')", LocalSQLConn);
                    //cmd = new SqlCeCommand("update Node_Status set Buddy_Status='" + status + "' where (Buddy_Name='" + BuddName + "')", LocalSQLConn);

                    if (status.ToLower() != "online")
                    {
                        cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = '" + DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) + "' Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                    }
                    else
                    {
                        cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status='" + status + "',Buddy_TimeStamp = getdate() Where Buddy_Name = '" + BuddName + "'", LocalSQLConn);
                    }

                    cmd.ExecuteNonQuery();

                }
                else
                {

                    //sb2.AppendLine("updating status ");
                    //sb2.AppendLine();
                    //sb2.AppendLine("Location : fncUpdateBuddyStatus()2");
                    //sb2.AppendLine();


                    string str = "insert into User_BuddyList(UserName,BuddyName) values(@UserName,@BuddyName) ";
                    cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@BuddyName", BuddName);
                    //cmd.Parameters.AddWithValue("@BuddyStatus", status);
                    cmd.ExecuteNonQuery();
                    //VMuktiAPI.ClsLogging.WriteToTresslog(sb2);
                }
                //ceConn.Close();
                //ceConn.Dispose();
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncUpdateBuddyStatus()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void OpenConnection()
        {
            try
            {
                if (LocalSQLConn.State != ConnectionState.Open)
                {
                    LocalSQLConn.Open();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "openConnection()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        private bool fncSNNodeStatusTimeStamp(string uname)
        {
            try
            {
                OpenConnection();
                string str = "SELECT COUNT(*) FROM Node_Status WHERE (Buddy_Name ='" + uname + "')";
                SqlCeCommand cmd = new SqlCeCommand(str, LocalSQLConn);

                object i = cmd.ExecuteScalar();
                if (int.Parse(i.ToString()) > 0)
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
                VMuktiHelper.ExceptionHandler(ex, "dncSNNodeStatusTimeStamp()", "Domains\\SuperNodeServiceDomain.cs");
                return false;
            }
        }

        private void fncSNUpdateNodeStatusTimeStamp(string uname)
        {

        }

        #endregion

        #region Disaster Recovery

        void timerBuzzSN_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP)
                {
                    clientNetP2pChannelBS.svcBuzzSuperNode(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                }
            }
            catch
            {
            }
        }

        void dtWebReqBS4SN_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //System.Text.StringBuilder sb = new StringBuilder();
            dtWebReqBS4SN.Stop();
            Uri objuri = new Uri("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":" + "80/HttpBootStrap");

            System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(objuri);
            System.Net.WebResponse objResp;
            try
            {
                objResp = objWebReq.GetResponse();
                objResp.Close();
                //ClsException.WriteToLogFile("NetAvail(dtWebReqBS4SN_Elapsed--1--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
                if (NetAvail)
                {
                    NetAvail = false;
                    //ClsException.WriteToLogFile("NetAvail(dtWebReqBS4SN_Elapsed--2--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
                    LoadSuperNodeConstructor();

                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt", FileMode.Open, FileAccess.Write, FileShare.ReadWrite);


                    StreamWriter sWriter = new StreamWriter(fs);

                    fs.SetLength(0);

                    sWriter.Write("Initializing");
                    sWriter.Flush();
                    sWriter.Close();
                    fs.Close();

                }
            }
            catch
            {
                //ClsException.WriteToLogFile("NetAvail(dtWebReqBS4SN_Elapsed--3--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
                if (!NetAvail)
                {
                    //ClsException.WriteToLogFile("NetAvail(dtWebReqBS4SN_Elapsed--4--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);
                    NetAvail = true;
                    //ClsException.WriteToLogFile("NetAvail(dtWebReqBS4SN_Elapsed--5--) Status is " + NetAvail.ToString() + "  :  " + DateTime.Now);


                    UnloadSuperNodeConstructor();

                    StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt");
                    sWriter.Write("SuperNodeDomain");
                    sWriter.Close();


                    BuddyOffline();

                    //sb.AppendLine("clearing the hashtable");
                    //sb.AppendLine();
                    //sb.AppendLine("Location : dtWebReqBS4SN_Elapsed()");
                    //sb.AppendLine();

                    //hsOnlineBuddyTable.Clear();

                    lstOfflinebuddies.Clear();
                    hsSuperNodeInfo.Clear();
                }

            }
            finally
            {
                dtWebReqBS4SN.Start();
            }
            //VMuktiAPI.ClsLogging.WriteToTresslog(sb);
        }

        bool WebReqBootStrap()
        {
            Uri objuri = new Uri("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":" + "80/HttpBootStrap");
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

        void BuddyOffline()
        {
            try
            {
                List<string> lstOnlineBuddies = new List<string>();
                OpenConnection();
                //ClsException.WriteToLogFile("BS NOde_status 25");
                string str = "SELECT Buddy_Name FROM Node_Status";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, LocalSQLConn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstOnlineBuddies.Add(dataTable.Rows[i]["Buddy_Name"].ToString());
                }

                foreach (string userName in lstOnlineBuddies)
                {
                    fncSNInsertBuddy(userName, "Offline");
                    fncSNDeleteNode(userName);
                    fncUpdateUserBuddyStatus(userName, "Offline");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyOffline()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void ostat_Offline(object sender, EventArgs e)
        {
            try
            {
                return;

                //    try
                //    {
                //        string NodeName = clientHttpChannelBS.svcGetOfflineNodeName(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                //        if (NodeName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                //        {
                //            ClsException.WriteToLogFile("Supernode :: " + NodeName + " is offline from Offline Event");
                //            clientNetP2pChannelBS.svcNetP2PRemoveUser(NodeName);
                //            clientHttpChannelBS.svcHttpBSUnJoin(NodeName, "", true);
                //        }

                //    }
                //    catch
                //    {
                //        if (WebReqBootStrap())
                //        {

                //        }
                //        else
                //        {
                //            /********** SuperNode's Internet Disconnected **********/
                //            //Unload the SuperNode's Constructor
                //            //SuperNodeDomain in txt file

                //            UnloadSuperNodeConstructor();

                //            StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt");
                //            sWriter.Write("SuperNodeDomain");
                //            sWriter.Close();

                //            BuddyOffline();

                //            dtWebReqBS4SN.Start();

                //        }
                //    }


            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ostat_Offline()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void LoadSuperNodeConstructor()
        {
            try
            {
                try
                {
                    npsSuperNodeServer = new NetPeerServer("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode");
                    npsSuperNodeServer.AddEndPoint("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode");
                    npsSuperNodeServer.OpenServer();
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()", "Domains\\SuperNodeServiceDomain.cs");
                }
                try
                {
                    clientNetP2pChannelBS = (INetP2PBootStrapChannel)npcBootStrapClient.OpenClient<INetP2PBootStrapChannel>("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrap", "P2PBootStrapMesh", ref objNetP2PBootStrap);
                    clientNetP2pChannelBS.svcNetP2PServiceJoin(VMuktiInfo.CurrentPeer.SuperNodeIP);
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()", "Domains\\SuperNodeServiceDomain.cs");
                }
                try
                {
                    clientNetP2PChannelSN = (INetP2PSuperNodeChannel)npcSuperNode.OpenClient<INetP2PSuperNodeChannel>("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/NetP2PSuperNode", "P2PSuperNodeMesh", ref objNetP2PSuperNode);
                    clientNetP2PChannelSN.svcJoin("Server");
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeCinstructor()--1", "Domains\\SuperNodeServiceDomain.cs");
                }

                try
                {
                    clientHttpChannelBS = (IHTTPBootStrapService)bhcBootStrap.OpenClient<IHTTPBootStrapService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");
                    clsPeerInfo objPeerInfo = new clsPeerInfo();
                    objPeerInfo.objPeerInforation = VMuktiInfo.CurrentPeer.GetPeerDataContract();
                    objSIPInformation = clientHttpChannelBS.svcHttpBSJoin(VMuktiInfo.CurrentPeer.DisplayName, objPeerInfo);
                    List<string> lstBuddies = clientHttpChannelBS.svcGetAllBuddies();

                    if (lstBuddies != null && lstBuddies.Count > 0)
                    {
                        foreach (string str in lstBuddies)
                        {
                            fncUpdateUserBuddyStatus(str.Split('-')[0].Trim(), str.Split('-')[1].Trim());
                            fncSNInsertBuddy(str.Split('-')[0].Trim(), str.Split('-')[1].Trim());
                            fncSNInsertNode(str.Split('-')[0].Trim());
                        }
                    }
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()--HttpBootStrapClient()", "Domains\\SuperNodeServiceDomain.cs");
                }

                try
                {
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion.ToString() == "1.1")
                    {
                        ClientNetP2PPredictiveChannel = (INetP2PBootStrapPreditiveService)npcBootStrapPredictiveClient.OpenClient<INetP2PBootStrapPreditiveService>("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapPredictive", "P2PBootStrapPredictiveMesh", ref objNetP2PBootStrapPredictive);
                        ClientNetP2PPredictiveChannel.svcJoin("", "");
                    }
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()--NetP2PBootStrapPredictive Client()", "Domains\\SuperNodeServiceDomain.cs");
                }
                try
                {
                    bhsHttpSuperNode = new BasicHttpServer(ref objHttpSuperNode, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                    bhsHttpSuperNode.AddEndPoint<IHttpSuperNodeService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                    bhsHttpSuperNode.OpenServer();
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstrctor()--HttpSuperNode Server", "Domains\\SuperNodeServiceDomain.cs");
                }
                try
                {
                    //bhsHttpDataBase = new BasicHttpServer(ref objHttpDatabaseSuperNode, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpDataBase");
                    //bhsHttpDataBase.AddEndPoint<IHttpDataBaseService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpDataBase");
                    //bhsHttpDataBase.OpenServer();
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()--HttpDataBase Server", "Domains\\SuperNodeServiceDomain.cs");
                }

                try
                {
                    bhsHttpFileUplaodDownload = new BasicHttpServer(ref objFileUploadDownload, "http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/HttpFileUploadDownload");
                    bhsHttpFileUplaodDownload.AddEndPoint<IHttpFileUploadDownload>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/HttpFileUploadDownload");
                    bhsHttpFileUplaodDownload.OpenServer();
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()--HttpFileUploadDownload Server()", "Domains\\SuperNodeServiceDomain.cs");
                }
                timerBuzzSN.Start();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LoadSuperNodeConstructor()--2", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void UnloadSuperNodeConstructor()
        {
            try
            {
                timerBuzzSN.Stop();

                bhsHttpSuperNode.CloseServer();
                //bhsHttpDataBase.CloseServer();
                bhsHttpFileUplaodDownload.CloseServer();
                npsSuperNodeServer.CloseServer();

                npcBootStrapClient.CloseClient<INetP2PBootStrapChannel>();
                clientNetP2pChannelBS = null;
                //clientHttpChannelBS = null;

                npcSuperNode.CloseClient<INetP2PSuperNodeChannel>();
                bhcBootStrap.CloseClient<IHTTPBootStrapService>();

                if (VMuktiAPI.VMuktiInfo.VMuktiVersion.ToString() == "1.1")
                {
                    npcBootStrapPredictiveClient.CloseClient<INetP2PBootStrapPreditiveService>();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UnLoadSuperNodeConstructor()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        void fncMakeAllBuddyOffline()
        {
            try
            {

                List<string> lstOnlineBuddies = new List<string>();
                OpenConnection();
                //ClsException.WriteToLogFile("BS NOde_status 26");
                string str = "SELECT Buddy_Name FROM Node_Status";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, LocalSQLConn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstOnlineBuddies.Add(dataTable.Rows[i]["Buddy_Name"].ToString());
                }

                foreach (string userName in lstOnlineBuddies)
                {
                    //System.Diagnostics.Debug.WriteLine("UserName is going to Offline :: " + userName);
                    fncSNInsertBuddy(userName, "Offline");
                    fncSNDeleteNode(userName);
                    fncUpdateUserBuddyStatus(userName, "Offline");
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncMakeAllNuddyOffline()", "Domains\\SuperNodeServiceDomain.cs");
            }

        }

        void timerBuddylist_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerBuddylist.Stop();
                BuddyListTimer();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "timerBuddyList_Elapsed()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        public void BuddyListTimer()
        {

            try
            {
                //System.Text.StringBuilder sb = new StringBuilder();
                #region Code for timer logic



                //sb.AppendLine("getting the enumerator of hashtable");
                //sb.AppendLine();
                //sb.AppendLine("Location : BuddyListTimer()");
                //sb.AppendLine();


                OpenConnection();

                DataSet dsNodeStat = new DataSet();

                SqlCeDataAdapter node_stat = new SqlCeDataAdapter("select * from Node_Status", LocalSQLConn);
                node_stat.Fill(dsNodeStat);



                //IDictionaryEnumerator hsList = hsOnlineBuddyTable.GetEnumerator();


                //sb.AppendLine("getting the count of hashtable");
                //sb.AppendLine();
                //sb.AppendLine("Location : BuddyListTimer()");
                //sb.AppendLine();


                //string[] delFromHash = new string[hsOnlineBuddyTable.Count];
                //int totDel = 0;

                //sb.AppendLine("moving to next element from hashtable");
                //sb.AppendLine();
                //sb.AppendLine("Location : BuddyListTimer()");
                //sb.AppendLine();
                SqlCeCommand cmd;
                for (int i = 0; i < dsNodeStat.Tables[0].Rows.Count; i++)
                {
                    string uname = dsNodeStat.Tables[0].Rows[i]["Buddy_Name"].ToString();
                    DateTime previousTime = DateTime.Parse(dsNodeStat.Tables[0].Rows[i]["Buddy_TimeStamp"].ToString());
                    TimeSpan difference = DateTime.Now - previousTime;
                    int TimeInMinutes = difference.Minutes;

                    //sb.AppendLine("User is : " + uname);
                    //sb.AppendLine("Previous Time was : " + previousTime.ToString());
                    //sb.AppendLine("Difference is " + difference.ToString());
                    //sb.AppendLine("Difference in Minutes : " + TimeInMinutes.ToString());

                    if (TimeInMinutes < 3)
                    {

                        //sb.AppendLine("updating timestamp");
                        //sb.AppendLine();
                        //sb.AppendLine("Location : BuddyListTimer()");
                        //sb.AppendLine();



                        //cmd = new SqlCeCommand("update Node_Status set Buddy_TimeStamp=GETDATE(),Buddy_Status='Online' where Buddy_Name='" + uname + "'", LocalSQLConn);
                        //cmd.ExecuteNonQuery();

                        ////fncSNInsertBuddy(uname, "Online");
                        //fncSNInsertNode(uname);
                        ////fncUpdateUserBuddyStatus(uname, "Online");

                        //clientHttpChannelBS.svcHttpBSAuthorizedUser(uname,"", false);
                        //clientNetP2pChannelBS.svcNetP2PAddUser(uname);
                    }
                    else
                    {

                        //sb.AppendLine("updating timestamp");
                        //sb.AppendLine();
                        //sb.AppendLine("Location : BuddyListTimer()2");
                        //sb.AppendLine();

                        string str = "update Node_Status set Buddy_TimeStamp='" + DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) + "',Buddy_Status='Offline' where Buddy_Name='" + uname + "'";
                        cmd = new SqlCeCommand(str, LocalSQLConn);
                        cmd.ExecuteNonQuery();


                        fncSNInsertBuddy(uname, "Offline");
                        fncSNDeleteNode(uname);
                        //fncUpdateUserBuddyStatus(uname, "Offline");


                        clientNetP2pChannelBS.svcNetP2PRemoveUser(uname);
                        clientHttpChannelBS.svcHttpBSUnJoin(uname, "", false);
                    }
                }

                //while (hsList.MoveNext())
                //{
                //    string uname = hsList.Key.ToString();
                //    DateTime previousTime = DateTime.Parse(hsList.Value.ToString());
                //    TimeSpan difference = DateTime.Now - previousTime;
                //    int TimeInMinutes = difference.Minutes;

                //    if (TimeInMinutes < 2)
                //    {
                //        if (lstOfflinebuddies.Contains(uname))
                //        {
                //            sb.AppendLine("updating the timestamp for user : " + uname);
                //            sb.AppendLine();
                //            sb.AppendLine("Location : BuddyListTimer()");
                //            sb.AppendLine();

                //            hsOnlineBuddyTable.Add(uname, DateTime.Now);
                //            ClsException.WriteToLogFile("Node has come after 2 Mins...Making node online :: " + uname);


                //            fncSNInsertBuddy(uname, "Online");
                //            fncSNInsertNode(uname);
                //            fncUpdateUserBuddyStatus(uname, "Online");

                //            clientHttpChannelBS.svcHttpBSAuthorizedUser(uname, "", false);
                //            clientNetP2pChannelBS.svcNetP2PAddUser(uname);
                //            lstOfflinebuddies.Remove(uname);
                //        }
                //    }
                //    else
                //    {
                //        ClsException.WriteToLogFile("In supernode domain :: " + hsList.Key.ToString() + " is not come for 2 Mins");
                //        if (hsList.Key.ToString() != null && hsList.Key.ToString() != string.Empty)
                //        {
                //            ClsException.WriteToLogFile("In supernode domain is making offlne :: " + hsList.Key.ToString());
                //            fncSNInsertBuddy(hsList.Key.ToString(), "Offline");
                //            fncSNDeleteNode(hsList.Key.ToString());
                //            fncUpdateUserBuddyStatus(hsList.Key.ToString(), "Offline");


                //            clientNetP2pChannelBS.svcNetP2PRemoveUser(hsList.Key.ToString());
                //            clientHttpChannelBS.svcHttpBSUnJoin(hsList.Key.ToString(), "", false);
                //            lstOfflinebuddies.Add(hsList.Key.ToString());
                //            delFromHash[totDel] = hsList.Key.ToString();
                //            totDel = totDel + 1;
                //        }
                //    }

                //    if (totDel > 0)
                //    {
                //        for (int i =totDel; i < totDel; i++)
                //        {
                //            sb.AppendLine("removing the entry : " + delFromHash[i]+ "from hashtable");
                //            sb.AppendLine();
                //            sb.AppendLine("Location : BuddyListTimer()");
                //            sb.AppendLine();



                //            hsOnlineBuddyTable.Remove(delFromHash[i]);
                //        }
                //    }
                //}
                #endregion
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BuddyListTimer()", "Domains\\SuperNodeServiceDomain.cs");
            }
            if (timerBuddylist != null)
            {
                timerBuddylist.Start();
            }
        }

        #endregion

        private Boolean checkSuperNodeAvailable(string currentSuperNodeIP)
        {
            //ClsException.WriteToLogFile("Checking Whether previous supernode is available or not :: " + currentSuperNodeIP);


            Uri objuri = new Uri("http://" + currentSuperNodeIP + ":" + "80/HttpSuperNode");
            System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(objuri);
            System.Net.WebResponse objResp;
            try
            {
                objResp = objWebReq.GetResponse();
                objResp.Close();
                //ClsException.WriteToLogFile("supernode is available No need to delete :: " + currentSuperNodeIP);
                return true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckSuperNodeAvailable()", "Domains\\SuperNodeServiceDomain.cs");
                return false;
            }
        }

        public void ShowDirectory(DirectoryInfo dir)
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

        public void DownloadZipFiles()
        {
            try
            {
                ClsModuleCollection cmc = ClsModuleCollection.GetAll();
                ass = Assembly.GetAssembly(typeof(SuperNodeServiceDomain));
                for (int i = 0; i < cmc.Count; i++)
                {
                    try
                    {
                        #region Downloading ZipFile

                        string filename = cmc[i].ZipFile;
                        Uri u = new Uri(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + filename);
                        if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files")))
                        {
                            Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files"));
                        }
                        string destination = ass.Location.Replace("VMukti.Presentation.exe", @"Zip Files");
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

                        #endregion
                        string strXmlPath = strModPath + "\\" + filename.Split('.')[0] + "\\Control\\configuration.xml";
                        if (File.Exists(strXmlPath))
                        {
                            #region Parsing XML

                            XmlParser xp = new XmlParser();
                            xp.Parse(strXmlPath);

                            #endregion

                            #region Loading ReferencedAssemblies
                            al.Clear();
                            DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + filename.Split('.')[0]);
                            ShowDirectory(dirinfomodule);


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
                                VMuktiHelper.ExceptionHandler(ex, "DownloadZipFiles()1--copying swf n txt files", "Domains\\SuperNodeServiceDomain.cs");
                            }

                            for (int j = 0; j < al.Count; j++)
                            {
                                string[] arraysplit = al[j].ToString().Split('\\');
                                if (arraysplit[arraysplit.Length - 1].ToString() == cmc[i].AssemblyFile)
                                {
                                    assDownload = Assembly.LoadFrom(al[j].ToString());
                                    AssemblyName[] an = assDownload.GetReferencedAssemblies();

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
                                    Type[] t1 = assDownload.GetTypes();
                                    #region CreatingObject

                                    for (int k = 0; k < t1.Length; k++)
                                    {
                                        if (t1[k].Name == xp.xMain.SuperNodeClass)
                                        {
                                            try
                                            {
                                                object obj1 = Activator.CreateInstance(t1[k]);
                                                lstObjSuperNode.Add(obj1);
                                                break;
                                            }

                                            catch (Exception exp)
                                            {
                                                VMuktiHelper.ExceptionHandler(exp, "DownloadZipFiles()--CreateObject-5", "Domains\\SuperNodeServiceDomain.cs");
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "DownloadZipFiles()", "Domains\\SuperNodeServiceDomain.cs");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DownloadZipFiles()--2", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        public void CopyToBase(string modulename, string filename)
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + filename))
                {
                    string[] ControlFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\Modules\\" + modulename + "\\Control", "*.dll");
                    string[] BalFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\Modules\\" + modulename + "\\Bal", "*.dll");
                    for (int i = 0; i < ControlFiles.Length; i++)
                    {
                        FileInfo fi = new FileInfo(ControlFiles[i]);
                        fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "\\" + fi.Name);
                    }
                    foreach (string str in BalFiles)
                    {
                        FileInfo fi = new FileInfo(str);
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + fi.Name))
                        {
                            fi.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "\\" + fi.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CopyToBase()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        ~SuperNodeServiceDomain()
        {
            try
            {
                Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~SuperNodeServiceDomain()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (objHttpSuperNode != null)
                {
                    objHttpSuperNode = null;
                }

                if (npsSuperNodeServer != null)
                {
                    npsSuperNodeServer = null;
                }
                if (npsModules != null)
                {
                    npsModules = null;
                }

                if (lstmyNodes != null)
                {
                    lstmyNodes = null;
                }
                if (objFileStream != null)
                {
                    objFileStream = null;
                }

                if (strBuddyName != null)
                {
                    strBuddyName = null;
                }



                if (objNetP2PBootStrap != null)
                {
                    objNetP2PBootStrap = null;
                }
                if (clientNetP2pChannelBS != null)
                {
                    clientNetP2pChannelBS = null;
                }



                if (clientHttpChannelBS != null)
                {
                    clientHttpChannelBS = null;
                }
                if (objSIPInformation != null)
                {
                    objSIPInformation = null;
                }
                if (objNetP2PSuperNode != null)
                {
                    objNetP2PSuperNode = null;
                }
                if (clientNetP2PChannelSN != null)
                {
                    clientNetP2PChannelSN = null;
                }
                if (lstObjRTCAuthClient != null)
                {
                    lstObjRTCAuthClient = null;
                }
                if (lstObjSuperNode != null)
                {
                    lstObjSuperNode = null;
                }
            }


            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Domains\\SuperNodeServiceDomain.cs");
            }
        }

        #endregion
    }
}
