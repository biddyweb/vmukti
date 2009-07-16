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
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using VMukti.Business.WCFServices.SuperNodeServices.DataContract;


namespace VMukti.Business.WCFServices.BootStrapServices.BasicHttp
{
    [ServiceContract]
    public interface IHTTPBootStrapService
    {
        [OperationContract(IsOneWay = false)]
        clsBootStrapInfo svcHttpBSJoin(string uName, clsPeerInfo objPeerInformation);

        [OperationContract(IsOneWay = false)]
        clsSuperNodeDataContract svcHttpBsGetSuperNodeIP(string uName, string IP, bool blSuperNode);

        [OperationContract(IsOneWay = false)]
        List<string> svcHttpGetSuperNodeBuddyList(string uName);

        [OperationContract(IsOneWay = false)]
        string svcHttpAddBuddy(string uName, string BuddyName);

		[OperationContract(IsOneWay = false)]
        void svcHttpRemoveBuddy(string uName, List<string> BuddyName);

		[OperationContract(IsOneWay = true)]
		void svcHttpBSAuthorizedUser(string uName,string IP, bool blSuperNode);

        [OperationContract(IsOneWay = true)]
		void svcHttpBSUnJoin(string uName, string IP, bool IsSuperNode);

        [OperationContract(IsOneWay = false)]
        string svcGetNodeNameByIP(string NodeIP);

        [OperationContract(IsOneWay = false)]
        string svcGetOfflineNodeName(string uName, string IP);

        [OperationContract(IsOneWay = false)]
        List<string> svcGetAllBuddies();

        [OperationContract(IsOneWay = false)]
        void svcUpdateVMuktiVersion(bool blIsMeetingPlace, bool blIsCallCenter);

    }

    public interface IHTTPBootStrapChannel : IClientChannel, IHTTPBootStrapService
    {
    }
}
