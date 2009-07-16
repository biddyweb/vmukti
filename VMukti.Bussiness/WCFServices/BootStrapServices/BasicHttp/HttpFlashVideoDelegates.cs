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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VMukti.Business.WCFServices.BootStrapServices.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HttpFlashVideoDelegates:IHttpBootStrapFlashVideo
    {
        #region Delegates

        public delegate void DelsvcHttpBSFVJoin(string uName);
        public delegate int DelsvcHttpBSFVCreateFolder(string uName, string Url);
        public delegate string DelsvcHttpBSFVGetUrl(int indentifier);
        public delegate void DelsvcHttpBSFVUnJoin(int identifier);
       

        #endregion

        #region Events

        public event DelsvcHttpBSFVJoin EntsvcHttpBSFVJoin;
        public event DelsvcHttpBSFVCreateFolder EntsvcHttpBSFVCreateFolder;
        public event DelsvcHttpBSFVGetUrl EntsvcHttpBSFVGetUrl;
        public event DelsvcHttpBSFVUnJoin EntsvcHttpBSFVUnJoin;

        #endregion

        #region IHttpBootStrapFlashVideo Members

        public void svcHttpBSFVJoin(string uName)
        {
            if (EntsvcHttpBSFVJoin != null)
            {
                EntsvcHttpBSFVJoin(uName);
            }
        }

        public int svcHttpBSFVCreateFolder(string uName, string Url)
        {
            if (EntsvcHttpBSFVCreateFolder != null)
            {
                return EntsvcHttpBSFVCreateFolder(uName, Url);
            }
            else
            {
                return -1;
            }
        }

        public string svcHttpBSFVGetUrl(int indentifier)
        {
            if (EntsvcHttpBSFVGetUrl != null)
            {
                return EntsvcHttpBSFVGetUrl(indentifier);
            }
            else
            {
                return "";
            }
        }

        public void svcHttpBSFVUnJoin(int identifier)
        {
            if (EntsvcHttpBSFVUnJoin != null)
            {
                EntsvcHttpBSFVUnJoin(identifier);
            }
        }

        #endregion
       
    }
}
