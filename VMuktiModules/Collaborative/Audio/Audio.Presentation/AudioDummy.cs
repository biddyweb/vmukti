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
using Audio.Business.Service.BasicHttp;
using Audio.Business.Service.DataContracts;
using Audio.Business.Service.NetP2P;
using System.Text;

namespace Audio.Presentation
{
    [Serializable]
    public class AudioDummy : IDisposable
    {
        
        List<string> lstLocalBuddyList = new List<string>();
        List<clsMessage> lstMessage = new List<clsMessage>();
        string UserName;
        string myMeshId;
        int MyId;
        int temp = 0;
        int tempcounter;

        object objHttpAudio;
        object objNetTcpAudio;

        public VMuktiService.BasicHttpServer HttpAudioServer;
        public INetTcpAudioChannel NetTcpAudioChannel;

        System.Threading.Thread HttpThread = null;
        System.Threading.Thread NetTcpThread = null;

        List<string> lstNodes = new List<string>();
        List<string> lstNodesToRemove4GetUserList = new List<string>();
        List<string> lstNodesToRemove4SetUserList = new List<string>();
        

        public AudioDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                MyId = Id;

                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy()", "AudioDummy.cs");
            }
        }
        void RegHttpServer(object httpUri)
        {
            //lock (this)
            //{
            try
            {
                objHttpAudio = new clsHttpAudio();
                ((clsHttpAudio)objHttpAudio).EntsvcJoin += new clsHttpAudio.DelsvcJoin(AudioDummy_EntsvcJoin);
                ((clsHttpAudio)objHttpAudio).EntsvcGetConference += new clsHttpAudio.DelsvcGetConference(AudioDummy_EntsvcGetConference);
                ((clsHttpAudio)objHttpAudio).EntsvcStartConference += new clsHttpAudio.DelsvcStartConference(AudioDummy_EntsvcStartConference);
                ((clsHttpAudio)objHttpAudio).EntsvcSetUserList += new clsHttpAudio.delsvcSetUserList(AudioDummy_EntsvcSetUserList);
                ((clsHttpAudio)objHttpAudio).EntsvcGetUserList += new clsHttpAudio.delsvcGetUserList(AudioDummy_EntsvcGetUserList);
                ((clsHttpAudio)objHttpAudio).EntsvcSignOutAudio += new clsHttpAudio.delsvcSignOutAudio(AudioDummy_EntsvcSignOutAudio);
                ((clsHttpAudio)objHttpAudio).EntsvcUnjoin += new clsHttpAudio.DelsvcUnJoin(AudioDummy_EntsvcUnjoin);
                HttpAudioServer = new VMuktiService.BasicHttpServer(ref objHttpAudio, httpUri.ToString());
                HttpAudioServer.AddEndPoint<Audio.Business.Service.BasicHttp.IHttpAudio>(httpUri.ToString());
                HttpAudioServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegHttpServer()", "AudioDummy.cs");
            }
        }

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                VMuktiService.NetPeerClient npcDummyChat = new VMuktiService.NetPeerClient();
                objNetTcpAudio = new clsNetTcpAudio();
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PJoin += new clsNetTcpAudio.DelsvcP2PJoin(AudioDummy_EntsvcP2PJoin);
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PStartConference += new clsNetTcpAudio.DelsvcP2PStartConference(AudioDummy_EntsvcP2PStartConference);
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcGetUserList += new clsNetTcpAudio.DelsvcGetUserList(AudioDummy_EntsvcP2PGetUserList);
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcSetUserList += new clsNetTcpAudio.DelsvcSetUserList(AudioDummy_EntsvcP2PSetUserList);
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PUnJoin += new clsNetTcpAudio.DelsvcP2PUnJoin(AudioDummy_EntsvcP2PUnJoin);
                NetTcpAudioChannel = (INetTcpAudioChannel)npcDummyChat.OpenClient<INetTcpAudioChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpAudio);

                while (tempcounter < 20)
                {
                    try
                    {
                        NetTcpAudioChannel.svcP2PJoin(UserName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient()", "AudioDummy.cs");
            }
        }

        #region Net.Tcp
        void AudioDummy_EntsvcP2PJoin(string uName)
        { }
        void AudioDummy_EntsvcP2PStartConference(string uName, string strConfNumber, string[] GuestInfo)
        {
            try
            {
                if (strConfNumber != "" && GuestInfo.Length > 0)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < GuestInfo.Length; i++)
                    {
                        temp.Add(GuestInfo[i]);
                    }
                    clsMessage objMessage = new clsMessage();
                    objMessage.strConfNumber = strConfNumber;
                    objMessage.strUserName = uName;
                    objMessage.lstTo = temp;
                    objMessage.msgType = "conf";
                    lstMessage.Add(objMessage);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcP2PStartConference()", "AudioDummy.cs");
            }
        }

        void AudioDummy_EntsvcP2PGetUserList(string uname, string strConf)
        {
            try
            {
                clsMessage objMessege = new clsMessage();
                objMessege.strUserName = uname;
                objMessege.strConfNumber = strConf;
                objMessege.msgType = "getuserlist";
                lstMessage.Add(objMessege);

            }
            catch
            {
            }
        }

        void AudioDummy_EntsvcP2PSetUserList(string uname, string strConf)

        {
            clsMessage objMessage = new clsMessage();
            objMessage.strUserName = uname;
            objMessage.strConfNumber = strConf;
            objMessage.msgType = "setuserlist";
            lstMessage.Add(objMessage);
        }
        
        void AudioDummy_EntsvcP2PUnJoin(string uName)
        {
            try
            {
                lstNodes.Remove(uName);
                clsMessage objMessege = new clsMessage();
                objMessege.strUserName = uName;
                objMessege.msgType = "unjoin";

                lstMessage.Add(objMessege);
                //NetTcpAudioChannel.Close();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcP2PUnJoin()", "AudioDummy.cs");
            }
        }
        #endregion

        #region Http Events
        void AudioDummy_EntsvcJoin(string uName)
        {
            try
            {
                lstNodes.Add(uName);
                NetTcpAudioChannel.svcP2PJoin(uName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcJoin()", "AudioDummy.cs");
            }
        }
        void AudioDummy_EntsvcStartConference(string uName, string strConfNumber, string[] GuestName)
        {
            try
            {
                NetTcpAudioChannel.svcP2PStartConference(uName, strConfNumber, GuestName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_entsvcStartConference()", "AudioDummy.cs");
            }
        }
        List<clsMessage> AudioDummy_EntsvcGetConference(string uName)
        {
            try
            {
                List<clsMessage> myMessages = new List<clsMessage>();
                List<int> lstMsgToRemove = new List<int>();

                for (int i = 0; i < lstMessage.Count; i++)
                {
                    if (lstMessage[i].msgType == "getuserlist" && lstMessage[i].strUserName != uName)
                    {
                        for (int intNodeCnt = 0; intNodeCnt < lstNodes.Count; intNodeCnt++)
                        {
                            if (uName == lstNodes[intNodeCnt])
                            {
                                myMessages.Add(lstMessage[i]);
                                lstNodesToRemove4GetUserList.Add(uName);
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
                    else if (lstMessage[i].msgType == "setuserlist")
                    {
                        for (int intNodeCnt = 0; intNodeCnt < lstNodes.Count; intNodeCnt++)
                        {
                            //if (recipient != lstNodes[intNodeCnt])
                            //{
                            myMessages.Add(lstMessage[i]);
                            if (!lstNodesToRemove4SetUserList.Contains(uName))
                            {
                                lstNodesToRemove4SetUserList.Add(uName);
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
                        }
                    }
                    else if (lstMessage[i].msgType == "conf")
                    {
                        for (int j = 0; j < lstMessage[i].lstTo.Count; j++)
                        {
                            if (lstMessage[i].lstTo[j] == uName)
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
                    else if (lstMessage[i].msgType == "unjoin")
                    {
                        myMessages.Add(lstMessage[i]);

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcGetConference()", "AudioDummy.cs");
                return null;
            }
        }
        void AudioDummy_EntsvcSignOutAudio(string from, List<string> to)
        {
            try
            {
                VMuktiAPI.ClsException.WriteToLogFile("SignOut of AUDIO");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcSignOutAudio()", "AudioDummy.cs");
            }
        }

        void AudioDummy_EntsvcGetUserList(string uname, string strConf)
        {
            try
            {
                NetTcpAudioChannel.svcGetUserList(uname, strConf);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcGetUserList()", "AudioDummy.cs");
            }
        }

        void AudioDummy_EntsvcSetUserList(string uname, string strConf)
        {
            try
            {
                NetTcpAudioChannel.svcSetUserList(uname, strConf);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcSetUserList()", "AudioDummy.cs");
            }
        }

        void AudioDummy_EntsvcUnjoin(string uName)
        {
            try
            {
                NetTcpAudioChannel.svcP2PUnJoin(uName);
                NetTcpAudioChannel.Close();
                HttpAudioServer.CloseServer();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AudioDummy_EntsvcUnJoin()", "AudioDummy.cs");
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        //void CurrentDomain_DomainUnload(object sender, EventArgs e)
        //{
            
        //}
        #endregion

          #region IDisposable Members
       // private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {

            try
            {
                lstMessage = null;
                UserName = null; ;
                myMeshId = null; ;
                objHttpAudio = null;
                objNetTcpAudio = null;

                HttpAudioServer = null;
                NetTcpAudioChannel = null; ;

                HttpThread = null;
                NetTcpThread = null;
                VMuktiAPI.ClsException.WriteToLogFile("Audio Dummy is Disposed");

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "AudioDummy.cs");
            }
        }

        ~AudioDummy()
        {
            Dispose(false);
        }

        #endregion

    }
}
