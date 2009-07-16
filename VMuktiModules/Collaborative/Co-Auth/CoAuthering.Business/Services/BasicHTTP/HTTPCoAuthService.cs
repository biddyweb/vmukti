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
using CoAuthering.Business.DataContracts;
using System;
using System.Text;

namespace CoAuthering.Business.BasicHTTP
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class HTTPCoAuthService : IHttpCoAuthService
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

		public delegate void delsvcJoin(string uName);
        public delegate bool DelsvcSetLength(int byteLength, string uName, string strRole);
        public delegate void DelsvcReplySetLength(int byteLength, bool isLenghtSet, string uName);
		public delegate void DelsvcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers);
		public delegate void DelsvcSaveDoc(string uName, List<string> to);
		public delegate void delsvcSendChangedContext(byte[] myDoc, string from, string[] to);
        public delegate List<clsCoAuthDataMember> delsvcGetChangedContext(string recipient, string strRole);
		public delegate void delsvcUnJoin(string uName);
        //public delegate void delsvcGetUserList(string uname, List<string> to);
        //public delegate void delsvcSetUserList(string uname, List<string> to);
        
        public delegate void delsvcGetUserList(string uname);
        public delegate void delsvcSetUserList(string uname);

		public delegate void delsvcSignOutCoAuth(string from, List<string> to);

        // Related to Asynchronous WCF Calling
        public delegate IAsyncResult delBeginsvcGetChangedContext(string recipient, string strRole, AsyncCallback callback, object asyncState);
        public delegate List<clsCoAuthDataMember> delEndsvcGetChangedContext(IAsyncResult result);
        // End

		public event delsvcJoin EntsvcJoin;
		public event DelsvcSetLength EntsvcSetLength;
        public event DelsvcReplySetLength EntsvcReplySetLength;
		public event DelsvcSetCompBytes EntsvcSetCompBytes;
		public event DelsvcSaveDoc EntsvcSaveDoc;
		public event delsvcSendChangedContext EntsvcSendChangedContext;
		public event delsvcGetChangedContext EntvcGetChangedContext;
		public event delsvcUnJoin EntsvcUnJoin;
		public event delsvcSetUserList EntsvcSetUserList;
		public event delsvcGetUserList EntsvcGetUserList;
		public event delsvcSignOutCoAuth EntsvcSignOutCoAuth;

        public event delBeginsvcGetChangedContext EntBeginsvcGetChangedContext;
        public event delEndsvcGetChangedContext EntEndsvcGetChangedContext;

		#region IHttpCoAuthService Members

		public void svcJoin(string uname)
		{
            try
            {
                if (EntsvcJoin != null)
                {
                    EntsvcJoin(uname);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcJoin", "HTTPCoAuthService.cs");
                
            }

		}

        public bool svcSetLength(int byteLength, string uName, string strRole)
		{
            try
            {
                if (EntsvcSetLength != null)
                {
                    return EntsvcSetLength(byteLength, uName, strRole);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetLength", "HTTPCoAuthService.cs");
                return false;
                
            }
		}

        public void svcReplySetLength(int byteLength, bool isLenghtSet, string uName)
        {
            try
            {
                if (EntsvcReplySetLength != null)
                {
                    EntsvcReplySetLength(byteLength, isLenghtSet, uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcReplySetLength", "HTTPCoAuthService.cs");
            }
        }

		public void svcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers)
		{
            try
            {
                if (EntsvcSetCompBytes != null)
                {
                    EntsvcSetCompBytes(byteLength, myDoc, uName, strReceivers);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetCompBytes", "HTTPCoAuthService.cs");
            }
		}

		public void svcSaveDoc(string uName, List<string> to)
		{
            try
            {
                if (EntsvcSaveDoc != null)
                {
                    EntsvcSaveDoc(uName, to);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSaveDoc", "HTTPCoAuthService.cs");
            }
		}
		public void svcSendChangedContext(byte[] myDoc, string from, string[] to)
		{
            try
            {
                if (EntsvcSendChangedContext != null)
                {
                    EntsvcSendChangedContext(myDoc, from, to);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSendChangedContext", "HTTPCoAuthService.cs");
            }
		}

        public List<CoAuthering.Business.DataContracts.clsCoAuthDataMember> svcGetChangedContext(string recipient, string strRole)
		{
            try
            {
                if (EntvcGetChangedContext != null)
                {
                    return EntvcGetChangedContext(recipient, strRole);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcGetChangedContext", "HTTPCoAuthService.cs");
                return null;
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
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcUnJoin", "HTTPCoAuthService.cs");
            }
		}

        public void svcSetUserList(string uname) //, List<string> to)
		{
            try
            {
                if (EntsvcSetUserList != null)
                {
                    EntsvcSetUserList(uname);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetUserList", "HTTPCoAuthService.cs");
            }


		}

        public void svcGetUserList(string uname)//, List<string> to)
		{
            try
            {
                if (EntsvcGetUserList != null)
                {
                    EntsvcGetUserList(uname);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcGetUserList", "HTTPCoAuthService.cs");
            }


		}

		public void svcSignOutCoAuth(string from, List<string> to)
		{
            try
            {
                if (EntsvcSignOutCoAuth != null)
                {
                    EntsvcSignOutCoAuth(from, to);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSignOutCoAuth", "HTTPCoAuthService.cs");
            }
		}

        public IAsyncResult BeginsvcGetChangedContext(string recipient, string strRole, AsyncCallback callback, object asyncState)
        {
            try
            {
                if (EntBeginsvcGetChangedContext != null)
                {
                    return EntBeginsvcGetChangedContext(recipient, strRole, callback, asyncState);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BeginsvcGetChangedContext", "HTTPCoAuthService.cs");
                return null;
            }
        }

        public List<clsCoAuthDataMember> EndsvcGetChangedContext(IAsyncResult result)
        {
            try
            {
                if (EntEndsvcGetChangedContext != null)
                {
                    return EntEndsvcGetChangedContext(result);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "EndsvcGetChangedContext", "HTTPCoAuthService.cs");
                return null;
            }
        }

       


		#endregion



    }
}
