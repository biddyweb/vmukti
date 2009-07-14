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
using System.Windows;
using VMuktiAPI;
using Video.Business.Service.NetP2P;

namespace Video.Presentation
{
    public class clsVideoSNode
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
        object objClsHttpVideo;
        //before apply rule.
        //public BasicHttpServer HttpVideoServer = null;
        //after rule.
        public BasicHttpServer HttpVideoServer;

        List<NetPeerServer> npsVideos = new List<NetPeerServer>();
        int intServerCounter;
        UserVideoDummyClient objUserVideoDummyClient = new UserVideoDummyClient(VMuktiInfo.CurrentPeer.DisplayName);


        INetP2PUserVideoChannel netp2pDirectXVideoChannel;

        public clsVideoSNode()
        {
            try
            {
                RegHttpServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsVideoSNode", "clsVideoSNode.cs");             
            }
        }

        public void RegHttpServer()
        {
            try
            {
                objClsHttpVideo = new ClsHttpSNodeVideo();
                ((ClsHttpSNodeVideo)objClsHttpVideo).EntsvcJoin += new ClsHttpSNodeVideo.delsvcJoin(clsVideoSNode_EntsvcJoin);
                ((ClsHttpSNodeVideo)objClsHttpVideo).EntsvcStartVideoServer += new ClsHttpSNodeVideo.delsvcStartVideoServer(clsVideoSNode_EntsvcStartVideoServer);
                ((ClsHttpSNodeVideo)objClsHttpVideo).EntsvcUnJoin += new ClsHttpSNodeVideo.delsvcUnJoin(clsVideoSNode_EntsvcUnJoin);

                HttpVideoServer = new BasicHttpServer(ref objClsHttpVideo, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/NetP2PVideoHoster");
                HttpVideoServer.AddEndPoint<IHttpSNodeVideo>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/NetP2PVideoHoster");
                HttpVideoServer.OpenServer();
                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer", "clsVideoSNode.cs");
            }
        }

        #region Http Events

        void clsVideoSNode_EntsvcUnJoin(string UName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsVideoSNode_EntsvcUnJoin", "clsVideoSNode.cs");             
            }

        }

        string clsVideoSNode_EntsvcStartVideoServer(string UName, string NodeType)
        {
            try
            {
                intServerCounter++;
                npsVideos.Add(new NetPeerServer("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/VMukti/" + intServerCounter.ToString()));
                npsVideos[npsVideos.Count - 1].AddEndPoint("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/VMukti/" + intServerCounter.ToString());
                npsVideos[npsVideos.Count - 1].Name = "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/VMukti/" + intServerCounter.ToString();
                npsVideos[npsVideos.Count - 1].OpenServer();

                

                if (NodeType == VMuktiAPI.PeerType.BootStrap.ToString() || NodeType == VMuktiAPI.PeerType.NodeWithNetP2P.ToString() || NodeType == VMuktiAPI.PeerType.SuperNode.ToString())
                {
                    // Opening Client For NodeWithP2P Communication
                    if (NodeType == VMuktiAPI.PeerType.NodeWithNetP2P.ToString())
                    {
                        RegNetP2pClient("net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/VMukti/" + intServerCounter.ToString());
                    }

                    return "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/VMukti/" + intServerCounter.ToString();
                }
                else
                {
                    string httpURI = objUserVideoDummyClient.UserVideoClientWithoutDomain(intServerCounter, "net.tcp://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":4000/VMukti/" + intServerCounter.ToString(), VMuktiInfo.CurrentPeer.SuperNodeIP);
                    return httpURI;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsVideoSNode_EntsvcStartVideoServer", "clsVideoSNode.cs");             
                return null;
            }
        }
        public void RegNetP2pClient(object videoURI)
        {
            try
            {
                object objDirectXVideo = new ClsNetP2PUserVideo();
                ((ClsNetP2PUserVideo)objDirectXVideo).EntsvcJoin += new ClsNetP2PUserVideo.delsvcJoin(ctlUserVideo_EntsvcJoin);
                ((ClsNetP2PUserVideo)objDirectXVideo).EntsvcSendStream += new ClsNetP2PUserVideo.delsvcSendStream(ctlUserVideo_EntsvcSendStream);
                ((ClsNetP2PUserVideo)objDirectXVideo).EntsvcUnJoin += new ClsNetP2PUserVideo.delsvcUnJoin(ctlUserVideo_EntsvcUnJoin);

                NetPeerClient npcDirectXVideo = new NetPeerClient();
                netp2pDirectXVideoChannel = (INetP2PUserVideoChannel)npcDirectXVideo.OpenClient<INetP2PUserVideoChannel>(videoURI.ToString(), videoURI.ToString().Split(':')[2].Split('/')[2], ref objDirectXVideo);
                netp2pDirectXVideoChannel.svcJoin("");
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient", "clsVideoSNode.cs");             
            }
        }
        void clsVideoSNode_EntsvcJoin(string UName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsVideoSNode_EntsvcJoin", "clsVideoSNode.cs");                 
            }

        }

        #region NetP2P functions

        void ctlUserVideo_EntsvcJoin(string uname)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_EntsvcJoin", "clsVideoSNode.cs");             
            }

        }

        void ctlUserVideo_EntsvcSendStream(string uname, byte[] byteArrayImage)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_EntsvcSendStream", "clsVideoSNode.cs");             
            }
           
        }

        void ctlUserVideo_EntsvcUnJoin(string uname)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_EntsvcUnJoin", "clsVideoSNode.cs");             
            }
        }

        #endregion

        #endregion
    }
}
