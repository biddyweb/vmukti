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
using System.Collections.Generic;
using System.ServiceModel;
using CoAuthering.Business.DataContracts;
using System;

namespace CoAuthering.Business.BasicHTTP
{
    [ServiceContract]
    public interface IHttpCoAuthService
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uname);

        [OperationContract(IsOneWay = false)]
        bool svcSetLength(int byteLength, string uName,string strRole);

        [OperationContract(IsOneWay = true)]
        void svcReplySetLength(int byteLength, bool isLenghtSet, string uName);

        [OperationContract(IsOneWay = true)]
        void svcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers);

        [OperationContract(IsOneWay = true)]
		void svcSaveDoc(string uName, List<string> to);

        [OperationContract(IsOneWay = true)]
        void svcSendChangedContext(byte[] myDoc, string from, string[] to);

        [OperationContract(IsOneWay = false)]
        List<clsCoAuthDataMember> svcGetChangedContext(string recipient, string strRole);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uname);

        //[OperationContract(IsOneWay = true)]
        //void svcGetUserList(string uname, List<string> to);

        //[OperationContract(IsOneWay = true)]
        //void svcSetUserList(string uname, List<string> to);

        [OperationContract(IsOneWay = true)]
        void svcGetUserList(string uname);

        [OperationContract(IsOneWay = true)]
        void svcSetUserList(string uname);

		[OperationContract(IsOneWay = true)]
		void svcSignOutCoAuth(string from, List<string> to);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcGetChangedContext(string recipient, string strRole, AsyncCallback callback, object asyncState);
        List<clsCoAuthDataMember> EndsvcGetChangedContext(IAsyncResult result);


    }

    public interface IHttpCoAuthServiceChannel : IHttpCoAuthService, IClientChannel
    {
    }
}
