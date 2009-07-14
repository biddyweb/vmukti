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
    [ServiceContract]
    public interface IHttpPresentationUploadService
    {
        [OperationContract(IsOneWay = true)]
        void svcHttpJoin();

        [OperationContract(IsOneWay = true)]
        void svcHttpSendUploadedFile(string strFileName, byte[] bytearr, int intSig);

        [OperationContract(IsOneWay = false)]
        string[] svcHttpGetPresentationName();

        [OperationContract(IsOneWay = false)]
        int svcHttpSlidesCount(string PName);

        [OperationContract(IsOneWay = false)]
        Stream svcHttpGetSlideStream(byte[] pnameCnt);

        [OperationContract(IsOneWay = false)]
        bool svcHttpPPTExists(string filename);

        [OperationContract(IsOneWay = true)]
        void svcHttpUnJoin();

    }

    public interface IHttpPresentationUploadServiceChannel : IHttpPresentationUploadService, IClientChannel
    {
    }
}
