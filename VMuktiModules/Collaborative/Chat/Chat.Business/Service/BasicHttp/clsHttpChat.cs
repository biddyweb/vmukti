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
using System.ServiceModel;
using Chat.Business.Service.DataContracts;
using VMuktiAPI;

namespace Chat.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class clsHttpChat : IHttpChat
    {
        public delegate void delsvcJoin(string uName);
        public delegate void delsvcSendMessage(string msg, string from, List<string> to);
        public delegate List<clsMessage> delsvcGetMessages(string recipient);
        public delegate void delsvcSetUserList(string uname);
        public delegate void delsvcGetUserList(string uname);
        public delegate void delsvcSignOutChat(string from, List<string> to);
        public delegate void delsvcUnJoin(string uName);
        public delegate int delsvcShowStatus(string uname, List<string> to, string keydownstatus);

        public delegate IAsyncResult delBeginsvcGetMessages(string recipient, AsyncCallback callback, object asyncState);
        public delegate List<clsMessage> delEndsvcGetMessages(IAsyncResult result);

        public delegate IAsyncResult delBeginsvcShowStatus(string uname, List<string> to, string keydownstatus, AsyncCallback callback, object asyncState);
        public delegate int delEndsvcShowStatus(IAsyncResult result);

        public event delsvcJoin EntsvcJoin;
        public event delsvcSendMessage EntsvcSendMessage;
        public event delsvcGetMessages EntsvcGetMessages;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSignOutChat EntsvcSignOutChat;
        public event delsvcUnJoin EntsvcUnJoin;
        public event delsvcShowStatus EntsvcShowStatus;

        public event delBeginsvcGetMessages EntBeginsvcGetMessages;
        public event delEndsvcGetMessages EntEndsvcGetMessages;

        public event delBeginsvcShowStatus EntBeginsvcShowStatus;
        public event delEndsvcShowStatus EntEndsvcShowStatus;

        #region IHttpChat Members

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcJoin", "clsHttpChat.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSendMessage()", "clsHttpChat.cs");
			}
        }

        public List<clsMessage> svcGetMessages(string recipient)
		{
			try
			{

				if (EntsvcGetMessages != null)
				{
					return EntsvcGetMessages(recipient);
				}
				else
				{
					return null;
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcGetMessage()", "clsHttpChat.cs");
				return null;
			}
        }

        public void svcSetUserList(string uname)
		{
			try
			{
				if (EntsvcSetUserList != null)
				{
					EntsvcSetUserList(uname);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSetUserList()", "clsHttpChat.cs");
			}
        }

        public void svcGetUserList(string uname)
		{
			try
			{
				if (EntsvcGetUserList != null)
				{                   
					EntsvcGetUserList(uname);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcGetUserList()", "clsHttpChat.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSignOutChat()", "clsHttpChat.cs");
			}
        }
        
        public void svcUnJoin(string uname)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcUnJoin()", "clsHttpChat.cs");
			}
        }

        public IAsyncResult BeginsvcGetMessages(string recipient, AsyncCallback callback, object asyncState)
        {
            if (EntBeginsvcGetMessages != null)
            {
                return EntBeginsvcGetMessages(recipient, callback, asyncState);
            }
            else
            { return null; }
            
        }

        public List<clsMessage> EndsvcGetMessages(IAsyncResult result)
        {
            if (EntEndsvcGetMessages != null)
            {
                return EntEndsvcGetMessages(result);
            }
            else
            { return null; }
        }

        public int svcShowStatus(string uname, List<string> to, string keydownstatus)
        {
            try
            {
                if (EntsvcShowStatus != null)
                {
                    return EntsvcShowStatus(uname, to, keydownstatus);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcShowStatus()", "clsHttpChat.cs");
                return -1;
            }
        }

        public IAsyncResult BeginsvcShowStatus(string uname, List<string> to, string keydownstatus, AsyncCallback callback, object asyncState)
        {
            if (EntBeginsvcShowStatus != null)
            {
                return EntBeginsvcShowStatus(uname, to, keydownstatus, callback, asyncState);
            }
            else
            {
                return null;
            }
        }

        public int EndsvcShowStatus(IAsyncResult result)
        {
            if (EntEndsvcShowStatus != null)
            {
                return EntEndsvcShowStatus(result);
            }
            else
            {
                return -1;
            }
        }
        #endregion        
    }
}
