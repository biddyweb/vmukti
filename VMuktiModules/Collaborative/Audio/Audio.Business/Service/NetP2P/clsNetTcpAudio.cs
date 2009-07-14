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
using System.Text;
using System;
namespace Audio.Business.Service.NetP2P
{
    public class clsNetTcpAudio : INetTcpAudio
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

        public delegate void DelsvcP2PJoin(string uName);
        public delegate void DelsvcP2PStartConference(string uName, string strConfNumber, string[] GuestInfo);
        public delegate void DelsvcGetUserList(string uName, string strConf);
        public delegate void DelsvcSetUserList(string uName, string strConf);
        public delegate void DelsvcP2PUnJoin(string uName);

        public event DelsvcP2PJoin EntsvcP2PJoin;
        public event DelsvcP2PStartConference EntsvcP2PStartConference;
        public event DelsvcGetUserList EntsvcGetUserList;
        public event DelsvcSetUserList EntsvcSetUserList;
        public event DelsvcP2PUnJoin EntsvcP2PUnJoin;

        #region INetTcpAudio Members

        public void svcP2PJoin(string uName)
        {
            try
            {
                if (EntsvcP2PJoin != null)
                {
                    EntsvcP2PJoin(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcP2PJoin()", "Audio\\Audio.Business\\Service\\NetP2P\\clsNetTcpAudio.cs");
            }
        }

        public void svcP2PStartConference(string uName, string strConfNumber, string[] GuestInformation)
        {
            try
            {
                if (EntsvcP2PStartConference != null)
                {
                    EntsvcP2PStartConference(uName, strConfNumber, GuestInformation);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcP@PStartConference()", "Audio\\Audio.Business\\Service\\NetP2P\\clsNetTcpAudio.cs");
            }
        }

        public void svcGetUserList(string uName, string strConf)
        {
            try
            {
                if (EntsvcGetUserList != null)
                {
                    EntsvcGetUserList(uName, strConf);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcGetUserList()", "Audio\\Audio.Business\\Service\\NetP2P\\clsNetTcpAudio.cs");
            }
        }

        public void svcSetUserList(string uName, string strConf)
        {
            try
            {
                if (EntsvcSetUserList != null)
                {
                    EntsvcSetUserList(uName, strConf);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetUserList()", "Audio\\Audio.Business\\Service\\NetP2P\\clsNetTcpAudio.cs");
            }
        }
        public void svcP2PUnJoin(string uName)
        {
            try
            {
                if (EntsvcP2PUnJoin != null)
                {
                    EntsvcP2PUnJoin(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcP2PUnJoin()", "Audio\\Audio.Business\\Service\\NetP2P\\clsNetTcpAudio.cs");
            }
        }
        #endregion
    }
}
