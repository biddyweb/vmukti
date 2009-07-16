/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    public class ClsNetP2PConsoleDelegates : INetP2PConsoleService
    {
        public delegate void DelsvcNetP2PConsoleJoin(string uName);
        public delegate void DelsvcNetP2PConsoleSendMsg(string msg);
        //public delegate void DelsvcNetP2PConsoleGetLog(string from, string to, string key);
        public delegate void DelsvcNetP2PConsoleGetLog(string from, List<string> to);
        public delegate void DelsvcNetP2pConsoleSendLog(string from, string to, string key, byte[] barr, int size, bool flag);
        public delegate void DelsvcNetP2PConsoleUnJoin(string uName);

        public event DelsvcNetP2PConsoleJoin EntsvcNetP2PConsoleJoin;
        public event DelsvcNetP2PConsoleSendMsg EntsvcNetP2PConsoleSendMsg;
        public event DelsvcNetP2PConsoleGetLog EntsvcNetP2PConsoleGetLog;
        public event DelsvcNetP2pConsoleSendLog EntsvcNetP2PConsoleSendLog;
        public event DelsvcNetP2PConsoleUnJoin EntsvcNetP2PConsoleUnJoin;

        #region INetP2PConsoleService Members

        public void svcNetP2ConsoleJoin(string uName)
        {
            if (EntsvcNetP2PConsoleJoin != null)
            {
                EntsvcNetP2PConsoleJoin(uName);
            }
        }

        public void svcNetP2ConsoleSendMsg(string msg)
        {
            if (EntsvcNetP2PConsoleSendMsg != null)
            {
                EntsvcNetP2PConsoleSendMsg(msg);
            }
        }

        //public void svcNetP2ConsoleGetLog(string from, string to, string key)
        //{
        //    if (EntsvcNetP2PConsoleGetLog != null)
        //    {
        //        EntsvcNetP2PConsoleGetLog(from, to, key);
        //    }
        //}

        public void svcNetP2ConsoleGetLog(string from, List<string> to)
        {
            if (EntsvcNetP2PConsoleGetLog != null)
            {
                EntsvcNetP2PConsoleGetLog(from, to);
            }
        }

        public void svcNetP2ConsoleSendLog(string from, string to, string key, byte[] barr, int size, bool flag)
        {
            if (EntsvcNetP2PConsoleSendLog != null)
            {
                EntsvcNetP2PConsoleSendLog(from, to, key, barr, size, flag);
            }
        }

        public void svcNetP2PConsoleUnJoin(string uName)
        {
            if (EntsvcNetP2PConsoleUnJoin != null)
            {
                EntsvcNetP2PConsoleUnJoin(uName);
            }
        }



        #endregion
    }
}
