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

using System.Collections.Generic;
using System.ServiceModel;
using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetP2PConsoleService))]
    public interface INetP2PConsoleService
    {
        [OperationContract(IsOneWay = true)]
        void svcNetP2ConsoleJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void svcNetP2ConsoleSendMsg(string msg);

        //[OperationContract(IsOneWay = true)]
        //void svcNetP2ConsoleGetLog(string from, string to, string key);

        [OperationContract(IsOneWay = true)]
        void svcNetP2ConsoleGetLog(string from, List<string> to);

        [OperationContract(IsOneWay = true)]
        void svcNetP2ConsoleSendLog(string from, string to, string key, byte[] barr, int size, bool flag);

        [OperationContract(IsOneWay = true)]
        void svcNetP2PConsoleUnJoin(string uName);
    }

    public interface INetP2PConsoleChannel : INetP2PConsoleService, IClientChannel
    {
    }
}
