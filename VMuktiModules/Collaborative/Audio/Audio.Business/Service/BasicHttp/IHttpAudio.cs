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
using Audio.Business.Service.DataContracts;
using System;

namespace Audio.Business.Service.BasicHttp
{
    [ServiceContract]
    public interface IHttpAudio
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void svcStartConference(string uName, string strConfNumber, string[] GuestName);

        [OperationContract(IsOneWay = false)]
        List<clsMessage> svcGetConference(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSetUserList(string uname, string strConf);

        [OperationContract(IsOneWay = true)]
        void svcGetUserList(string uname, string strConf);

        [OperationContract(IsOneWay = true)]
        void svcSignOutAudio(string from, List<string> to);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginsvcGetConference(string uName, AsyncCallback callback, object asyncState);
        List<clsMessage> EndsvcGetConference(IAsyncResult result);
    }

    public interface IHttpAudioChannel : IClientChannel, IHttpAudio
    { }
}
