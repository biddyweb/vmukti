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
using VMuktiAPI;
using VMuktiService;
using CoAuthering.Business.NetP2P;
using CoAuthering.Business.DataContracts;
using CoAuthering.Business.BasicHTTP;
using CoAuthering.Business;
using System.Text;


namespace CoAuthering.Presentation
{
    [Serializable]
    public class CoAuthDummy : IDisposable
    {
        
        object objHttpCoAuth;
        object objNetP2PCoAuthService;

        public INetP2PICoAuthServiceChannel channelNettcpCoAuth;

        public VMuktiService.BasicHttpServer HttpCoAuthServer;

        List<clsCoAuthDataMember> lstMessage = new List<clsCoAuthDataMember>();
        List<string> lstNodes = new List<string>();
        List<string> lstNodesToRemove4GetUserList = new List<string>();
        List<string> lstNodesToRemove4SetUserList = new List<string>();


        string UserName;
        //int MyId;
        
        int tempcounter;

        public CoAuthDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                //MyId = Id;

                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);
            }

            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CoAuthDummy", "CoAuthDummy.cs");
            }
        }

        void RegHttpServer(object httpUri)
        {
            
            try
            {
                objHttpCoAuth = new HTTPCoAuthService();
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcJoin += new HTTPCoAuthService.delsvcJoin(CoAuthDummy_EntsvcJoin);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcSetLength += new HTTPCoAuthService.DelsvcSetLength(CoAuthDummy_EntsvcSetLength);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcReplySetLength += new HTTPCoAuthService.DelsvcReplySetLength(CoAuthDummy_EntsvcReplySetLength);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcSetCompBytes += new HTTPCoAuthService.DelsvcSetCompBytes(CoAuthDummy_EntsvcSetCompBytes);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcSaveDoc += new HTTPCoAuthService.DelsvcSaveDoc(CoAuthDummy_EntsvcSaveDoc);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcSendChangedContext += new HTTPCoAuthService.delsvcSendChangedContext(CoAuthDummy_EntsvcSendChangedContext);
                ((HTTPCoAuthService)objHttpCoAuth).EntvcGetChangedContext += new HTTPCoAuthService.delsvcGetChangedContext(CoAuthDummy_EntvcGetChangedContext);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcUnJoin += new HTTPCoAuthService.delsvcUnJoin(CoAuthDummy_EntsvcUnJoin);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcGetUserList += new HTTPCoAuthService.delsvcGetUserList(CoAuthDummy_EntsvcGetUserList);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcSetUserList += new HTTPCoAuthService.delsvcSetUserList(CoAuthDummy_EntsvcSetUserList);
                ((HTTPCoAuthService)objHttpCoAuth).EntsvcSignOutCoAuth += new HTTPCoAuthService.delsvcSignOutCoAuth(CoAuthDummy_EntsvcSignOutCoAuth);
                HttpCoAuthServer = new BasicHttpServer(ref objHttpCoAuth, httpUri.ToString());
                HttpCoAuthServer.AddEndPoint<CoAuthering.Business.BasicHTTP.IHttpCoAuthService>(httpUri.ToString());
                HttpCoAuthServer.OpenServer();
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp,"RegHttpServer", "CoAuthDummy.cs");

            }
        }

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyChat = new NetPeerClient();
                objNetP2PCoAuthService = new NetP2PCoAuthService();

                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntSvcJoin += new NetP2PCoAuthService.DelSvcJoin(NetP2PCoAuthService_EntSvcJoin);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSaveDoc += new NetP2PCoAuthService.DelsvcSaveDoc(NetP2PCoAuthService_EntsvcSaveDoc);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetCompBytes += new NetP2PCoAuthService.DelsvcSetCompBytes(NetP2PCoAuthService_EntsvcSetCompBytes);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetLength += new NetP2PCoAuthService.DelsvcSetLength(NetP2PCoAuthService_EntsvcSetLength);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcUnJoin += new NetP2PCoAuthService.DelsvcUnJoin(NetP2PCoAuthService_EntsvcUnJoin);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcReplySetLength += new NetP2PCoAuthService.DelsvcReplySetLength(NetP2PCoAuthService_EntsvcReplySetLength);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcGetUserList += new NetP2PCoAuthService.DelsvcGetUserList(NetP2PCoAuthService_EntsvcGetUserList);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetUserList += new NetP2PCoAuthService.DelsvcSetUserList(NetP2PCoAuthService_EntsvcSetUserList);
                ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSignOutCoAuth += new NetP2PCoAuthService.delsvcSignOutCoAuth(NetP2PCoAuthService_EntsvcSignOutCoAuth);

                channelNettcpCoAuth = (INetP2PICoAuthServiceChannel)npcDummyChat.OpenClient<INetP2PICoAuthServiceChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetP2PCoAuthService);

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpCoAuth.svcJoin(UserName);
                        tempcounter = 20;
                    }
                    catch
                    {
                        tempcounter++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "RegNetP2PClient", "CoAuthDummy.cs");

            }
        }


        # region Co-Auth HTTP Functions

        void CoAuthDummy_EntsvcJoin(string uName)
        {
            lstNodes.Add(uName);
        }

        bool CoAuthDummy_EntsvcSetLength(int byteLength, string uName, string strRole)
        {
            try
            {
                channelNettcpCoAuth.svcSetLength(byteLength, uName, strRole);
                return false;

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CoAuthDummy_EntsvcSetLength", "CoAuthDummy.cs");

                return false;
            }
        }

        void CoAuthDummy_EntsvcReplySetLength(int byteLength, bool isLenghtSet, string uName)
        {
            try
            {
                channelNettcpCoAuth.svcReplySetLength(byteLength, isLenghtSet, uName);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CoAuthDummy_EntsvcReplySetLength", "CoAuthDummy.cs");
            }
        }

        void CoAuthDummy_EntsvcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers)
        {
            try
            {
                channelNettcpCoAuth.svcSetCompBytes(byteLength, myDoc, uName, strReceivers);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CoAuthDummy_EntsvcSetCompBytes", "CoAuthDummy.cs");
            }
        }

        void CoAuthDummy_EntsvcSaveDoc(string uName, List<string> to)
        {
            try
            {
                channelNettcpCoAuth.svcSaveDoc(uName, to);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CoAuthDummy_EntsvcSaveDoc", "CoAuthDummy.cs");
            }
        }

       

        List<clsCoAuthDataMember> CoAuthDummy_EntvcGetChangedContext(string recipient, string strRole)
        {
            List<clsCoAuthDataMember> myMessages = new List<clsCoAuthDataMember>();
            try
            {
                List<int> lstMsgToBeRemoved = new List<int>();
                for (int i = 0; i < lstMessage.Count; i++)
                {
                    if (lstMessage[i].strMsgType == "GetUserList" && lstMessage[i].strSender != recipient)
                    {
                        for (int intNodeCnt = 0; intNodeCnt < lstNodes.Count; intNodeCnt++)
                        {
                            if (recipient == lstNodes[intNodeCnt])
                            {
                                myMessages.Add(lstMessage[i]);
                                lstNodesToRemove4GetUserList.Add(recipient);
                                bool isCountOne = false;
                                if (lstNodes.Count == 1)
                                {
                                    isCountOne = true;
                                }
                                else if (lstNodesToRemove4GetUserList.Count == lstNodes.Count - 1)
                                {
                                    isCountOne = true;
                                }
                                if (isCountOne)
                                {
                                    lstMsgToBeRemoved.Add(i);
                                    lstNodesToRemove4GetUserList.Clear();
                                    break;
                                }
                            }
                        }

                    }
                    else if (lstMessage[i].strMsgType == "SetUserList")
                    {
                        for (int intNodeCnt = 0; intNodeCnt < lstNodes.Count; intNodeCnt++)
                        {
                            
                            myMessages.Add(lstMessage[i]);
                            if (!lstNodesToRemove4SetUserList.Contains(recipient))
                            {
                                lstNodesToRemove4SetUserList.Add(recipient);
                            }
                            bool isCountOne = false;
                            if (lstNodes.Count == 1)
                            {
                                isCountOne = true;
                            }
                            else if (lstNodesToRemove4SetUserList.Count == lstNodes.Count)
                            {
                                isCountOne = true;
                            }

                            if (isCountOne)
                            {
                                lstMsgToBeRemoved.Add(i);
                                lstNodesToRemove4SetUserList.Clear();
                                break;
                            }
                            
                        }
                    }
                    else if (lstMessage[i].strMsgType == "SetWritingFlag" && strRole == "Host")
                    {

                        myMessages.Add(lstMessage[i]);
                        lstMsgToBeRemoved.Add(i);
                    }

                    else if (lstMessage[i].strMsgType == "SetLength" && lstMessage[i].strSender == recipient)
                    {

                        myMessages.Add(lstMessage[i]);
                        lstMsgToBeRemoved.Add(i);
                    }

                    else
                    {
                        if (lstMessage[i].strReceivers != null)
                        {
                            for (int j = 0; j < lstMessage[i].strReceivers.Count; j++)
                            {

                                if (lstMessage[i].strReceivers[j] == recipient)
                                {
                                    myMessages.Add(lstMessage[i]);
                                    lstMessage[i].strReceivers.RemoveAt(j);
                                    if (lstMessage[i].strReceivers.Count == 0)
                                    {
                                        
                                        lstMsgToBeRemoved.Add(i);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                   
                }
                lstMsgToBeRemoved.Reverse();
                foreach (int pointer in lstMsgToBeRemoved)
                {
                    lstMessage.RemoveAt(pointer);
                }
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CoAuthDummy_EntvcGetChangedContext", "CoAuthDummy.cs");
                
            }
            return myMessages;
        }


        void CoAuthDummy_EntsvcSendChangedContext(byte[] myDoc, string from, string[] to)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CoAuthDummy_EntsvcSendChangedContext", "CoAuthDummy.cs");
                
            }
           
        }


        void CoAuthDummy_EntsvcUnJoin(string uName)
        {
            
            lstNodes.Remove(uName);
            channelNettcpCoAuth.svcUnJoin(uName);
            channelNettcpCoAuth.Close();
            HttpCoAuthServer.CloseServer();
            channelNettcpCoAuth.Close();
            
        }

        //void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    try
        //    {
        //    }

        //    catch (Exception ex)
        //    {
        //        VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_UnhandledException", "CoAuthDummy.cs");
                
        //    }
        //}

        //void CurrentDomain_DomainUnload(object sender, EventArgs e)
        //{
            

        //}

        void CoAuthDummy_EntsvcSetUserList(string uname)
        {
            try
            {
                channelNettcpCoAuth.svcSetUserList(uname);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CoAuthDummy_EntsvcSetUserList", "CoAuthDummy.cs");
            }
        }

        void CoAuthDummy_EntsvcGetUserList(string uname)
        {
            try
            {
                channelNettcpCoAuth.svcGetUserList(uname);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CoAuthDummy_EntsvcGetUserList", "CoAuthDummy.cs");
            }
        }

        void CoAuthDummy_EntsvcSignOutCoAuth(string from, List<string> to)
        {
            try
            {
                channelNettcpCoAuth.svcSignOutCoAuth(from, to);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CoAuthDummy_EntsvcSignOutCoAuth", "CoAuthDummy.cs");
                
            }
        }

        #endregion


        #region Co-Auth NetP2P Function Implementation

        void NetP2PCoAuthService_EntsvcSetLength(int byteLength, string uName, string strRole)
        {
            try
            {
                clsCoAuthDataMember clsDataMember = new clsCoAuthDataMember();
                clsDataMember.strMsgType = "SetWritingFlag";
                clsDataMember.strSender = uName;
                clsDataMember.byteLength = byteLength;
                clsDataMember.strReceivers = new List<string>();
                lstMessage.Add(clsDataMember);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PCoAuthService_EntsvcSetLength", "CoAuthDummy.cs");
                

            }
        }

        void NetP2PCoAuthService_EntsvcReplySetLength(int byteLength, bool isLenghtSet, string uName)
        {
            try
            {
                clsCoAuthDataMember clsDataMember = new clsCoAuthDataMember();
                clsDataMember.strMsgType = "SetLength";
                clsDataMember.strSender = uName;
                clsDataMember.isLengthSet = isLenghtSet;
                clsDataMember.strReceivers = new List<string>();
                lstMessage.Add(clsDataMember);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PCoAuthService_EntsvcReplySetLength", "CoAuthDummy.cs");
                
            }

        }

        void NetP2PCoAuthService_EntsvcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers)
        {
            try
            {
                clsCoAuthDataMember clsDataMember = new clsCoAuthDataMember();
                clsDataMember.myData = myDoc;
                clsDataMember.strReceivers = strReceivers;
                clsDataMember.isSaveOnClient = false;
                clsDataMember.strSender = uName;
                clsDataMember.intPointer = byteLength;
                clsDataMember.strMsgType = "CoAuthData";

                lstMessage.Add(clsDataMember);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp,"NetP2PCoAuthService_EntsvcSetCompBytes", "CoAuthDummy.cs");
                
            }
        }

        void NetP2PCoAuthService_EntsvcSaveDoc(string uName, List<string> to)
        {
            try
            {
               
                clsCoAuthDataMember clsCoAuthMessage = new clsCoAuthDataMember();
                clsCoAuthMessage.strSender = uName;
                clsCoAuthMessage.strReceivers = to;
                clsCoAuthMessage.strMsgType = "LoadCoAuthData";
                lstMessage.Add(clsCoAuthMessage);


            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PCoAuthService_EntsvcSaveDoc", "CoAuthDummy.cs");
                
            }
        }

        void NetP2PCoAuthService_EntSvcJoin(string uName)
        {
            
        }

        void NetP2PCoAuthService_EntsvcUnJoin(string uName)
        {
           
        }

        void NetP2PCoAuthService_EntsvcSetUserList(string uName)
        {
            try
            {
                clsCoAuthDataMember clsCoAuthMessage = new clsCoAuthDataMember();
                clsCoAuthMessage.strSender = uName;
                clsCoAuthMessage.strMsgType = "SetUserList";
                lstMessage.Add(clsCoAuthMessage);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PCoAuthService_EntsvcSetUserList", "CoAuthDummy.cs");
               
            }
        }

        void NetP2PCoAuthService_EntsvcGetUserList(string uName)
        {
            try
            {
                clsCoAuthDataMember clsCoAuthMessage = new clsCoAuthDataMember();
                clsCoAuthMessage.strSender = uName;
                clsCoAuthMessage.strMsgType = "GetUserList";
                lstMessage.Add(clsCoAuthMessage);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PCoAuthService_EntsvcGetUserList", "CoAuthDummy.cs");
                
            }
        }

        void NetP2PCoAuthService_EntsvcSignOutCoAuth(string from, List<string> to)
        {
            try
            {
                clsCoAuthDataMember clsDataMember = new clsCoAuthDataMember();
                clsDataMember.strSender = from;
                clsDataMember.strReceivers = to;
                clsDataMember.strMsgType = "SignOut";
                lstMessage.Add(clsDataMember);
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "NetP2PCoAuthService_EntsvcSignOutCoAuth", "CoAuthDummy.cs");
                
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
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "Dispose", "CoAuthDummy.cs");
            }

        }
        private void Dispose(bool disposing)
        {

            try
            {
                objHttpCoAuth = null; ;
                objNetP2PCoAuthService = null;
                channelNettcpCoAuth = null; ;
                HttpCoAuthServer = null;
                lstMessage = null;
                lstNodes = null;
                lstNodesToRemove4GetUserList = null;
                lstNodesToRemove4SetUserList = null;
                UserName = null;
               
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "Dispose", "CoAuthDummy.cs");
            }
        }

        ~CoAuthDummy()
        {
            Dispose(false);
        }

        #endregion
    }
}
