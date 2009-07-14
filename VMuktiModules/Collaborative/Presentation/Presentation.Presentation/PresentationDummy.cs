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
using Presentation.Bal;
using System.IO;
using System.Windows;
using VMuktiAPI;
using Presentation.Bal.Service.MessageContract;
using System.ServiceModel;
using System.Collections;


namespace Presentation.Control
{
    [Serializable]
    public class PresentationDummy : IDisposable
    {
        #region Variables Declarations

        object objHttpPresentation;
        object objNetTcpPresentation;

        Hashtable hashMessages = new Hashtable();
        //Hashtable hashSpecialMessage = new Hashtable();

        public INetTcpPresentationChannel channelNettcpPresentation;

        public VMuktiService.BasicHttpServer HttpPresentationServer;

        //List<clsMessageContract> lstMessage = new List<clsMessageContract>();
        List<string> lstNodes = new List<string>();
       // List<string> lstNodesToRemove4GetUserList = new List<string>();
       // List<string> lstNodesToRemove4SetUserList = new List<string>();        

        string UserName;
        //int MyId;
        int tempcounter;

        #endregion

        #region Constructor

        public PresentationDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy()", "PresentationDummy.cs");
            }
        }

        #endregion

        #region Http Presentation

        void RegHttpServer(object httpUri)
        {
            try
            {
                objHttpPresentation = new clsHttpPresentation();
                ((clsHttpPresentation)objHttpPresentation).EntsvcJoin += new clsHttpPresentation.delsvcJoin(PresentationDummy_EntsvcJoin);
                ((clsHttpPresentation)objHttpPresentation).EntsvcSetSlide += new clsHttpPresentation.delsvcSetSlide(PresentationDummy_EntsvcSetSlide);
                ((clsHttpPresentation)objHttpPresentation).EntsvcGetSlide += new clsHttpPresentation.delsvcGetSlide(PresentationDummy_EntsvcGetSlide);
                ((clsHttpPresentation)objHttpPresentation).EntsvcUnJoin += new clsHttpPresentation.delsvcUnJoin(PresentationDummy_EntsvcUnJoin);
                ((clsHttpPresentation)objHttpPresentation).EntsvcSetSlideList += new clsHttpPresentation.delsvcSetSlideList(PresentationDummy_EntsvcSetSlideList);
                ((clsHttpPresentation)objHttpPresentation).EntsvcSignOutPPT += new clsHttpPresentation.delsvcSignOutPPT(PresentationDummy_EntsvcSignOutPPT);
                ((clsHttpPresentation)objHttpPresentation).EntsvcGetUserList += new clsHttpPresentation.delsvcGetUserList(PresentationDummy_EntsvcGetUserList);
                ((clsHttpPresentation)objHttpPresentation).EntsvcSetUserList += new clsHttpPresentation.delsvcSetUserList(PresentationDummy_EntsvcSetUserList);

                HttpPresentationServer = new BasicHttpServer(ref objHttpPresentation, httpUri.ToString());
                HttpPresentationServer.AddEndPoint<IHttpPresentation>(httpUri.ToString());
                HttpPresentationServer.objBasicHttpBinding.TransferMode = TransferMode.Streamed;
                HttpPresentationServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer()", "PresentationDummy.cs");
            }
        }

        void PresentationDummy_EntsvcSetUserList(clsMessageContract mcSetUserList)
        {
            try
            {
                channelNettcpPresentation.svcSetUserList(mcSetUserList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcSetUserList()", "PresentationDummy.cs");
            }
        }

        void PresentationDummy_EntsvcGetUserList(clsMessageContract mcGetUserList)
        {
            try
            {
                channelNettcpPresentation.svcGetUserList(mcGetUserList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcGetUserList()", "PresentationDummy.cs");
            }
        }

        void PresentationDummy_EntsvcSignOutPPT(clsMessageContract mcSignOutPPT)
        {
            try
            {
                channelNettcpPresentation.svcSignOutPPT(mcSignOutPPT);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcSignOutPPT()", "PresentationDummy.cs");
            }
        }

        void PresentationDummy_EntsvcSetSlideList(clsMessageContract mcSetSlideList)
        {
            try
            {
                channelNettcpPresentation.svcSetSlideList(mcSetSlideList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcSetSlideList()", "PresentationDummy.cs");
            }
        }

        void PresentationDummy_EntsvcUnJoin(clsMessageContract mcUnJoin)
        {

            try
            {
                channelNettcpPresentation.svcUnJoin(mcUnJoin);
                channelNettcpPresentation.Close();
                HttpPresentationServer.CloseServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcUnJoin()/1", "PresentationDummy.cs");
            }
            try
            {
                AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                AppDomain.Unload(AppDomain.CurrentDomain);
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcUnJoin()/2", "PresentationDummy.cs");
            }

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {            
        }
        
        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {           
        }

        clsMessageContract PresentationDummy_EntsvcGetSlide(clsMessageContract mcrecipient)
        {
            try
            {
                List<clsMessageContract> lstTemp = (List<clsMessageContract>)hashMessages[mcrecipient.strFrom];

                if (lstTemp.Count > 0 && lstTemp != null)
                {
                    clsMessageContract objContract = new clsMessageContract();

                    objContract = lstTemp[0];
                    lstTemp.RemoveAt(0);

                    hashMessages[mcrecipient.strFrom] = lstTemp;

                    return objContract;
                }
                else if (lstTemp.Count == 0)
                {

                    List<clsMessageContract> lstSlide = (List<clsMessageContract>)hashMessages[mcrecipient.strFrom];

                    if (lstSlide.Count > 0 && lstSlide != null)
                    {
                        clsMessageContract objContractSlide = new clsMessageContract();

                        objContractSlide = lstSlide[0];
                        lstSlide.RemoveAt(0);

                        hashMessages[mcrecipient.strFrom] = lstSlide;

                        return objContractSlide;
                    }
                    else
                    {
                        clsMessageContract objContract1 = new clsMessageContract();
                        objContract1.lstTo = new List<string>();
                        objContract1.slideArr = new string[0];
                        objContract1.SlideID = 0;
                        objContract1.strFrom = "";
                        objContract1.strMsg = "";
                        objContract1.SlideStream = new MemoryStream();
                        return objContract1;
                    }
                }
                else
                {
                    clsMessageContract objContract1 = new clsMessageContract();
                    objContract1.lstTo = new List<string>();
                    objContract1.slideArr = new string[0];
                    objContract1.SlideID = 0;
                    objContract1.strFrom = "";
                    objContract1.strMsg = "";                    
                    objContract1.SlideStream = new MemoryStream();
                    return objContract1;
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcGetSlide()", "PresentationDummy.cs");

                clsMessageContract objContractNull = new clsMessageContract();
                objContractNull.lstTo = new List<string>();
                objContractNull.slideArr = new string[0];
                objContractNull.SlideID = 0;
                objContractNull.strFrom = "";
                objContractNull.strMsg = "";
                objContractNull.SlideStream = new MemoryStream();
                return objContractNull;
            }
        }
        
        void PresentationDummy_EntsvcSetSlide(clsMessageContract mcSetSlide)
        {
            try
            {
                channelNettcpPresentation.svcSetSlide(mcSetSlide);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcSetSlide()", "PresentationDummy.cs");
            }
        }

        void PresentationDummy_EntsvcJoin(clsMessageContract mcJoin)
        {
            try
            {
                lstNodes.Add(mcJoin.strFrom);
                hashMessages.Add(mcJoin.strFrom, new List<clsMessageContract>());
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PresentationDummy_EntsvcJoin()", "PresentationDummy.cs");
            }
        }
        #endregion

        #region P2P Presentation

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyPresentation = new NetPeerClient();
                objNetTcpPresentation = new clsNetTcpPresentation();
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcJoin += new clsNetTcpPresentation.delsvcJoin(P2PPresentationDummy_EntsvcJoin);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetSlide += new clsNetTcpPresentation.delsvcSetSlide(P2PPresentationDummy_EntsvcSetSlide);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcUnJoin += new clsNetTcpPresentation.delsvcUnJoin(P2PPresentationDummy_EntsvcUnJoin);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetSlideList += new clsNetTcpPresentation.delsvcSetSlideList(P2PPresentationDummy_EntsvcSetSlideList);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSignOutPPT += new clsNetTcpPresentation.delsvcSignOutPPT(P2PPresentationDummy_EntsvcSignOutPPT);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcGetUserList += new clsNetTcpPresentation.delsvcGetUserList(P2PPresentationDummy_EntsvcGetUserList);
                ((clsNetTcpPresentation)objNetTcpPresentation).EntsvcSetUserList += new clsNetTcpPresentation.delsvcSetUserList(P2PPresentationDummy_EntsvcSetUserList);

                channelNettcpPresentation = (INetTcpPresentationChannel)npcDummyPresentation.OpenClient<INetTcpPresentationChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpPresentation);

                while (tempcounter < 20)
                {
                    try
                    {
                        clsMessageContract objContract = new clsMessageContract();
                        objContract.strFrom = UserName;
                        objContract.strMsg = "";
                        objContract.lstTo = new List<string>();
                        objContract.slideArr = new string[0];
                        objContract.SlideID = 0;
                        objContract.SlideStream = new MemoryStream();
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient()", "PresentationDummy.cs");
            }
        }

        void P2PPresentationDummy_EntsvcSetSlideList(clsMessageContract mcSetSlideList)
        {
            try
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i] != mcSetSlideList.strFrom)
                    {                        
                        List<clsMessageContract> lstMessage = (List<clsMessageContract>)hashMessages[lstNodes[i]];
                        lstMessage.Add(mcSetSlideList);
                        hashMessages[lstNodes[i]] = lstMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationDummy_EntsvcSetSlideList()", "PresentationDummy.cs");
            }
        }

        void P2PPresentationDummy_EntsvcSetSlide(clsMessageContract mcSetSlide)
        {
            try
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i] != mcSetSlide.strFrom)
                    {
                        mcSetSlide.strMsg = "SHOW SLIDE";
                        List<clsMessageContract> lstMessage = (List<clsMessageContract>)hashMessages[lstNodes[i]];
                        lstMessage.Add(mcSetSlide);
                        hashMessages[lstNodes[i]] = lstMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationDummy_EntsvcSetSlide()", "PresentationDummy.cs");
            }
        }

        void P2PPresentationDummy_EntsvcJoin(clsMessageContract mcJoin)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationDummy_EntsvcJoin()", "PresentationDummy.cs");
            }
        }

        void P2PPresentationDummy_EntsvcUnJoin(clsMessageContract mcUnJoin)
        {
            try
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    if (mcUnJoin.strFrom != lstNodes[i])
                    {
                        List<clsMessageContract> lstMessage = (List<clsMessageContract>)hashMessages[lstNodes[i]];
                        lstMessage.Add(mcUnJoin);
                        hashMessages[lstNodes[i]] = lstMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationDummy_EntsvcUnJoin()", "PresentationDummy.cs");
            }
        }

        void P2PPresentationDummy_EntsvcSignOutPPT(clsMessageContract mcSignOutPPT)
        {
            try
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    mcSignOutPPT.strMsg = "SignOut";
                    List<clsMessageContract> lstMessage = (List<clsMessageContract>)hashMessages[lstNodes[i]];
                    lstMessage.Add(mcSignOutPPT);
                    hashMessages[lstNodes[i]] = lstMessage;
                }    
            }
            catch (Exception ex)
            {
                ClsException.WriteToLogFile("P2PPresentationDummy_EntsvcSignOutPPT() :- PresentationDummy.cs :_ +" + ex.Message);
            }
        }

        void P2PPresentationDummy_EntsvcGetUserList(clsMessageContract mcGetUserList)
        {
            try
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    mcGetUserList.strMsg = "GetUserList";
                    List<clsMessageContract> lstMessage = (List<clsMessageContract>)hashMessages[lstNodes[i]];
                    lstMessage.Add(mcGetUserList);
                    hashMessages[lstNodes[i]] = lstMessage;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationDummy_EntsvcGetUserList()", "PresentationDummy.cs");
            }
        }

        void P2PPresentationDummy_EntsvcSetUserList(clsMessageContract mcSetUserList)
        {
            try
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    mcSetUserList.strMsg = "SetUserList";
                    List<clsMessageContract> lstMessage = (List<clsMessageContract>)hashMessages[lstNodes[i]];
                    lstMessage.Add(mcSetUserList);
                    hashMessages[lstNodes[i]] = lstMessage;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "P2PPresentationDummy_EntsvcSetUserList()", "PresentationDummy.cs");
            }
        }

        #endregion

        #region User Defined Methods

        private byte[] ConvertStreamToByteBuffer(Stream stream)
        {
            try
            {
                int num;
                MemoryStream tempStream = new MemoryStream();

                while ((num = stream.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)num));
                }

                return tempStream.ToArray();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ConvertStreamToByteBuffer()", "PresentationDummy.cs");
                return null;
            }
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
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "PresentationDummy.cs");
               
            }
        }
        private void Dispose(bool disposing)
        {

            try
            {
                objHttpPresentation = null;
                objNetTcpPresentation = null;
                channelNettcpPresentation = null;
                HttpPresentationServer = null;
                //lstMessage = null;
                lstNodes = null;
                //lstNodesToRemove4GetUserList = null;
                //lstNodesToRemove4SetUserList = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "PresentationDummy.cs");
            }
        }

        ~PresentationDummy()
        {
            try
            {
            Dispose(false);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Destructor/PresentationDummy()", "PresentationDummy.cs");
            }
        }

        #endregion
    }
}
