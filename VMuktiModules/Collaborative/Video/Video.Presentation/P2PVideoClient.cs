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
using Video.Business.Service.BasicHttp;
using VMuktiService;
using Video.Business.Service.NetP2P;
using Video.Business.Service.DataContracts;
using VMuktiAPI;

namespace Video.Presentation
{
    [Serializable]
    public class P2PVideoClient
    {
        public static StringBuilder sb1;
        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }

        object objNetTcpVideo = null;

        public INetP2PMainVideoChannel channelNettcpVideo;

        string UserName;      
        
        int tempcounter = 0;

        public P2PVideoClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegVideop2pClient(P2PUri);                
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "P2PVideoClient()--:--P2PVideoClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void RegVideop2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyVideo = new NetPeerClient();
                objNetTcpVideo = new ClsNetP2PMainVideo();
                ((ClsNetP2PMainVideo)objNetTcpVideo).EntsvcJoin +=new ClsNetP2PMainVideo.delsvcJoin(P2PVideoClient_EntsvcJoin);
                ((ClsNetP2PMainVideo)objNetTcpVideo).EntsvcGetUserList += new ClsNetP2PMainVideo.delsvcGetUserList(P2PVideoClient_EntsvcGetUserList);
                ((ClsNetP2PMainVideo)objNetTcpVideo).EntsvcSetUserList += new ClsNetP2PMainVideo.delsvcSetUserList(P2PVideoClient_EntsvcSetUserList);
                ((ClsNetP2PMainVideo)objNetTcpVideo).EntsvcUnJoin += new ClsNetP2PMainVideo.delsvcUnJoin(P2PVideoClient_EntsvcUnJoin);
                channelNettcpVideo = (INetP2PMainVideoChannel)npcDummyVideo.OpenClient<INetP2PMainVideoChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpVideo);

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpVideo.svcJoin(UserName);
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
                exp.Data.Add("My Key", "RegVideop2pClient()--:--P2PVideoClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        #region supernode ipv6 event handlers
        void P2PVideoClient_EntsvcSetUserList(string UName, string videoURI)
        {
            try
            {
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "P2PVideoClient_EntsvcSetUserList()--:--P2PVideoClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            
        }

        void P2PVideoClient_EntsvcGetUserList(string UName, string videoURI)
        {
            try
            {
            }

            catch (Exception exp)
            {
                exp.Data.Add("My Key", "P2PVideoClient_EntsvcGetUserList()--:--P2PVideoClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void P2PVideoClient_EntsvcUnJoin(string UName)
        {
            try
            {
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "P2PVideoClient_EntsvcUnJoin()--:--P2PVideoClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            
        }

        void P2PVideoClient_EntsvcJoin(string UName)
        {
            try
            {
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "P2PVideoClient_EntsvcJoin()--:--P2PVideoClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }

        #endregion

    }
}
