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
using System.Text;
using System;

namespace CoAuthering.Business.NetP2P
{
    public class NetP2PCoAuthService : INetP2PCoAuthService
    {
        

        public delegate void DelSvcJoin(string uName);
        public delegate void DelsvcSetLength(int byteLength, string uName, string strRole);
        public delegate void DelsvcReplySetLength(int byteLength, bool isLenghtSet, string uName);
        public delegate void DelsvcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers);
		public delegate void DelsvcSaveDoc(string uName, List<string> to);
        public delegate void DelcliSetCompBytes(bool isLenghtSet, byte[] myDoc, string uName);        
        public delegate void DelsvcUnJoin(string uName);
       
		public delegate void delsvcSignOutCoAuth(string from, List<string> to);

        public delegate void DelsvcGetUserList(string uName);
        public delegate void DelsvcSetUserList(string uName);


        public event DelSvcJoin EntSvcJoin;
        public event DelsvcSetLength EntsvcSetLength;
        public event DelsvcReplySetLength EntsvcReplySetLength;
        public event DelsvcSetCompBytes EntsvcSetCompBytes;
        public event DelsvcSaveDoc EntsvcSaveDoc;        
        //public event DelcliSetCompBytes EntcliSetCompBytes;        
        public event DelsvcUnJoin EntsvcUnJoin;
		public event DelsvcGetUserList EntsvcGetUserList;
		public event DelsvcSetUserList EntsvcSetUserList;
		public event delsvcSignOutCoAuth EntsvcSignOutCoAuth;




        #region INetP2PCoAuthService Members

        public void svcJoin(string uName)
        {
            try
            {
                if (EntSvcJoin != null)
                {
                    EntSvcJoin(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcJoin", "NetP2PCoAuthService.cs");
            }

        }

        public void svcSetLength(int byteLength, string uName, string strRole)
        {
            try
            {
                if (EntsvcSetLength != null)
                {
                    EntsvcSetLength(byteLength, uName, strRole);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetLength", "NetP2PCoAuthService.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcReplySetLength", "NetP2PCoAuthService.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetCompBytes", "NetP2PCoAuthService.cs");
            }
        }      

        public void svcUnJoin(string uName)
        {
            try
            {
                if (EntsvcUnJoin != null)
                {
                    EntsvcUnJoin(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcUnJoin", "NetP2PCoAuthService.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSaveDoc", "NetP2PCoAuthService.cs");
            }
        }

        public void svcGetUserList(string uName) //, List<string> to)
		{
            try
            {
                if (EntsvcGetUserList != null)
                {
                    EntsvcGetUserList(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcGetUserList", "NetP2PCoAuthService.cs");
            }
		}

        public void svcSetUserList(string uName) //, List<string> to)
		{
            try
            {
                if (EntsvcSetUserList != null)
                {
                    EntsvcSetUserList(uName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetUserList", "NetP2PCoAuthService.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSignOutCoAuth", "NetP2PCoAuthService.cs");
            }
		}
       

        #endregion



		
	}
}
