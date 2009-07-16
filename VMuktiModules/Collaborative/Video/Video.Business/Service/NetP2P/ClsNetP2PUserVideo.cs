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

namespace Video.Business.Service.NetP2P
{
    public class ClsNetP2PUserVideo : INetP2PUserVideo
    {
        public delegate void delsvcJoin(string UName);
        public delegate void delsvcSendStream(string UName, byte[] byteArrayImage);
        public delegate void delsvcUnJoin(string UName);

        public event delsvcJoin EntsvcJoin;
        public event delsvcSendStream EntsvcSendStream;
        public event delsvcUnJoin EntsvcUnJoin;


        #region INetTcpVideo Members

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcJoin", "ClsNetP2PUserVideo.cs");             
            }
        }

        public void svcSendStream(string UName, byte[] byteArrayImage)
        {
            try
            {
                if (EntsvcSendStream != null)
                {
                    EntsvcSendStream(UName, byteArrayImage);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcSendStream", "ClsNetP2PUserVideo.cs");             
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "svcUnJoin", "ClsNetP2PUserVideo.cs");             
            }
        }

        #endregion
    }
}
