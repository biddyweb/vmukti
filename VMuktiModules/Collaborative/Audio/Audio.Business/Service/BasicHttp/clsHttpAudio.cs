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
using System.Collections.Generic;
using System.ServiceModel;
using Audio.Business.Service.DataContracts;
using VMuktiAPI;
using System;
using System.Text;

namespace Audio.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class clsHttpAudio:IHttpAudio
    {
        //public static StringBuilder sb1;
        public delegate void DelsvcJoin(string uName);
        public delegate void DelsvcStartConference(string uName, string strConfNumber, string[] GuestName);
        public delegate List<clsMessage> DelsvcGetConference(string uName);
        public delegate void delsvcSetUserList(string uname, string strConf);
        public delegate void delsvcGetUserList(string uname, string strConf);
        public delegate void delsvcSignOutAudio(string from, List<string> to);
        public delegate void DelsvcUnJoin(string uName);

        public delegate IAsyncResult delBeginsvcGetConference(string recipient, AsyncCallback callback, object asyncState);
        public delegate List<clsMessage> delEndsvcGetConference(IAsyncResult result);

        public event DelsvcJoin EntsvcJoin;
        public event DelsvcStartConference EntsvcStartConference;
        public event DelsvcGetConference EntsvcGetConference;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSignOutAudio EntsvcSignOutAudio;
        public event DelsvcUnJoin EntsvcUnjoin;

        public event delBeginsvcGetConference EntBeginsvcGetConference;
        public event delEndsvcGetConference EntEndsvcGetConference;

        #region IHttpAudio Members

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

        public void svcJoin(string uName)
        {
            try
            {
                if (EntsvcJoin != null)
                {
                    EntsvcJoin(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcJoin", "clsHttpAudio.cs");
            }
        }

        public void svcStartConference(string uName, string strConfNumber, string[] GuestName)
        {
            try
            {
            if (EntsvcStartConference != null)
            {
                EntsvcStartConference(uName, strConfNumber, GuestName);
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcStartConference", "clsHttpAudio.cs");
            }
        }

        public List<clsMessage> svcGetConference(string uName)
        {
            try
            {
            if (EntsvcGetConference != null)
            {
                return EntsvcGetConference(uName);
            }
            else
            {
                return null;
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcGetConference", "clsHttpAudio.cs");
                return null;
            }
        }

        public void svcSetUserList(string uname, string strConf)
        {
            try
            {
                if (EntsvcSetUserList != null)
                {
                    EntsvcSetUserList(uname,strConf);
                }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "svcSetUserList", "clsHttpAudio.cs");
            }
        }

        public void svcGetUserList(string uname, string strConf)
        {
            try
            {
                if (EntsvcGetUserList != null)
                {
                    EntsvcGetUserList(uname,strConf);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcGetUserList", "clsHttpAudio.cs");
            }
        }

        public void svcSignOutAudio(string from, List<string> to)
        {
            try
            {
                if (EntsvcSignOutAudio != null)
                {
                    EntsvcSignOutAudio(from, to);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcSignOutAudio", "clsHttpAudio.cs");
            }
        }

        public void svcUnJoin(string uName)
        {
            try
            {
            if (EntsvcUnjoin != null)
            {
                EntsvcUnjoin(uName);
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "svcUnJoin", "clsHttpAudio.cs");
            }
        }


        #endregion

        #region IHttpAudio Members


        public IAsyncResult BeginsvcGetConference(string uName, AsyncCallback callback, object asyncState)
        {
            try
            {
            if (EntBeginsvcGetConference != null)
            {
                return EntBeginsvcGetConference(uName, callback, asyncState);
            }
            else
            { return null; }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BeginsvcGetConference", "clsHttpAudio.cs");
                return null;
            }
        }
        public List<clsMessage> EndsvcGetConference(IAsyncResult result)
        {
            try
            {
            if (EntEndsvcGetConference != null)
            {
                return EntEndsvcGetConference(result);
            }
            else
            { return null; }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "EndsvcGetConference", "clsHttpAudio.cs");
                return null;
            }
        }

        #endregion
    }
}
