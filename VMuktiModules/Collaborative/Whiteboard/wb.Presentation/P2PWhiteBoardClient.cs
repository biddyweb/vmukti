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
using Whiteboard.Business.Service.BasicHttp;
using VMuktiService;
using Whiteboard.Business.Service.NetP2P;
using Whiteboard.Business.Service.DataContracts;
using VMuktiAPI;

namespace wb.Presentation
{
    [Serializable]
    public class P2PWhiteBoardClient
    {
        
        object objNetTcpWhiteBoardp = null;

        public INetTcpWhiteboardChannel channelNettcpWhiteBoard;

        string UserName;      
        
        int tempcounter;

       public P2PWhiteBoardClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegWhiteBoardp2pClient(P2PUri);                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PWhiteBoardClient()", "P2PWhiteBoardClient.cs");
            }
        }

        void RegWhiteBoardp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyWhiteBoardp = new NetPeerClient();
                objNetTcpWhiteBoardp = new clsNetTcpWhiteboard();
                ((clsNetTcpWhiteboard)objNetTcpWhiteBoardp).EJoin += new clsNetTcpWhiteboard.UserJoin(P2PWhiteBoardClient_EJoin);
                ((clsNetTcpWhiteboard)objNetTcpWhiteBoardp).ESignOutChat += new clsNetTcpWhiteboard.SignOutChat(P2PWhiteBoardClient_ESignOutChat);
                channelNettcpWhiteBoard = (INetTcpWhiteboardChannel)npcDummyWhiteBoardp.OpenClient<INetTcpWhiteboardChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpWhiteBoardp);
                VMuktiAPI.ClsException.WriteToLogFile("P2PUri: " + P2PUri + " P2PUri.ToString().Split(':')[2].Split('/')[2] " + P2PUri.ToString().Split(':')[2].Split('/')[2].ToString());
                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpWhiteBoard.svcJoin(UserName);
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

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegWhiteBoardP2pClient()", "p2pWhiteBoardClient.cs");


            }
        }

        #region Supernode IPV6 event handlers

        void P2PWhiteBoardClient_ESignOutChat(string from, List<string> to)
        {
            try
            {
                if (from == this.UserName)
                {
                    channelNettcpWhiteBoard.Close();
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PWhiteBoardClient_ESignOutChat() -1", "P2PWhiteBoardClient.cs");
            }
            //try
            //{
            //    AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
            //    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //    AppDomain.Unload(AppDomain.CurrentDomain);

            //}
            //catch (Exception exp)
            //{
            //    VMuktiHelper.ExceptionHandler(exp,"P2PWhiteBoardClient_ESignOutChat()","P2PWhiteBoardClient.cs");
            //}
        }

        void P2PWhiteBoardClient_EJoin(string uname)
        {
            
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
                GC.SuppressFinalize(this);
            }
          
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose()", "P2PWhiteBoardClient.cs");
               
            }
        }
        private void Dispose(bool disposing)
        {

            try
            {
                ClsException.WriteToLogFile("Dispose calling in P2P WhiteBoard");
                objNetTcpWhiteBoardp = null;
                channelNettcpWhiteBoard = null;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose()", "P2PWhiteBoardClient.cs");

            }
        }

        ~P2PWhiteBoardClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
