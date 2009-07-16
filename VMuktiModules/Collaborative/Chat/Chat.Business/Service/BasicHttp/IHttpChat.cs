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
using Chat.Business.Service.DataContracts;

namespace Chat.Business.Service.BasicHttp
{
    [ServiceContract]
    public interface IHttpChat
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uname);

        [OperationContract(IsOneWay = true)]
        void svcSendMessage(string msg, string from, List<string> to);

        [OperationContract(IsOneWay = false)]
        List<clsMessage> svcGetMessages(string recipient);

        [OperationContract(IsOneWay = true)]
        void svcSetUserList(string uname);

        [OperationContract(IsOneWay = true)]
        void svcGetUserList(string uname);

        [OperationContract(IsOneWay = true)]
        void svcSignOutChat(string from, List<string> to);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uname);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcGetMessages(string recipient, AsyncCallback callback, object asyncState);
        List<clsMessage> EndsvcGetMessages(IAsyncResult result);


        [OperationContract(IsOneWay = false)]
        int svcShowStatus(string uname, List<string> to, string keydownstatus);


        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcShowStatus(string uname, List<string> to, string keydownstatus, AsyncCallback callback, object asyncState);
        int EndsvcShowStatus(IAsyncResult result);



    }

    public interface IHttpChatChannel : IHttpChat, IClientChannel
    {
    }
}
