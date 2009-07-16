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
using Presentation.Bal;
using VMuktiService;
using VMuktiAPI;
using Presentation.Bal.Service.MessageContract;

namespace Presentation.Control
{
    [Serializable]
    public class P2PPresentationClient : IDisposable
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

        object objNetTcpPresentation;

        public INetTcpPresentationChannel channelNettcpPresentation;

        string UserName;

        int tempcounter;

        public P2PPresentationClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegPresentationp2pClient(P2PUri);                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationClient()", "P2PPresentationClient.cs");
            }
        }
        void RegPresentationp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyPresentation = new NetPeerClient();
                objNetTcpPresentation = new clsNetTcpPresentation();
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcJoin += new clsNetTcpPresentation.delsvcJoin(P2PPresentationClient_EntsvcJoin);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcUnJoin += new clsNetTcpPresentation.delsvcUnJoin(P2PPresentationClient_EntsvcUnJoin);
                channelNettcpPresentation = (INetTcpPresentationChannel)npcDummyPresentation.OpenClient<INetTcpPresentationChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpPresentation);

                while (tempcounter < 20)
                {
                    try
                    {
                        clsMessageContract objContract = new clsMessageContract();
                        objContract.strFrom = UserName;
                        objContract.strMsg = "";
                        channelNettcpPresentation.svcJoin(objContract); 
                        
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegPresentationp2pClient()", "P2PPresentationClient.cs");
            }
        }

        #region super node ipv6 event handlers

        void P2PPresentationClient_EntsvcUnJoin(clsMessageContract mcUnJoin)
        {
            try
            {
                if (mcUnJoin.strFrom == this.UserName)
                {
                    channelNettcpPresentation.Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationClient_EntsvcUnJoin()/1", "P2PPresentationClient.cs");
            }
            try
            {
                AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                AppDomain.Unload(AppDomain.CurrentDomain);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationClient_EntsvcUnJoin()/2", "P2PPresentationClient.cs");
            }
        }

        void P2PPresentationClient_EntsvcJoin(clsMessageContract mcJoin)
        {
            try
            {

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationClient_EntsvcJoin()", "P2PPresentationClient.cs");
            }
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        //    try
        //    {
        //}
        //    catch (Exception ex)
        //    {
        //         VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_UnhandledException()", "P2PPresentationClient.cs");
        //    }
        }
        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
        //    try
        //    {

        //}
        //    catch (Exception ex)
        //    {
        //        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CurrentDomain_DomainUnload()", "P2PPresentationClient.cs");
        //    }
        }
#endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            Dispose(true);
          //  GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "P2PPresentationClient.cs");
            }
        }
        private void Dispose(bool disposing)
        {

            try
            {
                ClsException.WriteToLogFile("Dispose Calling in P2P Presentation");
                objNetTcpPresentation = null;
                channelNettcpPresentation = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "P2PPresentationClient.cs");
            }
        }

        ~P2PPresentationClient()
        {
            try
            {
            Dispose(false);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~P2PPresentationClient()", "P2PPresentationClient.cs");
            }
        }

        #endregion
    }
}
