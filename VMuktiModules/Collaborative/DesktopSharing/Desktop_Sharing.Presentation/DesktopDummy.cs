using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Desktop_Sharing.Business.Service.NetP2P;
using Desktop_Sharing.Business.Service.BasicHttp;
using Desktop_Sharing.Business.Service.DataContracts;
using Desktop_Sharing.Business.Service.MessageContract;
using System.IO;
using System.Collections;
using VMuktiAPI;
using VMuktiService;
 
namespace Desktop_Sharing.Presentation
{
    [Serializable]
    public class DesktopDummy
    {
        object objHttpDesktop;
        object objNetTcpDesktop;

        public INetTcpDesktopChannel channelNettcpDesktop;

        public VMuktiService.BasicHttpServer HttpDesktopServer;

        Hashtable hashMessages = new Hashtable();
        //List<clsMessage> lstMessage = new List<clsMessage>();
        List<clsMessage> lstNodes = new List<clsMessage>();
       // List<string> lstNodesToRemove4GetUserList = new List<string>();
       // List<string> lstNodesToRemove4SetUserList = new List<string>();
        clsGetMessage objMouse = new clsGetMessage();
        string UserName;
        //int MyId;
        int tempcounter;
        string strXY;
        double gX;
        double gY;
        string strTO;
        string strSelected;

        public DesktopDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                //MyId = Id;

                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "DesktopDummy", "DesktopDummy.cs");
            }
        }

        private void RegHttpServer(string httpUri)
        {
            try
            {
                objHttpDesktop = new clsHttpDesktop();
                ((clsHttpDesktop)objHttpDesktop).EntsvcJoin += new clsHttpDesktop.delsvcJoin(Http_EntsvcJoin);
                ((clsHttpDesktop)objHttpDesktop).EntsvcGetUserList += new clsHttpDesktop.delsvcGetUserList(Http_EntsvcGetUserList);
                ((clsHttpDesktop)objHttpDesktop).EntsvcSetUserList += new clsHttpDesktop.delsvcSetUserList(Http_EntsvcSetUserList);
                ((clsHttpDesktop)objHttpDesktop).EntsvcSendMessage += new clsHttpDesktop.delsvcSendMessage(Http_EntsvcSendMessage);
                ((clsHttpDesktop)objHttpDesktop).EntsvcSelectedDesktop += new clsHttpDesktop.delsvcSelectedDesktop(Http_EntsvcSelectedDesktop);
                ((clsHttpDesktop)objHttpDesktop).EntsvcStopControl += new clsHttpDesktop.delsvcStopControl(Http_EntsvcStopControl);
                ((clsHttpDesktop)objHttpDesktop).EntsvcBtnDown += new clsHttpDesktop.delsvcBtnDown(Http_EntsvcBtnDown);
                ((clsHttpDesktop)objHttpDesktop).EntsvcBtnUp += new clsHttpDesktop.delsvcBtnUp(Http_EntsvcBtnUp);
                ((clsHttpDesktop)objHttpDesktop).EntsvcSendXY += new clsHttpDesktop.delsvcSendXY(Http_EntsvcSendXY);
                ((clsHttpDesktop)objHttpDesktop).EntsvcSendKey += new clsHttpDesktop.delsvcSendKey(Http_EntsvcSendKey);
                ((clsHttpDesktop)objHttpDesktop).EntsvcGetMessages += new clsHttpDesktop.delsvcGetMessages(Http_EntsvcGetMessages);
                ((clsHttpDesktop)objHttpDesktop).EntsvcAllowView += new clsHttpDesktop.delsvcAllowView(Http_EntsvcAllowView);
                ((clsHttpDesktop)objHttpDesktop).EntsvcAllowControl += new clsHttpDesktop.delsvcAllowControl(Http_EntsvcAllowControl);
                ((clsHttpDesktop)objHttpDesktop).EntsvcUnJoin += new clsHttpDesktop.delsvcUnJoin(Http_EntsvcUnJoin);

                HttpDesktopServer = new BasicHttpServer(ref objHttpDesktop, httpUri.ToString());
                HttpDesktopServer.AddEndPoint<IHttpDesktop>(httpUri.ToString());
                HttpDesktopServer.objBasicHttpBinding.TransferMode = System.ServiceModel.TransferMode.Streamed;
                HttpDesktopServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RegHttpServer", "DesktopDummy.cs");
            }
        }

        private void RegNetP2PClient(string netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyDesktop = new NetPeerClient();
                objNetTcpDesktop = new clsNetTcpDesktop();
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcJoin += new clsNetTcpDesktop.delsvcJoin(DesktopDummy_EntsvcJoin);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendMessage += new clsNetTcpDesktop.delsvcSendMessage(DesktopDummy_EntsvcSendMessage);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcGetUserList += new clsNetTcpDesktop.delsvcGetUserList(DesktopDummy_EntsvcGetUserList);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSetUserList += new clsNetTcpDesktop.delsvcSetUserList(DesktopDummy_EntsvcSetUserList);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSelectedDesktop += new clsNetTcpDesktop.delsvcSelectedDesktop(DesktopDummy_EntsvcSelectedDesktop);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcStopControl += new clsNetTcpDesktop.delsvcStopControl(DesktopDummy_EntsvcStopControl);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcBtnUp += new clsNetTcpDesktop.delsvcBtnUp(DesktopDummy_EntsvcBtnUp);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcBtnDown += new clsNetTcpDesktop.delsvcBtnDown(DesktopDummy_EntsvcBtnDown);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendKey += new clsNetTcpDesktop.delsvcSendKey(DesktopDummy_EntsvcSendKey);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendXY += new clsNetTcpDesktop.delsvcSendXY(DesktopDummy_EntsvcSendXY);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcAllowView += new clsNetTcpDesktop.delsvcAllowView(DesktopDummy_EntsvcAllowView);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcAllowControl += new clsNetTcpDesktop.delsvcAllowControl(DesktopDummy_EntsvcAllowControl);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcUnJoin += new clsNetTcpDesktop.delsvcUnJoin(DesktopDummy_EntsvcUnJoin);

                channelNettcpDesktop = (INetTcpDesktopChannel)npcDummyDesktop.OpenClient<INetTcpDesktopChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpDesktop);

                while (tempcounter < 20)
                {
                    try
                    {
                        #region MsgContract
                        clsMessageContract objContract = new clsMessageContract();
                        objContract.blControl = false;
                        objContract.blView = false;
                        objContract.key = 0;
                        objContract.mouseButton = 0;
                        objContract.stremImage = new MemoryStream();
                        objContract.strFrom = UserName;
                        objContract.strTo = "";
                        objContract.strType = "";
                        objContract.x = 0;
                        objContract.y = 0;
                        #endregion MsgContract
                        channelNettcpDesktop.svcJoin(objContract);
                        tempcounter = 20;
                    }
                    catch
                    {
                        tempcounter++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient", "DesktopDummy.cs");
            }
        }

        #region NetTCP Events

        void DesktopDummy_EntsvcJoin(clsMessageContract objContract)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "desktopDummy_EntsvcJoin", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcGetUserList(clsMessageContract objContract)
        {
            try
            {
//                MemoryStream mmsFinal = new MemoryStream();
                #region 20oct changes
                clsGetMessage objGetMessage = new clsGetMessage();
                objGetMessage.strFrom = objContract.strFrom;
                objGetMessage.strType = "";
                objGetMessage.strTo = "";
                objGetMessage.x = new double();
                objGetMessage.y = new double();
                objGetMessage.stremImage = new MemoryStream();
                objGetMessage.mouseButton = new int();
                objGetMessage.key = new int();
                objGetMessage.blView = new bool();
                objGetMessage.blControl = new bool();
                objGetMessage.id = 1;
                objGetMessage.ControlTag = new int();
                objGetMessage.ViewTag = new int();
                #endregion 20oct changes               
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                    lstStream.Add(objGetMessage);
                    hashMessages[lstNodes[i].uName] = lstStream;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcGetUserList", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcSetUserList(clsMessageContract objContract)
        {
            try
            {

                #region 20oct changes
                clsGetMessage objGetMessage = new clsGetMessage();
                objGetMessage.strFrom = objContract.strFrom;
                objGetMessage.strType = "";
                objGetMessage.strTo = "";
                objGetMessage.x = new double();
                objGetMessage.y = new double();
                objGetMessage.stremImage = new MemoryStream();
                objGetMessage.mouseButton = new int();
                objGetMessage.key = new int();
                objGetMessage.blView = new bool();
                objGetMessage.blControl = new bool();
                objGetMessage.id = 2;
                objGetMessage.ControlTag = new int();
                objGetMessage.ViewTag = new int();
                #endregion 20oct changes
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];                    
                    lstStream.Add(objGetMessage);
                    hashMessages[lstNodes[i].uName] = lstStream;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcSetUserList", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcSendMessage(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    #region 20oct changes
                    clsGetMessage objGetMessage = new clsGetMessage();
                    objGetMessage.strFrom = objContract.strFrom;
                    objGetMessage.strType = "";
                    objGetMessage.strTo = "";
                    objGetMessage.x = new double();
                    objGetMessage.y = new double();
                    objGetMessage.stremImage = objContract.stremImage;
                    objGetMessage.mouseButton = new int();
                    objGetMessage.key = new int();
                    objGetMessage.blView = new bool();
                    objGetMessage.blControl = new bool();
                    objGetMessage.id = 3;
                    objGetMessage.ControlTag = new int();
                    objGetMessage.ViewTag = new int();
                    #endregion 20oct changes
                    string uNameImg = objContract.strFrom;        

                    for (int i = 0; i < lstNodes.Count; i++)
                    {

                        if (lstNodes[i].isControlled)
                        {
                        }
                        else if ((!lstNodes[i].isControlled) && (!lstNodes[i].isControlling))
                        {
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;
                        }
                        if (lstNodes[i].isControlling && lstNodes[i].strControlled==uNameImg)
                        {
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcSendMessage", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcSelectedDesktop(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i].uName == objContract.strFrom)
                        {

                            #region 20oct changes
                            clsGetMessage objGetMessage = new clsGetMessage();
                            objGetMessage.strFrom = objContract.strFrom;
                            objGetMessage.strType = "";
                            objGetMessage.strTo = "";
                            objGetMessage.x = new double();
                            objGetMessage.y = new double();
                            objGetMessage.stremImage = new MemoryStream();
                            objGetMessage.mouseButton = new int();
                            objGetMessage.key = new int();
                            objGetMessage.blView = new bool();
                            objGetMessage.blControl = new bool();
                            objGetMessage.id = 4;
                            objGetMessage.ControlTag = new int();
                            objGetMessage.ViewTag = new int();
                            #endregion 20oct changes
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Clear();
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;

                            strSelected = objContract.strFrom;

                            lstNodes[i].isControlled = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcSelectedDesktop", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcStopControl(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {

                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i].uName == objContract.strFrom)
                        {
                            
                            #region 20oct changes
                            clsGetMessage objGetMessage = new clsGetMessage();
                            objGetMessage.strFrom = objContract.strFrom;
                            objGetMessage.strType = "";
                            objGetMessage.strTo = "";
                            objGetMessage.x = new double();
                            objGetMessage.y = new double();
                            objGetMessage.stremImage = new MemoryStream();
                            objGetMessage.mouseButton = new int();
                            objGetMessage.key = new int();
                            objGetMessage.blView = new bool();
                            objGetMessage.blControl = new bool();
                            objGetMessage.id = 5;
                            objGetMessage.ControlTag = new int();
                            objGetMessage.ViewTag = new int();
                            #endregion 20oct changes
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Clear();
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;
                            strSelected = "";
                            lstNodes[i].isControlled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcStopControl", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcBtnUp(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    
                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i].uName == objContract.strTo)
                        {
                            #region 20oct changes
                            clsGetMessage objGetMessage = new clsGetMessage();
                            objGetMessage.strFrom = "";
                            objGetMessage.strType = "";
                            objGetMessage.strTo = objContract.strTo;
                            objGetMessage.x = new double();
                            objGetMessage.y = new double();
                            objGetMessage.stremImage = new MemoryStream();
                            objGetMessage.mouseButton = objContract.mouseButton;
                            objGetMessage.key = new int();
                            objGetMessage.blView = new bool();
                            objGetMessage.blControl = new bool();
                            objGetMessage.id = 6;
                            objGetMessage.ViewTag = new int();
                            objGetMessage.ControlTag = new int();
                            #endregion 20oct changes
                            if (lstNodes[i].uName == strXY)
                            {
                                List<clsGetMessage> lstStreamPrev = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                                lstStreamPrev.Add(objMouse);
                                hashMessages[lstNodes[i].uName] = lstStreamPrev;
                            }
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcBtnUp", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcBtnDown(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i].uName == objContract.strTo)
                        {
                           
                            #region 20oct changes
                            clsGetMessage objGetMessage = new clsGetMessage();
                            objGetMessage.strFrom = "";
                            objGetMessage.strType = "";
                            objGetMessage.strTo = objContract.strTo;
                            objGetMessage.x = new double();
                            objGetMessage.y = new double();
                            objGetMessage.stremImage = new MemoryStream();
                            objGetMessage.mouseButton = objContract.mouseButton;
                            objGetMessage.key = new int();
                            objGetMessage.blView = new bool();
                            objGetMessage.blControl = new bool();
                            objGetMessage.id = 7;
                            objGetMessage.ViewTag = new int();
                            objGetMessage.ControlTag = new int();
                            #endregion 20oct changes

                            if (lstNodes[i].uName == strXY)
                            {
                                List<clsGetMessage> lstStreamPrev = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                                lstStreamPrev.Add(objMouse);
                                hashMessages[lstNodes[i].uName] = lstStreamPrev;
                            }
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcBtnDown", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcSendXY(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i].uName == objContract.strTo)
                        {
                            strXY = objContract.strTo;                           
                            #region 20oct changes
                            
                            objMouse.strFrom = "";
                            objMouse.strType = "";
                            objMouse.strTo = objContract.strTo;
                            objMouse.x = objContract.x;
                            objMouse.y = objContract.y;
                            objMouse.stremImage = new MemoryStream();
                            objMouse.mouseButton = new int();
                            objMouse.key = new int();
                            objMouse.blView = new bool();
                            objMouse.blControl = new bool();
                            objMouse.id = 8;
                            objMouse.ViewTag = new int();
                            objMouse.ControlTag = new int();
                            #endregion 20oct changes
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcSendXY", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcSendKey(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i].uName == objContract.strTo)
                        {
                          
                            #region 20oct changes
                            clsGetMessage objGetMessage = new clsGetMessage();
                            objGetMessage.strFrom = "";
                            objGetMessage.strType = "";
                            objGetMessage.strTo = objContract.strTo;
                            objGetMessage.x = new double();
                            objGetMessage.y = new double();
                            objGetMessage.stremImage = new MemoryStream();
                            objGetMessage.mouseButton = new int();
                            objGetMessage.key = objContract.key;
                            objGetMessage.blView = new bool();
                            objGetMessage.blControl = new bool();
                            objGetMessage.id = 9;
                            objGetMessage.ViewTag = new int();
                            objGetMessage.ControlTag = new int();
                            #endregion 20oct changes
                            List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                            lstStream.Add(objGetMessage);
                            hashMessages[lstNodes[i].uName] = lstStream;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcSendKey", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcAllowView(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                    int ViewTag = 0;

                    if (objContract.blView)
                    {
                        ViewTag = 1;
                    }
                    else if (!objContract.blView)
                    {
                        ViewTag = 0;
                    }

                   #region 20oct changes
                            clsGetMessage objGetMessage = new clsGetMessage();
                            objGetMessage.strFrom = objContract.strFrom;
                            objGetMessage.strType = "";
                            objGetMessage.strTo = "";
                            objGetMessage.x = new double();
                            objGetMessage.y = new double();
                            objGetMessage.stremImage = new MemoryStream();
                            objGetMessage.mouseButton = new int();
                            objGetMessage.key = new int();
                            objGetMessage.blView = new bool();
                            objGetMessage.blControl = new bool();
                            objGetMessage.id = 10;
                            objGetMessage.ViewTag = ViewTag;
                            objGetMessage.ControlTag = new int();
                            #endregion 20oct changes               

                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];

                        if (ViewTag == 0)
                        {
                            lstStream.Clear();
                        }

                        lstStream.Add(objGetMessage);
                        hashMessages[lstNodes[i].uName] = lstStream;
                    }

                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcAllowView", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcAllowControl(clsMessageContract objContract)
        {
            try
            {
                if (lstNodes.Count > 0)
                {
                   
                    int ControlTag = 0;

                    if (objContract.blControl)
                    {
                        ControlTag = 1;
                    }
                    else if (!objContract.blControl)
                    {
                        ControlTag = 0;
                    }

                    #region 20oct changes
                    clsGetMessage objGetMessage = new clsGetMessage();
                    objGetMessage.strFrom = objContract.strFrom;
                    objGetMessage.strType = "";
                    objGetMessage.strTo = "";
                    objGetMessage.x = new double();
                    objGetMessage.y = new double();
                    objGetMessage.stremImage = new MemoryStream();
                    objGetMessage.mouseButton = new int();
                    objGetMessage.key = new int();
                    objGetMessage.blView = new bool();
                    objGetMessage.blControl = new bool();
                    objGetMessage.id = 11;
                    objGetMessage.ViewTag = new int();
                    objGetMessage.ControlTag =ControlTag ;
                    #endregion 20oct changes

                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                        if (ControlTag == 0)
                        {
                            lstStream.Clear();
                        }
                        lstStream.Add(objGetMessage);
                        hashMessages[lstNodes[i].uName] = lstStream;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcAllowControl", "DesktopDummy.cs");
            }
        }

        void DesktopDummy_EntsvcUnJoin(clsMessageContract objContract)
        {
            try
            {
                 #region 20oct changes
                clsGetMessage objGetMessage = new clsGetMessage();
                objGetMessage.strFrom = objContract.strFrom;
                objGetMessage.strType = "";
                objGetMessage.strTo = "";
                objGetMessage.x = new double();
                objGetMessage.y = new double();
                objGetMessage.stremImage = new MemoryStream();
                objGetMessage.mouseButton = new int();
                objGetMessage.key = new int();
                objGetMessage.blView = new bool();
                objGetMessage.blControl = new bool();
                objGetMessage.id = 12;
                objGetMessage.ViewTag = new int();
                objGetMessage.ControlTag = new int();
                #endregion 20oct changes

                for (int i = 0; i < lstNodes.Count; i++)
                {
                    List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[lstNodes[i].uName];
                    lstStream.Add(objGetMessage);
                    hashMessages[lstNodes[i].uName] = lstStream;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DesktopDummy_EntsvcUnJoin", "DesktopDummy.cs");
            }
        }

        #endregion

        #region HTTP Events

        void Http_EntsvcJoin(clsMessageContract streamUName)
        {
            try
            {

                string uName = streamUName.strFrom;
                
                clsMessage objMessage = new clsMessage();
                objMessage.uName = uName;
                objMessage.isControlled = false;
                objMessage.isControlling = false;
                objMessage.strControlled = "";

                lstNodes.Add(objMessage);

                hashMessages.Add(uName, new List<clsGetMessage>());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcJoin", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcGetUserList(clsMessageContract streamGetUserList)
        {
            try
            {
               
                string uName = streamGetUserList.strFrom;
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = uName;
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0.0;
                objContract.y = 0.0;
                #endregion MsgContract

                channelNettcpDesktop.svcGetUserList(objContract);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcGetUserList", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcSetUserList(clsMessageContract streamSetUserList)
        {
            try
            {
                string uName = streamSetUserList.strFrom;
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = new int();
                objContract.mouseButton = new int();
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = uName;
                objContract.strTo = "";
                objContract.strType = "Set";
                objContract.x = new double();
                objContract.y = new double();
                #endregion MsgContract

                channelNettcpDesktop.svcSetUserList(objContract);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcSetUserList", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcSendMessage(clsMessageContract streamImage)
        {
            try
            {
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = streamImage.stremImage;
                objContract.strFrom = streamImage.strFrom;
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0;
                objContract.y = 0;
                #endregion MsgContract

                channelNettcpDesktop.svcSendMessage(objContract);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcSendMessage", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcSelectedDesktop(clsMessageContract streamUName)
        {
            try
            {

                string uName = streamUName.strTo;
                string strFrom = streamUName.strFrom;             


                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].uName == strFrom)
                    {
                        lstNodes[i].isControlling = true;
                        lstNodes[i].strControlled = uName;

                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[strFrom];
                        lstStream.Clear();
                         }
                }
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();               
                objContract.strFrom = uName;               
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0;
                objContract.y = 0;
                #endregion MsgContract

                channelNettcpDesktop.svcSelectedDesktop(objContract);

                strSelected = uName;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcSelectedDesktop", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcStopControl(clsMessageContract streamUName)
        {
            try
            {   string uName = streamUName.strTo;
                string strFrom = streamUName.strFrom;              

                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].uName == strFrom)
                    {
                        lstNodes[i].isControlling = false;
                        lstNodes[i].strControlled = "";

                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[strFrom];
                       
                        lstStream.Clear();
                       
                    }
                }
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = uName;
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0;
                objContract.y = 0;
                #endregion MsgContract

                channelNettcpDesktop.svcStopControl(objContract);
                strSelected = "";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcStopControl", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcBtnUp(clsMessageContract streamButtonUp)
        {
            try
            {
                string mouseButton = string.Empty;
                string ToBtnUp = string.Empty;
                mouseButton = streamButtonUp.mouseButton.ToString();               
                ToBtnUp = streamButtonUp.strTo;
                #region recent changes
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].uName == streamButtonUp.strFrom)
                    {
                        lstNodes[i].isControlling = true;
                        lstNodes[i].strControlled = streamButtonUp.strTo;

                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[streamButtonUp.strFrom];
                        lstStream.Clear();                       
                    }
                }
                #endregion recent changes

                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = int.Parse(mouseButton);
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = "";
                objContract.strTo = ToBtnUp;
                objContract.strType = "";
                objContract.x = new double();
                objContract.y = new double();
                #endregion MsgContract

                channelNettcpDesktop.svcBtnUp(objContract);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcBtnUp", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcBtnDown(clsMessageContract streamButtonDown)
        {
            try
            {
                string mouseButton = string.Empty;
                string ToBtnDown = string.Empty;
                mouseButton = streamButtonDown.mouseButton.ToString();

                #region recent changes
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].uName == streamButtonDown.strFrom)
                    {
                        lstNodes[i].isControlling = true;
                        lstNodes[i].strControlled = streamButtonDown.strTo;

                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[streamButtonDown.strFrom];                       
                        lstStream.Clear();                       
                    }
                }
                #endregion recent changes

                ToBtnDown = streamButtonDown.strTo;
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = "";
                objContract.strTo = strTO;
                objContract.strType = "";
                objContract.x = gX;
                objContract.y = gY;
                #endregion MsgContract
                channelNettcpDesktop.svcSendXY(objContract);
                #region MsgContract
                clsMessageContract objContract1 = new clsMessageContract();
                objContract1.blControl = false;
                objContract1.blView = false;
                objContract1.key = 0;
                objContract1.mouseButton = int.Parse(mouseButton);
                objContract1.stremImage = new MemoryStream();
                objContract1.strFrom = "";
                objContract1.strTo = ToBtnDown;
                objContract1.strType = "";
                objContract1.x = 0.0;
                objContract1.y = 0.0;
                #endregion MsgContract

                channelNettcpDesktop.svcBtnDown(objContract1);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcBtnDown", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcSendXY(clsMessageContract streamXY)
        {
            try
            {
                         
                strTO = streamXY.strTo;               
                gX = streamXY.x;
                gY = streamXY.y;
                #region recent changes
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].uName == streamXY.strFrom)
                    {
                        lstNodes[i].isControlling = true;
                        lstNodes[i].strControlled = streamXY.strTo;
                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[streamXY.strFrom];                        
                        lstStream.Clear();                        
                    }
                }
                #endregion recent changes
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcSendXY", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcSendKey(clsMessageContract streamKey)
        {
            try
            {
                string valKey = string.Empty;
                string ToKey = string.Empty;
                ToKey = streamKey.strTo;
                valKey = streamKey.key.ToString();
                #region recent changes
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].uName == streamKey.strFrom)
                    {
                        lstNodes[i].isControlling = true;
                        lstNodes[i].strControlled = streamKey.strTo;
                        List<clsGetMessage> lstStream = (List<clsGetMessage>)hashMessages[streamKey.strFrom];                       
                        lstStream.Clear();                        
                    }
                }
                #endregion recent changes

                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = int.Parse(valKey);
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = "";
                objContract.strTo = ToKey;
                objContract.strType = "";
                objContract.x = 0.0;
                objContract.y = 0.0;
                #endregion MsgContract
                channelNettcpDesktop.svcSendKey(objContract);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcSendKey", "DesktopDummy.cs");
            }
        }

        clsGetMessage Http_EntsvcGetMessages(clsMessageContract streamRecipient)
        {
            try
            {
                string strRecipient = streamRecipient.strFrom;
                List<clsGetMessage> lstTemp = (List<clsGetMessage>)hashMessages[strRecipient];              

                if (lstTemp.Count > 0 && lstTemp[0] != null && lstTemp != null)                {
                  
                    clsGetMessage objGet = new clsGetMessage();
                    objGet = lstTemp[0];        
                    
                    lstTemp.RemoveAt(0);
                    hashMessages[strRecipient] = lstTemp;
                    return objGet;                  
                }
                else
                {
                    clsGetMessage objGet = new clsGetMessage();
                    objGet.blControl = new bool();
                    objGet.blView = new bool();
                    objGet.ControlTag = new int();
                    objGet.id=new int();
                    objGet.key=new int();
                    objGet.mouseButton=new int();
                    objGet.stremImage = new MemoryStream();
                    objGet.strFrom = "";
                    objGet.strTo = "";
                    objGet.strType = "";
                    objGet.ViewTag = new int();
                    objGet.x = new double();
                    objGet.y = new double();

                    return objGet;
                   
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcGetMessages", "DesktopDummy.cs");
                clsGetMessage objGet = new clsGetMessage();
                objGet.blControl = new bool();
                objGet.blView = new bool();
                objGet.ControlTag = new int();
                objGet.id = new int();
                objGet.key = new int();
                objGet.mouseButton = new int();
                objGet.stremImage = new MemoryStream();
                objGet.strFrom = "";
                objGet.strTo = "";
                objGet.strType = "";
                objGet.ViewTag = new int();
                objGet.x = new double();
                objGet.y = new double();
                return objGet;
              
            }
        }

        void Http_EntsvcAllowView(clsMessageContract streamView)
        {
            try
            {
                string ToView = streamView.strFrom;              
                bool flg = streamView.blView;              

                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = flg;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = ToView;
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0.0;
                objContract.y = 0.0;
                #endregion MsgContract
                channelNettcpDesktop.svcAllowView(objContract);
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcAllowView", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcAllowControl(clsMessageContract streamControl)
        {
            try
            {
                string ToControl = streamControl.strFrom;
                bool flg = streamControl.blControl;             
                
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = flg;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom = ToControl;
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0.0;
                objContract.y = 0.0;
                #endregion MsgContract

                channelNettcpDesktop.svcAllowControl(objContract);
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcAllowControl", "DesktopDummy.cs");
            }
        }

        void Http_EntsvcUnJoin(clsMessageContract streamUName)
        {
            try
            {
                string uName = streamUName.strFrom;
               
                #region MsgContract
                clsMessageContract objContract = new clsMessageContract();
                objContract.blControl = false;
                objContract.blView = false;
                objContract.key = 0;
                objContract.mouseButton = 0;
                objContract.stremImage = new MemoryStream();
                objContract.strFrom =uName;
                objContract.strTo = "";
                objContract.strType = "";
                objContract.x = 0.0;
                objContract.y = 0.0;
                #endregion MsgContract

                channelNettcpDesktop.svcUnJoin(objContract);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Http_EntsvcUnJoin", "DesktopDummy.cs");
            }
        }

        #endregion

        #region Supported Functions

        string fncStreamToString(Stream streamInput)
        {
            try
            {

                byte[] byteArry = fncStreamToByteArry(streamInput);

                //convert byte[] to string

                char[] buffer = new char[byteArry.Length];

                for (int j = 0; j < byteArry.Length; j++)
                {
                    buffer[j] = (char)byteArry[j];
                }

                return new string(buffer);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToString", "DesktopDummy.cs");
                return null;
            }
        }

        Stream fncStringToStream(string strInput)
        {
            try
            {
                int length = strInput.Length;
                byte[] resultBytes = new byte[length];

                for (int i = 0; i < length; i++)
                {
                    resultBytes[i] = (byte)strInput[i];
                }

                MemoryStream mmsConvert = new MemoryStream(resultBytes);

                return mmsConvert;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStringToStream", "DesktopDummy.cs");
                return null;
            }
        }

        byte[] fncStreamToByteArry(Stream streamInput)
        {
            try
            {
                List<byte> myBytes = new List<byte>();
                int num;
                while ((num = streamInput.ReadByte()) != -1)
                {
                    myBytes.Add((byte)num);
                }

                return myBytes.ToArray();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToByteArry", "DesktopDummy.cs");
                return null;
            }
        }

        #endregion

    }
}
