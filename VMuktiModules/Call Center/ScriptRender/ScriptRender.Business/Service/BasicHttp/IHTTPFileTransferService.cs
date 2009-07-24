<<<<<<< HEAD:VMuktiModules/Call Center/ScriptRender/ScriptRender.Business/Service/BasicHttp/IHTTPFileTransferService.cs
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
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
=======
﻿using System;
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ScriptRender/ScriptRender.Business/Service/BasicHttp/IHTTPFileTransferService.cs
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ScriptRender.Business
{
    [ServiceContract]
    public interface IHTTPFileTransferService
    {
        [OperationContract(IsOneWay = true)]
        void svcHTTPFileTransferServiceJoin();

        [OperationContract(IsOneWay = true)]
        void svcHTTPFileTransferServiceUploadFile(RemoteFileInfo request);

        [OperationContract(IsOneWay = true)]
        void svcHTTPFileTransferServiceUploadFileToInstallationDirectory(RemoteFileInfo request);

        [OperationContract(IsOneWay = false)]
        RemoteFileInfo svcHTTPFileTransferServiceDownloadFile(DownloadRequest request);

    }

    public interface IHTTPFileTransferServiceChannel : IHTTPFileTransferService, IClientChannel
    {
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;

        [MessageBodyMember]
        public string FolderWhereFileIsStored;

    }
        
    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageHeader(MustUnderstand = true)]
        public string FolderNameToStore;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        

        public void Dispose()
        {
            // close stream when the contract instance is disposed. this ensures that stream is closed when file download is complete, since download procedure is handled by the client and the stream must be closed on server.
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }

    

}
