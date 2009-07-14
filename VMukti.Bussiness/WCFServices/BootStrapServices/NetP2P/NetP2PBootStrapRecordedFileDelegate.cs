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
using VMukti.Business.CommonMessageContract;

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    public class NetP2PBootStrapRecordedFileDelegate:INetP2PBootStrapRecordedFileService
    {
        public delegate void delsvcRecordedFileJoin(clsMessageContract mcRFJoin);
        public delegate void delsvcRecordedFileUnJoin(clsMessageContract mcRFUnJoin);
        public delegate void delsvcSendRecordedFiles(clsMessageContract mcSendRecordedFiles);

        public event delsvcRecordedFileJoin EntsvcRecordedFileJoin;
        public event delsvcRecordedFileUnJoin EntsvcRecordedFileUnJoin;
        public event delsvcSendRecordedFiles EntsvcSendRecordedFiles;


        #region INetP2PBootStrapRecordedFileService Members

        public void svcRecordedFileJoin(clsMessageContract mcRFJoin)
        {
            if (EntsvcRecordedFileJoin != null)
            {
                EntsvcRecordedFileJoin(mcRFJoin);
            }
        }

        public void svcRecordedFileUnJoin(clsMessageContract mcRFUnJoin)
        {
            if (EntsvcRecordedFileUnJoin != null)
            {
                EntsvcRecordedFileUnJoin(mcRFUnJoin);
            }
        }

        public void svcSendRecordedFiles(clsMessageContract mcSendRecordedFiles)
        {
            if (EntsvcSendRecordedFiles != null)
            {
                EntsvcSendRecordedFiles(mcSendRecordedFiles);
            }
        }

        #endregion

        #region INetP2PBootStrapRecordedFileService Members

        //void INetP2PBootStrapRecordedFileService.svcRecordedFileJoin(clsMessageContract mcRFJoin)
        //{
        //    throw new NotImplementedException();
        //}

        //void INetP2PBootStrapRecordedFileService.svcRecordedFileUnJoin(clsMessageContract mcRFUnJoin)
        //{
        //    throw new NotImplementedException();
        //}

        //void INetP2PBootStrapRecordedFileService.svcSendRecordedFiles(clsMessageContract mcSendRecordedFiles)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
