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
using System.Text;
using System;
using VMuktiAPI;

namespace VMukti.Business
{
    public class clsRTCAuthClient
    {        
      
        private string _UserName;
        private string _Password;
        private string _SIPServerIP;
        private string _AuthResult;

        private RTCAudio _objRTCAudio;

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        public string SIPServerIP
        {
            get
            {
                return _SIPServerIP;
            }
            set
            {
                _SIPServerIP = value;
            }
        }

        public string AuthResult
        {
            get
            {
                return _AuthResult;
            }
            set
            {
                _AuthResult = value;
            }
        }

        public delegate void DelAuthStatus(string strAuthStatus);
        public event DelAuthStatus EntAuthStatus;

        public clsRTCAuthClient(string pUserName, string pPassword, string pSIPServerIP)
        {
            try
            {
                _objRTCAudio = new RTCAudio(pUserName, pPassword, pSIPServerIP);
                _objRTCAudio.Register();
                _objRTCAudio.Entstatus += new RTCAudio.DelStatus(_objRTCAudio_Entstatus);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"clsRTCAuthClient()","clsRTCAuthClient.cs");
            }
        }

        void _objRTCAudio_Entstatus(RTCAudio sender, string status)
        {
            try
            {
                _AuthResult = status;
                if (EntAuthStatus != null)
                {
                    EntAuthStatus(status);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"_objRTCAudio_EntStatus()","clsRTCAuthClient.cs");
            }
        }
    }
}
