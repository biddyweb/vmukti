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
using System.Net;
using VMukti.Business.WCFServices.BootStrapServices.BasicHttp;
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using VMukti.Business.WCFServices.SuperNodeServices.DataContract;
using VMuktiAPI;
using VMuktiService;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServerCe;
using System.IO;
using VMukti.Business.WCFServices.BootStrapServices.NetP2P;
using System.Reflection;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using VMukti.Business.CommonDataContracts;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace VMukti.Presentation
{
    [Serializable]
    public class BootstrapServiceDomain : IDisposable
    {
        object objHttpBootStrap;
        object objHttpDatabaseBootStrap;

        static BasicHttpServer bhsHttpDataBase;

        NetPeerServer npsBootStrapServer;
        NetPeerServer npsBootStrapPredictviveServer;
        NetPeerServer npsBootStrapPBXServer;
        NetPeerServer npsBootStrapDashBoardService;
        NetPeerServer npsBootStrapActiveAgentReportService;
        NetPeerServer npsRecordingServer;
        NetPeerServer npsConsoleServer;

        BasicHttpServer bhsFileTransferServer;

        string strCurrentMachineIP;
        string appPath;
        string awClientConnectionString;
        SqlCeConnection LocalSQLConn;
        //string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "AllocateConferenceNumber.sdf";
        //string strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "AllocateConferenceNumber.sdf";
        string ClientConnectionString;
        string strLocalDBPath;

        //List<string> lstsSuperNodeInfo = new List<string>();
        //List<string> localSuperNodeInfo = new List<string>();
        List<string> lstsSuperNodeInfo;
        List<string> localSuperNodeInfo;
        bool isFirstTime;

        #region Uploading Recorded Files
        INetP2PBootStrapRecordedFileChannel clientNetP2pChannelRecording;
       // byte[] arr = new byte[5000];
        byte[] arr;
        #endregion

        #region FlashVideo
        BasicHttpServer bhsFlashVideo;
        //Dictionary<int, string> dictFlashUrl = new Dictionary<int, string>();
        Dictionary<int, string> dictFlashUrl;
        int identifier;
        #endregion

        public BootstrapServiceDomain()
        {
            try
            {

                #region Initialize global variables

                ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "AllocateConferenceNumber.sdf";
                strLocalDBPath = AppDomain.CurrentDomain.BaseDirectory + "AllocateConferenceNumber.sdf";
                lstsSuperNodeInfo = new List<string>();
                localSuperNodeInfo = new List<string>();
                dictFlashUrl = new Dictionary<int, string>();
                arr = new byte[5000];
                #endregion


                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceme35.dll"))
                {
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceme35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceme35.dll");
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlceqp35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceqp35.dll");
                    new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "sqlcese35.dll.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlcese35.dll");
                }

                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "AllocateConferenceNumber.sdf"))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();

                    LocalSQLConn = new SqlCeConnection();
                    LocalSQLConn.ConnectionString = ClientConnectionString;
                    LocalSQLConn.Open();

                    fncCreateSNNodeInfoTable();
                    fncCreateBuddyStatusTable();
                    fncCreateUserBuddyListTable();
                    fncCreateUserSuperNode_NodeInfoTable();
                    fncCreateUserSuperNodeInfoTable();
                    GetBuddyInfoFromServer();
                }
                else
                {
                    LocalSQLConn = new SqlCeConnection();
                    LocalSQLConn.ConnectionString = ClientConnectionString;
                    LocalSQLConn.Open();

                    fncSNInsertBuddy(VMuktiAPI.VMuktiInfo.BootStrapIPs[0], "Online");

                }
                awClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "AllocateConferenceNumber.sdf";
                strCurrentMachineIP = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;


                #region HttpServerHoster 4 Performance

                BootStrapDelegates objBSDel = new BootStrapDelegates();

                objBSDel.EntHttpAddBuddy += new BootStrapDelegates.DelsvcHttpAddBuddy(BootstrapServiceDomain_EntHttpAddBuddy);
                objBSDel.EntHttpRemoveBuddy += new BootStrapDelegates.DelsvcRemoveAddBuddy(BootstrapServiceDomain_EntHttpRemoveBuddy);
                objBSDel.EntHttpBsGetSuperNodeIP += new BootStrapDelegates.DelsvcHttpBsGetSuperNodeIP(BootstrapServiceDomain_EntHttpBsGetSuperNodeIP);
                objBSDel.EntHttpBSJoin += new BootStrapDelegates.DelHttpBSJoin(BootstrapServiceDomain_EntHttpBSJoin);
                objBSDel.EntHttpBSUnJoin += new BootStrapDelegates.DelsvcHttpBSUnJoin(BootstrapServiceDomain_EntHttpBSUnJoin);
                objBSDel.EntHttpGetSuperNodeBuddyList += new BootStrapDelegates.DelsvcHttpGetSuperNodeBuddyList(BootstrapServiceDomain_EntHttpGetSuperNodeBuddyList);
                objBSDel.EntHttpBSAuthorizedUser += new BootStrapDelegates.DelsvcHttpBSAuthorizedUser(BootstrapServiceDomain_EntHttpBSAuthorizedUser);

                objBSDel.EntsvcGetNodeNameByIP += new BootStrapDelegates.DelsvcGetNodeNameByIP(BootstrapServiceDomain_EntsvcGetNodeNameByIP);
                objBSDel.EntHTTPGetOfflineNodeName += new BootStrapDelegates.DelsvcGetOfflineNodeName(BootstrapServiceDomain_EntHTTPGetOfflineNodeName);
                objBSDel.EntsvcGetAllBuddies += new BootStrapDelegates.DelsvcGetAllBuddies(BootstrapServiceDomain_EntsvcGetAllBuddies);
                objBSDel.EntsvcUpdateVMuktiVersion += new BootStrapDelegates.DelsvcUpdateVMuktiVersion(objBSDel_EntsvcUpdateVMuktiVersion);
                objHttpBootStrap = objBSDel;

                BasicHttpServer bhsHttpBootStrap = new BasicHttpServer(ref objHttpBootStrap, "http://" + VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");
                bhsHttpBootStrap.AddEndPoint<IHTTPBootStrapService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/HttpBootStrap");
                bhsHttpBootStrap.OpenServer();

                #endregion

                #region HttpDataBase Server

                try
                {
                    BootStrapDataBaseDelegates objBSDBDel = new BootStrapDataBaseDelegates();
                    objBSDBDel.EntHttpsvcjoin += new BootStrapDataBaseDelegates.DelHttpsvcJoin(objBSDBDel_EntHttpsvcjoin);
                    objBSDBDel.EntHttpExecuteDataSet += new BootStrapDataBaseDelegates.DelHttpExecuteDataSet(objBSDBDel_EntHttpExecuteDataSet);
                    objBSDBDel.EntHttpExecuteStoredProcedure += new BootStrapDataBaseDelegates.DelHttpExecuteStoredProcedure(objBSDBDel_EntHttpExecuteStoredProcedure);
                    objBSDBDel.EntHttpExecuteNonQuery += new BootStrapDataBaseDelegates.DelHttpExecuteNonQuery(objBSDBDel_EntHttpExecuteNonQuery);
                    objBSDBDel.EntHttpExecuteReturnNonQuery += new BootStrapDataBaseDelegates.DelHttpExecuteReturnNonQuery(objBSDBDel_EntHttpExecuteReturnNonQuery);

                    objHttpDatabaseBootStrap = objBSDBDel;

                    bhsHttpDataBase = new BasicHttpServer(ref objHttpDatabaseBootStrap, "http://" + VMuktiInfo.BootStrapIPs[0] + ":80/HttpDataBase");
                    bhsHttpDataBase.AddEndPoint<IHttpBootStrapDataBaseService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/HttpDataBase");
                    bhsHttpDataBase.OpenServer();
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "SuperNoeServiceDomain()--HttpDataBase Server()", "Domains\\SuperNodeServiceDomain.cs");
                }

                #endregion


                #region HTTP Server For File Transfer Service 4 Performance

                HTTPFileTransferDelegates objFileTransferDel = new HTTPFileTransferDelegates();
                objFileTransferDel.EntsvcHTTPFileTransferServiceJoin += new HTTPFileTransferDelegates.DelsvcHTTPFileTransferServiceJoin(BootstrapServiceDomain_EntsvcHTTPFileTransferServiceJoin);
                objFileTransferDel.EntsvcHTTPFileTransferServiceUploadFile += new HTTPFileTransferDelegates.DelsvcHTTPFileTransferServiceUploadFile(BootstrapServiceDomain_EntsvcHTTPFileTransferServiceUploadFile);
                objFileTransferDel.EntsvcHTTPFileTransferServiceUploadFileToInstallationDirectory += new HTTPFileTransferDelegates.DelsvcHTTPFileTransferServiceUploadFileToInstallationDirectory(BootstrapServiceDomain_EntsvcHTTPFileTransferServiceUploadFileToInstallationDirectory);
                objFileTransferDel.EntsvcHTTPFileTransferServiceDownloadFile += new HTTPFileTransferDelegates.DelsvcHTTPFileTransferServiceDownloadFile(BootstrapServiceDomain_EntsvcHTTPFileTransferServiceDownloadFile);

                object objHTTPFileTransfer = objFileTransferDel;

                bhsFileTransferServer = new BasicHttpServer(ref objHTTPFileTransfer, "http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                bhsFileTransferServer.AddEndPoint<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                bhsFileTransferServer.OpenServer();

                #endregion

                #region Http Server For Flash Video

                HttpFlashVideoDelegates objFlashVideoDel = new HttpFlashVideoDelegates();
                objFlashVideoDel.EntsvcHttpBSFVUnJoin += new HttpFlashVideoDelegates.DelsvcHttpBSFVUnJoin(objFlashVideo_EntsvcHttpBSFVUnJoin);
                objFlashVideoDel.EntsvcHttpBSFVJoin += new HttpFlashVideoDelegates.DelsvcHttpBSFVJoin(objFlashVideo_EntsvcHttpBSFVJoin);
                objFlashVideoDel.EntsvcHttpBSFVGetUrl += new HttpFlashVideoDelegates.DelsvcHttpBSFVGetUrl(objFlashVideo_EntsvcHttpBSFVGetUrl);
                objFlashVideoDel.EntsvcHttpBSFVCreateFolder += new HttpFlashVideoDelegates.DelsvcHttpBSFVCreateFolder(objFlashVideo_EntsvcHttpBSFVCreateFolder);

                object objFlashVideo = objFlashVideoDel;

                bhsFlashVideo = new BasicHttpServer(ref objFlashVideo, "http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFlashVideo");
                bhsFlashVideo.AddEndPoint<IHttpBootStrapFlashVideo>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFlashVideo");
                bhsFlashVideo.OpenServer();

                #endregion

                #region NetP2PServerHoster
                npsBootStrapServer = new NetPeerServer("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrap");
                npsBootStrapServer.AddEndPoint("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrap");
                npsBootStrapServer.OpenServer();



                //MessageBox.Show("netp2p server hosted");
                #endregion

                #region NetP2PConsoleServerHoster

                npsConsoleServer = new NetPeerServer("net.tcp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PConsole");
                npsConsoleServer.AddEndPoint("net.tcp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PConsole");
                npsConsoleServer.OpenServer();


                #endregion

                #region CallCenter Services
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion.ToString() == "1.1")
                {
                    #region NetP2PPrictiveServerHoster
                    npsBootStrapPredictviveServer = new NetPeerServer("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapPredictive");
                    npsBootStrapPredictviveServer.AddEndPoint("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapPredictive");
                    npsBootStrapPredictviveServer.OpenServer();
                    #endregion

                    #region Hosting DashBoard Server
                    npsBootStrapDashBoardService = new NetPeerServer("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapDashBoard");
                    npsBootStrapDashBoardService.AddEndPoint("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapDashBoard");
                    npsBootStrapDashBoardService.OpenServer();
                    #endregion

                    # region Hosting Active Agent Server
                    npsBootStrapActiveAgentReportService = new NetPeerServer("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveAgentReport");
                    npsBootStrapActiveAgentReportService.AddEndPoint("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapActiveAgentReport");
                    npsBootStrapActiveAgentReportService.OpenServer();
                    #endregion


                    #region Server For Uploading Recorded Calls


                    npsRecordingServer = new NetPeerServer("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapRecordedFiles");
                    npsRecordingServer.AddEndPoint("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapRecordedFiles");
                    npsRecordingServer.OpenServer();

                    #endregion

                    #region Client For Uploading Recorded Calls 4 Performance

                    NetPeerClient npcRecordingClient = new NetPeerClient();
                    object objP2PRecording = new NetP2PBootStrapRecordedFileDelegate();

                    ((NetP2PBootStrapRecordedFileDelegate)objP2PRecording).EntsvcRecordedFileJoin += new NetP2PBootStrapRecordedFileDelegate.delsvcRecordedFileJoin(objP2PRecording_EntsvcRecordedFileJoin);
                    ((NetP2PBootStrapRecordedFileDelegate)objP2PRecording).EntsvcSendRecordedFiles += new NetP2PBootStrapRecordedFileDelegate.delsvcSendRecordedFiles(objP2PRecording_EntsvcSendRecordedFiles);
                    ((NetP2PBootStrapRecordedFileDelegate)objP2PRecording).EntsvcRecordedFileUnJoin += new NetP2PBootStrapRecordedFileDelegate.delsvcRecordedFileUnJoin(objP2PRecording_EntsvcRecordedFileUnJoin);
                    //objNetP2PRecording = objP2PRecording;
                    clientNetP2pChannelRecording = (INetP2PBootStrapRecordedFileChannel)npcRecordingClient.OpenClient<INetP2PBootStrapRecordedFileChannel>("net.tcp://" + VMuktiInfo.BootStrapIPs[0] + ":6000/NetP2PBootStrapRecordedFiles", "P2PRecordedFiles", ref objP2PRecording);

                    VMukti.Business.CommonMessageContract.clsMessageContract objContract = new VMukti.Business.CommonMessageContract.clsMessageContract();
                    objContract.uname = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.fName = "";
                    objContract.fExtension = "";
                    objContract.fLength = 0;
                    objContract.fStream = new MemoryStream();

                    clientNetP2pChannelRecording.svcRecordedFileJoin(objContract);
                    #endregion
                }
                #endregion

                #region Disaster Recovery

                //fncMakeAllBuddyOffline();

                #endregion

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootstrapServiceDomain()", "Domains\\BootstrapServiceDomain.cs");                               
            }
        }



        #region WCF Events For Flash Video

        int objFlashVideo_EntsvcHttpBSFVCreateFolder(string uName, string Url)
        {
            try
            {
                if (!string.IsNullOrEmpty(Url))
                {
                    //split url to retrieve folder name
                    //create the folder in bs main application path
                    //store the url and identifier in the dictionary

                    string foldername = Url.Split('/')[Url.Split('/').Length - 1];
                    string filePath;
                    string configurationFilePath;
                    string folderPath;
                    string basePath;
                    string ascPath;
                    string fmspath;

                    Assembly ass = Assembly.GetEntryAssembly();
                    XmlDocument xDoc = new XmlDocument();
                    configurationFilePath = (Assembly.GetAssembly(this.GetType()).Location.Replace("VMukti.Presentation.exe", ""));



                    configurationFilePath += @"sqlceds35.dll";
                    xDoc.Load(configurationFilePath);

                    XmlNode xn = xDoc.SelectSingleNode("/Root/OtherSettings/PhysicalPathOfVirtualDirectory");
                    basePath = DecodeBase64String(xn.Attributes[0].Value);
                    ascPath = Path.Combine(basePath, "main.asc");

                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey sk1 = rk.OpenSubKey(@"SOFTWARE\Adobe\Flash Media Server\1\FMS");
                    string path = (string)sk1.GetValue("Conf");
                    int index = path.IndexOf("conf");
                    fmspath = path.Substring(0, index);
                    fmspath = Path.Combine(fmspath, @"applications");


                    try
                    {
                        //set the permission of everyone to the applications folder of FMS
                        DirectoryInfo di = new DirectoryInfo(fmspath);
                        System.Security.AccessControl.DirectorySecurity ds = di.GetAccessControl();
                        ds.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule("Everyone", System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
                        di.SetAccessControl(ds);
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "objFlashVideo_EntsvcHttpBSFVCreateFolder()---setting permission to applications folder", "Domains\\BootstrapServiceDomain.cs");
                    }
           
                    folderPath = fmspath + "\\" + foldername;

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    

                    if (System.IO.File.Exists(ascPath))
                        File.Copy(ascPath, folderPath + "\\main.asc");

                    dictFlashUrl.Add(++identifier, Url);
                    return identifier;
                }
                return -1;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objFlashVideo_EntsvcHttpBSFVCreateFolder()", "Domains\\BootstrapServiceDomain.cs");
                return -1;
            }
        }

        string objFlashVideo_EntsvcHttpBSFVGetUrl(int indentifier)
        {
            try
            {
                if (indentifier != -1)
                {
                    foreach (KeyValuePair<int, string> kvp in dictFlashUrl)
                    {
                        if (kvp.Key == indentifier)
                        {
                            return kvp.Value;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objFlashVideo_EntsvcHttpBSFVGetUrl()", "Domains\\BootstrapServiceDomain.cs");
                return string.Empty;
            }
        }

        void objFlashVideo_EntsvcHttpBSFVJoin(string uName)
        {
            
        }

        void objFlashVideo_EntsvcHttpBSFVUnJoin(int identifier)
        {
            try
            {
                if (identifier != -1)
                {
                    foreach (KeyValuePair<int, string> kvp in dictFlashUrl)
                    {
                        if (kvp.Key == identifier)
                        {
                            string foldername = kvp.Value.Substring(kvp.Value.LastIndexOf('/') + 1);

                            RegistryKey rk = Registry.LocalMachine;
                            RegistryKey sk1 = rk.OpenSubKey(@"SOFTWARE\Adobe\Flash Media Server\1\FMS");
                            string path = (string)sk1.GetValue("Conf");
                            int index = path.IndexOf("conf");
                            string fmspath = path.Substring(0, index);
                            fmspath = Path.Combine(fmspath, @"applications");

                            Directory.Delete(fmspath + "\\" + foldername, true);
                            dictFlashUrl.Remove(kvp.Key);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objFlashVideo_EntsvcHttpBSFVUnJoin()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        #endregion

        #region Http DataBase EventHandlers

        int objBSDBDel_EntHttpExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam)
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

        void objBSDBDel_EntHttpExecuteNonQuery(string spName, clsSqlParameterContract objSParam)
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

        clsDataBaseInfo objBSDBDel_EntHttpExecuteStoredProcedure(string spName, clsSqlParameterContract objSParam)
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

        clsDataBaseInfo objBSDBDel_EntHttpExecuteDataSet(string querystring)
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

        void objBSDBDel_EntHttpsvcjoin()
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

        #region WCF Events For Uploading Recorded Files

        void objP2PRecording_EntsvcRecordedFileJoin(VMukti.Business.CommonMessageContract.clsMessageContract mcRFJoin)
        {
        }

        void objP2PRecording_EntsvcSendRecordedFiles(VMukti.Business.CommonMessageContract.clsMessageContract mcSendRecordedFiles)
        {
            try
            {
                //if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != mcSendRecordedFiles.uname)
                //{
                    System.Xml.XmlDocument ConfDoc = new System.Xml.XmlDocument();
                    ConfDoc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceds35.dll");
                    System.Xml.XmlNodeList xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("PhysicalPathOfVirtualDirectory");
                    string VirtualDirectoryPath = DecodeBase64String(xmlNodes[0].Attributes["Value"].Value.ToString());

                if (!Directory.Exists(VirtualDirectoryPath + "\\AudioRecordedFiles"))
                {
                    Directory.CreateDirectory(VirtualDirectoryPath + "\\AudioRecordedFiles");
                }

                byte[] byteArray = fncStreamToByteArry(mcSendRecordedFiles.fStream);
                FileStream fs = new FileStream(VirtualDirectoryPath + "\\AudioRecordedFiles\\" + mcSendRecordedFiles.fName, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(byteArray, 0, byteArray.Length);
                fs.Close();
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objP2PRecording_EntsvcSendRecordedFiles()", "Domains\\BootstrapServiceDomain.cs");
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
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToByteArry()", "Domains\\BootstrapServiceDomain.cs\\objP2PRecording_EntsvcSendRecordedFiles");                               
                return null;
            }
        }

        void objP2PRecording_EntsvcRecordedFileUnJoin(VMukti.Business.CommonMessageContract.clsMessageContract mcRFUnJoin)
        { }        
        #endregion

        #region WCF Events for File Transfer Service For CRM and Script and Module Upload Download

        void BootstrapServiceDomain_EntsvcHTTPFileTransferServiceJoin()
        {
            try
            {

        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntsvcHTTPFileTransferServiceJoin()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        void BootstrapServiceDomain_EntsvcHTTPFileTransferServiceUploadFile(RemoteFileInfo request)
        {
            string filePath;
            string baseDirectory;
            string installerPath;
            try
            {
                baseDirectory = (Assembly.GetAssembly(this.GetType()).Location.Replace("VMukti.Presentation.exe", ""));

                installerPath = baseDirectory;

                installerPath += request.FolderNameToStore;
                if (!Directory.Exists(installerPath))
                {
                    Directory.CreateDirectory(installerPath);
                }
                filePath = System.IO.Path.Combine(installerPath, request.FileName);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                int chunkSize = 2048;
                byte[] buffer = new byte[chunkSize];

                using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
                {
                    do
                    {
                        // read bytes from input stream
                        int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                        if (bytesRead == 0) break;

                        // write bytes to output stream
                        writeStream.Write(buffer, 0, bytesRead);
                    } while (true);

                    writeStream.Close();
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntsvcHTTPFileTransferServiceUploadFile()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        void BootstrapServiceDomain_EntsvcHTTPFileTransferServiceUploadFileToInstallationDirectory(RemoteFileInfo request)
        {
            string filePath;
            string configurationFilePath;
            string installerPath;
            string basePath;
            try
            {
                Assembly ass = Assembly.GetEntryAssembly();
                XmlDocument xDoc = new XmlDocument();
                configurationFilePath = (Assembly.GetAssembly(this.GetType()).Location.Replace("VMukti.Presentation.exe", ""));
                configurationFilePath += @"sqlceds35.dll";
                xDoc.Load(configurationFilePath);
                XmlNode xn = xDoc.SelectSingleNode("/Root/OtherSettings/PhysicalPathOfVirtualDirectory");
                basePath = DecodeBase64String(xn.Attributes[0].Value);
                
                installerPath = basePath + "\\" + request.FolderNameToStore;
                if (!Directory.Exists(installerPath))
                {
                    Directory.CreateDirectory(installerPath);
                }
                filePath = System.IO.Path.Combine(installerPath, request.FileName);


                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                int chunkSize = 2048;
                byte[] buffer = new byte[chunkSize];

                using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
                {
                    do
                    {
                        // read bytes from input stream
                        int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                        if (bytesRead == 0) break;

                        // write bytes to output stream
                        writeStream.Write(buffer, 0, bytesRead);
                    } while (true);

                    writeStream.Close();
                }

                //extracting the file uploaded in zip files folder to modules folder
                //if (request.FolderNameToStore == "Zip Files")
                //{
                //    DirectoryInfo diModule = new DirectoryInfo(basePath + "\\Modules");
                //    VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                //    if (!Directory.Exists(diModule.FullName + "\\" + request.FileName.Split('.')[0]))
                //    {
                //        fz.ExtractZip(installerPath + "\\" + request.FileName, diModule.FullName, null);
                //    }
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntsvcHTTPFileTransferServiceUploadFileToInstallationDirectory()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        RemoteFileInfo BootstrapServiceDomain_EntsvcHTTPFileTransferServiceDownloadFile(DownloadRequest request)
        {
            string filePath;            
            string installerPath;
            System.IO.FileInfo fileInfo;
            string configurationFilePath;            
            string basePath;
            try
            {
                Assembly ass = Assembly.GetEntryAssembly();
                XmlDocument xDoc = new XmlDocument();
                configurationFilePath = (Assembly.GetAssembly(this.GetType()).Location.Replace("VMukti.Presentation.exe", ""));
                configurationFilePath += @"sqlceds35.dll";
                xDoc.Load(configurationFilePath);
                XmlNode xn = xDoc.SelectSingleNode("/Root/OtherSettings/PhysicalPathOfVirtualDirectory");
                basePath = DecodeBase64String(xn.Attributes[0].Value);

                installerPath = basePath + "\\" + request.FolderWhereFileIsStored;

                //Check if directory specified exists
                if (Directory.Exists(installerPath))
                {
                    //Get info about the input file
                    filePath = System.IO.Path.Combine(installerPath, request.FileName);
                    fileInfo = new System.IO.FileInfo(filePath);

                    // check if exists
                    if (!fileInfo.Exists)
                    {
                        ClsException.WriteToLogFile("BootstrapServiceDomain_EntsvcHTTPFileTransferServiceDownloadFile()--:--BootStrapDomain.cs--:--File Not Found(" + request.FileName + ")" + " :--:--");
                    }

                    // open stream
                    System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    RemoteFileInfo result = new RemoteFileInfo();
                    result.FileName = request.FileName;
                    result.Length = fileInfo.Length;
                    result.FileByteStream = stream;
                    return result;

                }
                else
                {
                    ClsException.WriteToLogFile("BootstrapServiceDomain_EntsvcHTTPFileTransferServiceDownloadFile()--:--BootStrapDomain.cs--:--" + "Directory Not Found(" + request.FolderWhereFileIsStored + ")" + " :--:--");
                }


            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntsvcNetP2pFileTransferServiceUploadFileScript()", "Domains\\BootstrapServiceDomain.cs");
            }
            return null;
        }

        #endregion

        #region Buddy Related HttpBootStrap Events Handlers HTTP & NetP2P

        /// <summary>
        /// This function will use for giving Node/SuperNode to Connection string,authtype etc.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="objPeerInformation"></param>
        /// <returns></returns>
        VMukti.Business.WCFServices.BootStrapServices.DataContracts.clsBootStrapInfo BootstrapServiceDomain_EntHttpBSJoin(string uName, clsPeerInfo objPeerInformation)
        {           
            try
            {
                clsBootStrapInfo objBootStrapInfo = new clsBootStrapInfo();

                objBootStrapInfo.AuthServerIP = VMuktiInfo.CurrentPeer.AuthServerIP;
                objBootStrapInfo.AuthType = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType.ToString();
                objBootStrapInfo.ConnectionString = EncodeBase64(VMuktiAPI.VMuktiInfo.MainConnectionString);

                
                return objBootStrapInfo;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHttpBSJoin()", "Domains\\BootstrapServiceDomain.cs");                
                return null;
            }
        }

        /// <summary>
        /// This function will be called by everynode when they login..this function will return their buddies and their status..
        /// </summary>
        /// <param name="uName"></param>
        /// <returns></returns>
        List<string> BootstrapServiceDomain_EntHttpGetSuperNodeBuddyList(string uName)
        {
            lock (this)
            {
                List<string> lstOnlineBuddies = new List<string>();
                //  SqlCeConnection conn = new SqlCeConnection(awClientConnectionString);
                //   conn.Open();
                try
                {

                    //SqlCeConnection conn = new SqlCeConnection(ClientConnectionString);
                    //conn.Open();
                    //string str = "SELECT * FROM Node_Status";
                    //SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, conn);   
                    OpenConnection();
                    string str = "SELECT * FROM User_BuddyList Where UserName ='" + uName + "'";
                    SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(str, LocalSQLConn);

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        /****** Getting status of individual buddies from Node_Status for particular user ***/

                        string buddname=dataTable.Rows[i]["BuddyName"].ToString();

                        string strstat = "select Buddy_Status from Node_Status where Buddy_Name='" + buddname + "'";
                        SqlCeCommand comm = new SqlCeCommand(strstat, LocalSQLConn);
                        string buddstat = (string)comm.ExecuteScalar();

                        lstOnlineBuddies.Add(buddname + "-" + buddstat);
                    }
                    
                    return lstOnlineBuddies;
                    // End Meet Code.
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHttpGetSuperNodeBuddyList()", "Domains\\BootstrapServiceDomain.cs");                    
                    return lstOnlineBuddies;
                }
            }
        }

        /// <summary>
        /// This fucntion will be called when any node/supernode will get offline or signout or close their application
        /// or their application got the error.This function will make respecteive buddy offline.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="IP"></param>
        /// <param name="IsSuperNode"></param>
        void BootstrapServiceDomain_EntHttpBSUnJoin(string uName, string IP, bool IsSuperNode)
        {
            //   SqlCeConnection Conn = new SqlCeConnection(awClientConnectionString);
            //  Conn.Open();
            if (uName != null)
            {
                //ClsException.WriteToLogFile("Unjoin is called by :: " + uName + "IP address is :: " + IP + " IssuperNode is :: " + IsSuperNode.ToString());
            }
            DataTable dt = new DataTable();
            SqlCeCommand sqlcmd = null;
            try
            {
                // Meet Code For Buddylist Implementation.
                if (uName != null && uName != string.Empty && uName != IP)
                {
                    //fncSNDeleteBuddy(uName);
                    fncSNInsertBuddy(uName, "Offline");
                    fncSNDeleteNode(uName);
                    fncUpdateUserBuddyStatus(uName, "Offline");
                    VMukti.Business.clsBuddyStatus.UpdateBuddy(uName, "Offline");
                }
                OpenConnection();
                // End Meet Code.
                if (!IsSuperNode && uName != "")
                {
                    //	System.Windows.Forms.MessageBox.Show("1");					

                    sqlcmd = new SqlCeCommand("Select SuperNode_Id from SuperNode_Node_Info where Node_Name='" + uName + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    object objsvcUnJoinSuperNode_Node_Info = sqlcmd.ExecuteScalar();
                    //System.Windows.Forms.MessageBox.Show("2");
                    if (objsvcUnJoinSuperNode_Node_Info != null)
                    {
                        sqlcmd = new SqlCeCommand("Select NodeCount from SuperNode_Info where Id='" + objsvcUnJoinSuperNode_Node_Info.ToString() + "'");
                        sqlcmd.Connection = LocalSQLConn;
                        object objsvcUnJoinNodeCount = sqlcmd.ExecuteScalar();
                        if (objsvcUnJoinNodeCount != null && int.Parse(objsvcUnJoinNodeCount.ToString()) > 0)
                        {
                            //	System.Windows.Forms.MessageBox.Show("3");

                            int intDecrementCounter = int.Parse(objsvcUnJoinNodeCount.ToString()) - 1;
                            sqlcmd = new SqlCeCommand("UPDATE SuperNode_Info set NodeCount='" + intDecrementCounter + "' Where Id='" + objsvcUnJoinSuperNode_Node_Info.ToString() + "'");
                            sqlcmd.Connection = LocalSQLConn;
                            sqlcmd.ExecuteNonQuery();
                            //	System.Windows.Forms.MessageBox.Show("4");
                        }
                        sqlcmd = new SqlCeCommand("Delete from SuperNode_Node_Info where Node_Name='" + uName + "'");
                        sqlcmd.Connection = LocalSQLConn;
                        sqlcmd.ExecuteNonQuery();
                    }
                    //	System.Windows.Forms.MessageBox.Show("5");
                }
                else
                {
                    //ClsException.WriteToLogFile("Deleting SuperNode Name :: " + uName);

                    sqlcmd = new SqlCeCommand("Delete from SuperNode_Info where SuperNodeName='" + uName + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    sqlcmd.ExecuteNonQuery();                    
                }
              
                if (lstsSuperNodeInfo.Contains(IP))
                {
                    lstsSuperNodeInfo.Remove(IP);
                }
                else if (lstsSuperNodeInfo.Contains(uName))
                {
                    lstsSuperNodeInfo.Remove(uName);
                }               

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHttpBSUnJoin()", "Domains\\BootstrapServiceDomain.cs");
            }

        }     

        /// <summary>
        /// "BootstrapServiceDomain_EntHttpBsGetSuperNodeIP" function is use by Node for getting their supernode IP.
        /// in this function bootstrap will check whether supernode is available or not if supernode is available the it will give this supernode IP to
        /// Node. Node will communicate (Open HTTP or P2P clients) based on this IP.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="IP"></param>
        /// <param name="blSuperNode"></param>
        /// <returns></returns>
        VMukti.Business.WCFServices.SuperNodeServices.DataContract.clsSuperNodeDataContract BootstrapServiceDomain_EntHttpBsGetSuperNodeIP(string uName, string IP, bool blSuperNode)
        {
            // SqlCeConnection Conn = new SqlCeConnection(awClientConnectionString);

            SqlCeCommand sqlcmd = null;

            try
            {
              
                clsSuperNodeDataContract objSuperNodeInfo = new clsSuperNodeDataContract();

                //super node checking
                #region Node is not SuperNode
                if (!blSuperNode) //Find Free SuperNode from the list and allocate itto Node
                {

                    //code for super node service availabity checking 
                    //if super node is still available than findAvailableIp=true

                    Boolean isSuperNodeAvailable = false;
                    int intIncrementCounter = 0;
                    String SupId = null;
                    //Find Free available SuperNode untill can not be found
                    while (isSuperNodeAvailable == false)
                    {
                        //Find Free available SuperNode
                        SqlCeDataAdapter sqlDataAdap = null;
                        DataTable dt = null;
                        // Conn.Open();
                        OpenConnection();
                        sqlcmd = new SqlCeCommand("SELECT count(*) FROM SuperNode_Info");
                        sqlcmd.Connection = LocalSQLConn;
                        object objSuperNodeCount = sqlcmd.ExecuteScalar();
                        sqlDataAdap = null;

                        if (int.Parse(objSuperNodeCount.ToString()) > 1) // This means more than one supernode is present(1Bootstrap + supernodes)
                        {
                            //  ClsException.WriteToLogFile("More Than One supernode available");

                            sqlDataAdap = new SqlCeDataAdapter("SELECT * FROM SuperNode_Info WHERE SuperNodeIP <> '" + strCurrentMachineIP + "' ORDER BY NodeCount", LocalSQLConn);
                            dt = new DataTable();
                            sqlDataAdap.Fill(dt);

                            // ClsException.WriteToLogFile("Row Count is  :: " + dt.Rows.Count.ToString());

                            objSuperNodeInfo.SuperNodeIP = dt.Rows[0]["SuperNodeIP"].ToString();

                            //   ClsException.WriteToLogFile("Node :: "+ IP + " SuperNode IP :: " + objSuperNodeInfo.SuperNodeIP);

                            isSuperNodeAvailable = checkSuperNodeAvailable(objSuperNodeInfo.SuperNodeIP);

                            //ClsException.WriteToLogFile("Available  SuperNode IP is...... :: " + objSuperNodeInfo.SuperNodeIP);
                        }
                        else
                        {
                            //    ClsException.WriteToLogFile("Only One supernode available That means BootStap");

                            sqlDataAdap = new SqlCeDataAdapter("SELECT * FROM SuperNode_Info ORDER BY NodeCount", LocalSQLConn);
                            dt = new DataTable();
                            sqlDataAdap.Fill(dt);

                            //   ClsException.WriteToLogFile("Row Count is  :: " + dt.Rows.Count.ToString());

                            objSuperNodeInfo.SuperNodeIP = dt.Rows[0]["SuperNodeIP"].ToString();

                            //ClsException.WriteToLogFile("Available SuperNode IP :: " + objSuperNodeInfo.SuperNodeIP);
                            //shilpa 5-Feb-2008
                            //isSuperNodeAvailable = true because we are assigning  boostrap as super node
                            //which will be running continuesly
                            isSuperNodeAvailable = true;
                        }

                        //shilpa code 5-Feb-2008
                        //method for super node service checking

                        if (!isSuperNodeAvailable)
                        {
                            //assign all its  node to the another super node 
                            //shilpa code
                            //6-Feb-2008

                            //deleting the super node entry
                            adjustSuperNode(objSuperNodeInfo.SuperNodeIP);
                        }
                        else
                        {
                            //continue
                            //with the same super node ip
                            //exit from the while loop
                            intIncrementCounter = int.Parse(dt.Rows[0]["NodeCount"].ToString()) + 1;
                            SupId = dt.Rows[0]["Id"].ToString();
                        }
                        dt.Clear();
                        dt.Reset();
                        //  Conn.Close();

                    } // end while

            

                    // end code
                    // shilpa 5-Feb-2008
                    //  Conn.Open();
                    //int intIncrementCounter = int.Parse(dt.Rows[0]["NodeCount"].ToString()) + 1;
                    OpenConnection();
                    sqlcmd = new SqlCeCommand("Update SuperNode_Info Set NodeCount = '" + intIncrementCounter + "' where Id='" + SupId + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    int TotUpdate = sqlcmd.ExecuteNonQuery();

                    sqlcmd = new SqlCeCommand("Select Id from SuperNode_Node_Info where Node_Name = '" + uName + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    object objTemp = sqlcmd.ExecuteScalar();
                    if (objTemp != null)
                    {
                        sqlcmd = new SqlCeCommand("Update SuperNode_Node_Info set SuperNode_Id='" + SupId + "' where Node_Name='" + uName + "'");
                        sqlcmd.Connection = LocalSQLConn;
                        sqlcmd.ExecuteNonQuery();
                    }
                    else
                    {
                        sqlcmd = new SqlCeCommand("INSERT INTO SuperNode_Node_Info (SuperNode_Id,Node_Name) VALUES ('" + SupId + "','" + uName + "')");
                        sqlcmd.Connection = LocalSQLConn;
                        sqlcmd.ExecuteNonQuery();
                    }
                    

                }

                #endregion

                #region Node is SuperNode
                else //Add New SuperNode to available superNode List
                {
                    
                    OpenConnection();
                    sqlcmd = new SqlCeCommand("SELECT Count(*) from SuperNode_Info where SuperNodeIP = '" + IP + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    object temp = sqlcmd.ExecuteScalar();
                    if (int.Parse(temp.ToString()) <= 0)
                    {
                        sqlcmd = new SqlCeCommand("INSERT INTO SuperNode_Info (SuperNodeIP, NodeCount,SuperNodeName) VALUES ('" + IP + "', '0','" + uName + "')");
                        sqlcmd.Connection = LocalSQLConn;
                        sqlcmd.ExecuteNonQuery();
                    }
                    objSuperNodeInfo.SuperNodeIP = IP;
                    // **
                    if (!lstsSuperNodeInfo.Contains(IP))
                    {
                        lstsSuperNodeInfo.Add(IP);
                    }                   
                }
                #endregion
                // Meet Code For Buddylist Implementation.


                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType == AuthType.SIPAuthentication)
                {
                    objSuperNodeInfo.FileExists = false;
                    sqlcmd = new SqlCeCommand("SELECT SIP_Number FROM SIP_Info WHERE UserName='" + uName + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    object objTempSipNumber = sqlcmd.ExecuteScalar();

                    sqlcmd = new SqlCeCommand("INSERT INTO Registered_Users (UserName,SIPNumber) VALUES ('" + uName + "','" + objTempSipNumber.ToString() + "')");
                    sqlcmd.Connection = LocalSQLConn;
                    sqlcmd.ExecuteNonQuery();
                }
                
                return objSuperNodeInfo;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHTTPBsGetSuperNodeIP", "Domains\\BootstrapServiceDomain.cs");                
                return null;
            }
        }

        /// <summary>
        /// this function will be called when any Node wants to add new buddy to his/her buddy list. Here buddy will be added to both side. if buddy is not
        /// present in the main application it will return as a null string other wise return buddy and his/her status with return value.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="BuddyName"></param>
        /// <returns></returns>
        string BootstrapServiceDomain_EntHttpAddBuddy(string uName, string BuddyName)
        {

            try
            {
                OpenConnection();
                SqlCeCommand sqlcmd = new SqlCeCommand("SELECT Count(*) FROM Node_Status  where Buddy_Name='" + BuddyName + "'", LocalSQLConn);
                object objsqlAddUser = sqlcmd.ExecuteScalar();
                if (int.Parse(objsqlAddUser.ToString()) > 0)
                {
                    SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter("SELECT * FROM Node_Status where Buddy_Name='" + BuddyName + "'", LocalSQLConn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    fncUpdateBuddyStatus(uName, BuddyName, dataTable.Rows[0][2].ToString());
                    fncUpdateBuddyStatus(BuddyName, uName, "Online");

                    VMukti.Business.clsBuddyStatus.AddBuddy(uName, BuddyName, dataTable.Rows[0][2].ToString());

                    return dataTable.Rows[0][1].ToString() + "-" + dataTable.Rows[0][2].ToString();
                }
                else
                {

                    //Conn.Close();
                    //      Conn.Dispose();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHttpAddBuddy()", "Domains\\BootstrapServiceDomain.cs");                
                return string.Empty;
            }
        }

        /// <summary>
        /// This function will be called when any user want to remove perticular buddy from his/her buddylist.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="BuddyName"></param>
        void BootstrapServiceDomain_EntHttpRemoveBuddy(string uName, List<string> BuddyName)
        {
           try
            {
                foreach (string strBuddiesName in BuddyName)
                {
                    SqlCeCommand sqlcmd = new SqlCeCommand("delete from User_BuddyList where UserName='" + uName + "' and BuddyName='" + strBuddiesName + "'", LocalSQLConn);
                    sqlcmd.ExecuteNonQuery();

                    SqlCeCommand sqlcmd1 = new SqlCeCommand("delete from User_BuddyList where UserName='" + strBuddiesName + "' and BuddyName='" + uName + "'", LocalSQLConn);
                    sqlcmd1.ExecuteNonQuery();

                    VMukti.Business.clsBuddyStatus.DeleteBuddy(uName, strBuddiesName);
                   }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntFTTPRemoveBuddy()", "Domains\\BootstrapServiceDomain.cs");
                }
        }

        /// <summary>
        /// This function will be called when user has sucessfully authorized and login to the main system. From here buddy status will be "Online".
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="IP"></param>
        /// <param name="blSuperNode"></param>
        void BootstrapServiceDomain_EntHttpBSAuthorizedUser(string uName, string IP, bool blSuperNode)
        {
            SqlCeCommand sqlcmd = null;
            try
            {
                if (uName != null && uName != string.Empty)
                {
                    //ClsException.WriteToLogFile("Authorizing user :: " + uName + " IP Address is :: " + IP + ".....node type is supernode:::  " + blSuperNode.ToString());
                }

                if (uName != null && uName != string.Empty)
                {
                    fncSNInsertBuddy(uName, "Online");
                    fncSNInsertNode(uName);
                    fncUpdateUserBuddyStatus(uName, "Online");

                    VMukti.Business.clsBuddyStatus.UpdateBuddy(uName, "Online");
                }
                if (IP != null && IP != string.Empty)
                {
                OpenConnection();
                if (blSuperNode)
                {
                    sqlcmd = new SqlCeCommand("Update SuperNode_Info Set SuperNodeName = '" + uName + "' where SuperNodeName='" + IP + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    sqlcmd.ExecuteNonQuery();
                }
                else
                {
                    sqlcmd = new SqlCeCommand("Update SuperNode_Node_Info Set Node_Name = '" + uName + "' where Node_Name='" + IP + "'");
                    sqlcmd.Connection = LocalSQLConn;
                    sqlcmd.ExecuteNonQuery();
                }
                if (blSuperNode)
                {
                    for (int i = 0; i < lstsSuperNodeInfo.Count; i++)
                    {
                        if (lstsSuperNodeInfo[i] == IP)
                        {
                            lstsSuperNodeInfo[i] = uName;
                            break;
                        }
                    }
                }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHttpBSAuthorizedUSer()", "Domains\\BootstrapServiceDomain.cs");
            }
        }


        string BootstrapServiceDomain_EntHTTPGetOfflineNodeName(string uName, string IP)
        {
            try
            {
                if (!isFirstTime)
                {
                    isFirstTime = true;
                    foreach (string str in lstsSuperNodeInfo)
                    {
                        localSuperNodeInfo.Add(str);
                    }
                   
                }
                if (localSuperNodeInfo.Contains(uName))
                {
                    localSuperNodeInfo.Remove(uName);
                }

                if (localSuperNodeInfo.Count == 1)
                {
                    isFirstTime = false;
                    string strUname = localSuperNodeInfo[0];
                    localSuperNodeInfo.Clear();
                    fncSNInsertBuddy(strUname, "Offline");
                    fncSNDeleteNode(strUname);
                    fncUpdateUserBuddyStatus(strUname, "Offline");
                    lstsSuperNodeInfo.Remove(strUname);
                    return strUname;
                }
                else
                {
                    return uName;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntHttpGetOfflineNodeName()", "Domains\\BootstrapServiceDomain.cs");                
                return string.Empty;
            }
        }

        /// <summary>
        /// This function will be used when any Node (HTTP or P2P) 's Supernode is closed, so by this function node will get his/her supernode Name
        /// and pass this name to bootstrap so bootstrap will make that supernode offline and remove entry from supernode list.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        string BootstrapServiceDomain_EntsvcGetNodeNameByIP(string NodeIP)
        {
            try
            {
               OpenConnection();
                string cmdString = "Select SuperNodeName from  SuperNode_Info where SuperNodeIP='" + NodeIP + "' ";
                SqlCeCommand NodeSel = new SqlCeCommand(cmdString, LocalSQLConn);
                object NodeName = NodeSel.ExecuteScalar();
                return (string)NodeName;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntsvcGetNodeNameByIP()", "Domains\\BootstrapServiceDomain.cs");                
                return null;
            }
        }

        /// <summary>
        /// This function will be used by Supernodes for diaster recovery. when supernode's net down or in any case when supernode lost communication with Bootstrap.
        /// when supernode gets its communication back at that time this function will return current buddies status back to him.
        /// </summary>
        /// <returns></returns>
        List<string> BootstrapServiceDomain_EntsvcGetAllBuddies()
        {
            try
            {
                List<string> lstBuddies = new List<string>();
                OpenConnection();
                string query = "select * from Node_Status";
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(query, LocalSQLConn);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstBuddies.Add(dataTable.Rows[i]["Buddy_Name"].ToString() + "-" + dataTable.Rows[i]["Buddy_Status"].ToString());
                }

                return lstBuddies;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntsvcGetAllBuddies()", "Domains\\BootstrapServiceDomain.cs");                
                return null;
            }
        }

        /// <summary>
        /// This function will be used when Bootstrap will be down or closed.....it will make all buddies offline.
        /// </summary>
        void BootstrapServiceDomain_EntMakeAllBuddyOffline()
        {
            try
            {
                fncMakeAllBuddyOffline();

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BootStrapServiceDomain_EntMakeAllBuddyOffline()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// This function will be used to change settings in configuration.xml(sqlceds35.dll.zip).
        /// </summary>
        void objBSDel_EntsvcUpdateVMuktiVersion(bool blIsMeetingPlace, bool blIsCallCenter)
        {
            try
            {
                string strVersion = "";
                if (blIsCallCenter)
                {
                    strVersion = EncodeBase64("1.1");
                }
                else
                {
                    strVersion = EncodeBase64("1.0");
                }

                XmlDocument xDoc = new XmlDocument();
                string configurationFilePath = (Assembly.GetAssembly(this.GetType()).Location.Replace("VMukti.Presentation.exe", "sqlceds35.dll"));
                xDoc.Load(configurationFilePath);

                XmlNode xn = xDoc.SelectSingleNode("/Root/OtherSettings/PhysicalPathOfVirtualDirectory");
                string VrDirectoryPath = DecodeBase64String(xn.Attributes[0].Value);

                System.IO.File.Move(VrDirectoryPath + "\\sqlceds35.dll.zip", VrDirectoryPath + "\\configuration.xml");
                xDoc.SelectSingleNode("//VMuktiVersion/@Value").InnerText = strVersion;
                xDoc.Save(VrDirectoryPath + "\\configuration.xml");
                System.IO.File.Move(VrDirectoryPath + "\\configuration.xml", VrDirectoryPath + "\\sqlceds35.dll.zip");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objBSDel_EntsvcUpdateVMuktiVersion", "Domains\\BootStrapServiceDomain");
            }

        }

        #endregion

        // Meet Code For Buddylist Implementation.
        #region DataBase Creation Function
        // public static string ClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "AllocateConferenceNumber.sdf";

        /// <summary>
        /// this function will create Node_Status table if it is not created. Buddy_Status will contains all the users with his/her status.
        /// </summary>
        void fncCreateBuddyStatusTable()
        {
            try
            {

               if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("Node_Status"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblNode_Status = new SyncTable("Node_Status");

                    SyncSchema syncSchemaLead = new SyncSchema();
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
                VMuktiHelper.ExceptionHandler(ex, "fncCreateBuddyStatusTable()", "Domains\\BootstrapServiceDomain.cs");
            }
        }


        /// <summary>
        /// This function will create "Node_Info" table and it is used to track online buddies.
        /// </summary>
        void fncCreateSNNodeInfoTable()
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
                VMuktiHelper.ExceptionHandler(ex, "fncCreateSNNodeInforTable()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// this function will created "User_BuddyList" if it is not created, this table contains all user buddies with his/her status.
        /// </summary>
        void fncCreateUserBuddyListTable()
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
                VMuktiHelper.ExceptionHandler(ex, "fncCreateUserBuddyListable()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// this function will use for create "SuperNode_Info" table. This table will store all available SuperNode with his/her name,IP. all supernode allocation
        /// task will be done from this table.
        /// </summary>
        void fncCreateUserSuperNodeInfoTable()
        {
            try
            {
               if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("SuperNode_Info"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblSuperNodeInfo = new SyncTable("SuperNode_Info");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("SuperNode_Info");
                    syncSchemaLead.Tables["SuperNode_Info"].Columns.Add("ID");
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["ID"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["SuperNode_Info"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["SuperNode_Info"].Columns.Add("SuperNodeIP");
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["SuperNodeIP"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["SuperNodeIP"].MaxLength = 30;
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["SuperNodeIP"].AllowNull = false;


                    syncSchemaLead.Tables["SuperNode_Info"].Columns.Add("NodeCount");
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["NodeCount"].ProviderDataType = SqlDbType.Int.ToString();
                    //syncSchemaLead.Tables["SuperNode_Info"].Columns["NodeCount"].MaxLength = 4;


                    syncSchemaLead.Tables["SuperNode_Info"].Columns.Add("SuperNodeName");
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["SuperNodeName"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["SuperNodeName"].MaxLength = 50;
                    syncSchemaLead.Tables["SuperNode_Info"].Columns["SuperNodeName"].AllowNull = false;


                    sync.CreateSchema(tblSuperNodeInfo, syncSchemaLead);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncCreateUserSuperNodeInfoTable()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// this function will use for keeping track of supernode's node list. e.g. how many number of nodes(HTTP or P2P) is connected with perticular supernode.
        /// </summary>
        void fncCreateUserSuperNode_NodeInfoTable()
        {
            try
            {
               if (false == File.Exists(strLocalDBPath))
                {
                    SqlCeEngine clientEngine = new SqlCeEngine(ClientConnectionString);
                    clientEngine.CreateDatabase();
                }
                if (!IsTableExits("SuperNode_Node_Info"))
                {
                    SqlCeClientSyncProvider sync = new SqlCeClientSyncProvider(ClientConnectionString);

                    SyncTable tblSuperNode_Node_Info = new SyncTable("SuperNode_Node_Info");

                    SyncSchema syncSchemaLead = new SyncSchema();
                    syncSchemaLead.Tables.Add("SuperNode_Node_Info");
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns.Add("ID");
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["ID"].ProviderDataType = SqlDbType.Int.ToString();
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["ID"].AutoIncrement = true;
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["ID"].AutoIncrementSeed = 1;
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["ID"].AutoIncrementStep = 1;
                    syncSchemaLead.Tables["SuperNode_Node_Info"].PrimaryKey = new string[] { "ID" };

                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns.Add("SuperNode_Id");
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["SuperNode_Id"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["SuperNode_Id"].MaxLength = 30;
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["SuperNode_Id"].AllowNull = false;


                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns.Add("Node_Name");
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["Node_Name"].ProviderDataType = SqlDbType.NVarChar.ToString();
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["Node_Name"].MaxLength = 50;
                    syncSchemaLead.Tables["SuperNode_Node_Info"].Columns["Node_Name"].AllowNull = false;


                    sync.CreateSchema(tblSuperNode_Node_Info, syncSchemaLead);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncCreateUserSuperNode_NodeInfoTable()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// this fucntion is used for checking whether any Table is exists in database or not.
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        bool IsTableExits(string strTableName)
        {
            try
            {
                string str = "SELECT COUNT(*) FROM Information_Schema.Tables WHERE (TABLE_NAME ='" + strTableName + "')";
                OpenConnection();
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
                VMuktiHelper.ExceptionHandler(ex, "IsTableExits()", "Domains\\BootstrapServiceDomain.cs");                
                return false;
            }
        }

        /// <summary>
        /// This table is used for checking any perticular record is present in perticular table or not.
        /// </summary>
        /// <param name="strBuddyName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strPrimaryKey"></param>
        /// <returns></returns>
        bool IsRecordExists(string strBuddyName, string strTableName, string strPrimaryKey)
        {
            try
            {
                
                string str = "SELECT COUNT(*) FROM " + strTableName + " WHERE (" + strPrimaryKey + " ='" + strBuddyName + "')";
                OpenConnection();
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
                VMuktiHelper.ExceptionHandler(ex, "IsRecoredExists()", "Domains\\BootstrapServiceDomain.cs");                
                return false;
            }

        }

        /// <summary>
        /// This table is use for update buddy's status in the table and if the buddy is already in the table then update its status.
        /// </summary>
        /// <param name="strBuddyName"></param>
        /// <param name="strBuddyStatus"></param>
        void fncSNInsertBuddy(string strBuddyName, string strBuddyStatus)
        {
            try
            {
                //ClsException.WriteToLogFile("BS NOde_status 8");
                if (!IsRecordExists(strBuddyName, "Node_Status", "Buddy_Name"))
                {                    
                    OpenConnection();
                    //ClsException.WriteToLogFile("BS NOde_status 9");
                    string str = "insert into Node_Status(Buddy_Name,Buddy_Status,Buddy_TimeStamp) values(@Buddy_Name,@Buddy_Status,@Buddy_TimeStamp) ";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Buddy_Name", strBuddyName);
                    cmd.Parameters.AddWithValue("@Buddy_Status", strBuddyStatus);
                    if (strBuddyStatus.ToLower() != "online")
                    {
                        //cmd.Parameters.AddWithValue("@Buddy_TimeStamp", DateTime.MinValue);
                        cmd.Parameters.AddWithValue("@Buddy_TimeStamp", DateTime.Now.Subtract(TimeSpan.FromMinutes(5)));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Buddy_TimeStamp", DateTime.Now);
                    }
                    cmd.ExecuteNonQuery();                    
                }
                else
                {
                    OpenConnection();
                    string str = string.Empty;
                    //ClsException.WriteToLogFile("BS NOde_status 10");
                    if (strBuddyStatus.ToLower() != "online")
                    {
                         str = "Update Node_Status Set Buddy_Status='" + strBuddyStatus + "',Buddy_TimeStamp = '"+ DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) +"' Where Buddy_Name='" + strBuddyName + "'";
                    }
                    else
                    {
                         str = "Update Node_Status Set Buddy_Status='" + strBuddyStatus + "',Buddy_TimeStamp = getdate() Where Buddy_Name='" + strBuddyName + "'";
                    }
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNInsertBuddy()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// this function will Delete buddy from the database.
        /// </summary>
        /// <param name="strBuddyName"></param>
        void fncSNDeleteBuddy(string strBuddyName)
        {
            try
            {
                if (IsRecordExists(strBuddyName, "Node_Status", "Buddy_Name"))
                {
                    OpenConnection();
                    string str = "delete from Node_Status where Buddy_Name = @Buddy_Name";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Buddy_Name", strBuddyName);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNDeleteBuddy()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// this function will use for inserting buddy in to the table "Node_Info".
        /// </summary>
        /// <param name="uName"></param>
        void fncSNInsertNode(string uName)
        {
            try
            {
                if (!IsRecordExists(uName, "Node_Info", "Node_Name"))
                {
                    OpenConnection();
                    string str = "insert into Node_Info(Node_Name) values(@Node_Name) ";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Node_Name", uName);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNInsertNode()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// THis function will delete perticular buddy from the "Node_Info"
        /// </summary>
        /// <param name="uName"></param>
        void fncSNDeleteNode(string uName)
        {
            try
            {
                if (IsRecordExists(uName, "Node_Info", "Node_Name"))
                {
                    OpenConnection();
                    string str = "delete from Node_Info where Node_Name = @Node_Name";
                    SqlCeCommand cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@Node_Name", uName);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSNDeleteNode()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// This function will update buddy status in the Buddy_Status.so when any buddy will online it will be updated to all the records who has that user
        /// as in his/her buddy list.
        /// </summary>
        /// <param name="BuddName"></param>
        /// <param name="status"></param>
        void fncUpdateUserBuddyStatus(string BuddName, string status)
        {
            try
            {               
                OpenConnection();

                SqlCeCommand cmd = null;
                //SqlCeCommand cmd = new SqlCeCommand("Update User_BuddyList Set BuddyStatus='" + status + "' Where BuddyName = '" + BuddName + "'", LocalSQLConn);
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
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncUpdateUserBuddyStatus()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// This function will use when any user has added new buddy to his/her buddy list. and if buddy is already there in his/her buddy list then it will update
        /// buddy status.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="BuddName"></param>
        /// <param name="status"></param>
        void fncUpdateBuddyStatus(string userName, string BuddName, string status)
        {
            try
            {
                OpenConnection();
                SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM User_BuddyList WHERE (UserName = '" + userName + "') AND (BuddyName = '" + BuddName + "')", LocalSQLConn);
                object i = cmd.ExecuteScalar();
                if (int.Parse(i.ToString()) > 0)
                {
                    //cmd = new SqlCeCommand("Update User_BuddyList Set BuddyStatus ='" + status + "' Where (UserName = '" + userName + "') AND (BuddyName = '" + BuddName + "')", LocalSQLConn);
                    //cmd = new SqlCeCommand("Update Node_Status Set Buddy_Status ='" + status + "',Buddy_TimeStamp=getdate() Where (Buddy_Name = '" + BuddName + "')", LocalSQLConn);

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
                    string str = "insert into User_BuddyList(UserName,BuddyName) values(@UserName,@BuddyName) ";
                    cmd = new SqlCeCommand(str);
                    cmd.Connection = LocalSQLConn;

                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@BuddyName", BuddName);
                    //cmd.Parameters.AddWithValue("@BuddyStatus", status);
                    cmd.ExecuteNonQuery();

                }
                //    ceConn.Close();
                //   ceConn.Dispose();
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncUpdateBuddyStatus()", "Domains\\BootstrapServiceDomain.cs");
            }

        }

        /// <summary>
        /// THis function will open Connection to Sdf file if connection is not open.
        /// </summary>
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
                VMuktiHelper.ExceptionHandler(ex, "OpenConnection()", "Domains\\BootstrapServiceDomain.cs");
            }

        }

        /// <summary>
        /// When main .sdf file is deleted or new version of application will come at that time all previous buddies and their buddylist will be retrived from the 
        /// main database.
        /// </summary>
        void GetBuddyInfoFromServer()
        {
            try
            {
                List<object> objBuddyStatus = VMukti.Business.clsBuddyStatus.GetAllBuddies();
                List<VMukti.Business.VmuktiBuddy.VmuktiBuddyInfo> lstVMBuddyInfo = new List<VMukti.Business.VmuktiBuddy.VmuktiBuddyInfo>();
                List<VMukti.Business.VmuktiBuddy.BuddyStatus> lstVMBuddyStatus = new List<VMukti.Business.VmuktiBuddy.BuddyStatus>();
                lstVMBuddyInfo = (List<VMukti.Business.VmuktiBuddy.VmuktiBuddyInfo>)objBuddyStatus[0];
                lstVMBuddyStatus = (List<VMukti.Business.VmuktiBuddy.BuddyStatus>)objBuddyStatus[1];
                foreach (VMukti.Business.VmuktiBuddy.VmuktiBuddyInfo VMBuddies in lstVMBuddyInfo)
                {
                    fncSNInsertNode(VMBuddies.BuddyName.Trim());
                  //  fncSNInsertBuddy(VMBuddies.BuddyName.Trim(), VMBuddies.BuddyStatus.Trim());
                    //fncSNInsertBuddy(VMBuddies.BuddyName.Trim(), "Offline");
                    fncSNInsertBuddy(VMBuddies.BuddyName.Trim(),VMBuddies.BuddyStatus.Trim());
                }

                foreach (VMukti.Business.VmuktiBuddy.BuddyStatus VMBuddyStatus in lstVMBuddyStatus)
                {
                    //fncUpdateBuddyStatus(VMBuddyStatus.UserName.Trim(), VMBuddyStatus.BuddyName.Trim(), "Offline");
                    fncUpdateBuddyStatus(VMBuddyStatus.UserName.Trim(), VMBuddyStatus.BuddyName.Trim(), VMBuddyStatus.Status.Trim());
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetBuddyInfoFromServer()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        /// <summary>
        /// This function will make all the buddies offline....it will be called when bootstrap is closed.
        /// </summary>
        void fncMakeAllBuddyOffline()
        {
            try
            {
               
                List<string> lstOnlineBuddies = new List<string>();
                OpenConnection();
                //ClsException.WriteToLogFile("BS NOde_status 13");
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
                    //ClsException.WriteToLogFile("UserName is going to Offline :: " + userName);
                    fncSNInsertBuddy(userName, "Offline");
                    fncSNDeleteNode(userName);
                    fncUpdateUserBuddyStatus(userName, "Offline");
                   // VMukti.Business.clsBuddyStatus.UpdateBuddy(userName, "Offline");
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncMakeAllBuddyOffline()", "Domains\\BootstrapServiceDomain.cs");
            }

        }
        #endregion
        // End Meet Code.       

        #region IDisposable Members

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("Dispose is calling......");
            try
            {
            if (objHttpBootStrap != null)
            {
                objHttpBootStrap = null;
            }
            if (npsBootStrapServer != null)
            {
                npsBootStrapServer.CloseServer();
                npsBootStrapServer = null;
            }
            if (npsBootStrapPredictviveServer != null)
            {
                npsBootStrapPredictviveServer.CloseServer();
                npsBootStrapPredictviveServer = null;
            }
            if (npsBootStrapPBXServer != null)
            {
                npsBootStrapPBXServer.CloseServer();
                npsBootStrapPBXServer = null;
            }
            if (npsBootStrapDashBoardService != null)
            {
                npsBootStrapDashBoardService.CloseServer();
                npsBootStrapDashBoardService = null;
            }
            if (npsBootStrapActiveAgentReportService != null)
            {
                npsBootStrapActiveAgentReportService.CloseServer();
                npsBootStrapActiveAgentReportService = null;
            }
            if (npsRecordingServer != null)
            {
                npsRecordingServer.CloseServer();
                npsRecordingServer = null;
            }
            
            if (strCurrentMachineIP != null)
            {
                strCurrentMachineIP = null;
            }
            if (appPath != null)
            {
                appPath = null;
            }
            if (awClientConnectionString != null)
            {
                awClientConnectionString = null;
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        #endregion


        //code for super node checking and deleting
        #region shilpa code

        private Boolean checkSuperNodeAvailable(string currentSuperNodeIP)
        {
            //currentSuperNodeIP is ip of super node to check existance
            //net check method

            //ClsException.WriteToLogFile("Checking Whether supernode is available or not :: " + currentSuperNodeIP);


            Uri objuri = new Uri("http://" + currentSuperNodeIP + ":" + "80/HttpSuperNode");
            System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(objuri);
            System.Net.WebResponse objResp;
            try
            {
                objResp = objWebReq.GetResponse();
                objResp.Close();
                //ClsException.WriteToLogFile("supernode is available :: " + currentSuperNodeIP);
                return true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckSuperNodeAvailable()", "Domains\\BootstrapServiceDomain.cs");                
                return false;
            }
        }

        private void adjustSuperNode(string currentSuperNodeIP)
        {
            try
            {
                SqlCeCommand sqlcmd = null;
                //ClsException.WriteToLogFile("Deleting SuperNode IP :: " + currentSuperNodeIP);

                sqlcmd = new SqlCeCommand("Select SuperNodeName from SuperNode_Info where SuperNodeIP='" + currentSuperNodeIP + "'");
                sqlcmd.Connection = LocalSQLConn;
                object objName = sqlcmd.ExecuteScalar();

                sqlcmd = new SqlCeCommand("Delete from SuperNode_Info where SuperNodeIP='" + currentSuperNodeIP + "'");
                sqlcmd.Connection = LocalSQLConn;
                int totDel = sqlcmd.ExecuteNonQuery();

                
                if (lstsSuperNodeInfo.Contains(currentSuperNodeIP))
                {
                    lstsSuperNodeInfo.Remove(currentSuperNodeIP);
                }
                else if (lstsSuperNodeInfo.Contains((string)objName))
                {
                    lstsSuperNodeInfo.Remove((string)objName);
                }

                //ClsException.WriteToLogFile("SuperNode IP Removed successfully:: " + currentSuperNodeIP);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "adjustSuperNode()", "Domains\\BootstrapServiceDomain.cs");
            }
        }

        #endregion

        private string EncodeBase64(string strValue)
        {
            try
            {
                System.Text.UTF32Encoding objUTF32 = new System.Text.UTF32Encoding();
                byte[] objbytes = objUTF32.GetBytes(strValue);
                return Convert.ToBase64String(objbytes);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "EncodeBase64()", "bootStrapServiceDomain.cs");
                return null;
            }
        }

        private string DecodeBase64String(string StrValue)
        {
            try
            {
                System.Text.UTF32Encoding objUTF32 = new System.Text.UTF32Encoding();
                byte[] objbytes = Convert.FromBase64String(StrValue);
                return objUTF32.GetString(objbytes);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DecodeBase64String()", "bootStrapServiceDomain.cs");
                return null;
            }
        }

        ~BootstrapServiceDomain()
        {
            try
            {
            Dispose();
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~BootStrapServiceDomain()", "Domains\\BootstrapServiceDomain.cs");
            }

        }

    }
}
