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
using System.Text;
using System.Windows;
using System.Threading;
using System.ServiceModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.ServiceModel.Channels;

using Video.Business.Service.NetP2P;
using Video.Business.Service.BasicHttp;

using VMuktiAPI;
using VMuktiService;
using Video.Business.Service.DataContracts;

namespace Video.Presentation
{
    [Serializable]
    public partial class MainVideoDummies
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
        object objClsHttpMainVideo = null;
        object objClsNetP2PMainVideo = null;

        public INetP2PMainVideoChannel objINetP2PChannel;
        public BasicHttpServer HttpMainVideoServer = null;
        List<ClsMessage> lstMessage = new List<ClsMessage>();

        string UserName;
        int MyID;
        int tempcounter = 0;

        public MainVideoDummies(string MyName, string UName, int ID, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                MyID = ID;
                RegHttpServer(httpUri);
                RegNetP2pClient(netP2pUri);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
               // ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        public void RegHttpServer(object httpUri)
        {
            try
            {
                objClsHttpMainVideo = new ClsHttpMainVideo();
                ((ClsHttpMainVideo)objClsHttpMainVideo).EntsvcJoin += new ClsHttpMainVideo.delsvcJoin(MainVideoDummies_http_EntsvcJoin);
                ((ClsHttpMainVideo)objClsHttpMainVideo).EntsvcGetUserList += new ClsHttpMainVideo.delsvcGetUserList(MainVideoDummies_http_EntsvcGetUserList);
                ((ClsHttpMainVideo)objClsHttpMainVideo).EntsvcSetUserList += new ClsHttpMainVideo.delsvcSetUserList(MainVideoDummies_http_EntsvcSetUserList);
                ((ClsHttpMainVideo)objClsHttpMainVideo).EntsvcGetMessage += new ClsHttpMainVideo.delsvcGetMessage(MainVideoDummies_http_EntsvcGetMessage);
                ((ClsHttpMainVideo)objClsHttpMainVideo).EntsvcUnJoin += new ClsHttpMainVideo.delsvcUnJoin(MainVideoDummies_http_EntsvcUnJoin);

                HttpMainVideoServer = new BasicHttpServer(ref objClsHttpMainVideo, httpUri.ToString());
                HttpMainVideoServer.AddEndPoint<IHttpMainVideo>(httpUri.ToString());
                HttpMainVideoServer.OpenServer();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--RegHttpServer()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        public void RegNetP2pClient(object NetP2pUri)
        {
            try
            {
                NetPeerClient npcDummyClient = new NetPeerClient();
                objClsNetP2PMainVideo = new ClsNetP2PMainVideo();
                ((ClsNetP2PMainVideo)objClsNetP2PMainVideo).EntsvcJoin += new ClsNetP2PMainVideo.delsvcJoin(MainVideoDummies_NetP2P_EntsvcJoin);
                ((ClsNetP2PMainVideo)objClsNetP2PMainVideo).EntsvcGetUserList += new ClsNetP2PMainVideo.delsvcGetUserList(MainVideoDummies_NetP2P_EntsvcGetUserList);
                ((ClsNetP2PMainVideo)objClsNetP2PMainVideo).EntsvcSetUserList += new ClsNetP2PMainVideo.delsvcSetUserList(MainVideoDummies_NetP2P_EntsvcSetUserList);
                ((ClsNetP2PMainVideo)objClsNetP2PMainVideo).EntsvcUnJoin += new ClsNetP2PMainVideo.delsvcUnJoin(MainVideoDummies_NetP2P_EntsvcUnJoin);

                objINetP2PChannel = (INetP2PMainVideoChannel)npcDummyClient.OpenClient<INetP2PMainVideoChannel>(NetP2pUri.ToString(), NetP2pUri.ToString().Split(':')[2].Split('/')[2], ref objClsNetP2PMainVideo);

                while (tempcounter < 20)
                {
                    try
                    {
                        objINetP2PChannel.svcJoin(UserName);
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
                ex.Data.Add("My Key", "--RegNetP2pClient()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        #region http functions

        void MainVideoDummies_http_EntsvcJoin(string UName)
        {
            try
            {
                UserName = UName;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_http_EntsvcJoin()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void MainVideoDummies_http_EntsvcGetUserList(string UName, string videoURI)
        {
            try
            {
                objINetP2PChannel.svcGetUserList(UName, videoURI);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_http_EntsvcGetUserList()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void MainVideoDummies_http_EntsvcSetUserList(string UName, string videoURI)
        {
            try
            {
                objINetP2PChannel.svcSetUserList(UName, videoURI);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_http_EntsvcSetUserList()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        List<ClsMessage> MainVideoDummies_http_EntsvcGetMessage(string recipient)
        {
            try
            {
                List<ClsMessage> myMessages = new List<ClsMessage>();
                for (int i = 0; i < lstMessage.Count; i++)
                {
                    if (lstMessage[i].strTo[0] == recipient)
                    {
                        myMessages.Add(lstMessage[i]);
                        lstMessage.RemoveAt(i);
                    }
                }
                return myMessages;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_http_EntsvcGetMessage()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                return null;
            }
        }

        void MainVideoDummies_http_EntsvcUnJoin(string UName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_NetP2P_EntsvcUnJoin()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                // ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }

        #endregion

        #region NetP2P functions

        void MainVideoDummies_NetP2P_EntsvcJoin(string UName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_NetP2P_EntsvcJoin()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                // ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }

        void MainVideoDummies_NetP2P_EntsvcGetUserList(string UName, string videoURI)
        {
            try
            {
                if (UName != UserName)
                {
                    ClsMessage objMsg = new ClsMessage();
                    objMsg.strFrom = UName;
                    objMsg.strTo = null;
                    objMsg.strMsg = "EntsvcGetUserList";
                    objMsg.strUri = videoURI;
                    lstMessage.Add(objMsg);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_NetP2P_EntsvcGetUserList()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
               // ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void MainVideoDummies_NetP2P_EntsvcSetUserList(string UName, string videoURI)
        {
            try
            {
                if (UName != UserName)
                {
                    ClsMessage objMsg = new ClsMessage();
                    objMsg.strFrom = UName;
                    objMsg.strTo = null;
                    objMsg.strMsg = "EntsvcSetUserList";
                    objMsg.strUri = videoURI;
                    lstMessage.Add(objMsg);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_NetP2P_EntsvcSetUserList()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void MainVideoDummies_NetP2P_EntsvcUnJoin(string UName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "--MainVideoDummies_NetP2P_EntsvcUnJoin()---VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--MainVideoDummies.cs--:");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
                  

        }

        #endregion
    }
}
