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
using VMuktiService;
using FileSearch.Business.Service.NetP2P;
using FileSearch.Business.Service.DataContracts;
using VMuktiAPI;
using FileSearch.Business;


namespace FileSearch.Presentation
{
    [Serializable]
    public class P2PFileSearchClient : IDisposable
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
         object objNetTcpFileSearch;

         public IFileTransferChannel channelNettcpFileTransfer;

        string UserName;      
        
        int tempcounter;

        public P2PFileSearchClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegFileSearchp2pClient(P2PUri);                
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PFileSearchClient", "P2PFileSearchClient.cs");               
            }
        }

        void RegFileSearchp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyFileSearch = new NetPeerClient();
                objNetTcpFileSearch = new clsNetTcpFileSearch();
                ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcJoin += new clsNetTcpFileSearch.delsvcJoin(P2PFileSearchClient_EntsvcJoin);
                ((clsNetTcpFileSearch)objNetTcpFileSearch).EntsvcUnJoin += new clsNetTcpFileSearch.delsvcUnJoin(P2PFileSearchClient_EntsvcUnJoin);
                channelNettcpFileTransfer = (IFileTransferChannel)npcDummyFileSearch.OpenClient<IFileTransferChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpFileSearch);

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpFileTransfer.svcJoin(UserName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "RegFileSearchp2pClient", "P2PFileSearchClient.cs");               
            }
        }

        #region supernode IPV6 event handlers

        void P2PFileSearchClient_EntsvcUnJoin(string uName)
        {
            try
            {
                if (uName == this.UserName)
                {
                    channelNettcpFileTransfer.Close();
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PFileSearchClient_EntsvcUnJoin", "P2PFileSearchClient.cs");               
            }
            try
            {
                AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                AppDomain.Unload(AppDomain.CurrentDomain);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PFileSearchClient_EntsvcUnJoin", "P2PFileSearchClient.cs");               
            }
        }

        void P2PFileSearchClient_EntsvcJoin(string uName)
        {
            try
            {
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PFileSearchClient_EntsvcJoin", "P2PFileSearchClient.cs");               
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
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose", "P2PFileSearchClient.cs");               
            }
        }
        private void Dispose(bool disposing)
        {

            try
            {
                ClsException.WriteToLogFile("Dispose calling in P2P FileSearch");
                objNetTcpFileSearch = null;
                channelNettcpFileTransfer = null;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose", "P2PFileSearchClient.cs");               
            }
        }

        ~P2PFileSearchClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
