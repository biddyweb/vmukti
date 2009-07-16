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

namespace AutoProgressivePhone.Business.Service.NetP2P
{
    public class clsNetTcpAudio:INetTcpAudio
    {
        public delegate void DelsvcP2PJoin(string uName);
        public delegate void DelsvcP2PStartConference(string uName, string strConfNumber, string[] GuestInfo);
        public delegate void DelsvcP2PUnJoin(string uName);

        public event DelsvcP2PJoin EntsvcP2PJoin;
        public event DelsvcP2PStartConference EntsvcP2PStartConference;
        public event DelsvcP2PUnJoin EntsvcP2PUnJoin;

        #region INetTcpAudio Members

        public void svcP2PJoin(string uName)
        {
            if (EntsvcP2PJoin != null)
            {
                EntsvcP2PJoin(uName);
            }
        }

        public void svcP2PStartConference(string uName, string strConfNumber, string[] GuestInformation)
        {
            if (EntsvcP2PStartConference != null)
            {
                EntsvcP2PStartConference(uName, strConfNumber, GuestInformation);
            }
        }

        public void svcP2PUnJoin(string uName)
        {
            if (EntsvcP2PUnJoin != null)
            {
                EntsvcP2PUnJoin(uName);
            }
        }

        #endregion
    }
}
