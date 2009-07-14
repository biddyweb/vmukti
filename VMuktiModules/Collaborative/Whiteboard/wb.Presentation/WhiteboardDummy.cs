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
using VMuktiService;
using Whiteboard.Business.Service.NetP2P;
using Whiteboard.Business.Service.DataContracts;
using Whiteboard.Business.Service.BasicHttp;
using VMuktiAPI;


namespace wb.Presentation
{
    [Serializable]
    public class WhiteboardDummy : IDisposable
    {
       
        object objHttpWhiteboard = null;
        object objNetTcpWhiteboard = null;

        public INetTcpWhiteboardChannel channelNettcpWhiteboard;

        public VMuktiService.BasicHttpServer HttpWhiteboardServer = null;

        List<clsStrokes> lstMessage = new List<clsStrokes>();
        List<string> lstNodes = new List<string>();
        List<string> lstNodesToRemove4GetUserList = new List<string>();
        List<string> lstNodesToRemove4SetUserList = new List<string>();

        string UserName;
        //int MyId;
       
        int tempcounter;

        public WhiteboardDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                //MyId = Id;


                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);

            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy()", "WhiteBoardDummy.cs");
            }
        }

        void RegHttpServer(object httpUri)
        {
            try
            {
                objHttpWhiteboard = new clsHttpWhiteboard();
                ((clsHttpWhiteboard)objHttpWhiteboard).EJoin += new clsHttpWhiteboard.UserJoin(WhiteboardDummy_EJoin);
                ((clsHttpWhiteboard)objHttpWhiteboard).EClear += new clsHttpWhiteboard.ClearCnv(WhiteboardDummy_EClear);
                ((clsHttpWhiteboard)objHttpWhiteboard).EColor += new clsHttpWhiteboard.ChangeColor(WhiteboardDummy_EColor);
                ((clsHttpWhiteboard)objHttpWhiteboard).EEllipse += new clsHttpWhiteboard.DrawEllipse(WhiteboardDummy_EEllipse);
                ((clsHttpWhiteboard)objHttpWhiteboard).EGetStrokes += new clsHttpWhiteboard.GetAllStrokes(WhiteboardDummy_EGetStrokes);
                ((clsHttpWhiteboard)objHttpWhiteboard).ELine += new clsHttpWhiteboard.DrawLine(WhiteboardDummy_ELine);
                ((clsHttpWhiteboard)objHttpWhiteboard).ERect += new clsHttpWhiteboard.DrawRect(WhiteboardDummy_ERect);
                ((clsHttpWhiteboard)objHttpWhiteboard).EStamper += new clsHttpWhiteboard.DrawStamper(WhiteboardDummy_EStamper);
                ((clsHttpWhiteboard)objHttpWhiteboard).EStrokes += new clsHttpWhiteboard.DrawStrokes(WhiteboardDummy_EStrokes);
                ((clsHttpWhiteboard)objHttpWhiteboard).EText += new clsHttpWhiteboard.ChangeText(WhiteboardDummy_EText);
                ((clsHttpWhiteboard)objHttpWhiteboard).EThickness += new clsHttpWhiteboard.ChangeThickness(WhiteboardDummy_EThickness);
                ((clsHttpWhiteboard)objHttpWhiteboard).ETTool += new clsHttpWhiteboard.DrawTextTool(WhiteboardDummy_ETTool);
                ((clsHttpWhiteboard)objHttpWhiteboard).EGetUserList += new clsHttpWhiteboard.GetUserList(WhiteboardDummy_EGetUserList);
                ((clsHttpWhiteboard)objHttpWhiteboard).ESetUserList += new clsHttpWhiteboard.SetUserList(WhiteboardDummy_ESetUserList);
                ((clsHttpWhiteboard)objHttpWhiteboard).ESignOutChat += new clsHttpWhiteboard.SignOutChat(WhiteboardDummy_ESignOutChat);
                ((clsHttpWhiteboard)objHttpWhiteboard).EFontSize += new clsHttpWhiteboard.ChangeFontSize(WhiteboardDummy_EFontSize);
                ((clsHttpWhiteboard)objHttpWhiteboard).EUnjoin += new clsHttpWhiteboard.Unjoin(WhiteboardDummy_EUnjoin);

                VMuktiAPI.ClsException.WriteToLogFile("Opening server on URI: " + httpUri);
                HttpWhiteboardServer = new BasicHttpServer(ref objHttpWhiteboard, httpUri.ToString());
                HttpWhiteboardServer.AddEndPoint<Whiteboard.Business.Service.BasicHttp.IHttpWhiteboard>(httpUri.ToString());
                HttpWhiteboardServer.OpenServer();
                VMuktiAPI.ClsException.WriteToLogFile("Server open successfully on URI: " + httpUri);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer()", "WhiteBoardDummy.cs");
            }
        }

            

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyWhiteboard = new NetPeerClient();
                objNetTcpWhiteboard = new clsNetTcpWhiteboard();

                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EJoin += new clsNetTcpWhiteboard.UserJoin(NetP2PWhiteboard_EJoin);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EClear += new clsNetTcpWhiteboard.ClearCnv(NetP2PWhiteboard_EClear);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EColor += new clsNetTcpWhiteboard.ChangeColor(NetP2PWhiteboard_EColor);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EEllipse += new clsNetTcpWhiteboard.DrawEllipse(NetP2PWhiteboard_EEllipse);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ELine += new clsNetTcpWhiteboard.DrawLine(NetP2PWhiteboard_ELine);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ERect += new clsNetTcpWhiteboard.DrawRect(NetP2PWhiteboard_ERect);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EStamper += new clsNetTcpWhiteboard.DrawStamper(NetP2PWhiteboard_EStamper);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EStrokes += new clsNetTcpWhiteboard.DrawStrokes(NetP2PWhiteboard_EStrokes);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EText += new clsNetTcpWhiteboard.ChangeText(NetP2PWhiteboard_EText);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EThickness += new clsNetTcpWhiteboard.ChangeThickness(NetP2PWhiteboard_EThickness);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ETTool += new clsNetTcpWhiteboard.DrawTextTool(NetP2PWhiteboard_ETTool);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EGetUserList += new clsNetTcpWhiteboard.GetUserList(NetP2PWhiteboard_GetUserList);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ESetUserList += new clsNetTcpWhiteboard.SetUserList(NetP2PWhiteboard_SetUserList);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ESignOutChat += new clsNetTcpWhiteboard.SignOutChat(NetP2PWhiteboard_ESignOutChat);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EUnjoin += new clsNetTcpWhiteboard.Unjoin(NetP2PWhiteboard_EUnjoin);
                ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EFontSize += new clsNetTcpWhiteboard.ChangeFontSize(NetP2PWhiteboard_EFontSize);                
                VMuktiAPI.ClsException.WriteToLogFile("Opening P2pClient on URI: " + netP2pUri + "With mesh: " + netP2pUri.ToString().Split(':')[2].Split('/')[2].ToString());
                channelNettcpWhiteboard = (INetTcpWhiteboardChannel)npcDummyWhiteboard.OpenClient<INetTcpWhiteboardChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpWhiteboard);
                VMuktiAPI.ClsException.WriteToLogFile("Client opened Successfully");

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpWhiteboard.svcJoin(UserName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient()", "WhiteBoardDummy.cs");
            }
        }       

        #region Whiteboard WCF Events

        void WhiteboardDummy_ETTool(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
                channelNettcpWhiteboard.svcDrawTextTool(from, to, strOpName, x1, y1, x2, y2);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_ETTool()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EFontSize(string from, List<string> to, string strOpName, double fontSize)
        {
            try
            {
                channelNettcpWhiteboard.svcChangeFontSize(from, to, strOpName, fontSize);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EFontSize", "WhiteboardDummy.cs");
            }
        }   
       

        void WhiteboardDummy_EThickness(string from, List<string> to, string strOpName, double thickness)
        {
            try
            {
                channelNettcpWhiteboard.svcChangeThickNess(from, to, strOpName, thickness);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EThickness()", "WhiteBoardDummy.cs");

            }
        }

        void WhiteboardDummy_EText(string from, List<string> to, string strOpName, string text, int chldNo)
        {
            try
            {
                channelNettcpWhiteboard.svcChangeText(from, to, strOpName, text, chldNo);
            }
            catch (Exception ex)
            {

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EText()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EStrokes(string from, List<string> to, string strOpName, string strokecol)
        {
            try
            {
                channelNettcpWhiteboard.svcDrawStrokes(from, to, strOpName, strokecol);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EStrokes()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EStamper(string from, List<string> to, string strOpName, string strImg, double x1, double y1)
        {
            try
            {
                channelNettcpWhiteboard.svcDrawStamper(from, to, strOpName, strImg, x1, y1);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EStamper()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_ERect(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
                channelNettcpWhiteboard.svcDrawRect(from, to, strOpName, x1, y1, x2, y2);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_ERect()", "WhiteBoardDummy.cs");

            }
        }

        void WhiteboardDummy_ELine(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2,double thickness)
        {
            try
            {
                channelNettcpWhiteboard.svcDrawLine(from, to, strOpName, x1, y1, x2, y2,thickness);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_ELine()", "WhiteBoardDummy.cs");
            }
        }

        List<clsStrokes> WhiteboardDummy_EGetStrokes(string from, List<string> to, string strOpName, string recipient)
        {
            try
            {
                List<clsStrokes> myMessages = new List<clsStrokes>();
                List<int> lstMsgToRemove = new List<int>();

                for (int i = 0; i < lstMessage.Count; i++)
                {
                    if (lstMessage[i].strOpName == "GetUserList" && lstMessage[i].strFrom != recipient)
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

                    else if (lstMessage[i].strOpName == "SetUserList")
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
                    }

                    else if (lstMessage[i].strOpName == "SignOut")
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
                                        lstMessage.RemoveAt(i);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EGetStrokes()", "WhiteBoardDummy.cs");
                return null;
            }
        }

        void WhiteboardDummy_EEllipse(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
                channelNettcpWhiteboard.svcDrawEllipse(from, to, strOpName, x1, y1, x2, y2);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EEllipse()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EColor(string from, List<string> to, string strOpName, string color)
        {
            try
            {
                channelNettcpWhiteboard.svcChangeColor(from, to, strOpName, color);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EColor()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EClear(string from, List<string> to, string strOpName)
        {
            try
            {
                channelNettcpWhiteboard.svcClearCnv(from, to, strOpName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EClear()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EJoin(string uname)
        {
            try
            {

                lstNodes.Add(uname);
                //if (channelNettcpWhiteboard != null)
                //{
                //    channelNettcpWhiteboard.svcJoin(uname);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EJoin()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_ESetUserList(string uname)
        {
            try
            {
                channelNettcpWhiteboard.svcSetUserList(uname);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_ESetUserList()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_EGetUserList(string uname)
        {
            try
            {
                channelNettcpWhiteboard.svcGetUserList(uname);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_EGetUserList()", "WhiteBoardDummy.cs");
            }
        }

        void WhiteboardDummy_ESignOutChat(string from, List<string> to)
        {
            try
            {
                channelNettcpWhiteboard.svcSignOutChat(from, to);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_ESignOutChat()", "WhiteBoardDummy.cs");
            }

            try
            {
                lstNodes.Remove(from);
                channelNettcpWhiteboard.Close();
                HttpWhiteboardServer.CloseServer();
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "WhiteboardDummy_ESignOutChat()", "WhiteboardDummy.cs");
               
            }

            //try
            //{
            //    AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
            //    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //    AppDomain.Unload(AppDomain.CurrentDomain);
            //}
            //catch
            //{
            //}
        }

        void WhiteboardDummy_EUnjoin(string uName)
        {
            
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {

        }
        #endregion

        #region Whiteboard net p2p functions

        void NetP2PWhiteboard_ETTool(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.dblX1 = x1;
                    objMessage.dblY1 = y1;
                    objMessage.dblX2 = x2;
                    objMessage.dblY2 = y2;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_ETTool()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EThickness(string from, List<string> to, string strOpName, double thickness)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.dblThickness = thickness;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EThickness()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EFontSize(string from, List<string> to, string strOpName, double fontSize)
        {
            try
            {
                //if (from != UserName)
                //{
                clsStrokes objMessage = new clsStrokes();
                objMessage.strFrom = from;
                objMessage.lstTo = to;
                objMessage.strOpName = strOpName;
                objMessage.dblFontSize = fontSize;
                lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EFontSize()", "WhiteBoardDummy.cs");
            }
        }


        void NetP2PWhiteboard_EText(string from, List<string> to, string strOpName, string text, int chldNo)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.intCNo = chldNo;
                    objMessage.strText = text;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EText()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EStrokes(string from, List<string> to, string strOpName, string strokecol)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.strStrokeCollection = strokecol;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EStrokes()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EStamper(string from, List<string> to, string strOpName, string strImg, double x1, double y1)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.strImg = strImg;
                    objMessage.dblX1 = x1;
                    objMessage.dblY1 = y1;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EStamper()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_ERect(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.dblX1 = x1;
                    objMessage.dblY1 = y1;
                    objMessage.dblX2 = x2;
                    objMessage.dblY2 = y2;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_ERect()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_ELine(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2,double thickness)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.dblX1 = x1;
                    objMessage.dblY1 = y1;
                    objMessage.dblX2 = x2;
                    objMessage.dblY2 = y2;
                    objMessage.dblThickness = thickness;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_ELine()", "WhiteBoardDummy.cs");
            }

        }

        List<clsStrokes> NetP2PWhiteboard_EGetStrokes(string from, List<string> to, string strOpName, string strokes)
        {
            try
            {
                List<clsStrokes> myMessages = new List<clsStrokes>();
                for (int i = 0; i < lstMessage.Count; i++)
                {
                    myMessages.Add(lstMessage[i]);
                }
                lstMessage.Clear();
                return myMessages;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EGetStrokes()", "WhiteBoardDummy.cs");
                 return null;
            }
        }

        void NetP2PWhiteboard_EEllipse(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.dblX1 = x1;
                    objMessage.dblY1 = y1;
                    objMessage.dblX2 = x2;
                    objMessage.dblY2 = y2;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "NetP2PWhiteboard_EEllipse()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EColor(string from, List<string> to, string strOpName, string color)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    objMessage.strColor = color;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EColor()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EClear(string from, List<string> to, string strOpName)
        {
            try
            {
                //if (from != UserName)
                //{
                    clsStrokes objMessage = new clsStrokes();
                    objMessage.strFrom = from;
                    objMessage.lstTo = to;
                    objMessage.strOpName = strOpName;
                    lstMessage.Add(objMessage);
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_EClear()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EJoin(string uname)
        {

        }

        void NetP2PWhiteboard_GetUserList(string uname)
        {
            try
            {
                clsStrokes objMessage = new clsStrokes();
                objMessage.strFrom = uname;
                //objMessage.lstTo = to;
                objMessage.strOpName = "GetUserList";
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_GetUserList()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_SetUserList(string uname)
        {
            try
            {
                clsStrokes objMessage = new clsStrokes();
                objMessage.strFrom = uname;
                //objMessage.lstTo = to;
                objMessage.strOpName = "SetUserList";
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_SetUserList()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_ESignOutChat(string from, List<string> to)
        {
            try
            {
                clsStrokes objMessage = new clsStrokes();
                objMessage.strFrom = from;
                objMessage.lstTo = to;
                objMessage.strOpName = "SignOut";
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NetP2PWhiteboard_ESignOutChat()", "WhiteBoardDummy.cs");
            }
        }

        void NetP2PWhiteboard_EUnjoin(string uName)
        {

        }

        #endregion

        #region IDisposable Members
       // private bool disposed;

        public void Dispose()
        {
            //ClsException.WriteToLogFile("Dispose called in whiteboard");
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {

            try
            {
                //ClsException.WriteToLogFile("Dispose calling in whiteboard");
                objHttpWhiteboard = null;
                objNetTcpWhiteboard = null;

                channelNettcpWhiteboard = null;

                HttpWhiteboardServer = null;

                lstMessage = null;
                lstNodes = null;
                lstNodesToRemove4GetUserList = null;
                lstNodesToRemove4SetUserList = null;
                UserName = null;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose()", "WhiteBoardDummy.cs");
            }
        }

        ~WhiteboardDummy()
        {
            Dispose(false);
        }

        #endregion
    }
}
