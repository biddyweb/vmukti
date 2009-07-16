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
    public class P2PChatClient : IDisposable
    {
        public static StringBuilder sb1;
        object objNetTcpChat;

        public INetTcpChatChannel channelNettcpChat;

        string UserName;

        int tempcounter;

        public P2PChatClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegChatp2pClient(P2PUri);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PChatClient", "P2PChatClient.cs");
            }
        }

        void RegChatp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyChat = new NetPeerClient();
                objNetTcpChat = new clsNetTcpChat();
                ((clsNetTcpChat)objNetTcpChat).EntsvcJoin += new clsNetTcpChat.delsvcJoin(DummyClient_EntsvcJoin);
                //((clsNetTcpChat)objNetTcpChat).EntsvcSendMessage += new clsNetTcpChat.delsvcSendMessage(DummyClient_EntsvcSendMessage);
                //((clsNetTcpChat)objNetTcpChat).EntsvcGetUserList += new clsNetTcpChat.delsvcGetUserList(DummyClient_EntsvcGetUserList);
                //((clsNetTcpChat)objNetTcpChat).EntsvcSetUserList += new clsNetTcpChat.delsvcSetUserList(DummyClient_EntsvcSetUserList);
                //((clsNetTcpChat)objNetTcpChat).EntsvcSignOutChat += new clsNetTcpChat.delsvcSignOutChat(DummyClient_EntsvcSignOutChat);
                ((clsNetTcpChat)objNetTcpChat).EntsvcUnJoin += new clsNetTcpChat.delsvcUnJoin(DummyClient_EntsvcUnJoin);
                channelNettcpChat = (INetTcpChatChannel)npcDummyChat.OpenClient<INetTcpChatChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpChat);

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PChatp2pClient", "P2PChatClient.cs");
            }
        }

        #region super node ipv6 event handlers

        void DummyClient_EntsvcJoin(string uName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcJoin", "P2PChatClient.cs");
            }
        }


        //void DummyClient_EntsvcSendMessage(string msg, string from, List<string> to)
        //{          
        //}

        //void DummyClient_EntsvcSignOutChat(string from, List<string> to)
        //{
        //}

        //void DummyClient_EntsvcSetUserList(string uName)
        //{
        //}

        //void DummyClient_EntsvcGetUserList(string uName)
        //{           
        //}

        void DummyClient_EntsvcUnJoin(string uName)
        {
            try
            {
                if (uName == this.UserName)
                {
                    channelNettcpChat.Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcUnJoin", "P2PChatClient.cs");
            }
            try
            {
                AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                AppDomain.Unload(AppDomain.CurrentDomain);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DummyClient_EntsvcUnJoin", "P2PChatClient.cs");
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {           
        }
        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {               
        }
      
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                Dispose(true);
               // GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "P2PChatClient.cs");
            }
        }
        private void Dispose(bool disposing)
        {

            try
            {
                //ClsException.WriteToLogFile("Dispose calling in P2P Chat");
                objNetTcpChat = null;
                channelNettcpChat = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "P2PChatClient.cs");
            }
        }

        ~P2PChatClient()
        {
            try
            {
                Dispose(false);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~P2PChatClient", "P2PChatClient.cs");
            }
        }
        #endregion
    }
}