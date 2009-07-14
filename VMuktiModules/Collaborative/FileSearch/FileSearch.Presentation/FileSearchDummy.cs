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
using FileSearch.Business.Service.BasicHttp;
using FileSearch.Business.Service.DataContracts;
using VMuktiService;
using FileSearch.Business.Service.NetP2P;
using FileSearch.Business;
using System.IO;
using VMuktiAPI;

namespace FileSearch.Presentation
{
    class FileSearchDummy : IDisposable
    {
        //public static StringBuilder sb1;
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
        int MyId;
        //int count = 0;
        int tempcounter;
        string UserName;
        //string MyMeshID;
        List<string> lstLocalBuddyList = new List<string>();
        System.Collections.Hashtable HTFileDownloadList = new System.Collections.Hashtable();
        object objHttpFileSearch;
        object objNetFileSearch;

        List<clsMessage> lstMessage = new List<clsMessage>();
        //System.Threading.Thread HttpThread = null;
        //System.Threading.Thread NetP2PThread = null;
        FileStream objFileStreamWriter;

        public IFileTransferChannel NetP2PChannel;
        public VMuktiService.BasicHttpServer HttpFileSearchServer;

        public FileSearchDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                MyId = Id;
                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy", "FileSearchDummy.cs");               
            }
        }

        void RegHttpServer(object httpUri)
        {
            try
            {
                objHttpFileSearch = new clsHttpFileSearch();
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpJoin += new clsHttpFileSearch.DelsvcHttpJoin(FileSearchDummy_EntsvcHttpJoin);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpReplyQuery += new clsHttpFileSearch.DelsvcHttpReplyQuery(FileSearchDummy_EntsvcHttpReplyQuery);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpSendQuery += new clsHttpFileSearch.DelsvcHttpSendQuery(FileSearchDummy_EntsvcHttpSendQuery);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpGetMessage += new clsHttpFileSearch.DelsvcHttpGetMessage(FileSearchDummy_EntsvcHttpGetMessage);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpGetFileList += new clsHttpFileSearch.DelsvcHttpGetFileList(FileSearchDummy_EntsvcHttpGetFileList);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpFileRequest += new clsHttpFileSearch.DelsvcHttpFileRequest(FileSearchDummy_EntsvcHttpFileRequest);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpFileReply += new clsHttpFileSearch.DelsvcHttpFileReply(FileSearchDummy_EntsvcHttpFileReply);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpDownloadFile += new clsHttpFileSearch.DelsvcHttpDownloadFile(FileSearchDummy_EntsvcHttpDownloadFile);
                ((clsHttpFileSearch)objHttpFileSearch).EntsvcHttpUnJoin += new clsHttpFileSearch.DelsvcHttpUnJoin(FileSearchDummy_EntsvcHttpUnJoin);
                HttpFileSearchServer = new VMuktiService.BasicHttpServer(ref objHttpFileSearch, httpUri.ToString());
                HttpFileSearchServer.AddEndPoint<IHttpFileSearch>(httpUri.ToString());
                HttpFileSearchServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer", "FileSearchDummy.cs");               
            }
        }

        #region Http Events
        void FileSearchDummy_EntsvcHttpJoin(string uName)
       {
           try
           {
               if (!lstLocalBuddyList.Contains(uName))
               {
                   lstLocalBuddyList.Add(uName);
                   NetP2PChannel.svcJoin(uName);
               }
           }
           catch (Exception ex)
           {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpJoin", "FileSearchDummy.cs");               
           }
        }

        void FileSearchDummy_EntsvcHttpReplyQuery(string uName, string[] QueryResult)
        {
            try
            {
                NetP2PChannel.svcSearchResult(QueryResult, uName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpReplyQuery", "FileSearchDummy.cs");               
            }
        }

        void FileSearchDummy_EntsvcHttpSendQuery(string uName, string Query)
        {
            try
            {
                if (NetP2PChannel != null)
                {
                    NetP2PChannel.svcSearchQuery(Query, uName, lstLocalBuddyList);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpSendQuery", "FileSearchDummy.cs");               
            }
        }

        List<clsMessage> FileSearchDummy_EntsvcHttpGetMessage(string recipient)
        {
            lock (this)
            {
                try
                {
                    List<clsMessage> myMessages = new List<clsMessage>();
                    for (int i = 0; i < lstMessage.Count; i++)
                    {
                        if (lstMessage[i].strGuestList != null)
                        {
                            for (int j = 0; j < lstMessage[i].strGuestList.Count; j++)
                            {
                                if (lstMessage[i].strGuestList[j] == recipient)
                                {
                                    myMessages.Add(lstMessage[i]);
                                    lstMessage[i].strGuestList.RemoveAt(j);
                                }
                            }
                            if (lstMessage[i].strGuestList.Count == 0)
                            {
                                lstMessage.RemoveAt(i);
                            }
                        }

                        if (lstMessage[i].FileFrom.ToString() != "")
                        {
                            if (lstMessage[i].FileTo == recipient)
                            //if (lstMessage[i].FileTo == recipient && myMessages != null)
                            {
                                //myMessages.RemoveAt(0);
                                myMessages.Add(lstMessage[i]);
                                lstMessage.RemoveAt(i);
                                //lstMessage[i].FileFrom = "";
                            }
                        }
                    }
                    //lstMessage.Clear();
                    return myMessages;
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpGetMessage", "FileSearchDummy.cs");               
                    return null;
                }
            }
        }

        string[] FileSearchDummy_EntsvcHttpGetFileList(string recipient)
        {
            lock (this)
            {
                int i = 0;
                for (i = 0; i < lstMessage.Count; i++)
                {
                    try
                    {
                        
                        if (lstMessage[i].strQueryReply != null )
                        {
                            //for (int j = 0; j < lstMessage[i].strQueryReply.Length; j++)
                            //{
                                string[] strSplit = lstMessage[i].strQueryReply[0].ToString().Split(' ');
                                if (strSplit[0] != recipient)
                                {
                                    string[] strQueryTemp = lstMessage[i].strQueryReply;
                                    lstMessage[i].strQueryReply = null;
                                    return strQueryTemp;
                                }
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpGetFileList", "FileSearchDummy.cs");               
                        return null;
                    }                    
                }
                return null;
            }
        }

        void FileSearchDummy_EntsvcHttpFileRequest(string FilePath, string FileTo, string FileFrom)
        {
            try
            {
                if (NetP2PChannel != null)
                {
                    NetP2PChannel.svcRequestFile(FilePath, FileTo, FileFrom);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpFileRequest", "FileSearchDummy.cs");               
            }                    
        }

        void FileSearchDummy_EntsvcHttpFileReply(byte[] FileBlock, string FileTo, string FileFrom, string FileName,int Signal)
        {
            lock (this)
            {
                try
                {
                    string TempRemoteFilePath = AppDomain.CurrentDomain.BaseDirectory;
                    string[] sTemp = FileName.Split('\\');
                    TempRemoteFilePath = TempRemoteFilePath + sTemp[sTemp.Length - 1];
                    if (File.Exists(TempRemoteFilePath))
                    {
                        File.Delete(TempRemoteFilePath);
                    }
                    FileStream objFileStream = new FileStream(TempRemoteFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    objFileStream.Close();

                    if (objFileStreamWriter == null)
                    {
                        objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);
                        objFileStreamWriter.Position = objFileStreamWriter.Length;
                        objFileStreamWriter.Write(FileBlock, 0, FileBlock.Length);
                        objFileStreamWriter.Close();
                        if (Signal == 1)
                        {
                            objFileStreamWriter.Dispose();
                            objFileStreamWriter = null;
                            if (!HTFileDownloadList.Contains(FileName))
                            {
                                HTFileDownloadList.Add(FileName, FileFrom);
                            }
                        }
                    }
                    else if (objFileStreamWriter != null)
                    {
                        objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);

                        objFileStreamWriter.Position = objFileStreamWriter.Length;
                        objFileStreamWriter.Write(FileBlock, 0, FileBlock.Length);
                        objFileStreamWriter.Close();
                        if (Signal == 1)
                        {
                            objFileStreamWriter.Dispose();
                            objFileStreamWriter = null;
                            if (!HTFileDownloadList.Contains(FileName))
                            {
                                HTFileDownloadList.Add(FileName, FileFrom);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpFileReply", "FileSearchDummy.cs");               
                }
            }
            NetP2PChannel.svcSendFileBlock(FileBlock, FileTo, FileFrom, FileName, Signal);
        }

       List<byte[]> FileSearchDummy_EntsvcHttpDownloadFile(string UserName, string FilePath)
        {
            try
            {
                if (HTFileDownloadList.Contains(FilePath))
                {
                    List<byte[]> lstarry = new List<byte[]>();
                    byte[] arr = new byte[5000];
                    int i = 0;
                    byte[] Larr;
                    double smallPart;
                    string[] sFileName = FilePath.Split('\\');

                    string TempRemoteFilePath = AppDomain.CurrentDomain.BaseDirectory;
                    TempRemoteFilePath = TempRemoteFilePath + sFileName[sFileName.Length - 1];
                    FileStream fst = new FileStream(TempRemoteFilePath, FileMode.Open, FileAccess.ReadWrite);

                    if (fst.Length > 5000)
                    {
                        smallPart = fst.Length / 5000;

                        for (i = 0; i < fst.Length / 5000; i++)
                        {
                            fst.Read(arr, 0, 5000);
                            lstarry.Add(arr);
                        }

                        if (i * 5000 != fst.Length)
                        {
                            Larr = new byte[int.Parse(fst.Length.ToString()) - (i * 5000)];
                            fst.Read(Larr, 0, int.Parse(fst.Length.ToString()) - (i * 5000));
                            lstarry.Add(Larr);
                            fst.Close();
                            fst.Dispose();

                        }
                    }
                    else
                    {
                        Larr = new byte[int.Parse(fst.Length.ToString())];
                        fst.Read(Larr, 0, int.Parse(fst.Length.ToString()));
                        lstarry.Add(Larr);
                        fst.Close();
                        fst.Dispose();
                    }
                    HTFileDownloadList.Remove(FilePath);
                    return lstarry;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpDownloadFile", "FileSearchDummy.cs");               
                return null;
                
            }
        }

       void FileSearchDummy_EntsvcHttpUnJoin(string uName)
       {
           try
           {
               lstLocalBuddyList.Remove(uName);
               NetP2PChannel.svcUnJoin(uName);
               NetP2PChannel.Close();
               HttpFileSearchServer.CloseServer();

           }
           catch (Exception ex)
           {
               VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcHttpUnJoin", "FileSearchDummy.cs");               
           }
           try
           {
               AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
               AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
               AppDomain.Unload(AppDomain.CurrentDomain);

           }
           catch
           {
           }
       }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_UnhandleException", "FileSearchDummy.cs");               
            }

        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_DomainUnload", "FileSearchDummy.cs");               
            }
        }
        #endregion

        #region NetP2P
        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyFileSearch = new NetPeerClient();
                objNetFileSearch = new clsNetTcpFileSearch();
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcJoin += new clsNetTcpFileSearch.delsvcJoin(FileSearchDummy_EntsvcJoin);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcRequestFile += new clsNetTcpFileSearch.delsvcRequestFile(FileSearchDummy_EntsvcRequestFile);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcSearchQuery += new clsNetTcpFileSearch.delsvcSearchQuery(FileSearchDummy_EntsvcSearchQuery);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcSearchResult += new clsNetTcpFileSearch.delsvcSearchResult(FileSearchDummy_EntsvcSearchResult);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcSendFileBlock += new clsNetTcpFileSearch.delsvcSendFileBlock(FileSearchDummy_EntsvcSendFileBlock);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcGetUserList += new clsNetTcpFileSearch.delsvcGetUserList(FileSearchDummy_EntsvcGetUserList);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcSetUserList += new clsNetTcpFileSearch.delsvcSetUserList(FileSearchDummy_EntsvcSetUserList);
                ((clsNetTcpFileSearch)objNetFileSearch).EntsvcUnJoin += new clsNetTcpFileSearch.delsvcUnJoin(FileSearchDummy_EntsvcUnJoin);
                NetP2PChannel = (IFileTransferChannel)npcDummyFileSearch.OpenClient<IFileTransferChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetFileSearch);

                while (tempcounter < 20)
                {
                    try
                    {
                        NetP2PChannel.svcJoin(UserName);
                        tempcounter = 20;
                    }
                    catch
                    {
                        tempcounter++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient", "FileSearchDummy.cs");               
            }
        }

        void FileSearchDummy_EntsvcJoin(string uName)
        {
            try
            {
                if (!lstLocalBuddyList.Contains(uName))
                {
                    lstLocalBuddyList.Add(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcJoin", "FileSearchDummy.cs");               
            }
        }

        void FileSearchDummy_EntsvcSearchQuery(string strQuery, string uName, List<string> lstGuestList)
        {
            try
            {
                //Setting Data Members
                clsMessage objMessage = new clsMessage();
                objMessage.struName = uName;
                objMessage.strSendQuery = strQuery;
                objMessage.strGuestList = lstGuestList;
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcSearchQuery", "FileSearchDummy.cs");               
            }
        }

        void FileSearchDummy_EntsvcRequestFile(string FileName, string To, string From)
        {
            lock (this)
            {
                try
                {
                    clsMessage objMessage = new clsMessage();
                    objMessage.FilePath = FileName;
                    objMessage.FileTo = To;
                    objMessage.FileFrom = From;
                    lstMessage.Add(objMessage);
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcRequestFile", "FileSearchDummy.cs");               
                }
            }
        }

        void FileSearchDummy_EntsvcSearchResult(string[] strResult, string uName)
        {
            if (strResult != null && strResult.Length>0)
            {
                try
                {
                    clsMessage objMessage = new clsMessage();
                    objMessage.struName = uName;
                    objMessage.strQueryReply = strResult;
                    lstMessage.Add(objMessage);
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcSearchResult", "FileSearchDummy.cs");               
                }
            }
        }

        void FileSearchDummy_EntsvcSendFileBlock(byte[] FileBlock, string To, string FileFrom, string FileName, int Signal)
        {
            lock (this)
            {
                try
                {
                    string TempRemoteFilePath = AppDomain.CurrentDomain.BaseDirectory;
                    string[] sTemp = FileName.Split('\\');
                    TempRemoteFilePath = TempRemoteFilePath + sTemp[sTemp.Length - 1];


                    if (File.Exists(TempRemoteFilePath))
                    {
                        File.Delete(TempRemoteFilePath);
                    }

                    FileStream objFileStream = new FileStream(TempRemoteFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    objFileStream.Close();

                    if (objFileStreamWriter == null)
                    {
                        objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);
                        objFileStreamWriter.Position = objFileStreamWriter.Length;
                        objFileStreamWriter.Write(FileBlock, 0, FileBlock.Length);
                        objFileStreamWriter.Close();
                        if (Signal == 1)
                        {
                            objFileStreamWriter.Dispose();
                            objFileStreamWriter = null;
                            if (!HTFileDownloadList.Contains(FileName))
                            {
                                HTFileDownloadList.Add(FileName, FileFrom);
                            }
                        }
                    }
                    else if (objFileStreamWriter != null)
                    {
                        objFileStreamWriter = File.OpenWrite(TempRemoteFilePath);

                        objFileStreamWriter.Position = objFileStreamWriter.Length;
                        objFileStreamWriter.Write(FileBlock, 0, FileBlock.Length);
                        objFileStreamWriter.Close();
                        if (Signal == 1)
                        {
                            objFileStreamWriter.Dispose();
                            objFileStreamWriter = null;
                            if (!HTFileDownloadList.Contains(FileName))
                            {
                                HTFileDownloadList.Add(FileName, FileFrom);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcSendFileBlock", "FileSearchDummy.cs");               
                }
            }
        }

        void FileSearchDummy_EntsvcSetUserList(string uName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcSetUserList", "FileSearchDummy.cs");               
            }
        }

        void FileSearchDummy_EntsvcGetUserList(string uName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcGetUserList", "FileSearchDummy.cs");               
            }
        }

        void FileSearchDummy_EntsvcUnJoin(string uName)
        {
            try
            {
                lstLocalBuddyList.Remove(uName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchDummy_EntsvcUnJoin", "FileSearchDummy.cs");               
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "FileSearchDummy.cs");               
            }
        }
        
        private void Dispose(bool disposing)
        {

            try
            {
                lstLocalBuddyList = null;
                HTFileDownloadList = null;
                objHttpFileSearch = null;
                objNetFileSearch = null;

                lstMessage = null;
                objFileStreamWriter = null;
                NetP2PChannel = null;
                HttpFileSearchServer = null;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose", "FileSearchDummy.cs");               
            }
        }

        ~FileSearchDummy()
        {
            Dispose(false);
        }

        #endregion
    }
}
