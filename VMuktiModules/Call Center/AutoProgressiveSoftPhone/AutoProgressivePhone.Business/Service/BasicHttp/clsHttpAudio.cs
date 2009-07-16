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
using System.ServiceModel;

namespace AutoProgressivePhone.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class clsHttpAudio:IHttpAudio
    {

        public delegate void DelsvcJoin(string uName);
        public delegate void DelsvcStartConference(string uName, string strConfNumber, string[] GuestName);
        public delegate string DelsvcGetConference(string uName);
        public delegate void DelsvcUnJoin(string uName);

        public event DelsvcJoin EntsvcJoin;
        public event DelsvcStartConference EntsvcStartConference;
        public event DelsvcGetConference EntsvcGetConference;
        public event DelsvcUnJoin EntsvcUnjoin;

        #region IHttpAudio Members

        public void svcJoin(string uName)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uName);
            }
        }

        public void svcStartConference(string uName, string strConfNumber, string[] GuestName)
        {
            if (EntsvcStartConference != null)
            {
                EntsvcStartConference(uName, strConfNumber, GuestName);
            }
        }

        public string svcGetConference(string uName)
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

        public void svcUnJoin(string uName)
        {
            if (EntsvcUnjoin != null)
            {
                EntsvcUnjoin(uName);
            }
        }

        #endregion
    }
}
