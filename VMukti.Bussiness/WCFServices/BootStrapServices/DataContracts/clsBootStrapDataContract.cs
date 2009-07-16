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

using System.Runtime.Serialization;

namespace VMukti.Business.WCFServices.BootStrapServices.DataContracts
{
    [DataContract]
    public class clsBootStrapInfo
    {
        private string strAuthType;
        private string strAuthServerIP;
        private string strAuthSuperNodeIP;

        private string strConnectionString;
        private string strSIPUserNumber;

        [DataMember]
        public string AuthType
        {
            get
            {
                return strAuthType;
            }
            set
            {
                strAuthType = value;
            }
        }

        [DataMember]
        public string AuthServerIP
        {
            get
            {
                return strAuthServerIP;
            }
            set
            {
                strAuthServerIP = value;
            }
        }

        [DataMember]
        public string AuthSuperNodeIP
        {
            get
            {
                return strAuthSuperNodeIP;
            }
            set
            {
                strAuthSuperNodeIP = value;
            }
        }

        [DataMember]
        public string ConnectionString
        {
            get
            {
                return strConnectionString;
            }
            set
            {
                strConnectionString = value;
            }
        }

        [DataMember]
        public string SIPUserNumber
        {
            get
            {
                return strSIPUserNumber;
            }
            set
            {
                strSIPUserNumber = value;
            }
        }
    }
}
