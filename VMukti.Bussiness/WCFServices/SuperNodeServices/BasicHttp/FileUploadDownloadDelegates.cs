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
//using System.Linq;
using System.Text;
using System.ServiceModel;
using VMukti.Business.CommonMessageContract;

namespace VMukti.Business.WCFServices.SuperNodeServices.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FileUploadDownloadDelegates:IHttpFileUploadDownload
    {
        public delegate void delsvcJoin(string uName);
        public delegate void delUploadFile(MContractRemoteFileInfo request);
        public delegate MContractRemoteFileInfo delDownloadFile(MContractDownloadRequest request);
        public delegate IAsyncResult DelBeginDownloadFile(MContractDownloadRequest request, AsyncCallback callback, object asyncState);
        public delegate MContractRemoteFileInfo DelEndDownloadFile(IAsyncResult result);
        public delegate IAsyncResult DelBeginUploadFile(MContractRemoteFileInfo request, AsyncCallback callback, object asyncState);
        public delegate void DelEndUploadFile(IAsyncResult result);

        public event delsvcJoin entsvcJoin;
        public event delUploadFile entsvcUpload;
        public event delDownloadFile entsvcDownload;
        public event DelBeginDownloadFile entBeginDownloadFile;
        public event DelEndDownloadFile entEndDownloadFile;
        public event DelBeginUploadFile entBeginUploadFile;
        public event DelEndUploadFile entEndUploadFile;

        #region IFileTransferService Members

        public void svcJoin(string uName)
        {
            entsvcJoin(uName);
        }

        public void UploadFile(MContractRemoteFileInfo request)
        {
            entsvcUpload(request);
        }

        public MContractRemoteFileInfo DownloadFile(MContractDownloadRequest request)
        {
            return (entsvcDownload(request));
        }        

        public IAsyncResult BeginDownloadFile(MContractDownloadRequest request, AsyncCallback callback, object asyncState)
        {
            return entBeginDownloadFile(request, callback, asyncState);
        }

        public MContractRemoteFileInfo EndDownloadFile(IAsyncResult result)
        {
            return entEndDownloadFile(result);
        }

        public IAsyncResult BeginUploadFile(MContractRemoteFileInfo request, AsyncCallback callback, object asyncState)
        {
            return entBeginUploadFile(request, callback, asyncState);
        }

        public void EndUploadFile(IAsyncResult result)
        {
            entEndUploadFile(result);
        }

        #endregion
       
    }
}
