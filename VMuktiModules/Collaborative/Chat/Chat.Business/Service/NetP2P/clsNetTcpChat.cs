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
using VMuktiAPI;

namespace Chat.Business.Service.NetP2P
{
    public class clsNetTcpChat :INetTcpChat
    {
        public delegate void delsvcJoin(string uName);
        public delegate void delsvcSendMessage(string msg, string from, List<string> to);        
        public delegate void delsvcGetUserList(string uName);
        public delegate void delsvcSetUserList(string uName);
        public delegate void delsvcSignOutChat(string from, List<string> to);
        public delegate void delsvcUnJoin(string uName);
        public delegate void delsvcShowStatus(string uname, List<string> to, string keydownstatus);

        public event delsvcJoin EntsvcJoin;
        public event delsvcSendMessage EntsvcSendMessage;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcSignOutChat EntsvcSignOutChat;
        public event delsvcUnJoin EntsvcUnJoin;
        public event delsvcShowStatus EntsvcShowStatus;

        #region INetTcpChat Members

        public void svcJoin(string uname)
		{
			try
			{
				if (EntsvcJoin != null)
				{
					EntsvcJoin(uname);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcJoin", "clsNetTcpChat.cs");
			}
        }

        public void svcSendMessage(string msg, string from, List<string> to)
		{
			try
			{
				if (EntsvcSendMessage != null)
				{
					EntsvcSendMessage(msg, from, to);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSendMessage", "clsNetTcpChat.cs");
			}
        }

        public void svcGetUserList(string uName)
		{
			try
			{
				if (EntsvcGetUserList != null)
				{
					EntsvcGetUserList(uName);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcGetUserList", "clsNetTcpChat.cs");
			}
        }

        public void svcSetUserList(string uName)
		{
			try
			{
				if (EntsvcSetUserList != null)
				{
					EntsvcSetUserList(uName);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSetUSerList", "clsNetTcpChat.cs");
			}
        }

        public void svcSignOutChat(string from, List<string> to)
		{
			try
			{
				if (EntsvcSignOutChat != null)
				{
					EntsvcSignOutChat(from, to);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSignOutChat", "clsNetTcpChat.cs");
			}
        }

        public void svcUnJoin( string uname)
		{
			try
			{
				if (EntsvcUnJoin != null)
				{
					EntsvcUnJoin(uname);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcUnJoin", "clsNetTcpChat.cs");
			}
        }

        public void svcShowStatus(string uname, List<string> to, string keydownstatus)
        {
            try
            {
                if (EntsvcShowStatus != null)
                {
                    EntsvcShowStatus(uname, to, keydownstatus);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcShowStatus", "clsNetTcpChat.cs");
            }
        }

        #endregion
    }
}
