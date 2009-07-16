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
    public partial class UserVideoDummies
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
        object objClsHttpUserVideo = null;
        object objClsNetP2PUserVideo = null;

        public INetP2PUserVideoChannel objINetP2PChannel;
        public BasicHttpServer HttpUserVideoServer = null;
        public List<clsImages> lstImages = new List<clsImages>();

        string UserName;
        int MyID;
        int tempcounter;

        public UserVideoDummies(string MyName, string UName, int ID, string netP2pUri, string httpUri)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies", "UserVideoDummies.cs");             
            }
        }

        public void RegHttpServer(object httpUri)
        {
            try
            {
                objClsHttpUserVideo = new ClsHttpUserVideo();
                ((ClsHttpUserVideo)objClsHttpUserVideo).EntsvcJoin += new ClsHttpUserVideo.delsvcJoin(UserVideoDummies_http_EntsvcJoin);
                ((ClsHttpUserVideo)objClsHttpUserVideo).EntsvcSendStream += new ClsHttpUserVideo.delsvcSendStream(UserVideoDummies_http_EntsvcSendStream);
                ((ClsHttpUserVideo)objClsHttpUserVideo).EntsvcReceiveStream += new ClsHttpUserVideo.delsvcReceiveStream(UserVideoDummies_http_EntsvcReceiveStream);
                ((ClsHttpUserVideo)objClsHttpUserVideo).EntsvcUnJoin += new ClsHttpUserVideo.delsvcUnJoin(UserVideoDummies_http_EntsvcUnJoin);

                HttpUserVideoServer = new BasicHttpServer(ref objClsHttpUserVideo, httpUri.ToString());
                HttpUserVideoServer.AddEndPoint<IHttpUserVideo>(httpUri.ToString());
                HttpUserVideoServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer", "UserVideoDummies.cs");             
            }
        }

        public void RegNetP2pClient(object NetP2pUri)
        {
            try
            {
                NetPeerClient npcDummyClient = new NetPeerClient();
                objClsNetP2PUserVideo = new ClsNetP2PUserVideo();
                ((ClsNetP2PUserVideo)objClsNetP2PUserVideo).EntsvcJoin += new ClsNetP2PUserVideo.delsvcJoin(UserVideoDummies_NetP2P_EntsvcJoin);
                ((ClsNetP2PUserVideo)objClsNetP2PUserVideo).EntsvcSendStream += new ClsNetP2PUserVideo.delsvcSendStream(UserVideoDummies_NetP2P_EntsvcSendStream);
                ((ClsNetP2PUserVideo)objClsNetP2PUserVideo).EntsvcUnJoin += new ClsNetP2PUserVideo.delsvcUnJoin(UserVideoDummies_NetP2P_EntsvcUnJoin);

                //((ClsNetP2PMainVideo)objClsNetP2PUserVideo).EntsvcJoin += new ClsNetP2PUserVideo.delsvcJoin(UserVideoDummies_NetP2P_EntsvcJoin);
                //((ClsNetP2PMainVideo)objClsNetP2PUserVideo).EntsvcSendStream += new ClsNetP2PUserVideo.delsvcSendStream(UserVideoDummies_NetP2P_EntsvcSendStream);
                //((ClsNetP2PMainVideo)objClsNetP2PUserVideo).EntsvcUnJoin += new ClsNetP2PUserVideo.delsvcUnJoin(UserVideoDummies_NetP2P_EntsvcUnJoin);

                

                objINetP2PChannel = (INetP2PUserVideoChannel)npcDummyClient.OpenClient<INetP2PUserVideoChannel>(NetP2pUri.ToString(), NetP2pUri.ToString().Split(':')[2].Split('/')[2], ref objClsNetP2PUserVideo);

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient", "UserVideoDummies.cs");             
            }
        }


        #region http functions

        void UserVideoDummies_http_EntsvcJoin(string UName)
        {
            try
            {
                UserName = UName;
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_http_EntsvcJoin", "UserVideoDummies.cs");             
            }
        }

        void UserVideoDummies_http_EntsvcSendStream(string UName, byte[] byteArrayImage)
        {
            try
            {
                if (objINetP2PChannel != null)
                {
                    objINetP2PChannel.svcSendStream(UName, byteArrayImage);
                }
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_http_EntsvcSendStream", "UserVideoDummies.cs");             
            }
        }

        List<clsImages> UserVideoDummies_http_EntsvcReceiveStream(string UName)
        {
            try
            {
                List<clsImages> myImages = new List<clsImages>();
                for (int i = 0; i < lstImages.Count; i++)
                {
                    myImages.Add(lstImages[i]);
                }
                lstImages.Clear();
                return myImages;
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_http_EntsvcReceiveStream", "UserVideoDummies.cs");             
                return null;
            }
        }

        void UserVideoDummies_http_EntsvcUnJoin(string UName)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_http_EntsvcUnJoin", "UserVideoDummies.cs");             
            }

        }

        #endregion

        #region NetP2P functions

        void UserVideoDummies_NetP2P_EntsvcJoin(string uname)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_NetP2P_EntsvcJoin", "UserVideoDummies.cs");             
            }

        }

        void UserVideoDummies_NetP2P_EntsvcSendStream(string uname, byte[] byteArrayImage)
        {
            try
            {
                if (uname != UserName)
                {
                    clsImages objImage = new clsImages();
                    objImage.strUsername = uname;
                    objImage.byteArrayImage = byteArrayImage;
                    lstImages.Add(objImage);
                }
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_NetP2P_EntsvcSendStream", "UserVideoDummies.cs");             
            }
        }

        void UserVideoDummies_NetP2P_EntsvcUnJoin(string uname)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummies_NetP2P_EntsvcUnJoin", "UserVideoDummies.cs");             
            }

        }

        #endregion

    }
}
