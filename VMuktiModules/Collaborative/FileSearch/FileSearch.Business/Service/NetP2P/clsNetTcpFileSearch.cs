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

namespace FileSearch.Business.Service.NetP2P
{
    public class clsNetTcpFileSearch:IFileTransfer
    {
        public delegate void delsvcJoin(string uName);
        public delegate void delsvcSearchQuery(string strQuery, string uName, List<string> GuestList);
        public delegate void delsvcSearchResult(string[] strResult, string uName);
        public delegate void delsvcRequestFile(string FileName, string To, string From);
        public delegate void delsvcSendFileBlock(byte[] arr, string To, string From, string FileName, int signal);
		public delegate void delsvcGetUserList(string uName);
        public delegate void delsvcSetUserList(string uName);

        public delegate void delsvcUnJoin(string uName);

        public event delsvcJoin EntsvcJoin;
        public event delsvcSearchQuery EntsvcSearchQuery;
        public event delsvcSearchResult EntsvcSearchResult;
        public event delsvcRequestFile EntsvcRequestFile;
        public event delsvcSendFileBlock EntsvcSendFileBlock;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcUnJoin EntsvcUnJoin;

        public void svcJoin(string uName)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uName);
            }
        }

        public void svcSearchQuery(string strQuery, string uName, List<string> GuestList)
        {
            if (EntsvcSearchQuery != null)
            {
                EntsvcSearchQuery(strQuery, uName,GuestList);
            }
        }
        public void svcSearchResult(string[] strResult, string uName)
        {
            if (EntsvcSearchResult != null)
            {
                EntsvcSearchResult(strResult, uName);
            }
        }

        public void svcRequestFile(string FileName, string To, string From)
        {
            if (EntsvcRequestFile != null)
            {
                EntsvcRequestFile(FileName, To, From);
            }
        }

        public void svcSendFileBlock(byte[] arr, string To, string From, string FileName, int signal)
        {
            if (EntsvcSendFileBlock != null)
            {
                EntsvcSendFileBlock(arr, To, From, FileName, signal);
            }
        }

		public void svcGetUserList(string uName)
        {
            if (EntsvcGetUserList != null)
            {
                EntsvcGetUserList(uName);
            }
        }

        public void svcSetUserList(string uName)
        {
            if (EntsvcSetUserList != null)
            {
                EntsvcSetUserList(uName);
            }
        }

        public void svcUnJoin(string uName)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uName);
            }
        }
    }
}
