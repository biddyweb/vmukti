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

using System.IO;
using System.ServiceModel;

namespace VMukti.Bussiness.WCFServices.BootStrapServices.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HttpPresentationUploadDelegates : IHttpPresentationUploadService
    {
        public delegate void DelsvcHttpJoin();
        public delegate void DelsvcHttpSendUploadedFile(string strFileName, byte[] bytearr, int intSig);
        public delegate string[] DelsvcHttpGetPresentationName();
        public delegate int DelsvcHttpSlidesCount(string PName);
        public delegate Stream DelsvcHttpGetSlideStream(byte[] pnameCnt);
        public delegate bool DelsvcHttpPPTExists(string filename);
        public delegate void DelsvcHttpUnJoin();


        public event DelsvcHttpJoin EntsvcHttpJoin;
        public event DelsvcHttpSendUploadedFile EntsvcHttpSendUploadedFile;
        public event DelsvcHttpGetPresentationName EntsvcHttpGetPresentationName;
        public event DelsvcHttpSlidesCount EntsvcHttpSlidesCount;
        public event DelsvcHttpGetSlideStream EntsvcHttpGetSlideStream;
        public event DelsvcHttpPPTExists EntsvcHttpPPTExists;
        public event DelsvcHttpUnJoin EntsvcHttpUnJoin;

        public void svcHttpJoin()
        {
            if (EntsvcHttpJoin != null)
            {
                EntsvcHttpJoin();
            }
        }

        public void svcHttpSendUploadedFile(string strFileName, byte[] bytearr, int intSig)
        {
            if (EntsvcHttpSendUploadedFile != null)
            {
                EntsvcHttpSendUploadedFile(strFileName, bytearr, intSig);
            }
        }

        public string[] svcHttpGetPresentationName()
        {
            if (EntsvcHttpGetPresentationName != null)
            {
                return EntsvcHttpGetPresentationName();
            }
            else
            {
                return null;
            }
        }

        public int svcHttpSlidesCount(string PName)
        {
            if (EntsvcHttpSlidesCount != null)
            {
                return EntsvcHttpSlidesCount(PName);
            }
            else
            {
                return -1;
            }
        }

        public Stream svcHttpGetSlideStream(byte[] pnameCnt)
        {
            if (EntsvcHttpGetSlideStream != null)
            {
                return EntsvcHttpGetSlideStream(pnameCnt);
            }
            else
            {
                return null;
            }
        }

        public bool svcHttpPPTExists(string filename)
        {
            if (EntsvcHttpPPTExists != null)
            {
                return EntsvcHttpPPTExists(filename);
            }
            else
            {
                return false;
            }
        }

        public void svcHttpUnJoin()
        {
            if (EntsvcHttpUnJoin != null)
            {
                EntsvcHttpUnJoin();
            }
        }

    }
}
