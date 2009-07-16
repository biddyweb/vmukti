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
using System.Linq;
using System.Text;
using AutoProgressivePhone.Business.Service.NetP2P;
using AutoProgressivePhone.Business.Service.BasicHttp;
using AutoProgressivePhone.Business.Service.DataContracts;
using VMuktiAPI;

namespace AutoProgressivePhone.Presentation
{
    [Serializable]
    public class AudioDummy
    {
        List<clsMessage> lstMessage = new List<clsMessage>();
        string UserName;
        string myMeshId;
        int MyId;
        int temp = 0;
        int tempcounter = 0;

        object objHttpAudio = null;
        object objNetTcpAudio = null;

        public VMuktiService.BasicHttpServer HttpAudioServer = null;
        public INetTcpAudioChannel NetTcpAudioChannel;

        System.Threading.Thread HttpThread = null;
        System.Threading.Thread NetTcpThread = null;

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
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--AudioDummy()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                //Console.Write(exp.Message);
            }
        }

        void RegHttpServer(object httpUri)
        {            
            try
            {
                objHttpAudio = new clsHttpAudio();
                ((clsHttpAudio)objHttpAudio).EntsvcJoin += new clsHttpAudio.DelsvcJoin(AudioDummy_EntsvcJoin);
                ((clsHttpAudio)objHttpAudio).EntsvcGetConference += new clsHttpAudio.DelsvcGetConference(AudioDummy_EntsvcGetConference);
                ((clsHttpAudio)objHttpAudio).EntsvcStartConference += new clsHttpAudio.DelsvcStartConference(AudioDummy_EntsvcStartConference);
                ((clsHttpAudio)objHttpAudio).EntsvcUnjoin += new clsHttpAudio.DelsvcUnJoin(AudioDummy_EntsvcUnjoin);
                HttpAudioServer = new VMuktiService.BasicHttpServer(ref objHttpAudio, httpUri.ToString());
                HttpAudioServer.AddEndPoint<AutoProgressivePhone.Business.Service.BasicHttp.IHttpAudio>(httpUri.ToString());
                HttpAudioServer.OpenServer();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--RegHttpServer()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                //System.Windows.MessageBox.Show("RegHttpServer" + exp.Message);
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
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--RegNetP2PClient()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                throw ex;
            }
        }

        #region Net.Tcp
        void AudioDummy_EntsvcP2PJoin(string uName)
        { }

        void AudioDummy_EntsvcP2PStartConference(string uName, string strConfNumber, string[] GuestInfo)
        {
            try
            {
                clsMessage objMessage = new clsMessage();
                objMessage.strConfNumber = strConfNumber;
                lstMessage.Add(objMessage);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--AudioDummy_EntsvcP2PStartConference()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        void AudioDummy_EntsvcP2PUnJoin(string uName)
        { }

        #endregion

        #region Http Events
        void AudioDummy_EntsvcJoin(string uName)
        { }

        void AudioDummy_EntsvcStartConference(string uName, string strConfNumber, string[] GuestName)
        {
            try
            {
                NetTcpAudioChannel.svcP2PStartConference(uName, strConfNumber, GuestName);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--AudioDummy_EntsvcStartConference()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        string AudioDummy_EntsvcGetConference(string uName)
        {
            try
            {
                return lstMessage[0].strConfNumber.ToString();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--AudioDummy_EntsvcGetConference()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                return null;
            }
        }

        void AudioDummy_EntsvcUnjoin(string uName)
        {
            try
            {
                NetTcpAudioChannel.Close();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--VmuktiModules--:--Call Center--:--AutoProgressiveSoftPhone--:--AutoProgressivePhone.Presentation--:--AudioDummy.cs--:--AudioDummy_EntsvcUnjoin()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }
        #endregion


    }
}
