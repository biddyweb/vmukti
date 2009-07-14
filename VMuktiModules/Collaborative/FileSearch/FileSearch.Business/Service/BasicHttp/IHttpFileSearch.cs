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
using FileSearch.Business.Service.DataContracts;

namespace FileSearch.Business.Service.BasicHttp
{
    [ServiceContract]
    public interface IHttpFileSearch
    {
        [OperationContract(IsOneWay = true)]
        void svcHttpJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void svcHttpSendQuery(string uName, string Query);

        [OperationContract(IsOneWay = true)]
        void svcHttpReplyQuery(string uName, string[] QueryResult);

        [OperationContract(IsOneWay = false)]
        List<clsMessage> svcHttpGetMessage(string recipient);

        [OperationContract(IsOneWay = false)]
        string[] svcHttpGetFileList(string recipient);

        [OperationContract(IsOneWay = true)]
        void svcHttpFileRequest(string FilePath, string FileTo, string FileFrom);

        [OperationContract(IsOneWay = true)]
        void svcHttpFileReply(byte[] FileBlock, string FileTo, string FileFrom, string FileName,int Signal);

        [OperationContract(IsOneWay = false)]
        List<byte[]> svcHttpDownloadFile(string UserName, string FileName);

        [OperationContract(IsOneWay = true)]
        void svcHttpUnJoin(string uName);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcHttpGetMessage(string recipient, AsyncCallback callback, object asyncState);
        List<clsMessage> EndsvcHttpGetMessage(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcHttpGetFileList(string recipient, AsyncCallback callback, object asyncState);
        string[] EndsvcHttpGetFileList(IAsyncResult result);
    }

    public interface IHttpFileSearchChannel : IHttpFileSearch, IClientChannel
    {
    }
}
