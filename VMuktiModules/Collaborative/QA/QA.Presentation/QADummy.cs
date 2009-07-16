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
using QA.Business.Service.BasicHttp;
using QA.Business.Service.DataContracts;
using QA.Business.Service.NetP2P;
using VMuktiService;

namespace QA.Presentation
{
    [Serializable]
    public class QADummy
    {
        int MyId;
        int count = 0;
        int tempcounter = 0;
        string UserName;
        string MyMeshID;

        object objHttpQA = null;
        object objNetTcpQA = null;

        List<clsMessage> lstMessage = new List<clsMessage>();
        System.Threading.Thread HttpThread = null;
        System.Threading.Thread NetP2PThread = null;

        public INetTcpQAChannel NetP2PChannel;
        public VMuktiService.BasicHttpServer HttpQAServer = null;
         
        public QADummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                MyId = Id;
                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);

            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        void RegHttpServer(object httpUri)
        {
            try
            {
                objHttpQA = new clsHttpQA();
                ((clsHttpQA)objHttpQA).EntsvcHttpJoin += new clsHttpQA.DelsvcHttpJoin(QADummy_EntsvcHttpJoin);
                ((clsHttpQA)objHttpQA).EntsvcHttpAskQuestion += new clsHttpQA.DelsvcHttpAskQuestion(QADummy_EntsvcHttpAskQuestion);
                ((clsHttpQA)objHttpQA).EntsvcHttpReplyQuestion += new clsHttpQA.DelsvcHttpReplyQuestion(QADummy_EntsvcHttpReplyQuestion);
                ((clsHttpQA)objHttpQA).EntsvcHttpGetMessage += new clsHttpQA.DelsvcHttpGetMessage(QADummy_EntsvcHttpGetMessage);
                HttpQAServer = new VMuktiService.BasicHttpServer(ref objHttpQA, httpUri.ToString());
                HttpQAServer.AddEndPoint<QA.Business.Service.BasicHttp.IHttpQA>(httpUri.ToString());
                HttpQAServer.OpenServer();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("RegHttpServer" + ex.Message);
            }
        }

       

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyQA = new NetPeerClient();
                objNetTcpQA = new clsNetTcpQA();
                ((clsNetTcpQA)objNetTcpQA).EntsvcP2PJoin += new clsNetTcpQA.DelsvcP2Pjoin(QADummy_EntsvcP2PJoin);
                ((clsNetTcpQA)objNetTcpQA).entsvcP2PReplyQuestion += new clsNetTcpQA.DelsvcP2PReplyQuestion(QADummy_entsvcP2PReplyQuestion);
                ((clsNetTcpQA)objNetTcpQA).EntsvcP2PAskQuestion += new clsNetTcpQA.DelsvcP2PAskQuestion(QADummy_EntsvcP2PAskQuestion);
                NetP2PChannel = (INetTcpQAChannel)npcDummyQA.OpenClient<INetTcpQAChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[1], ref objNetTcpQA);

                while (tempcounter < 20)
                {
                    try
                    {
                        NetP2PChannel.svcP2PJoin(UserName);
                        tempcounter = 20;
                    }
                    catch (Exception ex)
                    {
                        tempcounter++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("RegNetP2PClient" + ex.Message);
            }
        }

        #region Http Functions
        void QADummy_EntsvcHttpJoin(string uName)
        {
        }

        void QADummy_EntsvcHttpAskQuestion(string uName, string Question, string Role)
        {
            NetP2PChannel.svcP2PAskQuestion(uName, Question, Role);
        }

        void QADummy_EntsvcHttpReplyQuestion(string uName, string Question, string Answer, string Role,List<string> strBuddyList)
        {
            NetP2PChannel.svcP2PReplyQuestion(uName, Question, Answer, Role,strBuddyList);
        }

        List<clsMessage> QADummy_EntsvcHttpGetMessage(string recepient)
        {
            lock (this)
            {
                List<clsMessage> myMessages = new List<clsMessage>();
                //int count = lstMessage.Count;

                for (int i = 0; i < lstMessage.Count; i++)
                {
                    if (lstMessage[i].strGuestList != null)
                    {
                        for (int j = 0; j < lstMessage[i].strGuestList.Count; j++)
                        {
                            if (lstMessage[i].strGuestList[j] == recepient)
                            {

                                myMessages.Add(lstMessage[i]);
                                lstMessage[i].strGuestList.RemoveAt(j);
                            }
                        }
                        if (lstMessage[i].strGuestList.Count == 0)
                        {
                            lstMessage.RemoveAt(i);
                        }

                    }
                    //lstMessage.RemoveAt(i);
                    
                }
                //lstMessage.Clear();
                return myMessages;
            }
        }

        #endregion

        #region Net Tcp Functions

        void QADummy_EntsvcP2PJoin(string uName)
        {
        }

        void QADummy_EntsvcP2PAskQuestion(string uName, string Question, string Role)
        {
            clsMessage objMessage = new clsMessage();
            objMessage.struName = uName;
            objMessage.strQuestion = Question;
            objMessage.strRole = Role;
            lstMessage.Add(objMessage);
        }

        void QADummy_entsvcP2PReplyQuestion(string uName, string Question, string Answer, string Role,List<string> strBuddyList)
        {
            clsMessage objMessage = new clsMessage();
            objMessage.struName = uName;
            objMessage.strQuestion = Question;
            objMessage.strAnswer = Answer;
            objMessage.strRole = Role;
            objMessage.strGuestList = strBuddyList;
            lstMessage.Add(objMessage);
        }
        #endregion


    }
}
