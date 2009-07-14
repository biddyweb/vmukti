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
using Audio.Business.Service.BasicHttp;
using VMuktiService;
using Audio.Business.Service.NetP2P;
using Audio.Business.Service.DataContracts;
using VMuktiAPI;

namespace Audio.Presentation
{
    [Serializable]
    public class P2PAudioClient : IDisposable
    {
        object objNetTcpAudio = null;

        public INetTcpAudioChannel channelNettcpAudio;

        string UserName;

        int tempcounter = 0;              

        public P2PAudioClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegAudiop2pClient(P2PUri);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "P2PAudioClient()", "Audio\\P2PAudioClient.cs");
            }
        }

        void RegAudiop2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyAudio = new NetPeerClient();
                objNetTcpAudio = new clsNetTcpAudio();
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PJoin += new clsNetTcpAudio.DelsvcP2PJoin(P2PAudioClient_EntsvcP2PJoin);
                ((clsNetTcpAudio)objNetTcpAudio).EntsvcP2PUnJoin += new clsNetTcpAudio.DelsvcP2PUnJoin(P2PAudioClient_EntsvcP2PUnJoin);
                channelNettcpAudio = (INetTcpAudioChannel)npcDummyAudio.OpenClient<INetTcpAudioChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpAudio);

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpAudio.svcP2PJoin(UserName);
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
                VMuktiHelper.ExceptionHandler(ex, "RegAudiop2pClient()", "Audio\\P2PAudioClient.cs");
            }
        }
        #region supernode IPV6 event handlers

        void P2PAudioClient_EntsvcP2PUnJoin(string uName)
        {
            try
            {
                if (uName == this.UserName)
                {
                    channelNettcpAudio.Close();
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "P2PAudioClient_EntsvcP2PUnJoin()", "Audio\\P2PAudioClient.cs");
            }            
        }        
        void P2PAudioClient_EntsvcP2PJoin(string uName)
        {            
        }
        #endregion

          #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {

            try
            {
                objNetTcpAudio = null;
                channelNettcpAudio = null;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Audio\\P2PAudioClient.cs");
            }
        }

        ~P2PAudioClient()
        {
            Dispose(false);
        }

        #endregion

    }
}
