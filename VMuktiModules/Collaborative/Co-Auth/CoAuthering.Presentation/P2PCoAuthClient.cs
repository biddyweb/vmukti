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
using System.Text;
using CoAuthering.Business.BasicHTTP;
using VMuktiService;
using CoAuthering.Business.NetP2P;
using CoAuthering.Business.DataContracts;
using VMuktiAPI;

namespace CoAuthering.Presentation
{
    [Serializable]
    public class P2PCoAuthClient : IDisposable
    {
        
        //object objNetTcpCoAuth = null;
        object objNetTcpCoAuth;

        public INetP2PICoAuthServiceChannel channelNettcpCoAuth;

        string UserName;

        //int tempcounter = 0;
        int tempcounter;

        public P2PCoAuthClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegCoAuthp2pClient(P2PUri);               
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "P2PCoAuthClient", "P2PCoAuthClient.cs");
            }
        }       

        void RegCoAuthp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyCoAuth = new NetPeerClient();
                objNetTcpCoAuth = new NetP2PCoAuthService();

                ((NetP2PCoAuthService)objNetTcpCoAuth).EntSvcJoin += new NetP2PCoAuthService.DelSvcJoin(P2PCoAuthClient_EntSvcJoin);
                ((NetP2PCoAuthService)objNetTcpCoAuth).EntsvcUnJoin += new NetP2PCoAuthService.DelsvcUnJoin(P2PCoAuthClient_EntsvcUnJoin);
                channelNettcpCoAuth = (INetP2PICoAuthServiceChannel)npcDummyCoAuth.OpenClient<INetP2PICoAuthServiceChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpCoAuth);

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
                VMuktiHelper.ExceptionHandler(exp, "RegCoAuthp2pClient", "P2PCoAuthClient.cs");
            }
        }
       
        #region super node ipv6 event handlers
        void P2PCoAuthClient_EntsvcUnJoin(string uName)
        {
            try
            {
                if (uName == this.UserName)
                {
                    channelNettcpCoAuth.Close();
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "P2PCoAuthClient_EntsvcUnJoin", "P2PCoAuthClient.cs");
            }
          
        }

        void P2PCoAuthClient_EntSvcJoin(string uName)
        {
            try
            {

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "P2PCoAuthClient_EntSvcJoin", "P2PCoAuthClient.cs");
            }
           
        }
        //void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
          
        //}

        //void CurrentDomain_DomainUnload(object sender, EventArgs e)
        //{
           

        //}
#endregion

        #region IDisposable Members

        public void Dispose()
        {
           Dispose(true);
           
        }
        private void Dispose(bool disposing)
        {

            try
            {                
                objNetTcpCoAuth = null;
                channelNettcpCoAuth = null; ;
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "Dispose", "P2PCoAuthClient.cs");
            }
        }

        ~P2PCoAuthClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
