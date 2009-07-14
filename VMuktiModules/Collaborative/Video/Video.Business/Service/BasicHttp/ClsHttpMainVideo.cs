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
using System.ServiceModel;
using Video.Business.Service.DataContracts;
using VMuktiAPI;

namespace Video.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ClsHttpMainVideo : IHttpMainVideo
    {
        public delegate void delsvcJoin(string UName);
        public delegate void delsvcGetUserList(string UName, string videoURI);
        public delegate void delsvcSetUserList(string UName, string videoURI);
        public delegate List<ClsMessage> delsvcGetMessage(string recipient);
        public delegate void delsvcUnJoin(string UName);

        public event delsvcJoin EntsvcJoin;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcGetMessage EntsvcGetMessage;
        public event delsvcUnJoin EntsvcUnJoin;

        #region IHttpMainVideo Members

        public void svcJoin(string UName)
        {
            try
            {
                if (EntsvcJoin != null)
                {
                    EntsvcJoin(UName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcJoin", "ClsHttpMainVideo.cs");
            }
        }

        public void svcGetUserList(string UName, string videoURI)
        {
            try
            {
                if (EntsvcGetUserList != null)
                {
                    EntsvcGetUserList(UName, videoURI);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcGetUserList", "ClsHttpMainVideo.cs");
            }
        }

        public void svcSetUserList(string UName, string videoURI)
        {
            try
            {
                if (EntsvcSetUserList != null)
                {
                    EntsvcSetUserList(UName, videoURI);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSetUserList", "ClsHttpMainVideo.cs");
            }
        }

        public List<ClsMessage> svcGetMessage(string recipient)
        {
            try
            {
                if (EntsvcGetMessage != null)
                {
                    return EntsvcGetMessage(recipient);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcGetMessage", "ClsHttpMainVideo.cs");
                return null;
            }
        }

        public void svcUnJoin(string UName)
        {
            try
            {
                if (EntsvcUnJoin != null)
                {
                    EntsvcUnJoin(UName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcUnJoin", "ClsHttpMainVideo.cs");
            }
        }

        #endregion
    }
}
