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
using Chat.Business.Service.BasicHttp;
using VMuktiService;
using Chat.Business.Service.NetP2P;
using Chat.Business.Service.DataContracts;
using VMuktiAPI;


namespace Chat.Presentation
{
    [Serializable]
    public class ChatDummy : IDisposable
    {
        public static StringBuilder sb1;
        object objHttpChat = null;
        object objNetTcpChat = null;

        public INetTcpChatChannel channelNettcpChat;

        public VMuktiService.BasicHttpServer HttpChatServer = null;

        List<clsMessage> lstMessage = new List<clsMessage>();
        List<string> lstNodes = new List<string>();
        List<string> lstNodesToRemove4GetUserList = new List<string>();
        List<string> lstNodesToRemove4SetUserList = new List<string>();


        string UserName;
        int MyId;

        int tempcounter = 0;

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

        public ChatDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                MyId = Id;

                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);
                if (netP2pUri != null || netP2pUri != "")
                {
                    ClsException.WriteToLogFile("NetP2PUri : " + netP2pUri);
                }
                if (httpUri != null || httpUri != "")
                {
                    ClsException.WriteToLogFile("HttpUri : " + httpUri);
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ChatDummy", "ChatDummy.cs");
            }
        }

        void RegHttpServer(object httpUri)
        {
            try
            {
                objHttpChat = new clsHttpChat();
                ((clsHttpChat)objHttpChat).EntsvcJoin += new Chat.Business.Service.BasicHttp.clsHttpChat.delsvcJoin(objHttp_EntsvcJoin);
                ((clsHttpChat)objHttpChat).EntsvcSendMessage += new Chat.Business.Service.BasicHttp.clsHttpChat.delsvcSendMessage(objHttp_EntsvcSendMessage);
                ((clsHttpChat)objHttpChat).EntsvcGetMessages += new Chat.Business.Service.BasicHttp.clsHttpChat.delsvcGetMessages(objHttp_EntsvcGetMessages);
                ((clsHttpChat)objHttpChat).EntsvcGetUserList += new clsHttpChat.delsvcGetUserList(objHttp_EntsvcGetUserList);
                ((clsHttpChat)objHttpChat).EntsvcSetUserList += new clsHttpChat.delsvcSetUserList(objHttp_EntsvcSetUserList);

                ((clsHttpChat)objHttpChat).EntsvcSignOutChat += new clsHttpChat.delsvcSignOutChat(objHttp_EntsvcSignOutChat);
                ((clsHttpChat)objHttpChat).EntsvcUnJoin += new Chat.Business.Service.BasicHttp.clsHttpChat.delsvcUnJoin(objHttp_EntsvcUnJoin);
                ((clsHttpChat)objHttpChat).EntsvcShowStatus += new clsHttpChat.delsvcShowStatus(objHttp_EntsvcShowStatus);

                HttpChatServer = new BasicHttpServer(ref objHttpChat, httpUri.ToString());
                HttpChatServer.AddEndPoint<Chat.Business.Service.BasicHttp.IHttpChat>(httpUri.ToString());
                HttpChatServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer", "ChatDummy.cs");
            }                                             
        }

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyChat = new NetPeerClient();
                objNetTcpChat = new clsNetTcpChat();
                ((clsNetTcpChat)objNetTcpChat).EntsvcJoin += new clsNetTcpChat.delsvcJoin(DummyClient_EntsvcJoin);
                ((clsNetTcpChat)objNetTcpChat).EntsvcSendMessage += new clsNetTcpChat.delsvcSendMessage(DummyClient_EntsvcSendMessage);
                ((clsNetTcpChat)objNetTcpChat).EntsvcGetUserList += new clsNetTcpChat.delsvcGetUserList(DummyClient_EntsvcGetUserList);
                ((clsNetTcpChat)objNetTcpChat).EntsvcSetUserList += new clsNetTcpChat.delsvcSetUserList(DummyClient_EntsvcSetUserList);
                ((clsNetTcpChat)objNetTcpChat).EntsvcSignOutChat += new clsNetTcpChat.delsvcSignOutChat(DummyClient_EntsvcSignOutChat);
                ((clsNetTcpChat)objNetTcpChat).EntsvcUnJoin += new clsNetTcpChat.delsvcUnJoin(DummyClient_EntsvcUnJoin);
                ((clsNetTcpChat)objNetTcpChat).EntsvcShowStatus += new clsNetTcpChat.delsvcShowStatus(DummyClient_EntsvcShowStatus);
                channelNettcpChat = (INetTcpChatChannel)npcDummyChat.OpenClient<INetTcpChatChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpChat);

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpChat.svcJoin(UserName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNeeetP2PClient", "ChatDummy.cs");
            }
        }





        #region chat net p2p functions

        void DummyClient_EntsvcJoin(string uName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcJoin", "ChatDummy.cs");
            }
        }

        void DummyClient_EntsvcSendMessage(string msg, string from, List<string> to)
        {
            try
            {
                clsMessage objMessage = new clsMessage();
                objMessage.strFrom = from;
                objMessage.lstTo = to;
                objMessage.strMessage = msg;
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcSendMessage", "ChatDummy.cs");
            }
        }
        void DummyClient_EntsvcSignOutChat(string from, List<string> to)
        {
            try
            {
                clsMessage objMessage = new clsMessage();
                objMessage.strFrom = from;
                objMessage.lstTo = to;
                objMessage.strMessage = "SignOut";
                lstMessage.Add(objMessage);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcSignOutChat", "ChatDummy.cs");
            }
        }
        void DummyClient_EntsvcSetUserList(string uName)
        {
            try
            {
                clsMessage objMessage = new clsMessage();
                objMessage.strFrom = uName;
                //objMessage.lstTo = to;
                objMessage.strMessage = "SetUserList";
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcSignOutChat", "ChatDummy.cs");
            }
        }

        void DummyClient_EntsvcGetUserList(string uName)
        {
            try
            {
                clsMessage objMessage = new clsMessage();
                objMessage.strFrom = uName;
                //objMessage.lstTo = to;
                objMessage.strMessage = "GetUserList";
                lstMessage.Add(objMessage);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcGetUserList", "ChatDummy.cs");
            }
        }
        void DummyClient_EntsvcUnJoin(string uName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcUnJoin", "ChatDummy.cs");
            }
        }
        void DummyClient_EntsvcShowStatus(string uname, List<string> to, string keydownstatus)
        {
            try
            {
                clsMessage objMessage = new clsMessage();
                objMessage.strFrom = uname;
                objMessage.lstTo = to;
                objMessage.strMessage = "ShowTypeMessage";
                objMessage.strShowMsg = keydownstatus;
                lstMessage.Add(objMessage);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcShowStatus", "ChatDummy.cs");
            }
        }
        #endregion

        #region chat http server functions

        void objHttp_EntsvcJoin(string uName)
        {
            try
            {
                lstNodes.Add(uName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcJoin", "ChatDummy.cs");
            }
        }
        void objHttp_EntsvcSendMessage(string msg, string from, List<string> to)
        {
            try
            {
                channelNettcpChat.svcSendMessage(msg, from, to);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcSendMessage", "ChatDummy.cs");
            }
        }
        List<clsMessage> objHttp_EntsvcGetMessages(string recipient)
        {
            try
            {
                List<clsMessage> myMessages = new List<clsMessage>();
                List<int> lstMsgToRemove = new List<int>();

                for (int i = 0; i < lstMessage.Count; i++)
                {
                    // lstNodes -- svcJoin
                    // lstMsgToRemove -- Those who have received the msg

                    if (lstMessage[i].strMessage == "GetUserList" && lstMessage[i].strFrom != recipient)
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
                                    lstMsgToRemove.Add(i);
                                    lstNodesToRemove4GetUserList.Clear();
                                    break;
                                }
                            }
                        }

                    }
                    else if (lstMessage[i].strMessage == "SetUserList")
                    {
                        for (int intNodeCnt = 0; intNodeCnt < lstNodes.Count; intNodeCnt++)
                        {
                            //if (recipient != lstNodes[intNodeCnt])
                            //{
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
                                lstMsgToRemove.Add(i);
                                lstNodesToRemove4SetUserList.Clear();
                                break;
                            }
                            //}
                        }

                        //lstMessage.RemoveAt(i);
                        //lstMsgToRemove.Add(i);
                    }
                    else if (lstMessage[i].strMessage == "SignOut")
                    {
                        if (lstMessage[i].lstTo != null)
                        {
                            for (int j = 0; j < lstMessage[i].lstTo.Count; j++)
                            {

                                if (lstMessage[i].lstTo[j] == recipient)
                                {
                                    myMessages.Add(lstMessage[i]);
                                    lstMessage[i].lstTo.RemoveAt(j);
                                    if (lstMessage[i].lstTo.Count == 0)
                                    {
                                        //lstMessage.RemoveAt(i);
                                        lstMsgToRemove.Add(i);
                                        break;
                                    }
                                }
                            }
                        }
                    }



                    else
                    {
                        if (lstMessage[i].lstTo != null)
                        {
                            for (int j = 0; j < lstMessage[i].lstTo.Count; j++)
                            {

                                if (lstMessage[i].lstTo[j] == recipient)
                                {
                                    myMessages.Add(lstMessage[i]);
                                    lstMessage[i].lstTo.RemoveAt(j);
                                    if (lstMessage[i].lstTo.Count == 0)
                                    {
                                        //lstMessage.RemoveAt(i);
                                        lstMsgToRemove.Add(i);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                lstMsgToRemove.Reverse();
                foreach (int pointer in lstMsgToRemove)
                {
                    lstMessage.RemoveAt(pointer);
                }

                return myMessages;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcGetMessages", "ChatDummy.cs");
                return null;
            }
        }
        void objHttp_EntsvcSetUserList(string uname)
        {
            try
            {
                channelNettcpChat.svcSetUserList(uname);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcSetUserList", "ChatDummy.cs");
            }
        }

        void objHttp_EntsvcGetUserList(string uname)
        {
            try
            {
                channelNettcpChat.svcGetUserList(uname);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcGetUserList", "ChatDummy.cs");
            }
        }

        void objHttp_EntsvcSignOutChat(string from, List<string> to)
        {
            try
            {
                channelNettcpChat.svcSignOutChat(from, to);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcSignOutChat", "ChatDummy.cs");
            }
        }
        void objHttp_EntsvcUnJoin(string uName)
        {
            try
            {
                lstNodes.Remove(uName);
                channelNettcpChat.Close();
                HttpChatServer.CloseServer();
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
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcUnJoin", "ChatDummy.cs");
            }
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_UnhandledException", "ChatDummy.cs");
            }
        }
        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_DomainUnload", "ChatDummy.cs");
            }
        }
        int objHttp_EntsvcShowStatus(string uname, List<string> to, string keydownstatus)
        {
            try
            {
                channelNettcpChat.svcShowStatus(uname, to, keydownstatus);
                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcShowStatus", "ChatDummy.cs");
            }
            return -1;
        }

        #endregion

        #region IDisposable Members
        // private bool disposed;

        public void Dispose()
        {
            try
            {
                ClsException.WriteToLogFile("CALLING DISPOSE(TRUE)");
                Dispose(true);
                GC.SuppressFinalize(this);
               
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "ChatDummy.cs");
            }
        }
        private void Dispose(bool disposing)
        {

            try
            {
                objHttpChat = null;
                objNetTcpChat = null;

                channelNettcpChat = null;

                HttpChatServer = null;

                lstMessage = null;
                lstNodes = null;
                lstNodesToRemove4GetUserList = null;
                lstNodesToRemove4SetUserList = null;

                //ClsException.WriteToLogFile("Disposed Called of Dummy Client");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "ChatDummy.cs");
            }
        }

        ~ChatDummy()
        {
            try
            {
                Dispose(false);
               // ClsException.WriteToLogFile("Destructor");
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~ChatDummy", "ChatDummy.cs");
            }
        }
        #endregion
    }
}